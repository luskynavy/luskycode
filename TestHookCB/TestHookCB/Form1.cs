using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Reflection;

namespace TestHookCB
{
    public partial class Form1 : Form
    {
        private int m_hMouseHook = 0;
        private static Win32.HookProc m_MouseHookProcedure;

        // A callback function which will be called every time a mouse activity detected.        
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == Win32.WM_PASTE)
                {
                    int x = 1;
                }
            }
            // Call next hook
            return Win32.CallNextHookEx(m_hMouseHook, nCode, wParam, lParam);
        }

        public Form1()
        {
            InitializeComponent();

            // Create an instance of HookProc.
            m_MouseHookProcedure = new Win32.HookProc(MouseHookProc);

            // Install hook
            m_hMouseHook = Win32.SetWindowsHookEx(Win32.WH_CALLWNDPROC/*WH_MOUSE_LL*/, m_MouseHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

            // If SetWindowsHookEx fails.
            if (m_hMouseHook == 0)
            {
                // Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                int errorCode = Marshal.GetLastWin32Error();
            }

            // Uninstall hook
            //int retMouse = Win32.UnhookWindowsHookEx(m_hMouseHook);

            // Reset invalid handle
            //m_hMouseHook = 0;
        }
    }
}
