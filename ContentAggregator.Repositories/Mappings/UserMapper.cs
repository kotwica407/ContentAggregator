using ContentAggregator.Models.Model;

namespace ContentAggregator.Repositories.Mappings
{
    public static class UserMapper
    {
        internal static User Map(Context.Entities.User user)
        {
            if (user == null)
                return null;

            return new User
            {
                CredentialLevel = user.CredentialLevel,
                Description = user.Description,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                BlackListedTags = user.BlackListedTags,
                BlackListedUserIds = user.BlackListedUserIds,
                FollowedTags = user.FollowedTags,
                FollowedUserIds = user.FollowedUserIds
            };
        }

        internal static Context.Entities.User Map(User user)
        {
            if (user == null)
                return null;

            return new Context.Entities.User
            {
                CredentialLevel = user.CredentialLevel,
                Description = user.Description,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                BlackListedTags = user.BlackListedTags,
                BlackListedUserIds = user.BlackListedUserIds,
                FollowedTags = user.FollowedTags,
                FollowedUserIds = user.FollowedUserIds
            };
        }
    }
}