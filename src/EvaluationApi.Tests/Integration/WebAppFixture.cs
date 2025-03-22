using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace MiddlewareNz.EvaluationApi.Tests.Integration;

public class WebAppFixture : IAsyncDisposable
{
	private const string AppsettingsFilename = "appsettings.IntegrationTests.json";
	private static int nextAvailablePortOffset;

	private readonly WebApplicationBuilder appBuilder;
	private readonly Lazy<(WebApplication app, Task task)> running;

	public WebAppFixture()
	{
		this.appBuilder = Program.CreateAppBuilder();
		this.appBuilder.Configuration.AddJsonFile(AppsettingsFilename, optional: false, reloadOnChange: true);
		this.running = new Lazy<(WebApplication, Task)>(() =>
		{
			var app = this.appBuilder.Build();
			try
			{
				Program.ConfigureApp(app);
				app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()?.Addresses.Clear();

				var port = FirstAvailablePortFor(app) + Interlocked.Increment(ref nextAvailablePortOffset);
				return (app, app.RunAsync("https://localhost:" + port));
			}
			catch
			{
				app.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
				throw;
			}
		});
	}

	private static int FirstAvailablePortFor(WebApplication app) => int.Parse(
		app.Configuration["WebAppFixtureFirstAvailablePort"]
		?? throw new InvalidOperationException("Missing integration test appsetting; name=WebAppFixtureFirstAvailablePort, filename=" + AppsettingsFilename));

	public T RestClientFor<T>() => RestClient.For<T>(this.App.Urls.Single());

	private WebApplication App => this.running.Value.app;

	public ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		return this.running.IsValueCreated
			? this.running.Value.app.DisposeAsync()
			: ValueTask.CompletedTask;
	}
}
