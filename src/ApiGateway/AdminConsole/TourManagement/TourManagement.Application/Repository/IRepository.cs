using Common.DomainDesign;

namespace TourManagement.Application.Repository
{
	public interface IRepository<TAggregate, TKey> where TAggregate : class, IAggregateRoot
	{
		/// <summary>
		/// Gets the aggregate root by its specified identifier.
		/// </summary>
		/// <param name="id">The identifier of the aggregate root to retrieve.</param>
		/// <returns>The aggregate root with the specified identifier, or <c>null</c> if not found.</returns>
		TAggregate? GetById(TKey id);

		
		/// <summary>
		/// Adds the specified aggregate root to the repository.
		/// </summary>
		/// <param name="aggregate">The aggregate root to add.</param>
		void Add(TAggregate aggregate);
	}
}