using Common.FunctionalExtensions;
using Dapper;
using Microsoft.Data.SqlClient;
using TourManagement.Application.Queries;
using TourManagement.Common;
using TourManagement.Contract;

namespace TourManagement.Query
{
	public class QueryTournament(string connectionString) : IQueryTournament
	{
		private const string SelectColumns =
			"SELECT Id AS TournamentId, Title, Location, Level, Surface, MensSingles, WomensSingles, " +
			"MensDoubles, WomensDoubles, MixedDoubles, IsActive FROM Tournaments";

		public Result<TournamentDto[]> GetAllTournaments()
		{
			using var connection = new SqlConnection(connectionString);
			var tournaments = connection.Query<TournamentDto>(SelectColumns).ToArray();

			return Result<TournamentDto[]>.Success(tournaments);
		}

		public Result<TournamentDto?> GetTournamentById(Guid tournamentId)
		{
			using var connection = new SqlConnection(connectionString);
			var tournament = connection.QuerySingleOrDefault<TournamentDto>(
				SelectColumns + " WHERE Id = @tournamentId", new { tournamentId });

			return Result<TournamentDto?>.Success(tournament);
		}

		private TournamentDto[] SampleTournaments { get; } =
		[
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
				Title = "Australian Open",
				Location = "Melbourne, Australia",
				Level = TournamentLevel.GsmSlam,
				Surface = Surface.Hard,
				MensSingles = true,
				WomensSingles = true,
				MensDoubles = true,
				WomensDoubles = true,
				MixedDoubles = true,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
				Title = "Monte-Carlo Masters",
				Location = "Monte Carlo, Monaco",
				Level = TournamentLevel.Gsm1000,
				Surface = Surface.Clay,
				MensSingles = true,
				WomensSingles = false,
				MensDoubles = true,
				WomensDoubles = false,
				MixedDoubles = true,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
				Title = "Wimbledon",
				Location = "London, United Kingdom",
				Level = TournamentLevel.GsmSlam,
				Surface = Surface.Grass,
				MensSingles = true,
				WomensSingles = true,
				MensDoubles = true,
				WomensDoubles = true,
				MixedDoubles = true,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
				Title = "Indian Wells Masters",
				Location = "Indian Wells, United States",
				Level = TournamentLevel.Gsm1000,
				Surface = Surface.Hard,
				MensSingles = true,
				WomensSingles = true,
				MensDoubles = true,
				WomensDoubles = true,
				MixedDoubles = false,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000005"),
				Title = "Barcelona Open",
				Location = "Barcelona, Spain",
				Level = TournamentLevel.Gsm500,
				Surface = Surface.Clay,
				MensSingles = true,
				WomensSingles = false,
				MensDoubles = true,
				WomensDoubles = false,
				MixedDoubles = false,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000006"),
				Title = "Stuttgart Open",
				Location = "Stuttgart, Germany",
				Level = TournamentLevel.Gsm250,
				Surface = Surface.Grass,
				MensSingles = false,
				WomensSingles = true,
				MensDoubles = false,
				WomensDoubles = true,
				MixedDoubles = false,
				IsActive = false
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000007"),
				Title = "Lyon Challenger",
				Location = "Lyon, France",
				Level = TournamentLevel.Gsm125,
				Surface = Surface.Carpet,
				MensSingles = true,
				WomensSingles = false,
				MensDoubles = false,
				WomensDoubles = true,
				MixedDoubles = false,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000008"),
				Title = "US Open",
				Location = "New York, United States",
				Level = TournamentLevel.GsmSlam,
				Surface = Surface.Hard,
				MensSingles = true,
				WomensSingles = true,
				MensDoubles = false,
				WomensDoubles = false,
				MixedDoubles = true,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-000000000009"),
				Title = "French Open",
				Location = "Paris, France",
				Level = TournamentLevel.GsmSlam,
				Surface = Surface.Clay,
				MensSingles = true,
				WomensSingles = true,
				MensDoubles = true,
				WomensDoubles = true,
				MixedDoubles = true,
				IsActive = true
			},
			new TournamentDto
			{
				TournamentId = Guid.Parse("00000000-0000-0000-0000-00000000000a"),
				Title = "Miami Open",
				Location = "Miami, United States",
				Level = TournamentLevel.Gsm1000,
				Surface = Surface.Hard,
				MensSingles = true,
				WomensSingles = true,
				MensDoubles = true,
				WomensDoubles = true,
				MixedDoubles = false,
				IsActive = false
			}
		];
	}
}
