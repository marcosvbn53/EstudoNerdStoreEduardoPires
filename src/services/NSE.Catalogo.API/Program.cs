using NSE.Catalogo.API;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var startup = new Startup(builder.Environment);
        startup.ConfigureServices(builder.Services);

        WebApplication app = builder.Build();

        startup.Configure(app);

        app.Run();
    }
}