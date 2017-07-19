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
            double[] products = { 2.41, 3.24, 2.09, 2.56, 3.28, 3.88, 1.70, 4.93, 3.30};

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
                if (sum == 2.41+2.09+3.3 )
                //if (sum == 13.51)
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
