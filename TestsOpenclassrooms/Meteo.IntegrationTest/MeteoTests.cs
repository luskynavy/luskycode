using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.ComponentModel.Design;

namespace Meteo.IntegrationTest
{
    [TestClass]
    public class MeteoTests
    {
        [TestMethod]
        public async Task ObtientLeTempsQuilFait()
        {
            //Startup ??? Venant de .net 5 ou plus ancien
            /*var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                string result = await client.GetStringAsync("/api/meteo");
                Assert.AreEqual("Soleil", result);
            }*/
            /*
            //https://stackoverflow.com/questions/74476594/asp-net-core-6-0-web-api-integration-testing-after-migration-to-minimal-hosting
            //Ne s'exècute pas
            var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // set up servises
                });
            });*/

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            try
            {
                //var client = app.GetTestServer().CreateClient();
                var client = app.GetTestClient();

                //var client = application.CreateClient();
                string result = await client.GetStringAsync("/api/meteo");
                Assert.AreEqual("Soleil", result);
            }
            catch (Exception)
            {
                throw;
            }

            //a voir : https://gist.github.com/davidfowl/0e0372c3c1d895c3ce195ba983b1e03d
        }
    }
}