using TourManagement.Application.Repository;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Data
{
	public class TempUnitOfWork : IUnitOfWork
	{
		public IRepository<Tournament, Guid> TournamentRepository => throw new NotImplementedException();

		public void SaveChanges()
		{
		}
	}
}
