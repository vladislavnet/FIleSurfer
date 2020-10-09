using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileSurfer
{
    internal class Client
    {
        private string uri;
        private string login;
        private string password;
        private int bufferSize = 1024;

        public Client(string uri, string login, string password)
        {
            this.uri = uri;
            this.login = login;
            this.password = password;
        }

        public bool IsPassive { get; set; } = true;
        public bool IsBinary { get; set; } = true;
        public bool IsEnableSSL { get; set; } = false;
        public bool IsHash { get; set; } = false;

        public string ChangeWorkingDirectory(string path)
        {
            uri = combinePath(uri, path);

            return PrintWorkingDirectory();
        }

        public string PrintWorkingDirectory()
        {
            var request = createRequest(WebRequestMethods.Ftp.PrintWorkingDirectory);

            return getStatus(request);
        }

        public long GetFileSize(string fileName)
        {
            var request = createRequest(combinePath(uri, fileName), WebRequestMethods.Ftp.GetFileSize);

            long contentLegnth = 0;
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                contentLegnth =  response.ContentLength;
            }
            return contentLegnth;
        }

        public DateTime GetDateTimestamp(string fileName)
        {
            var request = createRequest(combinePath(uri, fileName), WebRequestMethods.Ftp.GetDateTimestamp);
            DateTime lastModify;
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                lastModify = response.LastModified;
            }
            return lastModify;
        }

        public string DownloadFile(string source, string dest)
        {
            var request = createRequest(combinePath(uri, source), WebRequestMethods.Ftp.DownloadFile);
            string statusRespose = string.Empty;
            byte[] buffer = new byte[bufferSize];

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(dest, FileMode.OpenOrCreate))
                    {
                        int readCount = stream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            if (IsHash)
                                Console.Write("#");

                            fs.Write(buffer, 0, readCount);
                            readCount = stream.Read(buffer, 0, bufferSize);
                        }
                    }
                }

                statusRespose = response.StatusDescription;
            }

            return statusRespose;
        }

        public string DeleteFile(string fileName)
        {
            var request = createRequest(combinePath(uri, fileName), WebRequestMethods.Ftp.DeleteFile);

            return getStatus(request);
        }

        public string Rename(string currentName, string newName)
        {
            var request = createRequest(combinePath(uri, currentName), WebRequestMethods.Ftp.Rename);
            request.RenameTo = newName;

            return getStatus(request);
        }

        public string MakeDirectory(string directoryName)
        {
            var request = createRequest(combinePath(uri, directoryName), WebRequestMethods.Ftp.MakeDirectory);

            return getStatus(request);
        }

        public string RemoveDirectory(string directoryName)
        {
            var request = createRequest(combinePath(uri, directoryName), WebRequestMethods.Ftp.RemoveDirectory);

            return getStatus(request);
        }

        public string[] ListDirectory()
        {
            FtpWebRequest request = createRequest(WebRequestMethods.Ftp.ListDirectory);

            List<string> directories = new List<string>();

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            directories.Add(reader.ReadLine());
                        }
                    }
                }
            }

            return directories.ToArray();
        }

        public string[] ListDirectoryDetails()
        {
            var request = createRequest(WebRequestMethods.Ftp.ListDirectoryDetails);

            var directoryDetails = new List<string>();

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            directoryDetails.Add(reader.ReadLine());
                        }
                    }
                }
            }

            return directoryDetails.ToArray();
        }


        private FtpWebRequest createRequest(string method)
        {
            return createRequest(uri, method);
        }

        private FtpWebRequest createRequest(string uri, string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);

            request.Credentials = new NetworkCredential(login, password);
            request.Method = method;
            request.UseBinary = IsBinary;
            request.EnableSsl = IsEnableSSL;
            request.UsePassive = IsPassive;

            return request;
        }

        private string getStatus(FtpWebRequest request)
        {
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusDescription;
            }
        }

        private string combinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2).Replace("\\", "/");
        }
    }
}
