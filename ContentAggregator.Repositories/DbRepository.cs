using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentAggregator.Context;
using ContentAggregator.Context.Entities;
using ContentAggregator.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories
{
    public abstract class DbRepository<TModel, TEntity> : ICrudRepository<TModel>
        where TModel : BaseModel
        where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DbRepository(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Create(TModel obj)
        {
            var entity = _mapper.Map<TEntity>(obj);
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<TModel[]> GetAll()
        {
            return _context.Set<TEntity>()
               .AsNoTracking()
               .ProjectTo<TModel>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
        }
            

        public Task<TModel> GetById(string id)
        {
            return _context.Set<TEntity>()
               .AsNoTracking()
               .Where(x => x.Id == id)
               .ProjectTo<TModel>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync();
        }

        public Task<TModel[]> Find(Expression<Func<TModel, bool>> predicate)
        {
            Expression<Func<TEntity, bool>> mappedSelector = _mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            return _context.Set<TEntity>()
               .AsNoTracking()
               .Where(mappedSelector)
               .ProjectTo<TModel>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
        }

        public async Task<bool> Update(TModel obj)
        {
            var entity = _mapper.Map<TEntity>(obj);
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task Delete(string id)
        {
            TEntity existingEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (existingEntity != null)
            {
                _context.Set<TEntity>().Remove(existingEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}