namespace NLogTest
{
    internal class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            try
            {
                //Set current thread name
                Thread.CurrentThread.Name = "main";

                //Log with different levels
                Logger.Warn("Hello warn world");
                Logger.Info("Hello info world");
                Logger.Debug("Hello debug world");

                //Exception to test error log
                int x = 0;
                int y = 1 / x;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Goodbye cruel world");
            }
        }
    }
}