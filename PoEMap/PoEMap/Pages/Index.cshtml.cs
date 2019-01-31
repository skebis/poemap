using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoEMap.Classes;

namespace PoEMap.Pages
{
    public class IndexModel : PageModel
    {

        private readonly StashContext Context;

        public IndexModel(StashContext context)
        {
            Context = context;
        }

        public IQueryable<Map> MapsList { get; set; }
        public IEnumerable<Map> MapsDisplayed { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        //public SelectList MapTypes { get; set; }

        public int DefaultAmount = 50;

        public void OnGet()
        {
            MapsList = Context.Maps.Include(x => x.Stash);

            if (!string.IsNullOrEmpty(SearchString))
            {
                MapsList = MapsList.Where(s => s.MapName.Contains(SearchString));
            }
            MapsDisplayed = MapsList.Take(DefaultAmount);
        }
    }
}
