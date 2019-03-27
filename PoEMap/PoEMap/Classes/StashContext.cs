﻿using Newtonsoft.Json.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace PoEMap.Classes
{
    /// <summary>
    /// DatabaseContext-class that contains database sets for stashes and maps. Uses SQLite, database file is "maps.db".
    /// </summary>
    public class StashContext : DbContext
    {
        public DbSet<Stash> Stashes { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        public StashContext(DbContextOptions<StashContext> options)
            : base(options)
        {
        }

        // Builds the database model.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>()
                .HasOne(p => p.Stash)
                .WithMany(b => b.Maps)
                .HasForeignKey(m => m.StashForeignKey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Map>()
                .HasOne(p => p.Price)
                .WithOne(m => m.Map)
                .HasForeignKey<Currency>(c => c.MapForeignKey)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        /// Creates a new stash-object and initializes it with an id, owner and (stash)name.
        /// </summary>
        /// <param name="jsonStash">Currently processed stash from the json.</param>
        /// <returns>New stash object.</returns>
        public Stash CreateNewStash(JObject jsonStash)
        {
            Stash newStash = new Stash(
                (string)jsonStash.SelectToken("id"),
                (string)jsonStash.SelectToken("lastCharacterName"),
                (string)jsonStash.SelectToken("stash"));

            return newStash;
        }

        /// <summary>
        /// Stores all map-items from json to stash-objects adds them to database. Doesn't add empty stashes.
        /// </summary>
        /// <param name="jsonStashes">Stashes as JArray-type.</param>
        public async void StoreMaps(JArray jsonStashes)
        {
            try
            {
                Stash currentStash;
                foreach (JObject jsonStash in jsonStashes)
                {
                    // First let's check if the stash contains any items. If it doesn't, remove that stash from the database if it exists.
                    JArray itemsArray = (JArray)jsonStash.SelectToken("items");
                    Stash stashFromDb = Stashes.Find((string)jsonStash.SelectToken("id"));

                    if ((string)jsonStash.SelectToken("lastCharacterName") == "EasyForEnceEsports")
                    {
                        Console.WriteLine("Löytyi");
                    }

                    currentStash = CreateNewStash(jsonStash);
                    foreach (JObject item in itemsArray)
                    {
                        JObject category = item.Value<JObject>("category");
                        if (category.ContainsKey("maps"))
                        {
                            currentStash.AddMap(item);
                        }
                    }
                    // If the stash didn't contain any map-items, it can be deleted.
                    if (currentStash.MapCount() == 0 || currentStash.Maps == null)
                    {
                        if (stashFromDb != null)
                        {
                            Stashes.Remove(currentStash);
                        }
                        continue;
                    }
                    else
                    {
                        if (stashFromDb != null)
                        {
                            Entry(stashFromDb).CurrentValues.SetValues(currentStash);
                        }
                        else
                        {
                            await Stashes.AddAsync(currentStash);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
