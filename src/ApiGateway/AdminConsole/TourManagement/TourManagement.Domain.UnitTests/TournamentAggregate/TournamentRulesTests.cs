using TourManagement.Common;
using TourManagement.Domain.TournamentAggregate;

namespace TourManagement.Domain.UnitTests.TournamentAggregate
{
	public class TournamentRulesTests
	{
		[Theory]
		[InlineData("Game Set Match Open")]
		[InlineData("St. Mary's Cup (2025) & Finals: 1/2-3!")]
		public void CheckTitle_WithValidTitle_ReturnsNull(string title)
		{
			Assert.Null(TournamentRules.CheckTitle(title));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void CheckTitle_WithBlankTitle_ReturnsError(string? title)
		{
			Assert.Equal("Title cannot be blank", TournamentRules.CheckTitle(title));
		}

		[Fact]
		public void CheckTitle_WithMaxLengthTitle_ReturnsNull()
		{
			var title = new string('a', TournamentTitle.MaxLength);

			Assert.Null(TournamentRules.CheckTitle(title));
		}

		[Fact]
		public void CheckTitle_OverMaxLength_ReturnsError()
		{
			var title = new string('a', TournamentTitle.MaxLength + 1);

			Assert.Equal($"Title cannot be longer than {TournamentTitle.MaxLength} characters",
				TournamentRules.CheckTitle(title));
		}

		[Theory]
		[InlineData("Bad@Title")]
		[InlineData("Bad<Title>")]
		public void CheckTitle_WithInvalidCharacters_ReturnsError(string title)
		{
			Assert.Equal("Title contains invalid characters", TournamentRules.CheckTitle(title));
		}

		[Fact]
		public void CheckLocation_WithValidLocation_ReturnsNull()
		{
			Assert.Null(TournamentRules.CheckLocation("London"));
		}

		[Fact]
		public void CheckLocation_WithBlankLocation_ReturnsError()
		{
			Assert.Equal("Location cannot be blank", TournamentRules.CheckLocation(" "));
		}

		[Fact]
		public void CheckLocation_OverMaxLength_ReturnsError()
		{
			var location = new string('a', TournamentLocation.MaxLength + 1);

			Assert.Equal($"Location cannot be longer than {TournamentLocation.MaxLength} characters",
				TournamentRules.CheckLocation(location));
		}

		[Fact]
		public void CheckLocation_WithInvalidCharacters_ReturnsError()
		{
			Assert.Equal("Location contains invalid characters",
				TournamentRules.CheckLocation("Lon$don"));
		}

		[Fact]
		public void CheckId_WithNonEmptyGuid_ReturnsNull()
		{
			Assert.Null(TournamentRules.CheckId(Guid.NewGuid()));
		}

		[Fact]
		public void CheckId_WithEmptyGuid_ReturnsError()
		{
			Assert.Equal("Id cannot be empty", TournamentRules.CheckId(Guid.Empty));
		}

		[Fact]
		public void CheckLevel_WithDefinedLevel_ReturnsNull()
		{
			Assert.Null(TournamentRules.CheckLevel(TournamentLevel.Gsm1000));
		}

		[Fact]
		public void CheckLevel_WithUndefinedLevel_ReturnsError()
		{
			Assert.Equal("Invalid tournament level.",
				TournamentRules.CheckLevel((TournamentLevel)9999));
		}

		[Fact]
		public void CheckSurface_WithDefinedSurface_ReturnsNull()
		{
			Assert.Null(TournamentRules.CheckSurface(Surface.Grass));
		}

		[Fact]
		public void CheckSurface_WithUndefinedSurface_ReturnsError()
		{
			Assert.Equal("Invalid surface.", TournamentRules.CheckSurface((Surface)9999));
		}

		[Theory]
		[InlineData(true, false, false, false, false)]
		[InlineData(false, true, false, false, false)]
		[InlineData(false, false, true, false, false)]
		[InlineData(false, false, false, true, false)]
		[InlineData(false, false, false, false, true)]
		[InlineData(true, true, true, true, true)]
		public void CheckEvents_WithAtLeastOneEvent_ReturnsNull(bool ms, bool ws,
			bool md, bool wd, bool mx)
		{
			Assert.Null(TournamentRules.CheckEvents(ms, ws, md, wd, mx));
		}

		[Fact]
		public void CheckEvents_WithNoEvents_ReturnsError()
		{
			Assert.Equal("At least one event must be included.",
				TournamentRules.CheckEvents(false, false, false, false, false));
		}

		[Fact]
		public void CheckDetails_WithValidValues_ReturnsNull()
		{
			var result = TournamentRules.CheckDetails("Open", "London", TournamentLevel.Gsm1000,
				Surface.Grass, true, false, false, false, false);

			Assert.Null(result);
		}

		[Fact]
		public void CheckDetails_WithInvalidTitleAndLocation_ReturnsTitleErrorFirst()
		{
			var result = TournamentRules.CheckDetails("", "", TournamentLevel.Gsm1000,
				Surface.Grass, true, false, false, false, false);

			Assert.Equal("Title cannot be blank", result);
		}

		[Fact]
		public void CheckDetails_WithInvalidLocation_ReturnsLocationError()
		{
			var result = TournamentRules.CheckDetails("Open", "", TournamentLevel.Gsm1000,
				Surface.Grass, true, false, false, false, false);

			Assert.Equal("Location cannot be blank", result);
		}

		[Fact]
		public void CheckDetails_WithInvalidLevel_ReturnsLevelError()
		{
			var result = TournamentRules.CheckDetails("Open", "London", (TournamentLevel)9999,
				Surface.Grass, true, false, false, false, false);

			Assert.Equal("Invalid tournament level.", result);
		}

		[Fact]
		public void CheckDetails_WithInvalidSurface_ReturnsSurfaceError()
		{
			var result = TournamentRules.CheckDetails("Open", "London", TournamentLevel.Gsm1000,
				(Surface)9999, true, false, false, false, false);

			Assert.Equal("Invalid surface.", result);
		}

		[Fact]
		public void CheckDetails_WithNoEvents_ReturnsEventsError()
		{
			var result = TournamentRules.CheckDetails("Open", "London", TournamentLevel.Gsm1000,
				Surface.Grass, false, false, false, false, false);

			Assert.Equal("At least one event must be included.", result);
		}
	}
}
