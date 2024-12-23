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
    public class SubscriptionsModel(
        IApiExecutor<Channel> apiExecutor,
        IBearerTokenService tokenService,
        ILogger<SubscriptionsModel> logger,
        IOptions<MyGoogleOptions> options) : PageModel
    {
        public Channel Channels { get; private set; } = default!;
        public async Task<IActionResult> OnGet(string pageToken = "")
        {
            string? googleBearerToken = await tokenService.GetBearerToken();

            if (string.IsNullOrWhiteSpace(googleBearerToken))
            {
                logger.LogInformation("Redirecting to login page");
                // sign out the current user and delete their cookie
                await HttpContext.SignOutAsync();
                return Redirect("Identity/Account/Login?returnUrl=/Subscriptions");
            }

            string query = options.Value.SubscriptionsQuery;

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                query += $"&pageToken={pageToken}";
            }

            Channels = await apiExecutor.GetAsync(query, googleBearerToken!);

            return Page();
        }
    }
}