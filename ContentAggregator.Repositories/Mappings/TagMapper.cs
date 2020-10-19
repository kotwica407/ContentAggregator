﻿using ContentAggregator.Context.Entities;

namespace ContentAggregator.Repositories.Mappings
{
    public static class TagMapper
    {
        internal static Tag Map(Models.Model.Tag tag)
        {
            if (tag == null)
                return null;

            return new Tag
            {
                Name = tag.Name,
                PostsNumber = tag.PostsNumber
            };
        }


        internal static Models.Model.Tag Map(Tag tag)
        {
            if (tag == null)
                return null;

            return new Models.Model.Tag
            {
                Name = tag.Name,
                PostsNumber = tag.PostsNumber
            };
        }
            
    }
}