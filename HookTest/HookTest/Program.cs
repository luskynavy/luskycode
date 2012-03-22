using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HookTest
{
    static partial class  Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Toto tot = new Toto();
            tot.ClearCache();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());            
        }
    }
}
