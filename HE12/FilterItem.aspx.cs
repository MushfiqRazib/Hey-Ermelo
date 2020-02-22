using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Hey.Common.Objects;
using System.Collections.Generic;
using Hey.Business;
using System.Text;
using System.Globalization;

public partial class FilterItem : System.Web.UI.Page
{
    bool isFilterButton = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadFilterPane();
        if (!Page.IsPostBack)
        {
            if (Session["searchstring"] != null)
            {
                string searchString = Request.QueryString["s"];
                txtFilterSearch.Text = searchString;
                BindSearchGrid();
            }
        }
    }
    public void LoadFilterPane()
    {
        string ItemCode = Request.QueryString["code"];
        string ItemDescription = Request.QueryString["description"];
        ItemName.InnerText = ItemDescription;
        List<MaterialGroupFilter> FilterCombos = new FacadeManager().GetNumberOfFilterCombo(ItemCode);
        StringBuilder sb = new StringBuilder();

        foreach (MaterialGroupFilter filterCombo in FilterCombos)
        {
            LiteralControl lCntrl = new LiteralControl(String.Format("<div  class=\"filterdiv\"><div class=\"filterlabel\">{0}</div>", filterCombo.FilterLabel));
            ComboPanel.Controls.Add(lCntrl);
            DropDownList drp = new DropDownList();
            drp = new FacadeManager().CreteFilterCombo(filterCombo);
            lCntrl = new LiteralControl("<div class=\"filtercombo\">");
            ComboPanel.Controls.Add(lCntrl);
            ComboPanel.Controls.Add(drp);
            lCntrl = new LiteralControl("</div></div>");
            ComboPanel.Controls.Add(lCntrl);
        }
        bool isDrpdownExist = false;
        foreach (Control c in ComboPanel.Controls)
        {
            if (c.GetType() == typeof(DropDownList))
            {
                isDrpdownExist = true;
                break;
            }
        }
        if (!isDrpdownExist)
        {
            btnClearBtn.Visible = false;
            btnFilter.Visible = false;
        }        
        string WebShopCMSImageUrl = Hey.Common.Utils.Functions.GetValueFromWebConfig("HEYWS") + "CustomHandler/ImageHandler.ashx?url=images/";
        GetGroupImage(WebShopCMSImageUrl, "");
    }
    private void GetGroupImage(string path, string imageFile)
    {
        if (String.IsNullOrEmpty(imageFile))
        {
            groupImg.ImageUrl = path + "Filter_image.png";
        }
        else
        {
        }
    }
    protected void btnClearBtn_Click(object sender, EventArgs e)
    {
        foreach (Control c in ComboPanel.Controls)
        {
            if (c.GetType() == typeof(DropDownList))
            {
                ((DropDownList)c).SelectedIndex = 0;
            }
        }
        ResultPane.Attributes.Add("class", "resultpane resultpanehide");  
        ResultPane.Visible = false;
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        isFilterButton = true;
        BindGrid();
    }
    public void BindGrid()
    {
        string ItemCode = Request.QueryString["code"];
        string fieldNames = string.Empty;
        string fieldValues = string.Empty;
        foreach (Control c in ComboPanel.Controls)
        {
            if (c.GetType() == typeof(DropDownList))
            {
                if (((DropDownList)c).SelectedValue != "")
                {
                    fieldNames += ((DropDownList)c).ID + ",";
                    fieldValues += ((DropDownList)c).SelectedValue + ",";
                }
            }
        }

        if (!String.IsNullOrEmpty(fieldNames))
        {
            fieldNames = fieldNames.Remove(fieldNames.LastIndexOf(','), 1);
        }
        if (!String.IsNullOrEmpty(fieldValues))
        {
            fieldValues = fieldValues.Remove(fieldValues.LastIndexOf(','), 1);
        }

        List<BaseMaterial> bmObject = new FacadeManager().GetFilterData(ItemCode, fieldNames, fieldValues);
        ResultPane.Attributes.Add("class", "resultpane");     
        ResultPane.Visible = true;
        gvResult.DataSource = bmObject;
        gvResult.DataBind();
        OrganizeGridColumn();
    }
    public void BindSearchGrid()
    {
        string ItemCode = Request.QueryString["code"];
        string ItemDescription = Request.QueryString["description"];

        List<BaseMaterial> bmObject = new FacadeManager().GetSubgroupFilterSearchData(ItemCode);
        ResultPane.Attributes.Add("class", "resultpane");   
        ResultPane.Visible = true;
        gvResult.DataSource = bmObject;
        gvResult.DataBind();
        OrganizeGridColumn();

        //List<MaterialGroup> listObj = (List<MaterialGroup>)Session["SearchResultSet"];
        //foreach (MaterialGroup group in listObj)
        //{
        //    if (ItemCode.Equals(group.Code))
        //    {
        //        ResultPane.Visible = true;
        //        ClearGrid();
        //        gvResult.DataSource = group.MaterialGroups;
        //        gvResult.DataBind();
        //    }
        //} 
    }
    public void ClearGrid()
    {
        DataTable dt = new DataTable();
        gvResult.DataSource = dt;
        gvResult.DataBind();
    }
    protected void gvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvResult.PageIndex = e.NewPageIndex;
        if (isFilterButton)
        {
            BindGrid();
        }
        else
        {
            BindSearchGrid();
        }
    }
    protected void imgFilterSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (txtFilterSearch.Text.Length == 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert", "<script>alert('To search you need to enter your search key!');</script>");
        }
        else
        {
            GetSearchResult();
        }
    }
    private void GetSearchResult()
    {
        string ItemCode = Request.QueryString["code"];
        string input = txtFilterSearch.Text;
        List<BaseMaterial> bmObject = new FacadeManager().FilterPanelSearchData(ItemCode, input);
        ResultPane.Attributes.Add("class", "resultpane");  
        ResultPane.Visible = true;
        gvResult.DataSource = bmObject;
        gvResult.DataBind();
        OrganizeGridColumn();
    }
    private void OrganizeGridColumn()
    {
        HCustomers hUser = new HCustomers();
        string userRole = string.Empty;
        ImageButton specialBtn = null;
        if (Session["userrole"] == null)  // if user is anonymous i.e. webshoprole = 0
        {
            gvResult.Columns[3].Visible = false;
            gvResult.Columns[4].Visible = false;
            foreach (GridViewRow row in gvResult.Rows)
            {
                specialBtn = (ImageButton)row.FindControl("btnSpecialItem");
                specialBtn.Visible = false;
            }
        }
        else
        {
            hUser = (HCustomers)Session["userrole"];
            if (hUser.WebShopRole == 2)  // if user is not admin i.e. webshoprole = 2
            {
                foreach (GridViewRow row in gvResult.Rows)
                {
                    specialBtn = (ImageButton)row.FindControl("btnSpecialItem");
                    specialBtn.Visible = false;
                }
            }
        }
    }
    protected void btnSpecialItem_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton btnSpecial = sender as ImageButton;
            GridViewRow row = (GridViewRow)btnSpecial.NamingContainer;
            txtVan.Text = gvResult.DataKeys[row.RowIndex].Value.ToString();
            TextBox txtQty = row.Cells[4].FindControl("txtQuantity") as TextBox;
            Label lbl = row.Cells[5].FindControl("lblPurchasePrice") as Label;
            if (lbl != null)
            {
                txtKost.Text = lbl.Text.Replace(".", ",");
            }
            lbl = row.Cells[6].FindControl("lblSellPrice") as Label;
            if (lbl != null)
            {
                txtVerk.Text = lbl.Text.Replace(".", ",");
            }
            if (txtQty != null)
            {
                txtAntal.Text = txtQty.Text.Replace(".", ",");
                if (!String.IsNullOrEmpty(txtQty.Text))
                {
                    lblDefaultAantal.Text = txtQty.Text.Replace(".", ",");
                }
                else
                {
                    lblDefaultAantal.Text = "10";
                }
            }

            txtEnhd.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            txtOms.Text = HttpUtility.HtmlDecode(row.Cells[1].Text);
            ResultPane.Attributes.Add("class", "resultpane");  
            ResultPane.Visible = true;
            mpeSpecialItem.Show();
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResultPane.Attributes.Add("class", "resultpane");         
        ResultPane.Visible = true;
        mpeSpecialItem.Hide();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            HCustomers hUser = new HCustomers();
            hUser = (HCustomers)Session["userrole"];
            string ItemCode = Request.QueryString["code"];
            string SubItem = txtVan.Text;
            string Description = txtOms.Text;
            string Units = txtEnhd.Text;
            double Quantity = double.Parse(txtAntal.Text.Replace(",", "."));

            if (Quantity != 0.0)
            {

                double PriceCost = double.Parse(txtKost.Text.Replace(",", "."));
                double PriceSell = double.Parse(txtVerk.Text.Replace(",", "."));
                double Discount = 0.0;
                string Remarks = txtOpmerk.Text;
                string ProdNotes = txtProdNotes.Text;
                string ItemType = "C";
                int ToEDM = 0;
                int ToProducts = 0;

                string OrderCode = GetCustCode(hUser);
                string CustCode = string.Empty;
                string OrderStatus = "-";

                if (chkArtikel.Checked)
                {
                    ToProducts = 1;
                }
                if (chkEDM.Checked)
                {
                    ToEDM = 1;
                }
                Discount = GetDiscountValueForCustomer(hUser, SubItem);
                DataTable orderDT = new DataTable();
                orderDT = new FacadeManager().CheckREGOrADMOrderExist(hUser.RelCode);  // step 2. is order exists
                if (orderDT.Rows.Count > 0)  // step:3 - update order & insert item
                {
                    int OrderID = int.Parse(orderDT.Rows[0]["order_id"].ToString());
                    OrderIdSession(OrderID.ToString()); // orderid put in session
                    DataTable ItemTable = new DataTable();
                    ItemTable = new FacadeManager().CheckOrderItemExist(OrderID, SubItem);
                    if (ItemTable.Rows.Count > 0) // update item
                    {
                        int upItem = UpdateWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderID, Remarks, ProdNotes, ToEDM, ToProducts);
                    }
                    else // insert item
                    {
                        int item = InsertWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderID, Remarks, ProdNotes, ToEDM, ToProducts);
                        ItemSession();
                    }
                }
                else // step:4 - insert order   & insert item
                {
                    OrderCode = new FacadeManager().GetEDMCode(CustCode);
                    CustCode = hUser.RelCode;
                    OrderStatus = "-";
                    string SessionId = hUser.Email;
                    int OrderId = InsertWSOrder(OrderCode, CustCode, OrderStatus, SessionId);
                    OrderIdSession(OrderId.ToString()); // new orderid put in session
                    int item = InsertWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderId, Remarks, ProdNotes, ToEDM, ToProducts);
                    ItemSession();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnShoppingCart_Click(object sender, EventArgs e)
    {
        try
        {            
            HCustomers hUser = new HCustomers();
            string ItemCode = Request.QueryString["code"];
            ImageButton btnShoppingCart = sender as ImageButton;
            GridViewRow row = (GridViewRow)btnShoppingCart.NamingContainer;
            TextBox txtQty = row.Cells[4].FindControl("txtQuantity") as TextBox;
            Label lblCost = row.Cells[5].FindControl("lblPurchasePrice") as Label;
            Label lblSell = row.Cells[6].FindControl("lblSellPrice") as Label;

            string SubItem = gvResult.DataKeys[row.RowIndex].Value.ToString();
            string Description = HttpUtility.HtmlDecode(row.Cells[1].Text);
            string Units = HttpUtility.HtmlDecode(row.Cells[2].Text);
            double Quantity = 0.0;
            double PriceCost = 0.0;
            double PriceSell = 0.0;
            double Discount = 0.0;
            int ToEDM = 0;
            int ToProducts = 0;
            string Remarks = string.Empty;
            string ProdNotes = string.Empty;
            string OrderCode = string.Empty;
            string CustCode = string.Empty;
            string OrderStatus = "-";
            string SessionId = string.Empty;
            string ItemType = "S";

            if (!String.IsNullOrEmpty(txtQty.Text))
            {
                Quantity = double.Parse(txtQty.Text.Replace(",", "."));
                if (Quantity != 0.0)
                {

                    if (!String.IsNullOrEmpty(lblCost.Text))
                    {
                        PriceCost = double.Parse(lblCost.Text.Replace(",", "."));
                    }
                    else
                    {
                        PriceCost = 0.0;
                    }
                    if (!String.IsNullOrEmpty(lblSell.Text))
                    {
                        PriceSell = double.Parse(lblSell.Text.Replace(",", "."));
                    }
                    else
                    {
                        PriceSell = 0.0;
                    }

                    if (Session["userrole"] == null)  // if user is anonymous i.e. webshoprole = 0
                    {
                        // step1: chek if the cust has order with order_status= '-' with this session id
                        // step2: if order_status='-' exists, update ws_orders table
                        // step3: if order_status != '-', then insert order ws_orders table
                        // step4: for both 2 and 3, insert item to ws_order_items table.

                        HttpCookie cookie = new HttpCookie("usersession");
                        cookie.Expires = DateTime.Today.AddDays(31);
                        cookie.Value = Session.SessionID;
                        Response.Cookies.Add(cookie);
                        SessionId = Session.SessionID;

                        DataTable dt = new DataTable();
                        dt = new FacadeManager().CheckOrderExist(SessionId); // step:1 
                        if (dt.Rows.Count > 0)  // step:2 - order exists.. order will update & insert item
                        {
                            int OrderID = int.Parse(dt.Rows[0]["order_id"].ToString());
                            OrderIdSession(OrderID.ToString()); // orderid put in session
                            DataTable ItemTable = new DataTable();
                            ItemTable = new FacadeManager().CheckOrderItemExist(OrderID, SubItem);
                            if (ItemTable.Rows.Count > 0) // update item
                            {
                                int upItem = UpdateWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderID, Remarks, ProdNotes, ToEDM, ToProducts);
                            }
                            else // insert item
                            {
                                int item = InsertWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderID, Remarks, ProdNotes, ToEDM, ToProducts);
                                ItemSession();
                            }
                        }
                        else // step:3 - insert new order & insert item
                        {
                            //SessionId = SessionId;
                            int OrderId = InsertWSOrder(OrderCode, CustCode, OrderStatus, SessionId);
                            OrderIdSession(OrderId.ToString()); // new orderid put in session
                            int item = InsertWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderId, Remarks, ProdNotes, ToEDM, ToProducts);
                            ItemSession();
                        }
                    }
                    else  // if user is registered i.e. webshoprole = 2 or 1
                    {
                        // step1: check if the cust has discount
                        // step2: chek if the cust has order with order_status= '-'
                        // step3: if order_status='-' exists, update ws_orders table
                        // step4: if order_status != '-', then insert order ws_orders table
                        // step5: for both 3 and 4, insert item to ws_order_items table.

                        hUser = (HCustomers)Session["userrole"];
                        Discount = GetDiscountValueForCustomer(hUser, SubItem);
                        DataTable orderDT = new DataTable();
                        orderDT = new FacadeManager().CheckREGOrADMOrderExist(hUser.RelCode);  // step 2.
                        if (orderDT.Rows.Count > 0)  // step:3 - update order & insert item
                        {
                            int OrderID = int.Parse(orderDT.Rows[0]["order_id"].ToString());
                            OrderIdSession(OrderID.ToString()); // orderid put in session
                            DataTable ItemTable = new DataTable();
                            ItemTable = new FacadeManager().CheckOrderItemExist(OrderID, SubItem);
                            if (ItemTable.Rows.Count > 0) // update item
                            {
                                int upItem = UpdateWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderID, Remarks, ProdNotes, ToEDM, ToProducts);
                            }
                            else // insert item
                            {
                                int item = InsertWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderID, Remarks, ProdNotes, ToEDM, ToProducts);
                                ItemSession();
                            }
                        }
                        else // step:4 - insert order   & insert item
                        {
                            CustCode = GetCustCode(hUser);
                            OrderCode = new FacadeManager().GetEDMCode(CustCode);
                            CustCode = hUser.RelCode;
                            OrderStatus = "-";
                            SessionId = hUser.Email;
                            int OrderId = InsertWSOrder(OrderCode, CustCode, OrderStatus, SessionId);
                            OrderIdSession(OrderId.ToString()); // new orderid put in session
                            int item = InsertWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderId, Remarks, ProdNotes, ToEDM, ToProducts);
                            ItemSession();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    
    }
    public void OrderIdSession(string OrderId)
    {
        if (Session["orderid"] == null)
        {
            Session.Add("orderid", OrderId);
        }
    }
    public void ItemSession()
    {
        ClientScriptManager script = Page.ClientScript;
        string scriptText = "<script>function SetCurrentItemNumber() {  var num = '<%= Session[\"itemno\"] %>';" +
                               "alert(num); document.getElementById(\"<%=lblItemNo.ClientID %>\").innerHTML = num; alert(num); return true;}</script>";
        if (Session["itemno"] == null)
        {
            Session.Add("itemno", 1);

            //ScriptManager.RegisterClientScriptBlock(null, this.GetType(), "alert", scriptText,true);
            //script.RegisterStartupScript(this.GetType(), "Alert",scriptText, true);
            //UserControl ucl = (UserControl)Master.FindControl("HeaderControl1");
            ////UserControl ucl = Master.Master.Master.FindControl("ContentPlaceHolderDefault").FindControl("body").FindControl("HeaderControl1") as UserControl;
            //Label lbl = (Label)ucl.FindControl("lblItemNo");
            //lbl.Text = Session["itemno"].ToString();
        }
        else
        {
            int item = int.Parse(Session["itemno"].ToString());
            item = item + 1;
            Session["itemno"] = item.ToString();
            //ScriptManager.RegisterClientScriptBlock(null, this.GetType(), "alert", scriptText, true);
            //script.RegisterStartupScript(this.GetType(), "Alert", "alert('hi')", true);
            //UserControl ucl = (UserControl)Master.FindControl("HeaderControl1");
            ////UserControl ucl = Master.Master.Master.FindControl("ContentPlaceHolderDefault").FindControl("body").FindControl("HeaderControl1") as UserControl;
            //Label lbl = (Label)ucl.FindControl("lblItemNo");
            //lbl.Text = Session["itemno"].ToString();
        }
        //Response.Write("<script>alert('hi');</script>");
        //Page.RegisterStartupScript("click", "<script>alert('hi');</script>");

        Page.ClientScript.RegisterStartupScript(this.GetType(), "openwindow", "<script>alert('hi');</script>", true);
    }

    //public void scriptPrint()
    //{
    //    window.execScript("report_back('Printing complete!')", "JScript");
    //}

    private int InsertWSOrder(string OrderCode, string CustCode, string OrderStatus, string SessionId)
    {
        WebOrders wsOrder = new WebOrders();
        wsOrder.OrderCode = OrderCode;
        wsOrder.CustCode = CustCode;
        wsOrder.OrderStatus = OrderStatus;
        wsOrder.SessionId = SessionId;
        wsOrder.DeliveryDate = DateTime.Now;
        wsOrder.OrderDate = DateTime.Now;
        int OrderId = new FacadeManager().InsertShoppingOrder(wsOrder);
        return OrderId;
    }
    private int InsertWSItem(string SubItem, string ItemType, string Description, string Units, double Quantity, double PriceCost, double PriceSell,
        double Discount, int OrderID, string Remarks, string ProdNotes, int ToEDM, int ToProducts)
    {
        WebOrderItems wsItem = new WebOrderItems();
        wsItem.OrderId = OrderID;
        wsItem.ItemType = ItemType;
        wsItem.ItemCode = SubItem;
        wsItem.Description = Description;
        wsItem.Units = Units;
        wsItem.Quantity = Quantity;
        wsItem.PriceCost = PriceCost;
        wsItem.PriceSell = PriceSell;
        wsItem.PriceTotal = wsItem.Quantity * wsItem.PriceCost;
        wsItem.Discount = Discount;
        wsItem.Remarks = Remarks;
        wsItem.ProdNotes = ProdNotes;
        wsItem.ToEdm = ToEDM;
        wsItem.ToArticles = ToProducts;
        int item = new FacadeManager().InsertShoppingCartItem(wsItem);
        return item;
    }
    private int UpdateWSItem(string SubItem, string ItemType, string Description, string Units, double Quantity, double PriceCost, double PriceSell,
        double Discount, int OrderId, string Remarks, string ProdNotes, int ToEDM, int ToProducts)
    {
        WebOrderItems wsItem = new WebOrderItems();
        wsItem.OrderId = OrderId;
        wsItem.ItemType = ItemType;
        wsItem.ItemCode = SubItem;
        wsItem.Description = Description;
        wsItem.Units = Units;
        wsItem.Quantity = Quantity;
        wsItem.PriceCost = PriceCost;
        wsItem.PriceSell = PriceSell;
        wsItem.PriceTotal = wsItem.Quantity * wsItem.PriceCost;
        wsItem.Discount = Discount;
        wsItem.Remarks = Remarks;
        wsItem.ProdNotes = ProdNotes;
        wsItem.ToEdm = ToEDM;
        wsItem.ToArticles = ToProducts;
        int upItem = new FacadeManager().UpdateShoppingCartItem(wsItem);
        return upItem;
    }
    protected void btnWs_Click(object sender, EventArgs e)
    {
        //Response.Redirect("ShoppingCart.aspx?code=" + values[0] + "&description=" + values[1]);
        Response.Redirect("ShoppingCart.aspx");
    }
    protected void gvResult_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double price = Convert.ToDouble(e.Row.Cells[4].Text);
                e.Row.Cells[4].Text = price.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
            }
        }
        catch (Exception ex)
        {
        }
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
    private double GetDiscountValueForCustomer(HCustomers hUser, string SubItem)
    {
        double Discount = 0.0;
        DataTable dt = new DataTable();
        dt = new FacadeManager().GetDiscountForCustomer(hUser.RelCode); // step 1.
        if (dt.Rows.Count > 0)
        {
            DataRow[] drows = dt.Select(" prod_code='" + SubItem.ToUpper() + "'");
            if (drows.Length == 0)
            {
                drows = dt.Select(" prod_code='" + hUser.RelCode.ToUpper() + "'");
            }
            if (drows.Length == 0)
            {
                drows = dt.Select(" prod_code='" + hUser.RelCode.Substring(0, 1).ToUpper() + "'");
            }
            if (drows.Length == 0)
            {
                drows = dt.Select(" prod_code='*' ");
            }
            if (drows.Length > 0)
            {
                foreach (DataRow dr in drows)
                {
                    Discount = double.Parse(dr["discount"].ToString());
                }
            }
        }
        return Discount;
    }

}


