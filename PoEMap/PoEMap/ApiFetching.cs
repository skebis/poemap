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
        // Set data fetching to be always on.
        private static bool fetching = true;
        private static string nextId;

        /// <summary>
        /// Asyncronously fetches data from the API.
        /// </summary>
        /// <param name="maplist">Main list of all maps.</param>
        public static async void ApiFetch(Maplist maplist)
        {
            // Testing
            nextId = "283667914-294606306-277613284-318429055-300566765"; 
            // ReadFile.ReadNextIdFromFile();

            SetNextAddress();

            while (fetching)
            {
                // Print the current ID for debugging and testing.
                Console.WriteLine("Started parsing API with id " + nextId);
                try
                {
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
                    // Make sure to not continue making too frequent requests.
                    string errMsg = e.Message;
                    switch (errMsg)
                    {
                        // Maybe regex to check only "429".
                        case "Response status code does not indicate success: 429 (Too Many Requests).":
                            await Task.Delay(timeDelay * 20);
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
