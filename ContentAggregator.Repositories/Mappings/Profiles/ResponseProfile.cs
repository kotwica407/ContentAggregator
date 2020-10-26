using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap<Response, Context.Entities.Response>()
               .ForMember(dest => dest.ResponseLikes, opt => opt.Ignore());
            CreateMap<Context.Entities.Response, Response>();
        }
    }
}