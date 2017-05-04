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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();            
        }

        public void Authorize(string username, string password)
        {
            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();                
            if (client.Login(username, password))
            {
                MessageBox.Show("Authentication was successful. Welcome to Admin! ");
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            client.Close();
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            this.Authorize(this.textBox_Username.Text, this.passwordBox_Password.Password);
        }
    }
}
