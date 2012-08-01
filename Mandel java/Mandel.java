//import java.lang.Math;

public class Mandel
{
	static int STEPS_MAX = 3000;

	public static double CurrentSecond()
	{
		long time = System.currentTimeMillis();
		return (double)time / 1000.;
		/*LARGE_INTEGER current;
		QueryPerformanceCounter(&current);
		LONGLONG frequency;
		QueryPerformanceFrequency( (LARGE_INTEGER *)&frequency);
		return (double)current.QuadPart / (double)frequency;*/

	}

	public static void main(String[] args)
    {
		int steps = STEPS_MAX;
		/**/double xmin = -2;
		/**/double ymin = -1;
		/**/double xmax = 1;
		/**/double ymax = 1;
		/**/double xstep = (xmax - xmin) / steps;
		/**/double ystep = (ymax - ymin) / steps;	

		double starttime = CurrentSecond();
		int total = 0;

		//int* mandel = new int[STEPS_MAX * STEPS_MAX];
		int mandel[] = new int[STEPS_MAX * STEPS_MAX];

		int xmandel = 0;
		int ymandel = 0;
		for (/**/double x = xmin; xmandel < steps; x += xstep, xmandel++)
		{		
			ymandel = 0;
			for (/**/double y = ymin; ymandel < steps; y += ystep, ymandel++)
			{
				/**/double x1 = 0;
				/**/double y1 = 0;
				int looper = 0;
				while (looper < 100 && java.lang.Math.sqrt((x1 * x1) + (y1 * y1)) < 2)
				{
					looper++;
					/**/double xx = (x1 * x1) - (y1 * y1) + x;
					y1 = 2 * x1 * y1 + y;
					x1 = xx;
				}
				total += looper;
				mandel[xmandel + ymandel * steps] = looper;
			}
		}

		double time = CurrentSecond() - starttime;

		System.out.println("mandel 500 200 " + mandel[500 + 200 * steps]);
		System.out.println("total " + total);
		System.out.println("time "+ time);
		
		/*long debut = System.currentTimeMillis();
		        
		new Mandel().go(args);
		for(int i = 0; i < 1000000; i++)
		{
		}
		
		long fin = System.currentTimeMillis();
		System.out.println("temps " + (fin-debut) + "ms");
		
        System.out.println("Mandel finished");*/
    }
	
	void go(String[] args)
    {
    
	}
}