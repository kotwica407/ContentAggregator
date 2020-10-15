﻿namespace ContentAggregator.Models.Model
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public CredentialLevel CredentialLevel { get; set; }
        public string Description { get; set; }
        public string[] BlackListedTags { get; set; }
        public string[] FollowedTags { get; set; }
        public string[] BlackListedUserIds { get; set; }
        public string[] FollowedUserIds { get; set; }
    }
}