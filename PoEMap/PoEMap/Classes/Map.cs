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
        public string ItemId { get; set; }
        public string MapName { get; set; }
        public Currency Price { get; set; }
        public Uri IconAddress { get; set; }

        /// <summary>
        /// Constructor for map-item.
        /// </summary>
        /// <param name="itemid">Map-item id.</param>
        /// <param name="mapname">Name of the map.</param>
        public Map (string itemid, string mapname)
        {
            ItemId = itemid;
            MapName = mapname;
        }
    }
}
