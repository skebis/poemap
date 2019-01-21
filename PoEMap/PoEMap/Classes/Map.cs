using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoEMap.Classes
{
    /// <summary>
    /// Map-structure to store important information about map-items.
    /// </summary>
    public class Map
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MapId { get; set; }

        // Testing related data!!
        public string StashId { get; set; }
        public Stash Stash { get; set; }
        // Testing ends!!

        public string MapName { get; set; }
        public string Note { get; set; }
        public string Price { get; set; }
        public string League { get; set; }
        // icon maybe not needed.
        public string IconAddress { get; set; }

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
        /// <param name="defaultPrice">Price.</param>
        /// <param name="league">League where the item is.</param>
        public Map (string itemid, string mapname, string league)
        {
            MapId = itemid;
            MapName = mapname;
            League = league;
        }

        /// <summary>
        /// Sets the price of the map-item.
        /// </summary>
        /// <param name="price">Price.</param>
        public void SetPrice(string price)
        {
            Price = Currency.SetPrice(price);
        }

        /// <summary>
        /// Sets the price to "undefined".
        /// </summary>
        public void SetNoPrice()
        {
            Price = Currency.SetNoPrice();
        }
    }
}
