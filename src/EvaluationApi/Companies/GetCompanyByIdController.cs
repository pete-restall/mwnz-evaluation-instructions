using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestEase;

namespace MiddlewareNz.EvaluationApi.Companies;

[ApiController]
public class GetCompanyByIdController : ControllerBase
{
	private readonly IGetCompanyByIdBackendApi backendApi;

	public GetCompanyByIdController(IGetCompanyByIdBackendApi backendApi)
	{
		this.backendApi = backendApi ?? throw new ArgumentNullException(nameof(backendApi));
	}

	[HttpGet("companies/{id:int}")]
	public async Task<IActionResult> GetCompanyById(int id)
	{
		try
		{
			var company = await this.backendApi.GetCompanyById(id);
			return this.Ok(company);
		}
		catch (Exception exception)
		{
			var statusCode = exception is ApiException { StatusCode: HttpStatusCode.NotFound }
				? HttpStatusCode.NotFound
				: HttpStatusCode.InternalServerError;

			return this.StatusCode(
				(int) statusCode,
				new ErrorResponse(
					exception.GetType().FullName!,
					exception.Message));
		}
	}
}
