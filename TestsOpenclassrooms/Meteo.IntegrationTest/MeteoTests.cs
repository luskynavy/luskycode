using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Meteo.IntegrationTest
{
    [TestClass]
    public class MeteoTests
    {
        [TestMethod]
        public async Task ObtientLeTempsQuilFait()
        {
            //https://stackoverflow.com/questions/70093628/integration-test-and-hosting-asp-net-core-6-0-without-startup-class
            // ou https://github.com/dotnet-labs/ApiControllerIntegrationTests/
            try
            {
                //A utiliser avec une web api sans class Program et Main donc en top level statements
                //var webAppFactory = new WebApplicationFactory<Program>();

                //A utiliser avec une web api avec un namespace Meteo.WebApi et la class Program qui contient la fonction Main
                var webAppFactory = new WebApplicationFactory<Meteo.WebApi.Program>();

                HttpClient client = webAppFactory.CreateDefaultClient();

                string result = await client.GetStringAsync("/api/meteo");
                Assert.AreEqual("Soleil", result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}