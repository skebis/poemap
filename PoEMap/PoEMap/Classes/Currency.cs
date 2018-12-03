using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoEMap.Classes
{
    /// <summary>
    /// Currency-structure to store the currency and the number of the used currency. Basically the price.
    /// </summary>
    public class Currency
    {
        public string PriceString { get; set; }

        public Currency()
        {
            // Nothing needed here.
        }

        /// <summary>
        /// Constructor for currency / price of the item.
        /// </summary>
        /// <param name="price">Price of the item. Example: "~b/o 2 chaos" or "~price 1 alt".</param>
        public Currency(string price)
        {
            PriceString = price;
            // TODO: Parsing the given string. Should set the price as "2 Chaos Orbs" for example.
        }
    }
}
