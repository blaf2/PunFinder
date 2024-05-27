using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunFinder9000
{
    public class ListResultEventArgs : EventArgs
    {
        public ListResultEventArgs(List<ListResult> results)
        {
            Results = results;
        }

        public List<ListResult> Results;
    }
}
