using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using MiddlewareNz.EvaluationApi.Companies;

namespace MiddlewareNz.EvaluationApi.Tests.Unit.Companies;

public static class CompanyTestDoubles
{
	public static Company Dummy() => new(AnyId(), AnyName(), AnyDescription());

	private static int AnyId() => IntegerGenerator.GreaterThan(0);

	private static string AnyName() => StringGenerator.AnyNonNullNonWhitespaceNonEmpty();

	private static string AnyDescription() => StringGenerator.AnyNonNullNonWhitespaceNonEmpty();
}
