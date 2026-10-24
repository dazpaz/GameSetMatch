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

		[Fact]
		public void AmendDetails_WithValidValues_UpdatesAllMutableProperties_AndKeepsIdUnchanged()
		{
			var id = Guid.NewGuid();
			var tournament = Tournament.Create(
				id,
				"Initial Open",
				"Rome",
				TournamentLevel.Gsm250,
				Surface.Clay,
				mensSingles: true,
				womensSingles: true,
				mensDoubles: false,
				womensDoubles: false,
				mixedDoubles: false);

			var amendedTitle = "Championship Finals";
			var amendedLocation = "New York";
			var amendedLevel = TournamentLevel.Gsm1000;
			var amendedSurface = Surface.Hard;

			tournament.AmendDetails(
				amendedTitle,
				amendedLocation,
				amendedLevel,
				amendedSurface,
				mensSingles: false,
				womensSingles: true,
				mensDoubles: true,
				womensDoubles: true,
				mixedDoubles: true);

			Assert.Equal(id, tournament.Id);
			Assert.Equal(amendedTitle, tournament.Title);
			Assert.Equal(amendedLocation, tournament.Location);
			Assert.Equal(amendedLevel, tournament.Level);
			Assert.Equal(amendedSurface, tournament.Surface);
			Assert.False(tournament.MensSingles);
			Assert.True(tournament.WomensSingles);
			Assert.True(tournament.MensDoubles);
			Assert.True(tournament.WomensDoubles);
			Assert.True(tournament.MixedDoubles);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void AmendDetails_WithInvalidTitle_ThrowsArgumentException(string? title)
		{
			var tournament = Tournament.Create(
				Guid.NewGuid(),
				"Initial Open",
				"Rome",
				TournamentLevel.Gsm250,
				Surface.Clay,
				mensSingles: true,
				womensSingles: true,
				mensDoubles: false,
				womensDoubles: false,
				mixedDoubles: false);

			var exception = Assert.Throws<ArgumentException>(() => tournament.AmendDetails(
				title!,
				"Madrid",
				TournamentLevel.Gsm500,
				Surface.Grass,
				mensSingles: false,
				womensSingles: false,
				mensDoubles: true,
				womensDoubles: true,
				mixedDoubles: true));

			Assert.Equal("title", exception.ParamName);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("\t")]
		public void AmendDetails_WithInvalidLocation_ThrowsArgumentException(string? location)
		{
			var tournament = Tournament.Create(
				Guid.NewGuid(),
				"Initial Open",
				"Rome",
				TournamentLevel.Gsm250,
				Surface.Clay,
				mensSingles: true,
				womensSingles: true,
				mensDoubles: false,
				womensDoubles: false,
				mixedDoubles: false);

			var exception = Assert.Throws<ArgumentException>(() => tournament.AmendDetails(
				"Updated Open",
				location!,
				TournamentLevel.Gsm500,
				Surface.Grass,
				mensSingles: false,
				womensSingles: false,
				mensDoubles: true,
				womensDoubles: true,
				mixedDoubles: true));

			Assert.Equal("location", exception.ParamName);
		}
	}
}
