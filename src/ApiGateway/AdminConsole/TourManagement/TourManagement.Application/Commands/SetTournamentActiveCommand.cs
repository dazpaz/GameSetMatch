using TourManagement.Common;
using TourManagement.Contract;

namespace TourManagement.Application.Commands
{
	public class SetTournamentActiveCommand
	{
		public Guid Id { get; }
		public bool IsActive { get; }

		private SetTournamentActiveCommand(Guid id, bool isActive)
		{
			Id = id;
			IsActive = isActive;
		}

		public static Result<SetTournamentActiveCommand> Create(Guid id, bool isActive)
		{
			if (id == Guid.Empty)
				return Result<SetTournamentActiveCommand>
					.Failure(ErrorCode.ValidationFailed, "Id cannot be empty");

			return Result<SetTournamentActiveCommand>.Success(new SetTournamentActiveCommand(id, isActive));
		}

		public static Result<SetTournamentActiveCommand> CreateFromDto(Guid id, SetTournamentActiveDto dto)
		{
			return Create(id, dto.IsActive);
		}
	}
}
