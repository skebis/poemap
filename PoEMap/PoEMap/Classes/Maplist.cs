using Newtonsoft.Json.Linq;
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
        private List<Map> maps = new List<Map>();
        private readonly string noPrice = "No price";

        /// <summary>
        /// Constructor for Maplist.
        /// </summary>
        public Maplist()
        {
            // Nothing needed here.
        }

        /// <summary>
        /// Removes the maps that are marked to be removed. Marked maps are either duplicates or "ghostmaps"
        /// i.e. they have been removed from the stash.
        /// </summary>
        public void RemoveMaps()
        {
            for (int i = 0; i < maps.Count; i++)
            {
                if (maps[i].NeedToRemove)
                {
                    Console.WriteLine("found map to remove!");
                    maps.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Checks if the current map is already in the list and marks "ghostmaps" to be deleted.
        /// </summary>
        /// <param name="stashid">Stash id where the current map is.</param>
        /// <param name="itemid">Maps id.</param>
        /// <returns>True if the current map should be added to the maplist, false if not.</returns>
        public bool CheckDuplicatesAndDeleted(string stashid, string itemid)
        {
            foreach (Map map in maps)
            {
                if (map.StashId.Equals(stashid))
                {
                    if (map.ItemId.Equals(itemid))
                    {
                        map.NeedToRemove = false;
                        return false;
                    }
                    else
                    {
                        map.NeedToRemove = true;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Stores all map-items from API to a Maplist.
        /// </summary>
        /// <param name="jsoncontent">Json which is stored in JArray -type.</param>
        public void StoreMaps(JArray jsonStashes)
        {
            try
            {
                List<Map> mapsToAdd = new List<Map>();
                foreach (JObject stash in jsonStashes) {

                    JArray itemsArray = (JArray)stash.SelectToken("items"); ;
                    if (itemsArray == null || itemsArray.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (JObject item in itemsArray)
                        {
                            JObject category = item.Value<JObject>("category");
                            if (category.ContainsKey("maps"))
                            {
                                mapsToAdd = AddMap(item, stash, mapsToAdd);
                            }
                        }

                    }
                }
                if (maps != null || maps.Count > 0)
                {
                    RemoveMaps();
                }
                maps.AddRange(mapsToAdd);

            } catch (Exception e) { Console.WriteLine(e.Message); }
        }

        /// <summary>
        /// Adds the new map to the list (if conditions are met).
        /// </summary>
        /// <param name="item">Current map item.</param>
        /// <param name="stash">Current stash where the map is.</param>
        /// <param name="mapsToAdd">List of maps to be added after one API request.</param>
        /// <returns></returns>
        public List<Map> AddMap(JObject item, JObject stash, List<Map> mapsToAdd)
        {
            Map newMap = new Map(
                (string)stash.SelectToken("id"),
                (string)item.SelectToken("id"),
                (string)stash.SelectToken("lastCharacterName"),
                (string)item.SelectToken("typeLine"));

            if (CheckDuplicatesAndDeleted(newMap.StashId, newMap.ItemId))
            {
                if (item.ContainsKey("note"))
                {
                    string price = (string)item.SelectToken("note");
                    newMap.Price = new Currency(price);
                }
                else
                {
                    newMap.Price = new Currency(noPrice);
                }

                newMap.NeedToRemove = false;
                mapsToAdd.Add(newMap);
            }
            return mapsToAdd;
        }
    }
}
