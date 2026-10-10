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
					Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
					Title = "Australian Open",
					Location = "Melbourne, Australia",
					Level = TournamentLevel.GsmSlam,
					Surface = Surface.Hard,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
					Title = "Monte-Carlo Masters",
					Location = "Monte Carlo, Monaco",
					Level = TournamentLevel.Gsm1000,
					Surface = Surface.Clay,
					MensSingles = true,
					WomensSingles = false,
					MensDoubles = true,
					WomensDoubles = false,
					MixedDoubles = false,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
					Title = "Wimbledon",
					Location = "London, United Kingdom",
					Level = TournamentLevel.GsmSlam,
					Surface = Surface.Grass,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
					Title = "Indian Wells Masters",
					Location = "Indian Wells, United States",
					Level = TournamentLevel.Gsm1000,
					Surface = Surface.Hard,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = false,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
					Title = "Barcelona Open",
					Location = "Barcelona, Spain",
					Level = TournamentLevel.Gsm500,
					Surface = Surface.Clay,
					MensSingles = true,
					WomensSingles = false,
					MensDoubles = true,
					WomensDoubles = false,
					MixedDoubles = false,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
					Title = "Stuttgart Open",
					Location = "Stuttgart, Germany",
					Level = TournamentLevel.Gsm250,
					Surface = Surface.Grass,
					MensSingles = false,
					WomensSingles = true,
					MensDoubles = false,
					WomensDoubles = true,
					MixedDoubles = false,
					IsActive = false
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
					Title = "Lyon Challenger",
					Location = "Lyon, France",
					Level = TournamentLevel.Gsm125,
					Surface = Surface.Carpet,
					MensSingles = true,
					WomensSingles = false,
					MensDoubles = false,
					WomensDoubles = true,
					MixedDoubles = false,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
					Title = "US Open",
					Location = "New York, United States",
					Level = TournamentLevel.GsmSlam,
					Surface = Surface.Hard,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = false,
					WomensDoubles = false,
					MixedDoubles = true,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
					Title = "French Open",
					Location = "Paris, France",
					Level = TournamentLevel.GsmSlam,
					Surface = Surface.Clay,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true,
					IsActive = true
				},
				new TournamentDto
				{
					Id = Guid.Parse("00000000-0000-0000-0000-00000000000a"),
					Title = "Miami Open",
					Location = "Miami, United States",
					Level = TournamentLevel.Gsm1000,
					Surface = Surface.Hard,
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = false,
					IsActive = false
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
