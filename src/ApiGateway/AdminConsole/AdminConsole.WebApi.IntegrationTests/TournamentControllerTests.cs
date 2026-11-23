using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using TourManagement.Common;
using TourManagement.Contract;

namespace AdminConsole.WebApi.IntegrationTests
{
	public class TournamentControllerTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		public TournamentControllerTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task GetAllTournaments_ReturnsListOfAllTournamentsIncludingActiveAndInactive()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatedTheRequestWasSuccess(response);
			var tournaments = await GetTournamentsFromResponse(response);
			TheCorrectNumberofActiveAndInactiveTournamentsAreReturned(tournaments);
		}

		private static void TheResponseIndicatedTheRequestWasSuccess(HttpResponseMessage response)
		{
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
		}

		private static void TheCorrectNumberofActiveAndInactiveTournamentsAreReturned(TournamentDto[]? tournaments)
		{
			Assert.NotNull(tournaments);
			Assert.Equal(10, tournaments.Length);
			Assert.Equal(8, tournaments.Count(t => t.IsActive));
			Assert.Equal(2, tournaments.Count(t => !t.IsActive));
		}

		[Fact]
		public async Task GetAllTournaments_TournamentsHaveCorrectSurfaceAndLevelCounts()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);
			var json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			var tournaments = await GetTournamentsFromResponse(response);
			Assert.NotNull(tournaments);

			Assert.Equal(4, tournaments.Count(t => t.Level == TournamentLevel.GsmSlam));
			Assert.Equal(3, tournaments.Count(t => t.Level == TournamentLevel.Gsm1000));
			Assert.Equal(1, tournaments.Count(t => t.Level == TournamentLevel.Gsm500));
			Assert.Equal(1, tournaments.Count(t => t.Level == TournamentLevel.Gsm250));
			Assert.Equal(1, tournaments.Count(t => t.Level == TournamentLevel.Gsm125));

			Assert.Equal(2, tournaments.Count(t => t.Surface == Surface.Grass));
			Assert.Equal(3, tournaments.Count(t => t.Surface == Surface.Clay));
			Assert.Equal(4, tournaments.Count(t => t.Surface == Surface.Hard));
			Assert.Equal(1, tournaments.Count(t => t.Surface == Surface.Carpet));
		}

		[Fact]
		public async Task GetAllTournaments_ReturnsCorrectDetailsForATournament()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);
			var json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			var tournaments = await GetTournamentsFromResponse(response);
			Assert.NotNull(tournaments);
			var frenchOpen = tournaments.FirstOrDefault(t => t.Title == "French Open");
			Assert.NotNull(frenchOpen);

			Assert.Equal("French Open", frenchOpen.Title);
			Assert.Equal("Paris, France", frenchOpen.Location);
			Assert.Equal(TournamentLevel.GsmSlam, frenchOpen.Level);
			Assert.Equal(Surface.Clay, frenchOpen.Surface);
			Assert.True(frenchOpen.MensSingles);
			Assert.True(frenchOpen.WomensSingles);
			Assert.True(frenchOpen.MensDoubles);
			Assert.True(frenchOpen.WomensDoubles);
			Assert.True(frenchOpen.MixedDoubles);
			Assert.True(frenchOpen.IsActive);
		}

		[Fact]
		public async Task GetSingleTournament_ReturnsDetailsOfCorrectTournament()
		{
			// Arrange
			var client = _factory.CreateClient();
			var tournamentId = "00000000-0000-0000-0000-000000000006";

			// Act
			var response = await client.GetAsync($"/tournaments/{tournamentId}", TestContext.Current.CancellationToken);
			var json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
			
			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			var options = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);
			var tournament = await response.Content.ReadFromJsonAsync<TournamentDto>(options, TestContext.Current.CancellationToken);

			Assert.NotNull(tournament);
			Assert.Equal("Stuttgart Open", tournament.Title);
			Assert.Equal("Stuttgart, Germany", tournament.Location);
			Assert.Equal(TournamentLevel.Gsm250, tournament.Level);
			Assert.Equal(Surface.Grass, tournament.Surface);
			Assert.False(tournament.MensSingles);
			Assert.True(tournament.WomensSingles);
			Assert.False(tournament.MensDoubles);
			Assert.True(tournament.WomensDoubles);
			Assert.False(tournament.MixedDoubles);
			Assert.False(tournament.IsActive);
		}

		[Theory]
		[InlineData("00000000-0000-0000-0000-000000000000")]
		[InlineData("00000000-0000-0000-0000-111111111111")]
		public async Task GetSingleTournament_RequestingTounamentThatDoesNotExist_ReturnsNotFound(string tournamentId)
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync($"/tournaments/{tournamentId}", TestContext.Current.CancellationToken);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
		}

		[Theory]
		[InlineData("00000000-0000-0000-0000-00000011")]
		[InlineData("not-a-guid")]
		public async Task GetSingleTournament_RequestingTounamentWithInvalidId_ReturnsBadRequest(string tournamentId)
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync($"/tournaments/{tournamentId}", TestContext.Current.CancellationToken);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task AddTournament_WithValidData_ReturnsCreated_AndTournamentCanBeRetrieved()
		{
			// Arrange
			var client = _factory.CreateClient();
			AddTournamentDto newTournament = CreateTournamentToAdd();

			// Act
			var response = await client.PostAsJsonAsync("/tournaments", newTournament, TestContext.Current.CancellationToken);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
			Assert.NotNull(response.Headers.Location);

			var created = await client.GetFromJsonAsync<TournamentDto>(response.Headers.Location, TestContext.Current.CancellationToken);
			Assert.NotNull(created);
			Assert.Equal(newTournament.Title, created.Title);
			Assert.Equal(newTournament.Location, created.Location);
			Assert.Equal(newTournament.Level, created.Level);
			Assert.Equal(newTournament.Surface, created.Surface);

			// finally, clean up by deleting the tournament - this will have to be done with SQL
		}



		[Fact]
		public async Task UpdateTournament_AfterAdding_UpdatesAllFields()
		{
			// Arrange
			var client = _factory.CreateClient();
			var addResponse = await client.PostAsJsonAsync("/tournaments", CreateTournamentToAdd(), TestContext.Current.CancellationToken);
			Assert.Equal(System.Net.HttpStatusCode.Created, addResponse.StatusCode);
			Assert.NotNull(addResponse.Headers.Location);

			var updateTournament = new UpdateTournamentDto
			{
				Title = "Updated Integration Test Open",
				Location = "Updated City",
				Level = TournamentLevel.Gsm500,
				Surface = Surface.Clay,
				MensSingles = false,
				WomensSingles = false,
				MensDoubles = false,
				WomensDoubles = false,
				MixedDoubles = true
			};

			// Act
			var updateResponse = await client.PutAsJsonAsync(addResponse.Headers.Location, updateTournament, TestContext.Current.CancellationToken);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.NoContent, updateResponse.StatusCode);

			var updated = await client.GetFromJsonAsync<TournamentDto>(addResponse.Headers.Location, TestContext.Current.CancellationToken);
			Assert.NotNull(updated);
			Assert.Equal(updateTournament.Title, updated.Title);
			Assert.Equal(updateTournament.Location, updated.Location);
			Assert.Equal(updateTournament.Level, updated.Level);
			Assert.Equal(updateTournament.Surface, updated.Surface);
			Assert.Equal(updateTournament.MensSingles, updated.MensSingles);
			Assert.Equal(updateTournament.WomensSingles, updated.WomensSingles);
			Assert.Equal(updateTournament.MensDoubles, updated.MensDoubles);
			Assert.Equal(updateTournament.WomensDoubles, updated.WomensDoubles);
			Assert.Equal(updateTournament.MixedDoubles, updated.MixedDoubles);

			// finally, clean up by deleting the tournament - this will have to be done with SQL
		}

		private static AddTournamentDto CreateTournamentToAdd()
		{
			return new AddTournamentDto
			{
				Title = "Integration Test Open",
				Location = "Test City",
				Level = TournamentLevel.Gsm250,
				Surface = Surface.Hard,
				MensSingles = true,
				WomensSingles = true,
				MensDoubles = true,
				WomensDoubles = true,
				MixedDoubles = false
			};
		}

		private static async Task<TournamentDto[]?> GetTournamentsFromResponse(HttpResponseMessage response)
		{
			var options = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);
			var tournaments = await response.Content.ReadFromJsonAsync<TournamentDto[]>(options, TestContext.Current.CancellationToken);
			return tournaments;
		}
	}
}
