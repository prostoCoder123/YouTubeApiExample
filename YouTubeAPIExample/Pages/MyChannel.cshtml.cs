using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using YouTubeAPIExample.Models;
using YouTubeAPIExample.Options;
using YouTubeAPIExample.Services;

namespace YouTubeAPIExample.Pages
{
    [Authorize]
    public class MyChannelModel(
        IApiExecutor<Profile> apiExecutor,
        IBearerTokenService tokenService,
        ILogger<SubscriptionsModel> logger,
        IOptions<MyGoogleOptions> options) : PageModel
    {
        public ProfileItem Profile { get; private set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            string? googleBearerToken = await tokenService.GetBearerToken();

            if (string.IsNullOrWhiteSpace(googleBearerToken))
            {
                logger.LogInformation("Redirecting to login page");
                // sign out the current user and delete their cookie
                await HttpContext.SignOutAsync();
                return Redirect("Identity/Account/Login?returnUrl=/MyChannel");
            }

            string query = options.Value.ChannelQuery;

            throw new HttpRequestException();

            Profile = (await apiExecutor.GetAsync(query, googleBearerToken!)).Items.First();

            return Page();
        }
    }
}