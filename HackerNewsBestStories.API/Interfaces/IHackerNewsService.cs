using HackerNewsBestStories.API.Models;

namespace HackerNewsBestStories.API.Interfaces;

public interface IHackerNewsService
{
    Task<IEnumerable<StoryResponse>> GetBestStoriesAsync(int n);
}

