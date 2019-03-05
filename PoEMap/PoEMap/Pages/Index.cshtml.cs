using System.Collections.Generic;
using System.Linq;
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

        // The whole list of maps.
        public IQueryable<Map> MapsList { get; set; }
        public IEnumerable<Map> MapsDisplayed { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string LeagueString { get; set; }

        public SelectList Leagues { get; set; }
        //public SelectList MapTypes { get; set; }

        // How many maps are displayed for the user.
        public int DefaultAmount = 50;

        public void OnGet()
        {
            // Query for all possible leagues.
            IQueryable<string> leaguesQuery = from m in Context.Maps
                                              orderby m.League
                                              select m.League;

            // Load related data (Stash <-> Maps <-> Currency) so we can get the seller from Stash-object and price from Currency-object.
            MapsList = Context.Maps
                .Include(x => x.Stash)
                .Include(c => c.Price);

            // Filters search to only show maps from selected league.
            if (!string.IsNullOrEmpty(LeagueString))
            {
                MapsList = MapsList.Where(x => x.League.Equals(LeagueString));
            }

            // Search-function, ignores lowercase / uppercase characters when the search is done (toUpper() might be pretty expensive here).
            if (!string.IsNullOrEmpty(SearchString))
            {
                MapsList = MapsList.Where(s => s.MapName.ToUpper().Contains(SearchString.ToUpper()));
            }

            // Orders the list from cheapest map to most expensive map.
            //MapsList = MapsList.OrderBy(c => c.Price.PriceDouble * SetRatio(c.Price.Orb));

            // Shows all possible leagues in a selectlist.
            Leagues = new SelectList(leaguesQuery.Distinct().ToList());

            MapsDisplayed = MapsList.Take(DefaultAmount);
        }

        /// <summary>
        /// Gives the ratio compared to Chaos Orbs. Hand-picked from poe.ninja so every ratio is an estimate.
        /// </summary>
        /// <param name="orb">Orb to compare.</param>
        /// <returns>Ratio to Chaos Orbs</returns>
        public double SetRatio(string orb)
        {
            return ApiFetching.currencyRatios[orb];
        }
    }
}
