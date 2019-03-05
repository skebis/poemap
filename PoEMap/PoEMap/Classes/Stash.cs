using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoEMap.Classes
{
    /// <summary>
    /// Stash-object for storing maps and keeping track of stash owner and stash name.
    /// </summary>
    public class Stash
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string StashId { get; set; }

        public string Seller { get; set; }
        public string StashName { get; set; }

        // Relation to maps-list (one-to-many).
        public List<Map> Maps { get; set; }

        /// <summary>
        /// Constructor for empty stash-object.
        /// </summary>
        public Stash()
        {
            // Nothing needed here.
        }

        /// <summary>
        /// Constructor for Stash-object with three parameters (stash id, owner and stash name). Also initiates the maps-list.
        /// </summary>
        /// <param name="id">Stash id.</param>
        /// <param name="lastCharName">Owner or seller.</param>
        /// <param name="stashName">Stash name.</param>
        public Stash(string id, string lastCharName, string stashName)
        {
            StashId = id;
            Seller = lastCharName;
            StashName = stashName;
            Maps = new List<Map>();
        }

        /// <summary>
        /// Returns the map count of currently processed stash.
        /// </summary>
        /// <returns>Map count.</returns>
        public int MapCount()
        {
            return Maps.Count;
        }

        /// <summary>
        /// Empties the stash.
        /// </summary>
        public void Empty()
        {
            Maps.Clear();
        }

        /// <summary>
        /// Adds the new map to the list.
        /// </summary>
        /// <param name="item">Current map item.</param>
        public void AddMap(JObject item)
        {
            Map newMap = new Map(
                (string)item.SelectToken("id"),
                (string)item.SelectToken("typeLine"),
                (string)item.SelectToken("league"));

            string price = "";
            
            if (item.ContainsKey("note"))
            {
                price = (string)item.SelectToken("note");
                newMap.Note = price;
            }

            if (price.StartsWith('~'))
            {
                newMap.SetPrice(price);
            }
            else if (StashName != null && StashName.Length != 0 && StashName.StartsWith("~"))
            {
                newMap.SetPrice(StashName);
            }
            else newMap.SetNoPrice();

            Maps.Add(newMap);
        }
    }
}
