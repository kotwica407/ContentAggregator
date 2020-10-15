using System.Linq;
using ContentAggregator.Context.Entities;

namespace ContentAggregator.Repositories.Mappings
{
    public static class PostMapper
    {
        internal static Post Map(Models.Model.Post post)
        {
            if (post == null)
                return null;

            return new Post
            {
                Id = post.Id,
                Rate = post.Rate,
                Title = post.Title,
                Content = post.Content,
                CreationTime = post.CreationTime,
                LastUpdateTime = post.LastUpdateTime,
                Comments = post.Comments.Select(CommentsMapper.Map).ToList(),
                AuthorId = post.AuthorId,
                Tags = post.Tags
            };
        }

        internal static Models.Model.Post Map(Post post)
        {
            if (post == null)
                return null;

            return new Models.Model.Post
            {
                Id = post.Id,
                Rate = post.Rate,
                Title = post.Title,
                Content = post.Content,
                CreationTime = post.CreationTime,
                LastUpdateTime = post.LastUpdateTime,
                Comments = post.Comments.Select(CommentsMapper.Map).ToArray(),
                AuthorId = post.AuthorId,
                Tags = post.Tags
            };
        }
    }
}