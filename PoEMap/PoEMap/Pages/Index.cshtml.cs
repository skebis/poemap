using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PoEMap.Classes;

namespace PoEMap.Pages
{
    public class IndexModel : PageModel
    {

        private readonly StashContext _context;

        public IndexModel(StashContext context)
        {
            _context = context;
        }

        public IList<Map> MapsList { get; set; }
        public IEnumerable<Map> MapsDisplayed { get; set; }
        public string CurrentFilter { get; set; }
        public SelectList MapTypes { get; set; }
        public int DefaultAmount = 50;

        public async Task OnGetAsync(string searchString)
        {
            var maps = from m in _context.Maps
                       select m;

            CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                maps = maps.Where(s => s.MapName.Contains(searchString));
            }

            MapsList = maps.ToList();
            MapsDisplayed = MapsList.Take(DefaultAmount);
        }
    }
}
