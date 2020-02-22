using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public class MaterialGroupFilter
    {
        public MaterialGroupFilter()
        {
        }
        public int FilterId { get; set; }
        public string GroupCode { get; set; }
        public string FilterLabel { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public int ListPosition { get; set; }
    }
}
