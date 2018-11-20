using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoEMap.Classes
{
    /// <summary>
    /// Maplist-class which holds the list of all maps and any sub-lists.
    /// </summary>
    public class Maplist
    {
        private List<Map> maps = new List<Map>();

        /// <summary>
        /// Constructor for Maplist.
        /// </summary>
        public Maplist()
        {
            //
        }

        /// <summary>
        /// Stores all map-items from api to a Maplist.
        /// </summary>
        /// <param name="jsoncontent">Json which is stored in dynamic -type.</param>
        public void StoreMaps(dynamic jsonStashes)
        {
            try
            {
                foreach (dynamic stash in jsonStashes) {

                    JArray itemsArray = stash.items;
                    if (itemsArray == null || itemsArray.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        /// Needs testing!
                        foreach (dynamic item in itemsArray)
                        {
                            if (item.category == "maps")
                            {
                                Map newMap = new Map();
                                newMap.ItemId = item.id;
                                newMap.Seller = stash.accountName;
                                newMap.MapName = item.typeLine;
                                if (item.note != null)
                                {
                                    string price = item.note;
                                    newMap.Price.ParsePrice(price);
                                }
                                else newMap.Price = new Currency();
                                maps.Add(newMap);
                            }
                        }
                    }
                }
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
