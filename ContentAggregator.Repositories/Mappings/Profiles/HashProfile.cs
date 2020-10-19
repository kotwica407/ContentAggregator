using AutoMapper;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings.Profiles
{
    public class HashProfile : Profile
    {
        public HashProfile()
        {
            CreateMap<Hash, Context.Entities.Hash>();
            CreateMap<Context.Entities.Hash, Hash>();
        }
    }
}