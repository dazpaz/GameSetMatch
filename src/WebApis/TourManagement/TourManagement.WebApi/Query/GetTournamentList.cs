using TourManagement.WebApi.Contract;

namespace TourManagement.WebApi.Query
{
	public class GetTournamentList
	{
		public static IEnumerable<TournamentDto> Execute()
		{
			return new List<TournamentDto>
			{
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "Australian Open",
					Level = TournamentLevel.GrandSlam,
					Surface = Surface.Hard,
					Venue = "Melbourne Park",
					Country = "Australia",
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true
				},
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "French Open",
					Level = TournamentLevel.GrandSlam,
					Surface = Surface.Clay,
					Venue = "Roland Garros",
					Country = "France",
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true
				},
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "Wimbledon",
					Level = TournamentLevel.GrandSlam,
					Surface = Surface.Grass,
					Venue = "All England Lawn Tennis Club",
					Country = "United Kingdom",
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true
				},
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "US Open",
					Level = TournamentLevel.GrandSlam,
					Surface = Surface.Hard,
					Venue = "Flushing Meadows",
					Country = "United States",
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = true
				},
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "Indian Wells Masters",
					Level = TournamentLevel.Masters1000,
					Surface = Surface.Hard,
					Venue = "Indian Wells Tennis Garden",
					Country = "United States",
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = false
				},
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "Miami Open",
					Level = TournamentLevel.Masters1000,
					Surface = Surface.Hard,
					Venue = "Hard Rock Stadium",
					Country = "United States",
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = true,
					WomensDoubles = true,
					MixedDoubles = false
				},
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "Monte-Carlo Masters",
					Level = TournamentLevel.Masters1000,
					Surface = Surface.Clay,
					Venue = "Monte Carlo Country Club",
					Country = "Monaco",
					MensSingles = true,
					WomensSingles = false,
					MensDoubles = true,
					WomensDoubles = false,
					MixedDoubles = false
				},
				new TournamentDto
				{
					TournamentId = Guid.NewGuid(),
					Title = "Entry Level Tournament",
					Level = TournamentLevel.GSM125,
					Surface = Surface.Carpet,
					Venue = "My Tennis Centre",
					Country = "United Kingdom",
					MensSingles = true,
					WomensSingles = true,
					MensDoubles = false,
					WomensDoubles = false,
					MixedDoubles = false
				}
			};
		}
	}
}
