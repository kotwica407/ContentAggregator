using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentAggregator.Context;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models.Model;
using Microsoft.EntityFrameworkCore;
using Comment = ContentAggregator.Context.Entities.Comment;
using Response = ContentAggregator.Context.Entities.Response;

namespace ContentAggregator.Repositories.Posts
{
    public class PostRepository : DbRepository<Post, Context.Entities.Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PostRepository(IMapper mapper, ApplicationDbContext context) : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task<Post[]> GetPage(int skip, int take)
        {
            return _context.Posts.AsNoTracking()
               .OrderByDescending(x => x.CreationTime)
               .Skip(skip)
               .Take(take)
               .ProjectTo<Post>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
        }

        public override async Task Delete(string id)
        {
            IQueryable<Comment> commentEntities = _context.Comments.Where(x => x.PostId == id);
            IQueryable<Response> responseEntities =
                _context.Responses.Where(x => commentEntities.Select(c => c.Id).Contains(x.CommentId));

            IQueryable<ResponseLike> responseLikesEntities =
                _context.ResponseLikes.Where(x => responseEntities.Select(r => r.Id).Contains(x.EntityId));
            IQueryable<CommentLike> commentLikesEntities =
                _context.CommentLikes.Where(x => commentEntities.Select(c => c.Id).Contains(x.EntityId));
            IQueryable<CommentLike> likesEntities = _context.CommentLikes.Where(x => x.EntityId == id);

            _context.ResponseLikes.RemoveRange(responseLikesEntities);
            _context.Responses.RemoveRange(responseEntities);
            _context.Comments.RemoveRange(commentEntities);
            _context.CommentLikes.RemoveRange(commentLikesEntities);
            _context.CommentLikes.RemoveRange(likesEntities);

            await _context.SaveChangesAsync();
            await base.Delete(id);
        }
    }
}