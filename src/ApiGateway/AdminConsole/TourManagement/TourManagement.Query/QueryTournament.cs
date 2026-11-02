using Common.FunctionalExtensions;
using Dapper;
using Microsoft.Data.SqlClient;
using TourManagement.Application.Queries;
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
	}
}
