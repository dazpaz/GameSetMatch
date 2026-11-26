using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TourManagement.Common;
using TourManagement.Contract;
using TourManagement.Data;

namespace AdminConsole.WebApi.IntegrationTests
{
	public class TournamentControllerTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
	{
		private static readonly Guid FrenchOpenId = new("00000000-0000-0000-0000-000000000009");
		private static readonly Guid StuttgartOpenId = new("00000000-0000-0000-0000-000000000006");

		private readonly WebApplicationFactory<Program> _factory;

		public TournamentControllerTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		public ValueTask InitializeAsync() => ValueTask.CompletedTask;

		public async ValueTask DisposeAsync()
		{
			using var scope = _factory.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TourManagementDbContext>();
			await dbContext.Database.ExecuteSqlRawAsync(
				"DELETE FROM Tournaments WHERE Title LIKE '%Integration Test%'");
		}

		[Fact]
		public async Task GetAllTournaments_ReturnsListOfAllActiveAndInactiveTournaments()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheRequestWasSuccessful(response);
			var tournaments = await GetTournamentsFromResponse(response);
			TheCorrectNumberofActiveAndInactiveTournamentsAreReturned(tournaments);
		}

		[Fact]
		public async Task GetAllTournaments_TournamentsHaveCorrectSurfaceAndLevelCounts()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheRequestWasSuccessful(response);
			var tournaments = await GetTournamentsFromResponse(response);
			TheTournamentsReturnedHaveTheirLevelValueSet(tournaments);
			TheTournamentsReturnedHaveTheirSurfaceValueSet(tournaments);
		}

		[Fact]
		public async Task GetAllTournaments_ReturnsCorrectDetailsForATournament()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheRequestWasSuccessful(response);
			var frenchOpen = await GetTournamentFromResponseByTitle(response, "French Open");
			ValidateFrenchOpenHasTheCorrectValues(frenchOpen);
		}

		[Fact]
		public async Task GetSingleTournament_ReturnsCorrectDetailsOfRequestedTournament()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync($"/tournaments/{StuttgartOpenId}", TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheRequestWasSuccessful(response);
			TournamentDto? tournament = await GetSingleTournamentFromResponse(response);
			ValidateStuttgartOpenHasTheCorrectValues(tournament);
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
			await TheResponseIndicatesTheTournamentWasNotFound(response);
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
			TheResponseIndicatesTheRequestWasInvalid(response);
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
			var location = TheResponseIndicatesTheTournamentWasCreated(response);
			var created = await GetTournamentFromLocation(client, location);
			ValidateTournamentHasTheValuesOfTheAddedTournament(created, newTournament);

			// finally, clean up by deleting the tournament - this will have to be done with SQL
		}

		[Fact]
		public async Task UpdateTournament_AfterAdding_UpdatesAllFields()
		{
			// Arrange
			var client = _factory.CreateClient();
			var location = await AddATournament(client);
			UpdateTournamentDto updateTournament = CreateTournamentUpdate();

			// Act
			var updateResponse = await client.PutAsJsonAsync(location, updateTournament, TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheTournamentWasUpdated(updateResponse);
			var updated = await GetTournamentFromLocation(client, location);
			ValidateTournamentHasTheValuesOfTheUpdate(updated, updateTournament);

			// finally, clean up by deleting the tournament - this will have to be done with SQL
		}

		[Fact]
		public async Task SetTournamentActive_AfterAdding_CanBeDeactivated()
		{
			// Arrange
			var client = _factory.CreateClient();
			Uri location = await CreateATournamentAndEnsureItIsActive(client);

			// Act
			var response = await client.PatchAsJsonAsync($"{location}/active",
				new SetTournamentActiveDto { IsActive = false }, TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheTournamentWasUpdated(response);
			var updated = await GetTournamentFromLocation(client, location);
			Assert.False(updated.IsActive);
		}

		[Fact]
		public async Task SetTournamentActive_WhenInactive_CanBeActivated()
		{
			// Arrange
			var client = _factory.CreateClient();
			var location = await AddATournament(client);
			await DeactivateTournamentAndValidateItIsInactive(client, location);

			// Act
			var response = await client.PatchAsJsonAsync($"{location}/active",
				new SetTournamentActiveDto { IsActive = true }, TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheTournamentWasUpdated(response);
			var updated = await GetTournamentFromLocation(client, location);
			Assert.True(updated.IsActive);
		}

		[Theory]
		[InlineData("{\"isActive\": \"not-a-bool\"}")]
		[InlineData("not json")]
		public async Task SetTournamentActive_WithInvalidPayload_ReturnsBadRequest(string payload)
		{
			// Arrange
			var client = _factory.CreateClient();
			var location = await AddATournament(client);
			using var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

			// Act
			var response = await client.PatchAsync($"{location}/active", content, TestContext.Current.CancellationToken);

			// Assert
			TheResponseIndicatesTheRequestWasInvalid(response);
			var tournament = await GetTournamentFromLocation(client, location);
			Assert.True(tournament.IsActive);
		}

		[Fact]
		public async Task SetTournamentActive_ForTournamentThatDoesNotExist_ReturnsNotFound()
		{
			// Arrange
			var client = _factory.CreateClient();
			var nonExistentId = new Guid("00000000-0000-0000-0000-111111111111");

			// Act
			var response = await client.PatchAsJsonAsync($"/tournaments/{nonExistentId}/active",
				new SetTournamentActiveDto { IsActive = false }, TestContext.Current.CancellationToken);

			// Assert
			await TheResponseIndicatesTheTournamentWasNotFound(response);
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

		private static UpdateTournamentDto CreateTournamentUpdate()
		{
			return new UpdateTournamentDto
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
		}

		private static async Task<Uri> AddATournament(HttpClient client)
		{
			var response = await client.PostAsJsonAsync("/tournaments", CreateTournamentToAdd(), TestContext.Current.CancellationToken);
			return TheResponseIndicatesTheTournamentWasCreated(response);
		}

		private static async Task<TournamentDto> GetTournamentFromLocation(HttpClient client, Uri location)
		{
			var tournament = await client.GetFromJsonAsync<TournamentDto>(location, TestContext.Current.CancellationToken);
			Assert.NotNull(tournament);
			return tournament;
		}

		private static async Task<TournamentDto[]?> GetTournamentsFromResponse(HttpResponseMessage response)
		{
			var options = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);
			var tournaments = await response.Content.ReadFromJsonAsync<TournamentDto[]>(options, TestContext.Current.CancellationToken);
			return tournaments;
		}

		private static async Task<TournamentDto?> GetTournamentFromResponseByTitle(HttpResponseMessage response, string title)
		{
			var tournaments = await GetTournamentsFromResponse(response);
			Assert.NotNull(tournaments);
			var tournament = tournaments.FirstOrDefault(t => t.Title == title);
			return tournament;
		}

		private static async Task<TournamentDto?> GetSingleTournamentFromResponse(HttpResponseMessage response)
		{
			var options = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);
			var tournament = await response.Content.ReadFromJsonAsync<TournamentDto>(options, TestContext.Current.CancellationToken);
			return tournament;
		}

		private static async Task<Uri> CreateATournamentAndEnsureItIsActive(HttpClient client)
		{
			var location = await AddATournament(client);
			var added = await GetTournamentFromLocation(client, location);
			Assert.True(added.IsActive);
			return location;
		}

		private static async Task DeactivateTournamentAndValidateItIsInactive(HttpClient client, Uri location)
		{
			var deactivateResponse = await client.PatchAsJsonAsync($"{location}/active",
				new SetTournamentActiveDto { IsActive = false }, TestContext.Current.CancellationToken);
			TheResponseIndicatesTheTournamentWasUpdated(deactivateResponse);
			var deactivated = await GetTournamentFromLocation(client, location);
			Assert.False(deactivated.IsActive);
		}

		private static void TheResponseIndicatesTheRequestWasSuccessful(HttpResponseMessage response)
		{
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
		}

		private static async Task TheResponseIndicatesTheTournamentWasNotFound(HttpResponseMessage response)
		{
			Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

			var json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
			using var document = System.Text.Json.JsonDocument.Parse(json);
			var title = document.RootElement.GetProperty("title").GetString();
			var status = document.RootElement.GetProperty("status").GetInt32();
			Assert.Equal("Not Found", title);
			Assert.Equal(404, status);
		}

		private static void TheResponseIndicatesTheRequestWasInvalid(HttpResponseMessage response)
		{
			Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
		}

		private static Uri TheResponseIndicatesTheTournamentWasCreated(HttpResponseMessage response)
		{
			Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
			Assert.NotNull(response.Headers.Location);
			return response.Headers.Location;
		}

		private static void TheResponseIndicatesTheTournamentWasUpdated(HttpResponseMessage response)
		{
			Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
		}

		private static void ValidateTournamentHasTheValuesOfTheAddedTournament(TournamentDto tournament, AddTournamentDto added)
		{
			Assert.Equal(added.Title, tournament.Title);
			Assert.Equal(added.Location, tournament.Location);
			Assert.Equal(added.Level, tournament.Level);
			Assert.Equal(added.Surface, tournament.Surface);
		}

		private static void ValidateTournamentHasTheValuesOfTheUpdate(TournamentDto tournament, UpdateTournamentDto update)
		{
			Assert.Equal(update.Title, tournament.Title);
			Assert.Equal(update.Location, tournament.Location);
			Assert.Equal(update.Level, tournament.Level);
			Assert.Equal(update.Surface, tournament.Surface);
			Assert.Equal(update.MensSingles, tournament.MensSingles);
			Assert.Equal(update.WomensSingles, tournament.WomensSingles);
			Assert.Equal(update.MensDoubles, tournament.MensDoubles);
			Assert.Equal(update.WomensDoubles, tournament.WomensDoubles);
			Assert.Equal(update.MixedDoubles, tournament.MixedDoubles);
		}

		private static void TheCorrectNumberofActiveAndInactiveTournamentsAreReturned(TournamentDto[]? tournaments)
		{
			Assert.NotNull(tournaments);
			Assert.Equal(10, tournaments.Length);
			Assert.Equal(8, tournaments.Count(t => t.IsActive));
			Assert.Equal(2, tournaments.Count(t => !t.IsActive));
		}

		private static void TheTournamentsReturnedHaveTheirSurfaceValueSet(TournamentDto[]? tournaments)
		{
			Assert.NotNull(tournaments);
			Assert.Equal(2, tournaments.Count(t => t.Surface == Surface.Grass));
			Assert.Equal(3, tournaments.Count(t => t.Surface == Surface.Clay));
			Assert.Equal(4, tournaments.Count(t => t.Surface == Surface.Hard));
			Assert.Equal(1, tournaments.Count(t => t.Surface == Surface.Carpet));
		}

		private static void TheTournamentsReturnedHaveTheirLevelValueSet(TournamentDto[]? tournaments)
		{
			Assert.NotNull(tournaments);

			Assert.Equal(4, tournaments.Count(t => t.Level == TournamentLevel.GsmSlam));
			Assert.Equal(3, tournaments.Count(t => t.Level == TournamentLevel.Gsm1000));
			Assert.Equal(1, tournaments.Count(t => t.Level == TournamentLevel.Gsm500));
			Assert.Equal(1, tournaments.Count(t => t.Level == TournamentLevel.Gsm250));
			Assert.Equal(1, tournaments.Count(t => t.Level == TournamentLevel.Gsm125));
		}

		private static void ValidateFrenchOpenHasTheCorrectValues(TournamentDto? frenchOpen)
		{
			Assert.NotNull(frenchOpen);

			ValidatePropertiesOfATournament(frenchOpen, FrenchOpenId, "French Open", "Paris, France",
				TournamentLevel.GsmSlam, Surface.Clay, true, true, true, true, true, true);
		}

		private static void ValidateStuttgartOpenHasTheCorrectValues(TournamentDto? stuttgartOpen)
		{
			Assert.NotNull(stuttgartOpen);

			ValidatePropertiesOfATournament(stuttgartOpen, StuttgartOpenId, "Stuttgart Open",
				"Stuttgart, Germany", TournamentLevel.Gsm250, Surface.Grass, false, true, false,
				true, false, false);
		}

		private static void ValidatePropertiesOfATournament(TournamentDto tournament, Guid tournamentId, 
			string title, string location, TournamentLevel level, Surface surface, bool mensSingles,
			bool womensSingles, bool mensDoubles, bool womensDoubles, bool mixedDoubles, bool isActive)
		{
			Assert.Equal(tournamentId, tournament.TournamentId);
			Assert.Equal(title, tournament.Title);
			Assert.Equal(location, tournament.Location);
			Assert.Equal(level, tournament.Level);
			Assert.Equal(surface, tournament.Surface);
			Assert.Equal(mensSingles, tournament.MensSingles);
			Assert.Equal(womensSingles, tournament.WomensSingles);
			Assert.Equal(mensDoubles, tournament.MensDoubles);
			Assert.Equal(womensDoubles, tournament.WomensDoubles);
			Assert.Equal(mixedDoubles, tournament.MixedDoubles);
			Assert.Equal(isActive, tournament.IsActive);
		}
	}
}
