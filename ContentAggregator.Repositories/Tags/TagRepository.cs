using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContentAggregator.Context;
using ContentAggregator.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace ContentAggregator.Repositories.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TagRepository(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<Tag[]> GetAll()
        {
            return _context.Tags
               .AsNoTracking()
               .ProjectTo<Tag>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
        }

        public Task<Tag> GetByName(string tagName)
        {
            return _context.Tags
               .AsNoTracking()
               .ProjectTo<Tag>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(x => x.Name == tagName);
        }

        public async Task Create(Tag tag)
        {
            var entity = _mapper.Map<Context.Entities.Tag>(tag);
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Create(Tag[] tags)
        {
            Context.Entities.Tag[] existingEntitiesWithSameName =
                await _context.Tags.AsNoTracking()
                   .Where(x => tags.Select(t => t.Name).Contains(x.Name))
                   .ToArrayAsync();

            IEnumerable<Context.Entities.Tag> newEntities = tags
               .Where(t => !existingEntitiesWithSameName.Select(x => x.Name).Contains(t.Name)).Select(_mapper.Map<Context.Entities.Tag>);

            await _context.Tags.AddRangeAsync(newEntities);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string tagName)
        {
            Context.Entities.Tag entity = await _context.Tags
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Name == tagName);

            if (entity != null)
            {
                _context.Tags.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}