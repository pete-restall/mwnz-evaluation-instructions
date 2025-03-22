using FluentAssertions;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public class ErrorResponseTest
{
	[Fact]
	public void Constructor_CalledWithNullError_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var constructor = () => CreateErrorResponseWithError(null!);
		constructor
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("error");
	}

	private static ErrorResponse CreateErrorResponseWithError(string error) => new(error, AnyErrorDescription());

	private static string AnyErrorDescription() => StringGenerator.AnyNonNullNonWhitespaceNonEmpty();

	[Fact]
	public void Constructor_CalledWithEmptyError_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateErrorResponseWithError("");
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("error");
	}

	[Fact]
	public void Constructor_CalledWithWhitespaceError_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateErrorResponseWithError(StringGenerator.AnyWhitespace());
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("error");
	}

	[Fact]
	public void Error_Get_ExpectTrimmedValuePassedToConstructor()
	{
		var trimmedError = AnyError().Trim();
		var response = CreateErrorResponseWithError(trimmedError.WrapInWhitespace());
		response.Error.Should().Be(trimmedError);
	}

	private static string AnyError() => StringGenerator.AnyNonNullNonWhitespaceNonEmpty();

	[Fact]
	public void Constructor_CalledWithNullErrorDescription_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var constructor = () => CreateErrorResponseWithErrorDescription(null!);
		constructor
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("errorDescription");
	}

	private static ErrorResponse CreateErrorResponseWithErrorDescription(string description) => new(AnyError(), description);

	[Fact]
	public void Constructor_CalledWithEmptyErrorDescription_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateErrorResponseWithErrorDescription("");
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("errorDescription");
	}

	[Fact]
	public void Constructor_CalledWithWhitespaceErrorDescription_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateErrorResponseWithErrorDescription(StringGenerator.AnyWhitespace());
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("errorDescription");
	}

	[Fact]
	public void ErrorDescription_Get_ExpectTrimmedValuePassedToConstructor()
	{
		var trimmedErrorDescription = AnyErrorDescription().Trim();
		var response = CreateErrorResponseWithErrorDescription(trimmedErrorDescription.WrapInWhitespace());
		response.ErrorDescription.Should().Be(trimmedErrorDescription);
	}
}
