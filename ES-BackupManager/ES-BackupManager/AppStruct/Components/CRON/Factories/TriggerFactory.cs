using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Components.CRON.Factories
{
    public class TriggerFactory
    {
        #region Singleton
        private static TriggerFactory _instance { get; set; }
        private TriggerFactory()
        {
        }
        public static TriggerFactory GetInstance()
        {
            if (TriggerFactory._instance == null)
                TriggerFactory._instance = new TriggerFactory();

            return TriggerFactory._instance;
        }
        #endregion

        public ITrigger CreateTrigger(string name, string CRON)
        {
            return TriggerBuilder.Create()
            .WithIdentity(name)
            .StartNow()
            .WithCronSchedule(CRON) //CRON interval
            .Build();
        }
    }
}
