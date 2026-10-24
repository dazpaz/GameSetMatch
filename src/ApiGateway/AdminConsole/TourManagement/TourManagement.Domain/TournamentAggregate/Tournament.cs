using DomainDesign.Common;

namespace TourManagement.Domain.TournamentAggregate
{
	public class Tournament : AggregateRoot<Guid>
	{
		public string Title { get; private set; } = string.Empty;
		public string Location { get; private set; } = string.Empty;
		public TournamentLevel Level { get; private set; }
		public Surface Surface { get; private set; }
		public bool MensSingles { get; private set; }
		public bool WomensSingles { get; private set; }
		public bool MensDoubles { get; private set; }
		public bool WomensDoubles { get; private set; }
		public bool MixedDoubles { get; private set; }

		protected Tournament()
		{
		}

		private Tournament(Guid id) : base(id)
		{
		}

		public static Tournament Create(Guid id, string title, string location,
			TournamentLevel level, Surface surface, bool mensSingles, bool womensSingles,
			bool mensDoubles, bool womensDoubles, bool mixedDoubles)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));
			}
			if (string.IsNullOrWhiteSpace(location))
			{
				throw new ArgumentException("Location cannot be null or whitespace.", nameof(location));
			}

			return new Tournament(id)
			{
				Title = title,
				Location = location,
				Level = level,
				Surface = surface,
				MensSingles = mensSingles,
				WomensSingles = womensSingles,
				MensDoubles = mensDoubles,
				WomensDoubles = womensDoubles,
				MixedDoubles = mixedDoubles
			};
		}
	}
}
