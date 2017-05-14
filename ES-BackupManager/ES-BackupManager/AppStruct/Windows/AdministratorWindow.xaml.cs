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
    /// Interaction logic for AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow(Administrator admin)
        {
            InitializeComponent();

            this.Admin = admin;
            this._gridEmailsList = new BindingList<Email>(admin.Emails);
            this.LoadAdministratorData(admin);
        }

        #region Local properties
        private Administrator Admin { get; set; }

        private bool _isEditState;
        public bool IsEditState
        {
            get { return _isEditState; }
            set
            {
                _isEditState = value;
                if (value)
                {
                    this.btn_Edit.Content = "Save";
                    this.btn_Cancel.IsEnabled = true;
                }
                else
                {
                    this.btn_Edit.Content = "Edit";
                    this.btn_Cancel.IsEnabled = false;
                }
            }
        }
        private BindingList<Email> _gridEmailsList { get; set; }
        #endregion

        public void LoadAdministratorData(Administrator admin)
        {
            this.DisableComponents();

            this.textBox_FirstName.Text = admin.FirstName;
            this.textBox_LastName.Text = admin.LastName;
            this.dateTimePicker_RegistrationDate.Value = admin.UTCRegistrationDate;
            this.dataGrid_Emails.ItemsSource = this._gridEmailsList;
        }

        public void DisableComponents()
        {
            this.groupBox_Details.IsEnabled = false;
            this.groupBox_Emails.IsEnabled = false;
        }

        public void EnableComponents()
        {
            this.groupBox_Details.IsEnabled = true;
            this.groupBox_Emails.IsEnabled = true;
        }
        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {            
            if (this.IsEditState && this.IsValid())
            {
                ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
                
                this.Admin.FirstName = this.textBox_FirstName.Text;
                this.Admin.LastName = this.textBox_LastName.Text;
                this.Admin.Emails = new List<Email>(this._gridEmailsList);

                client.UpdateAdministrator(this.Admin);

                this.DisableComponents();
                this.IsEditState = false;

                client.Close();
            }
            else
            {
                this.EnableComponents();
                this.IsEditState = true;
            }
        }
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.IsEditState = false;
            this.LoadAdministratorData(this.Admin);
        }

        private void btn_EmailAdd_Click(object sender, RoutedEventArgs e)
        {
            EmailAddWindow emailWindow = new EmailAddWindow(this.Admin.ID);
            if (emailWindow.ShowDialog() == true)
                this._gridEmailsList.Add(emailWindow.GetEmail());
        }

        private void btn_EmailRemove_Click(object sender, RoutedEventArgs e)
        {
            int index = this.dataGrid_Emails.SelectedIndex;
            if (index >= 0)
                this._gridEmailsList.RemoveAt(index);
        }
        private bool IsValid()
        {     
            if (string.IsNullOrWhiteSpace(this.textBox_FirstName.Text))
                return false;

            if (string.IsNullOrWhiteSpace(this.textBox_LastName.Text))
                return false;

            return true;
        }
    }
}
