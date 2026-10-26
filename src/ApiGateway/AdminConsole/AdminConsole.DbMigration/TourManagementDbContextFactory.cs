using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TourManagement.Data;

namespace AdminConsole.DbMigration
{
	public class TourManagementDbContextFactory : IDesignTimeDbContextFactory<TourManagementDbContext>
	{
		public TourManagementDbContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true)
				.AddEnvironmentVariables()
				.Build();

			var connectionString = configuration.GetConnectionString("AdminConsole")
				?? throw new InvalidOperationException("Connection string 'AdminConsole' not found.");

			var options = new DbContextOptionsBuilder<TourManagementDbContext>()
				.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(TourManagementDbContextFactory).Assembly.FullName))
				.Options;

			return new TourManagementDbContext(options);
		}
	}
}
