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
    }
}
