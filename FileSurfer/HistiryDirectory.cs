using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSurfer
{
    public class HistoryDirectory
    {
        private List<string> historyDirectoryList;
        public HistoryDirectory(string uri)
        {
            historyDirectoryList = new List<string>();
            historyDirectoryList.Add(uri);
        }
        public string CurrentDirectory => historyDirectoryList.Last();
        public string SecondLastDirectory
        {
            get
            {
                if (historyDirectoryList.Count > 1)
                    return historyDirectoryList[historyDirectoryList.Count - 2];
                else
                    return CurrentDirectory;
            }
        }

        public string Add(string uri)
        {
            historyDirectoryList.Add(uri);
            return CurrentDirectory;
        }

        public string Back()
        {
            if (historyDirectoryList.Count > 1)
                historyDirectoryList.Remove(historyDirectoryList.Last());
            return CurrentDirectory;           
        }
    }
}
