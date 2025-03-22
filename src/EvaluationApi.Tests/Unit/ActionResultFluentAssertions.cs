using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public class ActionResultFluentAssertions : ReferenceTypeAssertions<IActionResult, ActionResultFluentAssertions>
{
	public ActionResultFluentAssertions(IActionResult subject, AssertionChain assertionChain) : base(subject, assertionChain)
	{
	}

	protected override string Identifier => this.Subject?.ToString() ?? nameof(IActionResult);

	[CustomAssertion]
	public AndConstraint<ActionResultFluentAssertions> BeHttpOk(string because = "", params object[] becauseArgs) =>
		this.Be(HttpStatusCode.OK, because, becauseArgs);

	private AndConstraint<ActionResultFluentAssertions> Be(HttpStatusCode status, string because = "", params object[] becauseArgs)
	{
		this.CurrentAssertionChain
			.BecauseOf(because, becauseArgs)
			.Given(() => this.Subject as IStatusCodeActionResult)
			.ForCondition(response => response?.StatusCode == (int) status)
			.FailWith(
				"Expected {context:response} to have HTTP status of {1} {2}{reason}, but found {3}",
				_ => this.Identifier,
				_ => status.ToString(),
				_ => (int) status,
				response => response?.StatusCode);

		return new(this);
	}

	[CustomAssertion]
	public AndConstraint<ActionResultFluentAssertions> BeHttpInternalServerError(string because = "", params object[] becauseArgs) =>
		this.Be(HttpStatusCode.InternalServerError, because, becauseArgs);

	[CustomAssertion]
	public AndConstraint<ActionResultFluentAssertions> BeNotFound(string because = "", params object[] becauseArgs) =>
		this.Be(HttpStatusCode.NotFound, because, becauseArgs);

	[CustomAssertion]
	public AndConstraint<ActionResultFluentAssertions> HaveHttpBodySameAs<T>(T body, string because = "", params object[] becauseArgs)
	{
		this.CurrentAssertionChain
			.BecauseOf(because, becauseArgs)
			.Given(() => this.Subject as ObjectResult)
			.ForCondition(response => ReferenceEquals(response?.Value, body))
			.FailWith(
				"Expected {context:response} to have HTTP body of {1}{reason}, but found {2}",
				_ => this.Identifier,
				_ => body,
				response => response?.Value);

		return new(this);
	}

	[CustomAssertion]
	public AndConstraint<ActionResultFluentAssertions> HaveHttpBodyEquivalentTo<T>(T body, string because = "", params object[] becauseArgs)
	{
		this.CurrentAssertionChain
			.BecauseOf(because, becauseArgs)
			.Given(() => this.Subject as ObjectResult)
			.ForCondition(response => response is not null)
			.FailWith(
				"Expected {context:response} to have HTTP body of {1}{reason}, but found {2}",
				_ => this.Identifier,
				_ => body,
				response => response?.Value)
			.Then
			.Given(response => response?.Value.Should().BeEquivalentTo(body));

		return new(this);
	}
}
