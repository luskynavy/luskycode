using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

/***************************************************************
 * Mandelbrot with 2 methods : double for or for-Parallel.For  *
****************************************************************/
namespace MandelCSharp
{
    //choose calc type
    using TYPE_CALC = System.Double;
    //using TYPE_CALC = System.Single;

    class Program
    {
        //import time function
        [System.Runtime.InteropServices.DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);

        [System.Runtime.InteropServices.DllImport("KERNEL32")]
        private static extern bool QueryPerformanceFrequency(ref long lpFrequency);

        //time function
        public static double CurrentSecond
        {
            get
            {
                long current = 0;
                QueryPerformanceCounter(ref current);
                long frequency = 0;
                QueryPerformanceFrequency(ref frequency);
                return (double)current / (double)frequency;
            }
        }

        /*private static IEnumerable<TYPE_CALC> Iterate(TYPE_CALC fromInclusive, TYPE_CALC toExclusive, TYPE_CALC step)
        {
            for (TYPE_CALC d = fromInclusive; d < toExclusive; d += step) yield return d;
        }*/

        static void Main(string[] args)
        {
            //method with 2 for
            //start timer
            double starttime = CurrentSecond;

            int steps = 3000;
            int maxLevel = 100;
            TYPE_CALC xmin = -2;
            TYPE_CALC ymin = -1;
            TYPE_CALC xmax = 1;
            TYPE_CALC ymax = 1;
            TYPE_CALC xstep = (xmax - xmin) / steps;
            TYPE_CALC ystep = (ymax - ymin) / steps;
            
            int total = 0;

            int[] mandel = new int[steps * steps];

            int xmandel = 0;            
            //for (TYPE_CALC x = xmin; xmandel < steps; x += xstep, xmandel++)
            for (; xmandel < steps; xmandel++)
            {
                //compute x from xformandel
                TYPE_CALC x = xmin + xmandel * xstep;

                //TYPE_CALC y = ymin;
                int ymandel = 0;
                for (ymandel = 0; ymandel < steps; ymandel++)
                {
                    //compute y from ymandel
                    TYPE_CALC y = ymin + ymandel * ystep;

                    TYPE_CALC x1 = 0;
                    TYPE_CALC y1 = 0;
                    //max level
                    int looper = 0;
                    while (looper < maxLevel && Math.Sqrt((x1 * x1) + (y1 * y1)) < 2)
                    {
                        looper++;
                        TYPE_CALC xx = (x1 * x1) - (y1 * y1) + x;
                        y1 = 2 * x1 * y1 + y;
                        x1 = xx;
                    }
                    total += looper;
                    mandel[xmandel + ymandel * steps] = looper;

                    //y += ystep;
                }
            }

            //end timer
            double time = CurrentSecond - starttime;

            //show results
            Console.WriteLine("double for way:");

            Console.WriteLine("mandel  500 200 " + mandel[500 + 200 * steps]);

            Console.WriteLine("total " + total);

            Console.WriteLine("time " + time);

            Console.WriteLine();


            //method with for and Parallel.For
            //start timer
            starttime = CurrentSecond;

            total = 0;

            mandel = new int[steps * steps];

            //xmandel = 0;
            //for (TYPE_CALC x = xmin; xmandel < steps; x += xstep, xmandel++)
            Parallel.For(0, steps, xformandel => 
            {
                //compute x from xformandel
                TYPE_CALC x = xmin + xformandel * xstep;

                int ymandel = 0;
                for (ymandel = 0; ymandel < steps; ymandel++)
                //Parallel.For(0, steps, yformandel =>                
                {
                    //compute y from yformandel
                    TYPE_CALC y = ymin + ymandel * ystep;

                    TYPE_CALC x1 = 0;
                    TYPE_CALC y1 = 0;
                    //max level
                    int looper = 0;
                    while (looper < maxLevel && Math.Sqrt((x1 * x1) + (y1 * y1)) < 2)
                    {
                        looper++;
                        TYPE_CALC xx = (x1 * x1) - (y1 * y1) + x;
                        y1 = 2 * x1 * y1 + y;
                        x1 = xx;
                    }
                    //equivalent to "total += looper" but with locks between threads for Parallel.For
                    Interlocked.Add(ref total, looper);
                    mandel[xformandel + ymandel * steps] = looper;
                }
                //);
            }
            );
            //end timer
            time = CurrentSecond - starttime;

            //show results
            Console.WriteLine("for and Parallel.For way:");

            Console.WriteLine("mandel  500 200 " + mandel[500 + 200 * steps]);

            Console.WriteLine("total Interlocked " + total);

            //recompute total from array 
            total = 0;
            for (int i = 0; i < mandel.Length; i++)
            {
                total += mandel[i];
            }

            Console.WriteLine("total array " + total);

            Console.WriteLine("time " + time);


            Console.WriteLine("\nPress a key to quit.");
            //wait a key
            Console.ReadKey();
        }
    }
}
