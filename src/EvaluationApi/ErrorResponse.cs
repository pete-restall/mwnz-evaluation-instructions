namespace MiddlewareNz.EvaluationApi;

public class ErrorResponse
{
	public ErrorResponse(string error, string errorDescription)
	{
		this.Error = error?.Trim() ?? throw new ArgumentNullException(nameof(error));
		if (this.Error == "")
			throw new ArgumentException("Error must be specified", nameof(error));

		this.ErrorDescription = errorDescription?.Trim() ?? throw new ArgumentNullException(nameof(errorDescription));
		if (this.ErrorDescription == "")
			throw new ArgumentException("Error Description must be specified", nameof(errorDescription));
	}

	public string Error { get; }

	public string ErrorDescription { get; }
}
