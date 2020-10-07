﻿using System;
using System.Collections.Generic;
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

        public string[] ListDirectory()
        {
            return null;
        }

        public string[] ListDirectoryDetails()
        {
            return null;
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
    }
}
