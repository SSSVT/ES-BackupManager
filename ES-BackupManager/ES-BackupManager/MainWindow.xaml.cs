using ES_BackupManager.AppStruct.Objects;
using ES_BackupManager.AppStruct.Windows;
using ES_BackupManager.ESBackupServerAdminService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ES_BackupManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Administrator admin)
        {
            InitializeComponent();

            this.Administrator = admin;            
            this._loadGrid(Filter.All,Sort.Asc);

            //DEBUG
            //this.TabControl_Main.IsEnabled = true;           
        }

        #region Local Properties
        private Administrator Administrator { get; set; }
        private bool TemplatesLoaded { get; set; }
        private bool BackupsLoaded { get; set; }
        private bool LogsLoaded { get; set; }
        private TemplateInputModes _templateMode;
        private TemplateInputModes TemplateMode
        {
            get { return _templateMode; }
            set
            {
                /* 
                 * VALUES
                 * 0 = NONE
                 * 1 = ADD
                 * 2 = EDIT
                 */
                _templateMode = value;
                if (value == TemplateInputModes.None)
                {
                    this.btn_Template_New.IsEnabled = true;
                    this.btn_Template_StatusChange.IsEnabled = true;
                    this.btn_Template_Edit.Content = "Edit";
                }
                else if (value == TemplateInputModes.Add)
                {
                    this.btn_Template_New.IsEnabled = false;
                    this.btn_Template_StatusChange.IsEnabled = false;
                    this.btn_Template_Edit.Content = "Add";
                }
                else if (value == TemplateInputModes.Edit)
                {
                    this.btn_Template_New.IsEnabled = false;
                    this.btn_Template_StatusChange.IsEnabled = false;
                    this.btn_Template_Edit.Content = "Save";
                }

            }
        }
        #endregion

        #region BindingLists for DataGrids/ListBoxes
        private BindingList<Client> _gridClientsList { get; set; } = new BindingList<Client>();
        private BindingList<BackupInfo> _gridBackupsList { get; set; } = new BindingList<BackupInfo>();
        private BindingList<Log> _gridLogsList { get; set; } = new BindingList<Log>();
        private BindingList<BackupTemplate> _gridTemplatesList { get; set; } = new BindingList<BackupTemplate>();        
        private BindingList<SourcePathInfo> _gridTemplateSourceList { get; set; } = new BindingList<SourcePathInfo>();
        private BindingList<DestinationPathInfo> _gridTemplateDestinationList { get; set; } = new BindingList<DestinationPathInfo>();
        #endregion

        #region Menu Controls
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            AdministratorWindow aw = new AdministratorWindow(this.Administrator);
            aw.Show();
        }
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
                this.listBox_Clients.SelectedIndex = 0;

            this.listBox_Clients.ItemsSource = this._gridClientsList;

            client.Close();
        }
        private void _loadComponents(Client c)
        {
            if (!this.TabControl_Main.IsEnabled)
                this.TabControl_Main.IsEnabled = true;

            this.TemplatesLoaded = false;
            this.BackupsLoaded = false;
            this.LogsLoaded = false;
        }
        private void comboBox_Main_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.comboBox_Main_Sort.IsEnabled = (this.comboBox_Main_Filter.SelectedIndex == 0) ? false : true;
        }
        private void TabControl_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            Client c = this.listBox_Clients.SelectedItem as Client;

            switch (this.TabControl_Main.SelectedIndex)
            {
                case 1:
                    this.LoadTemplatesData(c);
                    break;
                case 2:
                    this.LoadBackupsData(c);
                    break;
                case 3:
                    this.LoadLogsData(c);
                    break;
                default:
                    break;
            }
            client.Close();

        }
        #endregion

        #region Client Controls        
        private void listBox_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.TabControl_Main.IsEnabled = true;            
            this.btn_Client_Edit.IsEnabled = true;

            Client c = this.listBox_Clients.SelectedItem as Client;  
            if(c != null)
            {
                this._loadComponents(c);
                this.LoadClientInfo(c);
            }                                  
        }

        private void _clientTab_DisableComponents()
        {
            this.textBox_Client_Name.IsEnabled = false;
            this.textBox_Client_Description.IsEnabled = false;            
            this.btn_Client_Cancel.IsEnabled = false;
            this.btn_Client_Save.IsEnabled = false;
            this.comboBox_Client_Status.IsEnabled = false;
            this.groupBox_Client_Connection.IsEnabled = false;     
        }
        private void LoadClientInfo(Client c)
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            this._clientTab_DisableComponents();

            this.textBox_Client_Name.Text = c.Name;
            this.textBox_Client_Description.Text = c.Description;

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
                this.radioBtn_Client_ConnDefault.IsChecked = true;

            this.dateTimePicker_Client_LastBackupTime.Value = c.UTCLastBackupTime;
            this.dateTimePicker_Client_LastReportTime.Value = c.UTCLastStatusReportTime;
            this.dateTimePicker_Client_RegistrationDate.Value = c.UTCRegistrationDate;

            client.Close();
        }
        private void btn_Client_Save_Click(object sender, RoutedEventArgs e)
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
            Client c = this.listBox_Clients.SelectedItem as Client;
                        
            c.Description = this.textBox_Client_Description.Text;            
            c.Status = Convert.ToByte(this.comboBox_Client_Status.SelectedIndex);
            c.StatusReportEnabled = (bool)this.radioBtn_Client_ConnSet.IsChecked ? true : false ;
            if (c.StatusReportEnabled)
                c.ReportInterval = (int)this.IntUpDown_Client_StatusRepeat.Value;
                

            this._clientTab_DisableComponents();
            this.btn_Client_Edit.IsEnabled = true;            

            client.UpdateClient(c);
            client.Close();
        }

        private void btn_Client_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Client client = this.listBox_Clients.SelectedItem as Client;            
            this.LoadClientInfo(client);
        }
        private void btn_Client_Edit_Click(object sender, RoutedEventArgs e)
        {                        
            this.textBox_Client_Description.IsEnabled = true;
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
        #endregion
        #region Backup Template Controls
        private void LoadTemplatesData(Client c)
        {
            if (!this.TemplatesLoaded)
            {
                if (this._gridTemplatesList.Count > 0)
                    this._gridTemplatesList.Clear();

            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            this.dataGrid_Template_Source.ItemsSource = this._gridTemplateSourceList;
            this.dataGrid_Template_Destination.ItemsSource = this._gridTemplateDestinationList;
            //TODO: Implement                 
            Configuration config = client.GetConfigurationByClientID(c.ID);

            foreach (BackupTemplate item in config.Templates)
            {                    
                this._gridTemplatesList.Add(item);
            }
            this.dataGrid_Templates.ItemsSource = this._gridTemplatesList;
            this._templateTab_DisableComponents();

            if (this._gridTemplatesList.Count == 0)
            {
                this.dataGrid_Templates.SelectedIndex = -1;
                this.btn_Template_Edit.IsEnabled = false;
            }
            else
            {
                this.dataGrid_Templates.SelectedIndex = 0;
                this.btn_Template_Edit.IsEnabled = true;
            }
            this.TemplatesLoaded = true;

            client.Close();
            }
        }
        private void dataGrid_BackupTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BackupTemplate bt = this.dataGrid_Templates.SelectedItem as BackupTemplate;
            this._loadTemplateInfo(bt);
            e.Handled = true;
        }
        private void btn_Template_SourceAdd_Click(object sender, RoutedEventArgs e)
        {            
            string path = this.textBox_Template_Source.Text;            
            if (this._template_IsPathValid(path))
            {                
                this._gridTemplateSourceList.Add(new SourcePathInfo(path));
                this.textBox_Template_Source.Text = "";
            }
        }
        private void btn_Template_SourceRemove_Click(object sender, RoutedEventArgs e)
        {
            int index = this.dataGrid_Template_Source.SelectedIndex;
            if (index >= 0)
                this._gridTemplateSourceList.RemoveAt(index);
        }
        private void btn_Template_DestinationAdd_Click(object sender, RoutedEventArgs e)
        {
            string path = this.textBox_Template_Dest.Text;
            if (this._template_IsPathValid(path))
            {                
                this._gridTemplateDestinationList.Add(new DestinationPathInfo(path, Convert.ToByte(this.comboBox_Template_DestinationType.SelectedIndex)));
                this.textBox_Template_Dest.Text = "";
            }
        }
        private void btn_Template_DestinationRemove_Click(object sender, RoutedEventArgs e)
        {
            int index = this.dataGrid_Template_Destination.SelectedIndex;
            if (index >= 0)
                this._gridTemplateDestinationList.RemoveAt(index);
        }
        private void btn_Template_New_Click(object sender, RoutedEventArgs e)
        {
            this.TemplateMode = TemplateInputModes.Add;
            this.btn_Template_New.IsEnabled = false;
            this.btn_Template_Edit.IsEnabled = true;
            this._templateTab_EnableComponents();
            this._templateTab_SetDefaultValues();

        }
        private void btn_Template_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show(this,"Are you sure?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes )
            {
                //TODO: Implement - template remove
                Xceed.Wpf.Toolkit.MessageBox.Show(this,"deleted");
            }
        }
        private void btn_Template_Edit_Click(object sender, RoutedEventArgs e)
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            if (this.TemplateMode == TemplateInputModes.Add)
            {
                Client c = this.listBox_Clients.SelectedItem as Client;

                BackupTemplate template = new BackupTemplate();
                template.IDClient = c.ID;
                template.Name = this.textBox_Template_Name.Text;
                template.Description = this.textBox_Template_Description.Text;

                template.BackupType = this._template_GetTemplateType();
                template.Compression = this.radioBtn_Template_Compress.IsChecked == true ? true: false;
                                
                List<BackupTemplatePath> paths = new List<BackupTemplatePath>();
                foreach (SourcePathInfo source in this._gridTemplateSourceList)
                {
                    foreach (DestinationPathInfo dest in this._gridTemplateDestinationList)
                    {
                        paths.Add(new BackupTemplatePath()
                        {
                            Source = source.Value,
                            Destination = dest.Value,
                            PathOrder = Convert.ToInt16(paths.Count),
                            TargetType = dest.TypeByte
                        });
                    }
                }
                template.Paths = paths;

                template.BackupEmptyDirectories = this.checkBox_Template_EmptyFolders.IsChecked == true ? true : false ;

                if (this.checkBox_Template_SearchPattern.IsChecked == true)
                    template.SearchPattern = this.textBox_Template_SearchPattern.Text;
                else
                    template.SearchPattern = "*";

                template.CRONRepeatInterval = this.textBox_Template_CRON.Text;
                if(this.dateTimePicker_Template_Expire.Value != null)
                    template.DaysToExpiration = (uint?)((DateTime)this.dateTimePicker_Template_Expire.Value - DateTime.Now).Days;

                template.IsNotificationEnabled = this.radioBtn_Template_NotifEnable.IsChecked == true ? true :false;
                template.IsEmailNotificationEnabled = this.checkBox_Template_EmailReport.IsChecked == true ? true : false;

                this.TemplateMode = TemplateInputModes.None;
                client.SaveTemplate(template);
                this._gridTemplatesList.Add(template);
                this.dataGrid_Templates.SelectedIndex = this.dataGrid_Templates.Items.Count - 1;

                this.LoadTemplatesData(c);  
            }

            if (this.TemplateMode == TemplateInputModes.Edit)
            {
                //TODO: Edit mode

                BackupTemplate template = this.dataGrid_Templates.SelectedItem as BackupTemplate;

                template.Name = this.textBox_Template_Name.Text;
                template.Description = this.textBox_Template_Description.Text;

                template.BackupType = this._template_GetTemplateType();
                template.Compression = this.radioBtn_Template_Compress.IsChecked == true ? true : false;

                List<BackupTemplatePath> paths = new List<BackupTemplatePath>();
                foreach (SourcePathInfo source in this._gridTemplateSourceList)
                {
                    foreach (DestinationPathInfo dest in this._gridTemplateDestinationList)
                    {
                        paths.Add(new BackupTemplatePath()
                        {
                            IDBackupTemplate = template.ID,
                            Source = source.Value,
                            Destination = dest.Value,
                            PathOrder = Convert.ToInt16(paths.Count),
                            TargetType = dest.TypeByte
                        });
                    }
                }
                template.Paths = paths;

                template.BackupEmptyDirectories = this.checkBox_Template_EmptyFolders.IsChecked == true ? true : false;

                if (this.checkBox_Template_SearchPattern.IsChecked == true)
                    template.SearchPattern = this.textBox_Template_SearchPattern.Text;
                else
                    template.SearchPattern = "*";

                template.CRONRepeatInterval = this.textBox_Template_CRON.Text;
                if (this.dateTimePicker_Template_Expire.Value != null)
                    template.DaysToExpiration = (uint?)((DateTime)this.dateTimePicker_Template_Expire.Value - DateTime.Now).Days;

                template.IsNotificationEnabled = this.radioBtn_Template_NotifEnable.IsChecked == true ? true : false;
                template.IsEmailNotificationEnabled = this.checkBox_Template_EmailReport.IsChecked == true ? true : false;

                client.SaveTemplate(template);
                this._templateTab_DisableComponents();
                this.TemplateMode = TemplateInputModes.None;
            }
            else if (this.TemplateMode == TemplateInputModes.None)
            {
                this.TemplateMode = TemplateInputModes.Edit;
                this._templateTab_EnableComponents();
            }                
            client.Close();
        }

        private void btn_Template_Cancel_Click(object sender, RoutedEventArgs e)
        {
            BackupTemplate bt = this.dataGrid_Templates.SelectedItem as BackupTemplate;

            this.TemplateMode = TemplateInputModes.None;

            if (this._gridTemplatesList.Count == 0)
                this.btn_Template_Edit.IsEnabled = false;

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

            this.btn_Template_New.IsEnabled = true;
            this.btn_Template_Remove.IsEnabled = false;
            this.btn_Template_StatusChange.IsEnabled = false;
            this.btn_Template_Edit.Content = "Edit";
        }
        private void Hyperlink_RequestNavigate_CRONDocumentation(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void Hyperlink_RequestNavigate_CRONGenerator(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void _loadTemplateInfo(BackupTemplate bt)
        {
            this._templateTab_DisableComponents();
            this._gridTemplateSourceList.Clear();
            this._gridTemplateDestinationList.Clear();

            if (bt != null)
            {
                this.btn_Template_Remove.IsEnabled = true;

                this.textBox_Template_Name.Text = bt.Name;
                this.textBox_Template_Description.Text = bt.Description;
                
                switch (bt.BackupType)
                {
                    case 0:
                        this.radioBtn_Template_Full.IsChecked = true;
                        break;
                    case 1:
                        this.radioBtn_Template_Diff.IsChecked = true;
                        break;
                    case 2:
                        this.radioBtn_Template_Increm.IsChecked = true;
                        break;
                    default:
                        break;
                }

                this.radioBtn_Template_Compress.IsChecked = bt.Compression;
                                
                if(bt.Paths != null)
                {               
                    foreach (BackupTemplatePath item in bt.Paths)
                    {           
                        //TODO: Remake source path, ONLY ONE!

                        if(this._gridTemplateSourceList.Where(x => x.Value == item.Source).FirstOrDefault() == null)
                            this._gridTemplateSourceList.Add(new SourcePathInfo(item.Source));

                        this.dataGrid_Template_Source.ItemsSource = this._gridTemplateSourceList;

                        if (this._gridTemplateDestinationList.Where(x => x.Value == item.Destination).FirstOrDefault() == null)
                            this._gridTemplateDestinationList.Add(new DestinationPathInfo(item.Destination, item.TargetType));
                        this.dataGrid_Template_Destination.ItemsSource = this._gridTemplateDestinationList;
                    }
                }

                this.checkBox_Template_EmptyFolders.IsChecked = bt.BackupEmptyDirectories;
                if (!string.IsNullOrWhiteSpace(bt.SearchPattern))
                {
                    this.checkBox_Template_SearchPattern.IsChecked = true;
                    this.textBox_Template_SearchPattern.Text = bt.SearchPattern;
                }

                this.textBox_Template_CRON.Text = bt.CRONRepeatInterval;
                if (bt.DaysToExpiration != null)
                    this.dateTimePicker_Template_Expire.Value = DateTime.Now.AddDays(Convert.ToDouble(bt.DaysToExpiration));
                else
                    this.dateTimePicker_Template_Expire.Value = null;

                this.radioBtn_Template_NotifEnable.IsChecked = bt.IsNotificationEnabled;
                this.checkBox_Template_EmailReport.IsChecked = bt.IsEmailNotificationEnabled;

                this.btn_Template_StatusChange.Content = bt.Enabled ? "Disable" : "Enable";
                this.btn_Template_StatusChange.IsEnabled = true;
            }
            else
                this._templateTab_SetDefaultValues();
        }
        private void checkBox_Template_SearchPattern_Checked(object sender, RoutedEventArgs e)
        {
            if (this.checkBox_Template_SearchPattern.IsChecked == true)
                this.textBox_Template_SearchPattern.IsEnabled = true;
            else
                this.textBox_Template_SearchPattern.IsEnabled = false;
        }

        private void btn_Template_StatusChange_Click(object sender, RoutedEventArgs e)
        {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            BackupTemplate template = this.dataGrid_Templates.SelectedItem as BackupTemplate;
            
            if(template.Enabled)
            {
                this.btn_Template_StatusChange.Content = "Enable";
                template.Enabled = false;
                client.SetTemplateStatus(template.ID, false);
            }
            else if(!template.Enabled)
            {             
                this.btn_Template_StatusChange.Content = "Disable";
                template.Enabled = true;
                client.SetTemplateStatus(template.ID, true);
            }                        

            client.Close();
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
            this.checkBox_Template_SearchPattern.IsChecked = false;
            this.textBox_Template_SearchPattern.Text = "";
        }
        private byte _template_GetTemplateType()
        {
            if (this.radioBtn_Template_Full.IsChecked == true)
                return 0;
            else if (this.radioBtn_Template_Diff.IsChecked == true)
                return 1;
            else
                return 2;
        }        
        private bool _template_IsPathValid(string path)
        {
            //TODO: Implement Path validation
            if (Regex.IsMatch(path, @"^(([a-zA-Z]:|\\\\\w[ \w\.]*)(\\\w[ \w\.]*|\\%[ \w\.]+%+)+|%[ \w\.]+%(\\\w[ \w\.]*|\\%[ \w\.]+%+)*)"))
                return true;
            else
                return false; 
        }
        private void comboBox_Template_CRONTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.comboBox_Template_CRONTemplates.SelectedIndex;
            switch (index)
            {
                case 1:
                    this.textBox_Template_CRON.Text = "0 0 0/1 1/1 * ? *";                    
                    break;
                case 2:
                    this.textBox_Template_CRON.Text = "0 0 12 1/1 * ? *";                    
                    break;
                case 3:
                    this.textBox_Template_CRON.Text = "0 0 12 ? * SUN *";                    
                    break;
                case 4:
                    this.textBox_Template_CRON.Text = "0 0 12 ? 1/1 SUN#1 *";                    
                    break;
                default:                    
                    break;
            }            
        }
        private void textBox_Template_CRON_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = this.textBox_Template_CRON.Text;
            switch (text)
            {
                case "0 0 0/1 1/1 * ? *":
                    this.comboBox_Template_CRONTemplates.SelectedIndex = 1;
                    break;
                case "0 0 12 1/1 * ? *":
                    this.comboBox_Template_CRONTemplates.SelectedIndex = 2;
                    break;
                case "0 0 12 ? * SUN *":
                    this.comboBox_Template_CRONTemplates.SelectedIndex = 3;
                    break;
                case "0 0 12 ? 1/1 SUN#1 *":
                    this.comboBox_Template_CRONTemplates.SelectedIndex = 4;
                    break;
                default:
                    this.comboBox_Template_CRONTemplates.SelectedIndex = 0;
                    break;
            }            
        }
        #endregion
        #region Backup Controls
        private void LoadBackupsData(Client c)
        {
            if (!this.BackupsLoaded)
            {           
                if(this._gridBackupsList.Count > 0)
                    this._gridBackupsList.Clear();

            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            foreach (BackupInfo item in client.GetBackupsByClientID(c.ID))
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

            client.Close();
            }
        }
        private void _loadBackupInfo(BackupInfo b)
        {            
            this._backupTab_DisableComponents();
            if (b != null)
            {
                ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

                this.textBox_Backup_Name.Text = b.Name;
                this.textBox_Backup_Description.Text = b.Description;
                
                //this.textBox_Backup_Template.Text = client.GetTemplateByID(Convert.ToInt32(b.IDBackupTemplate)).Name;
               
                
                if(b.BaseBackupID != null)
                    this.textBox_Backup_BaseBackup.Text = client.GetBackupByID((long)b.BaseBackupID).Name;

                if (b.Compressed)
                    this.radioBtn_Backup_Compress.IsChecked = true;

                switch (b.BackupType)
                {
                    case 0:
                        this.radioBtn_Backup_Full.IsChecked = true;
                        break;
                    case 1:
                        this.radioBtn_Backup_Diff.IsChecked = true;
                        break;
                    case 2:
                        this.radioBtn_Backup_Increm.IsChecked = true;
                        break;
                    default:
                        break;
                }

                this.textBox_Backup_Source.Text = b.Source;
                this.textBox_Backup_Dest.Text = b.Destination;

                if (b.UTCExpiration == null)
                {
                    this.dateTimePicker_Backup_Expire.Watermark = "Expire date is not set.";
                    this.dateTimePicker_Backup_Expire.Value = null;
                }
                else
                    this.dateTimePicker_Backup_Expire.Value = b.UTCExpiration;

                this.dateTimePicker_Backup_Start.Value = b.UTCStart;

                if (b.UTCEnd == null)
                    this.dateTimePicker_Backup_End.Watermark = "End time is not set.";
                else
                    this.dateTimePicker_Backup_End.Value = b.UTCEnd;

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
            BackupInfo b = this.dataGrid_Backups.SelectedItem as BackupInfo;
            this._loadBackupInfo(b);
            e.Handled = true;
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

            this.btn_Template_Remove.IsEnabled = false;
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
            BackupInfo backup = this.dataGrid_Backups.SelectedItem as BackupInfo;            
            this._loadBackupInfo(backup);
        }
        private void btn_Backup_Save_Click(object sender, RoutedEventArgs e)
        {
            if (this._isBackupExpireDateValid(this.dateTimePicker_Backup_Expire.Value))
            {
                ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();
                BackupInfo backup = this.dataGrid_Backups.SelectedItem as BackupInfo;

                backup.Name = this.textBox_Backup_Name.Text;
                backup.Description = this.textBox_Backup_Description.Text;
                backup.UTCExpiration = this.dateTimePicker_Backup_Expire.Value;

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
        private void LoadLogsData(Client c)
        {
            this._gridLogsList.Clear();

            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();

            //TODO: Implement

            foreach (Log item in client.GetLogsByClientID(c.ID))
            {
                this._gridLogsList.Add(item);
            }
            this.dataGrid_Logs.ItemsSource = this._gridLogsList;

            this._logTab_DisableComponents();

            client.Close();
        }
        private void _logTab_DisableComponents()
        {
            this.textBox_Log_BackupName.IsEnabled = false;
            this.comboBox_Log_LogType.IsEnabled = false;
            this.dateTimePicker_Log_Time.IsEnabled = false;
        }

        #endregion
    }
}
