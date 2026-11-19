using Ardalis.GuardClauses;

namespace TourManagement.Domain.TournamentAggregate
{
	public static class TournamentGuards
	{
		public static void NoEventsIncluded(this IGuardClause guardClause, bool mensSingles, bool womensSingles,
			bool mensDoubles, bool womensDoubles, bool mixedDoubles)
		{
			var anyEvents = mensSingles || womensSingles || mensDoubles || womensDoubles || mixedDoubles;
			if (!anyEvents) throw new ArgumentException("At least one event must be included.");
		}
	}
}
