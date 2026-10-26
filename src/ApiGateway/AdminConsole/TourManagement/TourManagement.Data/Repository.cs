using Common.DomainDesign;
using Microsoft.EntityFrameworkCore;
using TourManagement.Application.Repository;

namespace TourManagement.Data
{
	public class Repository<TAggregate, TKey>(TourManagementDbContext context) : IRepository<TAggregate, TKey>
		where TAggregate : class, IAggregateRoot
	{
		protected readonly DbSet<TAggregate> _dbSet = context.Set<TAggregate>();

		public virtual void Add(TAggregate entity)
		{
			_dbSet.Add(entity);
		}

		public virtual TAggregate? GetById(TKey id)
		{
			var aggregate = _dbSet.Find(id);
			return aggregate;
		}
	}
}
