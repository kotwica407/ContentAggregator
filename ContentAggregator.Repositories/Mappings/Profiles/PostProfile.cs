using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, Context.Entities.Post>()
               .ForMember(dest => dest.Comments, opt => opt.Ignore())
               .ForMember(dest => dest.PostLikes, opt => opt.Ignore());
            CreateMap<Context.Entities.Post, Post>()
               .ForMember(dest => dest.Author, opt => opt.Ignore());
        }
    }
}