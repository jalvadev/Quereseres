using Microsoft.EntityFrameworkCore;
using Quereseres.Models;

namespace Quereseres.Contexts
{
    public class QuereseresContext : DbContext
    {
        public QuereseresContext(DbContextOptions<QuereseresContext> options)
            :base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
