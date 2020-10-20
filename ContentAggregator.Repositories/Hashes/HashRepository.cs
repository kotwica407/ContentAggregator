using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories.Hashes
{
    public class HashRepository : IHashRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public HashRepository(
            ApplicationDbContext applicationDbContext,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task CreateOrUpdate(Hash hash)
        {
            Context.Entities.Hash entity =
                await _applicationDbContext.Hashes.FirstOrDefaultAsync(x => x.Id == hash.Id);

            if (entity != null)
                entity.PasswordHash = hash.PasswordHash;
            else
                await _applicationDbContext.Hashes.AddAsync(_mapper.Map<Context.Entities.Hash>(hash));

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            Context.Entities.Hash entity = await _applicationDbContext.Hashes.FirstOrDefaultAsync(x => x.Id == userId);
            if (entity != null)
            {
                _applicationDbContext.Hashes.Remove(entity);
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public Task<Hash> Get(string userId)
        {
            return _applicationDbContext.Hashes.AsNoTracking()
               .ProjectTo<Hash>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}