using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Objects
{
    public class SourcePathInfo
    {
        public SourcePathInfo(string value)
        {
            this.Value = value;
        }
        public string Value { get; set; }
    }
}
