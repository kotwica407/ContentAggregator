using System.Linq;
using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, Context.Entities.Post>()
               .ForMember(p => p.Comments, o => o.MapFrom(p => p.Comments.Select(CommentsMapper.Map)));
            CreateMap<Context.Entities.Post, Post>()
               .ForMember(p => p.Comments, o => o.MapFrom(p => p.Comments.Select(CommentsMapper.Map)));
        }
    }
}