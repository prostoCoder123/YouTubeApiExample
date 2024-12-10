using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YouTubeAPIExample.Identity.Pages.Account
{
    [Area("Identity")]
    [AllowAnonymous]
    public class LoginModel(
        SignInManager<IdentityUser> signInManager,
        ILogger<LoginModel> logger
        ) : PageModel
    {
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }
        public string? ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            logger.LogInformation("Signed out");

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null) =>
            await OnGetAsync(returnUrl);
    }
}