using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Posts
{
    public interface IPostRepository
    {
        Task Create(Post obj);
        Task<Post[]> GetAll();
        Task<Post[]> GetPage(int skip, int take);
        Task<Post> GetById(string id);
        Task<Post[]> Find(Expression<Func<Post, bool>> predicate);
        Task<bool> Update(Post obj);
        Task Delete(string id);
    }
}