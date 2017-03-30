using ES_BackupManager.AppStruct.Objects;
using ES_BackupManager.ESBackupServerAdminService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Components
{
    public static class ClassConvertor
    {
        public static List<LogWPF> ToLogWPF(Log[] src)
        {
            List<LogWPF> list = new List<LogWPF>();

            foreach (Log item in src)
            {                
                LogWPF log = new LogWPF();
                log.ID = item.ID;

                log.IDClient = item.IDClient;
                log.IDBackup = item.IDBackup;
                log.IDLogType = item.IDLogType;
                log.Client = item.Client;
                log.Backup = item.Backup;
                log.LogType = item.LogType;

                log.UTCTime = item.UTCTime;
                log.Value = item.Value;                            

                list.Add(log);
            }
            return list;
        }
    }
}
