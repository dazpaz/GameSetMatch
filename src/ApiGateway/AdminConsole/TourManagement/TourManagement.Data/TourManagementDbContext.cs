using Microsoft.EntityFrameworkCore;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Data
{
	public class TourManagementDbContext(DbContextOptions<TourManagementDbContext> options) : DbContext(options)
	{
		public DbSet<Tournament> Tournaments => Set<Tournament>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Tournament>(builder =>
			{
				builder.ToTable("Tournaments");
				builder.HasKey(t => t.Id);
				builder.Property(t => t.Id).ValueGeneratedNever();
				builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
				builder.Property(t => t.Location).IsRequired().HasMaxLength(200);
			});
		}
	}
}
