using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoEMap.Classes
{
    /// <summary>
    /// Map-structure to store important information about map-items.
    /// </summary>
    public class Map
    {
        public string StashId { get; set; }
        public string ItemId { get; set; }
        public string Seller { get; set; }
        public string MapName { get; set; }
        public Currency Price { get; set; }
        public Uri IconAddress { get; set; }
        public bool NeedToRemove { get; set; }

        public Map(string stashid, string itemid, string seller, string mapname)
        {
            StashId = stashid;
            ItemId = itemid;
            Seller = seller;
            MapName = mapname;
        }
    }
}
