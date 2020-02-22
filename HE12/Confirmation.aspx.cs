using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using Hey.Common.Objects;
using Hey.Business;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Web;

public partial class Confirmation : System.Web.UI.Page
{
    public enum EditingGrid
    {
        STANDARD,
        SPECIAL
    }
    string userType = "userType";
    string sessionid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        HCustomers hUser = new HCustomers();
        if (Session["userrole"] != null)
        {
            hUser = (HCustomers)Session["userrole"];           
            sessionid = hUser.RelCode;           
        }
        else
        {
            HttpCookie cookie = Request.Cookies["usersession"];
            if (cookie != null)
            {
                sessionid = cookie.Value.ToString();                
            }
        }        
        if (!IsPostBack)
        {
            try
            {
                CheckUserType();
                FillShoppingCartStandardData();
                PopulateOrderDetail();
                if ((UserType)ViewState[userType] == UserType.ADMIN)
                {
                    FillShoppingCartSpecialData();
                }
                else
                {
                    HideUnauthorizedView();
                }
                GetFileList();
            }
            catch (Exception ex)
            {
            }
        }
        ConfirmationButtonManagement();       
    }

    void ConfirmationButtonManagement()
    {
        if ((UserType)ViewState[userType] == UserType.ANONYMOUS)
        {
            divAdm.Visible = false;
            divReg.Visible = false;
            divAno.Visible = true;
        }
        if ((UserType)ViewState[userType] == UserType.REGISTERED)
        {
            divAdm.Visible = false;
            divReg.Visible = true;
            divAno.Visible = false;
        }
        if ((UserType)ViewState[userType] == UserType.ADMIN)
        {
            divAdm.Visible = true;
            divReg.Visible = false;
            divAno.Visible = false;
        }
    }

    void PopulateOrderDetail()
    {
        if (Session["orderid"] != null)
        {
            FacadeManager facade = new FacadeManager();
            DataTable dt = facade.ShopOrderCostDetail(Session["orderid"].ToString());
            try
            {
                if (dt.Rows.Count > 0)
                {
                    if (ViewState[userType] != null && (UserType)ViewState[userType] == UserType.ANONYMOUS)
                    {
                        costdetailContainer.Visible = false;
                    }
                    else
                    {
                        lblTotal.Text = Convert.ToDouble(dt.Rows[0]["total_order"]).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                        lblProductDiscout.Text = Convert.ToDouble(dt.Rows[0]["discount"]).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                        lblOtherDiscount.Text = Convert.ToDouble(dt.Rows[0]["other_discount"]).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                        lblShippingCost.Text = Convert.ToDouble(dt.Rows[0]["shipping_cost"]).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                        lblTotalVat.Text = Convert.ToDouble(dt.Rows[0]["total_vat"]).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                        lblTotalIncludingVat.Text = Convert.ToDouble(dt.Rows[0]["total_order_with_vat"]).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    }
                    txtCustRefCode.Text = dt.Rows[0]["cust_ref"].ToString();
                    txtOrderRemark.Text = dt.Rows[0]["remarks"].ToString();
                }
            }
            catch
            {
            }
        }
    }

    void CheckUserType()
    {
        ViewState[userType] = UserType.ADMIN;
        if (Session["userrole"] == null)  // if user is anonymous i.e. webshoprole = 0
        {
            ViewState[userType] = UserType.ANONYMOUS;
        }
        else
        {
            HCustomers hUser = (HCustomers)Session["userrole"];           
            if (hUser.WebShopRole == 2) // if user is not admin i.e. webshoprole = 2
            {
                ViewState[userType] = UserType.REGISTERED;
            }
            else //if user is admin i.e. webshoprole = 1
            {
                //if (Request.QueryString["order_id"] != null)
                //{                
                //if (Session["orderid"] == null)
                //{
                //    //Session["orderid"] = Request.QueryString["order_id"];
                //    //Session.Add("orderid", Request.QueryString["order_id"]);
                //}
                //}
            }
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            FileUpload fUpload = gvFiles.FooterRow.FindControl("fileUploadsCart") as FileUpload;
            FacadeManager facade = new FacadeManager();
            if (Session["orderid"] != null)
            {
                facade.UploadFile(fUpload, Session["orderid"].ToString());
                GetFileList();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void FillShoppingCartStandardData()
    {
        try
        {
            FacadeManager facade = new FacadeManager();
            List<IShoppingCart> itemList = null;
            if ((UserType)ViewState[userType] == UserType.ANONYMOUS)
            {
                itemList = facade.GetShoppingCart(sessionid, ItemTypeEnum.STANDARD, UserType.ANONYMOUS);
            }
            if ((UserType)ViewState[userType] == UserType.REGISTERED)
            {
                itemList = facade.GetShoppingCart(sessionid, ItemTypeEnum.STANDARD, UserType.REGISTERED);
            }
            if ((UserType)ViewState[userType] == UserType.ADMIN)
            {
                itemList = facade.GetShoppingCart(sessionid, ItemTypeEnum.STANDARD, UserType.ADMIN);
            }

            gvSpCart.DataSource = itemList;
            gvSpCart.DataBind();
            ManageAuthorizedFields();
            if (itemList.Count > 0 && ViewState["OrderID"] == null)
            {
                ViewState["OrderID"] = itemList[0].OrderID;
                if (Session["orderid"] == null)
                {
                    Session.Add("orderid", itemList[0].OrderID);
                }
            }
            PopulateOrderDetail();
        }
        catch (Exception ex)
        {
        }
    }
    private void FillShoppingCartSpecialData()
    {
        try
        {
            FacadeManager facade = new FacadeManager();
            List<IShoppingCart> sCart = new List<IShoppingCart>();
            sCart = facade.GetShoppingCart(sessionid, ItemTypeEnum.SPECIAL, UserType.ADMIN);
            if (sCart.Count > 0)
            {
                gvShoppingSpecial.DataSource = sCart;
                gvShoppingSpecial.DataBind();
            }
            else
            {
                sCart.Add(new ShoppingCartManager());
                gvShoppingSpecial.DataSource = sCart;
                gvShoppingSpecial.DataBind();

                int colCount = gvShoppingSpecial.Columns.Count;
                gvShoppingSpecial.Rows[0].Cells.Clear();
                gvShoppingSpecial.Rows[0].Cells.Add(new TableCell());
                gvShoppingSpecial.Rows[0].Cells[0].ColumnSpan = colCount;
                gvShoppingSpecial.Rows[0].Cells[0].Text = gvShoppingSpecial.EmptyDataText;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void ManageAuthorizedFields()
    {
        if (ViewState[userType] != null)
        {
            UserType type = (UserType)ViewState[userType];
            if (type == UserType.ANONYMOUS || type == UserType.REGISTERED)
            {
                //gvSpCart.Columns[0].ItemStyle.Width = Unit.Pixel(10);
                //foreach (GridViewRow row in gvSpCart.Rows)
                //{
                //    ((ImageButton)row.FindControl("btnSpecialItem")).Visible = false;
                //    ((ImageButton)row.FindControl("btnStandardItem")).Visible = false;

                //}
                gvSpCart.Columns[3].Visible = false;
                gvSpCart.Columns[4].Visible = false;
                gvSpCart.Columns[5].Visible = false;
                gvSpCart.Columns[6].Visible = false;
            }
        }
    }
    private void HideUnauthorizedView()
    {
        dvAdminSpecialGrid.Visible = false;
    }

    void GetFileList()
    {
        try
        {
            FacadeManager facade = new FacadeManager();
            List<UploadedFile> uFileList = new List<UploadedFile>();
            if (ViewState["OrderID"] != null)
            {
                uFileList = facade.GetUploadedFiles(ViewState["OrderID"].ToString());
            }
            if (uFileList.Count > 0)
            {
                gvFiles.DataSource = uFileList;
                gvFiles.DataBind();
            }
            else
            {
                uFileList.Add(new UploadedFile());
                gvFiles.DataSource = uFileList;
                gvFiles.DataBind();

                int colCount = gvFiles.Columns.Count;
                gvFiles.Rows[0].Cells.Clear();
                gvFiles.Rows[0].Cells.Add(new TableCell());
                gvFiles.Rows[0].Cells[0].ColumnSpan = colCount;
                gvFiles.Rows[0].Cells[0].Text = gvFiles.EmptyDataText;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void gvSpCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSpCart.PageIndex = e.NewPageIndex;
            FillShoppingCartStandardData();
            udpShoppingCart.Update();
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvSpecial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvShoppingSpecial.PageIndex = e.NewPageIndex;
            FillShoppingCartSpecialData();
            udpShoppingCart.Update();
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFiles.PageIndex = e.NewPageIndex;
        GetFileList();
    }

    protected void gvStdCart_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IShoppingCart spManager = e.Row.DataItem as ShoppingCartManager;
                if (spManager.ItemID != null)
                {
                    double total = Convert.ToDouble(e.Row.Cells[6].Text);
                    double price = Convert.ToDouble(e.Row.Cells[4].Text);
                    double quantity = Convert.ToDouble(e.Row.Cells[2].Text);

                    e.Row.Cells[6].Text = total.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    e.Row.Cells[4].Text = price.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    e.Row.Cells[2].Text = Convert.ToDouble(quantity).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvSpRow_dataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IShoppingCart spManager = e.Row.DataItem as ShoppingCartManager;
                if (spManager.ItemID != null)
                {
                    double total = Convert.ToDouble(e.Row.Cells[6].Text);
                    double price = Convert.ToDouble(e.Row.Cells[4].Text);
                    double quantity = Convert.ToDouble(e.Row.Cells[2].Text);

                    e.Row.Cells[6].Text = total.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    e.Row.Cells[4].Text = price.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    e.Row.Cells[2].Text = Convert.ToDouble(quantity).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void ManageSession()
    {
        Session.Remove("orderid");
        Session.Remove("itemno");
    }

    #region For Anonymous User

    protected void btnPriceRequest_Click(object sender, EventArgs e)
    {
        try
        {
            ShoppingCartManager cart = new ShoppingCartManager();
            FacadeManager facade = new FacadeManager();
            if (Session["orderid"] != null)
            {
                cart.OrderID = Session["orderid"].ToString();
                facade.ConfirmOrder(cart, "A");
                if (Session["anonymousmail"] != null)
                {
                    string orderid = Session["orderid"] != null ? Session["orderid"].ToString() : "Order ID Could not retrieve";
                    string heyEmail = ConfigurationManager.AppSettings["hey-admin-mail"];
                    string msg = "Product price request order submitted.<br /> Order ID : " + orderid + "<br/> Total Amount: " + lblTotal.Text +
                        "<br/><br/> With kind regards,<br/>" + Session["anonymousmail"].ToString();
                    EmailInfo eInfo = new EmailInfo();
                    eInfo.Email = Session["anonymousmail"].ToString().Trim();
                    bool success = new FacadeManager().OrderConfirmationMailToAdmin(eInfo, heyEmail, msg);
                }
                ManageSession();
                Response.Redirect("Home.aspx");
            }
        }
        catch(Exception ex) 
        { }
    }

    #endregion
    #region For Registered User

    protected void btnSendOffer_Click(object sender, EventArgs e)
    {
        try
        {
            ShoppingCartManager cart = new ShoppingCartManager();
            FacadeManager facade = new FacadeManager();
            if (Session["orderid"] != null)
            {
                cart.OrderID = Session["orderid"].ToString();
                facade.ConfirmOrder(cart, "O");
                HCustomers hUser = (HCustomers)Session["userrole"];
                if (hUser != null)
                {
                    string orderid = Session["orderid"] != null ? Session["orderid"].ToString() : "Order ID Could not retrieve";
                    string heyEmail = ConfigurationManager.AppSettings["hey-admin-mail"];
                    string msg = "Product purchase offer submitted.<br /> Order ID : " + orderid + "<br/> Total Amount: " + lblTotal.Text + 
                        "<br/><br/> With kind regards,<br/>" + hUser.Email;
                    EmailInfo eInfo = new EmailInfo();
                    eInfo.Email = hUser.Email;
                    bool success = new FacadeManager().OrderConfirmationMailToAdmin(eInfo, heyEmail, msg);

                }
                ManageSession();
                Response.Redirect("Home.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPlaceOffer_Click(object sender, EventArgs e)
    {
        try
        {
            ShoppingCartManager cart = new ShoppingCartManager();
            FacadeManager facade = new FacadeManager();
            if (Session["orderid"] != null)
            {
                cart.OrderID = Session["orderid"].ToString();
                facade.ConfirmOrder(cart, "B");
                HCustomers hUser = (HCustomers)Session["userrole"];
                if (hUser != null)
                {
                    string orderid = Session["orderid"] != null ? Session["orderid"].ToString() : "Order ID Could not retrieve";
                    string heyEmail = ConfigurationManager.AppSettings["hey-admin-mail"];
                    string msg = "Product purchase order submitted.<br /> Order ID : " + orderid + "<br/> Total Amount: " + lblTotal.Text +
                        "<br/><br/> With kind regards,<br/>" + hUser.Email; ;
                    EmailInfo eInfo = new EmailInfo();
                    eInfo.Email = hUser.Email;
                    bool success = new FacadeManager().OrderConfirmationMailToAdmin(eInfo, heyEmail, msg);

                }
                ManageSession();
                Response.Redirect("Home.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region For Admin

    protected void btnNaarProd_Click(object sender, EventArgs e)
    {
        try
        {
            ShoppingCartManager cart = new ShoppingCartManager();
            FacadeManager facade = new FacadeManager();
            if (Session["orderid"] != null)
            {
                cart.OrderID = Session["orderid"].ToString();
                HCustomers hUser = (HCustomers)Session["userrole"];
                if (ViewState[userType] != null && hUser != null)
                {
                    facade.ConfirmOrder(cart, "P");
                    // insert toedm and basematerial
                    int IsEDM = 0;
                    int IsBaseMaterial = 0;
                    DataTable ItemTable = new DataTable();
                    ItemTable = new FacadeManager().GetAllItemForOrder(int.Parse(cart.OrderID));
                    if (ItemTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in ItemTable.Rows)
                        {
                            IsEDM = Convert.ToInt32(row["to_edm"].ToString());
                            if (IsEDM == 1)
                            {
                                // insert EDM
                                int res = InsertEDMData(hUser);
                            }
                            IsBaseMaterial = Convert.ToInt32(row["to_products"].ToString());
                            if (IsBaseMaterial == 1)
                            {
                                // insert into BaseMaterial
                                int res = InsertBaseMaterialData(hUser);
                            }
                        }
                    }
                    if (chkConfirmation.Checked) // if confirmation checkbox selected then send Order Finalize mail to customer.
                    {
                        string orderid = Session["orderid"] != null ? Session["orderid"].ToString() : "Order ID Could not retrieve";
                        string heyEmail = ConfigurationManager.AppSettings["hey-admin-mail"];
                        EmailInfo eInfo = new EmailInfo();
                        eInfo.Email = hUser.Email;
                        string msg = "Product purchase order finalized.<br /> Order ID : " + orderid +  
                                        "<br/> Total Amount: " + lblTotal.Text +
                                        "<br/> Product Discount: " + lblProductDiscout.Text +
                                        "<br/> Other Discount: " + lblOtherDiscount.Text +
                                        "<br/> Shipping Cost: " + lblShippingCost.Text +
                                        "<br/> Total Amount (Including vat): " + lblTotalIncludingVat.Text +
                                        "<br/><br/> With kind regards,<br/><a href='new.hey-ermelo.nl'>HEY</a>" ;
                        bool success = new FacadeManager().OrderFinalizeMailToCustomer(eInfo, heyEmail, msg);
                    }
                    ManageSession();
                    Response.Redirect("Home.aspx");
                }
            }
        }
        catch(Exception ex) 
        {
        }
    }
    protected void btnAdmOffer_Click(object sender, EventArgs e)
    {
        try
        {
            ShoppingCartManager cart = new ShoppingCartManager();
            FacadeManager facade = new FacadeManager();
            if (Session["orderid"] != null)
            {
                cart.OrderID = Session["orderid"].ToString();
                facade.ConfirmOrder(cart, "O");
                HCustomers hUser = (HCustomers)Session["userrole"];
                if (hUser != null)
                {
                    if (chkConfirmation.Checked) // if confirmation checkbox selected then send Order Confirmation mail to customer.
                    {
                        string orderid = Session["orderid"] != null ? Session["orderid"].ToString() : "Order ID Could not retrieve";
                        string heyEmail = ConfigurationManager.AppSettings["hey-admin-mail"];
                        string msg = "Product purchase offer submitted.<br /> Order ID : " + orderid +
                                        "<br/> Total Amount: " + lblTotal.Text +
                                        "<br/> Product Discount: " + lblProductDiscout.Text +
                                        "<br/> Other Discount: " + lblOtherDiscount.Text +
                                        "<br/> Shipping Cost: " + lblShippingCost.Text +
                                        "<br/> Total Amount (Including vat): " + lblTotalIncludingVat.Text +
                                        "<br/><br/> With kind regards,<br/><a href='new.hey-ermelo.nl'>HEY</a>";
                        EmailInfo eInfo = new EmailInfo();
                        eInfo.Email = hUser.Email;
                        bool success = new FacadeManager().OrderConfirmationToCustomer(eInfo, heyEmail, msg);
                    }
                    
                }
                ManageSession();
                Response.Redirect("Home.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnAdmOrder_Click(object sender, EventArgs e)
    {
        try
        {
            ShoppingCartManager cart = new ShoppingCartManager();
            FacadeManager facade = new FacadeManager();
            if (Session["OrderID"] != null)
            {
                cart.OrderID = Session["orderid"].ToString();
                HCustomers hUser = (HCustomers)Session["userrole"];
                if (ViewState[userType] != null && hUser != null)
                {
                    facade.ConfirmOrder(cart, "P");
                    // insert toedm and basematerial                    
                    int IsEDM = 0;
                    int IsBaseMaterial = 0;
                    DataTable ItemTable = new DataTable();
                    ItemTable = new FacadeManager().GetAllItemForOrder(int.Parse(cart.OrderID));
                    if (ItemTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in ItemTable.Rows)
                        {
                            IsEDM = Convert.ToInt32(row["to_edm"].ToString());
                            if (IsEDM == 1)
                            {
                                // insert EDM
                                int res = InsertEDMData(hUser);
                            }
                            IsBaseMaterial = Convert.ToInt32(row["to_products"].ToString());
                            if (IsBaseMaterial == 1)
                            {
                                // insert into BaseMaterial
                                int res = InsertBaseMaterialData(hUser);
                            }
                        }
                    }
                    if (chkConfirmation.Checked) // if confirmation checkbox selected then send Order Confirmation mail to customer.
                    {
                        string orderid = Session["orderid"] != null ? Session["orderid"].ToString() : "Order ID Could not retrieve";
                        string heyEmail = ConfigurationManager.AppSettings["hey-admin-mail"];
                        EmailInfo eInfo = new EmailInfo();
                        eInfo.Email = hUser.Email;
                        string msg = "Product purchase order Confirmed.<br /> Order ID : " + orderid +
                                        "<br/> Total Amount: " + lblTotal.Text +
                                        "<br/> Product Discount: " + lblProductDiscout.Text +
                                        "<br/> Other Discount: " + lblOtherDiscount.Text +
                                        "<br/> Shipping Cost: " + lblShippingCost.Text +
                                        "<br/> Total Amount (Including vat): " + lblTotalIncludingVat.Text +
                                        "<br/><br/> With kind regards,<br/><a href='new.hey-ermelo.nl'>HEY</a>";
                           
                        bool success = new FacadeManager().OrderConfirmationToCustomer(eInfo, heyEmail, msg);
                    }
                    ManageSession();
                    Response.Redirect("Home.aspx");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region INSERT EDM & BASEMATERIAL

    public int InsertEDMData(HCustomers hUser)
    {
        string EDMCode = string.Empty;
        string CustCode = string.Empty;
        int Deleted = 0;
        string Author = string.Empty;
        string Description = string.Empty;
        string LastEdBy = string.Empty;
        string Revision = string.Empty;
        string Remarks = string.Empty;
        string Scale = string.Empty;
        string Format = string.Empty;

        CustCode = GetCustCode(hUser);
        EDMCode = new FacadeManager().GetEDMCode(CustCode);
        CustCode = hUser.RelCode;
        int insert = InsertEDM(EDMCode, CustCode, Deleted, Author, Description, LastEdBy, Revision, Remarks, Scale,Format);
        return insert;
    }

    public int InsertEDM(string EDMCode, string CustCode, int Deleted, string Author, string Description, string LastEdBy, string Revision,
        string Remarks, string Scale, string Format)
    {
        EDM edm = new EDM();
        edm.EDMCode = EDMCode;
        edm.CustCode = CustCode;
        edm.Deleted = Deleted;
        edm.Author = Author;
        edm.Description = Description;
        edm.Format = Format;
        edm.FirstPubl = DateTime.Now;
        edm.LastPubl = DateTime.Now;
        edm.LastEditedBy = LastEdBy;
        edm.Revision = Revision;
        edm.Remarks = Remarks;
        edm.Scale = Scale;
        edm.LastEdited = DateTime.Now;
        int item = new FacadeManager().InsertEDM(edm);
        return item;
    }

    public int InsertBaseMaterialData(HCustomers hUser)
    {
        string MatCode = string.Empty;
        string CustCode = string.Empty;
        string Description = string.Empty;
        CustCode = GetCustCode(hUser);
        MatCode = new FacadeManager().GetKLSPCode();
        CustCode = hUser.RelCode;        
        int insert = InsertBaseMaterial(MatCode,MatCode,CustCode,Description);
        return insert;
    }

    public int InsertBaseMaterial(string Matcode, string EDMCode, string CustCode, string Description)
    {
        BaseMaterial bm = new BaseMaterial();
        bm.Matcode = Matcode;
        bm.Description = Description;
        bm.OrderDescription = string.Empty;
        bm.SuppCode = string.Empty;
        bm.PrjCode = string.Empty;
        bm.EdmCode = EDMCode;
        bm.Unit = string.Empty;
        bm.PackUnit = string.Empty;
        bm.PurchasePrice = 0.0;
        bm.Discount1 = 0.0;
        bm.Discount2 = 0.0;
        bm.Freight = 0.0;
        bm.Addon1 = 0.0;
        bm.Addon2 = 0.0;
        bm.NetPrice = 0.0;
        bm.SellPrice = 0.0;
        bm.SellPerfect = 0.0;
        bm.OrderDate = DateTime.Now.ToShortDateString();
        bm.DeliveryDate = DateTime.Now.ToShortDateString();
        bm.InStock = 0.0;
        bm.MinStock = 0.0;
        bm.MaxStock = 0.0;
        bm.MinOrder = 0.0;
        bm.StockLoc = string.Empty;
        bm.SparePart = 0;
        bm.WebShop = 1;
        bm.Remark = string.Empty;
        bm.MergeCode = 1;
        bm.DerivedFrom = string.Empty;
        bm.RemarkEng = string.Empty;
        bm.KeyWords = string.Empty;
        bm.Filter1 = string.Empty;
        bm.Filter2 = string.Empty;
        bm.Filter3 = string.Empty;
        bm.Filter4 = string.Empty;
        bm.Filter5 = string.Empty;
        bm.NewSuppCode = string.Empty;
        bm.CustCode = CustCode;
        bm.MatcodeSuppl = string.Empty;
        bm.Deleted = 0;

        int item = new FacadeManager().InsertBaseMaterial(bm);
        return item;
    }

    private static string GetCustCode(HCustomers hUser)
    {
        string CustCode = string.Empty;
        if (hUser.RelCode.Length > 4)
        {
            CustCode = hUser.RelCode.Substring(0, 4) + DateTime.Now.Date.ToString("yyMMdd");
        }
        else
        {
            CustCode = hUser.RelCode + DateTime.Now.Date.ToString("yyMMdd");
        }
        return CustCode;
    }
    #endregion
}
