using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hey.Common.Objects;
using System.Data;
using Npgsql;
using Hey.DataAccess.DatabaseManager.DatabaseFactory;
using Hey.DataAccess.SQLStatements;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using System.Security.AccessControl;
using System.Data;

namespace Hey.Business
{

    public class ShoppingCartManager : IShoppingCart
    {
        public string OrderID { get; set; }
        public string ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Total { get; set; }
        public string ProductNotes { get; set; }
        public string Unit { get; set; }
        public string PriceCost { get; set; }
        public string PriceSell { get; set; }
        public string ToEdm { get; set; }
        public string ToProduct { get; set; }

        private static string ACTIVE_DATABASE = Hey.Common.Utils.Functions.GetValueFromWebConfig("activeDB");

        public void InsertNewItem()
        {
        }

        public IShoppingCart GetACartItem(string item_id)
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);

            NpgsqlCommand command = new NpgsqlCommand(SQLClass.SHOPPING_ITEM);
            command.Parameters.Add("item_id", item_id);

            dt = dbObject.GetDataTable(command);

            IShoppingCart sCart = null;
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                sCart = new ShoppingCartManager();
                sCart.ItemCode = dr["item_code"].ToString();
                sCart.ItemType = dr["item_type"].ToString();
                sCart.Description = dr["description"].ToString();
                sCart.Remark = dr["remarks"].ToString();
                sCart.Quantity = dr["quantity"].ToString();
                sCart.ProductNotes = dr["prodNotes"].ToString();
                sCart.Unit = dr["units"].ToString();
                sCart.ToEdm = dr["to_edm"].ToString();
                sCart.ToProduct = dr["to_products"].ToString();
                sCart.PriceCost = dr["price_cost"].ToString();
                sCart.PriceSell = dr["price_sell"].ToString();
            }

            return sCart;
        }

        public void DeleteItem()
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.SHOPPING_ITEMS_DELETE);
            command.Parameters.Add("item_id", this.ItemID);
            dbObject.ExecuteNonQuery(command);
        }

        public void DeleteCart()
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.SHOPPING_ITEMS_DELETE_ALL);
            command.Parameters.Add("order_id", this.OrderID);

            dbObject.ExecuteNonQuery(command);
        }

        public void UpdateItems()
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.SHOPPING_ITEMS_UPDATE);
            command.Parameters.Add("quantity", this.Quantity);
            command.Parameters.Add("item_id", this.ItemID);
            command.Parameters.Add("remark", this.Remark);
            dbObject.ExecuteNonQuery(command);
        }

        public void UpdateItem()
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.SHOPPING_ITEM_UPDATE);
            command.Parameters.Add("quantity", this.Quantity);
            command.Parameters.Add("prodNotes", this.ProductNotes);
            command.Parameters.Add("remarks", this.Remark);
            command.Parameters.Add("item_id", this.ItemID);
            dbObject.ExecuteNonQuery(command);
        }

        public List<IShoppingCart> GetShoppingCart(string sessionid, ItemTypeEnum iType, UserType uType)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            string selectQuery = string.Empty;
            if (iType == ItemTypeEnum.STANDARD && uType == UserType.ANONYMOUS)
            {
                selectQuery = SQLClass.SHOPPING_ITEMS_QUERY;
                command = new NpgsqlCommand(selectQuery);
                command.Parameters.Add("sessionid", sessionid);
            }
            else if(iType == ItemTypeEnum.STANDARD && uType == UserType.REGISTERED)
            {
                selectQuery = SQLClass.SHOPPING_ITEMS_REG_QUERY;
                command = new NpgsqlCommand(selectQuery);
                command.Parameters.Add("cust_code", sessionid);
            }
            else if (iType == ItemTypeEnum.STANDARD && uType == UserType.ADMIN)
            {
                selectQuery = SQLClass.SHOPPING_ITEMS_ADM_QUERY;
                command = new NpgsqlCommand(selectQuery);
                command.Parameters.Add("cust_code", sessionid);
            }
            else
            {
                selectQuery = SQLClass.SHOPPING_ITEMS_QUERY_SPECIAL;
                command = new NpgsqlCommand(selectQuery);
                command.Parameters.Add("cust_code", sessionid);
            }

            List<IShoppingCart> shoppingCart = new List<IShoppingCart>();

            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);          

            dt = dbObject.GetDataTable(command);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    IShoppingCart sCart = new ShoppingCartManager();
                    sCart.ItemID = dr["item_id"].ToString();
                    sCart.OrderID = dr["order_id"].ToString();
                    sCart.Description = dr["description"].ToString();
                    sCart.Remark = dr["remarks"].ToString();
                    sCart.Quantity = dr["quantity"].ToString();
                    sCart.UnitPrice = dr["price_sell"].ToString();
                    sCart.Total = dr["Total"].ToString();
                    shoppingCart.Add(sCart);
                }

            }
            return shoppingCart;
        }

        public DataTable ShopOrderCostDetail(string order_id)
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.SHOPPING_ORDER);
            command.Parameters.Add("order_id", order_id);
            dt = dbObject.GetDataTable(command);
            return dt;
        }

        public void UpdateShopOrderData(string order_id, string cust_ref, string remarks)
        {
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.SHOPPING_ORDER_UPDATE);
            command.Parameters.Add("order_id", order_id);
            command.Parameters.Add("customer_ref", cust_ref);
            command.Parameters.Add("remarks", remarks);
            dbObject.ExecuteNonQuery(command);
        }
        public string CheckCustomItemExists()
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);

            NpgsqlCommand command = new NpgsqlCommand(SQLClass.CHECK_CUSTOM_ITEM_EXIST);
            command.Parameters.Add("order_id", this.OrderID);
            command.Parameters.Add("item_code", this.ItemCode);

            dt = dbObject.GetDataTable(command);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public void UpdateToCustomItem()
        {
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.UPDATE_TO_CUSTOM_ITEM);
            command.Parameters.Add("quantity", this.Quantity);
            command.Parameters.Add("item_id", this.ItemID);
            dbObject.ExecuteNonQuery(command);
        }

        public void UpdateCustomItem()
        {
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.UPDATE_CUSTOM_ITEM);
            command.Parameters.Add("quantity", this.Quantity);
            command.Parameters.Add("item_id", this.ItemID);
            command.Parameters.Add("description", this.Description);
            command.Parameters.Add("price_cost", this.PriceCost);
            command.Parameters.Add("price_sell", this.PriceSell);
            command.Parameters.Add("units", this.Unit);
            command.Parameters.Add("remarks", this.Remark);
            command.Parameters.Add("prodNotes", this.ProductNotes);
            command.Parameters.Add("to_edm", this.ToEdm);
            command.Parameters.Add("to_products", this.ToProduct);
            dbObject.ExecuteNonQuery(command);

            command.CommandText = SQLClass.UPDATE_CUSTOM_ITEM_TOTAL_PRICE;
            dbObject.ExecuteNonQuery(command);

        }

        public void ReduceStandardItemQuanity()
        {
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.REDUCE_STANDARD_ITEM_QUNATITY);
            command.Parameters.Add("quantity", this.Quantity);
            command.Parameters.Add("item_id", this.ItemID);
            dbObject.ExecuteNonQuery(command);
        }

        public void CreateNewCustomizedItem()
        {
            
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.INSERT_CUSTOM_ITEM);
            command.Parameters.Add("item_id", this.ItemID);
            command.Parameters.Add("quantity", this.Quantity);
            command.Parameters.Add("description", this.Description);
            command.Parameters.Add("price_cost", this.PriceCost);
            command.Parameters.Add("price_sell", this.PriceSell);
            command.Parameters.Add("units", this.Unit);
            command.Parameters.Add("remarks", this.Remark);
            command.Parameters.Add("prodnotes", this.ProductNotes);
            command.Parameters.Add("to_edm", this.ToEdm);
            command.Parameters.Add("to_products", this.ToProduct);
            dbObject.ExecuteNonQuery(command);
            NpgsqlCommand commandLastID = new NpgsqlCommand(SQLClass.GET_ORDER_ITEM_LAST_ID);
            DataTable dt = dbObject.GetDataTable(commandLastID);
            if (dt.Rows.Count > 0)
            {
                commandLastID.Parameters.Add("item_id", dt.Rows[0][0].ToString());
                commandLastID.CommandText = SQLClass.UPDATE_CUSTOM_ITEM_TOTAL_PRICE;
                dbObject.ExecuteNonQuery(commandLastID);
            }

        }

        public void ConvertItemToCustomized()
        {
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.CONVERT_TO_CUSTOM);
            command.Parameters.Add("item_id", this.ItemID);
            command.Parameters.Add("quantity", this.Quantity);
            command.Parameters.Add("description", this.Description);
            command.Parameters.Add("price_cost", this.PriceCost);
            command.Parameters.Add("price_sell", this.PriceSell);
            command.Parameters.Add("units", this.Unit);
            command.Parameters.Add("remarks", this.Remark);
            command.Parameters.Add("prodNotes", this.ProductNotes);
            command.Parameters.Add("to_edm", this.ToEdm);
            command.Parameters.Add("to_products", this.ToProduct);
            dbObject.ExecuteNonQuery(command);

            command.CommandText = SQLClass.UPDATE_CUSTOM_ITEM_TOTAL_PRICE;
            dbObject.ExecuteNonQuery(command);
        }

        public void CofirmOrder(string status)
        {
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            NpgsqlCommand command = new NpgsqlCommand(SQLClass.ORDER_CONFIRMATION);
            command.Parameters.Add("order_status", status);
            command.Parameters.Add("order_id", this.OrderID);
            dbObject.ExecuteNonQuery(command);
        }      
    }
    public class UploadedFile
    {
        public string fileName { set; get; }
        public string fileURI { set; get; }
        public static string UploadLocation = ConfigurationSettings.AppSettings["FileUploadRoot"];
        public static List<UploadedFile> GetFileList(string userDirectory)
        {
            string fullLocation = UploadLocation + Path.DirectorySeparatorChar + userDirectory;
            List<UploadedFile> uFileList = new List<UploadedFile>();
            if (Directory.Exists(fullLocation))
            {
                DirectoryInfo dir = new DirectoryInfo(fullLocation);
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    UploadedFile uFile = new UploadedFile() { fileName = file.Name, fileURI = file.FullName };
                    uFileList.Add(uFile);

                }
            }

            return uFileList;
        }

        public static void UploadFile(FileUpload file, string userDirectory)
        {
            string directoryLocation = UploadLocation + Path.DirectorySeparatorChar + userDirectory;
            try
            {
                if (!Directory.Exists(directoryLocation))
                {
                    Directory.CreateDirectory(directoryLocation);
                }

                string fullFileLocation = UploadLocation + Path.DirectorySeparatorChar + userDirectory + Path.DirectorySeparatorChar + file.FileName;
                file.SaveAs(fullFileLocation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    //public enum UserType
    //{
    //    ANONYMOUS,
    //    REGISTERED,
    //    ADMIN
    //}
}
