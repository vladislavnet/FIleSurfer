using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSurfer
{
    public class InfoStringFTP
    {

        public InfoStringFTP(string value)
        {
            parsingString(value);
        }
        public bool IsDirectory { get; private set; }
        public string Size { get; private set; }
        public string Name { get; private set; }
        public string Date { get; private set; }

        private void parsingString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                string[] masValue = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IsDirectory = masValue[0].StartsWith("d") ? true : false;
                Size = !IsDirectory ? (Int32.Parse(masValue[4].Trim()) / 1024).ToString() + " кБ" : string.Empty;
                Date = $"{masValue[5]} {masValue[6]} {masValue[7]}";
                Name = masValue[8];
            }
        }
    }
}
