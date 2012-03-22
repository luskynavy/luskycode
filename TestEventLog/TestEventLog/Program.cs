using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace TestEventLog
{
    class Program
    {
        static void Main(string[] args)
        {
            string sSource;
            string sLog;
            string sEvent;

            sSource = "dotNET Sample App";
            sLog = "Application";
            sEvent = "Sample Event";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            EventLog.WriteEntry(sSource, sEvent);
            EventLog.WriteEntry(sSource, sEvent,
                EventLogEntryType.Warning, 234);

            EventLog.WriteEntry("rien de rien", "event with rien de rien source");
            EventLog.WriteEntry("empty", "event with empty source", EventLogEntryType.Warning);

            EventLog myLog = new EventLog();
            try
            {
                // Write an informational entry to the event log.    
                myLog.WriteEntry("Writing to event without source log.");
            }
            catch (Exception ex)
            {
            }

            myLog.Source = "MySource";

            //don't work
            try
            {
                // Write an informational entry to the event log.    
                myLog.WriteEntry("Writing to event log.");
            }
            catch (Exception ex)
            {
            }

            //don't work
            try
            {
                EventLog.WriteEntry(null, "event with null source");
            }
            catch(Exception ex)
            {
            }
        }
    }
}
