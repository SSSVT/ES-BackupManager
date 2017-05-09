using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Components
{
    public class CRONScheduler
    {
        public void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            IJobDetail job = JobBuilder.Create<EventJob>()
                .WithIdentity("job1")
                .Build();

            Tester r = new Tester();
            r.name = "pepa";
            job.JobDataMap.Put("instance", r); //Poslání instance třídy Tester do jobu

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1")
                .StartNow()
                .WithCronSchedule("*/2 * * * * ?") //CRON interval
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
