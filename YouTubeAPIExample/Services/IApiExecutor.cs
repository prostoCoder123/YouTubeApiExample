namespace YouTubeAPIExample.Services
{
    public interface IApiExecutor<T> where T : class
    {
        public Uri BaseUri { get; }
        public Task<T> GetAsync(string query, string bearerToken);
    }
}