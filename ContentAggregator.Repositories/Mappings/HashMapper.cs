using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings
{
    public static class HashMapper
    {
        internal static Hash Map(Context.Entities.Hash hash)
        {
            if (hash == null)
                return null;

            return new Hash
            {
                PasswordHash = hash.PasswordHash,
                UserId = hash.UserId
            };
        }

        internal static Context.Entities.Hash Map(Hash hash)
        {
            if (hash == null)
                return null;

            return new Context.Entities.Hash
            {
                PasswordHash = hash.PasswordHash,
                UserId = hash.UserId
            };
        }
    }
}