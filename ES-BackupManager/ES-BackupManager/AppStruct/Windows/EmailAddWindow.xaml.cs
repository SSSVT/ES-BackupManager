using ES_BackupManager.ESBackupServerAdminService;
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

namespace ES_BackupManager.AppStruct.Windows
{
    /// <summary>
    /// Interaction logic for EmailAddWindow.xaml
    /// </summary>
    public partial class EmailAddWindow : Window
    {
        public EmailAddWindow()
        {
            InitializeComponent();
        }

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
            //TODO: Regex email validation
            return true;
           // return Regex.IsMatch(address,"");
        }
        public Email GetEmail()
        {
            return new Email()
            {
                Address = this.textBox_EmailAdress.Text
            };
        }
    }
}
