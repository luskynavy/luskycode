using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HookTest
{
    [StructLayout(LayoutKind.Sequential)]
    public class MOUSEHOOK
    {
        public int      x;
        public int      y; 
        public int      hwnd;
        public int      wHitTestCode;
        public int      dwExtraInfo;
    }

    [StructLayout( LayoutKind.Sequential )]
	public struct MOUSEINPUT 
    {
        public int      x;
        public int      y; 
		public uint     mouseData;
		public uint     dwFlags;
		public uint     time;
		public IntPtr   dwExtraInfo;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct KEYBDINPUT 
    {
		public ushort   wVk;
		public ushort   wScan;
		public uint     dwFlags;
		public uint     time;
		public IntPtr   dwExtraInfo;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct HARDWAREINPUT 
    {
		public uint     uMsg;
		public ushort   wParamL;
		public ushort   wParamH;
	}

	[StructLayout( LayoutKind.Explicit )]
	public struct INPUT 
    {
		[FieldOffset( 0 )]
		public int type;
		[FieldOffset( 4 )]
		public MOUSEINPUT mi;
		[FieldOffset( 4 )]
		public KEYBDINPUT ki;
		[FieldOffset( 4 )]
		public HARDWAREINPUT hi;
	}

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HANDLE
    {
        public int      dbch_size;
        public int      dbch_devicetype;
        public int      dbch_reserved;
        public IntPtr   dbch_handle;
        public IntPtr   dbch_hdevnotify;
        public Guid     dbch_eventguid;
        public long     dbch_nameoffset;
        public byte     dbch_data;
        public byte     dbch_data1;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_VOLUME
    {
        public int      dbcv_size;
        public int      dbcv_devicetype;
        public int      dbcv_reserved;
        public int      dbcv_unitmask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

	public static class Win32 
    {
        // Input devices
		public const int  INPUT_MOUSE                   = 0;
		public const int  INPUT_KEYBOARD                = 1;
		public const int  INPUT_HARDWARE                = 2;

        // Keyboard
		public const uint KEYEVENTF_EXTENDEDKEY         = 0x0001;
		public const uint KEYEVENTF_KEYUP               = 0x0002;
		public const uint KEYEVENTF_UNICODE             = 0x0004;
		public const uint KEYEVENTF_SCANCODE            = 0x0008;

        // Pad / Extended mouse
		public const uint XBUTTON1                      = 0x0001;
		public const uint XBUTTON2                      = 0x0002;

        // Hooks
        public const int  WH_KEYBOARD                   = 2;
        public const int  WH_MOUSE                      = 7;
        public const int  WH_KEYBOARD_LL                = 13;
        public const int  WH_MOUSE_LL                   = 14;

        // Keyboard state
        public const byte VK_SHIFT                      = 0x10;
        public const byte VK_CONTROL                    = 0x11;
        public const byte VK_MENU                       = 0x12;
        public const byte VK_CAPITAL                    = 0x14;
        public const byte VK_NUMLOCK                    = 0x90;

        // Keyboard messages
        public const int  WM_KEYDOWN                    = 0x100;
        public const int  WM_KEYUP                      = 0x101;
        public const int  WM_SYSKEYDOWN                 = 0x104;
        public const int  WM_SYSKEYUP                   = 0x105;

        // Mouse messages
        public const int  WM_MOUSEMOVE                  = 0x200;
        public const int  WM_LBUTTONDOWN                = 0x201;
        public const int  WM_RBUTTONDOWN                = 0x204;
        public const int  WM_MBUTTONDOWN                = 0x207;
        public const int  WM_LBUTTONUP                  = 0x202;
        public const int  WM_RBUTTONUP                  = 0x205;
        public const int  WM_MBUTTONUP                  = 0x208;
        public const int  WM_LBUTTONDBLCLK              = 0x203;
        public const int  WM_RBUTTONDBLCLK              = 0x206;
        public const int  WM_MBUTTONDBLCLK              = 0x209;
        public const int  WM_MOUSEWHEEL                 = 0x20A;

        // Mouse events
		public const uint MOUSEEVENTF_MOVE              = 0x0001;
		public const uint MOUSEEVENTF_LEFTDOWN          = 0x0002;
		public const uint MOUSEEVENTF_LEFTUP            = 0x0004;
		public const uint MOUSEEVENTF_RIGHTDOWN         = 0x0008;
		public const uint MOUSEEVENTF_RIGHTUP           = 0x0010;
		public const uint MOUSEEVENTF_MIDDLEDOWN        = 0x0020;
		public const uint MOUSEEVENTF_MIDDLEUP          = 0x0040;
		public const uint MOUSEEVENTF_XDOWN             = 0x0080;
		public const uint MOUSEEVENTF_XUP               = 0x0100;
		public const uint MOUSEEVENTF_WHEEL             = 0x0800;
		public const uint MOUSEEVENTF_VIRTUALDESK       = 0x4000;
		public const uint MOUSEEVENTF_ABSOLUTE          = 0x8000;

        // Window states
        public const int  SW_HIDE                       = 0;
        public const int  SW_SHOWNORMAL                 = 1;
        public const int  SW_SHOWMINIMIZED              = 2;
        public const int  SW_SHOWMAXIMIZED              = 3;
        public const int  SW_SHOWNOACTIVATE             = 4;
        public const int  SW_RESTORE                    = 9;
        public const int  SW_SHOWDEFAULT                = 10;

        // File modes
        public const uint GENERIC_READ                  = 0x80000000;
        public const uint FILE_SHARE_READ               = 0x00000001;
        public const uint FILE_SHARE_WRITE              = 0x00000002;
        public const uint OPEN_EXISTING                 = 0x00000003;
        public const uint FILE_ATTRIBUTE_NORMAL         = 128;
        public const uint FILE_FLAG_BACKUP_SEMANTICS    = 0x02000000;

        // Device constants
        public const int  DBT_DEVTYP_DEVICEINTERFACE    = 5;
        public const int  DBT_DEVTYP_HANDLE             = 6;
        public const int  BROADCAST_QUERY_DENY          = 0x424D5144;
        public const int  WM_DEVICECHANGE               = 0x0219;
        public const int  DBT_DEVICEARRIVAL             = 0x8000;
        public const int  DBT_DEVICEQUERYREMOVE         = 0x8001;
        public const int  DBT_DEVICEREMOVECOMPLETE      = 0x8004;
        public const int  DBT_DEVTYP_VOLUME             = 0x00000002;

        // Shell constants
        public const uint SHGFI_ICON                    = 0x100;
        public const uint SHGFI_LARGEICON               = 0x0;
        public const uint SHGFI_SMALLICON               = 0x1;

        // Invalid handle
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        // Event for keybord or mouse hook
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
/*
        // DLL Imports
		[DllImport("user32.dll", SetLastError = true )]
		public static extern uint   SendInput( uint nInputs, INPUT[] pInputs, int cbSize );

        [DllImport("user32.dll")]
		public static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        public static extern bool   ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool   ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int    SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool   IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool   IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint   UnregisterDeviceNotification(IntPtr hHandle);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr CreateFile( string FileName, uint DesiredAccess, uint ShareMode, uint SecurityAttributes, uint CreationDisposition, uint FlagsAndAttributes, int hTemplateFile );

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool   CloseHandle( IntPtr hObject );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GlobalAddAtom(string lpString);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GlobalDeleteAtom(IntPtr nAtom);
*/
        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern int SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess,
            bool bInheritHandle,
            uint dwThreadId
        );

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, uint fuState);

        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int vKey);


	}
}