#pragma once

#ifdef CPPDLL_EXPORTS
#define CPPDLL_API __declspec(dllexport)
#else
#define CPPDLL_API __declspec(dllimport)
#endif

extern "C" CPPDLL_API int get_value();

class CPPDLL_API CppDll
{
public:
	CppDll();
	int Init();
	int WaitForInit();
	int LaunchCommand();
private:
	bool mInitDone = false;
};
