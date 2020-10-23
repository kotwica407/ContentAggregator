using AutoMapper;
using ContentAggregator.Models.Model.Likes;

namespace ContentAggregator.Repositories.Mappings.Profiles.Likes
{
    public class CommentLikeProfile : Profile
    {
        public CommentLikeProfile()
        {
            CreateMap<CommentLike, Context.Entities.Likes.CommentLike>();
            CreateMap<Context.Entities.Likes.CommentLike, CommentLike>();
        }
    }
}