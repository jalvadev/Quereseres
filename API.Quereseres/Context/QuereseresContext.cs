using API.Quereseres.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Quereseres.Context
{
    public class QuereseresContext : DbContext
    {
        public QuereseresContext(DbContextOptions<QuereseresContext> options)
        : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<House> Houses { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Housework> Houseworks { get; set; }

        public DbSet<HouseworkWeekly> HouseworkWeeklies { get; set; }

    }
}
