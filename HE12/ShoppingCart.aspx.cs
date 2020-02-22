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
using System.IO;

public partial class ShoppingCart : System.Web.UI.Page
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
    }

    void PopulateOrderDetail()
    {        
        if (ViewState["OrderID"] != null)
        {
            FacadeManager facade = new FacadeManager();
            DataTable dt = facade.ShopOrderCostDetail(ViewState["OrderID"].ToString());
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
                        lblBedrag.Text = string.Format("Eindbedrag € {0}", lblTotalIncludingVat.Text);
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
        try
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
                    if (Request.QueryString["order_id"] != null)
                    {
                        ViewState["OrderID"] = Request.QueryString["order_id"];
                        if (Session["orderid"] == null)
                        {
                            Session.Add("orderid", Request.QueryString["order_id"]);
                        }
                        else
                        {
                            Session["orderid"] = Request.QueryString["order_id"];
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnShowUpload_Click(object sender, EventArgs e)
    {
        modalPopupAttachment.Show();
    }

    protected void UploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            FileUpload fUpload = gvFiles.FooterRow.FindControl("UploadFile_Click") as FileUpload;
            FacadeManager facade = new FacadeManager();
            if (ViewState["OrderID"] != null)
            {
                facade.UploadFile(fileUploadsCart, ViewState["OrderID"].ToString());
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
        try
        {
            if (ViewState[userType] != null)
            {
                UserType type = (UserType)ViewState[userType];
                if (type == UserType.ANONYMOUS || type == UserType.REGISTERED)
                {
                    gvSpCart.Columns[0].ItemStyle.Width = Unit.Pixel(10);
                    foreach (GridViewRow row in gvSpCart.Rows)
                    {
                        ((ImageButton)row.FindControl("btnSpecialItem")).Visible = false;
                        ((ImageButton)row.FindControl("btnStandardItem")).Visible = false;

                    }
                    gvSpCart.Columns[4].Visible = false;
                    gvSpCart.Columns[5].Visible = false;
                    gvSpCart.Columns[6].Visible = false;
                    gvSpCart.Columns[7].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
        }

    }

    private void HideUnauthorizedView()
    {
        dvAdminSpecialGrid.Visible = false;
        btnAddSpecialItem.Visible = false;
        btnEditInfo.Visible = false;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            TextBox txtQuantity, txtRemark;
            string item_id;
            int index;
            foreach (GridViewRow row in gvSpCart.Rows)
            {
                index = row.RowIndex;
                txtRemark = row.FindControl("txtRemark") as TextBox;
                txtQuantity = row.FindControl("txtAantal") as TextBox;
                item_id = gvSpCart.DataKeys[index].Values[1].ToString();
                FacadeManager facade = new FacadeManager();
                IShoppingCart cart = new ShoppingCartManager { ItemID = item_id, Quantity = txtQuantity.Text.Replace(",", "."), Remark = txtRemark.Text.Trim() };
                facade.UpdateShoppingCart(cart);

            }
            foreach (GridViewRow row in gvShoppingSpecial.Rows)
            {
                index = row.RowIndex;
                txtRemark = row.FindControl("txtRemark") as TextBox;
                txtQuantity = row.FindControl("txtAantalSp") as TextBox;
                item_id = gvShoppingSpecial.DataKeys[index].Values[1].ToString();
                FacadeManager facade = new FacadeManager();
                IShoppingCart cart = new ShoppingCartManager { ItemID = item_id, Quantity = txtQuantity.Text.Replace(",", "."), Remark = txtRemark.Text.Trim() };
                facade.UpdateShoppingCart(cart);

            }

            FillShoppingCartStandardData();
            FillShoppingCartSpecialData();
            PopulateOrderDetail();
            udpShoppingCart.Update();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnRemoveall_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["OrderID"] != null)
            {
                FacadeManager facade = new FacadeManager();
                IShoppingCart cart = new ShoppingCartManager { OrderID = ViewState["OrderID"].ToString() };
                facade.RemoveShoppingCart(cart);
                FillShoppingCartStandardData();
                FillShoppingCartSpecialData();
                PopulateOrderDetail();
                udpShoppingCart.Update();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvSpCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            string item_id = gvSpCart.DataKeys[index].Values[1].ToString();
            FacadeManager facade = new FacadeManager();
            IShoppingCart cart = new ShoppingCartManager { ItemID = item_id };
            facade.RemoveShoppingCartItem(cart);
            FillShoppingCartStandardData();
            PopulateOrderDetail();
            udpShoppingCart.Update();
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvSpecial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            string item_id = gvShoppingSpecial.DataKeys[index].Values[1].ToString();
            FacadeManager facade = new FacadeManager();
            IShoppingCart cart = new ShoppingCartManager { ItemID = item_id };
            facade.RemoveShoppingCartItem(cart);
            FillShoppingCartSpecialData();
            PopulateOrderDetail();
            udpShoppingCart.Update();
        }
        catch (Exception ex)
        {
        }
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

    protected void gvFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            string fileURI = gvFiles.DataKeys[index].Value.ToString();
            if (File.Exists(fileURI))
            {
                File.Delete(fileURI);
                GetFileList();
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
                    double total = Convert.ToDouble(e.Row.Cells[7].Text);
                    double price = Convert.ToDouble(e.Row.Cells[5].Text);
                    TextBox quantityText = e.Row.FindControl("txtAantal") as TextBox;
                    double quantity = Convert.ToDouble(quantityText.Text);
                    e.Row.Cells[7].Text = total.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    e.Row.Cells[5].Text = price.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    quantityText.Text = Convert.ToDouble(quantity).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
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
                    double total = Convert.ToDouble(e.Row.Cells[7].Text);
                    double price = Convert.ToDouble(e.Row.Cells[5].Text);
                    TextBox quantityText = e.Row.FindControl("txtAantalSp") as TextBox;
                    double quantity = Convert.ToDouble(quantityText.Text);
                    e.Row.Cells[7].Text = total.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    e.Row.Cells[5].Text = price.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    quantityText.Text = Convert.ToDouble(quantity).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                }
            }
        }
        catch (Exception ex)
        { }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string cust_ref = txtCustRefCode.Text;
            string remarks = txtOrderRemark.Text;
            FacadeManager facade = new FacadeManager();
            if (ViewState["OrderID"] != null)
            {
                facade.UpdateShopOrderData(ViewState["OrderID"].ToString(), cust_ref, remarks);
            }

            if (Session["userrole"] == null)  // if user is anonymous i.e. webshoprole = 0
            {
                Response.Redirect("WebShopAccount.aspx");
            }
            else
            {
                Response.Redirect("Shipping.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }


    #region Modal

    protected void btnSpecialItem_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton btnImage = sender as ImageButton;
            int index = btnImage.CommandArgument != "" ? Convert.ToInt32(btnImage.CommandArgument) : -1;
            string ItemID = gvSpCart.DataKeys[index].Values[1].ToString();
            if (ItemID != string.Empty)
            {
                FacadeManager facade = new FacadeManager();
                IShoppingCart cartItem = facade.GetACartItem(ItemID);
                if (cartItem != null)
                {
                    txtVan.Text = cartItem.ItemCode;
                    txtItemDescCustomise.Text = cartItem.Description;
                    txtQuanityCustomise.Text = Convert.ToDouble(cartItem.Quantity).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    txtSellPrice.Text = Convert.ToDouble(cartItem.PriceSell).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    txtCostPrice.Text = Convert.ToDouble(cartItem.PriceCost).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    txtUnit.Text = cartItem.Unit;
                    lblTotalQuantityCustomise.Text = " / " + txtQuanityCustomise.Text;
                    txtRemarkCustomise.Text = cartItem.Remark;
                    txtProdNodesCustomise.Text = cartItem.ProductNotes;
                    chkToEdm.Checked = cartItem.ToEdm == "0" ? false : true;
                    chkToProduct.Checked = cartItem.ToProduct == "0" ? false : true;

                    hdnItemIDCustomise.Value = ItemID;
                    hdnCurrentGridforCustom.Value = EditingGrid.STANDARD.ToString();
                    mpeSpecialItem.Show();
                }

            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnCustomise_Click(object sender, EventArgs e)
    {
        try
        {
            if ((EditingGrid)Enum.Parse(typeof(EditingGrid), hdnCurrentGridforCustom.Value) == EditingGrid.STANDARD)
            {
                DoCustomizeItems();
                FillShoppingCartStandardData();

            }
            else
            {
                EditCustomItem();
            }

            FillShoppingCartSpecialData();
            udpShoppingCart.Update();
            mpeStandardItem.Hide();
        }
        catch (Exception ex)
        {
        }
    }

    private void EditCustomItem()
    {
        try
        {
            FacadeManager facade = new FacadeManager();
            ShoppingCartManager cartItem = new ShoppingCartManager();
            cartItem.ItemID = hdnItemIDCustomise.Value;

            cartItem.Description = txtItemDescCustomise.Text.Trim();
            cartItem.Quantity = Convert.ToDouble(txtQuanityCustomise.Text.Replace(",", ".")).ToString();
            cartItem.PriceSell = Convert.ToDouble(txtSellPrice.Text.Replace(",", ".")).ToString();
            cartItem.PriceCost = Convert.ToDouble(txtCostPrice.Text.Replace(",", ".")).ToString();
            cartItem.Unit = txtUnit.Text;
            cartItem.Remark = txtRemarkCustomise.Text.Trim();
            cartItem.ProductNotes = txtProdNodesCustomise.Text.Trim();
            cartItem.ToEdm = chkToEdm.Checked ? "1" : "0";
            cartItem.ToProduct = chkToProduct.Checked ? "1" : "0";           
            facade.UpdateCustomItem(cartItem);
        }
        catch (Exception ex)
        {
        }
    }

    private void DoCustomizeItems()
    {
        try
        {
            double totalQuantity = Convert.ToDouble(lblTotalQuantityCustomise.Text.Replace("/", "").Trim().Replace(",", "."));
            double customisedQuatity = Convert.ToDouble(txtQuanityCustomise.Text.Trim().Replace(",", "."));
            if (customisedQuatity > 0 && ViewState["OrderID"] != null)
            {
                ShoppingCartManager cartItem = new ShoppingCartManager();
                cartItem.ItemID = hdnItemIDCustomise.Value;
                cartItem.Description = txtItemDescCustomise.Text.Trim();
                cartItem.Quantity = customisedQuatity.ToString();
                cartItem.PriceSell = Convert.ToDouble(txtSellPrice.Text.Replace(",", ".")).ToString();
                cartItem.PriceCost = Convert.ToDouble(txtCostPrice.Text.Replace(",", ".")).ToString();
                cartItem.Unit = txtUnit.Text;
                cartItem.Remark = txtRemarkCustomise.Text.Trim();
                cartItem.ProductNotes = txtProdNodesCustomise.Text.Trim();
                cartItem.ToEdm = chkToEdm.Checked ? "1" : "0";
                cartItem.ToProduct = chkToProduct.Checked ? "1" : "0";


                FacadeManager facade = new FacadeManager();
                string item_id = facade.CheckCustomItemExists(cartItem);

                if (totalQuantity == customisedQuatity)
                {
                    facade.ConvertItemToCustomized(cartItem);
                }
                else if (customisedQuatity < totalQuantity)
                {
                    facade.CreateNewCustomizedItem(cartItem);

                    facade.ReduceStandardItemQuanity(cartItem);
                }

                FillShoppingCartSpecialData();

                FillShoppingCartStandardData();
                udpShoppingCart.Update();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnStandardGridItemEdit_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton btnImage = sender as ImageButton;
            int index = btnImage.CommandArgument != "" ? Convert.ToInt32(btnImage.CommandArgument) : -1;
            string ItemID = gvSpCart.DataKeys[index].Values[1].ToString();
            if (ItemID != string.Empty)
            {
                FacadeManager facade = new FacadeManager();
                IShoppingCart cartItem = facade.GetACartItem(ItemID);
                if (cartItem != null)
                {
                    txtStdMatCode.Text = cartItem.ItemCode;
                    txtStdDescription.Text = cartItem.Description;
                    txtStdQuantiry.Text = Convert.ToDouble(cartItem.Quantity).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    txtStdRemark.Text = cartItem.Remark;
                    txtStdProdNote.Text = cartItem.ProductNotes;
                    hdnItemID.Value = ItemID;
                    hdnCurrentGrid.Value = EditingGrid.STANDARD.ToString();
                    mpeStandardItem.Show();
                }

            }


        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSpecialGridItemEdit_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton btnImage = sender as ImageButton;
            int index = btnImage.CommandArgument != "" ? Convert.ToInt32(btnImage.CommandArgument) : -1;
            string ItemID = gvShoppingSpecial.DataKeys[index].Values[1].ToString();
            if (ItemID != string.Empty)
            {
                FacadeManager facade = new FacadeManager();
                IShoppingCart cartItem = facade.GetACartItem(ItemID);
                if (cartItem != null)
                {
                    txtVan.Text = cartItem.ItemType.ToLower() == "c" ? cartItem.ItemCode : string.Empty;
                    txtItemDescCustomise.Text = cartItem.Description;
                    txtQuanityCustomise.Text = Convert.ToDouble(cartItem.Quantity).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    txtSellPrice.Text = Convert.ToDouble(cartItem.PriceSell).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    txtCostPrice.Text = Convert.ToDouble(cartItem.PriceCost).ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
                    txtUnit.Text = cartItem.Unit;
                    lblTotalQuantityCustomise.Text = " / " + txtQuanityCustomise.Text;
                    txtRemarkCustomise.Text = cartItem.Remark;
                    txtProdNodesCustomise.Text = cartItem.ProductNotes;
                    chkToEdm.Checked = cartItem.ToEdm == "0" ? false : true;
                    chkToProduct.Checked = cartItem.ToProduct == "0" ? false : true;

                    hdnItemIDCustomise.Value = ItemID;
                    hdnCurrentGridforCustom.Value = EditingGrid.SPECIAL.ToString();
                    mpeSpecialItem.Show();
                }

            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void SaveItem_Click(object sender, EventArgs e)
    {
        try
        {
            FacadeManager facade = new FacadeManager();
            IShoppingCart cartItem = new ShoppingCartManager();
            cartItem.ItemID = hdnItemID.Value;
            cartItem.ItemCode = txtStdMatCode.Text;
            cartItem.Description = txtStdDescription.Text;
            cartItem.Quantity = txtStdQuantiry.Text.Replace(",", ".");
            cartItem.Remark = txtStdRemark.Text;
            cartItem.ProductNotes = txtStdProdNote.Text;
            facade.UpdateShoppingCartItem(cartItem);
            EditingGrid currentGrid = (EditingGrid)Enum.Parse(typeof(EditingGrid), hdnCurrentGrid.Value);
            if (EditingGrid.SPECIAL == currentGrid)
            {
                FillShoppingCartSpecialData();
            }
            else
            {
                FillShoppingCartStandardData();
            }


            PopulateOrderDetail();
            udpShoppingCart.Update();
            mpeStandardItem.Hide();
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnAddSpecialItem_Click(object sender, EventArgs e)
    {
        try
        {            
            string ItemID = string.Empty;
            if (gvSpCart.Rows.Count > 0)
            {
                ItemID = gvSpCart.DataKeys[0].Values[1].ToString();
            }
            if (gvShoppingSpecial.Rows.Count > 0)
            {
                ItemID = gvShoppingSpecial.DataKeys[0].Values[1].ToString();
            }
            if (ItemID != string.Empty)
            {
                FacadeManager facade = new FacadeManager();
                IShoppingCart cartItem = facade.GetACartItem(ItemID);
                if (cartItem != null)
                {                      
                    txtAddSpDescription.Text = cartItem.Description;                   
                    mpeAddSpecialItem.Show();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnEditInfo_Click(object sender, EventArgs e)
    {
        try
        {
            BindCustomerCombo();
            HCustomers hUser = new HCustomers();
            string PrjCode = string.Empty;
            hUser = (HCustomers)Session["userrole"];
            if (hUser.RelCode.Length > 4)
            {
                PrjCode = "KLSP" + hUser.RelCode.Substring(0, 4) + DateTime.Now.Date.ToString("yyMMdd");
            }
            else
            {
                PrjCode = "KLSP" + hUser.RelCode + DateTime.Now.Date.ToString("yyMMdd");
            }
            DataTable orderDT = new DataTable();
            orderDT = new FacadeManager().CheckREGOrADMOrderExist(hUser.RelCode);
            int OrderID = int.Parse(orderDT.Rows[0]["order_id"].ToString());
            double TotalPrice = double.Parse(orderDT.Rows[0]["total_order_with_vat"].ToString());
            txtAddInfoTotalPrc.Text = TotalPrice.ToString("#0,0.00", CultureInfo.CreateSpecificCulture("nl-NL"));
            txtAddInfoPrjCode.Text = drpKlant.SelectedValue;

            //hdnInfoShipCost.Value = orderDT.Rows[0]["shipping_cost"].ToString();
            //hdnInfoTotalItems.Value = orderDT.Rows[0]["total_items"].ToString();
            //hdnInfoDiscountItems.Value = orderDT.Rows[0]["discount_items"].ToString();

            OrderIdSession(OrderID.ToString()); // orderid put in session
            mpeEditInfo.Show();
        }
        catch (Exception ex)
        {
        }

    }
    protected void btnAddSpOK_Click(object sender, EventArgs e)
    {
        try
        {
            HCustomers hUser = new HCustomers();
            hUser = (HCustomers)Session["userrole"];
            string SubItem = string.Empty; string EDMCode = string.Empty; string Artikelnr = string.Empty;
            string Description = txtAddSpDescription.Text;
            string Units = txtAddEenhd.Text;
            double Quantity = double.Parse(txtAddSpAntal.Text.Replace(",", "."));

            if (Quantity != 0.0)
            {

                double PriceCost = double.Parse(txtAddSpCost.Text.Replace(",", "."));
                double PriceSell = double.Parse(txtAddSpSell.Text.Replace(",", "."));
                double Discount = 0.0;
                string Remarks = txtAddOpmerk.Text;
                string ProdNotes = txtAddProductie.Text;
                string ItemType = "O";
                int ToEDM = 0;
                int ToProducts = 0;
                string CustCode = GetCustCode(hUser);

                if (chkAddSpEDM.Checked)
                {
                    ToEDM = 1;
                    EDMCode = new FacadeManager().GetEDMCode(CustCode);
                }
                if (chkAddSpArtikel.Checked)
                {
                    ToProducts = 1;
                    Artikelnr = new FacadeManager().GetKLSPCode();
                }
                if (!String.IsNullOrEmpty(Artikelnr))
                {
                    SubItem = Artikelnr;
                }
                if (String.IsNullOrEmpty(SubItem))
                {
                    if (!String.IsNullOrEmpty(EDMCode))
                    {
                        SubItem = EDMCode;
                    }
                }

                string OrderCode = string.Empty;
                string OrderStatus = "-";
                DataTable orderDT = new DataTable();
                orderDT = new FacadeManager().CheckREGOrADMOrderExist(hUser.RelCode);  // step 2. is order exists
                if (orderDT.Rows.Count > 0)  // step:3 - update order & insert item
                {
                    int OrderID = int.Parse(orderDT.Rows[0]["order_id"].ToString());
                    OrderIdSession(OrderID.ToString()); // orderid put in session
                    int item = InsertWSItem(SubItem, ItemType, Description, Units, Quantity, PriceCost, PriceSell, Discount, OrderID, Remarks, ProdNotes, ToEDM, ToProducts);
                    ItemSession();
                   
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

                FillShoppingCartSpecialData();
                udpShoppingCart.Update();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnAddSpCancel_Click(object sender, EventArgs e)
    {
        mpeAddSpecialItem.Hide();
    }
    protected void btnAddInfoOK_Click(object sender, EventArgs e)
    {
        try
        {
            double total_order_with_vat = double.Parse(txtAddInfoTotalPrc.Text.Replace(",", "."));  //total_order_with_vat
            double vat = double.Parse(Hey.Common.Utils.Functions.GetValueFromWebConfig("BTW"));
            double total_order = total_order_with_vat * (1 - (vat / 100));  //total_order          
            //double discount_other = (double.Parse(hdnInfoShipCost.Value) + 
            //                         double.Parse(hdnInfoTotalItems.Value)) - 
            //                         double.Parse(hdnInfoDiscountItems.Value) - total_order; //discount_other    
            HCustomers hUser = new HCustomers();
            string PrjCode = txtAddInfoPrjCode.Text;
            string Description = txtAdditionalInfoDescription.Text;
            string ProdNotes = txtAddInfoProductie.Text;
            // update items...
            int result = UpdateWSOrder(PrjCode, Description, ProdNotes, total_order_with_vat, total_order);
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnAddInfoCancel_Click(object sender, EventArgs e)
    {
        mpeEditInfo.Hide();
    }
    #endregion
    
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
    public void BindCustomerCombo()
    {
        DataTable dt = new DataTable();
        dt = new FacadeManager().GetAllCustomers();
        drpKlant.DataSource = dt;
        drpKlant.DataTextField = dt.Columns[1].ToString();
        drpKlant.DataValueField = dt.Columns[0].ToString();
        drpKlant.DataBind();
        drpKlant.Items.Insert(0, new ListItem("-------------"));
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
        if (Session["itemno"] == null)
        {
            Session.Add("itemno", 1);
        }
        else
        {
            int item = int.Parse(Session["itemno"].ToString());
            item = item + 1;
            Session["itemno"] = item.ToString();
        }
    }
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
    private int UpdateWSOrder(string OrderCode, string OrderDesc, string ProdNotes, double TotalOrderWithVat, double TotalOrder)
    {
        WebOrders wsOrder = new WebOrders();
        wsOrder.OrderCode = OrderCode;
        wsOrder.OrderDescription = OrderDesc;
        wsOrder.ProdNotes = ProdNotes;
        wsOrder.TotalOrderWithVat = TotalOrderWithVat;
        wsOrder.TotalOrder = TotalOrder;
        int OrderId = new FacadeManager().UpdateAdditionalInfo(wsOrder);
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

    protected void drpKlant_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAddInfoPrjCode.Text = drpKlant.SelectedValue;
        mpeEditInfo.Show();
    }
   
}


