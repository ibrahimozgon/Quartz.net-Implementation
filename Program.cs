using System;
using System.Threading;
using Quartz;
using Quartz.Impl;

namespace QuartzImpl
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };

                // Grab the Scheduler instance from the Factory 
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // and start it off
                scheduler.Start();

                // define the job and tie it to our HelloJob class
                var job = JobBuilder.Create<EmailNotificationJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .RepeatForever()
                        .WithIntervalInSeconds(10))
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job, trigger);

                Console.WriteLine("Press any key to close the application");
                Console.ReadKey();
                // and last shut down the scheduler when you are ready to close your program
                scheduler.Shutdown();

            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}
