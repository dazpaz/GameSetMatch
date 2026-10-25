using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Application.Repository
{
	public interface IUnitOfWork
	{
		IRepository<Tournament, Guid> TournamentRepository { get; }
		void SaveChanges();
	}
}