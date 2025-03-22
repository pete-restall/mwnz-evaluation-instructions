using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public static class ActionResultFluentAssertionExtensions
{
	public static ActionResultFluentAssertions Should(this IActionResult response) => new(response, AssertionChain.GetOrCreate());
}
