using Newtonsoft.Json.Linq;
using PoEMap.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoEMap
{
    /// <summary>
    /// Asyncronously fetches stash tab API data from the site and creates a dynamic (Json) object out of it.
    /// </summary>
    public class ApiFetching
    {
        private static readonly string baseAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        private static string nextAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly int timeDelay = 1000;
        private static bool fetching = true;
        private static string nextId;

        /// <summary>
        /// Asyncronously fetches data from the API.
        /// </summary>
        /// <param name="maplist">Main list of all maps.</param>
        public static async void ApiFetch(Maplist maplist)
        {
            while (fetching)
            {
                try
                {
                    Task<string> taskJsonContent = httpClient.GetStringAsync(nextAddress);
                    string stringJsonContent = await taskJsonContent;
                    dynamic jsonContent = JObject.Parse(stringJsonContent);
                    maplist.StoreMaps(jsonContent);
                    nextId = jsonContent.next_change_id;
                    nextAddress = baseAddress + "?id=" + nextId;
                } catch (Exception e) { Console.WriteLine(e.Message); }
                await Task.Delay(timeDelay);
            }
        }
    }
}
