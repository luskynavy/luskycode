#pragma once

#ifdef DELAYEDLIBRARY_EXPORTS
#define DELAYEDLIBRARY_API __declspec(dllexport)
#else
#define DELAYEDLIBRARY_API __declspec(dllimport)
#endif

extern "C" DELAYEDLIBRARY_API int get_value();
