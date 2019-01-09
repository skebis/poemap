using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoEMap.Classes
{
    /// <summary>
    /// Stash-class for storing maps.
    /// </summary>
    public class Stash
    {
        public string StashId { get; set; }
        public string Seller { get; set; }
        public string StashName { get; set; }
        public List<Map> Maps = new List<Map>();

        /// <summary>
        /// Constructor for empty stash-object.
        /// </summary>
        public Stash()
        {
            // Nothing needed here.
        }

        /// <summary>
        /// Constructor for Stash-object with two parameters (stash id and owner).
        /// </summary>
        /// <param name="id">Stash id.</param>
        /// <param name="lastCharName">Owner or seller.</param>
        /// <param name="stashPrice"></param>
        public Stash(string id, string lastCharName, string stashName)
        {
            StashId = id;
            Seller = lastCharName;
            StashName = stashName;
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
                new Currency(),
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
