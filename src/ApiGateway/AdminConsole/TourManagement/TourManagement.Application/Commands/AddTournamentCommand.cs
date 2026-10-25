using Common.FunctionalExtensions;
using TourManagement.Common;

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
			if (string.IsNullOrWhiteSpace(title))
				return Result<AddTournamentCommand>.Failure("Title cannot be blank");

			if (string.IsNullOrWhiteSpace(location))
				return Result<AddTournamentCommand>.Failure("Location cannot be blank");

			if (!Enum.IsDefined(level))
				return Result<AddTournamentCommand>.Failure("Invalid tournament level.");

			if (!Enum.IsDefined(surface))
				return Result<AddTournamentCommand>.Failure("Invalid surface.");

			var command = new AddTournamentCommand(title, location, level, surface,
				mensSingles, womensSingles, mensDoubles, womensDoubles, mixedDoubles);

			return Result<AddTournamentCommand>.Success(command);
		}
	}
}
