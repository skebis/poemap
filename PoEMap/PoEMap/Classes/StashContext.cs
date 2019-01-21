using Newtonsoft.Json.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace PoEMap.Classes
{
    /// <summary>
    /// Maplist-class with all stashes that contain maps.
    /// </summary>

    public class StashContext : DbContext
    {
        // Testing DB!
        public DbSet<Stash> StashesDb { get; set; }
        public DbSet<Map> MapsDb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=maps.db");
        }
        // Testing DB ends!

        /// <summary>
        /// Searches if the currently processed stash is already in the database and uses the stash from the database if it exists. Otherwise creates a new stash-object.
        /// </summary>
        /// <param name="jsonStash">Currently processed stash from the json.</param>
        /// <returns>Stash-object to work with.</returns>
        public Stash StashToUse(JObject jsonStash)
        {
            string currentStashId = (string)jsonStash.SelectToken("id");

            // Searches if the stash exists in the database and uses it. Otherwise creates a new Stash object.
            Stash stashFromDb = StashesDb.Find(currentStashId);

            if (stashFromDb == null)
            {
                return CreateNewStash(jsonStash);
            }
            else return stashFromDb;
        }

        /// <summary>
        /// Creates a new stash-object and initializes it with an id and owner.
        /// </summary>
        /// <param name="jsonStash">Currently processed stash.</param>
        /// <returns>New stash object.</returns>
        public Stash CreateNewStash(JObject jsonStash)
        {
            Stash newStash = new Stash(
                (string)jsonStash.SelectToken("id"),
                (string)jsonStash.SelectToken("lastCharacterName"),
                (string)jsonStash.SelectToken("stash"));

            return newStash;
        }

        /// <summary>
        /// Stores all map-items from API to stash-objects adds them to stashes-list.
        /// </summary>
        /// <param name="jsoncontent">Json which is stored in JArray -type.</param>
        public async void StoreMaps(JArray jsonStashes)
        {
            try
            {
                Stash currentStash;
                foreach (JObject jsonStash in jsonStashes)
                {
                    currentStash = StashToUse(jsonStash);

                    JArray itemsArray = (JArray)jsonStash.SelectToken("items");

                    // If the json-stash is empty, there is no need to keep record of it until it appears again.
                    if (itemsArray == null || itemsArray.Count == 0)
                    {
                        Remove(currentStash);
                        continue;
                    }
                    else
                    {
                        // Empty the current stash and update it with the items from the JSON.
                        currentStash.Empty();
                        foreach (JObject item in itemsArray)
                        {
                            JObject category = item.Value<JObject>("category");
                            if (category.ContainsKey("maps"))
                            {
                                currentStash.AddMap(item);
                            }
                        }
                    }
                    // If the stash didn't contain any map-items, it can be deleted.
                    if (currentStash.MapCount() == 0 || currentStash.Maps == null)
                    {
                        Remove(currentStash);
                    }
                    else await AddAsync(currentStash);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
