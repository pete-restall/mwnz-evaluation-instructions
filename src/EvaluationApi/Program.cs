using System.Xml.Serialization;
using MiddlewareNz.EvaluationApi.Companies;
using RestEase;

namespace MiddlewareNz.EvaluationApi;

public static class Program
{
	public static void Main(string[] args)
	{
		using var app = CreateAppBuilder(args).Build();
		ConfigureApp(app);
		app.Run();
	}

	public static WebApplicationBuilder CreateAppBuilder(params string[] args)
	{
		if (args is null)
			throw new ArgumentNullException(nameof(args));

		var builder = WebApplication.CreateBuilder(args);
		builder.Services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(cfg => cfg.SupportNonNullableReferenceTypes());

		builder.Services
			.AddSingleton<IMap<XmlBackendCompany, Company>, XmlBackendCompanyToCompanyMapper>()
			.AddSingleton(ctx => new XmlResponseDeserialiser<XmlBackendCompany, Company>(
				new XmlSerializer(typeof(XmlBackendCompany)),
				ctx.GetRequiredService<IMap<XmlBackendCompany, Company>>()))
			.AddSingleton(CreateXmlRestClientFor<IGetCompanyByIdBackendApi, XmlResponseDeserialiser<XmlBackendCompany, Company>>);

		return builder;
	}

	private static T CreateXmlRestClientFor<T, TXmlDeserialiser>(IServiceProvider services) where TXmlDeserialiser : ResponseDeserializer =>
		new RestClient(services.Appsetting<string>("BackendApiBaseUrl"))
		{
			ResponseDeserializer = services.GetRequiredService<TXmlDeserialiser>()
		}.For<T>();

	private static T? Appsetting<T>(this IServiceProvider services, string name) =>
		services.GetRequiredService<IConfiguration>().GetValue<T>(name);

	public static void ConfigureApp(WebApplication app)
	{
		if (app is null)
			throw new ArgumentNullException(nameof(app));

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers();
	}
}
