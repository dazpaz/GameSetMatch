
using TourManagement.Application.Handlers;
using TourManagement.Application.Repository;
using TourManagement.Data;
using TourManagement.Domain.TournamentAggregate;

namespace AdminConsole.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddScoped<ITournamentCommandHandler, TournamentCommandHandler>();
			builder.Services.AddScoped<IUnitOfWork, TempUnitOfWork>();
			builder.Services.AddScoped<IRepository<Tournament, Guid>, TempRepository>();

			builder.Services.AddControllers();
			// Allow cross-origin requests from Angular dev server
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAdminConsole", policy =>
					policy.WithOrigins("http://localhost:4200")
						.AllowAnyHeader()
						.AllowAnyMethod());
			});
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			var app = builder.Build();
			
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/openapi/v1.json", "AdminConsole API v1");
				});
			}

			// Enable CORS policy for requests from the Angular dev server
			app.UseCors("AllowAdminConsole");

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
