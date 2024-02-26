using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDeclaratif
{
    internal static class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        // Disable the AutoDeclaratif.exe.config file
        private static Assembly CurrentDomain_AssemblyResolve(object sender,
            ResolveEventArgs args)
        {
            if (new AssemblyName(args.Name).Name == "SQLitePCLRaw.core")
                return Assembly.LoadFrom(Path.Combine(Application.StartupPath, "SQLitePCLRaw.core.dll"));
            throw new Exception();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}