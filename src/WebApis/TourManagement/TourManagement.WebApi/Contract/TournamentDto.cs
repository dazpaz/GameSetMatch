namespace TourManagement.WebApi.Contract
{
	public enum TournamentLevel
	{
		GrandSlam,
		Masters1000,
		Gsm500,
		Gsm250,
		Gsm125
	}

	public enum Surface
	{
		Grass,
		Clay,
		Hard,
		Carpet
	}

	public class TournamentDto
	{
		public Guid TournamentId { get; set; }
		public required string Title { get; set; }
		public TournamentLevel Level { get; set; }
		public Surface Surface { get; set; }
		public required string Venue { get; set; }
		public required string Country { get; set; }
		public bool MensSingles { get; set; }
		public bool WomensSingles { get; set; }
		public bool MensDoubles { get; set; }
		public bool WomensDoubles { get; set; }
		public bool MixedDoubles { get; set; }
	}
}
