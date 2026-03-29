using HackerNewsBestStories.API.Interfaces;
using HackerNewsBestStories.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsBestStories.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {

        private readonly IHackerNewsService _hackerNewsService;
        private readonly ILogger<StoriesController> _logger;
        public StoriesController(IHackerNewsService hackerNewsService, ILogger<StoriesController> logger)
        {
            _hackerNewsService = hackerNewsService;
            _logger = logger;
        }

        [HttpGet(nameof(GetBestStories))]
        [ProducesResponseType(typeof(IEnumerable<>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StoryResponse>>> GetBestStories([FromQuery] StoryRequest request)
        {
            var errors = request.Validate().ToList();
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            try
            {
                var stories = await _hackerNewsService.GetBestStoriesAsync(request.NumberOfStories);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching best stories.");
                return StatusCode(500, "An internal error occurred while processing your request.");
            }
        }

    }
}
