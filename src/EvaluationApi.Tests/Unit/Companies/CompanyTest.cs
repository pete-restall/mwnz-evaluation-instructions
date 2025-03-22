using FluentAssertions;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using MiddlewareNz.EvaluationApi.Companies;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Unit.Companies;

public class CompanyTest
{
	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	[InlineData(-10)]
	[InlineData(-12387)]
	public void Constructor_CalledWithInvalidId_ExpectArgumentOutOfRangeExceptionWithCorrectParamNameAndInvalidValue(int invalidId)
	{
		var constructor = () => CreateCompanyWithId(invalidId);
		constructor
			.Should().Throw<ArgumentOutOfRangeException>()
			.WithParameterName("id")
			.And.ActualValue.Should().Be(invalidId);
	}

	private static Company CreateCompanyWithId(int id) => new(id, AnyName(), AnyDescription());

	private static string AnyName() => StringGenerator.AnyNonNullNonWhitespaceNonEmpty();

	private static string AnyDescription() => StringGenerator.AnyNonNullNonWhitespaceNonEmpty();

	[Fact]
	public void Id_Get_ExpectSameValuePassedToConstructor()
	{
		var id = AnyId();
		var company = CreateCompanyWithId(id);
		company.Id.Should().Be(id);
	}

	[Fact]
	public void Constructor_CalledWithNullName_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var constructor = () => CreateCompanyWithName(null!);
		constructor
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("name");
	}

	private static Company CreateCompanyWithName(string name) => new(AnyId(), name, AnyDescription());

	private static int AnyId() => IntegerGenerator.GreaterThan(0);

	[Fact]
	public void Constructor_CalledWithEmptyName_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateCompanyWithName("");
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("name");
	}

	[Fact]
	public void Constructor_CalledWithWhitespaceName_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateCompanyWithName(StringGenerator.AnyWhitespace());
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("name");
	}

	[Fact]
	public void Name_Get_ExpectTrimmedValuePassedToConstructor()
	{
		var trimmedName = AnyName().Trim();
		var company = CreateCompanyWithName(trimmedName.WrapInWhitespace());
		company.Name.Should().Be(trimmedName);
	}

	[Fact]
	public void Constructor_CalledWithNullDescription_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		var constructor = () => CreateCompanyWithDescription(null!);
		constructor
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("description");
	}

	private static Company CreateCompanyWithDescription(string description) => new(AnyId(), AnyName(), description);

	[Fact]
	public void Constructor_CalledWithEmptyDescription_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateCompanyWithDescription("");
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("description");
	}

	[Fact]
	public void Constructor_CalledWithWhitespaceDescription_ExpectArgumentExceptionWithCorrectParamName()
	{
		var constructor = () => CreateCompanyWithDescription(StringGenerator.AnyWhitespace());
		constructor
			.Should().Throw<ArgumentException>()
			.WithParameterName("description");
	}

	[Fact]
	public void Description_Get_ExpectTrimmedValuePassedToConstructor()
	{
		var trimmedDescription = AnyDescription().Trim();
		var company = CreateCompanyWithDescription(trimmedDescription.WrapInWhitespace());
		company.Description.Should().Be(trimmedDescription);
	}
}
