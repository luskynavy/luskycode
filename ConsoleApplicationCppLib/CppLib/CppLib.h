#pragma once

int return42();

class CppLib
{
public:
	CppLib();
	int Init();
	int WaitForInit();
	int LaunchCommand();
private:
	bool mInitDone = false;
};
