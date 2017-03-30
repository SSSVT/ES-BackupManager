using ES_BackupManager.AppStruct.Objects;
using ES_BackupManager.ESBackupServerAdminService;
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

namespace ES_BackupManager.AppStruct.Windows
{
    /// <summary>
    /// Interaction logic for BackupWindow.xaml
    /// </summary>
    public partial class BackupWindow : Window
    {
        //TODO: Implementovat do kontruktoru parametry (Client...)
        public BackupWindow(ClientWPF c)
        {
            InitializeComponent();

            this.LoadGrid(c);
        }
        private void LoadGrid(ClientWPF c)
        {
            foreach (Backup item in c.Backups)
            {
                this.Backup_dataGrid.Items.Add(item);
            }
        }
    }
}
