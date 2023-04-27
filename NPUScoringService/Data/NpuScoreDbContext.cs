using Microsoft.EntityFrameworkCore;
using NpuScoreService.Models;

namespace NpuScoreService.Data
{
    /// <summary>
    /// Represents the NpuScore database context.
    /// </summary>
    public class NpuScoreDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the NpuScoreDbContext class.
        /// </summary>
        /// <param name="options">The options for configuring the database context.</param>
        public NpuScoreDbContext(DbContextOptions<NpuScoreDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet of NpuScore objects.
        /// </summary>
        public DbSet<NpuScore> NpuScores { get; set; }
    }
}