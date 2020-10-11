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
        //Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
        //    RegexOptions.Compiled 
        //    | RegexOptions.Multiline 
        //    | RegexOptions.IgnoreCase 
        //    | RegexOptions.IgnorePatternWhitespace);

      

        //Данные для ананимного входа на сервер
        private string prevAdress = "ftp://";
        private string anonymousLogin = "anonymous";
        private string anonymousPassword = "anonymous@testingdomain.com";
        private HistoryDirectory historyDirectory;
        private string pathIconFolder = "Resources/Img/Folder.ico";
        private string pathIconTXT = "Resources/Img/TXT.ico";

        private Client client;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void connectServer(object sender, RoutedEventArgs e)
        {
            try
            {
                txtAddressServer.Text = convertFTPAddress(txtAddressServer.Text);
                historyDirectory = new HistoryDirectory(txtAddressServer.Text);
                client = createClient(historyDirectory.CurrentDirectory, cbAnonymous.IsChecked);
                lvFiles.DataContext = getListDirectoryDetails(historyDirectory.SecondLastDirectory, historyDirectory.CurrentDirectory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
            }
        }

        private void folderDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount >= 2)
                {
                    FileDirectoryInfo fdi = (FileDirectoryInfo)(sender as StackPanel).DataContext;
                    if (fdi.Type == pathIconFolder && fdi.Name != "...")
                    {
                        openFolder(fdi);
                    }
                    else if (fdi.Type == pathIconFolder && fdi.Name == "...")
                    {
                        historyDirectory.Back();
                        client = createClient(historyDirectory.CurrentDirectory, cbAnonymous.IsChecked);
                        lvFiles.DataContext = getListDirectoryDetails(historyDirectory.SecondLastDirectory, historyDirectory.CurrentDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
            }

        }

        private void openContextMenu(object sender, MouseButtonEventArgs e)
        {            
            FileDirectoryInfo fdi = (FileDirectoryInfo)(sender as StackPanel).DataContext;
            StackPanel itemList = sender as StackPanel;
            if (fdi.Type == pathIconFolder && fdi.Name != "...")
                itemList.ContextMenu = createContextMenuFolder(fdi);
            else if (fdi.Type != pathIconFolder && fdi.Name != "...")
                itemList.ContextMenu = createContextMenuFile(fdi);           
        }



        //private void openFolder_ClickContextMenu(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
        //    }
        //}


        private List<FileDirectoryInfo> getListDirectoryDetails(string addressParent, string addressPath)
        {
            List<FileDirectoryInfo> list = client.ListDirectoryDetails()
                            .Select(x =>
                            {
                                InfoStringFTP info = new InfoStringFTP(x);
                                string type = info.IsDirectory ? pathIconFolder : getTypeImage(string.Empty);
                                return new FileDirectoryInfo(info.Size, type, info.Name, info.Date, addressPath);
                            }).ToList();
            list.Add(new FileDirectoryInfo("", pathIconFolder, "...", "", addressParent));
            list.Reverse();
            return list;
        }

        private string getTypeImage(string value)
        {
            return pathIconTXT;
        }

        private Client createClient(string address, bool? isAnon = true)
        {
            if (isAnon == true)
                return new Client(address, anonymousLogin, anonymousPassword);
            else
                return new Client(address, txtLogin.Text, txtPassword.Password);
        }

        private string convertFTPAddress(string address)
        {
            if (!address.StartsWith(prevAdress))
                address = prevAdress + address;
            return address;
        }

        private string combinePath(string mainPath, string nameDirectory)
        {
            if (mainPath.LastOrDefault() == '/')
                return mainPath + nameDirectory + "/";
            else
                return mainPath + "/" + nameDirectory + "/";
        }

        private void openFolder(FileDirectoryInfo fdi)
        {
            historyDirectory.Add(combinePath(historyDirectory.CurrentDirectory, fdi.Name));
            client = createClient(historyDirectory.CurrentDirectory, cbAnonymous.IsChecked);
            lvFiles.DataContext = getListDirectoryDetails(historyDirectory.SecondLastDirectory, historyDirectory.CurrentDirectory);
        }

        

        private ContextMenu createContextMenuFolder(FileDirectoryInfo fdi)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem openMenuItem = new MenuItem();
            openMenuItem.Header = "Открыть";
            openMenuItem.Click += (sender, e) => openFolder(fdi);
            contextMenu.Items.Add(openMenuItem);
            return contextMenu;
        }

        private ContextMenu createContextMenuFile(FileDirectoryInfo fdi)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem downloadMenuItem = new MenuItem();
            downloadMenuItem.Header = "Скачать";
            contextMenu.Items.Add(downloadMenuItem);
            return contextMenu;
        }
    }
}
