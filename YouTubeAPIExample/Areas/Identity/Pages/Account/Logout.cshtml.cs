using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YouTubeAPIExample.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel(
        SignInManager<IdentityUser> signInManager,
        ILogger<LogoutModel> logger
        ) : PageModel
    {
        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            signInManager.AuthenticationScheme = IdentityConstants.ExternalScheme;
            await signInManager.SignOutAsync();
            logger.LogInformation(LoggerEventIds.UserLoggedOut, "User logged out.");

            // This needs to be a redirect so that the browser performs a new
            // request and the identity for the user gets updated.
            return returnUrl == null ? RedirectToPage(returnUrl) : LocalRedirect(returnUrl);
        }
    }
}