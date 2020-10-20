using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, Context.Entities.Tag>();
            CreateMap<Context.Entities.Tag, Tag>();
        }
    }
}