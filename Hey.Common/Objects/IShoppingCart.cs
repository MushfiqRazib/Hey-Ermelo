using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public enum ItemTypeEnum
    {
        STANDARD,
        CUSTOM,
        SPECIAL
    }
    public interface IShoppingCart
    {
        string OrderID { get; set; }
        string ItemID { get; set; }
        string ItemCode { get; set; }
        string ItemType { get; set; }
        string Description { get; set; }
        string Remark { get; set; }
        string Quantity { get; set; }
        string UnitPrice { get; set; }
        string Total { get; set; }
        string ProductNotes { get; set; }
        string Unit { get; set; }
        string PriceCost { get; set; }
        string PriceSell { get; set; }
        string ToEdm { get; set; }
        string ToProduct { get; set; }
        
        void InsertNewItem();
        void DeleteItem();
        void DeleteCart();
        void UpdateItems();
        void UpdateItem();
        IShoppingCart GetACartItem(string item_id);

        List<IShoppingCart> GetShoppingCart(string order_code, ItemTypeEnum iType, UserType uType);
    }
}
