using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using PoEMap.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoEMap
{
    /// <summary>
    /// Static class that asyncronously fetches stash tab API (json) data from the site and creates a JObject out of it.
    /// Opens an SQLite connection for storing the stashes and the maps.
    /// </summary>
    public class ApiFetching
    {
        // Dictionary-structure to hold correct currency-representations.
        public static Dictionary<string, string> currencyNames;
        // Dictionary-structure to hold estimated currency ratios.
        public static Dictionary<string, double> currencyRatios;

        private static HttpClient httpClient = new HttpClient();
        private static readonly string baseAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        private static string nextAddress = "http://www.pathofexile.com/api/public-stash-tabs";
        // Base delay between every web request (1.5 seconds).
        private static readonly int timeDelay = 1500;
        // Initial delay when starting the application (15 seconds).
        private static readonly int initialDelay = 15000;
        // Delay if we got error for too many requests.
        private static readonly int floodDelay = 20000;

        private static bool fetching = false;

        private static string nextId;
        private static readonly string nextIdFile = "nextid.txt";

        /// <summary>
        /// Static method that asyncronously fetches data from the API and writes the next id to a file.
        /// </summary>
        public static async void ApiFetch(IServiceProvider serviceProvider)
        {
            InitializeDictionaries();

            // Let our web service start before we start fetching data.
            await Task.Delay(initialDelay);

            ReadNextIdFromFile();

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

                    using (var context = new StashContext(serviceProvider
                            .GetRequiredService<DbContextOptions<StashContext>>()))
                    {
                        context.StoreMaps(jsonStashes);
                        await context.SaveChangesAsync();
                    }

                    nextId = (string)jsonContent.SelectToken("next_change_id");

                    WriteNextIdToFile();
                    
                    SetNextAddress();
                }
                catch (Exception e)
                {
                    // If we somehow made too many requests, make sure to not continue it.
                    // Future TODO: site maintenance and other responses.
                    string errMsg = e.Message;
                    switch (errMsg)
                    {
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
        private static void SetNextAddress()
        {
            nextAddress = baseAddress + "?id=" + nextId;
        }

        /// <summary>
        /// Creates a file or overwrites an existing one and writes the next id to there. File is nextid.txt in the same directory.
        /// </summary>
        private static void WriteNextIdToFile()
        {
            if (!File.Exists(nextIdFile))
            {
                File.Create(nextIdFile);
            }
            else
            {
                File.WriteAllText(nextIdFile, nextId);
            }
        }

        /// <summary>
        /// Reads the next id from the file if the file exists. File is "nextid.txt" in the same directory.
        /// </summary>
        private static void ReadNextIdFromFile()
        {
            if (File.Exists(nextIdFile))
            {
                nextId = File.ReadAllText(nextIdFile);
            }
        }

        /// <summary>
        /// Initializes the static dictionaries, currencyNames and currencyRatios, with keys and values.
        /// Ratios are hand-picked and rough estimates.
        /// </summary>
        private static void InitializeDictionaries()
        {
            currencyNames = new Dictionary<string, string>
            {
                {"chaos", "Chaos Orb"},
                {"alch", "Orb of Alchemy"},
                {"chisel", "Cartographer's Chisel"},
                {"vaal", "Vaal Orb"},
                {"jew", "Jeweller's Orb"},
                {"fuse", "Orb of Fusing"},
                {"chance", "Orb of Chance"},
                {"scour", "Orb of Scouring"},
                {"alt", "Orb of Alteration"},
                {"regal", "Regal Orb"},
                {"chrom", "Chromatic Orb"},
                {"regret", "Orb of Regret"},
                {"blessed", "Blessed Orb"},
                {"exa", "Exalted Orb"},
                {"divine", "Divine Orb"},
                {"gcp", "Gemcutter's Prism"}
            };

            currencyRatios = new Dictionary<string, double>
            {
                {"Chaos Orb", 1},
                {"Orb of Alchemy", 0.5},
                {"Cartographer's Chisel", 0.5},
                {"Vaal Orb", 2},
                {"Jeweller's Orb", 0.125},
                {"Orb of Fusing", 0.5},
                {"Orb of Chance", 0.1},
                {"Orb of Scouring", 0.5},
                {"Orb of Alteration", 0.25},
                {"Regal Orb", 1},
                {"Chromatic Orb", 0.25},
                {"Orb of Regret", 1},
                {"Blessed Orb", 0.2},
                {"Exalted Orb", 170},
                {"Divine Orb", 20},
                {"Gemcutter's Prism", 1.5},
                {"Undefined", 999999} // TODO: Some other way to calculate undefined.
            };
        }
    }
}
