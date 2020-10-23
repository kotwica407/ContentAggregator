using AutoMapper;
using ContentAggregator.Models.Model.Likes;

namespace ContentAggregator.Repositories.Mappings.Profiles.Likes
{
    public class PostLikeProfile : Profile
    {
        public PostLikeProfile()
        {
            CreateMap<PostLike, Context.Entities.Likes.PostLike>();
            CreateMap<Context.Entities.Likes.PostLike, PostLike>();
        }
    }
}