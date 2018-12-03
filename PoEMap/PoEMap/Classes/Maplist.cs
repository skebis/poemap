﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Check if the stashes-list is empty. If it is, a new stash-object should be created.
            if (stashes == null || stashes.Count == 0)
            {
                return CreateNewStash(jsonStash);
            }

            // Otherwise find the same stash (with the same id and owner) from the stashes-list.
            foreach (Stash stash in stashes)
            {
                // Needs testing!
                if ((string)jsonStash.SelectToken("id") == stash.StashId)
                {
                    // Testing
                    if (stash.Seller == "testtradeboi")
                    {
                        Console.WriteLine("loytyi!");
                    }

                    return stash;
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
                (string)jsonStash.SelectToken("lastCharacterName"));
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
