using NSE.Bff.Compras;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var startup = new Startup(builder.Environment);

        startup.ConfigureService(builder.Services);

        var app = builder.Build();                

        startup.Configure(app);

        app.Run();
    }
}