using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ContentAggregator.IntegrationTests.Common
{
    public static class Helpers
    {
        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            string? projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            string applicationBasePath = AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName,
                        projectName,
                        $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                }
            } while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        internal static async Task<HttpClient> InitAsync()
        {
            IHostBuilder hostBuilder = new HostBuilder()
               .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseEnvironment("Testing");
                    string startupProjectDir = GetProjectPath("", typeof(Startup).GetTypeInfo().Assembly);
                    IConfigurationRoot config = new ConfigurationBuilder()
                       .SetBasePath(startupProjectDir)
                       .AddJsonFile("appsettings.Testing.json")
                       .Build();
                    webHost.UseConfiguration(config);
                    webHost.UseStartup<Startup>();
                });

            // Build and start the IHost
            IHost host = await hostBuilder.StartAsync();

            return host.GetTestClient();
        }

        internal static StringContent GetStringContent<T>(T dto) => new StringContent(JsonSerializer.Serialize(dto),
            Encoding.UTF8,
            Consts.ContentTypes.Json);
    }
}