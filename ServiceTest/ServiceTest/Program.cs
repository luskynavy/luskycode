using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceProcess;


namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {
                Console.WriteLine(scTemp.ServiceName + " : " + scTemp.Status
                    + (scTemp.CanPauseAndContinue ? " : CanPauseAndContinue" : ""));
            }
        }
    }
}
