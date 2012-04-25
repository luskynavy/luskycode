using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace TestProcess
{
    class Program
    {
        //http://stackoverflow.com/questions/394816/how-to-get-parent-process-in-net-in-managed-way
        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                //first call is slow then it's ok
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            Process p = null;
            try
            {
                p = Process.GetProcessById((int)parentId.NextValue());
            }
            catch (Exception)
            {
            }

            return p;
        }

        //http://stackoverflow.com/questions/394816/how-to-get-parent-process-in-net-in-managed-way
        /// <summary>
        /// A utility class to determine a process parent.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ParentProcessUtilities
        {
            // These members must match PROCESS_BASIC_INFORMATION
            internal IntPtr Reserved1;
            internal IntPtr PebBaseAddress;
            internal IntPtr Reserved2_0;
            internal IntPtr Reserved2_1;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;

            [DllImport("ntdll.dll")]
            private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

            /// <summary>
            /// Gets the parent process of the current process.
            /// </summary>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess()
            {
                return GetParentProcess(Process.GetCurrentProcess().Handle);
            }

            /// <summary>
            /// Gets the parent process of specified process.
            /// </summary>
            /// <param name="id">The process id.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(int id)
            {
                Process process = Process.GetProcessById(id);
                return GetParentProcess(process.Handle);
            }

            /// <summary>
            /// Gets the parent process of a specified process.
            /// </summary>
            /// <param name="handle">The process handle.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(IntPtr handle)
            {
                ParentProcessUtilities pbi = new ParentProcessUtilities();
                int returnLength;
                int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
                if (status != 0)
                    throw new Win32Exception(status);

                try
                {
                    return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
                }
                catch (ArgumentException)
                {
                    // not found
                    return null;
                }
            }
        }

        //http://www.rhyous.com/2010/04/30/how-to-get-the-parent-process-that-launched-a-c-application/
        private static Process GetParentProcess(int iCurrentPid)
        {
            int iParentPid = 0;
            //int iCurrentPid = Process.GetCurrentProcess().Id;

            IntPtr oHnd = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

            if (oHnd == IntPtr.Zero)
                return null;

            PROCESSENTRY32 oProcInfo = new PROCESSENTRY32();

            oProcInfo.dwSize =
            (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(PROCESSENTRY32));

            if (Process32First(oHnd, ref oProcInfo) == false)
                return null;

            do
            {
                if (iCurrentPid == oProcInfo.th32ProcessID)
                    iParentPid = (int)oProcInfo.th32ParentProcessID;
            }
            while (iParentPid == 0 && Process32Next(oHnd, ref oProcInfo));



            if (iParentPid > 0)
                try
                {
                    return Process.GetProcessById(iParentPid);
                }
                catch (Exception)
                {
                    return null;
                }
            //return Process.GetProcessById(iParentPid);
            else
                return null;
        }

        static uint TH32CS_SNAPPROCESS = 2;

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        };

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
        static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);


        static void Main(string[] args)
        {
            Process[] localAll = System.Diagnostics.Process.GetProcesses();

            foreach (Process p in localAll)
            {
                //Method #1
                //works but first call is slow then it's ok
                //string name = FindIndexedProcessName(p.Id);
                //Process parent = FindPidFromIndexedProcessName(name);
                //Process processFromProcessName = FindPidFromIndexedProcessName(p.ProcessName);

                //Method #2
                //Dont'always work
                //Process parent = ParentProcessUtilities.GetParentProcess(p.Id);
                
                //Method #3
                //Work and is fast
                Process parent = GetParentProcess(p.Id);

                /*if ((parent != null) & (parent2 != null))
                {
                    if (parent.Id != parent2.Id)
                    {
                        int x = 0;
                    }
                }/**/

                Console.WriteLine(p.ProcessName + ":" + p.Id + " parent " + (parent != null ? parent.ProcessName : "")
                    + ":" + (parent != null ? parent.Id : 0));
            }
        }
    }
}
