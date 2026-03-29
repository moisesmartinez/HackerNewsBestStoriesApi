using HackerNewsBestStories.API.DTOs;
using HackerNewsBestStories.API.Interfaces;
using HackerNewsBestStories.API.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text.Json;

namespace HackerNewsBestStories.API.Services;

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsHttpClient _client;
    private readonly IMemoryCache _cache;
    private readonly ILogger<HackerNewsService> _logger;
    private const string BestStoriesCacheKey = "BestStoriesIds";
    private const string StoryCacheKeyPrefix = "Story_";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public HackerNewsService(IHackerNewsHttpClient client, IMemoryCache cache, ILogger<HackerNewsService> logger)
    {
        _client = client;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IEnumerable<StoryResponse>> GetBestStoriesAsync(int n)
    {
        if (n <= 0) return Enumerable.Empty<StoryResponse>();

        // Get best story IDs from cache or API
        var bestStoryIds = await _cache.GetOrCreateAsync(BestStoriesCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            _logger.LogInformation("Fetching best story IDs from Hacker News API");
            return await _client.GetBestStoryIdsAsync();
        });

        // Take more IDs than needed to ensure top scoring stories
        var idsToFetch = bestStoryIds.Take(Math.Min(n * 2, bestStoryIds.Count)).ToList();

        // Fetch stories in parallel
        var stories = new ConcurrentBag<StoryDetailsDTO>();
        var tasks = idsToFetch.Select(async id =>
        {
            // Get story from cache or HTTP client
            string cacheKey = $"{StoryCacheKeyPrefix}{id}";
            var story = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                return await _client.GetStoryByIdAsync(id);
            });

            if (story != null)
            {
                stories.Add(story);
            }
        });

        await Task.WhenAll(tasks);

        return stories
            .OrderByDescending(s => s.Score)
            .Take(n)
            .Select(s => new StoryResponse
            {
                Title = s.Title,
                Uri = s.Uri,
                PostedBy = s.PostedBy,
                // Note to self: the format "yyyy-MM-ddTHH:mm:ss+00:00" sometimes throw runtime error.
                // The format for timezone "...zzz" is current standard. 
                Time = s.Time.ToString("yyyy-MM-ddTHH:mm:sssszzz"),
                Score = s.Score,
                CommentCount = s.CommentCount
            });
    }
}
