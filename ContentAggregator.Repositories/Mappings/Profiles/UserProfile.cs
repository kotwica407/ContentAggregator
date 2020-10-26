using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, Context.Entities.User>()
               .ForMember(dest => dest.Posts, opt => opt.Ignore())
               .ForMember(dest => dest.Comments, opt => opt.Ignore())
               .ForMember(dest => dest.Responses, opt => opt.Ignore())
               .ForMember(dest => dest.PostLikes, opt => opt.Ignore())
               .ForMember(dest => dest.CommentLikes, opt => opt.Ignore())
               .ForMember(dest => dest.ResponseLikes, opt => opt.Ignore());
            CreateMap<Context.Entities.User, User>();
        }
    }
}