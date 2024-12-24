using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using YouTubeAPIExample.Data;
using YouTubeAPIExample.Models;
using YouTubeAPIExample.Options;
using YouTubeAPIExample.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    //.AddJsonFile($"appsettings.{builder.Environment}.json")
    .AddUserSecrets<Program>()
    .Build();

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

// Add external authentication
services.AddAuthentication(authOptions =>
{
    authOptions.DefaultScheme = IdentityConstants.ExternalScheme;
    authOptions.DefaultAuthenticateScheme = IdentityConstants.ExternalScheme;
    authOptions.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    authOptions.DefaultSignOutScheme = IdentityConstants.ExternalScheme;
})
    .AddGoogle(googleOptions => // Add external scheme = "Google"
    {
        googleOptions.ClientId = configuration["Authentication:Google:ClientId"]
            ?? throw new ArgumentNullException(nameof(configuration), "The Google configuration (ClientId) was not found");
        googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]
            ?? throw new ArgumentNullException(nameof(configuration), "The Google configuration (ClientSecret) was not found"); ;

        googleOptions.Scope.Add(options.YoutubeScope);
        googleOptions.ClaimActions.MapJsonKey(options.ProfilePictureKey, "picture", "url");
        googleOptions.SaveTokens = true;
    })
    .AddCookie(IdentityConstants.ExternalScheme, cookieOptions =>
    {
        // paths to login/logout pages if Authorize attribute applied
        // it's different from the default /Account/Login and /Account/Logout values
        cookieOptions.LoginPath = "/Identity/Account/Login";
        cookieOptions.LogoutPath = "/Identity/Account/Logout";
    });

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new ArgumentNullException(nameof(configuration), "The default connection string was not found");

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