using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentTests
	{
		[Fact]
		public void Create_WithValidValues_SetsAllProperties()
		{
			var id = Guid.NewGuid();
			var title = "Game Set Match Open";
			var location = "London";
			var level = TournamentLevel.Gsm1000;
			var surface = Surface.Grass;

			var tournament = Tournament.Create(
				id,
				title,
				location,
				level,
				surface,
				mensSingles: true,
				womensSingles: true,
				mensDoubles: true,
				womensDoubles: false,
				mixedDoubles: true);

			Assert.Equal(id, tournament.Id);
			Assert.Equal(title, tournament.Title);
			Assert.Equal(location, tournament.Location);
			Assert.Equal(level, tournament.Level);
			Assert.Equal(surface, tournament.Surface);
			Assert.True(tournament.MensSingles);
			Assert.True(tournament.WomensSingles);
			Assert.True(tournament.MensDoubles);
			Assert.False(tournament.WomensDoubles);
			Assert.True(tournament.MixedDoubles);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Create_WithInvalidTitle_ThrowsArgumentException(string? title)
		{
			var id = Guid.NewGuid();

			var exception = Assert.Throws<ArgumentException>(() => Tournament.Create(
				id,
				title!,
				"Paris",
				TournamentLevel.Gsm500,
				Surface.Clay,
				mensSingles: true,
				womensSingles: true,
				mensDoubles: false,
				womensDoubles: false,
				mixedDoubles: false));

			Assert.Equal("title", exception.ParamName);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("\t")]
		public void Create_WithInvalidLocation_ThrowsArgumentException(string? location)
		{
			var id = Guid.NewGuid();

			var exception = Assert.Throws<ArgumentException>(() => Tournament.Create(
				id,
				"Masters",
				location!,
				TournamentLevel.Gsm250,
				Surface.Hard,
				mensSingles: true,
				womensSingles: false,
				mensDoubles: true,
				womensDoubles: false,
				mixedDoubles: false));

			Assert.Equal("location", exception.ParamName);
		}
	}
}
