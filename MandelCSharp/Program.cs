using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MandelCSharp
{
    using TYPE_CALC = System.Double;
    //using TYPE_CALC = System.Single;

    class Program
    {
        [System.Runtime.InteropServices.DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);

        [System.Runtime.InteropServices.DllImport("KERNEL32")]
        private static extern bool QueryPerformanceFrequency(ref long lpFrequency);


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

        static void Main(string[] args)
        {
            double starttime = CurrentSecond;

            int steps = 3000;
            TYPE_CALC xmin = -2;
            TYPE_CALC ymin = -1;
            TYPE_CALC xmax = 1;
            TYPE_CALC ymax = 1;
            TYPE_CALC xstep = (xmax - xmin) / steps;
            TYPE_CALC ystep = (ymax - ymin) / steps;
            
            int total = 0;

            int[] mandel = new int[steps * steps];

            int xmandel = 0;
            int ymandel = 0;
            for (TYPE_CALC x = xmin; xmandel < steps; x += xstep, xmandel++)
            {
                ymandel = 0;
                for (TYPE_CALC y = ymin; ymandel < steps; y += ystep, ymandel++)
                {
                    TYPE_CALC x1 = 0;
                    TYPE_CALC y1 = 0;
                    int looper = 0;
                    while (looper < 100 && Math.Sqrt((x1 * x1) + (y1 * y1)) < 2)
                    {
                        looper++;
                        TYPE_CALC xx = (x1 * x1) - (y1 * y1) + x;
                        y1 = 2 * x1 * y1 + y;
                        x1 = xx;
                    }
                    total += looper;
                    mandel[xmandel + ymandel * steps] = looper;
                }
            }

            double time = CurrentSecond - starttime;

            Console.WriteLine("mandel  500 200 " + mandel[500 + 200 * steps]);

            Console.WriteLine("total " + total);

            Console.WriteLine("time " + time);
        }
    }
}
