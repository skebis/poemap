using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PoEMap.Classes;
using System;

namespace PoEMap
{
    /// <summary>
    /// Automatically generated class to start the program. Ensures that the database is created.
    /// Creates a scope to access services (mainly the database).
    /// 
    /// Author: Emil Keränen
    /// 14.4.2019
    /// </summary>
    public class Program
    {

        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<StashContext>();
                    context.Database.EnsureCreated();
                    ApiFetching.ApiFetch(services);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                host.Run();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
            }
}
