using RestEase;

namespace MiddlewareNz.EvaluationApi.Companies;

public interface IGetCompanyByIdBackendApi
{
	[Get("{id}.xml")]
	Task<Company> GetCompanyById(int id);
}
