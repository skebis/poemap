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

        public void CheckDuplicates(JObject item, JObject stash)
        {
            foreach (Map map in maps)
            {
                // Check if an item is removed or the maplist contains a duplicate of the item.
            }
        }

        /// <summary>
        /// Stores all map-items from API to a Maplist.
        /// </summary>
        /// <param name="jsoncontent">Json which is stored in JArray -type.</param>
        public void StoreMaps(JArray jsonStashes)
        {
            try
            {
                foreach (JObject stash in jsonStashes) {

                    JArray itemsArray = (JArray)stash.SelectToken("items"); ;
                    if (itemsArray == null || itemsArray.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        // Needs testing!
                        foreach (JObject item in itemsArray)
                        {
                            JObject category = item.Value<JObject>("category");
                            if (category.ContainsKey("maps"))
                            {
                                CheckDuplicates(item, stash);
                                Map newMap = new Map
                                {
                                    ItemId = (string)item.SelectToken("id"),
                                    Seller = (string)stash.SelectToken("lastCharacterName"),
                                    MapName = (string)item.SelectToken("typeLine")
                                };
                                if (item.ContainsKey("note"))
                                {
                                    string price = (string)item.SelectToken("note");
                                    newMap.Price = price;
                                }
                                else newMap.Price = noPrice;

                                maps.Add(newMap);
                            }
                        }
                    }
                }
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
