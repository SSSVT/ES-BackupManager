using ES_BackupManager.ESBackupServerAdminService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private BindingList<Backup> list { get; set; }
        //TODO: Implementovat do kontruktoru parametry (Client...)
        public BackupWindow(Client c)
        {
            InitializeComponent();

            this.LoadGrid(c);
        }
        private void LoadGrid(Client c)
        {
            list = new BindingList<Backup>();
            foreach (Backup item in c.Backups)
            {
                this.list.Add(item);
            }
            this.dataGrid_Backups.ItemsSource = this.list;
        }
    }
}
