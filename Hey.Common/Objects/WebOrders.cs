using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public class WebOrders
    {
        public WebOrders()
        {
        }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string OrderType { get; set; }
        public string OrderDescription { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string CustContact { get; set; }
        public string CustPhone { get; set; }
        public string CustEmail { get; set; }
        public string CustRef { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryZipcode { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryCountry { get; set; }
        public string ShippingMethod { get; set; }
        public double ShippingCost { get; set; }
        public double TotalItems { get; set; }
        public double DiscountItems { get; set; }
        public double DiscountOther { get; set; }
        public double TotalVat { get; set; }
        public double TotalOrder { get; set; }
        public double TotalOrderWithVat { get; set; }
        public string Remarks { get; set; }
        public string ProdNotes { get; set; }
        public string SessionId { get; set; }
        public DateTime LastUpdated { get; set; }
        public double Replicated { get; set; }
    }

  //order_id integer NOT NULL DEFAULT nextval('ws_orders_seq'::regclass),
  //order_code character varying(12) NOT NULL,
  //order_type character varying(2),
  //order_descr character varying(255),
  //order_date date DEFAULT now(),
  //order_status character(1) DEFAULT '-'::bpchar,
  //cust_code character varying(12),
  //cust_name character varying(50),
  //cust_contact character varying(50),
  //cust_phone character varying(15),
  //cust_email character varying(255),
  //cust_ref character varying(50),
  //delivery_date date,
  //delivery_address character varying(50),
  //delivery_zipcode character varying(7),
  //delivery_city character varying(35),
  //delivery_country character varying(50),
  //shipping_method character varying(20),
  //shipping_cost numeric(8,2) DEFAULT 0,
  //total_items numeric(8,2) DEFAULT 0,
  //discount_items numeric(8,2) DEFAULT 0,
  //discount_other numeric(8,2) DEFAULT 0,
  //total_vat numeric(8,2) DEFAULT 0,
  //total_order numeric(8,2) DEFAULT 0,
  //total_order_with_vat numeric(8,2) DEFAULT 0,
  //remarks text,
  //prodnotes text,
  //session_id character varying(32),
  //last_updated timestamp without time zone DEFAULT now(),
  //replicated smallint NOT NULL DEFAULT 0,
 
}
