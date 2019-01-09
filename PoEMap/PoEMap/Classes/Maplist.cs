using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace PoEMap.Classes
{
    /// <summary>
    /// Maplist-class which holds the list of all maps and any sub-lists for future searching-function.
    /// </summary>
    
    public class Maplist
    {
        // List of maps that fulfill the conditions in user-made search.
        private List<Map> searchedMaps = new List<Map>();

        // List of all stashes.
        private List<Stash> stashes = new List<Stash>();

        /// <summary>
        /// Constructor for Maplist.
        /// </summary>
        public Maplist()
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
            if (stashes != null && stashes.Count != 0)
            {
                // Find the same stash (with the same id and owner) from the stashes-list.
                foreach (Stash stash in stashes)
                {
                    if ((string)jsonStash.SelectToken("id") == stash.StashId)
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
                (string)jsonStash.SelectToken("id"),
                (string)jsonStash.SelectToken("lastCharacterName"),
                (string)jsonStash.SelectToken("stash"));
            stashes.Add(newStash);

            return newStash;
        }

        /// <summary>
        /// Stores all map-items from API to stash-objects adds them to stashes-list.
        /// </summary>
        /// <param name="jsoncontent">Json which is stored in JArray -type.</param>
        public void StoreMaps(JArray jsonStashes)
        {
            try
            {
                // Testing sql connection!
                /*string conString = "Server=tcp:poemapsql.database.windows.net,1433;Initial Catalog=PoEMapDatabase;Persist Security Info=False;" +
                    "Integrated Security=True;MultipleActiveResultSets=False;" +
                    "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection sqlCon = new SqlConnection(conString))
                {
                    sqlCon.Open();
                    Console.WriteLine("Connection opened!");
                }
                Console.WriteLine("Conncetion should be closed!");*/

                foreach (JObject jsonStash in jsonStashes) {

                    Stash currentStash = new Stash();
                    currentStash = StashToUse(jsonStash);

                    JArray itemsArray = (JArray)jsonStash.SelectToken("items");

                    // If the stash is empty, there is no need to keep record of it until it appears again.
                    if (itemsArray == null || itemsArray.Count == 0)
                    {
                        stashes.Remove(currentStash);
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
                    if (currentStash.Maps == null || currentStash.Maps.Count == 0)
                    {
                        stashes.Remove(currentStash);
                    }
                }

            } catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
