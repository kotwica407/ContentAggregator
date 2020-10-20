using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, Context.Entities.Comment>();
            CreateMap<Context.Entities.Comment, Comment>();
        }
    }
}