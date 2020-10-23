using ContentAggregator.Context.Entities;
using ContentAggregator.Context.Entities.Likes;
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
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<ResponseLike> ResponseLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostLike>()
               .HasKey(l => new {l.EntityId, l.UserId});
            modelBuilder.Entity<CommentLike>()
               .HasKey(l => new { l.EntityId, l.UserId });
            modelBuilder.Entity<ResponseLike>()
               .HasKey(l => new { l.EntityId, l.UserId });
        }
    }
}