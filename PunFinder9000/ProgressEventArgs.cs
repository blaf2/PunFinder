using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunFinder9000
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(string info, bool isGreek)
        {
            Info = info;
            IsGreek = isGreek;
        }

        public string Info;
        public bool IsGreek;
    }
}
