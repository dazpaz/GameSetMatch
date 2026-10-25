using Common.DomainDesign;

namespace TourManagement.Application.Repository
{
	public interface IRepository<TAggregate, TKey> where TAggregate : class, IAggregateRoot
	{
		TAggregate GetById(TKey id);
		void Add(TAggregate aggregate);
	}
}