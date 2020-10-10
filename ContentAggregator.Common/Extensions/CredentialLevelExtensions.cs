using ContentAggregator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentAggregator.Common.Extensions
{
    public static class CredentialLevelExtensions
    {
        //I think, admin should have Moderator role
        public static string[] GetAllPossibleRoles(this CredentialLevel credentialLevel)
        {
            var list = new List<string>() { CredentialLevel.User.ToString("G") };

            switch (credentialLevel)
            {
                case CredentialLevel.Moderator:
                    list.Add(CredentialLevel.Moderator.ToString("G"));
                    break;
                case CredentialLevel.Admin:
                    list.AddRange(new[] { 
                        CredentialLevel.Admin.ToString("G"),
                        CredentialLevel.Moderator.ToString("G")
                    });
                    break;
                case CredentialLevel.SuperAdmin:
                    list.AddRange(new[] { 
                        CredentialLevel.Admin.ToString("G"),
                        CredentialLevel.Moderator.ToString("G"),
                        CredentialLevel.SuperAdmin.ToString("G") 
                    });
                    break;
                default:
                    break;
            }

            return list.ToArray();
        }
    }
}
