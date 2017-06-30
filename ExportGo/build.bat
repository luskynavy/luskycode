go build -buildmode=c-archive exportGo.go
rem gcc callGo.c exportgo.a -lWS2_32 -lwinmm

rem build exe
i686-w64-mingw32-gcc callgo.c exportgo.a -lWS2_32 -lwinmm -limm32 -lntdll -lkernel32 -ladvapi32 -lole32 -loleaut32 -lversion -oprintByeFromGo.exe

rem build dll
i686-w64-mingw32-gcc godll.c exportgo.a -lWS2_32 -lwinmm -limm32 -lntdll -lkernel32 -ladvapi32 -lole32 -loleaut32 -lversion -shared -pthread -o goDll.dll