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

//naive recursion
static unsigned long FiboRecur(int n)
{
	if (n == 0)
	{
		return 0;
	}
	if (n == 1)
	{
		return 1;
	}
	return (FiboRecur(n - 1) + FiboRecur(n - 2));
}

static unsigned long fibRt (int n , unsigned long a, unsigned long b)
{
	if (n == 1)
	{
		return a;
	}

	return fibRt ( n - 1, a + b , a );
}

static unsigned long fibTerminal (int n )
{
	if (n == 0)
	{
		return 0;
	}
	return fibRt ( n , 1 , 0 );
}

static unsigned long FiboIter(int n)
{
	if (n == 0)
	{
		return 0;
	}
	if (n == 1)
	{
		return 1;
	}

	unsigned long n1 = 1;
	unsigned long n2 = 0;
	unsigned long fiboN = 0;
	for (int i = 2; i <= n; i++)
	{
		fiboN = n1 + n2;
		n2 = n1;
		n1 = fiboN;
	}
	return fiboN;
}

int main(int argc, char* argv[])
{	
	int n = 40;	
	if (argc >= 1)
	{
		n = atoi(argv[1]);
	}
	
	double startTime = CurrentSecond();
	printf("FiboIter %u", FiboIter(n));
	double time = CurrentSecond() - startTime;
	printf(" %.3fms\n", time);
	
	startTime = CurrentSecond();
	printf("fibTerminal %u", fibTerminal(n));
	time = CurrentSecond() - startTime;
	printf(" %.3fms\n", time);
	
	if (n < 45)
	{
		startTime = CurrentSecond();
		printf("FiboRecur %u", FiboRecur(n));
		time = CurrentSecond() - startTime;
		printf(" %.3fms\n", time);
	}

	return 0;
}

