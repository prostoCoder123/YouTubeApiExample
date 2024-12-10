using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text.Json;
using YouTubeAPIExample.Options;

namespace YouTubeAPIExample.Services
{
    public class YouTubeApiExecutor<T>(
        IHttpClientFactory httpClientFactory,
        IOptions<MyGoogleOptions> options,
        ILogger<YouTubeApiExecutor<T>> logger) : IApiExecutor<T> where T : class
    {
        public Uri BaseUri => new Uri(options.Value.YoutubeApiBaseUrl);
        public async Task<T> GetAsync(string query, string bearerToken)
        {
            // Create the client
            using HttpClient client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);
            var requestUri = new Uri(BaseUri, query);

            logger.LogInformation($"Send request to get {typeof(T)} model");

            // Make HTTP GET request
            // Parse JSON response deserialize into T type
            T model = await client.GetFromJsonAsync<T>(
                requestUri.ToString(),
                new JsonSerializerOptions(JsonSerializerDefaults.Web))
                ?? throw new SerializationException($"Could not parse {typeof(T)} model");

            return model;
        }
    }
}