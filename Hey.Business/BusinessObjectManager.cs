using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Hey.DataAccess.DatabaseManager.DatabaseFactory;
using Hey.DataAccess.SQLStatements;
using Hey.Common.Objects;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using Npgsql;


namespace Hey.Business
{
    public class BusinessObjectManager
    {
        public static string ACTIVE_DATABASE = Hey.Common.Utils.Functions.GetValueFromWebConfig("activeDB");

        //public static string ACTIVE_DATABASE = "postgres";
        public BusinessObjectManager()
        {
        }

        public static HCustomers GetCustomerDataByLogin(string username, string password)
        {
            HCustomers CustomerObject = new HCustomers();
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            Npgsql.NpgsqlCommand command = new Npgsql.NpgsqlCommand(SQLClass.GET_CUSTOMERINFO);
            command.Parameters.Add("email", username.ToLower());
            command.Parameters.Add("webshop_pwd", password);
            dt = dbObject.GetDataTable(command);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CustomerObject.RelCode = dr["rel_code"].ToString();
                    CustomerObject.WebShopRole = int.Parse(dr["webshop_role"].ToString());
                    CustomerObject.OldCode = dr["old_code"].ToString();
                    CustomerObject.Remarks = dr["remarks"].ToString();
                    CustomerObject.RelName = dr["rel_name"].ToString();
                    CustomerObject.Reference = dr["reference"].ToString();
                    CustomerObject.Suspect = dr["suspect"].ToString();
                    CustomerObject.CompanyType = dr["companytype"].ToString();
                    CustomerObject.Archive = dr["archive"].ToString();
                    CustomerObject.Contact = dr["contact"].ToString();
                    CustomerObject.CellPhone = dr["cellphone"].ToString();
                    CustomerObject.Email = dr["email"].ToString();
                    CustomerObject.Address = dr["v_address"].ToString();
                    CustomerObject.City = dr["v_city"].ToString();
                    CustomerObject.ZipCode = dr["v_zipcode"].ToString();
                    CustomerObject.Website = dr["website"].ToString();
                    CustomerObject.Phone = dr["phone"].ToString();
                    CustomerObject.Password = dr["webshop_pwd"].ToString();

                    CustomerObject.VisitAddress = dr["v_address"].ToString();
                    CustomerObject.VisitZipcode = dr["v_zipcode"].ToString();
                    CustomerObject.DeliveryAddress = dr["d_address"].ToString();
                    CustomerObject.DeliveryZipcode = dr["d_zipcode"].ToString();
                    
                    break;
                }
                return CustomerObject;
            }
            else
            {
                return CustomerObject = null;
            }
            
        }

        public static bool SendRequestMailToUser(EmailInfo eInfo)
        {
            bool success = false;
            string adminMail = Hey.Common.Utils.Functions.GetValueFromWebConfig("hey-admin-mail");            
            try
            {
                success = RequestMailFromUser(eInfo, adminMail);
                success = ConfirmationMailToUser(eInfo, adminMail);
            }
            catch(Exception ex) 
            {
                throw new Exception("The email is unknown to the server.");
            }

            return success;
        }

        private static bool ConfirmationMailToUser(EmailInfo eInfo, string adminMail)  // Mail from Hey Admin to CUstomer (reg/ano)
        {
            try
            {
                MailMessage messageInfo = new MailMessage(adminMail, eInfo.Email);
                messageInfo.Subject = "Confirmation mail from HEY";
                messageInfo.Body = "Dear " + eInfo.CompanyName + ",<br/><br/>" +
                                   "       Your request mail to create account has been sent.<br/>" +
                                   " The login credentials will be sent to the following mail: " + eInfo.Email +
                                   " <br/><br/> With kind regards,<br/> <a href='new.hey-ermelo.nl'>HEY</a> ";
                SendMail(messageInfo);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(SendMail), messageInfo); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Mail is failed to deliver.");
            }
            return true;
        }

        private static bool RequestMailFromUser(EmailInfo eInfo, string adminMail)  // Mail from CUstomer (reg/ano) to admin
        {
            try
            {
                MailMessage messageInfo = new MailMessage(eInfo.Email, adminMail);
                messageInfo.Subject = "Request mail from User";
                messageInfo.Body = "Dear HEY,<br/><br/>" +
                                   eInfo.CompanyName + " wants to create an account .<br/>" +
                                   " The login mail: " + eInfo.Email +
                                   " <br/><br/> With kind regards,<br/> " + eInfo.CompanyName;
                SendMail(messageInfo);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(SendMail), messageInfo); 
            }
            catch(Exception ex)
            {
                throw new Exception("Error: Mail is failed to deliver.");
            }
            return true;
        }

        public static bool OrderConfirmationToAdmin(EmailInfo eInfo, string adminMail,string message)
        {
            try
            {
                MailMessage messageInfo = new MailMessage(eInfo.Email, adminMail);
                messageInfo.Subject = "Order Confirmed";
                messageInfo.Body = "Dear HEY,<br/><br/>" + message;
                SendMail(messageInfo);
               
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Mail is failed to deliver.");
            }
            return true;
        }
        
        public static bool OrderFinalizeToCustomer(EmailInfo eInfo, string adminMail, string message)
        {
            try
            {
                MailMessage messageInfo = new MailMessage(adminMail,eInfo.Email);
                messageInfo.Subject = "Order Finalized";
                messageInfo.Body = "Dear Customer,<br/><br/>" + message;
                SendMail(messageInfo);                 
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Mail is failed to deliver.");
            }
            return true;
        }

        public static bool OrderConfirmationToCustomer(EmailInfo eInfo, string adminMail, string message)
        {
            try
            {
                MailMessage messageInfo = new MailMessage(adminMail, eInfo.Email);
                messageInfo.Subject = "Order Confirmation (Offer/Order)";
                messageInfo.Body = "Dear Customer,<br/><br/>" + message;
                SendMail(messageInfo);

            }
            catch (Exception ex)
            {
                throw new Exception("Error: Mail is failed to deliver.");
            }
            return true;
        }

        private static bool SendPasswordRecoveryMailToUser(EmailInfo eInfo, string adminMail)
        {
            try
            {
                MailMessage messageInfo = new MailMessage(adminMail, eInfo.Email);
                messageInfo.Subject = "Password Recovery mail from HEY";
                messageInfo.Body = "Dear " + eInfo.CompanyName + ",<br/><br/>" +
                                   " The password is: " + eInfo.Password +
                                   " <br/><br/> With kind regards,<br/>  <a href='new.hey-ermelo.nl'>HEY</a> ";
                SendMail(messageInfo);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(SendMail), messageInfo); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Mail is failed to deliver.");
            }
            return true;
        }
        public static bool SendPasswordRecoveryMail(EmailInfo eInfo)
        {
            bool success = false;           
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            Npgsql.NpgsqlCommand command = new Npgsql.NpgsqlCommand(SQLClass.GET_FORGETUSERPASSWORD);
            command.Parameters.Add("email", eInfo.Email.ToLower());            
            dt = dbObject.GetDataTable(command);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    eInfo.Email = dr["email"].ToString();
                    eInfo.Password = dr["webshop_pwd"].ToString();
                    eInfo.CompanyName = dr["companytype"].ToString();
                }
                string adminMail = Hey.Common.Utils.Functions.GetValueFromWebConfig("hey-admin-mail");
                try
                {
                    success = SendPasswordRecoveryMailToUser(eInfo, adminMail);
                }
                catch (Exception ex)
                {
                    throw new Exception("The email is unknown to the server.");
                }
            }
            else
            {
                success = false;
            }

            return success;
        }

        private static void SendMail(object message)
        {
            try
            {
                MailMessage msg = message as MailMessage;
                msg.IsBodyHtml = true;
                string mailServer = Hey.Common.Utils.Functions.GetValueFromWebConfig("mail-server");
                string credentialUser = Hey.Common.Utils.Functions.GetValueFromWebConfig("credential-user");
                string credentialPassword = Hey.Common.Utils.Functions.GetValueFromWebConfig("credential-password");
                SmtpClient client = new SmtpClient(mailServer);
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential(credentialUser, credentialPassword);
                client.Credentials = credential;
                client.Send(msg);
            }
            catch (Exception exp)
            {
                Hey.Common.Utils.LogWriter.Log("Exception: " + exp.InnerException + "StackTrace: " + exp.StackTrace);
                throw new Exception("Sending mail failed.");
            }
        }

        public static List<MaterialGroup> GetMenuItems()
        {
            List<MaterialGroup> mGroups = new List<MaterialGroup>();
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);            
            dt = dbObject.GetDataTable(SQLClass.GET_GROUPS);
            DataView dv = new DataView(dt);
            
            DataTable dtP = dv.ToTable(true, new string[] {"parentitem"});
            if (dtP.Rows.Count > 0)
            {
                foreach (DataRow parentRow in dtP.Rows)
                {
                    MaterialGroup mGroup = new MaterialGroup();
                    mGroup.ParentItem = parentRow["parentitem"].ToString();
                    DataRow[] dtChild = dt.Select("parentitem ='" + mGroup.ParentItem + "'");
                    foreach (DataRow row in dtChild)
                    {
                        mGroup.Code = row["code"].ToString();
                        if (!mGroup.Code.ToUpper().Equals(mGroup.ParentItem.ToUpper()))
                        {
                            MaterialGroup childGroup = new MaterialGroup();
                            childGroup.Code = row["code"].ToString();
                            childGroup.Description = row["description"].ToString();
                            childGroup.ListPosition = int.Parse(row["list_position"].ToString());
                            childGroup.Webshop = int.Parse(row["webshop"].ToString());
                            mGroup.ChildGroupItems.Add(childGroup);
                        }
                        else
                        {
                            mGroup.Description = row["description"].ToString();
                            mGroup.ListPosition = int.Parse(row["list_position"].ToString());
                            mGroup.Webshop = int.Parse(row["webshop"].ToString());
                            mGroup.ParentItem = row["parentitem"].ToString();
                        }                       
                    }
                    mGroups.Add(mGroup);
                }
            }
            return mGroups;      
        }

        public static List<MaterialGroupFilter> GetNumberOfFilterCombos(string code)
        {
            List<MaterialGroupFilter> ListOfFilters = new List<MaterialGroupFilter>();
            MaterialGroupFilter FilterObject; 
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            Npgsql.NpgsqlCommand command = new Npgsql.NpgsqlCommand(SQLClass.GET_NUMBEROFFIILTERCOMBO);
            command.Parameters.Add("group_code", code.ToUpper());
            dt = dbObject.GetDataTable(command);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    FilterObject = new MaterialGroupFilter();
                    FilterObject.FilterId = int.Parse(dr["filter_id"].ToString());
                    FilterObject.FilterLabel = dr["description"].ToString();
                    FilterObject.FieldName = dr["field_name"].ToString();
                    FilterObject.TableName = dr["table_name"].ToString();
                    FilterObject.ListPosition = int.Parse(dr["list_position"].ToString());
                    ListOfFilters.Add(FilterObject);          
                }
            }
            return ListOfFilters;  
        }

        public static DropDownList CreateFilterCombo(MaterialGroupFilter filterCombo)
        {
            DataTable dt = new DataTable();
            DropDownList drp = new DropDownList();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            string query = "SELECT DISTINCT " + filterCombo.FieldName +
                           " FROM " + filterCombo.TableName + " ORDER BY " + filterCombo.FieldName;            
            dt = dbObject.GetDataTable(query);
            drp = CreateDropdownList(dt);
            drp.Items.Insert(0, new ListItem(""));
            return drp;
        }

        public static DropDownList CreateDropdownList(DataTable dt)
        {
            DropDownList drp = new DropDownList();
            drp.ID = dt.Columns[0].ColumnName;            
            drp.DataSource = dt;
            drp.DataTextField = dt.Columns[0].ToString();
            drp.DataValueField = dt.Columns[0].ToString();
            drp.DataBind();            
            return drp;
        }

        public static List<BaseMaterial> GetFilterData(string code, string fieldnames, string fieldvalues)
        {
            List<BaseMaterial> BMList = new List<BaseMaterial>();
            BaseMaterial bm;
            string query = SQLClass.FILTER_QUERY + " WHERE matcode LIKE '"+code.ToUpper()+ "%' AND webshop = 1 ";
            string[] fNames = null;
            string[] fValues = null;
            if (!String.IsNullOrEmpty(fieldnames))
            {
                fNames = fieldnames.Split(',');
            }
            if (!String.IsNullOrEmpty(fieldvalues))
            {
                fValues = fieldvalues.Split(',');
            }

            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            if (fNames != null)
            {
                for (int i = 0; i < fNames.Length; i++)
                {
                    query += " AND " + fNames[i] + " LIKE '" + fValues[i] + "%' ";
                }
            }
            query += " ORDER BY matcode";

            dt = dbObject.GetDataTable(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //matcode, description, unit, sellprice
                    bm = new BaseMaterial();                    
                    bm.Matcode = dr["matcode"].ToString();
                    bm.Description = dr["description"].ToString();
                    bm.Unit = dr["unit"].ToString();
                    bm.UnitSign = "$";
                    if (String.IsNullOrEmpty(dr["sellprice"].ToString()))
                    {
                        bm.SellPrice = 0.0;
                    }
                    else
                    {
                        bm.SellPrice = double.Parse(dr["sellprice"].ToString());
                    }
                    if (String.IsNullOrEmpty(dr["purchaseprice"].ToString()))
                    {
                        bm.PurchasePrice = 0.0;
                    }
                    else
                    {
                        bm.PurchasePrice = double.Parse(dr["purchaseprice"].ToString());
                    }
                                        
                    BMList.Add(bm);
                }
            }
            return BMList;
        }

        public static List<MaterialGroup> GetSearchFilterData(string input)
        {
            List<MaterialGroup> mGroups = new List<MaterialGroup>();
            string query = SQLClass.SEARCHQUERY;       
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("searchstring", input.ToLower());
            command.CommandText = query;           
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);                      
            dt = dbObject.GetDataTable(command);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    MaterialGroup mGroup = new MaterialGroup();
                    mGroup.Code = row["matgroup"].ToString();
                    mGroup.Description = row["description"].ToString();
                    mGroup.ItemCount = int.Parse(row["count"].ToString());                    
                    mGroups.Add(mGroup);                    
                }
            }
            return mGroups;
        }
        
        public static List<BaseMaterial> GetSubgroupFilterSearchData(string code)
        {
            List<BaseMaterial> BMList = new List<BaseMaterial>();
            BaseMaterial bm;
            //string query = SQLClass.FILTER_QUERY + " WHERE matcode LIKE '" + code.ToUpper() + "%' AND webshop = 1 ORDER BY matcode ";
            string query = SQLClass.ITEMGROUPDATAQUERY;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("code", code.ToUpper());
            command.CommandText = query;  
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            //dt = dbObject.GetDataTable(query);
            dt = dbObject.GetDataTable(command);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //matcode, description, unit, sellprice
                    bm = new BaseMaterial();
                    bm.Matcode = dr["matcode"].ToString();
                    bm.Description = dr["description"].ToString();
                    bm.Unit = dr["unit"].ToString();
                    bm.UnitSign = "$";
                    if (String.IsNullOrEmpty(dr["sellprice"].ToString()))
                    {
                        bm.SellPrice = 0.0;
                    }
                    else
                    {
                        bm.SellPrice = double.Parse(dr["sellprice"].ToString());
                    }

                    BMList.Add(bm);
                }
            }
            return BMList;
        }

        public static List<BaseMaterial> FilterPanelSearchData(string code, string searchstring)
        {
            List<BaseMaterial> BMList = new List<BaseMaterial>();
            BaseMaterial bm;
            string query = SQLClass.FILTERSEARCHQUERY;
            //string query = " select matcode, description, unit, sellprice from basematerial WHERE (matcode LIKE '" + code.ToUpper() + "%' AND webshop = 1 ) AND "+
            //    " ( matcode LIKE '%"+ searchstring.ToUpper() + "%' or description LIKE '%" + searchstring.ToLower() + "%' OR unit LIKE '%" + searchstring.ToLower() +
            //    "%' OR to_char(sellprice, '9999.99') like '%"+ searchstring.ToLower() +"%') ";
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("searchstring", searchstring.ToLower());
            command.CommandText = query;  

            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            //dt = dbObject.GetDataTable(query);
            dt = dbObject.GetDataTable(command);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //matcode, description, unit, sellprice
                    bm = new BaseMaterial();
                    bm.Matcode = dr["matcode"].ToString();
                    bm.Description = dr["description"].ToString();
                    bm.Unit = dr["unit"].ToString();
                    bm.UnitSign = "$";
                    if (String.IsNullOrEmpty(dr["sellprice"].ToString()))
                    {
                        bm.SellPrice = 0.0;
                    }
                    else
                    {
                        bm.SellPrice = double.Parse(dr["sellprice"].ToString());
                    }

                    BMList.Add(bm);
                }
            }
            return BMList;
        }

        public static int InsertShoppingOrder(WebOrders webOrder)
        {
            int OrderId = 0;
            try
            {
                string query = SQLClass.INSERT_INTO_WEB_ORDER;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_code", webOrder.OrderCode);
                command.Parameters.Add("order_type", webOrder.OrderType);
                command.Parameters.Add("order_descr", webOrder.OrderDescription);
                command.Parameters.Add("order_date", webOrder.OrderDate);
                command.Parameters.Add("order_status", webOrder.OrderStatus);
                command.Parameters.Add("cust_code", webOrder.CustCode);
                command.Parameters.Add("cust_name", webOrder.CustName);
                command.Parameters.Add("cust_contact", webOrder.CustContact);
                command.Parameters.Add("cust_phone", webOrder.CustPhone);
                command.Parameters.Add("cust_email", webOrder.CustEmail);
                command.Parameters.Add("cust_ref", webOrder.CustRef);
                command.Parameters.Add("delivery_date", webOrder.DeliveryDate);
                command.Parameters.Add("delivery_address", webOrder.DeliveryAddress);
                command.Parameters.Add("delivery_zipcode", webOrder.DeliveryZipcode);
                command.Parameters.Add("delivery_city", webOrder.DeliveryCity);
                command.Parameters.Add("delivery_country", webOrder.DeliveryCountry);
                command.Parameters.Add("shipping_method", webOrder.ShippingMethod);
                command.Parameters.Add("shipping_cost", webOrder.ShippingCost);
                command.Parameters.Add("total_items", webOrder.TotalItems);
                command.Parameters.Add("discount_items", webOrder.DiscountItems);
                command.Parameters.Add("discount_other", webOrder.DiscountOther);
                command.Parameters.Add("total_vat", webOrder.TotalVat);
                command.Parameters.Add("total_order", webOrder.TotalOrder);
                command.Parameters.Add("total_order_with_vat", webOrder.TotalOrderWithVat);
                command.Parameters.Add("remarks", webOrder.Remarks);
                command.Parameters.Add("prodnotes", webOrder.ProdNotes);
                command.Parameters.Add("session_id", webOrder.SessionId);
                command.Parameters.Add("last_updated", webOrder.LastUpdated);
                command.Parameters.Add("replicated", webOrder.Replicated);
            
                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                dbObject.ExecuteQuery(command);

                query = SQLClass.MAX_ORDER_ID;
                command = new NpgsqlCommand(query);
                DataTable dt = new DataTable();
                dt = dbObject.GetDataTable(command);
                OrderId = int.Parse(dt.Rows[0][0].ToString());
            }
            catch(Exception ex) 
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return OrderId;
        }

        public static int InsertShoppingCartItem(WebOrderItems woItem)
        {
            int isInsert = 0;
            try
            {
                string query = SQLClass.INSERT_INTO_WEB_ORDER_ITEMS;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_id", woItem.OrderId);
                command.Parameters.Add("item_id", woItem.ItemSeq);
                command.Parameters.Add("item_type", woItem.ItemType);
                command.Parameters.Add("item_code", woItem.ItemCode);
                command.Parameters.Add("derived_from", woItem.DerivedFrom);
                command.Parameters.Add("description", woItem.Description);
                command.Parameters.Add("suppl_code", woItem.SuppliedCode);
                command.Parameters.Add("quantity", woItem.Quantity);
                command.Parameters.Add("units", woItem.Units);
                command.Parameters.Add("price_cost", woItem.PriceCost);
                command.Parameters.Add("price_sell", woItem.PriceSell);
                command.Parameters.Add("price_total", woItem.PriceTotal);
                command.Parameters.Add("discount", woItem.Discount);
                command.Parameters.Add("remarks", woItem.Remarks);
                command.Parameters.Add("prodnotes", woItem.ProdNotes);
                command.Parameters.Add("to_edm", woItem.ToEdm);
                command.Parameters.Add("to_products", woItem.ToArticles);

                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isInsert = dbObject.ExecuteQuery(command);
               
            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isInsert;
        }

        public static int UpdateShoppingCartItem(WebOrderItems woItem)
        {
            int isUpdate = 0;
            try
            {
                string query = SQLClass.UPDATE_ITEM;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_id", woItem.OrderId);                
                command.Parameters.Add("item_type", woItem.ItemType);
                command.Parameters.Add("item_code", woItem.ItemCode);                
                command.Parameters.Add("description", woItem.Description);                
                command.Parameters.Add("quantity", woItem.Quantity);
                command.Parameters.Add("units", woItem.Units);
                command.Parameters.Add("price_cost", woItem.PriceCost);
                command.Parameters.Add("price_sell", woItem.PriceSell);
                command.Parameters.Add("price_total", woItem.PriceTotal);
                command.Parameters.Add("discount", woItem.Discount);
                command.Parameters.Add("remarks", woItem.Remarks);
                command.Parameters.Add("prodnotes", woItem.ProdNotes);
                command.Parameters.Add("to_edm", woItem.ToEdm);
                command.Parameters.Add("to_products", woItem.ToArticles);
                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isUpdate = dbObject.ExecuteQuery(command);

            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isUpdate;
        }

        public static int UpdateShoppingCartSpecialItem(WebOrderItems woItem)
        {
            int isUpdate = 0;
            try
            {
                string query = SQLClass.UPDATE_ITEM;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_id", woItem.OrderId);
                command.Parameters.Add("item_code", woItem.ItemCode);
                command.Parameters.Add("to_edm", woItem.ToEdm);
                command.Parameters.Add("to_articles", woItem.ToArticles);

                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isUpdate = dbObject.ExecuteQuery(command);

            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isUpdate;
        }

        public static DataTable GetDiscountForCustomer(string custCode)
        {            
            string query = SQLClass.GET_DISCOUNT_FOR_CUSTOMER;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("cust_code", custCode.ToUpper());
            command.CommandText = query;
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            dt = dbObject.GetDataTable(command);
            return dt;            
        }

        public static DataTable CheckOrderExist(string sessionId)
        {
            string query = SQLClass.IS_ORDER_EXIST;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("session_id", sessionId);
            command.CommandText = query;
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            dt = dbObject.GetDataTable(command);
            return dt;
        }

        public static DataTable CheckREGOrADMOrderExist(string CustCode)
        {
            string query = SQLClass.IS_REG_ADM_ORDER_EXIST;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("cust_code", CustCode.ToUpper());
            command.CommandText = query;
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            dt = dbObject.GetDataTable(command);
            return dt;
        }

        public static DataTable CheckOrderItemExist(int OrderId, string ItemCode)
        {
            string query = SQLClass.IS_ORDER_ITEM_EXIST;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("order_id", OrderId);
            command.Parameters.Add("item_code", ItemCode.ToUpper());
            command.CommandText = query;
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            dt = dbObject.GetDataTable(command);
            return dt;
        }

        public static DataTable GetCountryList()
        {
            //DropDownList drp = new DropDownList();
            string query = SQLClass.COUNTRY_LIST;
            NpgsqlCommand command = new NpgsqlCommand();            
            command.CommandText = query;
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            dt = dbObject.GetDataTable(command);           
            return dt;            
        }

        public static int UpdateOrderforCollect(WebOrders wsOrder)
        {
            int isUpdate = 0;
            try
            {
                string query = SQLClass.UPDATE_ORDER_FOR_COLLECT;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_id", wsOrder.OrderID);
                command.Parameters.Add("shipping_method", wsOrder.ShippingMethod);
                command.Parameters.Add("delivery_date", wsOrder.DeliveryDate);
                
                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isUpdate = dbObject.ExecuteQuery(command);

            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isUpdate;
        }

        public static int UpdateOrderforDelivery(WebOrders wsOrder)
        {
            int isUpdate = 0;
            try
            {                
                string query = SQLClass.UPDATE_ORDER_FOR_Delivery;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_id", wsOrder.OrderID);
                command.Parameters.Add("shipping_method", wsOrder.ShippingMethod);
                command.Parameters.Add("delivery_date", wsOrder.DeliveryDate);
                command.Parameters.Add("cust_name", wsOrder.CustName);
                command.Parameters.Add("cust_contact", wsOrder.CustContact);
                command.Parameters.Add("delivery_address", wsOrder.DeliveryAddress);
                command.Parameters.Add("delivery_zipcode", wsOrder.DeliveryZipcode);
                command.Parameters.Add("delivery_city", wsOrder.DeliveryCity);
                command.Parameters.Add("delivery_country", wsOrder.DeliveryCountry);

                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isUpdate = dbObject.ExecuteQuery(command);

            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isUpdate;
        }

        public static int UpdateWSOrder(WebOrders wsOrder)
        {
            int isUpdate = 0;
            try
            {
                string query = SQLClass.WSHOP_ORDER_UPDATE;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_id", wsOrder.OrderID);
                command.Parameters.Add("session_id", wsOrder.SessionId);                
                command.Parameters.Add("order_code", wsOrder.OrderCode);
                command.Parameters.Add("cust_code", wsOrder.CustCode);              

                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isUpdate = dbObject.ExecuteQuery(command);

            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isUpdate;
        }

        public static int TotalItemofOrder(int OrderId)
        {
            int result = 0;
            try
            {
                string query = SQLClass.TOTAL_ITEM_OF_AN_ORDER;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_id", OrderId);
                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                DataTable dt = dbObject.GetDataTable(command);
                result = int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return result;
        }


        public static int InsertEDM(EDM edm)
        {           
            int isInsert = 0;
            try
            {
                string query = SQLClass.INSERT_INTO_EDM;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("edm_code", edm.EDMCode);
                command.Parameters.Add("description", edm.Description);
                command.Parameters.Add("revision", edm.Revision);
                command.Parameters.Add("cust_code", edm.CustCode);
                command.Parameters.Add("format", edm.Format);
                command.Parameters.Add("scale", edm.Scale);
                command.Parameters.Add("first_published", edm.FirstPubl);
                command.Parameters.Add("last_published", edm.LastPubl);
                command.Parameters.Add("remarks", edm.Remarks);
                command.Parameters.Add("author", edm.Author);
                command.Parameters.Add("last_edited", edm.LastEdited);
                command.Parameters.Add("last_edited_by", edm.LastEditedBy);
                command.Parameters.Add("deleted", edm.Deleted);              

                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isInsert = dbObject.ExecuteQuery(command);
            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isInsert;
        }

        public static int InsertBaseMaterial(BaseMaterial bm)
        {   
            int isInsert = 0;
            try
            {
                string query = SQLClass.INSERT_INTO_BASEMATERIAL;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("matcode", bm.Matcode);
                command.Parameters.Add("description", bm.Description);
                command.Parameters.Add("orderdescription", bm.OrderDescription);
                command.Parameters.Add("suppcode", bm.SuppCode);
                command.Parameters.Add("prjcode", bm.PrjCode);
                command.Parameters.Add("edmcode", bm.EdmCode);
                command.Parameters.Add("unit", bm.Unit);
                command.Parameters.Add("packunit", bm.PackUnit);
                command.Parameters.Add("purchaseprice", bm.PurchasePrice);
                command.Parameters.Add("discount1", bm.Discount1);
                command.Parameters.Add("discount2", bm.Discount2);
                command.Parameters.Add("freight", bm.Freight);
                command.Parameters.Add("addon1", bm.Addon1);
                command.Parameters.Add("addon2", bm.Addon2);
                command.Parameters.Add("netprice", bm.NetPrice);
                command.Parameters.Add("sellprice", bm.SellPrice);
                command.Parameters.Add("sellperfect", bm.SellPerfect);
                command.Parameters.Add("orderdate", bm.OrderDate);
                command.Parameters.Add("deliverydate", bm.DeliveryDate);
                command.Parameters.Add("instock", bm.InStock);
                command.Parameters.Add("minstock", bm.MinStock);
                command.Parameters.Add("maxstock", bm.MaxStock);
                command.Parameters.Add("minorder", bm.MinOrder);
                command.Parameters.Add("stockloc", bm.StockLoc);
                command.Parameters.Add("sparepart", bm.SparePart);
                command.Parameters.Add("webshop", bm.WebShop);
                command.Parameters.Add("remark", bm.Remark);
                command.Parameters.Add("mergecode", bm.MergeCode);
                command.Parameters.Add("derivedfrom", bm.DerivedFrom);
                command.Parameters.Add("remarkeng", bm.RemarkEng);
                command.Parameters.Add("keywords", bm.KeyWords);
                command.Parameters.Add("filter1", bm.Filter1);
                command.Parameters.Add("filter2", bm.Filter2);
                command.Parameters.Add("filter3", bm.Filter3);
                command.Parameters.Add("filter4", bm.Filter4);
                command.Parameters.Add("filter5", bm.Filter5);
                command.Parameters.Add("new_suppcode", bm.NewSuppCode);
                command.Parameters.Add("cust_code", bm.CustCode);
                command.Parameters.Add("matcode_suppl", bm.MatcodeSuppl);
                command.Parameters.Add("deleted", bm.Deleted);

                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                isInsert = dbObject.ExecuteQuery(command);
            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return isInsert;
        }

        public static DataTable GetAllItemForOrder(int OrderId)
        {
            string query = SQLClass.GET_ALL_ITEM_FOR_ORDER;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Parameters.Add("order_id", OrderId);
            command.CommandText = query;
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            dt = dbObject.GetDataTable(command);
            return dt;
        }

        public static DataTable GetAllCustomers()
        {
            //DropDownList drp = new DropDownList();
            string query = SQLClass.GET_TOTAL_CUSTOMERS;
            NpgsqlCommand command = new NpgsqlCommand();
            command.CommandText = query;
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
            dt = dbObject.GetDataTable(command);
            return dt;
        }

        public static int UpdateAdditionalInfo(WebOrders wsOrder)
        {
              int isUpdate = 0;
              try
              {
                  string query = SQLClass.UPDATE_ADDITIONAL_INFO;
                  NpgsqlCommand command = new NpgsqlCommand(query);
                  command.CommandType = CommandType.Text;
                  command.Parameters.Add("order_id", wsOrder.OrderID);
                  command.Parameters.Add("cust_code", wsOrder.CustCode);
                  command.Parameters.Add("order_descr", wsOrder.OrderDescription);
                  command.Parameters.Add("total_order_with_vat", wsOrder.TotalOrderWithVat);
                  command.Parameters.Add("total_order", wsOrder.TotalOrder);                  
                  command.Parameters.Add("prodnotes", wsOrder.ProdNotes);
                  IDatabaseFactory dbFactory = new DatabaseFactory();
                  IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                  isUpdate = dbObject.ExecuteQuery(command);                 
              }
              catch (Exception ex)
              {
                  Hey.Common.Utils.LogWriter.Log(ex.Message);
              }
              return isUpdate;
        }

        public static string GetKLSPCode()
        {
            string code = string.Empty;
            int max = 0;
            try
            {
                string query = SQLClass.GET_MAX_KLSP;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                DataTable dt = new DataTable();
                dt = dbObject.GetDataTable(command);
                string klsp = dt.Rows[0][0].ToString().Trim();
                if (klsp.Length > 4)
                {
                    string num = klsp.Substring(4);
                    max = int.Parse(num);
                    max = max + 1;
                }
                code = "KLSP" + max.ToString();
            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return code;
        }

        public static string GetEDMCode(string oCode)
        {
            string code = string.Empty;
            int max = 0;
            try
            {
                string query = SQLClass.IS_ORDER_CODE_EXISTS;
                NpgsqlCommand command = new NpgsqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("order_code", oCode);
                IDatabaseFactory dbFactory = new DatabaseFactory();
                IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);
                DataTable dt = new DataTable();
                dt = dbObject.GetDataTable(command);
                string EDM = dt.Rows[0][0].ToString().Trim();
                if (oCode.Length == EDM.Length)
                {
                    code = EDM + "1";
                }
                else
                {
                    string num = EDM.Substring(oCode.Length);
                    max = int.Parse(num) + 1;
                    code = EDM.Substring(0, oCode.Length - 1) + max.ToString();
                }
                
            }
            catch (Exception ex)
            {
                Hey.Common.Utils.LogWriter.Log(ex.Message);
            }
            return code;
        }

        
    }
}
