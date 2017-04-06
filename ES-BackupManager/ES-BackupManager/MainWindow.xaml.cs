using ES_BackupManager.AppStruct.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace ES_BackupManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this._loadGrid();
        }

        #region BindingLists for DataGrids
        private BindingList<Client> _gridClients { get; set; } = new BindingList<Client>();
        private BindingList<Backup> _gridBackups { get; set; } = new BindingList<Backup>();
        private BindingList<Log> _gridLogs { get; set; } = new BindingList<Log>();
        #endregion

        #region Setup Controls
        private void _loadGrid()
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            foreach (Client item in client.GetClients())
            {
                this._gridClients.Add(item);
            }
            this.dataGrid_Clients.SelectedIndex = this._gridClients.Count == 0 ? -1 : 0;
            this.dataGrid_Clients.ItemsSource = this._gridClients;

            client.Close();
        }
        private void _loadComponents(Client c)
        {
            if (!this.TabControl_Main.IsEnabled)
                this.TabControl_Main.IsEnabled = true;

            this._gridBackups.Clear();
            this._gridLogs.Clear();

            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            #region AboutClient_Tab

            #endregion
            #region Backups_Tab
            foreach (Backup item in client.GetBackups(c))
            {
                this._gridBackups.Add(item);
            }
            this.dataGrid_Backups.SelectedIndex = this._gridBackups.Count == 0 ? -1: 0;
            this.dataGrid_Backups.ItemsSource = this._gridBackups;
            this._backupTab_DisableComponents();
            #endregion
            #region BackupTemplates_Tab

            #endregion
            #region Logs_Tab
            foreach (Log item in client.GetLogsByClient(c))
            {
                this._gridLogs.Add(item);
            }
            this.dataGrid_Logs.ItemsSource = this._gridLogs;
            #endregion
            #region Logins_Tab

            #endregion
            #region Settings_Tab

            #endregion
            client.Close();
        }
        #endregion

        #region Client Controls        
        private void dataGrid_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.TabControl_Main.IsEnabled)
                this.TabControl_Main.IsEnabled = true;

            Client c = this.dataGrid_Clients.SelectedItem as Client;            
            this._loadComponents(c);
            this.TabControl_Main.SelectedIndex = 0;
        }
        #endregion
        #region Backup Controls
        private void _loadBackupInfo(Backup b)
        {
            this._backupTab_DisableComponents();

            this.textBox_Backup_Name.Text = b.Name;
            this.textBox_Backup_Description.Text = b.Description;

            if (b.Compressed)
                this.radioBtn_Backup_Compress.IsChecked = true;

            if (b.Type)
                this.radioBtn_Backup_Full.IsChecked = true;

            this.textBox_Backup_Source.Text = b.Source;
            this.textBox_Backup_Dest.Text = b.Destination;

            if (b.Expiration == null)
            {
                this.dateTimePicker_Backup_Expire.Watermark = "Expire date is not set.";
                this.dateTimePicker_Backup_Expire.Value = null;
            }                
            else
                this.dateTimePicker_Backup_Expire.Value = b.Expiration;

            this.dateTimePicker_Backup_Start.Value = b.Start;

            if (b.End == null)
                this.dateTimePicker_Backup_End.Watermark = "End time is not set.";
            else
                this.dateTimePicker_Backup_End.Value = b.End;


        }
        private void dataGrid_Backups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Backup b = this.dataGrid_Backups.SelectedItem as Backup;
            this._loadBackupInfo(b);
        }
        private void _backupTab_DisableComponents()
        {
            this.textBox_Backup_Name.IsEnabled = false;
            this.textBox_Backup_Description.IsEnabled = false;
            this.radioBtn_Backup_Full.IsEnabled = false;
            this.radioBtn_Backup_Diff.IsEnabled = false;
            this.radioBtn_Backup_Compress.IsEnabled = false;
            this.radioBtn_Backup_NoCompress.IsEnabled = false;
            this.textBox_Backup_Source.IsEnabled = false;
            this.textBox_Backup_Dest.IsEnabled = false;
            this.dateTimePicker_Backup_Expire.IsEnabled = false;
            this.dateTimePicker_Backup_Start.IsEnabled = false;
            this.dateTimePicker_Backup_End.IsEnabled = false;
            this.btn_Backup_Save.IsEnabled = false;
            this.btn_Backup_Cancel.IsEnabled = false;
        }
        private void btn_Backup_Edit_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Backup_Save.IsEnabled = true;
            this.btn_Backup_Cancel.IsEnabled = true;

            this.textBox_Backup_Name.IsEnabled = true;
            this.textBox_Backup_Description.IsEnabled = true;
            this.dateTimePicker_Backup_Expire.IsEnabled = true;
        }
        #endregion

        private void btn_Backup_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Backup backup = this.dataGrid_Backups.SelectedItem as Backup;
            this._loadBackupInfo(backup);
        }
    }
}
