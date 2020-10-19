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
    public class DbRepository<Tmodel, Tentity> : ICrudRepository<Tmodel>
        where Tmodel : BaseModel
        where Tentity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DbRepository(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Create(Tmodel obj)
        {
            var entity = _mapper.Map<Tentity>(obj);
            await _context.Set<Tentity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<Tmodel[]> GetAll()
        {
            return _context.Set<Tentity>()
               .AsNoTracking()
               .ProjectTo<Tmodel>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
        }
            

        public Task<Tmodel> GetById(string id)
        {
            return _context.Set<Tentity>()
               .AsNoTracking()
               .Where(x => x.Id == id)
               .ProjectTo<Tmodel>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync();
        }

        public Task<Tmodel[]> Find(Expression<Func<Tmodel, bool>> predicate)
        {
            Expression<Func<Tentity, bool>> mappedSelector = _mapper.Map<Expression<Func<Tentity, bool>>>(predicate);
            return _context.Set<Tentity>()
               .AsNoTracking()
               .Where(mappedSelector)
               .ProjectTo<Tmodel>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
        }

        public async Task<bool> Update(Tmodel obj)
        {
            var entity = _mapper.Map<Tentity>(obj);
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

        public async Task Delete(string id)
        {
            Tentity existingEntity = await _context.Set<Tentity>().FirstOrDefaultAsync(x => x.Id == id);
            if (existingEntity != null)
            {
                _context.Set<Tentity>().Remove(existingEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}