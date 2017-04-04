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
        private BindingList<Backup> backupList { get; set; }
        private BindingList<BackupTemplate> templateList { get; set; }
        public BackupWindow(Client c)
        {
            InitializeComponent();

            this.LoadGrid(c);
            this.btn_TemplateAction.IsEnabled = false;
        }
        private void LoadGrid(Client c)
        {
            backupList = new BindingList<Backup>();
            foreach (Backup item in c.Backups)
            {
                this.backupList.Add(item);
            }
            this.dataGrid_Backups.ItemsSource = this.backupList;

            templateList = new BindingList<BackupTemplate>();
            foreach (BackupTemplate item in c.Templates)
            {
                this.templateList.Add(item);
            }
            this.dataGrid_Templates.ItemsSource = this.templateList;
        }

        private void btn_TemplateAction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid_Templates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackupTemplate bt = this.dataGrid_Templates.SelectedItem as BackupTemplate;

            if(!this.btn_TemplateAction.IsEnabled)
                this.btn_TemplateAction.IsEnabled = true;

            //TODO: Implementovat změnu tlačítka přes STATUS
            switch (bt.ID)
            {
                case 1:
                    this.btn_TemplateAction.Content = "Pause";
                    break;
                case 2:
                    this.btn_TemplateAction.Content = "Resume";
                    break;
                default:
                    break;
            }

        }

        private void btn_EditExpireDate_Click(object sender, RoutedEventArgs e)
        {
            Backup b = this.dataGrid_Backups.SelectedItem as Backup;


        }

        private void btn_TemplateRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid_Backups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Backup b = this.dataGrid_Backups.SelectedItem as Backup;

            this.label_Name.Content = b.Name;
            this.label_Description.Content = b.Description;
            this.label_Source.Content = b.Source;
            this.label_Destination.Content = b.Destination;
            this.label_BeginTime.Content = b.Start.ToString();
            this.label_EndTime.Content = b.End.ToString();

            if (b.Expiration != null)
            {
                this.label_ExpireDate.IsEnabled = true;
                this.label_ExpireDate.Content = b.Expiration.ToString();
                this.btn_EditExpireDate.IsEnabled = true;
            }
            else
            {
                this.label_ExpireDate.IsEnabled = false;
                this.label_ExpireDate.Content = "Not set";
                this.btn_EditExpireDate.IsEnabled = false;
            }
        }
    }
}
