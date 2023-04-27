namespace NpuScoreService.Models
{
    /// <summary>
    /// Represents an NpuCreation object containing information about a created NPU.
    /// </summary>
    public class NpuCreation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the NPU.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the NPU.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the title of the NPU.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the NPU.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the URL of the NPU image.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
