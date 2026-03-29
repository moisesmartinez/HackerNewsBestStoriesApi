using HackerNewsBestStories.API.DTOs;
using HackerNewsBestStories.API.Interfaces;
using System.Text.Json;

namespace HackerNewsBestStories.API.Services;

public class HackerNewsHttpClient : IHackerNewsHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HackerNewsHttpClient> _logger;

    private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/";
    private const string GetBestStoriesEndpoint = BaseUrl + "beststories.json";
    private const string GetStoryDetailsEndpointFormat = BaseUrl + "item/{0}.json";

    public HackerNewsHttpClient(HttpClient httpClient, ILogger<HackerNewsHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<int>> GetBestStoryIdsAsync()
    {
        var response = await _httpClient.GetAsync(GetBestStoriesEndpoint);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<int>>(content) ?? new List<int>();
    }

    public async Task<StoryDetailsDTO?> GetStoryByIdAsync(int id)
    {
        try
        {
            string formattedStoryDetailsEndpointURL = string.Format(GetStoryDetailsEndpointFormat, id);
            var response = await _httpClient.GetAsync(formattedStoryDetailsEndpointURL);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch story {Id}. Status: {StatusCode}", id, response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<StoryDetailsDTO>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching story {Id}", id);
            return null;
        }
    }
}
