using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.IntegrationTests.Common;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Dtos.Posts;
using Microsoft.Net.Http.Headers;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ContentAggregator.IntegrationTests.Posts
{
    public class PostControllerTests : IAsyncLifetime
    {
        private HttpClient _client;
        private string[] _cookies = new string[0];

        [Fact]
        public async Task GetTenPosts()
        {
            var response = await GetAsync("/api/post/?skip=0&take=10");
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TryToCreatePostWhenUnauthorized()
        {
            var response = await PostAsync("/api/post", new StringContent(JsonSerializer.Serialize(new CreatePostDto
                {
                    Title = "Some title",
                    Content = "Some content"
                }), Encoding.UTF8, Consts.ContentTypes.Json));
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreatePost()
        {
            var loginResponseMessage = await PostAsync("/api/auth/login",
                Helpers.GetStringContent(new LoginDto
                {
                    Name = "kotwica407",
                    Password = "password",
                    RememberMe = true
                }));

            var response = await PostAsync("/api/post",
                Helpers.GetStringContent(new CreatePostDto
                {
                    Title = "Some title",
                    Content = "Some content"
                }));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        public async Task InitializeAsync()
        {
            _client = await Helpers.InitAsync();
            await PostAsync("/api/auth/register",
                new StringContent(JsonSerializer.Serialize(new UserRegisterDto
                    {
                        Name = "kotwica407",
                        Email = "kotwica407@gmail.com",
                        Password = "password",
                        Description = "Some description"
                    }),
                    Encoding.UTF8,
                    Consts.ContentTypes.Json));
        }

        public Task DisposeAsync()
        {
            _client.Dispose();
            return Task.CompletedTask;
        }

        private void UpdateCookies(HttpResponseMessage response)
        {
            if (response.Headers.Contains(HeaderNames.SetCookie))
            {
                var cookies = response.Headers.GetValues(HeaderNames.SetCookie);
                _cookies = cookies.ToArray();
            }
        }

        private async Task<HttpResponseMessage> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_client.BaseAddress, url));

            foreach (string cookie in _cookies)
            {
                request.Headers.Add(HeaderNames.Cookie, cookie);
            }

            var response = await _client.SendAsync(request);
            UpdateCookies(response);

            return response;
        }

        private async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(_client.BaseAddress, url));
            request.Content = content;

            foreach (string cookie in _cookies)
            {
                request.Headers.Add(HeaderNames.Cookie, cookie);
            }

            var response = await _client.SendAsync(request);
            UpdateCookies(response);

            return response;
        }
    }
}