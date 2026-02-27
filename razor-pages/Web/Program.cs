using Web.Pages;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Context;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

// Load .env from project directory (optional; no-op if file is missing)
if (File.Exists(".env"))
    Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add sql server
builder.Services.AddDbContext<MiniTwitContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), settings =>
         settings.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

// Add repositories to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
    .SetApplicationName("razor-pages");

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();