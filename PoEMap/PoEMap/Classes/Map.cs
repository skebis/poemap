using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoEMap.Classes
{
    /// <summary>
    /// Map-structure to store important information about map-items.
    /// </summary>
    public class Map
    {
        private string ItemId { get; set; }
        private string Seller { get; set; }
        private string MapName { get; set; }
        private Currency Price { get; set; }
        private Uri IconAddress { get; set; }
    }
}
