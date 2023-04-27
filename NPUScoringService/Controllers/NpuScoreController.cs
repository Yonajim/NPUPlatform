using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NpuScoreService.Data;
using NpuScoreService.Models;
using NpuScoreService.Services;

namespace NpuScoreService.Controllers
{
    /// <summary>
    /// Represents the controller responsible for handling NpuScore-related requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NpuScoreController : ControllerBase
    {
        private readonly NpuScoreDbContext _context;
        private readonly NpuCreationClient _npuCreationServiceClient;

        /// <summary>
        /// Initializes a new instance of the NpuScoreController with the specified context and NpuCreationClient.
        /// </summary>
        /// <param name="context">The NpuScoreDbContext instance for database operations.</param>
        /// <param name="npuCreationClient">The NpuCreationClient instance for interacting with the NpuCreationService.</param>
        public NpuScoreController(NpuScoreDbContext context, NpuCreationClient npuCreationClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _npuCreationServiceClient = npuCreationClient ?? throw new ArgumentNullException(nameof(npuCreationClient));
        }

        /// <summary>
        /// Adds a new NpuScore to the database.
        /// </summary>
        /// <param name="npuScore">The NpuScore instance to add to the database.</param>
        /// <returns>An IActionResult containing the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> PostScore([FromForm] NpuScore npuScore)
        {
            try
            {
                var npuCreation = await _npuCreationServiceClient.GetNpuCreationByIdAsync(npuScore.NpuCreationId);
                if (npuCreation == null)
                {
                    return NotFound("NpuCreation not found.");
                }

                _context.NpuScores.Add(npuScore);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetScore), new { id = npuScore.Id }, npuScore);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while posting the score: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an NpuScore by its ID.
        /// </summary>
        /// <param name="id">The ID of the NpuScore to retrieve.</param>
        /// <returns>An IActionResult containing the result of the operation.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScore(long id)
        {
            try
            {
                var npuScore = await _context.NpuScores.FindAsync(id);

                if (npuScore == null)
                {
                    return NotFound();
                }

                return Ok(npuScore);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting the score: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all NpuScores for a specific NpuCreation.
        /// </summary>
        /// <param name="npuCreationId">The ID of the NpuCreation for which to retrieve the scores.</param>
        /// <returns>An IActionResult containing the result of the operation.</returns>
        [HttpGet("npu/{npuCreationId}")]
        public async Task<ActionResult<IEnumerable<NpuScore>>> GetScoresForNpuCreation(long npuCreationId)
        {
            try
            {
                var npuCreation = await _npuCreationServiceClient.GetNpuCreationByIdAsync(npuCreationId);
                if (npuCreation == null)
                {
                    return NotFound("NpuCreation not found.");
                }

                var scores = await _context.NpuScores
                    .Where(score => score.NpuCreationId == npuCreationId)
                    .ToListAsync();

                return Ok(scores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting scores for the NpuCreation: {ex.Message}");
            }
        }
    }
}