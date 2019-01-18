using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PoEMap.Classes
{
    /// <summary>
    /// Maplist-class with all stashes that contain maps.
    /// </summary>
    
    public class StashContext : DbContext
    {
        // Testing DB!
        public StashContext(DbContextOptions<StashContext> options)
        : base(options)
        { }

        public DbSet<Stash> StashesDb { get; set; }
        public DbSet<Map> MapsDb { get; set; }
        // Testing DB ends!
        
        // shouldn't be needed anymore
        public ICollection<Stash> Stashes { get; set; }

        /// <summary>
        /// Constructor for Maplist.
        /// </summary>
        public StashContext()
        {
            // Nothing needed here.
        }
        
        /// <summary>
        /// Searches if the currently processed stash is already in the stash-list and uses the stash from the list if it exists. Otherwise creates a new stash-object.
        /// </summary>
        /// <param name="jsonStash">Currently processed stash.</param>
        /// <returns>Current stash-object to work with.</returns>
        public Stash StashToUse(JObject jsonStash)
        {
            // Check if the stashes-list is not null or empty so we can iterate through it.
            if (Stashes != null && Stashes.Count != 0)
            {
                // Find the same stash (with the same id and owner) from the stashes-list.

                /*var queryStash = from stash in stashes
                    where stash.StashId == (string)jsonStash.SelectToken("id")
                    select stash;*/

                foreach (Stash stash in Stashes)
                {
                    if (stash.HasSameId(jsonStash))
                    {
                        return stash;
                    }
                }
            }

            // If the stash wasn't found, return a new stash-object.
            return CreateNewStash(jsonStash);
        }

        /// <summary>
        /// Creates a new stash-object and initializes it with an id and owner.
        /// </summary>
        /// <param name="jsonStash">Currently processed stash.</param>
        /// <returns>New stash object.</returns>
        public Stash CreateNewStash(JObject jsonStash)
        {
            Stash newStash = new Stash(
                (int)jsonStash.SelectToken("id"),
                (string)jsonStash.SelectToken("lastCharacterName"),
                (string)jsonStash.SelectToken("stash"));
            Stashes.Add(newStash);

            return newStash;
        }

        /// <summary>
        /// Stores all map-items from API to stash-objects adds them to stashes-list.
        /// </summary>
        /// <param name="jsoncontent">Json which is stored in JArray -type.</param>
        public async void StoreMaps(JObject jsonStash)
        {
            try
            {
                Stash currentStash = new Stash();

                currentStash = StashToUse(jsonStash);

                JArray itemsArray = (JArray)jsonStash.SelectToken("items");

                // If the stash is empty, there is no need to keep record of it until it appears again.
                if (itemsArray == null || itemsArray.Count == 0)
                {
                    Stashes.Remove(currentStash);
                    return;
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
                if (currentStash.Maps == null || currentStash.Maps.Count == 0)
                {
                    Stashes.Remove(currentStash);
                }
                // Start serializing the new stash-object and add it to json file.
                // TODO: best method to object -> json and append new jobject to existing json.
                /*JObject currentStashJOb = (JObject)JToken.FromObject(currentStash);

                using (StreamWriter sw = new StreamWriter("stashes.json"))
                {
                    using (JsonTextWriter writer = new JsonTextWriter(sw))
                    {
                        await currentStashJOb.WriteToAsync(writer);
                    }
                }*/
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
