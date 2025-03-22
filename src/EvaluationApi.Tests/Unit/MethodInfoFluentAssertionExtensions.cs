using System.Reflection;
using FluentAssertions.Execution;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public static class MethodInfoFluentAssertionExtensions
{
	public static MethodInfoFluentAssertions Should(this MethodInfo method) => new(method, AssertionChain.GetOrCreate());
}
