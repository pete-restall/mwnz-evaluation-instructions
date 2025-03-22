using System.Net;
using FluentAssertions;
using Lophtware.Testing.Utilities;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using Microsoft.AspNetCore.Mvc;
using MiddlewareNz.EvaluationApi.Companies;
using RestEase;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Unit.Companies;

public class GetCompanyByIdControllerTest
{
	[Fact]
	public void Class_GetMetadata_ExpectDecoratedWithApiControllerAttribute()
	{
		typeof(GetCompanyByIdController).Should().BeDecoratedWith<ApiControllerAttribute>();
	}

	[Fact]
	public void Constructor_CalledWithNullBackendApi_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var constructor = () => new GetCompanyByIdController(null!);
		constructor.Should().Throw<ArgumentNullException>().WithParameterName("backendApi");
	}

	[Fact]
	public void GetCompanyById_GetRouteMetadata_ExpectHttpGetWithPathAndId()
	{
		Info.OfMethod<GetCompanyByIdController>(x => x.GetCompanyById(0)).Should().BeHttpGetFor("companies/{id:int}");
	}

	[Fact]
	public async Task GetCompanyById_CalledWhenBackendApiReturnsCompany_ExpectSuccessWithSameCompanyAsBody()
	{
		var id = AnyCompanyId();
		var company = CompanyTestDoubles.Dummy();
		var backendApi = GetCompanyByIdBackendApiTestDoubles.StubFor(id, company);
		var controller = new GetCompanyByIdController(backendApi);
		var response = await controller.GetCompanyById(id);
		response.Should().BeHttpOk().And.HaveHttpBodySameAs(company);
	}

	private static int AnyCompanyId() => IntegerGenerator.Any();

	[Fact]
	public async Task GetCompanyById_CalledWhenBackendApiThrowsSynchronousException_ExpectInternalServerErrorWithErrorResponseContainingExceptionTypeAndMessage()
	{
		var exception = StubException();
		var backendApi = GetCompanyByIdBackendApiTestDoubles.StubForSynchronousThrow(exception);
		var controller = new GetCompanyByIdController(backendApi);
		var response = await controller.GetCompanyById(AnyCompanyId());
		response.Should().BeHttpInternalServerError().And.HaveHttpBodyEquivalentTo(
			new ErrorResponse(
				exception.GetType().ToString(),
				exception.Message));
	}

	private static Exception StubException() => new(StringGenerator.AnyNonNullNonWhitespaceNonEmpty());

	[Fact]
	public async Task GetCompanyById_CalledWhenBackendApiThrowsAsynchronousException_ExpectInternalServerErrorWithErrorResponseContainingExceptionTypeAndMessage()
	{
		var exception = StubException();
		var backendApi = GetCompanyByIdBackendApiTestDoubles.StubForAsynchronousThrow(exception);
		var controller = new GetCompanyByIdController(backendApi);
		var response = await controller.GetCompanyById(AnyCompanyId());
		response.Should().BeHttpInternalServerError().And.HaveHttpBodyEquivalentTo(
			new ErrorResponse(
				exception.GetType().ToString(),
				exception.Message));
	}

	[Fact]
	public async Task GetCompanyById_CalledWhenBackendApiThrowsExceptionIndicatingNotFound_ExpectNotFoundWithErrorResponseBodyContainingExceptionTypeAndMessage()
	{
		var notFoundException = StubNotFoundException();
		var backendApi = GetCompanyByIdBackendApiTestDoubles.StubForAsynchronousThrow(notFoundException);
		var controller = new GetCompanyByIdController(backendApi);
		var response = await controller.GetCompanyById(AnyCompanyId());
		response.Should().BeNotFound().And.HaveHttpBodyEquivalentTo(
			new ErrorResponse(
				notFoundException.GetType().ToString(),
				notFoundException.Message));
	}

	private static Exception StubNotFoundException() => StubApiExceptionWith(HttpStatusCode.NotFound);

	private static ApiException StubApiExceptionWith(HttpStatusCode statusCode) => new(
		HttpMethod.Get,
		requestUri: null,
		statusCode,
		reasonPhrase: null,
		headers: null!,
		contentHeaders: null,
		contentString: null);

	[Fact]
	public async Task GetCompanyById_CalledWhenBackendApiThrowsExceptionNotIndicatingNotFound_ExpectInternalServerErrorWithErrorResponseBodyContainingExceptionTypeAndMessage()
	{
		var exception = StubApiExceptionWith(EnumGenerator.AnyExcept(HttpStatusCode.NotFound));
		var backendApi = GetCompanyByIdBackendApiTestDoubles.StubForAsynchronousThrow(exception);
		var controller = new GetCompanyByIdController(backendApi);
		var response = await controller.GetCompanyById(AnyCompanyId());
		response.Should().BeHttpInternalServerError().And.HaveHttpBodyEquivalentTo(
			new ErrorResponse(
				exception.GetType().ToString(),
				exception.Message));
	}
}
