using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
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
		public async Task GetAllTournaments_ReturnsSuccessStatusCode()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);

			// Assert
			response.EnsureSuccessStatusCode();
		}

		[Fact]
		public async Task GetAllTournaments_ReturnsTenTournaments()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

			// Assert
			using var document = System.Text.Json.JsonDocument.Parse(json);
			Assert.Equal(System.Text.Json.JsonValueKind.Array, document.RootElement.ValueKind);
			Assert.Equal(10, document.RootElement.GetArrayLength());
		}

		[Fact]
		public async Task GetAllTournaments_ContainsFrenchOpen()
		{
			// Arrange
			var client = _factory.CreateClient();
			var options = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);
			options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

			// Act
			var response = await client.GetAsync("/tournaments", TestContext.Current.CancellationToken);
			response.EnsureSuccessStatusCode();
			var tournaments = await response.Content.ReadFromJsonAsync<TournamentDto[]>(options, TestContext.Current.CancellationToken);

			// Assert
			Assert.NotNull(tournaments);
			Assert.Contains(tournaments, t => t.Title == "French Open");
		}
	}
}
