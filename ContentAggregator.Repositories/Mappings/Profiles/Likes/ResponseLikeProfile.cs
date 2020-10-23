using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles.Likes
{
    public class ResponseLikeProfile : Profile
    {
        public ResponseLikeProfile()
        {
            CreateMap<Response, Context.Entities.Response>();
            CreateMap<Context.Entities.Response, Response>();
        }
    }
}