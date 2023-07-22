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

        public DbSet<Home> Houses { get; set; }

        public DbSet<Room> Rooms { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
