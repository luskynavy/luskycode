﻿namespace ConsoleAppCore
{
    public class ConsoleDeSortie : IConsole
    {
        public void Ecrire(string message)
        {
            Console.Write(message);
        }

        public void EcrireLigne(string message)
        {
            Console.WriteLine(message);
        }
    }
}