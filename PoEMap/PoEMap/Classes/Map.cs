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

        // Relation to stash-object.
        public virtual Stash Stash { get; set; }

        public string MapName { get; set; }
        public string Note { get; set; }
        public double PriceDouble { get; set; }
        public string Orb { get; set; }

        public string League { get; set; }
        // icon maybe not needed.
        //public string IconAddress { get; set; }

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

        /// <summary>
        /// Sets the price of the map-item.
        /// </summary>
        /// <param name="price">Price.</param>
        public void SetPrice(string price)
        {
            CreateValidPrice(price);
        }

        /// <summary>
        /// Sets the price to "undefined".
        /// </summary>
        public void SetNoPrice()
        {
            PriceDouble = 0;
            Orb = "Undefined";
        }

        /// <summary>
        /// Extracts a valid price out of the items note. Example: "~b/o 50 chaos" becomes "50 Chaos Orb".
        /// </summary>
        /// <param name="price">Price in note-format.</param>
        public void CreateValidPrice(string price)
        {
            try
            {
                // Gets rid of "~b/o " or "~price " part of the price.
                string clearPrice = price.Substring(price.IndexOf(' ') + 1);
                // Creates a string array which holds the number at [0] and the used currency item at [1].
                string[] numberAndPrice = clearPrice.Split(' ');
                if (numberAndPrice.Length < 2)
                {
                    SetNoPrice();
                }
                string priceDouble = numberAndPrice[0];
                string orb = numberAndPrice[1];

                SetPriceDouble(priceDouble);
                SetOrb(orb);
            }
            catch (Exception e)
            {
                // If something went wrong with the array and parsing, just return undefined.
                Console.WriteLine(e.Message);
                SetNoPrice();
            }
        }

        /// <summary>
        /// Sets the numeral part of the price.
        /// </summary>
        /// <param name="priceDouble">The numeral part of the price.</param>
        public void SetPriceDouble(string priceDouble)
        {
            PriceDouble = double.Parse(priceDouble);
        }

        /// <summary>
        /// Sets the orb-part of the price.
        /// </summary>
        /// <param name="orb">Orb-part of the price.</param>
        public void SetOrb(string orb)
        {
            switch (orb)
            {
                case "chaos":
                    Orb = "Chaos Orb";
                    break;

                case "alch":
                    Orb = "Orb of Alchemy";
                    break;

                case "chisel":
                    Orb = "Cartographer's Chisel";
                    break;

                case "vaal":
                    Orb = "Vaal Orb";
                    break;

                case "jew":
                    Orb = "Jeweller Orb";
                    break;

                case "fuse":
                    Orb = "Orb of Fusing";
                    break;

                case "chance":
                    Orb = "Orb of Chance";
                    break;

                case "scour":
                    Orb = "Orb of Scouring";
                    break;

                case "alt":
                    Orb = "Orb of Alteration";
                    break;

                case "regal":
                    Orb = "Regal Orb";
                    break;

                case "chrom":
                    Orb = "Chromatic Orb";
                    break;

                case "regret":
                    Orb = "Orb of Regret";
                    break;

                case "blessed":
                    Orb = "Blessed Orb";
                    break;

                case "exa":
                    Orb = "Exalted Orb";
                    break;

                case "divine":
                    Orb = "Divine Orb";
                    break;

                case "gcp":
                    Orb = "Gemcutter's Prism";
                    break;

                default:
                    SetNoPrice();
                    break;
            }
        }
    }
}
