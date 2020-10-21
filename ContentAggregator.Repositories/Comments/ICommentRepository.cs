using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Comments
{
    public interface ICommentRepository
    {
        Task Create(Comment obj);
        Task<Comment[]> GetAll();
        Task<Comment> GetById(string id);
        Task<Comment[]> Find(Expression<Func<Comment, bool>> predicate);
        Task<bool> Update(Comment obj);
        Task Delete(string id);
    }
}