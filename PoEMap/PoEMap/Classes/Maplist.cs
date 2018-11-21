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

        /// <summary>
        /// Constructor for Maplist.
        /// </summary>
        public Maplist()
        {
            // Nothing needed here.
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
                                Map newMap = new Map();
                                newMap.ItemId = (string)item.SelectToken("id");
                                newMap.Seller = (string)stash.SelectToken("lastCharacterName");
                                newMap.MapName = (string)item.SelectToken("typeLine");
                                if (item.ContainsKey("note"))
                                {
                                    string price = (string)item.SelectToken("note");
                                    newMap.Price = new Currency(price);
                                }
                                else newMap.Price = new Currency("No price");
                                maps.Add(newMap);
                            }
                        }
                    }
                }
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
