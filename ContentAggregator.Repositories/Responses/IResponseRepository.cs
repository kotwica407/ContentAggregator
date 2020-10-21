using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Responses
{
    public interface IResponseRepository
    {
        Task Create(Response obj);
        Task<Response[]> GetAll();
        Task<Response> GetById(string id);
        Task<Response[]> Find(Expression<Func<Response, bool>> predicate);
        Task<bool> Update(Response obj);
        Task Delete(string id);
    }
}