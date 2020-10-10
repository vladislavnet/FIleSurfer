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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileSurfer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Регулярное выражение, которое ищет информацию о папках и файлах 
        // в строке ответа от сервера
        Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
            RegexOptions.Compiled 
            | RegexOptions.Multiline 
            | RegexOptions.IgnoreCase 
            | RegexOptions.IgnorePatternWhitespace);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void connectServer(object sender, RoutedEventArgs e)
        {
            try
            {
                Client client = new Client(txtAddressServer.Text, txtLogin.Text, txtPassword.Password);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
            }
        }
    }
}
