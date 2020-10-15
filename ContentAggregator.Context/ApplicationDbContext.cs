using ContentAggregator.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Hash> Hashes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
               .HasMany(p => p.Comments)
               .WithOne(c => c.Post);

            modelBuilder.Entity<Comment>()
               .HasMany(c => c.Responses)
               .WithOne(r => r.Comment);
        }
    }
}