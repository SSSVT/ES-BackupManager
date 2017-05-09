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
    /// Interaction logic for AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow(Administrator admin)
        {
            InitializeComponent();

            this.Admin = admin;

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

        #endregion

        public void LoadAdministratorData(Administrator admin)
        {
            this.DisableComponents();

            this.textBox_FirstName.Text = admin.FirstName;
            this.textBox_LastName.Text = admin.LastName;
            this.dateTimePicker_RegistrationDate.Value = admin.UTCRegistrationDate;            
            this.listBox_Emails.ItemsSource = admin.Emails;
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

        public void Emails()
        {
            /*
            #region Email Controls
            private List<string> _convertEmailsToListBox(string emails)
            {
                List<string> list = new List<string>();
                foreach (string item in emails.Split(';'))
                {
                    list.Add(item);
                }
                return list;
            }
            private string _convertEmailsFromListBox(BindingList<string> emails)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < emails.Count; i++)
                {
                    sb.Append(emails[i] + ';');
                }
                sb.Length--;
                return sb.ToString();
            }
            private void btn_Client_EmailAdd_Click(object sender, RoutedEventArgs e)
            {
                if (!this._emailChangeMode)
                {
                    string emailAddress = this.textBox_Client_Email.Text;

                    if (this._isEmailValid(emailAddress))
                    {
                        this._listBoxEmailsList.Add(emailAddress);
                        this._emailChangeState(2);
                        this.btn_Client_EmailAdd.IsEnabled = false;
                    }
                }
                else if (this._emailChangeMode)
                {
                    this._listBoxEmailsList[this.listBox_Client_Emails.SelectedIndex] = this.textBox_Client_Email.Text;

                    this._emailChangeState(2);
                    this.btn_Client_EmailAdd.IsEnabled = false;
                    this.btn_Client_EmailEdit.IsEnabled = false;
                    this.btn_Client_EmailRemove.IsEnabled = false;
                }
            }

            private void btn_Client_EmailEdit_Click(object sender, RoutedEventArgs e)
            {
                if (this.listBox_Client_Emails.SelectedIndex >= 0)
                {
                    if (!this._emailChangeMode)
                    {
                        this._emailChangeState(1);
                        this.textBox_Client_Email.Text = this.listBox_Client_Emails.SelectedItem as string;
                    }
                    else if (this._emailChangeMode)
                    {
                        this._emailChangeState(2);
                        this.btn_Client_EmailAdd.IsEnabled = false;
                    }
                }
            }

            private void _emailChangeState(int code)
            {
                if (code == 1)
                {
                    this._emailChangeMode = true;
                    this.btn_Client_EmailAdd.Content = "Save";
                    this.btn_Client_EmailAdd.IsEnabled = true;
                    this.btn_Client_EmailEdit.Content = "Cancel";
                    this.btn_Client_EmailRemove.IsEnabled = false;
                }
                else if (code == 2)
                {
                    this._emailChangeMode = false;
                    this.btn_Client_EmailAdd.Content = "Add";
                    this.btn_Client_EmailEdit.Content = "Edit";
                    this.btn_Client_EmailRemove.IsEnabled = true;
                    this.textBox_Client_Email.Text = "";
                    this.textBox_Client_Email.Watermark = "Client email";
                }
            }
            private void btn_Client_EmailRemove_Click(object sender, RoutedEventArgs e)
            {
                int index = this.listBox_Client_Emails.SelectedIndex;

                if (this.listBox_Client_Emails.SelectedIndex == 0)
                {
                    this.btn_Client_EmailEdit.IsEnabled = false;
                    this.btn_Client_EmailRemove.IsEnabled = false;
                }

                if (this.listBox_Client_Emails.SelectedIndex != -1)
                {
                    this._listBoxEmailsList.RemoveAt(index);
                    this.listBox_Client_Emails.SelectedIndex = index - 1;
                }
            }
            private void textBox_Client_Email_TextChanged(object sender, TextChangedEventArgs e)
            {
                if (!this._emailChangeMode)
                {
                    if (!this.btn_Client_EmailAdd.IsEnabled)
                        this.btn_Client_EmailAdd.IsEnabled = true;

                    this.btn_Client_EmailEdit.IsEnabled = false;
                    this.btn_Client_EmailRemove.IsEnabled = false;

                    this.listBox_Client_Emails.SelectedIndex = -1;
                }
            }
            private void listBox_Client_Emails_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (this.listBox_Client_Emails.SelectedIndex >= 0)
                {
                    if (this._emailChangeMode)
                    {
                        this._emailChangeMode = false;
                        this.btn_Client_EmailAdd.Content = "Add";
                        this.btn_Client_EmailEdit.Content = "Edit";
                        this.btn_Client_EmailRemove.IsEnabled = true;
                    }

                    if (this.btn_Client_EmailAdd.IsEnabled)
                        this.btn_Client_EmailAdd.IsEnabled = false;

                    if (!this.btn_Client_EmailEdit.IsEnabled)
                        this.btn_Client_EmailEdit.IsEnabled = true;

                    if (!this.btn_Client_EmailRemove.IsEnabled)
                        this.btn_Client_EmailRemove.IsEnabled = true;
                }
            }
            private bool _isEmailValid(string emailAddress)
            {
                if (!this._isEmailRegistered(emailAddress))
                {
                    this.label_Client_EmailError.Content = "*Email is already registred";
                    this.label_Client_EmailError.ToolTip = "Please add email which is not in the list";
                    this.label_Client_EmailError.Visibility = Visibility.Visible;
                    return false;
                }
                else if (Regex.IsMatch(emailAddress, @"^[a-z0-9]+(\.?[a-z0-9]+)*@([a-z0-9]+\.)+[a-z]{2,}$"))
                {
                    this.label_Client_EmailError.Visibility = Visibility.Hidden;
                    return true;
                }
                else
                {
                    this.label_Client_EmailError.Content = "*Email is not in valid format";
                    this.label_Client_EmailError.ToolTip = "Example: evostudio@evostudio.com or evo.studio@evostudio.co.uk";
                    this.label_Client_EmailError.Visibility = Visibility.Visible;
                    return false;
                }
            }
            private bool _isEmailRegistered(string emailAdress)
            {
                foreach (string item in this._listBoxEmailsList)
                {
                    if (emailAdress == item)
                    {
                        return false;
                    }
                }
                return true;
            }
            #endregion
            */
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsEditState)
            {

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
    }
}
