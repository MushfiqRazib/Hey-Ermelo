using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public class WebOrderItems
    {
        public WebOrderItems()
        {
        }
        public int OrderId { get; set; }
        public int ItemSeq { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string DerivedFrom { get; set; }
        public string Description { get; set; }
        public string SuppliedCode { get; set; }
        public double Quantity { get; set; }
        public string Units { get; set; }
        public double PriceCost { get; set; }
        public double PriceSell { get; set; }
        public double PriceTotal { get; set; }
        public double Discount { get; set; }
        public string Remarks { get; set; }
        public string ProdNotes { get; set; }
        public int ToEdm { get; set; }
        public int ToArticles { get; set; }
    }

  //order_id integer NOT NULL,
  //item_id integer NOT NULL DEFAULT nextval('ws_order_items_seq'::regclass),
  //item_type character varying(1),
  //item_code character varying(20),
  //derived_from character varying(20),
  //description character varying(255),
  //suppl_code character varying(20),
  //quantity numeric(8,2) DEFAULT 1,
  //units character varying(15),
  //price_cost numeric(8,2) DEFAULT 0,
  //price_sell numeric(8,2) DEFAULT 0,
  //price_total numeric(8,2) DEFAULT 0,
  //discount numeric(4,1) DEFAULT 0,
  //remarks text,
  //prodnotes text,
  //to_edm smallint DEFAULT 0,
  //to_articles smallint DEFAULT 0,
}
