using System.Threading.Tasks;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories.Hashes
{
    public class HashRepository : IHashRepository
    {
        private readonly UserContext _userContext;

        public HashRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task CreateOrUpdate(Hash hash)
        {
            Context.Entities.Hash entity =
                await _userContext.HashEntities.FirstOrDefaultAsync(x => x.UserId == hash.UserId);

            if (entity != null)
                entity.PasswordHash = hash.PasswordHash;
            else
                await _userContext.HashEntities.AddAsync(HashMapper.Map(hash));

            await _userContext.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            Context.Entities.Hash entity = await _userContext.HashEntities.FirstOrDefaultAsync(x => x.UserId == userId);
            if (entity != null)
            {
                _userContext.HashEntities.Remove(entity);
                await _userContext.SaveChangesAsync();
            }
        }

        public async Task<Hash> Get(string userId)
        {
            Context.Entities.Hash entity =
                await _userContext.HashEntities.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            return HashMapper.Map(entity);
        }
    }
}