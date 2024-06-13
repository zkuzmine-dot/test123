using Gorb.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gorb.Server.DB
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FriendShip> FriendShips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=YAICO.db");
        }
    }
}
