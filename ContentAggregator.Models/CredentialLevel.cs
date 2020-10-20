using System;

namespace ContentAggregator.Models
{
    [Flags]
    public enum CredentialLevel : byte
    {
        User = 1,
        Moderator = 2,
        Admin = 4,
        SuperAdmin = 8
    }
}
