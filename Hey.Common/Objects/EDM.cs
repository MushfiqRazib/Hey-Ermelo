using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public class EDM
    {
        public string EDMCode {get;set;}
        public string Description {get;set;}
        public string Revision {get;set;}
        public string CustCode {get;set;}
        public string Format {get;set;}
        public string Scale {get;set;}
        public DateTime FirstPubl {get;set;}
        public DateTime LastPubl {get;set;}
        public string Remarks {get;set;}
        public string Author {get;set;}
        public DateTime LastEdited {get;set;}
        public string LastEditedBy {get;set;}
        public int Deleted {get;set;}
    }
}
