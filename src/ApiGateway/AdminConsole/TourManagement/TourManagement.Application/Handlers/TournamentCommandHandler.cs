using Common.FunctionalExtensions;
using TourManagement.Application.Commands;
using TourManagement.Application.Repository;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Application.Handlers
{
	public interface ITournamentCommandHandler
	{
		Result<Guid> HandleAddTournamentCommand(AddTournamentCommand command);
		Result HandleUpdateTournamentCommand(UpdateTournamentCommand command);
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

			_uow.TournamentRepository.Add(tournament);
			
			var saveResult =_uow.SaveChanges();
			if (saveResult.IsFailure) return Result<Guid>.Failure(saveResult.Error);

			return Result<Guid>.Success(id);
		}

		public Result HandleUpdateTournamentCommand(UpdateTournamentCommand command)
		{
			var tournament = _uow.TournamentRepository.GetById(command.Id);
			if (tournament == null)
				return Result.Failure($"Tournament with Id {command.Id} not found.");

			tournament.AmendDetails(command.Title, command.Location, command.Level, command.Surface,
				command.MensSingles, command.WomensSingles, command.MensDoubles, command.WomensDoubles,
				command.MixedDoubles);

			var saveResult = _uow.SaveChanges();
			if (saveResult.IsFailure) return Result.Failure(saveResult.Error);
			
			return Result.Success();
		}
	}
}
