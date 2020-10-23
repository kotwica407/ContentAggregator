using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models.Model;
using Response = ContentAggregator.Context.Entities.Response;

namespace ContentAggregator.Repositories.Comments
{
    public class CommentRepository : DbRepository<Comment, Context.Entities.Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(IMapper mapper, ApplicationDbContext context) : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }

        public override async Task Delete(string id)
        {
            IQueryable<Response> responseEntities = _context.Responses.Where(x => x.CommentId == id);
            IQueryable<ResponseLike> responseLikesEntities =
                _context.ResponseLikes.Where(x => responseEntities.Select(r => r.Id).Contains(x.EntityId));
            IQueryable<CommentLike> likesEntities = _context.CommentLikes.Where(x => x.EntityId == id);

            _context.Responses.RemoveRange(responseEntities);
            _context.ResponseLikes.RemoveRange(responseLikesEntities);
            _context.CommentLikes.RemoveRange(likesEntities);

            await _context.SaveChangesAsync();
            await base.Delete(id);
        }
    }
}