﻿using ES_BackupManager.AppStruct.Objects;
using ES_BackupManager.ESBackupServerAdminService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

            //this._loadGrid(Filter.All,Sort.Asc);
            
            //TODO: DEBUG
            this.TabControl_Main.IsEnabled = true;
            this.dataGrid_Template_Source.ItemsSource = this._gridTemplateSourceList;
            this.dataGrid_Template_Destination.ItemsSource = this._gridTemplateDestinationList;
        }

        #region Local Properties
        private bool _emailChangeMode { get; set; }

        private bool template_AddMode;
        private bool _template_AddMode
        {
            get { return template_AddMode; }
            set
            {
                template_AddMode = value;
                if(value == true)
                {
                    this.btn_Template_New.IsEnabled = false;
                    this.btn_Template_Edit.Content = "Add";
                }
                else
                {
                    this.btn_Template_New.IsEnabled = true;
                    this.btn_Template_Edit.Content = "Edit";
                }
            }
        }


        private bool template_EditMode;
        private bool _template_EditMode
        {
            get { return template_EditMode; }
            set
            {
                template_EditMode = value;
                if (value == true)                
                    this.btn_Template_Edit.Content = "Add";                
                else                
                    this.btn_Template_Edit.Content = "Edit";                
            }
        }

        #endregion

        #region BindingLists for DataGrids/ListBoxes
        private BindingList<Client> _gridClientsList { get; set; } = new BindingList<Client>();
        private BindingList<Backup> _gridBackupsList { get; set; } = new BindingList<Backup>();
        private BindingList<Log> _gridLogsList { get; set; } = new BindingList<Log>();
        private BindingList<BackupTemplate> _gridTemplatesList { get; set; } = new BindingList<BackupTemplate>();
        private BindingList<string> _listBoxEmailsList { get; set; } = new BindingList<string>();

        private BindingList<PathInfo> _gridTemplateSourceList { get; set; } = new BindingList<PathInfo>();
        private BindingList<PathInfo> _gridTemplateDestinationList { get; set; } = new BindingList<PathInfo>();
        #endregion

        #region Setup Controls
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
            this._gridClientsList.Clear();
            this._loadGrid(filter, sort);
        }
        private void _loadGrid(Filter f,Sort s)
        {           
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            
            foreach (Client item in client.GetClients(f,s))
            {
                this._gridClientsList.Add(item);
            }

            if (this._gridClientsList.Count > 0)
                this.dataGrid_Clients.SelectedIndex = 0;

            this.dataGrid_Clients.ItemsSource = this._gridClientsList;

            client.Close();
        }
        private void _loadComponents(Client c)
        {
            if (!this.TabControl_Main.IsEnabled)
                this.TabControl_Main.IsEnabled = true;

            this._gridBackupsList.Clear();
            this._gridTemplatesList.Clear();
            this._gridLogsList.Clear();
            this._listBoxEmailsList.Clear();

            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            #region AboutClient_Tab

            #endregion
            #region Backups_Tab
            foreach (Backup item in client.GetBackupsByClientID(c.ID))
            {
                this._gridBackupsList.Add(item);
            }            
            this.dataGrid_Backups.ItemsSource = this._gridBackupsList;
            this._backupTab_DisableComponents();

            if (this._gridBackupsList.Count == 0)
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
            this.dataGrid_Template_Source.ItemsSource = this._gridTemplateSourceList;
            this.dataGrid_Template_Destination.ItemsSource = this._gridTemplateDestinationList;
            //TODO: Implement     
            /*
            Configuration config = client.GetConfigurationByClientID(c.ID);

            foreach (BackupTemplate item in config.Templates)
            {
                this._gridTemplatesList.Add(item);
            }

            if (this._gridTemplatesList.Count == 0)
            {
                this.dataGrid_BackupTemplates.SelectedIndex = -1;
                this.btn_Template_Edit.IsEnabled = false;
            }
            else
            {
                this.dataGrid_BackupTemplates.SelectedIndex = 0;
                this.btn_Template_Edit.IsEnabled = true;
            }
            */
            #endregion
            #region Logs_Tab
            foreach (Log item in client.GetLogsByClientID(c.ID))
            {
                this._gridLogsList.Add(item);
            }
            this.dataGrid_Logs.ItemsSource = this._gridLogsList;

            this._logTab_DisableComponents();
            #endregion
            #region Logins_Tab

            #endregion
            #region Settings_Tab

            #endregion
            client.Close();
        }
        private void comboBox_Main_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.comboBox_Main_Sort.IsEnabled = (this.comboBox_Main_Filter.SelectedIndex == 0) ? false : true;
        }
        #endregion

        #region Client Controls        
        private void dataGrid_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.TabControl_Main.IsEnabled)
                this.TabControl_Main.IsEnabled = true;

            if (!this.btn_Client_Edit.IsEnabled)
                this.btn_Client_Edit.IsEnabled = true;

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
            this.textBox_Client_Email.IsEnabled = false;
            this.btn_Client_EmailAdd.IsEnabled = false;
            this.btn_Client_EmailEdit.IsEnabled = false;
            this.btn_Client_EmailRemove.IsEnabled = false;
            this.btn_Client_Cancel.IsEnabled = false;
            this.btn_Client_Save.IsEnabled = false;
            this.listBox_Client_Emails.IsEnabled = false;
            this.label_Client_EmailError.Visibility = Visibility.Hidden;
            this.textBox_Client_Email.Text = "";
            this.comboBox_Client_Status.IsEnabled = false;
            this.groupBox_Client_Connection.IsEnabled = false;     
        }
        private void _loadClientInfo(Client c)
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            this._clientTab_DisableComponents();

            this.textBox_Client_Name.Text = c.Name;
            this.textBox_Client_Description.Text = c.Description;

            //foreach (string item in this._convertEmailsToListBox(c.Emails))
            //{
            //    this._listBoxEmailsList.Add(item);
            //}
            //this.listBox_Client_Emails.ItemsSource = this._listBoxEmailsList;

            switch (c.Status)
            {
                case 0:
                    this.comboBox_Client_Status.SelectedIndex = 0;
                    break;
                case 1:
                    this.comboBox_Client_Status.SelectedIndex = 1;
                    break;
                case 2:
                    this.comboBox_Client_Status.SelectedIndex = 2;
                    break;
                default:
                    this.comboBox_Client_Status.SelectedIndex = 1;
                    break;
            }

            if (c.StatusReportEnabled)
            {
                this.radioBtn_Client_ConnSet.IsChecked = true;
                this.IntUpDown_Client_StatusRepeat.Value = c.ReportInterval;

            }
            else
            {
                this.radioBtn_Client_ConnDefault.IsChecked = true;
            }           
            
            //TODO: Connection Repeat Info
            

            client.Close();
        }
        private void btn_Client_Save_Click(object sender, RoutedEventArgs e)
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            Client c = this.dataGrid_Clients.SelectedItem as Client;

            c.Name = this.textBox_Client_Name.Text;
            c.Description = this.textBox_Client_Description.Text;
            //c.Emails = this._convertEmailsFromListBox(this._listBoxEmailsList);
            c.Status = Convert.ToByte(this.comboBox_Client_Status.SelectedIndex);
            c.StatusReportEnabled = (bool)this.radioBtn_Client_ConnSet.IsChecked ? true : false ;
            if (c.StatusReportEnabled)
                c.ReportInterval = this.IntUpDown_Client_StatusRepeat.Value;

            this._clientTab_DisableComponents();
            this.btn_Client_Edit.IsEnabled = true;

            //TODO: Connection Repeat Info

            client.UpdateClient(c);
            client.Close();
        }

        private void btn_Client_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Client client = this.dataGrid_Clients.SelectedItem as Client;
            this._listBoxEmailsList.Clear();
            this._loadClientInfo(client);
        }
        private void btn_Client_Edit_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Connection Repeat Info
            this.textBox_Client_Name.IsEnabled = true;
            this.textBox_Client_Description.IsEnabled = true;
            this.textBox_Client_Email.IsEnabled = true;
            this.listBox_Client_Emails.IsEnabled = true;
            this.comboBox_Client_Status.IsEnabled = true;
            this.groupBox_Client_Connection.IsEnabled = true;

            this.btn_Client_Save.IsEnabled = true;
            this.btn_Client_Cancel.IsEnabled = true;

        }
        private void radioBtn_Client_ConnSet_Checked(object sender, RoutedEventArgs e)
        {
            this.IntUpDown_Client_StatusRepeat.IsEnabled = true;
        }

        private void radioBtn_Client_ConnDefault_Checked(object sender, RoutedEventArgs e)
        {
            this.IntUpDown_Client_StatusRepeat.IsEnabled = false;
            this.IntUpDown_Client_StatusRepeat.Value = null;
        }
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
                sb.Append(emails[i]+';');
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
            if(this.listBox_Client_Emails.SelectedIndex >= 0)
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
            if(code == 1)
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

            if(this.listBox_Client_Emails.SelectedIndex == 0)
            {
                this.btn_Client_EmailEdit.IsEnabled = false;
                this.btn_Client_EmailRemove.IsEnabled = false;
            }

            if(this.listBox_Client_Emails.SelectedIndex != -1)
            {
                this._listBoxEmailsList.RemoveAt(index);
                this.listBox_Client_Emails.SelectedIndex = index - 1;
            }            
        }
        private void textBox_Client_Email_TextChanged(object sender, TextChangedEventArgs e)
        {            
            if(!this._emailChangeMode)
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
            if(this.listBox_Client_Emails.SelectedIndex >= 0)
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

        #endregion
        #region Backup Template Controls
        private void radioBtn_Template_Ignore_Checked(object sender, RoutedEventArgs e)
        {
            this.textBox_Template_Ignore.IsEnabled = true;
            this.textBox_Template_Only.IsEnabled = false;
        }
        private void radioBtn_Template_Only_Checked(object sender, RoutedEventArgs e)
        {
            this.textBox_Template_Ignore.IsEnabled = false;
            this.textBox_Template_Only.IsEnabled = true;
        }
        private void dataGrid_BackupTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackupTemplate bt = this.dataGrid_BackupTemplates.SelectedItem as BackupTemplate;
            this._loadTemplateInfo(bt);
        }
        private void btn_Template_SourceAdd_Click(object sender, RoutedEventArgs e)
        {
            this._gridTemplateSourceList.Add(new PathInfo(this.textBox_Template_Source.Text));
        }
        private void btn_Template_SourceRemove_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_Template_DestinationAdd_Click(object sender, RoutedEventArgs e)
        {
            this._gridTemplateDestinationList.Add(new PathInfo(this.textBox_Template_Source.Text,this.comboBox_Template_DestinationType.SelectedIndex));
        }
        private void btn_Template_DestinationRemove_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_Template_New_Click(object sender, RoutedEventArgs e)
        {
            this._template_AddMode = true;
            this._templateTab_EnableComponents();
            this._templateTab_SetDefaultValues();

        }
        private void btn_Template_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (this._template_AddMode)
            {
                this._template_AddMode = false;
            }

            if (this._template_EditMode)
            {
                this._template_EditMode = false;
            }
        }

        private void btn_Template_Cancel_Click(object sender, RoutedEventArgs e)
        {
            BackupTemplate bt = this.dataGrid_BackupTemplates.SelectedItem as BackupTemplate;

            if (this._template_AddMode)
                this._template_AddMode = false;

            if (this._template_EditMode)
                this._template_EditMode = false;

            this._loadTemplateInfo(bt);
        }
        private void _templateTab_EnableComponents()
        {
            this.textBox_Template_Name.IsEnabled = true;
            this.textBox_Template_Description.IsEnabled = true;

            this.groupBox_Template_Exception.IsEnabled = true;
            this.groupBox_Template_Type.IsEnabled = true;
            this.groupBox_Template_Compression.IsEnabled = true;
            this.groupBox_Template_Path.IsEnabled = true;
            this.groupBox_Template_Time.IsEnabled = true;
            //this.checkBox_Template_TimeBox.IsEnabled = true;            
            this.groupBox_Template_Notification.IsEnabled = true;
        }
        private void _templateTab_DisableComponents()
        {
            this.textBox_Template_Name.IsEnabled = false;
            this.textBox_Template_Description.IsEnabled = false;

            this.groupBox_Template_Exception.IsEnabled = false;
            this.groupBox_Template_Type.IsEnabled = false;
            this.groupBox_Template_Compression.IsEnabled = false;
            this.groupBox_Template_Path.IsEnabled = false;
            this.groupBox_Template_Time.IsEnabled = false;
            //this.checkBox_Template_TimeBox.IsEnabled = false;            
            this.groupBox_Template_Notification.IsEnabled = false;
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void _loadTemplateInfo(BackupTemplate bt)
        {
            this._templateTab_DisableComponents();

            if (bt != null)
            {                
                this.textBox_Template_Name.Text = bt.Name;
                this.textBox_Template_Description.Text = bt.Description;

                //TODO: Rework to Incremental
                this.radioBtn_Template_Full.IsChecked = bt.Type;

                this.radioBtn_Template_Compress.IsChecked = bt.Compression;

                //TODO: Get templates source path(s)

                //TODO: Get templates destination path(s)

                //TODO: Get templates exceptions

                this.textBox_Template_CRON.Text = bt.CRONRepeatInterval;
                if (bt.DaysToExpiration != null)
                    this.dateTimePicker_Template_Expire.Value = DateTime.Now.AddDays(Convert.ToDouble(bt.DaysToExpiration));
                else
                    this.dateTimePicker_Template_Expire.Value = null;

                this.radioBtn_Template_NotifEnable.IsChecked = bt.IsNotificationEnabled;
            }
            else
                this._templateTab_SetDefaultValues();
        }
        private void _templateTab_SetDefaultValues()
        {
            //Name
            this.textBox_Template_Name.Text = null;
            this.textBox_Template_Description.Text = null;

            //Backup Type
            this.radioBtn_Template_Full.IsChecked = true;

            //Compression
            this.radioBtn_Template_NoCompress.IsChecked = true;

            //Source path(s)
            this.textBox_Template_Source.Text = null;
            this._gridTemplateSourceList.Clear();

            //Destination path(s)
            this.textBox_Template_Dest.Text = null;
            this._gridTemplateDestinationList.Clear();

            //Time settings
            this.textBox_Template_CRON.Text = null;
            this.dateTimePicker_Template_Expire.Value = null;

            //Notification settings
            this.radioBtn_Template_NotifEnable.IsChecked = false;

            //Exception settings
            this.checkBox_Template_EmptyFolders.IsChecked = false;
            this.textBox_Template_Ignore.Text = "";
            this.textBox_Template_Only.Text = "";
        }
        #endregion
        #region Backup Controls
        private void _loadBackupInfo(Backup b)
        {            
            this._backupTab_DisableComponents();
            if (b != null)
            {
                ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

                this.textBox_Backup_Name.Text = b.Name;
                this.textBox_Backup_Description.Text = b.Description;
                
                //this.textBox_Backup_Template.Text = client.GetTemplateByID(Convert.ToInt32(b.IDBackupTemplate)).Name;

                if(b.BaseFullBackupID != null)
                    this.textBox_Backup_BaseBackup.Text = client.GetBackupByID((long)b.BaseFullBackupID).Name;

                if (b.Compressed)
                    this.radioBtn_Backup_Compress.IsChecked = true;

                if (b.IsDifferential)
                    this.radioBtn_Backup_Diff.IsChecked = true;
                else
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

                client.Close();

                this.label_Backup_ExpireError.Visibility = Visibility.Hidden;
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
            this.textBox_Backup_BaseBackup.IsEnabled = false;
            this.textBox_Backup_Template.IsEnabled = false;
            this.radioBtn_Backup_Full.IsEnabled = false;
            this.radioBtn_Backup_Diff.IsEnabled = false;
            this.radioBtn_Backup_Increm.IsEnabled = false;
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
            this.label_Backup_ExpireError.Visibility = Visibility.Hidden;            
        }
        private void _backupTab_SetDefaultValues()
        {
            this.textBox_Backup_Name.Text = "Backup name";
            this.textBox_Backup_Description.Text = "Backup description";
            this.textBox_Backup_BaseBackup.Text = "";

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
            this.label_Backup_ExpireError.Visibility = Visibility.Hidden;
        }
        private void btn_Backup_Edit_Click(object sender, RoutedEventArgs e)
        {        
            if (this.dataGrid_Backups.SelectedIndex >= 0)
            {
                this.btn_Backup_Save.IsEnabled = true;
                this.btn_Backup_Cancel.IsEnabled = true;
                this.btn_Backup_Edit.IsEnabled = false;

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
            if (this._isBackupExpireDateValid(this.dateTimePicker_Backup_Expire.Value))
            {
                ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
                Backup backup = this.dataGrid_Backups.SelectedItem as Backup;

                backup.Name = this.textBox_Backup_Name.Text;
                backup.Description = this.textBox_Backup_Description.Text;
                backup.Expiration = this.dateTimePicker_Backup_Expire.Value;

                this._backupTab_DisableComponents();
                this.btn_Backup_Edit.IsEnabled = true;                

                client.UpdateBackup(backup);
                client.Close();
            }
            else
                this.label_Backup_ExpireError.Visibility = Visibility.Visible;
        }
        private bool _isBackupExpireDateValid(DateTime? date)
        {
            return date.Value > DateTime.Now.Date ? true : false;
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
        
    }
}
