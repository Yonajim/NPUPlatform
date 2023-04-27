using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NpuCreationService.Data;
using NpuCreationService.Models;
using NpuCreationService.Services;

namespace NpuCreationService.Controllers
{
    /// <summary>
    /// Represents the API controller for managing NPU creations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NpuCreationController : ControllerBase
    {
        private readonly NpuDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        /// <summary>
        /// Initializes a new instance of the NpuCreationController class with the specified DbContext and file storage service.
        /// </summary>
        /// <param name="context">The NPU creation database context.</param>
        /// <param name="fileStorageService">The file storage service for storing image files.</param>
        public NpuCreationController(NpuDbContext context, IFileStorageService fileStorageService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
        }

        /// <summary>
        /// Asynchronously creates a new NPU creation with the specified data.
        /// </summary>
        /// <param name="npuCreation">The NPU creation data to be added.</param>
        /// <returns>A task representing the operation, with an IActionResult containing the result.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateNpuCreation([FromForm] NpuCreation npuCreation)
        {
            if (npuCreation == null)
            {
                return BadRequest("NpuCreation object is null.");
            }

            if (npuCreation.ImageFile == null)
            {
                return BadRequest("An image file is required.");
            }

            if (!ValidateFileType(npuCreation))
            {
                return BadRequest("Invalid file type. Only JPEG, PNG, GIF, and BMP images are allowed.");
            }

            var imageUrl = await _fileStorageService.SaveFileAsync(npuCreation.ImageFile);
            npuCreation.ImageUrl = imageUrl;

            _context.NpuCreations.Add(npuCreation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating NPU: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetNpuCreation), new { id = npuCreation.Id }, npuCreation);
        }

        /// <summary>
        /// Asynchronously retrieves an NPU creation with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the NPU creation to retrieve.</param>
        /// <returns>A task representing the operation, with an IActionResult containing the result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNpuCreation(long id)
        {
            var npuCreation = await _context.NpuCreations.FindAsync(id);

            if (npuCreation == null)
            {
                return NotFound();
            }

            return Ok(npuCreation);
        }

        /// <summary>
        /// Asynchronously retrieves all NPU creations.
        /// </summary>
        /// <returns>A task representing the operation, with an ActionResult containing a list of NPU creations.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NpuCreation>>> GetNpuCreations()
        {
            // Retrieve all NPU creations and return the result
            try
            {
                return await _context.NpuCreations.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while retrieving NPU creations: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously deletes an NPU creation with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the NPU creation to delete.</param>
        /// <returns>A task representing the operation, with an IActionResult containing the result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNpuCreation(long id)
        {
            var npuCreation = await _context.NpuCreations.FindAsync(id);

            if (npuCreation == null)
            {
                return NotFound();
            }

            _context.NpuCreations.Remove(npuCreation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting NPU creation: {ex.Message}");
            }

            await _fileStorageService.DeleteFileAsync(npuCreation.ImageUrl);

            return NoContent();
        }

        /// <summary>
        /// Asynchronously updates an NPU creation with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the NPU creation to update.</param>
        /// <param name="npuCreationUpdate">The updated NPU creation data.</param>
        /// <returns>A task representing the operation, with an IActionResult containing the result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNpuCreation(long id, [FromForm] NpuCreation npuCreationUpdate)
        {
            if (npuCreationUpdate == null)
            {
                return BadRequest("NpuCreationUpdate object is null.");
            }

            var npuCreation = await _context.NpuCreations.FindAsync(id);

            if (npuCreation == null)
            {
                return NotFound();
            }

            if (npuCreationUpdate.ImageFile != null)
            {
                if (!ValidateFileType(npuCreationUpdate))
                {
                    return BadRequest("Invalid file type. Only JPEG, PNG, GIF, and BMP images are allowed.");
                }

                await _fileStorageService.DeleteFileAsync(npuCreation.ImageUrl);

                var imageUrl = await _fileStorageService.SaveFileAsync(npuCreationUpdate.ImageFile);
                npuCreation.ImageUrl = imageUrl;
            }

            if (npuCreationUpdate.UserId != null) { npuCreation.UserId = npuCreationUpdate.UserId; }
            if (npuCreationUpdate.Title != null) { npuCreation.Title = npuCreationUpdate.Title; }
            if (npuCreationUpdate.Description != null) { npuCreation.Description = npuCreationUpdate.Description; }

            _context.Entry(npuCreation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict($"A concurrency error occurred while updating the NPU creation: {ex.Message}");
            }

            return NoContent();
        }


        /// <summary>
        /// Asynchronously searches for NPU creations with a title or description containing the specified element name.
        /// </summary>
        /// <param name="elementName">The element name to search for.</param>
        /// <returns>A task representing the operation, with an ActionResult containing a list of matching NPU creations.</returns>
        [HttpGet("search/{elementName}")]
        public async Task<ActionResult<IEnumerable<NpuCreation>>> SearchByElementNameAsync(string elementName)
        {
            if (string.IsNullOrEmpty(elementName))
            {
                return BadRequest("Element name cannot be null or empty.");
            }

            try
            {
                var npuCreations = await _context.NpuCreations
                    .Where(nc => nc.Title.Contains(elementName) || nc.Description.Contains(elementName))
                    .ToListAsync();

                return Ok(npuCreations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while searching NPU creations: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates the file type of the NPU creation's image file.
        /// </summary>
        /// <param name="npuCreation">The NPU creation to validate.</param>
        /// <returns>True if the file type is valid, false otherwise.</returns>
        private bool ValidateFileType(NpuCreation npuCreation)
        {
            var allowedContentTypes = new List<string> { "image/jpeg", "image/png", "image/gif", "image/bmp" };

            if (allowedContentTypes.Contains(npuCreation.ImageFile.ContentType))
            {
                return true;
            }
            return false;
        }
    }
}