using ContentAggregator.Context.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentAggregator.Repositories.Hashes
{
    public interface IHashRepository
    {
        Task CreateOrUpdate(HashEntity hashEntity);
        Task<HashEntity> Get(string userId);
        Task Delete(string userId);
    }
}
