using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NpuCreationService.Models
{
    /// <summary>
    /// Represents an NPU creation with associated metadata.
    /// </summary>
    public class NpuCreation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the NPU creation.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier associated with the NPU creation.
        /// </summary>
        [Required]
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the title of the NPU creation.
        /// </summary>
        [Required]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the NPU creation.
        /// </summary>
        [Required]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image associated with the NPU creation.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the IFormFile representation of the image associated with the NPU creation.
        /// </summary>
        [NotMapped]
        [Required]
        public IFormFile? ImageFile { get; set; }
    }
}
