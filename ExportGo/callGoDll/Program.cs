using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace callGoDll
{
    class Program
    {

        [System.Runtime.InteropServices.DllImport("goDll")]
        private static extern void PrintBye();

        static void Main(string[] args)
        {
            PrintBye();
        }
    }
}
