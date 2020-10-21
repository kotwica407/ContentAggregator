using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using Microsoft.EntityFrameworkCore;

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
    }
}