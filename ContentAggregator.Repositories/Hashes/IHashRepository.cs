using System.Threading.Tasks;
using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Hashes
{
    public interface IHashRepository
    {
        Task CreateOrUpdate(Hash hash);
        Task<Hash> Get(string userId);
        Task Delete(string userId);
    }
}
