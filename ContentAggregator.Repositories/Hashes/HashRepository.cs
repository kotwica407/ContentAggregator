using ContentAggregator.Context;
using ContentAggregator.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContentAggregator.Repositories.Hashes
{
    public class HashRepository : IHashRepository
    {
        private readonly UserContext _userContext;

        public HashRepository(UserContext userContext)
        {
            _userContext = userContext;
        }
        public async Task CreateOrUpdate(HashEntity hashEntity)
        {
            var existingEntity = await _userContext.HashEntities.FirstOrDefaultAsync(x => x.UserId == hashEntity.UserId);

            if (existingEntity != null)
            {
                existingEntity.PasswordHash = hashEntity.PasswordHash;
            }
            else
            {
                await _userContext.HashEntities.AddAsync(hashEntity);
            }
            
           await _userContext.SaveChangesAsync();
        }

        public async Task Delete(string userId)
        {
            var entity = await _userContext.HashEntities.FirstOrDefaultAsync(x => x.UserId == userId);
            if(entity != null)
            {
                _userContext.HashEntities.Remove(entity);
                await _userContext.SaveChangesAsync();
            }
        }

        public async Task<HashEntity> Get(string userId)
        {
            return await _userContext.HashEntities.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
