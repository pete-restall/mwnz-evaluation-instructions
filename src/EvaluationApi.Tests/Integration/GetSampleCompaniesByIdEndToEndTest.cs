using FluentAssertions;
using MiddlewareNz.EvaluationApi.Companies;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Integration;

[Collection(IntegrationTests.Collection)]
public class GetSampleCompaniesByIdEndToEndTest : IClassFixture<WebAppFixture>
{
	private class Samples : TheoryData<int, Company>
	{
		public Samples()
		{
			this.Add(1, new Company(id: 1, name: "MWNZ", description: "..is awesome"));
			this.Add(2, new Company(id: 2, name: "Other", description: "....is not"));
		}
	}

	private readonly WebAppFixture fixture;

	public GetSampleCompaniesByIdEndToEndTest(WebAppFixture fixture)
	{
		this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
	}

	[Theory]
	[ClassData(typeof(Samples))]
	public async Task ExpectSampleCompanyCanBeRetrievedSuccessfully(int id, Company sample)
	{
		var restClient = this.fixture.RestClientFor<IGetCompanyByIdApi>();
		var company = await restClient.GetCompanyById(id);
		company.Should().BeEquivalentTo(sample);
	}
}
