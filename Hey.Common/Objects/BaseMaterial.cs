using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public class BaseMaterial
    {        
        public string Matcode { get;set;}
        public string Description {get; set;}
        public string OrderDescription {get; set;}
        public string SuppCode {get; set;}
        public string PrjCode {get; set;}
        public string EdmCode {get; set;}
        public string Unit {get; set;}
        public string PackUnit {get; set;}
        public double PurchasePrice {get; set;}
        public double Discount1 {get; set;}
        public double Discount2 {get; set;}
        public double Freight {get; set;}
        public double Addon1 {get; set;}
        public double Addon2 {get; set;}
        public double NetPrice {get; set;}
        public double SellPrice {get; set;}
        public double SellPerfect {get; set;}
        public string OrderDate {get; set;}
        public string DeliveryDate {get; set;}
        public double InStock {get; set;}
        public double MinStock {get; set;}
        public double MaxStock {get; set;}
        public double MinOrder {get; set;}
        public string StockLoc {get; set;}
        public int SparePart {get; set;}
        public int WebShop {get; set;}
        public string Remark {get; set;}
        public int MergeCode {get; set;}
        public string DerivedFrom {get; set;}
        public string RemarkEng {get; set;}
        public string KeyWords {get; set;}
        public string Filter1 {get; set;}
        public string Filter2 {get; set;}
        public string Filter3 {get; set;}
        public string Filter4 {get; set;}
        public string Filter5 {get; set;}
        public string UnitSign { get; set; }

        public string NewSuppCode {get; set;}
        public string CustCode {get; set;}
        public string MatcodeSuppl { get; set; }
        public int Deleted {get;set;}
        
    }
}
