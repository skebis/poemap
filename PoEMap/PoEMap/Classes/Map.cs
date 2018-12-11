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
        public string Note { get; set; }
        public Currency Price { get; set; }
        public Uri IconAddress { get; set; }

        /// <summary>
        /// Constructor for empty map-object.
        /// </summary>
        public Map ()
        {
            // Nothing needed here.
        }

        /// <summary>
        /// Constructor for map-item.
        /// </summary>
        /// <param name="itemid">Map-item id.</param>
        /// <param name="mapname">Name of the map.</param>
        public Map (string itemid, string mapname, Currency defPrice)
        {
            ItemId = itemid;
            MapName = mapname;
            Price = defPrice;
        }
    }
}
