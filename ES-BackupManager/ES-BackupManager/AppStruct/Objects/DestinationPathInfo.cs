﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESBackupManager.AppStruct.Objects
{
    public class DestinationPathInfo
    {
        public DestinationPathInfo(string value, byte type)
        {
            this.Value = value;
            this.Type = this.ConvertToType(type);
            this.TypeByte = type;
        }
        public string Value { get; set; }
        public string Type { get; set; }
        public byte TypeByte { get; set; }
        private string ConvertToType(byte code)
        {
            switch (code)
            {
                case 0:
                    return "LOCAL";
                case 1:
                    return "FTP";
                case 2:
                    return "SSH";
                default:
                    return "WIN";
            }
        }
    }
}
