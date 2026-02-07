#pragma once

#ifdef DLLDELAYED_EXPORTS
#define DELAYED_API __declspec(dllexport)
#else
#define DELAYED_API __declspec(dllimport)
#endif

extern "C" DELAYED_API int get_value();

class DELAYED_API CppDll
{
public:
	CppDll();
	int IncrementCounter();
	int Counter();
	int Return43();
private:
	bool mInitDone = false;
	int mCounter = 0;
};