using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Context.Entities;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models.Model;
using Microsoft.EntityFrameworkCore;
using User = ContentAggregator.Models.Model.User;

namespace ContentAggregator.Repositories.Likes
{
    public class LikeRepository<TModel, TEntity> : ILikeRepository<TModel>
        where TEntity : PostBaseEntity
        where TModel : PostBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LikeRepository(IMapper mapper, ApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task GiveLike(User user, TModel postBase, bool isLike)
        {
            var existingEntity = await _context.Set<BaseLikeEntity<TEntity>>()
               .FirstOrDefaultAsync(x => x.UserId == user.Id && x.EntityId == postBase.Id);

            if (existingEntity == null)
            {
                var newEntity = new BaseLikeEntity<TEntity>()
                {
                    IsLike = isLike,
                    EntityId = postBase.Id,
                    UserId = user.Id
                };
                await _context.Set<BaseLikeEntity<TEntity>>()
                   .AddAsync(newEntity);
            }
            else
            {
                existingEntity.IsLike = isLike;
            }

            await _context.SaveChangesAsync();
        }

        public async Task CancelLikeOrDislike(string userId, string postBaseId)
        {
            var existingEntity = await _context.Set<BaseLikeEntity<TEntity>>()
               .FirstOrDefaultAsync(x => x.UserId == userId && x.EntityId == postBaseId);

            if (existingEntity != null)
            {
                _context.Set<BaseLikeEntity<TEntity>>().Remove(existingEntity);
                await _context.SaveChangesAsync();
            }
        }

        public Task<int> GetNumberOfLikes(string postBaseId)
        {
            return _context.Set<BaseLikeEntity<TEntity>>()
               .Where(x => x.EntityId == postBaseId && x.IsLike)
               .CountAsync();
        }

        public Task<int> GetNumberOfDislikes(string postBaseId)
        {
            return _context.Set<BaseLikeEntity<TEntity>>()
               .Where(x => x.EntityId == postBaseId && !x.IsLike)
               .CountAsync();
        }
    }
}