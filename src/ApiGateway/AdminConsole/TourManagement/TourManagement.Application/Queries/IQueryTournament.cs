using Common.FunctionalExtensions;
using TourManagement.Contract;

namespace TourManagement.Application.Queries
{
	public interface IQueryTournament
	{
		public Result<TournamentDto[]> GetAllTournaments();

		public Result<TournamentDto?> GetTournamentById(Guid tournamentId);
	}
}
