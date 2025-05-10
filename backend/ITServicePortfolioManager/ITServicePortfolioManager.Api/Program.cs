using ITServicePortfolioManager.Api.Extensions;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddApplicationServices();

        var app = builder.Build();

        app.UseApplicationMiddlewares();

        app.Run();
    }
}