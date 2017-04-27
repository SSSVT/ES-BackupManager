using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES_BackupManager.AppStruct.Objects
{
    public class PathInfo
    {
        public PathInfo(string value, int type = 0)
        {
            this.Value = value;
            this.Type = this.ConvertToType(type);
        }
        //TODO: Fix enum in datagrid
        [Browsable(false)]
        public string Value { get; set; }
        [Browsable(false)]
        public PathType Type { get; set; }
        private PathType ConvertToType(int type)
        {
            switch (type)
            {
                case 0:
                    return PathType.Windows;                    
                case 1:
                    return PathType.FTP;
                case 2:
                    return PathType.SSH;
                case 3:
                    return PathType.SC;
                default:
                    return PathType.Windows;
            }            
        }
    }
    public enum PathType
    {
        Windows,
        FTP,
        SSH,
        SC
    }
}
