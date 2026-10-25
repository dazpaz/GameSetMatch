using Common.FunctionalExtensions;
using TourManagement.Application.Commands;
using TourManagement.Application.Repository;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Application.Handlers
{
	public interface ITournamentCommandHandler
	{
		Result<Guid> HandleAddTournamentCommand(AddTournamentCommand command);
	}

	public class TournamentCommandHandler(IUnitOfWork uow) : ITournamentCommandHandler
	{
		private readonly IUnitOfWork _uow = uow;

		public Result<Guid> HandleAddTournamentCommand(AddTournamentCommand command)
		{
			var id = Guid.NewGuid();

			var tournament = Tournament.Create(id, command.Title, command.Location, command.Level, command.Surface,
				command.MensSingles, command.WomensSingles, command.MensDoubles, command.WomensDoubles,
				command.MixedDoubles);

			try
			{
				_uow.TournamentRepository.Add(tournament);
				_uow.SaveChanges();
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure($"Failed to add tournament: {ex.Message}");
			}

			return Result<Guid>.Success(id);
		}
	}
}
