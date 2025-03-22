using FluentAssertions;
using Xunit;

namespace MiddlewareNz.EvaluationApi.Tests.Unit;

public class ProgramTest
{
	[Fact]
	public void CreateAppBuilder_CalledWithNullArgs_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		FluentActions
			.Invoking(() => Program.CreateAppBuilder(null!))
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("args");
	}

	[Fact]
	public void ConfigureApp_CalledWithNullApp_ExpectArgumentNullExceptionWithCorrectParamName()
	{
		FluentActions
			.Invoking(() => Program.ConfigureApp(null!))
			.Should().Throw<ArgumentNullException>()
			.WithParameterName("app");
	}
}
