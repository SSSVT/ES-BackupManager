using ES_BackupManager.AppStruct.Windows;
using ES_BackupManager.ESBackupServerService;
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

            ESBackupServerServiceClient client = new ESBackupServerServiceClient();

            //TODO:Implementovat načítání komponentů, které jsou potřeba

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
            LogWindow lw = new LogWindow();

            if (lw.ShowDialog() == true)
            {

            }
        }

        private void button_ViewBackups_Click(object sender, RoutedEventArgs e)
        {
            BackupWindow bw = new BackupWindow();

            if (bw.ShowDialog() == true)
            {

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
    }
}
