using System.Linq;
using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, Context.Entities.Comment>()
               .ForMember(c => c.Post, o => o.MapFrom(c => PostMapper.Map(c.Post)))
               .ForMember(c => c.Responses, o => o.MapFrom(c => c.Responses.Select(ResponseMapper.Map)));
            CreateMap<Context.Entities.Comment, Comment>()
               .ForMember(c => c.Post, o => o.MapFrom(c => PostMapper.Map(c.Post)))
               .ForMember(c => c.Responses, o => o.MapFrom(c => c.Responses.Select(ResponseMapper.Map)));
        }   
    }
}