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

        //Данные для ананимного входа на сервер
        private string anonymousLogin = "anonymous";
        private string anonymousPassword = "anonymous@testingdomain.com";

        private Client client;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void connectServer(object sender, RoutedEventArgs e)
        {
            try
            {
                client = createClient();
                List<FileDirectoryInfo> list = getListDirectoryDetails();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
            }
        }

        private Client createClient()
        {
            if (!txtAddressServer.Text.StartsWith("ftp://"))
                txtAddressServer.Text += "ftp://" + txtAddressServer.Text;
            if (cbAnonymous.IsChecked == true)
                return new Client(txtAddressServer.Text, anonymousLogin, anonymousPassword);
            else
                return new Client(txtAddressServer.Text, txtLogin.Text, txtPassword.Password);
        }

        private List<FileDirectoryInfo> getListDirectoryDetails()
        {
            List<FileDirectoryInfo> list = client.ListDirectoryDetails()
                            .Select(x =>
                            {
                                Match match = regex.Match(x);
                                if (match.Length > 5)
                                {
                                    string type = isDirectory(match.Groups[1].Value) ? "Resources/Img/Folder.ico" : getTypeImage(match.Groups[1].Value);
                                    string size = string.Empty;
                                    if (!isDirectory(match.Groups[1].Value))
                                        size = (Int32.Parse(match.Groups[3].Value.Trim()) / 1024).ToString() + " кБ";

                                    return new FileDirectoryInfo(size, type, match.Groups[6].Value, match.Groups[4].Value, txtAddressServer.Text);
                                }
                                else return new FileDirectoryInfo();
                            }).ToList();
            list.Add(new FileDirectoryInfo("", "Resources/Img/Folder.ico", "...", "", txtAddressServer.Text));
            return list;
        }

        private bool isDirectory(string value)
        {
            return value == "d";
        }

        private string getTypeImage(string value)
        {
            return "Resources/Img/TXT.ico";
        }
    }
}
