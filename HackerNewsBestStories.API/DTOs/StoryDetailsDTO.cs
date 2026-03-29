using System.Text.Json.Serialization;

namespace HackerNewsBestStories.API.DTOs;

public class StoryDetailsDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Uri { get; set; } = string.Empty;

    [JsonPropertyName("by")]
    public string PostedBy { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public long TimeUnix { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("descendants")]
    public int CommentCount { get; set; }

    [JsonIgnore]
    public DateTimeOffset Time => DateTimeOffset.FromUnixTimeSeconds(TimeUnix);

    [JsonPropertyName("time_formatted")]
    public string FormattedTime => Time.ToString("yyyy-MM-ddTHH:mm:ssK");
}
