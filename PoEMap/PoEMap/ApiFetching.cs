using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoEMap.Classes;
using System;
using System.IO;
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
        /// <param name="stashContext">Main list of all maps.</param>
        public static async void ApiFetch(StashContext stashContext)
        {
            // Let our web service start before we start fetching data.
            await Task.Delay(timeDelay * 10);
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
                    /*using (Stream s = httpClient.GetStreamAsync(nextAddress).Result)
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            using (JsonReader jreader = new JsonTextReader(sr))
                            {
                                //JsonSerializer serializer = new JsonSerializer();
                                while (jreader.Read())
                                {
                                    if (jreader.Value != null)
                                    {
                                        Console.WriteLine("Token: {0}, Value: {1}", jreader.TokenType, jreader.Value);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Token: {0}", jreader.TokenType);
                                    }*/

                    //JObject jsonContent = JObject.Parse(nextContent);
                    //JObject jsonContent = (JObject)serializer.Deserialize(jreader);
                    string stringJsonContent = await httpClient.GetStringAsync(nextAddress);

                    JObject jsonContent = JObject.Parse(stringJsonContent);

                    JArray jsonStashes = (JArray)jsonContent.SelectToken("stashes");
                    foreach (JObject jsonStash in jsonStashes)
                    {
                        stashContext.StoreMaps(jsonStash);
                    }
                    //}
                    // Get the next id from the current API and set it to next address.
                    nextId = (string)jsonContent.SelectToken("next_change_id");
                    SetNextAddress();
                    //}
                    //}
                    //}
                }
                catch (Exception e)
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
