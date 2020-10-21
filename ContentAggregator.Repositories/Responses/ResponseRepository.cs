using AutoMapper;
using ContentAggregator.Context;
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