using System.ComponentModel.DataAnnotations;

namespace NpuScoreService.Models
{
    /// <summary>
    /// Represents an NpuScore object containing information about a user's score for an NPU.
    /// </summary>
    public class NpuScore
    {
        /// <summary>
        /// Gets or sets the unique identifier for the NpuScore.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the score.
        /// </summary>
        [Required]
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the NpuCreation identifier associated with the score.
        /// </summary>
        [Required]
        public long? NpuCreationId { get; set; }

        /// <summary>
        /// Gets or sets the score value.
        /// </summary>
        [Required]
        public int? Score { get; set; }
    }
}
