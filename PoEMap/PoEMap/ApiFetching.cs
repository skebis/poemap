using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using PoEMap.Classes;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoEMap
{
    /// <summary>
    /// Asyncronously fetches stash tab API (json) data from the site and creates a JObject out of it.
    /// </summary>
    public class ApiFetching
    {
        private static HttpClient httpClient = new HttpClient();
        private static readonly string baseAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        private static string nextAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        // Base delay between every web request (1.5 seconds).
        private static readonly int timeDelay = 1500;
        // Initial delay when starting the application (15 seconds).
        private static readonly int initialDelay = 15000;
        // Delay if we got error for too many requests.
        private static readonly int floodDelay = 20000;
        // Set data fetching to be always on.
        private static bool fetching = true;
        private static string nextId;

        /// <summary>
        /// Asyncronously fetches data from the API.
        /// </summary>
        public static async void ApiFetch()
        {
            // Let our web service start before we start fetching data.
            await Task.Delay(initialDelay);
            // Testing
            nextId = "328231349-339788986-320879142-367803389-347562285";
            // ReadFile.ReadNextIdFromFile();

            SetNextAddress();

            while (fetching)
            {
                // Print the current ID for debugging and testing.
                Console.WriteLine("Started parsing API with id " + nextId);

                try
                {
                    string stringJsonContent = await httpClient.GetStringAsync(nextAddress);

                    JObject jsonContent = JObject.Parse(stringJsonContent);

                    JArray jsonStashes = (JArray)jsonContent.SelectToken("stashes");

                    using (var db = new StashContext())
                    {
                        db.StoreMaps(jsonStashes);
                        db.SaveChanges();
                    }

                    // Get the next id from the current API and set it to next address.
                    nextId = (string)jsonContent.SelectToken("next_change_id");
                    SetNextAddress();
                }
                catch (Exception e)
                {
                    // If we somehow made too many requests, make sure to not continue it.
                    string errMsg = e.Message;
                    switch (errMsg)
                    {
                        // Maybe regex to check only "429".
                        case "Response status code does not indicate success: 429 (Too Many Requests).":
                            await Task.Delay(floodDelay);
                            break;

                        default:
                            break;
                    }
                    Console.WriteLine(e.Message);
                }

                // Wait a bit before making another request to avoid flooding / errors.
                await Task.Delay(timeDelay);
            }
        }

        /// <summary>
        /// Sets the next address for fetching data.
        /// </summary>
        public static void SetNextAddress()
        {
            nextAddress = baseAddress + "?id=" + nextId;
        }
    }
}
