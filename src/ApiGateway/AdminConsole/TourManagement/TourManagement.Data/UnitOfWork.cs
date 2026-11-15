using Common.FunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TourManagement.Application.Repository;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Data
{
	public class UnitOfWork(TourManagementDbContext context) : IUnitOfWork
	{
		/// <summary>
		/// Provides access to the tour management database context.
		/// </summary>
		private readonly TourManagementDbContext _context = context;

		/// <summary>
		/// Repository used to access and persist <see cref="Tournament"/> entities by <see cref="Guid"/> identifier.
		/// </summary>
		private IRepository<Tournament, Guid>? _tournamentRepository;

		/// <summary>
		/// Gets the repository for managing <c>Tournament</c> entities.
		/// </summary>
		public IRepository<Tournament, Guid> TournamentRepository => GetTournamentRepository();

		/// <summary>
		/// Saves all pending changes in the underlying data context.
		/// </summary>
		/// <remarks>Handles <c>DbUpdateConcurrencyException</c> and <c>DbUpdateException</c> and returns a failure
		/// result instead of throwing.</remarks>
		/// <returns>A successful <c>Result&lt;int&gt;</c> containing the number of state entries written to the database, or a failed
		/// result containing an error message when a concurrency or update error occurs.</returns>
		public Result<int> SaveChanges()
		{
			try
			{
				var changes = _context.SaveChanges();
				return Result<int>.Success(changes);
			}
			catch (DbUpdateConcurrencyException ex)
			{
				// Log the concurrency exception details for debugging purposes
				return Result<int>.Failure($"A concurrency error occurred while saving changes to the database: {ex.Message}");
			}
			catch (DbUpdateException ex)
			{
				// Log the update exception details for debugging purposes
				return Result<int>.Failure($"An error occurred while saving changes to the database: {ex.Message}");
			}
		}

		private IRepository<Tournament, Guid> GetTournamentRepository()
		{
			_tournamentRepository ??= new Repository<Tournament, Guid>(_context);
			return _tournamentRepository;
		}
	}
}
