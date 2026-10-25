using TourManagement.Application.Repository;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Data
{
	public class TempUnitOfWork : IUnitOfWork
	{
		public IRepository<Tournament, Guid> TournamentRepository => GetTournamentRepository();

		private IRepository<Tournament, Guid> GetTournamentRepository()
		{
			return new TempRepository();
		}

		public void SaveChanges()
		{
		}
	}
}
