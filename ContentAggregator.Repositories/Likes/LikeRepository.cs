using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContentAggregator.Context;
using ContentAggregator.Context.Entities;
using ContentAggregator.Context.Entities.Likes;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model.Likes;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories.Likes
{
    public class LikeRepository<TLikeModel, TLikeEntity, TEntity> : ILikeRepository<TLikeModel>
        where TLikeModel : BaseLike
        where TLikeEntity : BaseLikeEntity
        where TEntity : PostBaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LikeRepository(IMapper mapper, ApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task GiveLike(TLikeModel like)
        {
            TLikeEntity existingLikeEntity = await _context.Set<TLikeEntity>()
               .FirstOrDefaultAsync(x => x.UserId == like.UserId && x.EntityId == like.EntityId);
            TEntity existingEntity = await _context.Set<TEntity>()
               .FirstOrDefaultAsync(x => x.Id == like.EntityId);

            if (existingEntity == null)
                throw HttpError.NotFound($"There is no {typeof(TEntity).Name} with id {like.EntityId} in db.");

            bool valueChanged = existingLikeEntity?.IsLike != like.IsLike;

            if (existingLikeEntity == null)
            {
                var newEntity = _mapper.Map<TLikeEntity>(like);
                await _context.Set<TLikeEntity>().AddAsync(newEntity);
            }
            else
            {
                existingLikeEntity.IsLike = like.IsLike;
            }

            if (existingLikeEntity == null)
            {
                if (like.IsLike)
                    existingEntity.Likes++;
                else
                    existingEntity.Dislikes++;
            }
            else
            {
                if (valueChanged)
                {
                    if (like.IsLike)
                    {
                        existingEntity.Dislikes--;
                        existingEntity.Likes++;
                    }
                    else
                    {
                        existingEntity.Dislikes++;
                        existingEntity.Likes--;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task CancelLikeOrDislike(string entityId, string userId)
        {
            TLikeEntity existingLikeEntity = await _context.Set<TLikeEntity>()
               .FirstOrDefaultAsync(x => x.EntityId == entityId && x.UserId == userId);

            if (existingLikeEntity != null)
            {
                _context.Set<TLikeEntity>().Remove(existingLikeEntity);

                TEntity existingEntity = await _context.Set<TEntity>()
                   .FirstOrDefaultAsync(x => x.Id == entityId);

                if (existingLikeEntity.IsLike)
                    existingEntity.Likes--;
                else
                    existingEntity.Dislikes--;

                await _context.SaveChangesAsync();
            }
        }

        public Task<int> GetNumberOfLikes(string entityId)
        {
            return _context.Set<TLikeEntity>()
               .Where(x => x.EntityId == entityId && x.IsLike)
               .CountAsync();
        }

        public Task<int> GetNumberOfDislikes(string entityId)
        {
            return _context.Set<TLikeEntity>()
               .Where(x => x.EntityId == entityId && !x.IsLike)
               .CountAsync();
        }
    }
}