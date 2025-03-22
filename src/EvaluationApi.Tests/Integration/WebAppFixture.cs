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
	private readonly Lazy<WebApplication> app;

	public WebAppFixture()
	{
		this.appBuilder = Program.CreateAppBuilder();
		this.appBuilder.Configuration.AddJsonFile(AppsettingsFilename, optional: false, reloadOnChange: true);
		this.app = new Lazy<WebApplication>(() =>
		{
			var builtApp = this.appBuilder.Build();
			try
			{
				Program.ConfigureApp(builtApp);

				var port = FirstAvailablePortFor(builtApp) + Interlocked.Increment(ref nextAvailablePortOffset);
				var addresses = builtApp.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()?.Addresses
					?? throw new InvalidOperationException("No IServerAddressFeature !");

				addresses.Clear();
				addresses.Add("https://localhost:" + port);

				builtApp.StartAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
				return builtApp;
			}
			catch
			{
				builtApp.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
				throw;
			}
		});
	}

	private static int FirstAvailablePortFor(WebApplication app) => int.Parse(
		app.Configuration["WebAppFixtureFirstAvailablePort"]
		?? throw new InvalidOperationException("Missing integration test appsetting; name=WebAppFixtureFirstAvailablePort, filename=" + AppsettingsFilename));

	public T RestClientFor<T>() => RestClient.For<T>(this.App.Urls.Single());

	private WebApplication App => this.app.Value;

	public ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		return this.app.IsValueCreated
			? this.app.Value.DisposeAsync()
			: ValueTask.CompletedTask;
	}
}
