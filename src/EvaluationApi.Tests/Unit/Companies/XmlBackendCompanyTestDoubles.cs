using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using MiddlewareNz.EvaluationApi.Companies;

namespace MiddlewareNz.EvaluationApi.Tests.Unit.Companies;

public static class XmlBackendCompanyTestDoubles
{
	public static XmlBackendCompany StubValid()
	{
		var valid = CompanyTestDoubles.Stub();
		return new()
		{
			Id = valid.Id,
			Name = valid.Name.WrapInWhitespace(),
			Description = valid.Description.WrapInWhitespace()
		};
	}
}
