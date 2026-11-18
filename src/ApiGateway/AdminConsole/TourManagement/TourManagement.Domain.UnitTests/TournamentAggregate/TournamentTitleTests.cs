using TourManagement.Common;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentTitleTests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Create_WithInvalidTitle_ThrowsArgumentException(string? title)
		{
			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentTitle.Create(title!));

			Assert.Equal("title", exception.ParamName);
		}

		[Theory]
		[InlineData("Game<Set>Match")]
		[InlineData("Open\nFinals")]
		[InlineData("Open\tFinals")]
		[InlineData("Open@Home")]
		[InlineData("Open;DROP")]
		[InlineData("Open\u200BFinals")]
		public void Create_WithInvalidCharacters_ThrowsArgumentException(string title)
		{
			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentTitle.Create(title));

			Assert.Equal("title", exception.ParamName);
		}

		[Theory]
		[InlineData("Game Set Match Open")]
		[InlineData("St. Mary's Cup (Men & Women)")]
		[InlineData("Roland-Garros 2025: Finals!")]
		[InlineData("Open d'Été, Paris")]
		public void Create_WithValidCharacters_Succeeds(string title)
		{
			var result = TournamentTitle.Create(title);

			Assert.Equal(title, result);
		}

		[Fact]
		public void Create_WithMaxLengthTitle_Succeeds()
		{
			var title = new string('a', TournamentTitle.MaxLength);

			var result = TournamentTitle.Create(title);

			Assert.Equal(title, result);
		}

		[Fact]
		public void Create_WithTitleOneCharacterOverMaxLength_ThrowsArgumentException()
		{
			var title = new string('a', TournamentTitle.MaxLength + 1);

			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentTitle.Create(title));

			Assert.Equal("title", exception.ParamName);
		}

		[Fact]
		public void Create_WithSingleCharacterTitle_Succeeds()
		{
			var result = TournamentTitle.Create("a");

			Assert.Equal("a", result);
		}

		[Fact]
		public void Create_WithMaxLengthTitleAndSurroundingWhitespace_SucceedsAndTrims()
		{
			var trimmed = new string('a', TournamentTitle.MaxLength);

			var result = TournamentTitle.Create("  " + trimmed + "\t ");

			Assert.Equal(trimmed, result);
		}

		[Fact]
		public void Create_WithTitleOverMaxLengthAfterTrimming_ThrowsArgumentException()
		{
			var title = " " + new string('a', TournamentTitle.MaxLength + 1) + " ";

			var exception = Assert.ThrowsAny<ArgumentException>(() => TournamentTitle.Create(title));

			Assert.Equal("title", exception.ParamName);
		}

		[Fact]
		public void Explicit_Conversion_WithTitleOverMaxLength_ThrowsArgumentException()
		{
			var title = new string('a', TournamentTitle.MaxLength + 1);

			Assert.ThrowsAny<ArgumentException>(() => (TournamentTitle)title);
		}

		[Fact]
		public void Tournament_Create_WithTitleOverMaxLength_ThrowsArgumentException()
		{
			var title = new string('a', TournamentTitle.MaxLength + 1);

			var exception = Assert.ThrowsAny<ArgumentException>(() => Tournament.Create(
				Guid.NewGuid(),
				title,
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
	}
}
