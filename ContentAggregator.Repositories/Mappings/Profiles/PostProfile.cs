using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, Context.Entities.Post>();
            CreateMap<Context.Entities.Post, Post>();
        }
    }
}