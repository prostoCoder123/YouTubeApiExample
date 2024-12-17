using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace YouTubeAPIExample.Identity.Pages.Account
{
    [Area("Identity")]
    [AllowAnonymous]
    public class ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<ExternalLoginModel> logger) : PageModel
    {
        /// <summary>
        /// </summary>
        public string? ProviderDisplayName { get; set; }

        /// <summary>
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// </summary>
        [TempData]
        public string? ErrorMessage { get; set; }

        public void OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string? returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(LoggerEventIds.UserLoggedInByExternalProvider, "User logged in with {LoginProvider} provider.", info.LoginProvider);
            }

            var user = await userManager.GetUserAsync(info.Principal);

            if (user == null)
            {
                user = CreateUser();
                user.UserName = info.Principal.FindFirst(ClaimTypes.Name)?.Value;
                user.Email = info.Principal.FindFirst(ClaimTypes.Email)?.Value;

                var userCreated = await userManager.CreateAsync(user);

                if (userCreated.Succeeded)
                {
                    var loginCreated = await userManager.AddLoginAsync(user, info);

                    if (loginCreated.Succeeded)
                    {
                        if (logger.IsEnabled(LogLevel.Information))
                        {
                            logger.LogInformation(LoggerEventIds.UserCreatedByExternalProvider, "User created an account using {Name} provider.", info.LoginProvider);
                        }

                        await userManager.AddClaimsAsync(user, info.Principal.Claims);
                    }
                }
            }

            // set the external auth cookie
            await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
            await signInManager.UpdateExternalAuthenticationTokensAsync(info);

            return RedirectToPage(returnUrl);
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;

            return RedirectToPage(returnUrl);
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }
    }
}