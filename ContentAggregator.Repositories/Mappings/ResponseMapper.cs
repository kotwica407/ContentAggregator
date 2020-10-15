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
                Author = UserMapper.Map(response.Author),
                Comment = new Comment
                {
                    Id = response.Comment.Id,
                    Rate = response.Comment.Rate,
                    Author = UserMapper.Map(response.Comment.Author),
                    Content = response.Comment.Content,
                    CreationTime = response.Comment.CreationTime,
                    LastUpdateTime = response.Comment.LastUpdateTime,
                    Post = new Post
                    {
                        Id = response.Comment.Post.Id,
                        Rate = response.Comment.Post.Rate,
                        Content = response.Comment.Post.Content,
                        CreationTime = response.Comment.Post.CreationTime,
                        LastUpdateTime = response.Comment.Post.LastUpdateTime,
                        Author = UserMapper.Map(response.Comment.Post.Author),
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
                Author = UserMapper.Map(response.Author),
                Comment = new Models.Model.Comment
                {
                    Id = response.Comment.Id,
                    Rate = response.Comment.Rate,
                    Author = UserMapper.Map(response.Comment.Author),
                    Content = response.Comment.Content,
                    CreationTime = response.Comment.CreationTime,
                    LastUpdateTime = response.Comment.LastUpdateTime,
                    Post = new Models.Model.Post
                    {
                        Id = response.Comment.Post.Id,
                        Rate = response.Comment.Post.Rate,
                        Content = response.Comment.Post.Content,
                        CreationTime = response.Comment.Post.CreationTime,
                        LastUpdateTime = response.Comment.Post.LastUpdateTime,
                        Author = UserMapper.Map(response.Comment.Post.Author),
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