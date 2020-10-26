using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, Context.Entities.Comment>()
               .ForMember(dest => dest.CommentLikes, opt => opt.Ignore())
               .ForMember(dest => dest.Responses, opt => opt.Ignore());
            CreateMap<Context.Entities.Comment, Comment>();
        }
    }
}