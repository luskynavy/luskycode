#include "pch.h"
#include "DllDelayed.h"


int get_value()
{
	return 42;
}

CppDll::CppDll()
{
    mInitDone = true;
    mCounter = 0;
}

int CppDll::IncrementCounter()
{
    return ++mCounter;
}

int CppDll::Counter()
{
    return mCounter;
}

int CppDll::Return43()
{
    return 43;
}