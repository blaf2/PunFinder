using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunFinder9000
{
    public class DocResultsEventArgs : EventArgs
    {
        public DocResultsEventArgs(List<Tuple<int, int, string, string, string>> data, string originialText)
        {
            Data = data;
            OriginalText = originialText;
        }

        public List<Tuple<int, int, string, string, string>> Data;
        public string OriginalText;
    }
}
