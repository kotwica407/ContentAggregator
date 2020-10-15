using ContentAggregator.Context.Entities;

namespace ContentAggregator.Repositories.Mappings
{
    public static class ResponseMapper
    {
        internal static Response Map(Models.Model.Response response)
        {
            if (response == null)
                return null;

            return new Response
            {
                Id = response.Id,
                AuthorId = response.AuthorId,
                Comment = new Comment
                {
                    Id = response.Comment.Id,
                    Rate = response.Comment.Rate,
                    AuthorId = response.Comment.AuthorId,
                    Content = response.Comment.Content,
                    CreationTime = response.Comment.CreationTime,
                    LastUpdateTime = response.Comment.LastUpdateTime,
                    Post = new Post
                    {
                        Id = response.Comment.Post.Id,
                        Rate = response.Comment.Post.Rate,
                        Title = response.Comment.Post.Title,
                        Content = response.Comment.Post.Content,
                        CreationTime = response.Comment.Post.CreationTime,
                        LastUpdateTime = response.Comment.Post.LastUpdateTime,
                        AuthorId = response.Comment.Post.AuthorId,
                        Tags = response.Comment.Post.Tags
                    }
                },
                CreationTime = response.CreationTime,
                LastUpdateTime = response.LastUpdateTime,
                Rate = response.Rate,
                Content = response.Content
            };
        }


        internal static Models.Model.Response Map(Response response)
        {
            if (response == null)
                return null;

            return new Models.Model.Response
            {
                Id = response.Id,
                AuthorId = response.AuthorId,
                Comment = new Models.Model.Comment
                {
                    Id = response.Comment.Id,
                    Rate = response.Comment.Rate,
                    AuthorId = response.Comment.AuthorId,
                    Content = response.Comment.Content,
                    CreationTime = response.Comment.CreationTime,
                    LastUpdateTime = response.Comment.LastUpdateTime,
                    Post = new Models.Model.Post
                    {
                        Id = response.Comment.Post.Id,
                        Rate = response.Comment.Post.Rate,
                        Title = response.Comment.Post.Title,
                        Content = response.Comment.Post.Content,
                        CreationTime = response.Comment.Post.CreationTime,
                        LastUpdateTime = response.Comment.Post.LastUpdateTime,
                        AuthorId = response.Comment.Post.AuthorId,
                        Tags = response.Comment.Post.Tags
                    }
                },
                CreationTime = response.CreationTime,
                LastUpdateTime = response.LastUpdateTime,
                Rate = response.Rate,
                Content = response.Content
            };
        }
            
    }
}