using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoEMap.Classes
{
    /// <summary>
    /// Map-object to store important information about map-items.
    /// </summary>
    public class Map
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MapId { get; set; }

        // Relation to stash-object.
        [Required]
        public string StashForeignKey { get; set; }
        public Stash Stash { get; set; }

        public string MapName { get; set; }
        public string Note { get; set; }
        public string League { get; set; }

        // Relation to currency-object (one-to-one).
        public Currency Price { get; set; }

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
        /// <param name="league">League where the item is.</param>
        public Map (string itemid, string mapname, string league)
        {
            MapId = itemid;
            MapName = mapname;
            League = league;
        }

        public void SetPrice(string price)
        {
            Price = new Currency(price);
        }

        public void SetNoPrice()
        {
            Price = new Currency();
        }
    }
}
