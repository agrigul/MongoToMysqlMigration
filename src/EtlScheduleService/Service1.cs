using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace EtlScheduleService
{
    public partial class Service1 : ServiceBase
    {
        private Timer Schedular;
        string path = AppDomain.CurrentDomain.BaseDirectory + "ServiceLog.txt";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Simple Service started {0}");
            ScheduleService();
        }

        protected override void OnStop()
        {
            WriteToFile("Simple Service stopped {0}");
            Schedular.Dispose();
        }

        internal void DebugRun()
        {
            OnStart(null);
        }

        public void ScheduleService()
        {
            try
            {
                Schedular = new Timer(new TimerCallback(SchedularCallback));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();

                WriteToFile("Simple Service Mode: " + mode + " {0}");


                DateTime scheduledTime = DateTime.MinValue;

                if (mode == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode.ToUpper() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }


                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                WriteToFile("Simple Service scheduled to run after: " + schedule + " {0}");
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
                Schedular.Change(dueTime, Timeout.Infinite);

            }
            catch (Exception e)
            {

                WriteToFile("Simple Service Error on: {0} " + e.Message + e.StackTrace);

                //Stop the Windows Service.
                using (ServiceController serviceController = new ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }


        private void SchedularCallback(object e)
        {
            WriteToFile("Simple Service Log: {0}");
            ScheduleService();
        }

        private void WriteToFile(string text)
        {

            if (File.Exists(path) == false)
            {
                File.Create(path).Dispose();
            }

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
    }
}
