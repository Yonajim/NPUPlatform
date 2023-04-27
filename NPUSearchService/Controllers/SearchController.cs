using Microsoft.AspNetCore.Mvc;
using NpuSearchService.Models;
using NpuSearchService.Services;

namespace NpuSearchService.Controllers
{
    /// <summary>
    /// Represents the API controller for searching NPU creations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly NpuCreationClient _npuCreationClient;

        /// <summary>
        /// Initializes a new instance of the SearchController class with the specified NpuCreationClient.
        /// </summary>
        /// <param name="npuCreationClient">The NpuCreationClient for searching NPU creations.</param>
        public SearchController(NpuCreationClient npuCreationClient)
        {
            _npuCreationClient = npuCreationClient ?? throw new ArgumentNullException(nameof(npuCreationClient));
        }

        /// <summary>
        /// Asynchronously searches NPU creations by element name.
        /// </summary>
        /// <param name="elementName">The element name to search for in NPU creations.</param>
        /// <returns>A task representing the operation, with an IActionResult containing the result.</returns>
        [HttpGet("{elementName}")]
        public async Task<ActionResult<IEnumerable<NpuCreation>>> SearchByElementNameAsync(string elementName)
        {
            if (string.IsNullOrEmpty(elementName))
            {
                return BadRequest("Element name is required.");
            }

            try
            {
                var npuCreations = await _npuCreationClient.SearchByElementNameAsync(elementName);
                return Ok(npuCreations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during the search: {ex.Message}");
            }
        }
    }
}