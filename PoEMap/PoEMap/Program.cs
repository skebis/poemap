using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PoEMap
{
    public class Program
    {

        public static void Main(string[] args)
        {
            ApiFetching.ApiFetch();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
