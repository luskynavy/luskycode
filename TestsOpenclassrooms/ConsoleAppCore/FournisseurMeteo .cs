﻿namespace ConsoleAppCore
{
    public class FournisseurMeteo : IFournisseurMeteo
    {
        private readonly Random _random;

        public FournisseurMeteo()
        {
            _random = new Random();
        }

        public Meteo QuelTempsFaitIl(DateTime dateSouhaitee)
        {
            var tirage = _random.Next(0, 21);
            if (tirage < 10)
                return Meteo.Soleil;
            if (tirage < 20)
                return Meteo.Pluie;
            return Meteo.Tempete;
        }
    }
}