using ES_BackupManager.AppStruct.Components;
using ES_BackupManager.AppStruct.Objects;
using ES_BackupManager.AppStruct.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            this.LoadComponents();
        }

        private void LoadComponents()
        {
            this.radioButton_ExpireAfter.IsChecked = true;

            //TODO:Implementovat načítání komponentů, které jsou potřeba
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            foreach (Client item in client.GetClients())
            {                
                ClientWPF cWPF = new ClientWPF();
                cWPF.ID = item.ID;
                cWPF.Name = item.Name;
                cWPF.Description = item.Description;
                cWPF.Verified = item.Verified;
                cWPF.LogWPF = ClassConvertor.ToLogWPF(item.Logs);
                //cWPF.Backups = item.Backups;
                cWPF.Templates = item.Templates;
                this.listView_Clients.Items.Add(cWPF);              
            }            

            client.Close();
        }
        private void radioButton_ExpireAfter_Checked(object sender, RoutedEventArgs e)
        {
            this.textBox_ExpireDate_After.IsEnabled = true;
            this.DatePicker_ExpireOn.IsEnabled = false;
        }

        private void radioButton_ExpireOn_Checked(object sender, RoutedEventArgs e)
        {
            this.textBox_ExpireDate_After.IsEnabled = false;
            this.DatePicker_ExpireOn.IsEnabled = true;
        }

        private void button_ViewLogs_Click(object sender, RoutedEventArgs e)
        {        
            ClientWPF c = this.listView_Clients.SelectedItem as ClientWPF;
            if (c != null)
            {
                LogWindow lw = new LogWindow(c);
                lw.ShowDialog();
            }
        }

        private void button_ViewBackups_Click(object sender, RoutedEventArgs e)
        {
            ClientWPF c = this.listView_Clients.SelectedItem as ClientWPF;
            if (c != null)
            {
                BackupWindow bw = new BackupWindow(c);
                bw.ShowDialog();
            }            
        }

        private void btn_LoadSett_Click(object sender, RoutedEventArgs e)
        {
            LoadSettingsWindow lsw = new LoadSettingsWindow();

            if (lsw.ShowDialog() == true)
            {

            }
        }

        private void btn_SaveSett_Click(object sender, RoutedEventArgs e)
        {
            SaveSettingsWindows ssw = new SaveSettingsWindows();

            if (ssw.ShowDialog() == true)
            {

            }
        }

        private void button_MoreSettings_Click(object sender, RoutedEventArgs e)
        {
            MoreSettingsWindow msw = new MoreSettingsWindow();

            if (msw.ShowDialog() == true)
            {

            }
        }

        private void listView_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientWPF c = this.listView_Clients.SelectedItem as ClientWPF;

            this.label_Client.Content = c.Name;

            //TEST
            try
            {
                if (c.Verified)
                    this.label_Verification.Content = "Verified";
                else
                    this.label_Verification.Content = "Unknown";
            }
            catch (Exception)
            {
                this.label_Status.Content = "REMOVE";
                this.label_Verification.Content = "EXCEPTION";
            }
        }
    }
}
