using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunFinder9000
{
    public class ListResult : IEquatable<ListResult>
    {
        public string LatinText { get; set; }
        public string MatchedWords { get; set; }
        public int Frequency { get; set; }

        public bool Equals(ListResult other)
        {
            return LatinText == other.LatinText;
        }
    }
}
