//From https://openclassrooms.com/fr/courses/5641591-testez-votre-application-c

using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

/*
 * Création de la bdd
CREATE TABLE[dbo].InfosMeteo
(

    [Valeur] VARCHAR(10) NOT NULL,

    [Date] Datetime NOT NULL
)
* Valeur de test
INSERT INTO dbo.InfosMeteo values ('Soleil', '20230802')
*/

namespace ConsoleAppCore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Version avec météo aléatoire
            //var ihm = new Ihm(new ConsoleDeSortie(), new De(), new FournisseurMeteo());
            //Version avec météo venant d'une bdd
            var ihm = new Ihm(new ConsoleDeSortie(), new De(), new MeteoRepository(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestsOpenclassrooms;Integrated Security=True;"));
            ihm.Demarre();
        }
    }
}