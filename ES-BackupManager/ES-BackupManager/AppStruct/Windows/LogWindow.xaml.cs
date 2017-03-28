using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ES_BackupManager.ESBackupServerAdminService;

namespace ES_BackupManager.AppStruct.Windows
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        //TODO: Implementovat do kontruktoru parametry (Client...)
        public LogWindow(Client c)
        {
            InitializeComponent();

            this.LoadList(c);
        }
        private void LoadList(Client c)
        {
            foreach (Log item in c.Logs)
            {
                this.listView_Logs.Items.Add(item);
            }
        }
    }
}
