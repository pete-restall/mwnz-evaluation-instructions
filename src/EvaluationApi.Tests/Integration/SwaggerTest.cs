using FluentAssertions;
using RestEase;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Integration;

[Collection(IntegrationTests.Collection)]
public class SwaggerTest : IClassFixture<WebAppFixture>
{
	private readonly WebAppFixture fixture;

	public SwaggerTest(WebAppFixture fixture)
	{
		this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
	}

	[Fact]
	public async Task ExpectSwaggerDescriptionIsAccessible()
	{
		var restClient = this.fixture.RestClientFor<ISwaggerApi>();
		var swagger = await restClient.GetSwaggerDescription();
		swagger.OpenApi.Should().MatchRegex(@"^(\d{1,5}\.){2}\d{1,5}$");
	}

	public interface ISwaggerApi
	{
		[Get("swagger/v1/swagger.json")]
		Task<SwaggerDescription> GetSwaggerDescription();
	}

	public record SwaggerDescription(string OpenApi);
}
