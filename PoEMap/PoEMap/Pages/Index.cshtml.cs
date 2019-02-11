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

        // How many maps are displayed for the user.
        public int DefaultAmount = 50;

        public void OnGet()
        {
            // Query for all possible leagues.
            IQueryable<string> leaguesQuery = from m in Context.Maps
                                              orderby m.League
                                              select m.League;

            // Load related data (Stash -> Maps -> Stash) so we can get the seller from Stash-object.
            MapsList = Context.Maps.Include(x => x.Stash);

            // Search-function, ignores lower / upper characters when the search is done.
            if (!string.IsNullOrEmpty(SearchString))
            {
                MapsList = MapsList.Where(s => s.MapName.ToUpper().Contains(SearchString.ToUpper()));
            }
            // Filters search to only show maps from selected league.
            if (!string.IsNullOrEmpty(LeagueString))
            {
                MapsList = MapsList.Where(x => x.League == LeagueString);
            }

            MapsList = MapsList.OrderBy(c => c.PriceDouble * SetRatio(c.Orb));

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
            switch (orb)
            {
                case "Chaos Orb":
                    return 1;

                case "Orb of Alchemy":
                    return 0.5;

                case "Cartographer's Chisel":
                    return 0.5;

                case "Vaal Orb":
                    return 2;

                case "Jeweller Orb":
                    return 0.125;

                case "Orb of Fusing":
                    return 0.5;

                case "Orb of Chance":
                    return 0.1;

                case "Orb of Scouring":
                    return 0.5;

                case "Orb of Alteration":
                    return 0.25;

                case "Regal Orb":
                    return 1;

                case "Chromatic Orb":
                    return 0.25;

                case "Orb of Regret":
                    return 1;

                case "Blessed Orb":
                    return 0.2;

                case "Exalted Orb":
                    return 170;

                case "Divine Orb":
                    return 20;

                case "Gemcutter's Prism":
                    return 1.5;

                default:
                    return 1;
            }
        }
    }
}
