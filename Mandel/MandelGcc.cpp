// Mandel.cpp : définit le point d'entrée pour l'application console.
//

//#include "stdafx.h"

#include <stdio.h>
//#include <tchar.h>

#include "windows.h"
#include "math.h"


double CurrentSecond()
{
	LARGE_INTEGER current;
	QueryPerformanceCounter(&current);
	LONGLONG frequency;
	QueryPerformanceFrequency( (LARGE_INTEGER *)&frequency);
	return (double)current.QuadPart / (double)frequency;

}

#define STEPS_MAX 9000
#define TYPE_CALC double
//#define TYPE_CALC float

int main(int argc, char* argv[])
{
	int steps = STEPS_MAX;
	TYPE_CALC xmin = -2;
	TYPE_CALC ymin = -1;
	TYPE_CALC xmax = 1;
	TYPE_CALC ymax = 1;
	TYPE_CALC xstep = (xmax - xmin) / steps;
	TYPE_CALC ystep = (ymax - ymin) / steps;	

	double starttime = CurrentSecond();
	int total = 0;

	int* mandel = new int[STEPS_MAX * STEPS_MAX];

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
			while (looper < 100 && sqrt((x1 * x1) + (y1 * y1)) < 2)
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

	double time = CurrentSecond() - starttime;

	printf("mandel 500 200 %d\n", mandel[500 + 200 * steps]);
	printf("total %d\n", total);
	printf("time %.3f\n", time);

	return 0;
}

