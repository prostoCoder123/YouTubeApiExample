using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouTubeAPIExample.Data;
using YouTubeAPIExample.Models;
using YouTubeAPIExample.Options;
using YouTubeAPIExample.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration
    ?? throw new ArgumentNullException("The configuration was not found");

// Add services to the container.
services.AddHttpClient();
services.AddSingleton<IApiExecutor<Channel>, YouTubeApiExecutor<Channel>>();
services.AddSingleton<IApiExecutor<Profile>, YouTubeApiExecutor<Profile>>();
services.AddTransient<IBearerTokenService, GoogleTokenService>();

services.Configure<MyGoogleOptions>(
    configuration.GetSection(MyGoogleOptions.SectionName),
    options => options.ErrorOnUnknownConfiguration = true
);
var options = configuration.GetSection(MyGoogleOptions.SectionName).Get<MyGoogleOptions>()
    ?? throw new ArgumentNullException(nameof(configuration), "The options were not found");

// Add external authentication provider - Google
services.AddAuthentication(authOptions =>
{
    authOptions.DefaultScheme = IdentityConstants.ExternalScheme;
})
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = configuration["Authentication:Google:ClientId"]
            ?? throw new ArgumentNullException("The Google configuration (ClientId) was not found");
        googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]
            ?? throw new ArgumentNullException("The Google configuration (ClientSecret) was not found"); ;

        googleOptions.Scope.Add(options.YoutubeScope);
        googleOptions.ClaimActions.MapJsonKey(options.ProfilePictureKey, "picture", "url");
        googleOptions.SaveTokens = true;
    })
    .AddCookie(IdentityConstants.ApplicationScheme, cookieOptions =>
    {
        cookieOptions.LoginPath = "/Identity/Account/Login";
        cookieOptions.LogoutPath = "/Identity/Account/Logout";
    })
    .AddCookie(IdentityConstants.ExternalScheme, cookieOptions =>
    {
        cookieOptions.LoginPath = "/Identity/Account/Login";
        cookieOptions.LogoutPath = "/Identity/Account/Logout";
    });

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentNullException("The default connection string was not found");

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
).AddDatabaseDeveloperPageExceptionFilter();

services.AddIdentityCore<IdentityUser>()
    .AddSignInManager<SignInManager<IdentityUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddRazorPages();

var app = builder.Build();

// apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();