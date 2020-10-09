using System;
using System.Collections.Generic;
using System.Text;

namespace ContentAggregator.Models
{
    public enum CredentialLevel : byte
    {
        User,
        Moderator,
        Admin,
        SuperAdmin
    }
}
