using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories
{
    public interface ICrudRepository<T> where T : BaseModel
    {
        Task Create(T obj);
        Task<T[]> GetAll();
        Task<T> GetById(string id);
        Task<T[]> Find(Expression<Func<T, bool>> predicate);
        Task<bool> Update(T obj);
        Task Delete(string id);
    }
}