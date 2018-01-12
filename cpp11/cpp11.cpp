// cpp11.cpp : Defines the entry point for the console application.
//


#include "stdafx.h"

#include <iostream>
#include <vector>
#include <string>

#include <chrono>
#include <future>
#include <thread>

int arrayAsync[30];

int sum(int imax)
{
	/*int total = 0;
	for (int i = 0; i < 100 - imax; i++)
	{
		total += i;
	}
	
	arrayAsync[imax] = total;*/
	int r = (rand() % 5);
	
	std::this_thread::sleep_for(std::chrono::milliseconds(1000 * r));

	std::cout << r << ' ';
	return imax;
}


int main()
{
	int x = 1'000'000;
	std::cout << x << ' ' << 1'000'000 << '\n';
	
	std::string s5 = "5";
	int t{ stoi(s5) };

	std::cout << t << '\n';

	auto i = 1;
	auto d = 2.3;

	int j = 3;
	decltype(j) k = 0;

	int a[] = { 0, 1, 2, 3, 4, 5 };
	for (auto n : a)	
	{
		std::cout << n << ' ';
	}

	std::cout << '\n';

#ifdef _MSC_VER
	for each(auto n in a)
	{
		std::cout << n << ' ';
	}

	std::cout << '\n';
#endif

	std::vector< int > v{ 1, 2, 3, 4 };

#ifdef _MSC_VER
	for each(auto n in v)
	{
		std::cout << n << ' ';
	}

	std::cout << '\n';
#endif

	for(auto n : v)
	{
		std::cout << n << ' ';
	}

	std::cout << '\n';

	std::string s = "string";	
	for (auto c : s)
	{
		std::cout << c << ' ';
	}

	std::cout << '\n';	

	char s2[] = "char[]";
	for (auto c : s2)
	{
		std::cout << c << ' ';
	}
	
	std::cout << '\n';

	srand((unsigned)time(NULL));
	
	std::vector<std::future<int>> futures;

	auto start = std::chrono::steady_clock::now();
	for (int id = 0; id <= sizeof(arrayAsync) / sizeof(int); id++) {		
		futures.push_back(std::async(sum, id));
	}
	
	for (auto &e : futures)
	{
		/*std::cout << */e.get()/* << ' '*/;
	}

	auto end = std::chrono::steady_clock::now();

	std::cout << '\n';

	auto diff = end - start;
	std::cout << std::chrono::duration <double, std::milli>(diff).count() << " ms" << '\n';
	
	//wait for a key
	system("pause");

	return 0;
}

