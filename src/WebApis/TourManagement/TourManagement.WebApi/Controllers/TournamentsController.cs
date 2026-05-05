using Microsoft.AspNetCore.Mvc;
using TourManagement.WebApi.Contract;
using TourManagement.WebApi.Query;

namespace TourManagement.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TournamentsController : ControllerBase
	{
		// GET: api/Tournament
		[HttpGet]
		public IEnumerable<TournamentDto> Get()
		{
			return GetTournamentList.Execute();
		}
	}
}
