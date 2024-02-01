using ConsoleAppCore;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Meteo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeteoController : ControllerBase
    {
        private const string _connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestsOpenclassrooms;Integrated Security=True;";

        [HttpGet]
        public ActionResult<string> Get()
        {
            var repository = new MeteoRepository(_connectionstring);
            var resultat = repository.QuelTempsFaitIl(new DateTime(2023, 8, 2));
            return Ok(resultat.ToString());
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] DateType maDate)
        {
            var repository = new MeteoRepository(_connectionstring);
            try
            {
                var resultat = repository.QuelTempsFaitIl(new DateTime(maDate.Annee, maDate.Mois, maDate.Jour));
                return Ok(resultat.ToString());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}