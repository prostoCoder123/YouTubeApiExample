namespace YouTubeAPIExample.Options
{
    public class MyGoogleOptions
    {
        public const string SectionName = "GoogleParams";
        public string YoutubeScope { get; set; } = string.Empty;
        public string SubscriptionsQuery { get; set; } = string.Empty;
        public string ProfilePictureKey { get; set; } = string.Empty;
        public string YoutubeApiBaseUrl { get; set; } = string.Empty;
        public string ChannelQuery { get; set; } = string.Empty;
    }
}