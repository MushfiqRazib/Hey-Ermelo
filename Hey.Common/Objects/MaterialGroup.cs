using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public class MaterialGroup
    {
        public MaterialGroup()
        {
           ChildGroupItems = new List<MaterialGroup>();
           MaterialGroups = new List<BaseMaterial>();
        }
        public string Code { get; set; }
        public string Description {get;set;}
        public int ListPosition {get;set;}
        public int Webshop {get;set;}
        public string ParentItem { get; set; }
        public List<MaterialGroup> ChildGroupItems { get; set; }
        public int ItemCount { get; set; }
        public List<BaseMaterial> MaterialGroups { get; set; }
        
    }
}
