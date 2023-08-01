//From https://openclassrooms.com/fr/courses/5641591-testez-votre-application-c

namespace ConsoleAppCore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ihm = new Ihm(new ConsoleDeSortie(), new De(), new FournisseurMeteo());
            ihm.Demarre();
        }
    }
}