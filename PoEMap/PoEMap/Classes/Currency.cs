using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoEMap.Classes
{
    public class Currency
    {
        public string CurrencyId { get; set; }

        // Relation to Map-object (one-to-one).
        [Required]
        public string MapForeignKey { get; set; }
        public Map Map { get; set; }

        public double PriceDouble { get; set; }
        public string Orb { get; set; }

        /// <summary>
        /// Constructor for currency-object. If no parameter is given, sets the price "Undefined".
        /// </summary>
        public Currency()
        {
            SetNoPrice();
        }

        /// <summary>
        /// Constructor for currency-object if parameter is given. Creates a valid price from the parameter.
        /// </summary>
        /// <param name="price">Price.</param>
        public Currency(string price)
        {
            SetPrice(price);
        }

        /// <summary>
        /// Returns correct representation of the Currency-object.
        /// </summary>
        /// <returns>Undefined, or price and the currency-item (example: 2 Orb of Alteration).</returns>
        public override string ToString()
        {
            if (Orb.Equals("Undefined"))
            {
                return Orb;
            }
            else return PriceDouble + " " + Orb;
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
        /// Sets the price to "Undefined".
        /// </summary>
        public void SetNoPrice()
        {
            PriceDouble = 1;
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
                // If the array is too small, the price wasn't set correctly by the player.
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
                // If something went wrong with the array and parsing, i.e. player set invalid price, just set the price as undefined.
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
            // Checks if the price is set as a fractional. Example: "~price 5/2 chaos"
            if (priceDouble.Contains('/'))
            {
                string[] pd = priceDouble.Split('/');
                double newPrice = Math.Round(double.Parse(pd[0]) / double.Parse(pd[1]), 2);
                PriceDouble = newPrice;
            }
            else PriceDouble = double.Parse(priceDouble);
        }

        /// <summary>
        /// Sets the orb-part of the price.
        /// </summary>
        /// <param name="orb">Orb-part of the price.</param>
        public void SetOrb(string orb)
        {
            Orb = ApiFetching.currencyNames[orb];
        }
    }
}
