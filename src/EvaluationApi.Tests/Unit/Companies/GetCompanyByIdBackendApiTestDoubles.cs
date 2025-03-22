using MiddlewareNz.EvaluationApi.Companies;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace MiddlewareNz.EvaluationApi.Tests.Unit.Companies;

public static class GetCompanyByIdBackendApiTestDoubles
{
	public static IGetCompanyByIdBackendApi StubFor(int id, Company company)
	{
		var api = Substitute.For<IGetCompanyByIdBackendApi>();
		api.GetCompanyById(id).Returns(Task.FromResult(company));
		return api;
	}

	public static IGetCompanyByIdBackendApi StubForSynchronousThrow(Exception exception)
	{
		var api = Substitute.For<IGetCompanyByIdBackendApi>();
		api.GetCompanyById(Arg.Any<int>()).Throws(exception);
		return api;
	}

	public static IGetCompanyByIdBackendApi StubForAsynchronousThrow(Exception exception)
	{
		var api = Substitute.For<IGetCompanyByIdBackendApi>();
		api.GetCompanyById(Arg.Any<int>()).ThrowsAsync(exception);
		return api;
	}
}
