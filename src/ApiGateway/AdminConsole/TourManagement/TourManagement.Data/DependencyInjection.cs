using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TourManagement.Application.Repository;

namespace TourManagement.Data
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddTourManagementData(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<TourManagementDbContext>(options => options.UseSqlServer(connectionString));
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}
