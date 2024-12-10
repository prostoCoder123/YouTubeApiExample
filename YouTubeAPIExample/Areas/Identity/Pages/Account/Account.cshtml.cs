using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using YouTubeAPIExample.Options;

namespace YouTubeAPIExample.Areas.Identity.Pages.Account
{
    [Authorize]
    public class AccountModel : PageModel
    {
        public IDictionary<string, string> AuthProperties { get; private set; } = new Dictionary<string, string>();
        public Claim? GivenNameClaim { get; private set; }
        public Claim? PictureUrlClaim { get; private set; }

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IOptions<MyGoogleOptions> _options;

        public AccountModel(
            SignInManager<IdentityUser> signInManager,
            IOptions<MyGoogleOptions> options
        ) => (_signInManager, _options) = (signInManager, options);
        public async Task<IActionResult> OnGet()
        {
            GivenNameClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            PictureUrlClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == _options.Value.ProfilePictureKey);

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToPage("./Login");
            }

            if (info.AuthenticationTokens != null)
            {
                foreach (var token in info.AuthenticationTokens)
                {
                    AuthProperties.Add(token.Name, token.Value);
                }
            }

            return Page();
        }
    }
}