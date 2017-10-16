#include "windows.h"
#include "math.h"
#include <stdio.h>
#include <iostream>

//gcc tests
//23 sec for 27 values in O3 with testVal < pow(2, n)
//8 sec for 27 values in Ofast with testVal < pow(2, n)

//8 sec for 27 vlaues in O3/Ofast with testVal < max

#define TYPE_CALC int
//#define TYPE_CALC double

double CurrentSecond()
{
	LARGE_INTEGER current;
	QueryPerformanceCounter(&current);
	LONGLONG frequency;
	QueryPerformanceFrequency( (LARGE_INTEGER *)&frequency);
	return (double)current.QuadPart / (double)frequency;
}


void printAllSubsets(TYPE_CALC products[], int n, TYPE_CALC wantedSum)
{
	//double max = pow(2, n);
	//for (int testVal = 0; testVal < max; testVal++)
	for (int testVal = 0; testVal < pow(2, n); testVal++)
	{
		TYPE_CALC sum = 0;
		for(int choice = 0; choice < n; choice++)
		{
			int test = (1 << choice);
			if ((test & testVal) == test)
			//if (((1 << choice) & testVal) == (1 << choice))
			{                        
				sum += products[choice];
			}
		}
		
		if (sum == wantedSum)
		{
			for (int j = 0; j < n; j++)
			{
				if (((1 << j) & testVal) == (1 << j))
				{
					std::cout << " " << products[j];
				}
			}
			
			std::cout << "\n";
		}
	}
}			
			// Driver code
int main()
{
    //int arr[] = {1, 2, 3, 4, 5};
	//int sum = 10;
	
	//TYPE_CALC arr[] = {241, 324, 209, 256, 328, 388, 170, 493, 330}; //9 values
	//TYPE_CALC arr[] = {241, 324, 209, 256, 328, 388, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330}; //21 values
	TYPE_CALC arr[] = {241, 324, 209, 256, 328, 388, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330}; //27 values
	//TYPE_CALC arr[] = {241, 324, 209, 256, 328, 388, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330, 170, 493, 330}; //30 values values
	TYPE_CALC sum = 241 + 209 + 330;
	
    int n = sizeof(arr)/sizeof(arr[0]);
	
	double starttime = CurrentSecond();
	
	printAllSubsets(arr, n, sum);
	
	double time = CurrentSecond() - starttime;
	std::cout.precision(3);
	std::cout << "time " << time << "\n";
}
