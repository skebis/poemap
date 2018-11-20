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
        public int Price { get; set; }
        public Orb CurrencyOrb { get; set; }

        public void ParsePrice(string note) {
            //
        }
    }
}
