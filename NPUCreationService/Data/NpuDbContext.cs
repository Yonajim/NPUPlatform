using Microsoft.EntityFrameworkCore;
using NpuCreationService.Models;

namespace NpuCreationService.Data
{
    /// <summary>
    /// Represents the NPU creation database context, which handles interactions with the database.
    /// </summary>
    public class NpuDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the NpuDbContext class with the specified options.
        /// </summary>
        /// <param name="options">The options for the DbContext.</param>
        public NpuDbContext(DbContextOptions<NpuDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet representing the NPU creations in the database.
        /// </summary>
        public DbSet<NpuCreation> NpuCreations { get; set; }
    }
}