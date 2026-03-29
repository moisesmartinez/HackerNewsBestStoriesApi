using HackerNewsBestStories.API.Controllers;
using HackerNewsBestStories.API.DTOs;
using HackerNewsBestStories.API.Interfaces;
using HackerNewsBestStories.API.Models;
using HackerNewsBestStories.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNewsBestStories.Tests
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<IHackerNewsHttpClient> _httpClientMock;
        private readonly IMemoryCache _cache;
        private readonly Mock<ILogger<HackerNewsService>> _serviceLoggerMock;
        private readonly Mock<ILogger<StoriesController>> _controllerLoggerMock;
        private readonly Mock<IHackerNewsService> _serviceMock;
        private readonly StoriesController _controller;

        public HackerNewsServiceTests()
        {
            // Setup mocks
            _httpClientMock = new Mock<IHackerNewsHttpClient>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _serviceLoggerMock = new Mock<ILogger<HackerNewsService>>();
            _controllerLoggerMock = new Mock<ILogger<StoriesController>>();

            // Initialize service mock for the controller
            _serviceMock = new Mock<IHackerNewsService>();

            // Pass the mock service to the controller
            _controller = new StoriesController(_serviceMock.Object, _controllerLoggerMock.Object);
        }

        [Fact]
        public async Task GetBestStoriesAsync_ReturnsSortedStories()
        {
            // Arrange
            var storyIds = new List<int> { 1, 2 };
            var story1 = new StoryDetailsDTO { Title = "Story 1", Score = 100, TimeUnix = 1570887781, Uri = "https://story1.com", PostedBy = "user1", CommentCount = 5 };
            var story2 = new StoryDetailsDTO { Title = "Story 2", Score = 200, TimeUnix = 1570888881, Uri = "https://story2.com", PostedBy = "user2", CommentCount = 10 };

            _httpClientMock.Setup(c => c.GetBestStoryIdsAsync()).ReturnsAsync(storyIds);
            _httpClientMock.Setup(c => c.GetStoryByIdAsync(1)).ReturnsAsync(story1);
            _httpClientMock.Setup(c => c.GetStoryByIdAsync(2)).ReturnsAsync(story2);

            var service = new HackerNewsService(_httpClientMock.Object, _cache, _serviceLoggerMock.Object);

            // Act
            var result = (await service.GetBestStoriesAsync(2)).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Story 2", result[0].Title); // Higher score first
            Assert.Equal(200, result[0].Score);
            Assert.Equal("Story 1", result[1].Title);
            Assert.Equal(100, result[1].Score);
        }

        [Fact]
        public async Task GetBestStories_WhenNegativeNumberOfStories_ReturnsBadRequest()
        {
            // Arrange
            var request = new StoryRequest { NumberOfStories = -5 };

            // Act
            var result = await _controller.GetBestStories(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

    }
}
