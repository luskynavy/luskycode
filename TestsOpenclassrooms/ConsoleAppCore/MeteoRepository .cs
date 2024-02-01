using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCore
{
    public class MeteoRepository : IFournisseurMeteo
    {
        private readonly string _connectionstring;

        public MeteoRepository(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public Meteo? QuelTempsFaitIl(DateTime dateSouhaitee)
        {
            using (var connection = new SqlConnection(_connectionstring))
            {
                var ligne = connection.QueryFirstOrDefault("select Valeur from [dbo].InfosMeteo where DATE = Convert(date, @date)", new { date = dateSouhaitee });
                if (ligne != null)
                {
                    return Enum.Parse(typeof(Meteo), ligne.Valeur);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}