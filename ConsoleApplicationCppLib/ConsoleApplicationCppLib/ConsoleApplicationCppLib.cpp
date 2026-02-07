#include <iostream>

#include "CppLib.h"
#include "CppDll.h"

int main()
{
    int z = return42();
    std::cout << "return42() returned " << z << "!\n";

    CppLib cppLib;
	cppLib.Init();
	cppLib.WaitForInit();

    CppDll cppDll;
    cppDll.Init();
    cppDll.WaitForInit();

    std::cout << "CppLib.LaunchCommand returned " << cppLib.LaunchCommand() << "\n";

    std::cout << "CppDll get_value returned " << get_value() << "\n";

    std::cout << "CppDll.LaunchCommand get_value returned " << cppDll.LaunchCommand() << "\n";


}

