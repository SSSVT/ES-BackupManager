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
            string username = this.textBox_Username.Text;
            string password = this.passwordBox_Password.Password;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                this.Authorize(username, password);
            }
        }
    }
}
