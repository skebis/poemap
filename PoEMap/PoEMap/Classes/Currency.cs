using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoEMap.Classes
{
    /// <summary>
    /// Currency-structure to store the currency and the number of the used currency. Basically the price.
    /// </summary>
    public class Currency
    {
        public string PriceString { get; set; }

        /// <summary>
        /// Constructor for empty currency-object.
        /// </summary>
        public Currency()
        {
            // Nothing needed here.
        }

        /// <summary>
        /// Constructor for currency / price of the item.
        /// </summary>
        /// <param name="price">Usually the price of the item. Example: "~b/o 2 chaos" or "~price 1 alt". If it's just a note or there is nothing, mark price as "undefined".</param>
        public Currency(string price)
        {
            if (price.Equals("Undefined")) PriceString = price;
            else
            {
                string validPrice = CreateValidPrice(price);
                PriceString = validPrice;
            }
        }
        
        /// <summary>
        /// Modifies the price-note to a better format. Example: "~b/o 50 chaos" becomes "50 Chaos Orb".
        /// </summary>
        /// <param name="price">Price in note-format.</param>
        /// <returns>Price in modified format.</returns>
        public string CreateValidPrice(string price)
        {
            try
            {
                // Gets rid of "~b/o" or "~price" part of the price.
                string clearPrice = price.Substring(price.IndexOf(' ') + 1);
                // Creates a string array which holds the number at [0] and the used currency item at [1].
                string[] numberAndPrice = clearPrice.Split(' ');

                string number = numberAndPrice[0];
                string orb = numberAndPrice[1];
                // TODO: All possible cases AND TESTING!
                switch (numberAndPrice[1])
                {
                    case "chaos":
                        orb = "Chaos Orb";
                        break;

                    case "jew":
                        orb = "Jeweller Orb";
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
