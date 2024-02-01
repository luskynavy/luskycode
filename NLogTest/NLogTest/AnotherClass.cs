using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogTest
{
    internal class AnotherClass
    {
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public void FunctionToLog()
        {
            log.Info("Hello from AnotherClass.FunctionToLog");
        }
    }
}