using System.Threading.Tasks;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories.Hashes
{
    public class HashRepository : IHashRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HashRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task CreateOrUpdate(Hash hash)
        {
            Context.Entities.Hash entity =
                await _applicationDbContext.Hashes.FirstOrDefaultAsync(x => x.UserId == hash.UserId);

            if (entity != null)
                entity.PasswordHash = hash.PasswordHash;
            else
                await _applicationDbContext.Hashes.AddAsync(HashMapper.Map(hash));

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            Context.Entities.Hash entity = await _applicationDbContext.Hashes.FirstOrDefaultAsync(x => x.UserId == userId);
            if (entity != null)
            {
                _applicationDbContext.Hashes.Remove(entity);
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task<Hash> Get(string userId)
        {
            Context.Entities.Hash entity =
                await _applicationDbContext.Hashes.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            return HashMapper.Map(entity);
        }
    }
}