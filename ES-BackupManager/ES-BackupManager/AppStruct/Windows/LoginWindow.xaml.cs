using ES_BackupManager.ESBackupServerAdminService;
using System.Windows;
using System.Windows.Media;

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
            ESBackupServerAdminServiceClient client = new ESBackupServerAdminServiceClient();                                
            if (client.Login(username, password))
            {                                
                MainWindow mw = new MainWindow(client.GetProfile(username));
                mw.Show();
                this.Close();
            }
            client.Close();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            this.Login(this.textBox_Username.Text,this.passwordBox_Password.Password);
        }

        private void passwordBox_Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Login(this.textBox_Username.Text, this.passwordBox_Password.Password);
            }
        }
        private void Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                this.Authorize(username, password);
            }
        }

        private void textBox_Username_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Login(this.textBox_Username.Text, this.passwordBox_Password.Password);
            }
        }
    }
}
