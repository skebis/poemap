using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoEMap
{
    /// <summary>
    /// Asyncronously fetches stash tab API data from the site and creates a JObject out of it.
    /// </summary>
    public class ApiFetching
    {
        private static Uri baseAddress = new Uri("http://www.pathofexile.com/api/public-stash-tabs");
        private static Uri testAddress = new Uri("http://www.pathofexile.com/api/public-stash-tabs?id=278632021-289226584-272524755-312580195-294901203");
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly int timeDelay = 1000;
        private static bool fetching = true;
        private static string nextId;

        public static async void ApiFetch()
        {
            while (fetching)
            {
                Task<string> taskJsonContent = _httpClient.GetStringAsync(testAddress.OriginalString);
                string stringJsonContent = await taskJsonContent;
                JObject jsonContent = JObject.Parse(stringJsonContent);

                Console.WriteLine("voitto");

                await Task.Delay(timeDelay);
            }
        }

    }
}
