#include "stdafx.h"
#include "Hook.h"

#include "explorer1.h"

extern CExplorer1* g_CExplorer1;

HHOOK Hook::hHook = NULL;

Hook::Hook()
{
	hHook = SetWindowsHookEx(WH_KEYBOARD_LL, (HOOKPROC)HookProc, 0, 0);
}

Hook::~Hook()
{
 	if (hHook)
 	{
 		UnhookWindowsHookEx(hHook);
 		hHook = NULL;
 	}	
}

__declspec(dllexport) LRESULT CALLBACK Hook::HookProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	if (nCode>=0)
	{
		if (!HookCallback(nCode,wParam,lParam))
			return(-1);
	}
	if (hHook)
	{
		return(CallNextHookEx(hHook,nCode,wParam,lParam));
	}
	else
	{
		return(CallNextHookEx(NULL,nCode,wParam,lParam));
	}    // same as return(0);
	return(0);
}

bool Hook::HookCallback(int nCode, WPARAM wParam, LPARAM lParam)
{
	if (nCode == HC_ACTION && wParam == WM_KEYDOWN && g_CExplorer1)
	{
		KBDLLHOOKSTRUCT *hookstruct = ((KBDLLHOOKSTRUCT*)lParam);

		//MyCallback(hookstruct->vkCode);		
 		if (hookstruct->vkCode == VK_MEDIA_PLAY_PAUSE)
 		{
			g_CExplorer1->Navigate(L"javascript:top.player.onPlayPause();", NULL, NULL, NULL, NULL);
 		}

		if (hookstruct->vkCode == VK_MEDIA_PREV_TRACK)
		{
			g_CExplorer1->Navigate(L"javascript:top.player.onSkip(true);", NULL, NULL, NULL, NULL);
		}

		if (hookstruct->vkCode == VK_MEDIA_NEXT_TRACK)
		{
			g_CExplorer1->Navigate(L"javascript:top.player.onSkip(true);", NULL, NULL, NULL, NULL);
		}

		if (hookstruct->vkCode == VK_VOLUME_UP)
		{
			g_CExplorer1->Navigate(L"javascript:top.player.onPlayPause();", NULL, NULL, NULL, NULL);
		}

		if (hookstruct->vkCode == VK_VOLUME_UP)
		{
			g_CExplorer1->Navigate(L"javascript:top.player.onPlayPause();", NULL, NULL, NULL, NULL);
		}
	}
	

	return true;
}