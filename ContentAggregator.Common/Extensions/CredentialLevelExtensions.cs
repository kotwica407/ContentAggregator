using ContentAggregator.Models;
using System;
using System.Collections.Generic;
namespace ContentAggregator.Common.Extensions
{
    public static class CredentialLevelExtensions
    {
        //I think, admin should have Moderator role
        public static string[] GetAllPossibleRoles(this CredentialLevel credentialLevel)
        {
            var list = new List<string>();

            if(credentialLevel.HasFlag(CredentialLevel.SuperAdmin))
                list.Add(CredentialLevel.SuperAdmin.ToString("G"));

            if (credentialLevel.HasFlag(CredentialLevel.Admin))
                list.Add(CredentialLevel.Admin.ToString("G"));

            if (credentialLevel.HasFlag(CredentialLevel.Moderator))
                list.Add(CredentialLevel.Moderator.ToString("G"));

            if (credentialLevel.HasFlag(CredentialLevel.User))
                list.Add(CredentialLevel.User.ToString("G"));

            return list.ToArray();
        }
    }
}
