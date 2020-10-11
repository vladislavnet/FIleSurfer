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

        }
        public bool IsDirectory { get; private set; }
        public string Size { get; private set; }
        public string Name { get; private set; }
        public string Date { get; private set; }

        private void parsingString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {

            }
        }
    }
}
