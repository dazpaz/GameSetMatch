using TourManagement.Application.Repository;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Data
{
	public class UnitOfWork(TourManagementDbContext context) : IUnitOfWork
	{
		private readonly TourManagementDbContext _context = context;

		private IRepository<Tournament, Guid>? _tournamentRepository;

		public IRepository<Tournament, Guid> TournamentRepository => GetTournamentRepository();

		private IRepository<Tournament, Guid> GetTournamentRepository()
		{
			_tournamentRepository ??= new Repository<Tournament, Guid>(_context);
			return _tournamentRepository;
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}
