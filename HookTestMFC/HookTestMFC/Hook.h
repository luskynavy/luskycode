#pragma once

class Hook
{
public:
	Hook();
	~Hook();

	void* MyCallback(DWORD);

public:
	static HHOOK hHook;
	static bool HookCallback(int nCode, WPARAM wParam, LPARAM lParam);	
	static __declspec(dllexport) LRESULT CALLBACK HookProc(int nCode, WPARAM wParam, LPARAM lParam);
};