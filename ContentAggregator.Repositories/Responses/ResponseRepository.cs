using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Responses
{
    public class ResponseRepository : DbRepository<Response, Context.Entities.Response>, IResponseRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public ResponseRepository(IMapper mapper, ApplicationDbContext context) : base(mapper, context)
        {
            _context = context;
            _mapper = mapper;
        }

        public override async Task Delete(string id)
        {
            IQueryable<ResponseLike> likesEntities = _context.ResponseLikes.Where(x => x.EntityId == id);

            _context.ResponseLikes.RemoveRange(likesEntities);

            await _context.SaveChangesAsync();
            await base.Delete(id);
        }
    }
}