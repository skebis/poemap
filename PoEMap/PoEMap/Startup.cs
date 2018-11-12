using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PoEMap
{
    public class Startup
    {
        private Uri baseAddress = new Uri("http://www.pathofexile.com/api/public-stash-tabs?id=");
        private Uri startAddress = new Uri("http://www.pathofexile.com/api/public-stash-tabs?id=278632021-289226584-272524755-312580195-294901203");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            try
            {
                WebClient client = new WebClient();
                string jsonContent = client.DownloadString(startAddress.OriginalString);
                JObject tempObj = JObject.Parse(jsonContent);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
