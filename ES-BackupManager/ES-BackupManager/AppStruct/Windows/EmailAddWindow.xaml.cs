using ESBackupManager.ESBackupServerAdminService;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace ESBackupManager.AppStruct.Windows
{
    /// <summary>
    /// Interaction logic for EmailAddWindow.xaml
    /// </summary>
    public partial class EmailAddWindow : Window
    {
        public EmailAddWindow(long id)
        {
            InitializeComponent();
            this.AdminID = id;
        }
        private long AdminID { get; set; }
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if(this.IsEmailValid(this.textBox_EmailAdress.Text))
                this.DialogResult = true;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
        private bool IsEmailValid(string address)
        {            
            return Regex.IsMatch(address, @"^[a-z0-9]+(\.?[a-z0-9]+)*@([a-z0-9]+\.)+[a-z]{2,}$");
        }
        public Email GetEmail()
        {
            return new Email()
            {
                IDAdministrator = this.AdminID,
                Address = this.textBox_EmailAdress.Text
            };
        }
    }
}
