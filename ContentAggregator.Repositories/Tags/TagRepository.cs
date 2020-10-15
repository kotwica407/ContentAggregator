using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TagRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task<Tag[]> GetAll()
        {
            IQueryable<Context.Entities.Tag> entities = _applicationDbContext.Tags
               .AsNoTracking();
            return Task.FromResult(entities.AsEnumerable().Select(TagMapper.Map).ToArray());
        }

        public async Task<Tag> GetByName(string tagName)
        {
            Context.Entities.Tag entity = await _applicationDbContext.Tags
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Name == tagName);

            return TagMapper.Map(entity);
        }

        public async Task Create(Tag tag)
        {
            Context.Entities.Tag existingEntity =
                await _applicationDbContext.Tags.FirstOrDefaultAsync(x => x.Name == tag.Name);
            if (existingEntity != null)
                return;

            Context.Entities.Tag newEntity = TagMapper.Map(tag);

            await _applicationDbContext.Tags.AddAsync(newEntity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Create(Tag[] tags)
        {
            Context.Entities.Tag[] existingEntitiesWithSameName =
                await _applicationDbContext.Tags.AsNoTracking().Where(x => tags.Select(t => t.Name).Contains(x.Name))
                   .ToArrayAsync();

            IEnumerable<Context.Entities.Tag> newEntities = tags
               .Where(t => !existingEntitiesWithSameName.Select(x => x.Name).Contains(t.Name)).Select(TagMapper.Map);

            await _applicationDbContext.Tags.AddRangeAsync(newEntities);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(string tagName)
        {
            Context.Entities.Tag entity = await _applicationDbContext.Tags
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Name == tagName);

            if (entity != null)
            {
                _applicationDbContext.Tags.Remove(entity);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}