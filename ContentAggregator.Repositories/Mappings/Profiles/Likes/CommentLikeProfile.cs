using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles.Likes
{
    public class CommentLikeProfile : Profile
    {
        public CommentLikeProfile()
        {
            CreateMap<Comment, Context.Entities.Comment>();
            CreateMap<Context.Entities.Comment, Comment>();
        }
    }
}