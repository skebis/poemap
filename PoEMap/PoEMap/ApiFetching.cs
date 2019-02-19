﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using PoEMap.Classes;
using System;
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
        private static readonly string nextIdFile = "nextid.txt";

        /// <summary>
        /// Static method that asyncronously fetches data from the API and writes the next id to a file.
        /// </summary>
        public static async void ApiFetch(IServiceProvider serviceProvider)
        {
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

                    // Get the next id from the current API and set it to next address.
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
    }
}
