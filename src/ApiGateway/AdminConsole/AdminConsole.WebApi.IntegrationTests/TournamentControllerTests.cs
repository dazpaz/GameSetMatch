using Microsoft.AspNetCore.Mvc.Testing;

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
	}
}
