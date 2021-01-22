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
        public ResponseRepository(IMapper mapper, ApplicationDbContext context) : base(mapper, context)
        {
        }
    }
}