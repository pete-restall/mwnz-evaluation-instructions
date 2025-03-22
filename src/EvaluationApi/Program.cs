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
		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		return builder;
	}

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
