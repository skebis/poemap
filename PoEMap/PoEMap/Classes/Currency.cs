using System;

namespace PoEMap.Classes
{
    /// <summary>
    /// Currency-structure to return valid price for map.
    /// </summary>
    public static class Currency
    {

        /// <summary>
        /// Sets the price to "undefined".
        /// </summary>
        /// <return>Returns undefined.</return>
        public static string SetNoPrice()
        {
            return "Undefined";
        }

        /// <summary>
        /// Setter for price.
        /// </summary>
        /// <param name="price">Usually the price of the item. Example: "~b/o 2 chaos" or "~price 1 alt".</param>
        /// <returns>Returns valid price. Example: "2 Chaos Orb".</returns>
        public static string SetPrice(string price)
        {
            return CreateValidPrice(price);
        }

        /// <summary>
        /// Modifies the price-note to a better format. Example: "~b/o 50 chaos" becomes "50 Chaos Orb".
        /// </summary>
        /// <param name="price">Price in note-format.</param>
        /// <returns>Price in modified format.</returns>
        public static string CreateValidPrice(string price)
        {
            try
            {
                // Gets rid of "~b/o " or "~price " part of the price.
                string clearPrice = price.Substring(price.IndexOf(' ') + 1);
                // Creates a string array which holds the number at [0] and the used currency item at [1].
                string[] numberAndPrice = clearPrice.Split(' ');
                if (numberAndPrice.Length < 2)
                {
                    return "Undefined";
                }
                string number = numberAndPrice[0];
                string orb = numberAndPrice[1];
                // Tested in-game

                switch (numberAndPrice[1])
                {
                    case "chaos":
                        orb = "Chaos Orb";
                        break;

                    case "alch":
                        orb = "Orb of Alchemy";
                        break;

                    case "chisel":
                        orb = "Cartographer's Chisel";
                        break;

                    case "vaal":
                        orb = "Vaal Orb";
                        break;

                    case "jew":
                        orb = "Jeweller Orb";
                        break;

                    case "fuse":
                        orb = "Orb of Fusing";
                        break;

                    case "chance":
                        orb = "Orb of Chance";
                        break;

                    case "scour":
                        orb = "Orb of Scouring";
                        break;

                    case "alt":
                        orb = "Orb of Alteration";
                        break;

                    case "regal":
                        orb = "Regal Orb";
                        break;

                    case "chrom":
                        orb = "Chromatic Orb";
                        break;

                    case "regret":
                        orb = "Orb of Regret";
                        break;

                    case "blessed":
                        orb = "Blessed Orb";
                        break;

                    case "exa":
                        orb = "Exalted Orb";
                        break;

                    case "divine":
                        orb = "Divine Orb";
                        break;

                    case "gcp":
                        orb = "Gemcutter's Prism";
                        break;

                    default:
                        return "Undefined";
                }
                return number + " " + orb;
            }
            catch (Exception e)
            {
                // If something went wrong with the array and parsing, just return undefined.
                Console.WriteLine(e.Message);
                return "Undefined";
            }
        }
    }
}
