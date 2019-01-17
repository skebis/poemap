using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PoEMap.Classes;

namespace PoEMap
{
    public class Program
    {

        public static void Main(string[] args)
        {
            /*var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }*/
            StashContext stashContext = new StashContext();
            //mapList = ReadFile.ReadMapsFromFile();
            ApiFetching.ApiFetch(stashContext);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
