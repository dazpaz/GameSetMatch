using TourManagement.Common;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentLocationTests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Create_WithInvalidLocation_ThrowsArgumentException(string? location)
		{
			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentLocation.Create(location!));

			Assert.Equal("location", exception.ParamName);
		}

		[Theory]
		[InlineData("Game<Set>Match")]
		[InlineData("Open\nFinals")]
		[InlineData("Open\tFinals")]
		[InlineData("Open@Home")]
		[InlineData("Open;DROP")]
		[InlineData("Open\u200BFinals")]
		public void Create_WithInvalidCharacters_ThrowsArgumentException(string location)
		{
			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentLocation.Create(location));

			Assert.Equal("location", exception.ParamName);
		}

		[Theory]
		[InlineData("London")]
		[InlineData("St. Mary's Park (Court 1)")]
		[InlineData("Flushing Meadows, New York")]
		[InlineData("Roland-Garros: Paris!")]
		[InlineData("Rue d'Été & Co")]
		public void Create_WithValidCharacters_Succeeds(string location)
		{
			var result = TournamentLocation.Create(location);

			Assert.Equal(location, result);
		}

		[Fact]
		public void Create_WithMaxLengthLocation_Succeeds()
		{
			var location = new string('a', TournamentLocation.MaxLength);

			var result = TournamentLocation.Create(location);

			Assert.Equal(location, result);
		}

		[Fact]
		public void Create_WithLocationOneCharacterOverMaxLength_ThrowsArgumentException()
		{
			var location = new string('a', TournamentLocation.MaxLength + 1);

			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentLocation.Create(location));

			Assert.Equal("location", exception.ParamName);
		}

		[Fact]
		public void Create_WithSingleCharacterLocation_Succeeds()
		{
			var result = TournamentLocation.Create("a");

			Assert.Equal("a", result);
		}

		[Fact]
		public void Create_WithMaxLengthLocationAndSurroundingWhitespace_SucceedsAndTrims()
		{
			var trimmed = new string('a', TournamentLocation.MaxLength);

			var result = TournamentLocation.Create("  " + trimmed + "\t ");

			Assert.Equal(trimmed, result);
		}

		[Fact]
		public void Create_WithLocationOverMaxLengthAfterTrimming_ThrowsArgumentException()
		{
			var location = " " + new string('a', TournamentLocation.MaxLength + 1) + " ";

			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentLocation.Create(location));

			Assert.Equal("location", exception.ParamName);
		}

		[Fact]
		public void Explicit_Conversion_WithLocationOverMaxLength_ThrowsArgumentException()
		{
			var location = new string('a', TournamentLocation.MaxLength + 1);

			Assert.ThrowsAny<ArgumentException>(() => (TournamentLocation)location);
		}

		[Fact]
		public void Tournament_Create_WithLocationOverMaxLength_ThrowsArgumentException()
		{
			var location = new string('a', TournamentLocation.MaxLength + 1);

			var exception = Assert.ThrowsAny<ArgumentException>(() => Tournament.Create(
				Guid.NewGuid(),
				"Paris Open",
				location,
				TournamentLevel.Gsm500,
				Surface.Clay,
				mensSingles: true,
				womensSingles: true,
				mensDoubles: false,
				womensDoubles: false,
				mixedDoubles: false));

			Assert.Equal("location", exception.ParamName);
		}
	}
}
