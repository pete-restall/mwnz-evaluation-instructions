using MiddlewareNz.EvaluationApi.Companies;
using RestEase;

namespace MiddlewareNz.EvaluationApi.Tests.Integration;

public interface IGetCompanyByIdApi
{
    [Get("companies/{id}")]
    Task<Company> GetCompanyById([Path("id")] int id);
}
