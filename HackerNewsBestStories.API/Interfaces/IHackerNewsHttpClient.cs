using HackerNewsBestStories.API.DTOs;

namespace HackerNewsBestStories.API.Interfaces;

public interface IHackerNewsHttpClient
{
    Task<List<int>> GetBestStoryIdsAsync();
    Task<StoryDetailsDTO?> GetStoryByIdAsync(int id);
}
