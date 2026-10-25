using TourManagement.Application.Repository;
using TourManagement.Common;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Data
{
	public class TempRepository : IRepository<Tournament, Guid>
	{
		public void Add(Tournament aggregate)
		{
			
		}

		public Tournament GetById(Guid id)
		{
			return Tournament.Create(id, "Sample Tournament", "Sample Location", TournamentLevel.GsmSlam,
				Surface.Hard, true, true, true, true, true);
		}
	}
}
