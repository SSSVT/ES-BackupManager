﻿using ES_BackupManager.ESBackupServerService;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            this.LoadComponents();
        }

        private void LoadComponents()
        {
            ESBackupServerServiceClient client = new ESBackupServerServiceClient();

            //TODO:Implementovat načítání komponentů, které jsou potřeba

            client.Close();
        }
    }
}
