Dim objDll, str
WScript.Echo "test"
Set objDll = WScript.CreateObject("COMDLL2.MaDll")
WScript.Echo objDll
WScript.Echo objDll.BonjourMonde()