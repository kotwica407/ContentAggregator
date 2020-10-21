using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Pictures
{
    public interface IPictureRepository
    {
        Task Create(Picture obj);
        Task<Picture[]> GetAll();
        Task<Picture> GetById(string id);
        Task<Picture[]> Find(Expression<Func<Picture, bool>> predicate);
        Task<bool> Update(Picture obj);
        Task Delete(string id);
    }
}