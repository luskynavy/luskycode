using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

namespace JangoPlayer2
{
    /*
    [Flags]
    public enum HotKeyModifiers : int
    {
        None    = 0x0,
        Alt     = 0x1,
        Control = 0x2,
        Shift   = 0x4,
        Win     = 0x8
    }

    public class HotKeyArgs : EventArgs
    {
        private Keys            m_Key;
        private HotKeyModifiers m_Modifiers;

        public HotKeyArgs(Keys key, HotKeyModifiers modifiers)
        {
            m_Modifiers = modifiers;
            m_Key       = key;
        }

        public Keys Key
        {
            get 
            { 
                return m_Key; 
            }
        }

        public HotKeyModifiers Modifiers
        {
            get 
            { 
                return m_Modifiers; 
            }
        }
    }

    public class Hotkeys : NativeWindow
    {
        private const int WM_HOTKEY = 0x312;

        public delegate void HotKeyHandler(object sender, HotKeyArgs args);

        private class HotKeyData
        {
            public static HotKeyData Empty = new HotKeyData(Keys.None, HotKeyModifiers.None, IntPtr.Zero);
            
            protected Keys              m_Key;
            protected HotKeyModifiers   m_Modifiers;
            protected IntPtr            m_AtomID;

            public Keys Key
            {
                get
                {
                    return m_Key;
                }
                set
                {
                    m_Key = value;
                }
            }

            public HotKeyModifiers Modifiers
            {
                get
                {
                    return m_Modifiers;
                }
                set
                {
                    m_Modifiers = value;
                }
            }

            public IntPtr AtomID
            {
                get
                {
                    return m_AtomID;
                }
                set
                {
                    m_AtomID = value;
                }
            }

            public HotKeyData(Keys key, HotKeyModifiers modifiers, IntPtr atomId)
            {
                m_Key        = key;
                m_Modifiers  = modifiers;
                m_AtomID     = atomId;
            }

            public override string ToString()
            {
                return m_Modifiers.ToString() + "+" + m_Key.ToString();
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                HotKeyData pOther = obj as HotKeyData;
                if (pOther == null)
                {
                    return false;
                }
                return this.m_AtomID.Equals(pOther.AtomID);
            }
        }

        private List<HotKeyData>    m_Hotkeys;
        private Form                m_Owner;
        public event HotKeyHandler  HotKeyPress;

        public Hotkeys(Form owner)
        {
            AssignHandle(owner.Handle);
            m_Owner              = owner;
            owner.HandleCreated += new EventHandler(owner_HandleCreated);
            m_Hotkeys            = new List<HotKeyData>();
        }

        public void RegisterHotKey(Keys key, HotKeyModifiers modifiers)
        {
            if ((key == Keys.None) || (modifiers == HotKeyModifiers.None))
            {
                // Forbid hotkeys without modifier or key... for now.
                return;
            }
            if (FindHotKey(key, modifiers).Equals(HotKeyData.Empty))
            {
                InternalRegisterHotKey(key, modifiers);
            }
        }

        public void UnregisterHotKey(Keys key, HotKeyModifiers modifiers)
        {
            HotKeyData hkData = FindHotKey(key, modifiers);
            if (!hkData.Equals(HotKeyData.Empty))
            {
                InternalUnregisterHotKey(hkData);
                m_Hotkeys.Remove(hkData);
            }
        }

        private HotKeyData FindHotKey(Keys key, HotKeyModifiers modifiers)
        {
            foreach (HotKeyData hkData in m_Hotkeys)
            {
                if (hkData.Key == key && hkData.Modifiers == modifiers)
                {
                    return hkData;
                }
            }
            return HotKeyData.Empty;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                foreach (HotKeyData hkData in m_Hotkeys)
                {
                    if (hkData.AtomID == m.WParam)
                    {
                        if (HotKeyPress != null)
                        {
                            HotKeyPress(m_Owner, new HotKeyArgs(hkData.Key, hkData.Modifiers));
                        }
                        break;
                    }
                }
            }

            base.WndProc(ref m);
        }

        ~Hotkeys()
        {
            foreach (HotKeyData hkData in m_Hotkeys)
            {
                this.InternalUnregisterHotKey(hkData);
            }
        }

        private void InternalRegisterHotKey(Keys key, HotKeyModifiers modifiers)
        {
            string atomName = string.Empty;
            IntPtr atomId;
            atomName = key.ToString() + modifiers.ToString() + Environment.TickCount.ToString();
            if (atomName.Length > 255)
            {
                atomName = atomName.Substring(0, 255);
            }
            atomId = Win32.GlobalAddAtom(atomName);
            if (atomId == IntPtr.Zero)
            {
                throw new Exception("Impossible d'enregistrer l'atome du raccourci !");
            }
            if (!Win32.RegisterHotKey(Handle, atomId.ToInt32(), (int)modifiers, (int)key))
            {
                Win32.GlobalDeleteAtom(atomId);
                throw new Exception("Impossible d'enregistrer le raccourci !");
            }
            m_Hotkeys.Add(new HotKeyData(key, modifiers, atomId));
        }

        private void InternalUnregisterHotKey(HotKeyData hk)
        {
            Win32.UnregisterHotKey(Handle, hk.AtomID.ToInt32());
            Win32.GlobalDeleteAtom(hk.AtomID);
        }

        private void owner_HandleCreated(object sender, EventArgs e)
        {
            this.AssignHandle(m_Owner.Handle);
        }
    }
    */

    public class Hooks
    {
        // Public events
        public event MouseEventHandler OnMouseActivity;
        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyUp;

        // Data members
        private int m_hMouseHook = 0;
        private int m_hKeyboardHook = 0;
        private static Win32.HookProc m_MouseHookProcedure;
        private static Win32.HookProc m_KeyboardHookProcedure;

        // Default constructor
        public Hooks()
        {
            Start();
        }

        // Constructor with flags to what to hook
        public Hooks(bool InstallMouseHook, bool InstallKeyboardHook)
        {
            Start(InstallMouseHook, InstallKeyboardHook);
        }

        // Destructor, stop hooks
        ~Hooks()
        {
            // Uninstall hooks and do not throw exceptions
            Stop(true, true, false);
        }

        // Installs both mouse and keyboard hooks and starts rasing events        
        public void Start()
        {
            this.Start(true, true);
        }

        // Installs both or one of mouse and/or keyboard hooks and starts rasing events
        public void Start(bool InstallMouseHook, bool InstallKeyboardHook)
        {
            // Install Mouse hook only if it is not installed and must be installed
            if (m_hMouseHook == 0 && InstallMouseHook)
            {
                // Create an instance of HookProc.
                m_MouseHookProcedure = new Win32.HookProc(MouseHookProc);

                // Install hook
                m_hMouseHook = Win32.SetWindowsHookEx(Win32.WH_MOUSE_LL, m_MouseHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

                // If SetWindowsHookEx fails.
                if (m_hMouseHook == 0)
                {
                    // Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();

                    // Do cleanup
                    Stop(true, false, false);

                    // Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    //throw new Win32Exception(errorCode);

                }
            }

            // Install Keyboard hook only if it is not installed and must be installed
            if (m_hKeyboardHook == 0 && InstallKeyboardHook)
            {
                // Create an instance of HookProc.
                m_KeyboardHookProcedure = new Win32.HookProc(KeyboardHookProc);

                // Install hook
                m_hKeyboardHook = Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, m_KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

                // If SetWindowsHookEx fails.
                if (m_hKeyboardHook == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();

                    // Do cleanup
                    Stop(false, true, false);

                    // Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        // Stops monitoring both mouse and keyboard events and rasing events.
        public void Stop()
        {
            this.Stop(true, true, true);
        }

        // Stops monitoring both or one of mouse and/or keyboard events and rasing events.
        public void Stop(bool UninstallMouseHook, bool UninstallKeyboardHook, bool ThrowExceptions)
        {
            // If mouse hook set and must be uninstalled
            if (m_hMouseHook != 0 && UninstallMouseHook)
            {
                // Uninstall hook
                int retMouse = Win32.UnhookWindowsHookEx(m_hMouseHook);

                // Reset invalid handle
                m_hMouseHook = 0;

                // If failed and exception must be thrown
                if (retMouse == 0 && ThrowExceptions)
                {
                    // Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();

                    // Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }

            //if keyboard hook set and must be uninstalled
            if (m_hKeyboardHook != 0 && UninstallKeyboardHook)
            {
                // Uninstall hook
                int retKeyboard = Win32.UnhookWindowsHookEx(m_hKeyboardHook);

                // Reset invalid handle
                m_hKeyboardHook = 0;

                // If failed and exception must be thrown
                if (retKeyboard == 0 && ThrowExceptions)
                {
                    // Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();

                    // Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        // A callback function which will be called every time a mouse activity detected.        
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            // If ok and someone listens to our events
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                // Marshall the data from callback.
                MOUSEINPUT mouseHookStruct = (MOUSEINPUT)Marshal.PtrToStructure(lParam, typeof(MOUSEINPUT));

                // Detect button clicked
                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                switch (wParam)
                {
                    case Win32.WM_LBUTTONDOWN:
                        {
                            button = MouseButtons.Left;
                            break;
                        }

                    case Win32.WM_RBUTTONDOWN:
                        {
                            button = MouseButtons.Right;
                            break;
                        }

                    case Win32.WM_MOUSEWHEEL:
                        {
                            mouseDelta = (short)((mouseHookStruct.mouseData >> 16) & 0xffff);
                            break;
                        }
                }

                // Double clicks
                int clickCount = 0;
                if (button != MouseButtons.None)
                {
                    if (wParam == Win32.WM_LBUTTONDBLCLK || wParam == Win32.WM_RBUTTONDBLCLK)
                    {
                        clickCount = 2;
                    }
                    else
                    {
                        clickCount = 1;
                    }
                }

                // Generate event 
                MouseEventArgs e = new MouseEventArgs(button, clickCount, mouseHookStruct.x, mouseHookStruct.y, mouseDelta);

                // Raise it
                OnMouseActivity(this, e);
            }

            // Call next hook
            return Win32.CallNextHookEx(m_hMouseHook, nCode, wParam, lParam);
        }

        // Set Control, Alt and Shift modifiers.
        private Keys SetModifiers(Keys keyData)
        {
            if ((Win32.GetKeyState(Win32.VK_CONTROL) & 0x8000) == 0x8000)
                keyData |= Keys.Control;

            if ((Win32.GetKeyState(Win32.VK_MENU) & 0x8000) == 0x8000)
                keyData |= Keys.Alt;

            if ((Win32.GetKeyState(Win32.VK_SHIFT) & 0x8000) == 0x8000)
                keyData |= Keys.Shift;

            return keyData;
        }

        // A callback function which will be called every time a keyboard activity detected.
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // Indicates if any of underlaing events set e.Handled flag
            bool handled = false;

            // It was ok and someone listens to events
            if ((nCode >= 0) && (KeyDown != null || KeyUp != null || KeyPress != null))
            {
                // Read structure KeyboardHookStruct at lParam
                KEYBDINPUT MyKeyboardHookStruct = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));


                // Raise KeyDown
                if (KeyDown != null && (wParam == Win32.WM_KEYDOWN || wParam == Win32.WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.wVk;

                    keyData = SetModifiers(keyData);

                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyDown(this, e);
                    handled = handled || e.Handled;
                }

                // Raise KeyPress
                if (KeyPress != null && wParam == Win32.WM_KEYDOWN)
                {
                    bool isDownShift = ((Win32.GetKeyState(Win32.VK_SHIFT) & 0x80) == 0x80 ? true : false);
                    bool isDownCapslock = (Win32.GetKeyState(Win32.VK_CAPITAL) != 0 ? true : false);
                    byte[] keyState = new byte[256];
                    Win32.GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (Win32.ToAscii(MyKeyboardHookStruct.wVk, MyKeyboardHookStruct.wScan, keyState, inBuffer, MyKeyboardHookStruct.dwFlags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        KeyPressEventArgs e = new KeyPressEventArgs(key);
                        KeyPress(this, e);
                        handled = handled || e.Handled;
                    }
                }

                // Raise KeyUp
                if (KeyUp != null && (wParam == Win32.WM_KEYUP || wParam == Win32.WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.wVk;

                    keyData = SetModifiers(keyData);

                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyUp(this, e);
                    handled = handled || e.Handled;
                }

            }

            // If event handled in application do not handoff to other listeners
            if (handled)
            {
                return 1;
            }
            else
            {
                return Win32.CallNextHookEx(m_hKeyboardHook, nCode, wParam, lParam);
            }
        }
    }
}
