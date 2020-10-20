using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<Picture, Context.Entities.Picture>();
            CreateMap<Context.Entities.Picture, Picture>();
        }
    }
}