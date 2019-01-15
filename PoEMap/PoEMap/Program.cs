using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PoEMap.Classes;

namespace PoEMap
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Maplist maplist = new Maplist();
            //mapList = ReadFile.ReadMapsFromFile();
            ApiFetching.ApiFetch(maplist);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
