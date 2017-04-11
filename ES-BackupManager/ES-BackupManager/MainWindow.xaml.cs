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

            this._loadGrid(Filter.All,Sort.Asc);
        }

        #region BindingLists for DataGrids
        private BindingList<Client> _gridClients { get; set; } = new BindingList<Client>();
        private BindingList<Backup> _gridBackups { get; set; } = new BindingList<Backup>();
        private BindingList<Log> _gridLogs { get; set; } = new BindingList<Log>();
        private BindingList<BackupTemplate> _gridTemplates { get; set; } = new BindingList<BackupTemplate>();
        #endregion

        #region Setup Controls
        private void _loadGrid(Filter f,Sort s)
        {           
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            
            foreach (Client item in client.GetClients(f,s))
            {
                this._gridClients.Add(item);
            }

            if (this._gridClients.Count > 0)
                this.dataGrid_Clients.SelectedIndex = 0;

            this.dataGrid_Clients.ItemsSource = this._gridClients;

            client.Close();
        }
        private void _loadComponents(Client c)
        {
            if (!this.TabControl_Main.IsEnabled)
                this.TabControl_Main.IsEnabled = true;

            this._gridBackups.Clear();
            this._gridTemplates.Clear();
            this._gridLogs.Clear();

            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            #region AboutClient_Tab

            #endregion
            #region Backups_Tab
            foreach (Backup item in client.GetBackupsByClientID(c.ID))
            {
                this._gridBackups.Add(item);
            }            
            this.dataGrid_Backups.ItemsSource = this._gridBackups;
            this._backupTab_DisableComponents();

            if (this._gridBackups.Count == 0)
            {
                this.dataGrid_Backups.SelectedIndex = -1;
                this.btn_Backup_Edit.IsEnabled = false;
            }
            else
            {
                this.dataGrid_Backups.SelectedIndex = 0;
                this.btn_Backup_Edit.IsEnabled = true;
            }
            #endregion
            #region BackupTemplates_Tab
            //TODO: Implement            
            
            foreach (BackupTemplate item in client.GetConfiguration(c).Templates)
            {
                this._gridTemplates.Add(item);
            }
            this.dataGrid_BackupTemplates.ItemsSource = this._gridTemplates;
            #endregion
            #region Logs_Tab
            foreach (Log item in client.GetLogsByClientID(c.ID))
            {
                this._gridLogs.Add(item);
            }
            this.dataGrid_Logs.ItemsSource = this._gridLogs;

            this._logTab_DisableComponents();
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
            if(c != null)
            {
                this._loadComponents(c);
                this._loadClientInfo(c);
            }                      
        }

        private void _clientTab_DisableComponents()
        {
            this.textBox_Client_Name.IsEnabled = false;
            this.textBox_Client_Description.IsEnabled = false;
        }
        private void _loadClientInfo(Client c)
        {
            this._clientTab_DisableComponents();

            this.textBox_Client_Name.Text = c.Name;
            this.textBox_Client_Description.Text = c.Description;
        }
        #endregion
        #region Backup Controls
        private void _loadBackupInfo(Backup b)
        {
            this._backupTab_DisableComponents();
            if (b != null)
            {
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

                if (this.btn_Backup_Edit.IsEnabled == false)
                    this.btn_Backup_Edit.IsEnabled = true;
            }
            else
                this._backupTab_SetDefaultValues();
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
            this.btn_Backup_Edit.IsEnabled = false;
        }
        private void _backupTab_SetDefaultValues()
        {
            this.textBox_Backup_Name.Text = "Backup name";
            this.textBox_Backup_Description.Text = "Backup description";
            this.radioBtn_Backup_NoCompress.IsChecked = true;
            this.radioBtn_Backup_Diff.IsChecked = true;

            this.textBox_Backup_Source.Text = "Backup source path";
            this.textBox_Backup_Dest.Text = "Backup destination path";


            this.dateTimePicker_Backup_Expire.Value = null;
            this.dateTimePicker_Backup_Expire.Watermark = "Backup expiration date";

            this.dateTimePicker_Backup_Start.Watermark = "Backup start time";
            this.dateTimePicker_Backup_Start.Value = null;

            this.dateTimePicker_Backup_End.Watermark = "Backup end time";
            this.dateTimePicker_Backup_End.Value = null;
        }
        private void btn_Backup_Edit_Click(object sender, RoutedEventArgs e)
        {
            if(this.dataGrid_Backups.SelectedIndex != -1)
            {
                this.btn_Backup_Save.IsEnabled = true;
                this.btn_Backup_Cancel.IsEnabled = true;

                this.textBox_Backup_Name.IsEnabled = true;
                this.textBox_Backup_Description.IsEnabled = true;
                this.dateTimePicker_Backup_Expire.IsEnabled = true;
            }            
        }
        private void btn_Backup_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Backup backup = this.dataGrid_Backups.SelectedItem as Backup;
            this._loadBackupInfo(backup);
        }
        private void btn_Backup_Save_Click(object sender, RoutedEventArgs e)
        {
            Backup backup = this.dataGrid_Backups.SelectedItem as Backup;

            backup.Name = this.textBox_Backup_Name.Text;
            backup.Description = this.textBox_Backup_Description.Text;
            backup.Expiration = this.dateTimePicker_Backup_Expire.Value;

            this._backupTab_DisableComponents();
            this.btn_Backup_Edit.IsEnabled = true;                

            //TODO: Send data to server and update database
        }
        #endregion
        #region Log Controls
        private void _logTab_DisableComponents()
        {
            this.textBox_Log_BackupName.IsEnabled = false;
            this.comboBox_Log_LogType.IsEnabled = false;
            this.dateTimePicker_Log_Time.IsEnabled = false;
        }
        #endregion

        private void btn_Main_ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            Filter filter;
            Sort sort;

            switch (this.comboBox_Main_Filter.SelectedIndex)
            {
                case 0:
                    filter = Filter.All;
                    break;
                case 1:
                    filter = Filter.Verified;
                    break;
                case 2:
                    filter = Filter.Unverified;
                    break;
                case 3:
                    filter = Filter.Banned;
                    break;
                default:
                    filter = Filter.All;
                    break;
            }
            sort = this.comboBox_Main_Sort.SelectedIndex == 0 ? Sort.Asc : Sort.Desc;
            this._gridClients.Clear();
            this._loadGrid(filter,sort);
        }
    }
}
