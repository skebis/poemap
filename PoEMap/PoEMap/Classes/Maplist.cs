﻿using Newtonsoft.Json.Linq;
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
        public void StoreMaps(dynamic jsoncontent)
        {
            try
            {
                foreach (dynamic stash in jsoncontent["stashes"]) {

                    JArray itemsArray = stash.items;
                    if (itemsArray == null || itemsArray.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (dynamic item in itemsArray)
                        {
                            // Check that "category" = "maps" and store the map-item into map-structure.
                        }
                    }
                }
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
