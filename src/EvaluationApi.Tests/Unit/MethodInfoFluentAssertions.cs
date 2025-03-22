using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public class MethodInfoFluentAssertions : ReferenceTypeAssertions<MethodInfo, MethodInfoFluentAssertions>
{
	public MethodInfoFluentAssertions(MethodInfo subject, AssertionChain assertionChain) : base(subject, assertionChain)
	{
	}

	protected override string Identifier => this.Subject?.ToString() ?? nameof(MethodInfo);

	[CustomAssertion]
	public AndConstraint<MethodInfoFluentAssertions> BeHttpGetFor(string template, string because = "", params object[] becauseArgs)
	{
		this.CurrentAssertionChain
			.BecauseOf(because, becauseArgs)
			.Given(this.Subject.GetCustomAttribute<HttpGetAttribute>)
			.ForCondition(attr => attr?.Template == template)
			.FailWith(
				"Expected {0} to be decorated with [HttpGet(\"{1}\")]{reason}, but found {2}",
				_ => this.Identifier,
				_ => template,
				attr => attr?.Template);

		return new(this);
	}
}
