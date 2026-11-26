using TourManagement.Common;
using TourManagement.Contract;

namespace TourManagement.Application.Commands
{
	public class UpdateTournamentCommand
	{
		public Guid Id { get; }
		public string Title { get; } = string.Empty;
		public string Location { get; } = string.Empty;
		public TournamentLevel Level { get; }
		public Surface Surface { get; }
		public bool MensSingles { get; }
		public bool WomensSingles { get; }
		public bool MensDoubles { get; }
		public bool WomensDoubles { get; }
		public bool MixedDoubles { get; }

		private UpdateTournamentCommand(Guid id, string title, string location, TournamentLevel level,
			Surface surface, bool mensSingles, bool womensSingles, bool mensDoubles,
			bool womensDoubles, bool mixedDoubles)
		{
			Id = id;
			Title = title;
			Location = location;
			Level = level;
			Surface = surface;
			MensSingles = mensSingles;
			WomensSingles = womensSingles;
			MensDoubles = mensDoubles;
			WomensDoubles = womensDoubles;
			MixedDoubles = mixedDoubles;
		}

		public static Result<UpdateTournamentCommand> Create(Guid id, string title, string location,
			TournamentLevel level, Surface surface, bool mensSingles, bool womensSingles,
			bool mensDoubles, bool womensDoubles, bool mixedDoubles)
		{
			if (id == Guid.Empty)
				return Result<UpdateTournamentCommand>
					.Failure(ErrorCode.ValidationFailed, "Id cannot be empty");

			if (string.IsNullOrWhiteSpace(title))
				return Result<UpdateTournamentCommand>
					.Failure(ErrorCode.ValidationFailed, "Title cannot be blank");

			if (string.IsNullOrWhiteSpace(location))
				return Result<UpdateTournamentCommand>
					.Failure(ErrorCode.ValidationFailed, "Location cannot be blank");

			if (!Enum.IsDefined(level))
				return Result<UpdateTournamentCommand>
					.Failure(ErrorCode.ValidationFailed, "Invalid tournament level.");

			if (!Enum.IsDefined(surface))
				return Result<UpdateTournamentCommand>
					.Failure(ErrorCode.ValidationFailed, "Invalid surface.");

			if (!(mensSingles || womensSingles || mensDoubles || womensDoubles || mixedDoubles))
				return Result<UpdateTournamentCommand>
					.Failure(ErrorCode.ValidationFailed, "At least one event must be included.");

			var command = new UpdateTournamentCommand(id, title, location, level, surface,
				mensSingles, womensSingles, mensDoubles, womensDoubles, mixedDoubles);

			return Result<UpdateTournamentCommand>.Success(command);
		}

		public static Result<UpdateTournamentCommand> CreateFromDto(Guid id, UpdateTournamentDto dto)
		{
			return Create(id, dto.Title, dto.Location, dto.Level, dto.Surface,
				dto.MensSingles, dto.WomensSingles, dto.MensDoubles,
				dto.WomensDoubles, dto.MixedDoubles);
		}
	}
}
