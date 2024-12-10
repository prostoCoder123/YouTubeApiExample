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
            await signInManager.SignOutAsync();
            logger.LogInformation(LoggerEventIds.UserLoggedOut, "User logged out.");

            return returnUrl == null ? Page() : RedirectToPage(returnUrl);
        }
    }
}