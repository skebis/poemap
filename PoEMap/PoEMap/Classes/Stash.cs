using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoEMap.Classes;

namespace PoEMap.Classes
{
    /// <summary>
    /// Stash-class for storing maps.
    /// </summary>
    public class Stash
    {
        public string StashId { get; set; }
        public string Seller { get; set; }
        public List<Map> Maps = new List<Map>();
        private readonly string noPrice = "No price";

        // Constructor for empty stash-object.
        public Stash()
        {
            // Nothing needed here.
        }

        /// <summary>
        /// Constructor for Stash-object with two parameters (stash id and owner).
        /// </summary>
        /// <param name="id">Stash id.</param>
        /// <param name="lastCharName">Owner or seller.</param>
        public Stash(string id, string lastCharName)
        {
            StashId = id;
            Seller = lastCharName;
        }

        /// <summary>
        /// Empties the stash.
        /// </summary>
        public void Empty()
        {
            Maps.Clear();
        }

        /// <summary>
        /// Adds the new map to the list (if conditions are met).
        /// </summary>
        /// <param name="item">Current map item.</param>
        public void AddMap(JObject item)
        {
            Map newMap = new Map(
                (string)item.SelectToken("id"),
                (string)item.SelectToken("typeLine"));

            if (item.ContainsKey("note"))
            {
                string price = (string)item.SelectToken("note");
                newMap.Price = new Currency(price);
            }
            else
            {
                newMap.Price = new Currency(noPrice);
            }

            Maps.Add(newMap);
        }

        /// <summary>
        /// Checks if the current map is already in the list and deletes the old map.
        /// </summary>
        /// <param name="stashid">Map-item stash id.</param>
        /// <param name="itemid">Map-item id.</param>
        /*public void CheckDuplicateAndDelete(string itemid)
        {
            foreach (Map map in maps)
            {
                if (map.StashId.Equals(stashid) && map.ItemId.Equals(itemid))
                {
                    Console.WriteLine("found duplicate, removing..");
                    maps.Remove(map);
                }
            }
        }*/
    }
}
