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
    /// Asyncronously fetches stash tab API (json) data from the site and creates a JObject out of it.
    /// </summary>
    public class ApiFetching
    {
        private static readonly string baseAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        private static string nextAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        // Base delay between every webrequest (1 second).
        private static readonly int timeDelay = 1000;
        // Set data fetching to always on.
        private static bool fetching = true;
        private static string nextId;

        /// <summary>
        /// Asyncronously fetches data from the API.
        /// </summary>
        /// <param name="maplist">Main list of all maps.</param>
        public static async void ApiFetch(Maplist maplist)
        {
            nextId = ReadFile.ReadNextIdFromFile();
            SetNextAddress();
            while (fetching)
            {
                // Print the current ID for debugging and testing.
                Console.WriteLine("Started parsing API with id " + nextId);
                try
                {
                    HttpClient httpClient = new HttpClient();
                    Task<string> taskJsonContent = httpClient.GetStringAsync(nextAddress);

                    string stringJsonContent = await taskJsonContent;
                    JObject jsonContent = JObject.Parse(stringJsonContent);

                    JArray jsonStashes = (JArray)jsonContent.SelectToken("stashes");
                    maplist.StoreMaps(jsonStashes);

                    // Get the next id from the current API and set it to next address.
                    nextId = (string)jsonContent.SelectToken("next_change_id");
                    SetNextAddress();
                } catch (Exception e)
                {
                    // Make sure not to continue making too frequent requests.
                    string errMsg = e.Message;
                    switch (errMsg)
                    {
                        // TODO: Regex to check only "429".
                        case "Response status code does not indicate success: 429 (Too Many Requests).":
                            await Task.Delay(timeDelay * 30);
                            break;

                        default:
                            break;
                    }
                    Console.WriteLine(e.Message);
                }

                // Wait 1 second before making another request to avoid errors.
                await Task.Delay(timeDelay);
            }
        }

        public static void SetNextAddress()
        {
            nextAddress = baseAddress + "?id=" + nextId;
        }
    }
}
