namespace MiddlewareNz.EvaluationApi.Companies;

public class Company
{
	public Company(int id, string name, string description)
	{
		this.Id = id > 0 ? id : throw new ArgumentOutOfRangeException(nameof(id), id, "Company ID must be a positive integer");

		this.Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
		if (this.Name == "")
			throw new ArgumentException("Company Name must be specified", nameof(name));

		this.Description = description?.Trim() ?? throw new ArgumentNullException(nameof(description));
		if (this.Description == "")
			throw new ArgumentException("Company Description must be specified", nameof(description));
	}

	public int Id { get; }

	public string Name { get; }

	public string Description { get; }
}
