using Microsoft.AspNetCore.Mvc;

namespace AdminConsoleApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TournamentsController : ControllerBase
	{
		[HttpGet(Name = "GetTournaments")]
		public IEnumerable<TournamentDto> Get()
		{
			return GetTournamentList();
		}

		private static List<TournamentDto> GetTournamentList()
		{
			return
			[
				new TournamentDto
				{
					Id = Guid.NewGuid(),
					Title = "Australian Open",
					Location = "Melbourne, Australia",
					Surface = Surface.Hard,
					Level = TournamentLevel.GsmSlam,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.NewGuid(),
					Title = "French Open",
					Location = "Paris, France",
					Surface = Surface.Clay,
					Level = TournamentLevel.GsmSlam,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.NewGuid(),
					Title = "Wimbledon",
					Location = "London, England",
					Surface = Surface.Grass,
					Level = TournamentLevel.GsmSlam,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.NewGuid(),
					Title = "US Open",
					Location = "New York, USA",
					Surface = Surface.Hard,
					Level = TournamentLevel.GsmSlam,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true,
					IsActive = true
				}
			];
		}
	}

	public enum Surface
	{
		Grass,
		Clay,
		Hard,
		Carpet
	}

	public enum TournamentLevel
	{
		GsmSlam,
		Gsm1000,
		Gsm500,
		Gsm250,
		Gsm125
	}

	public class TournamentDto
	{
		public Guid Id { get; set; }
		public required string Title { get; set; }
		public required string Location { get; set; }
		public Surface Surface { get; set; }
		public TournamentLevel Level { get; set; }
		public bool MensSingles { get; set; }
		public bool WomensSingles { get; set; }
		public bool MensDoubles { get; set; }
		public bool WomensDoubles { get; set; }
		public bool MixedDoubles { get; set; }
		public bool IsActive { get; set; }
	}
}
