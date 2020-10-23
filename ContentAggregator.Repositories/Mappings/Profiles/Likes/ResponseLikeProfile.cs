using AutoMapper;
using ContentAggregator.Models.Model.Likes;

namespace ContentAggregator.Repositories.Mappings.Profiles.Likes
{
    public class ResponseLikeProfile : Profile
    {
        public ResponseLikeProfile()
        {
            CreateMap<ResponseLike, Context.Entities.Likes.ResponseLike>();
            CreateMap<Context.Entities.Likes.ResponseLike, ResponseLike>();
        }
    }
}