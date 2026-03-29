using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackerNewsBestStories.API.Models;

public class StoryRequest
{
    [FromQuery(Name = "n")]
    public int NumberOfStories { get; set; } = 10; //Defaults to 10 if N is not present.

    public IEnumerable<string> Validate()
    {
        if (NumberOfStories <= 0)
        {
            yield return "The number of stories must be greater than 0.";
        }
        if (NumberOfStories > 200)
        {
            yield return "The number of stories cannot exceed 200 for performance reasons.";
        }
    }
}
