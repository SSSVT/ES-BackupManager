using ES_BackupManager.ESBackupServerAdminService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Objects
{
    public class ClientWPF: Client
    {
        public List<LogWPF> LogWPF { get; set; }
        public List<BackupWPF> BackupWPF { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
