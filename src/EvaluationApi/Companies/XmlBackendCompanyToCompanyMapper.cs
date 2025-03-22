namespace MiddlewareNz.EvaluationApi.Companies;

public class XmlBackendCompanyToCompanyMapper : IMap<XmlBackendCompany, Company>
{
	public Company MapFrom(XmlBackendCompany unmapped)
	{
		if (unmapped is null)
			throw new ArgumentNullException(nameof(unmapped));

		return new(unmapped.Id, unmapped.Name, unmapped.Description);
	}
}
