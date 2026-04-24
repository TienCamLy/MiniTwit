using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Context;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using DotNetEnv;
using Infrastructure.Context.Services;
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

var app = builder.Build();

app.MapRazorPages();
app.MapControllers();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MiniTwitContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
// HTTP-only Kestrel (e.g. Docker compose): skip HTTPS redirect so clients are not sent to an unused TLS port.
var aspNetCoreUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? string.Empty;
var httpsConfigured = aspNetCoreUrls.Contains("https:", StringComparison.OrdinalIgnoreCase);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    if (httpsConfigured)
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
}

if (httpsConfigured)
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseMetricServer();
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();