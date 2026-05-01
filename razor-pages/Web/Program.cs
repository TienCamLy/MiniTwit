using System.Threading.RateLimiting;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Context;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using DotNetEnv;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

using Prometheus;

// Load .env from project directory (optional; no-op if file is missing)
if (File.Exists(".env"))
    Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add postgreSQL server
var connectionStringName = builder.Configuration.GetValue<bool>("ISLOCALDEVELOPMENT") ? "TESTCONNECTION" : "DEFAULTCONNECTION";
builder.Services.AddDbContext<MiniTwitContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(connectionStringName), npgsqlOptions => 
        { npgsqlOptions.CommandTimeout(180); }));

// Add repositories to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<ISimulatorLatestRepository, SimulatorLatestRepository>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
    .SetApplicationName("razor-pages");

builder.Services.AddScoped<MetricsService>();
builder.Services.AddHostedService<MetricsUpdater>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Add rate limiter
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim() // Take client-ip (avoid proxies)
                          ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 120,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
    options.RejectionStatusCode = 429;
});

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MiniTwitContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseRateLimiter(); // Enable rate limiter

app.UseMetricServer();
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapControllers();

app.Run();