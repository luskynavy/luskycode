using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text;

namespace Meteo.IntegrationTest
{
    [TestClass]
    public class MeteoTests
    {
        private HttpClient _client;

        [TestInitialize]
        public void TestSetup()
        {
            //https://stackoverflow.com/questions/70093628/integration-test-and-hosting-asp-net-core-6-0-without-startup-class
            // ou https://github.com/dotnet-labs/ApiControllerIntegrationTests/

            //A utiliser avec une web api sans class Program et Main donc en top level statements
            //var webAppFactory = new WebApplicationFactory<Program>();

            //A utiliser avec une web api avec un namespace Meteo.WebApi et la class Program qui contient la fonction Main
            var webAppFactory = new WebApplicationFactory<Meteo.WebApi.Program>();

            _client = webAppFactory.CreateDefaultClient();
        }

        [TestMethod]
        public async Task ObtientLeTempsQuilFait()
        {
            try
            {
                string result = await _client.GetStringAsync("/api/meteo");
                Assert.AreEqual("Soleil", result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public async Task ObtientLeTempsQuilFaitEnPostDateNonPresente()
        {
            try
            {
                //date non présente en base
                var obj = new
                {
                    annee = 2023,
                    mois = 8,
                    jour = 12
                };
                HttpContent content = JsonContent.Create(obj);

                var response = await _client.PostAsync("/api/meteo", content);
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public async Task ObtientLeTempsQuilFaitEnPostDateImpossible()
        {
            try
            {
                //date impossible
                var obj = new
                {
                    annee = 2023,
                    mois = 31,
                    jour = 31
                };
                HttpContent content = JsonContent.Create(obj);

                var response = await _client.PostAsync("/api/meteo", content);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public async Task ObtientLeTempsQuilFaitEnPost()
        {
            try
            {
                //date correcte
                var obj = new
                {
                    annee = 2023,
                    mois = 8,
                    jour = 2
                };
                HttpContent content = JsonContent.Create(obj);

                var response = await _client.PostAsync("/api/meteo", content);
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.AreEqual("Soleil", responseBody);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}