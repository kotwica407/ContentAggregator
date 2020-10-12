using ContentAggregator.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Hash> Hashes { get; set; }
    }
}