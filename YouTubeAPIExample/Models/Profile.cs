using System.Text.Json.Serialization;

namespace YouTubeAPIExample.Models
{
    public class Profile
    {
        [JsonPropertyName("items")]
        public IEnumerable<ProfileItem> Items { get; init; } = Enumerable.Empty<ProfileItem>();
    }

    public class ProfileItem
    {
        [JsonPropertyName("snippet")]
        public ProfileSnippet Snippet { get; init; } = default!;

        [JsonPropertyName("statistics")]
        public Statistics Statistics { get; init; } = default!;
    }
    public class ProfileSnippet
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("customUrl")]
        public string CustomUrl { get; set; } = string.Empty;

        [JsonPropertyName("publishedAt")]
        public DateTime PublishedAt { get; set; }
    }

    public class Statistics
    {
        [JsonPropertyName("videoCount")]
        public int VideoCount { get; set; }

        [JsonPropertyName("subscriberCount")]
        public int SubscriberCount { get; set; }
    }
}