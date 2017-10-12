using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseTR
{
    class Program
    {
        static void Main(string[] args)
        {
            //double[] produits = { 2.24, .81, 1.93, 3.14, 1.32, 1.89, 3.67, 1.61 };
            double[] products = { 3.57, 2.95, 2.79, 1.13, 4.91, 1.63, 4.10, 1, 1.41, 1.46, 2.09};
            //double[] products = { 357, 295, 279, 113, 491, 163, 410, 100, 141, 146, 209 };

            for (int testVal = 0; testVal < Math.Pow(2, products.Length); testVal++)
            {
                double sum = 0;
                for(int choice = 0; choice < products.Length; choice++)
                {
                    if (((1 << choice) & testVal) == (1 << choice))
                    {                        
                        sum += products[choice];
                    }
                }
                //if (sum == 2.41+2.09+3.3 )
                if (Math.Abs(sum - 21.13) < 1e-5)
                //if (sum == 2113)
                {
                    for (int j = 0; j < products.Length; j++)
                    {
                        if (((1 << j) & testVal) == (1 << j))
                        {
                            Console.Out.Write(" " + products[j]);                           
                        }
                    }
                    Console.Out.WriteLine();
                    int x=1;
                }
            }

        }
    }
}
