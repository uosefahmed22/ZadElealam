
using ZadElealam.Apis.Extentions;

namespace ZadElealam.Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Split service configuration into extension methods
            builder.Services.ConfigureServices(builder.Configuration);



            var app = builder.Build();

            // Split middleware configuration into extension methods
            app.ConfigureMiddleware();

            app.Run();
        }
    }
}
