using System.Text.Json.Serialization;

namespace YouTubeAPIExample.Models
{
    public class Channel
    {
        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; init; } = string.Empty;

        [JsonPropertyName("prevPageToken")]
        public string PrevPageToken { get; init; } = string.Empty;

        [JsonPropertyName("items")]
        public IEnumerable<Item> Items { get; init; } = Enumerable.Empty<Item>();

        [JsonPropertyName("pageInfo")]
        public PageInfo PageInfo { get; init; } = default!;
    }

    public class Item
    {
        [JsonPropertyName("snippet")]
        public Snippet Snippet { get; init; } = default!;

        [JsonPropertyName("contentDetails")]
        public ContentDetails ContentDetails { get; init; } = default!;
    }

    public class Snippet
    {
        [JsonPropertyName("title")]
        public string Title { get; init; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; init; } = string.Empty;

        [JsonPropertyName("thumbnails")]
        public Thumbnails Thumbnails { get; init; } = default!;
    }

    public class ContentDetails
    {
        [JsonPropertyName("totalItemCount")]
        public int TotalVideos { get; init; }

        [JsonPropertyName("newItemCount")]
        public int NewVideos { get; init; }
    }

    public class Thumbnails
    {
        [JsonPropertyName("default")]
        public Thumbnail Default { get; init; } = default!;
    }

    public class Thumbnail
    {
        [JsonPropertyName("url")]
        public string Url { get; init; } = string.Empty;
    }

    public class PageInfo
    {
        [JsonPropertyName("totalResults")]
        public int TotalResults { get; init; }

        [JsonPropertyName("resultsPerPage")]
        public int ResultsPerPage { get; init; }
    }
}