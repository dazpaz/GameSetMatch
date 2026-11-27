using Ardalis.GuardClauses;

namespace TourManagement.Domain.TournamentAggregate
{
	public static class TournamentGuards
	{
		public static void NoEventsIncluded(this IGuardClause guardClause, bool mensSingles, bool womensSingles,
			bool mensDoubles, bool womensDoubles, bool mixedDoubles)
		{
			var error = TournamentRules.CheckEvents(mensSingles, womensSingles, mensDoubles, womensDoubles, mixedDoubles);
			if (error is not null) throw new ArgumentException(error);
		}
	}
}
