using Microsoft.AspNetCore.Identity;

namespace YouTubeAPIExample.Services
{
    public class GoogleTokenService(SignInManager<IdentityUser> signInManager)
        : IBearerTokenService
    {
        public async Task<string?> GetBearerToken()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return null;
            }

            string? googleBearerToken = info.AuthenticationTokens
                ?.FirstOrDefault(t => t.Name == "access_token")?.Value;

            string? expiresAt = info.AuthenticationTokens
                ?.FirstOrDefault(t => t.Name == "expires_at")?.Value;

            if (string.IsNullOrEmpty(googleBearerToken) ||
                string.IsNullOrEmpty(expiresAt) ||
                DateTime.Parse(expiresAt).ToUniversalTime() <= DateTime.UtcNow)
            {
                return null;
            }

            return googleBearerToken;
        }
    }
}