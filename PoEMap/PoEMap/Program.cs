using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoEMap.Classes;

namespace PoEMap
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Maplist mapList = new Maplist();

            //mapList = ReadFile.ReadMapsFromFile();

            ApiFetching.ApiFetch(mapList);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
