using Microsoft.AspNetCore.Mvc;

namespace TourManagement.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TournamentsController : ControllerBase
	{
		// GET: api/Tournament
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "Australian Open", "French Open", "Wimbledon",
				"Brisbane International", "Nottingham Open", "Monte-Carlo Masters" };
		}
	}
}
