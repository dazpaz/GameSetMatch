using Common.FunctionalExtensions;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Application.Repository
{
	public interface IUnitOfWork
	{
		/// <summary>
		/// Gets the repository for Tournament entities.
		/// </summary>
		IRepository<Tournament, Guid> TournamentRepository { get; }

		/// <summary>
		/// Persists all pending changes to the underlying data store.
		/// </summary>
		/// <returns>A <c>Result&lt;int&gt;</c> containing the number of entries written.</returns>
		Result<int> SaveChanges();
	}
}