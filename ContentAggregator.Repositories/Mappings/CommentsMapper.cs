using ContentAggregator.Context.Entities;

namespace ContentAggregator.Repositories.Mappings
{
    public static class CommentsMapper
    {
        internal static Comment Map(Models.Model.Comment comment)
        {
            if (comment == null)
                return null;

            return new Comment
            {
                Id = comment.Id,
                Rate = comment.Rate,
                Author = UserMapper.Map(comment.Author),
                Content = comment.Content,
                CreationTime = comment.CreationTime,
                LastUpdateTime = comment.LastUpdateTime,
                Post = new Post
                {
                    Id = comment.Post.Id,
                    Rate = comment.Post.Rate,
                    Content = comment.Post.Content,
                    CreationTime = comment.Post.CreationTime,
                    LastUpdateTime = comment.Post.LastUpdateTime,
                    Author = UserMapper.Map(comment.Post.Author),
                    Tags = comment.Post.Tags
                }
            };
        }
            

        internal static Models.Model.Comment Map(Comment comment)
        {
            if (comment == null)
                return null;

            return new Models.Model.Comment
            {
                Id = comment.Id,
                Rate = comment.Rate,
                Author = UserMapper.Map(comment.Author),
                Content = comment.Content,
                CreationTime = comment.CreationTime,
                LastUpdateTime = comment.LastUpdateTime,
                Post = new Models.Model.Post
                {
                    Id = comment.Post.Id,
                    Rate = comment.Post.Rate,
                    Content = comment.Post.Content,
                    CreationTime = comment.Post.CreationTime,
                    LastUpdateTime = comment.Post.LastUpdateTime,
                    Author = UserMapper.Map(comment.Post.Author),
                    Tags = comment.Post.Tags
                }
            };
        }
            
    }
}