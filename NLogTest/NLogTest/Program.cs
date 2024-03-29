﻿namespace NLogTest
{
    internal class Program
    {
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            try
            {
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
                log.Error(ex, "Goodbye cruel world");
            }
        }
    }
}