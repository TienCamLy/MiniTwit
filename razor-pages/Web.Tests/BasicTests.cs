using Xunit;
using Infrastructure.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using DotNetEnv;

namespace Web.Tests;

// TODO: seems like dotnet test still doesn't detect this suite?

public class BasicTests
{
    private readonly MiniTwitContext _context;
    private readonly UserRepository _userRepo;
    private readonly MessageRepository _messageRepo;
    private readonly FollowerRepository _followerRepo;

    public BasicTests()
    {
        // Load .env like the main app if present
        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), ".env")))
        {
            Env.Load();
        }

        // This registers the environment variables as configuration source, meaning CONNECTIONSTRINGS__TESTCONNECTION
        // will be possible to read as ConnectionStrings:TESTCONNECTION later
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var connStr = config.GetConnectionString("TESTCONNECTION");

        // Npgsql takes care of translation from LINQ to PostgreSQL
        var options = new DbContextOptionsBuilder<MiniTwitContext>()
            .UseNpgsql(connStr)
            .Options;

        // You pass the options object, in this case containing the connStr setting, to context
        _context = new MiniTwitContext(options);

        // Create Repository objects from context with built-in methods instead of python helpers
        _userRepo = new UserRepository(_context);
        _messageRepo = new MessageRepository(_context);
        _followerRepo = new FollowerRepository(_context);

        // Ensure a clean database state for each test by truncating relevant tables 
        // (it might not be the minimal set of tables, I'm not sure) <- TODO
        // SimulatorLatest is omitted as it's only valid for API calls which these tests don't do
        _context.Database.ExecuteSqlRaw(@"
            TRUNCATE TABLE
            ""Followers"",
            ""Messages"",
            ""AspNetUserTokens"",
            ""AspNetUserRoles"",
            ""AspNetUserLogins"",
            ""AspNetUserClaims"",
            ""AspNetUsers""
            RESTART IDENTITY CASCADE;
            ");
    }

    [Fact]
    public void Register_User_Works()
    {
        var username = "user1";
        var password = "default";
        var email = "user1@example.com";
        _userRepo.CreateUser(username, email, password);
        var user = _userRepo.GetUserByUsername(username);

        // Check if the registration succeeded
        Assert.NotNull(user);
        Assert.Equal(username, user.UserName);

        // Try to register same user again
        Assert.Throws<ArgumentException>(() => _userRepo.CreateUser(username, email, password));
    }

    [Fact]
    public void Register_Validation_Works()
    {
        // Empty username TODO: can I throw specific Exceptions like in python code?
        Assert.Throws<ArgumentException>(() => _userRepo.CreateUser("", "test@example.com", "pw"));
        // Empty password
        Assert.Throws<ArgumentException>(() => _userRepo.CreateUser("meh", "meh@example.com", ""));
        // TODO: 
        // Copilot hint: Password mismatch and email validation would be handled in service/controller, not repo
    }

    [Fact]
    public void Login_Logout_Works()
    {
        var username = "user1";
        var password = "default";
        var email = "user1@example.com";
        _userRepo.CreateUser(username, email, password);

        // Login success
        var user = _userRepo.Login(username, password);
        Assert.NotNull(user);

        // Login wrong password
        var userWrongPw = _userRepo.Login(username, "wrongpassword");
        Assert.Null(userWrongPw);

        // Login wrong username
        var userWrongUser = _userRepo.Login("user2", "wrongpassword");
        Assert.Null(userWrongUser);
    }

    [Fact]
    public void Message_Recording_Works()
    {
        var username = "foo";
        var password = "default";
        var email = "foo@example.com";
        _userRepo.CreateUser(username, email, password);
        var user = _userRepo.Login(username, password);
        Assert.NotNull(user);

        _messageRepo.CreateMessage(user.Id, "test message 1");
        _messageRepo.CreateMessage(user.Id, "<test message 2>");

        var timeline = _messageRepo.GetUserTimeline(user.Id);
        Assert.Contains(timeline, m => m.Text == "test message 1");
        Assert.Contains(timeline, m => m.Text == "<test message 2>");
    }

    [Fact]
    public void Timelines_Work()
    {
        // foo posts
        _userRepo.CreateUser("foo", "foo@example.com", "default");
        var foo = _userRepo.Login("foo", "default");
        _messageRepo.CreateMessage(foo.Id, "the message by foo");

        // bar posts
        _userRepo.CreateUser("bar", "bar@example.com", "default");
        var bar = _userRepo.Login("bar", "default");
        _messageRepo.CreateMessage(bar.Id, "the message by bar");

        // Public timeline
        var publicTimeline = _messageRepo.GetPublicTimeline();
        Assert.Contains(publicTimeline, m => m.Text == "the message by foo");
        Assert.Contains(publicTimeline, m => m.Text == "the message by bar");

        // bar's timeline should just show bar's message
        var barTimeline = _messageRepo.GetUserTimeline(bar.Id);
        Assert.DoesNotContain(barTimeline, m => m.Text == "the message by foo");
        Assert.Contains(barTimeline, m => m.Text == "the message by bar");

        // bar follows foo
        _followerRepo.FollowUser(bar.Id, foo.Id);
        var myTimeline = _messageRepo.GetMyTimeline(bar.Id);
        Assert.Contains(myTimeline, m => m.Text == "the message by foo");
        Assert.Contains(myTimeline, m => m.Text == "the message by bar");

        // Unfollow foo
        _followerRepo.UnfollowUser(bar.Id, foo.Id);
        var myTimelineAfterUnfollow = _messageRepo.GetMyTimeline(bar.Id);
        Assert.DoesNotContain(myTimelineAfterUnfollow, m => m.Text == "the message by foo");
        Assert.Contains(myTimelineAfterUnfollow, m => m.Text == "the message by bar");
    }
}
