using Common.DomainDesign;
using Microsoft.EntityFrameworkCore;
using TourManagement.Application.Repository;

namespace TourManagement.Data
{
	public class Repository<TAggregate, TKey>(TourManagementDbContext context) : IRepository<TAggregate, TKey>
		where TAggregate : class, IAggregateRoot
	{
		protected readonly DbSet<TAggregate> _dbSet = context.Set<TAggregate>();

		/// <summary>
		/// Adds the specified aggregate entity to the set.
		/// </summary>
		/// <remarks>The entity is added to the context for insertion and is persisted when changes are
		/// saved.</remarks>
		/// <param name="entity">The aggregate entity to add.</param>
		public virtual void Add(TAggregate entity)
		{
			_dbSet.Add(entity);
		}

		/// <summary>
		/// Gets the aggregate root by its specified identifier.
		/// </summary>
		/// <param name="id">The identifier of the aggregate root to retrieve.</param>
		/// <returns>The aggregate root with the specified identifier, or <c>null</c> if not found.</returns>
		public virtual TAggregate? GetById(TKey id)
		{
			var aggregate = _dbSet.Find(id);
			return aggregate;
		}
	}
}
