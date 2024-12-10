namespace YouTubeAPIExample.Services
{
    public interface IBearerTokenService
    {
        public Task<string?> GetBearerToken();
    }
}