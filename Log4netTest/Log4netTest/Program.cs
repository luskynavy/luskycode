using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace Log4netTest
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            try
            {
                //Set config file name
                XmlConfigurator.Configure(new FileInfo("log4net.config"));

                //Set current thread name
                Thread.CurrentThread.Name = "main";

                //Log with different levels
                log.Warn("Hello warn world");
                log.Info("Hello info world");
                log.Debug("Hello debug world");

                var anObject = new AnotherClass();
                anObject.FunctionToLog();

                //Exception to test error log
                int x = 0;
                int y = 1 / x;
            }
            catch (Exception ex)
            {
                log.Error("Goodbye cruel world", ex);
            }
        }
    }
}