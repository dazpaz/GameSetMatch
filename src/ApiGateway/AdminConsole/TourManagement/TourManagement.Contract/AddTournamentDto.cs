using TourManagement.Common;

namespace TourManagement.Contract
{
	public class AddTournamentDto
	{
		public string Title { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;
		public TournamentLevel Level { get; set; }
		public Surface Surface { get; set; }
		public bool MensSingles { get; set; }
		public bool WomensSingles { get; set; }
		public bool MensDoubles { get; set; }
		public bool WomensDoubles { get; set; }
		public bool MixedDoubles { get; set; }
	}
}
