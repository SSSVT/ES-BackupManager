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
using System.ComponentModel;

namespace ES_BackupManager.AppStruct.Windows
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        private BindingList<Log> list { get; set; }
        public LogWindow(Client c)
        {
            InitializeComponent();

            this.LoadList(c);
        }
        private void LoadList(Client c)
        {
            if (c.Logs != null)
            {
                list = new BindingList<Log>();
                foreach (Log item in c.Logs)
                {
                    this.list.Add(item);
                }
                this.dataGrid_Logs.ItemsSource = list;
                this.dataGrid_Logs.SelectedIndex = 0;
            }    
            
        }

        private void dataGrid_Logs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Log log = this.dataGrid_Logs.SelectedItem as Log;

            if (log.Backup != null)
            {
                this.label_Backup_Static.IsEnabled = true;                
                this.label_Backup.IsEnabled = true;
                this.label_Backup.Content = log.Backup.Name;
            }                
            else
            {                
                this.label_Backup_Static.IsEnabled = false;
                this.label_Backup.IsEnabled = false;
                this.label_Backup.Content = "NONE";
            }

            this.label_Time.Content = log.UTCTime.ToString();
            this.label_Value.Content = log.Value;
        }
    }
}
