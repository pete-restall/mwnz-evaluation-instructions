using FluentAssertions;
using MiddlewareNz.EvaluationApi.Companies;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Unit.Companies;

public class XmlBackendCompanyToCompanyMapperTest
{
	[Fact]
	public void MapFrom_CalledWithNullUnmapped_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var mapper = new XmlBackendCompanyToCompanyMapper();
		mapper
			.Invoking(x => x.MapFrom(null!))
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("unmapped");
	}

	[Fact]
	public void MapFrom_Called_ExpectIdIsMapped() =>
		MapFrom_Called_ExpectMapped((unmapped, mapped) => mapped.Id.Should().Be(unmapped.Id));

	private static void MapFrom_Called_ExpectMapped(Action<XmlBackendCompany, Company> assertion)
	{
		var unmapped = XmlBackendCompanyTestDoubles.StubValid();
		var mapper = new XmlBackendCompanyToCompanyMapper();
		assertion(unmapped, mapper.MapFrom(unmapped));
	}

	[Fact]
	public void MapFrom_Called_ExpectNameIsMapped() =>
		MapFrom_Called_ExpectMapped((unmapped, mapped) => mapped.Name.Should().Be(unmapped.Name.Trim()));

	[Fact]
	public void MapFrom_Called_ExpectDescriptionIsMapped() =>
		MapFrom_Called_ExpectMapped((unmapped, mapped) => mapped.Description.Should().Be(unmapped.Description.Trim()));
}
