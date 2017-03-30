using ES_BackupManager.ESBackupServerAdminService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Objects
{
    public class LogWPF: Log
    {
        public override string ToString()
        {
            return this.LogType.ToString() + " ---- " + this.UTCTime.ToString();
        }
    }
}
