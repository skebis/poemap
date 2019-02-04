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
        [BindProperty(SupportsGet = true)]
        public string LeagueString { get; set; }
        public SelectList Leagues { get; set; }
        //public SelectList MapTypes { get; set; }

        public int DefaultAmount = 50;

        public void OnGet()
        {
            IQueryable<string> leaguesQuery = from m in Context.Maps
                                              orderby m.League
                                              select m.League;

            MapsList = Context.Maps.Include(x => x.Stash);

            if (!string.IsNullOrEmpty(SearchString))
            {
                MapsList = MapsList.Where(s => s.MapName.ToUpper().Contains(SearchString.ToUpper()));
            }
            if (!string.IsNullOrEmpty(LeagueString))
            {
                MapsList = MapsList.Where(x => x.League == LeagueString);
            }

            Leagues = new SelectList(leaguesQuery.Distinct().ToList());

            MapsDisplayed = MapsList.Take(DefaultAmount);
        }
    }
}
