using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap<Response, Context.Entities.Response>()
               .ForMember(r => r.Comment, o => o.MapFrom(r => CommentsMapper.Map(r.Comment)));
            CreateMap<Context.Entities.Response, Response>()
               .ForMember(r => r.Comment, o => o.MapFrom(r => CommentsMapper.Map(r.Comment)));
        }
    }
}