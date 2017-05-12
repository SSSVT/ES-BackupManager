using ES_BackupManager.AppStruct.Components.CRON.Factories;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Components
{
    public class CRONScheduler
    {
        #region Singleton
        private static CRONScheduler _instance { get; set; }
        private CRONScheduler()
        {
            this.Start();
        }
        public static CRONScheduler GetInstance()
        {
            if (CRONScheduler._instance == null)
                CRONScheduler._instance = new CRONScheduler();

            return CRONScheduler._instance;
        }
        #endregion
        #region Local properties
        #region Factories
        private JobFactory EventFactory { get; set; } = JobFactory.GetInstance();
        private TriggerFactory TriggerFactory { get; set; } = TriggerFactory.GetInstance();
        #endregion

        private IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
        #endregion
        public void Start()
        {
            scheduler.Start();
        }
        public void StartJobs()
        {

        }
        public void ShutdownJobs()
        {

        }
    }
}
