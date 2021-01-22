using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories.Pictures
{
    public class PictureRepository : IPictureRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PictureRepository(IMapper mapper, ApplicationDbContext context) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateOrUpdate(Picture picture)
        {
            Context.Entities.Picture entity =
                await _context.Pictures.FirstOrDefaultAsync(x => x.Id == picture.Id);

            if (entity != null)
            {
                entity = _mapper.Map<Context.Entities.Picture>(picture);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                await _context.Pictures.AddAsync(_mapper.Map<Context.Entities.Picture>(picture));
            }    

            await _context.SaveChangesAsync();
        }

        public Task<Picture> Get(string userId)
        {
            return _context.Pictures.AsNoTracking()
               .ProjectTo<Picture>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task Delete(string userId)
        {
            Context.Entities.Picture entity = await _context.Pictures.FirstOrDefaultAsync(x => x.Id == userId);
            if (entity != null)
            {
                _context.Pictures.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}