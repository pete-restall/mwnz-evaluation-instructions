using FluentAssertions;
using RestEase;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Integration;

[Collection(IntegrationTests.Collection)]
public class SnakeCaseJsonSerialisationTest : IClassFixture<WebAppFixture>
{
	private readonly WebAppFixture fixture;

	public SnakeCaseJsonSerialisationTest(WebAppFixture fixture)
	{
		this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
	}

	[Fact]
	public async Task ExpectErrorDescriptionPropertyNameIsSerialisedAsSnakeCase()
	{
		var restClient = this.fixture.RestClientFor<ICauseAnError>();
		var response = await restClient.GetErrorResponseAsString();
		response.StringContent.Should().MatchRegex("\"error_description\":\\s*\".+404.+\"");
	}

	public interface ICauseAnError
	{
		[AllowAnyStatusCode]
		[Get("companies/-42")]
		Task<Response<string>> GetErrorResponseAsString();
	}
}
