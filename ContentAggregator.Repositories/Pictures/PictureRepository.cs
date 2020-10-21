using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Pictures
{
    public class PictureRepository : DbRepository<Picture, Context.Entities.Picture>, IPictureRepository
    {
        public PictureRepository(IMapper mapper, ApplicationDbContext context) : base(mapper, context)
        {
        }
    }
}