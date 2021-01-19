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
        public DbSet<BaseLikeEntity<Post>> PostLikes { get; set; }
        public DbSet<BaseLikeEntity<Comment>> CommentLikes { get; set; }
        public DbSet<BaseLikeEntity<Response>> ResponseLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
               .HasOne<User>(p => p.Author)
               .WithMany(u => u.Posts)
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
               .HasOne<Post>(c => c.Post)
               .WithMany(p => p.Comments)
               .HasForeignKey(c => c.PostId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
               .HasOne<User>(c => c.Author)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.AuthorId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Response>()
               .HasOne<Comment>(r => r.Comment)
               .WithMany(c => c.Responses)
               .HasForeignKey(r => r.CommentId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Response>()
               .HasOne<User>(r => r.Author)
               .WithMany(u => u.Responses)
               .HasForeignKey(r => r.AuthorId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
               .HasOne<Picture>(u => u.Picture)
               .WithOne(p => p.User)
               .HasForeignKey<Picture>(p => p.UserId);

            ConfigurePostLike(modelBuilder);

            ConfigureCommentLike(modelBuilder);

            ConfigureResponseLike(modelBuilder);
        }

        private static void ConfigureResponseLike(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseLikeEntity<Response>>()
               .HasKey(rl => new {rl.UserId, rl.EntityId});

            modelBuilder.Entity<BaseLikeEntity<Response>>()
               .HasOne<User>(rl => rl.User)
               .WithMany(u => u.ResponseLikes)
               .HasForeignKey(rl => rl.UserId)
               .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<BaseLikeEntity<Response>>()
               .HasOne<Response>(rl => rl.Entity)
               .WithMany(r => r.ResponseLikes)
               .HasForeignKey(rl => rl.EntityId)
               .OnDelete(DeleteBehavior.ClientCascade);
        }

        private static void ConfigureCommentLike(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseLikeEntity<Comment>>()
               .HasKey(cl => new {cl.UserId, cl.EntityId});

            modelBuilder.Entity<BaseLikeEntity<Comment>>()
               .HasOne<User>(cl => cl.User)
               .WithMany(u => u.CommentLikes)
               .HasForeignKey(cl => cl.UserId)
               .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<BaseLikeEntity<Comment>>()
               .HasOne<Comment>(cl => cl.Entity)
               .WithMany(c => c.CommentLikes)
               .HasForeignKey(cl => cl.EntityId)
               .OnDelete(DeleteBehavior.ClientCascade);
        }

        private static void ConfigurePostLike(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseLikeEntity<Post>>()
               .HasKey(pl => new {pl.UserId, pl.EntityId});

            modelBuilder.Entity<BaseLikeEntity<Post>>()
               .HasOne<User>(pl => pl.User)
               .WithMany(u => u.PostLikes)
               .HasForeignKey(pl => pl.UserId)
               .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<BaseLikeEntity<Post>>()
               .HasOne<Post>(pl => pl.Entity)
               .WithMany(p => p.PostLikes)
               .HasForeignKey(pl => pl.EntityId)
               .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}