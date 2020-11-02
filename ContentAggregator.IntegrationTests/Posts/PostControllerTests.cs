using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentAggregator.IntegrationTests.Common;
using ContentAggregator.Models.Dtos.Posts;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ContentAggregator.IntegrationTests.Posts
{
    public class PostControllerTests
    {
        [Fact]
        public async Task GetTenPosts()
        {
            using (var client = await Helpers.InitAsync())
            {
                var response = await client.GetAsync("/api/post/?skip=0&take=10");
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task TryToCreatePostWhenUnauthorized()
        {
            using (var client = await Helpers.InitAsync())
            {
                var response = await client.PostAsync("/api/post", new StringContent(JsonSerializer.Serialize(new CreatePostDto
                {
                    Title = "Some title",
                    Content = "Some content"
                }), Encoding.UTF8, "application/json"));
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
    }
}