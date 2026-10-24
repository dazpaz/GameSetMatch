using Common.DomainDesign;
using Ardalis.GuardClauses;

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
		public bool IsActive { get; private set; }

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
			title = Guard.Against.NullOrWhiteSpace(title);
			location = Guard.Against.NullOrWhiteSpace(location);

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
				MixedDoubles = mixedDoubles,
				IsActive = true
			};
		}

		public void AmendDetails(string title, string location,
			TournamentLevel level, Surface surface, bool mensSingles, bool womensSingles,
			bool mensDoubles, bool womensDoubles, bool mixedDoubles)
		{
			title = Guard.Against.NullOrWhiteSpace(title);
			location = Guard.Against.NullOrWhiteSpace(location);

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

		public void SetActive()
		{
			IsActive = true;
		}

		public void SetDeactive()
		{
			IsActive = false;
		}
	}
}