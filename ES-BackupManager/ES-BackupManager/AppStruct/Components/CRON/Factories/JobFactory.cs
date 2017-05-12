using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Components.CRON.Factories
{
    public class JobFactory
    {
        #region Singleton
        private static JobFactory _instance { get; set; }
        private JobFactory()
        {
        }
        public static JobFactory GetInstance()
        {
            if (JobFactory._instance == null)
                JobFactory._instance = new JobFactory();

            return JobFactory._instance;
        }
        #endregion

        public IJobDetail CreateJob<T>(string name) where T : IJob
        {
           return JobBuilder.Create<T>()
                .WithIdentity(name)
                .Build();
        }
    }
}
