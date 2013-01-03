using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceProcess;


using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using Microsoft.Win32;
using COMAdmin;

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

            ICOMAdminCatalog2 oCatalog = (ICOMAdminCatalog2)Activator.CreateInstance(Type.GetTypeFromProgID("ComAdmin.COMAdminCatalog"));

            ICatalogCollection applications = (ICatalogCollection)oCatalog.GetCollection("Applications");
            applications.Populate();
            foreach (ICatalogObject applicationInstance in applications)
            {
                ICatalogCollection comps = (ICatalogCollection)applications.GetCollection("Components", applicationInstance.Key);
                comps.Populate();
                foreach (ICatalogObject comp in comps)
                {
                    Console.WriteLine("{0} - {1} - {2}", comp.Name, comp.Key, comp.ToString());
                }
            }

            COMAdminCatalogCollection applications2;
            COMAdminCatalog catalog;

            catalog = new COMAdminCatalog();
            applications2 = (COMAdminCatalogCollection)catalog.GetCollection("Applications");
            applications2.Populate();

            foreach(COMAdminCatalogObject application in applications2)
            {
                //do something with the application
                //if(  application.Name.Equals("MyAppName") )
                {
                    COMAdminCatalogCollection components;
                    components = (COMAdminCatalogCollection)applications2.GetCollection("Components", application.Key);

                    foreach(COMAdminCatalogObject component in components)
                    {
                        // do something with component
                    }
                }

            }

        }
    }
}
