using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Pictures
{
    public interface IPictureRepository
    {
        Task CreateOrUpdate(Picture picture);
        Task<Picture> Get(string userId);
        Task Delete(string userId);
    }
}