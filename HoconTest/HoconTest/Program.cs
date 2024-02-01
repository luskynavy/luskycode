using Hocon;

namespace HoconTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Get the config from a file
            Config conf = Hocon.HoconConfigurationFactory.FromFile("test.conf");
            if (conf != null)
            {
                //Check if path exist
                if (conf.HasPath("all.your.database"))
                {
                    //Get value
                    var val = conf.GetString("all.your.database");

                    Console.WriteLine("all.your.database = " + val);
                }

                if (conf.HasPath("all.stringWithout"))
                {
                    //Get value
                    var val = conf.GetString("all.stringWithout");

                    Console.WriteLine("all.stringWithout = " + val);
                }

                //non existant environment variable NON_EXISTANT_ENVIRONMENT_VARIABLE, line will be removed after loading
                if (conf.HasPath("all.willBeRemoved"))
                {
                    Console.WriteLine("It's not possible, line has not been removed");
                }
                else
                {
                    Console.WriteLine("Everything is ok, line has been removed");
                }

                //Get sub config
                Config confAll = conf.GetConfig("all");

                if (confAll != null)
                {
                    //Get host using environment variable COMPUTERNAME
                    var host = confAll.HasPath("server.host") ? confAll.GetString("server.host") : "";
                    var port = confAll.HasPath("server.port") ? confAll.GetInt("server.port") : 0;

                    Console.WriteLine("server= " + host + ":" + port);
                }
            }
        }
    }
}