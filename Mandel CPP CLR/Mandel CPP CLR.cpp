// Mandel CPP CLR.cpp : fichier projet principal.

#include "stdafx.h"
#include "windows.h"

using namespace System;

#define TYPE_CALC double;
//using TYPE_CALC = System.Double;
//using TYPE_CALC = System.Single;

double CurrentSecond()
{
	LARGE_INTEGER current;
	QueryPerformanceCounter(&current);
	LONGLONG frequency;
	QueryPerformanceFrequency( (LARGE_INTEGER *)&frequency);
	return (double)current.QuadPart / (double)frequency;

}


int main(array<System::String ^> ^args)
{
	double starttime = CurrentSecond();

	int steps = 3000;
	double xmin = -2;
	double ymin = -1;
	double xmax = 1;
	double ymax = 1;
	double xstep = (xmax - xmin) / steps;
	double ystep = (ymax - ymin) / steps;

	int total = 0;

	int* mandel = new int[steps * steps];

	int xmandel = 0;
	int ymandel = 0;
	for (double x = xmin; xmandel < steps; x += xstep, xmandel++)
	{
		ymandel = 0;
		for (double y = ymin; ymandel < steps; y += ystep, ymandel++)
		{
			double x1 = 0;
			double y1 = 0;
			int looper = 0;
			while (looper < 100 && Math::Sqrt((x1 * x1) + (y1 * y1)) < 2)
			{
				looper++;
				double xx = (x1 * x1) - (y1 * y1) + x;
				y1 = 2 * x1 * y1 + y;
				x1 = xx;
			}
			total += looper;
			mandel[xmandel + ymandel * steps] = looper;
		}
	}

	double time = CurrentSecond() - starttime;

	Console::WriteLine("mandel  500 200 " + mandel[500 + 200 * steps]);

	Console::WriteLine("total " + total);

	Console::WriteLine("time " + time);

	return 0;
}
