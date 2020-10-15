using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentAggregator.Context;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Comment = ContentAggregator.Context.Entities.Comment;

namespace ContentAggregator.Repositories.Posts
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PostRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task<Post[]> GetAll()
        {
            IQueryable<Context.Entities.Post> entities = _applicationDbContext.Posts
               .AsNoTracking()
               .Include(x => x.Comments);
            return Task.FromResult(entities.AsEnumerable().Select(PostMapper.Map).ToArray());
        }

        public async Task<Post> GetById(string id)
        {
            Context.Entities.Post entity =
                await _applicationDbContext.Posts
                   .AsNoTracking()
                   .FirstOrDefaultAsync(x => x.Id == id);
            return PostMapper.Map(entity);
        }

        public Task<Post[]> GetByAuthor(string authorId)
        {
            IIncludableQueryable<Context.Entities.Post, List<Comment>> entities = _applicationDbContext.Posts
               .AsNoTracking()
               .Where(x => x.AuthorId == authorId)
               .Include(x => x.Comments);
            return Task.FromResult(entities.AsEnumerable().Select(PostMapper.Map).ToArray());
        }

        public Task<Post[]> GetByTag(string tagName)
        {
            IIncludableQueryable<Context.Entities.Post, List<Comment>> entities = _applicationDbContext.Posts
               .AsNoTracking()
               .Where(x => x.Tags.Contains(tagName))
               .Include(x => x.Comments);
            return Task.FromResult(entities.AsEnumerable().Select(PostMapper.Map).ToArray());
        }

        public async Task Create(Post post)
        {
            Context.Entities.Post entity = PostMapper.Map(post);
            if (entity == null)
                throw HttpError.InternalServerError("Post entity cannot be null");
            await _applicationDbContext.Posts.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(Post post)
        {
            Context.Entities.Post entity = await _applicationDbContext.Posts.FirstOrDefaultAsync(x => x.Id == post.Id);
            if (entity == null)
                return false;

            Context.Entities.Post newEntity = PostMapper.Map(post);
            entity.Tags = newEntity.Tags;
            entity.CreationTime = newEntity.CreationTime;
            entity.LastUpdateTime = newEntity.LastUpdateTime;
            entity.Content = newEntity.Content;

            try
            {
                return await _applicationDbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task Delete(string id)
        {
            Context.Entities.Post existingEntity =
                await _applicationDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (existingEntity != null)
            {
                _applicationDbContext.Posts.Remove(existingEntity);
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task RateUp(string id)
        {
            var entity = await _applicationDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw HttpError.InternalServerError("Post entity cannot be null");

            entity.Rate++;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task RateDown(string id)
        {
            var entity = await _applicationDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw HttpError.InternalServerError("Post entity cannot be null");

            entity.Rate--;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}