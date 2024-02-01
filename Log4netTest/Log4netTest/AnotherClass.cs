using log4net;

namespace Log4netTest
{
    internal class AnotherClass
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void FunctionToLog()
        {
            log.Info("Hello from AnotherClass.FunctionToLog");
        }
    }
}