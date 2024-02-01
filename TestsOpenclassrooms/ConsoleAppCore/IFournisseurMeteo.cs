namespace ConsoleAppCore
{
    public interface IFournisseurMeteo
    {
        Meteo? QuelTempsFaitIl(DateTime dateSouhaitee);
    }
}