using TourManagement.Common;
using TourManagement.Contract;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Application.Commands
{
	public sealed class AddTournamentCommand
	{
		public string Title { get; } = string.Empty;
		public string Location { get; } = string.Empty;
		public TournamentLevel Level { get; }
		public Surface Surface { get; }
		public bool MensSingles { get; }
		public bool WomensSingles { get; }
		public bool MensDoubles { get; }
		public bool WomensDoubles { get; }
		public bool MixedDoubles { get; }

		private AddTournamentCommand(string title, string location, TournamentLevel level,
			Surface surface, bool mensSingles, bool womensSingles, bool mensDoubles,
			bool womensDoubles, bool mixedDoubles)
		{
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

		public static Result<AddTournamentCommand> Create(string title, string location,
			TournamentLevel level, Surface surface, bool mensSingles, bool womensSingles,
			bool mensDoubles, bool womensDoubles, bool mixedDoubles)
		{
			var error = TournamentRules.CheckDetails(title, location, level, surface,
				mensSingles, womensSingles, mensDoubles, womensDoubles, mixedDoubles);

			if (error is not null)
				return Result<AddTournamentCommand>.Failure(ErrorCode.ValidationFailed, error);

			var command = new AddTournamentCommand(title, location, level, surface,
				mensSingles, womensSingles, mensDoubles, womensDoubles, mixedDoubles);

			return Result<AddTournamentCommand>.Success(command);
		}

		public static Result<AddTournamentCommand> CreateFromDto(AddTournamentDto dto)
		{
			return Create(dto.Title, dto.Location, dto.Level, dto.Surface,
				dto.MensSingles, dto.WomensSingles, dto.MensDoubles,
				dto.WomensDoubles, dto.MixedDoubles);
		}
	}
}
