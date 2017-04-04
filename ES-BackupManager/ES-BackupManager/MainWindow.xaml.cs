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

namespace ES_BackupManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BindingList<Client> _gridClients { get; set; } = new BindingList<Client>();
        private BindingList<Backup> _gridBackups { get; set; } = new BindingList<Backup>();
        private BindingList<Log> _gridLogs { get; set; } = new BindingList<Log>();
        public MainWindow()
        {
            InitializeComponent();

            this._loadGrid();
        }
        private void _loadGrid()
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            foreach (Client item in client.GetClients())
            {
                this._gridClients.Add(item);
            }
            this.dataGrid_Clients.ItemsSource = this._gridClients;

            client.Close();
        }
        private void _loadComponents(Client c)
        {
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
            this.dataGrid_Backups.ItemsSource = this._gridBackups;
            #endregion
            #region BackupTemplates_Tab

            #endregion
            #region Logs_Tab
            foreach (Log item in client.GetLogByClientID(c))
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

        private void dataGrid_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client c = this.dataGrid_Clients.SelectedItem as Client;            
            this._loadComponents(c);
            this.TabControl_Main.SelectedIndex = 0;
        }
    }
}
