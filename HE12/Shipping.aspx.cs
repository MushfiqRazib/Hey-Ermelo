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
using Hey.Business;

public partial class Shipping : System.Web.UI.Page
{
    string OrderID = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userrole"] != null)
        {
            HCustomers hUser = new HCustomers();
            hUser = (HCustomers)Session["userrole"];
            if (Session["orderid"] != null)
            {
                OrderID = Session["orderid"].ToString();
            }
            BindCountryCombo();                      
        }            
    }

    public void BindCountryCombo()
    {
        DataTable dt = new DataTable();
        dt = new FacadeManager().GetCountryList();
        drpCountry.DataSource = dt;
        drpCountry.DataTextField = dt.Columns[0].ToString();
        drpCountry.DataValueField = dt.Columns[0].ToString();
        drpCountry.DataBind();
        drpCountry.Items.Insert(0, new ListItem("Default Country"));  
    }
   
    public void RefreshAccountControl()
    {
        //txtEmail.Text = "";
        //txtContactPerson.Text = "";
        //txtCompanyName.Text = "";
        //txtPhoneNo.Text = "";
    }
    public void RefreshLoginControl()
    {
        //txtPassword.Text = "";
        //txtUserName.Text = "";
    }
    
    protected void btnCollect_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["userrole"] != null)
            {
                HCustomers hUser = new HCustomers();
                hUser = (HCustomers)Session["userrole"];
                // get the order_id
                if (Session["orderid"] != null)
                {
                    // update ws_orders.
                    OrderID = Session["orderid"].ToString();
                    string ShippingMethod = "Afgehaald";
                    DateTime DeliveryDate = new DateTime();
                    if (!rboCollectDate.Checked)
                    {
                        DeliveryDate = DateTime.Now.AddDays(7.0); 
                    }
                    else
                    {
                        DeliveryDate = Convert.ToDateTime(txtCollectDate.Text);                        
                    }
                    int updateId = UpdateOrderForCollect(OrderID, DeliveryDate, ShippingMethod);

                }
            }
            Response.Redirect("Confirmation.aspx");
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
    protected void btnDelivery_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["userrole"] != null)
            {
                HCustomers hUser = new HCustomers();
                hUser = (HCustomers)Session["userrole"];
                // get the order id
                if (Session["orderid"] != null)
                {
                    // update ws_orders.
                    OrderID = Session["orderid"].ToString();
                    string ShippingMethod = "Franko'";
                    DateTime DeliveryDate = new DateTime();
                    string CustName = string.Empty;
                    string CustContact = string.Empty;
                    string DelAddress = string.Empty;
                    string DelZip = string.Empty;
                    string DelCity = string.Empty;
                    string DelCountry = string.Empty;
                    if (!rboDeliverDate.Checked)
                    {
                        DeliveryDate = DateTime.Now.AddDays(7.0);
                        CustName = hUser.RelName;
                        if (!String.IsNullOrEmpty(hUser.DeliveryAddress) && !String.IsNullOrEmpty(hUser.DeliveryZipcode))
                        {
                            DelAddress = hUser.DeliveryAddress;
                            DelZip = hUser.DeliveryZipcode;
                        }
                        else
                        {
                            DelAddress = hUser.VisitAddress;
                            DelZip = hUser.VisitZipcode;
                        }
                        CustContact = hUser.Contact;
                        DelCity = hUser.City;
                        DelCountry = hUser.Country;
                    }
                    else
                    {
                        DeliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                        CustName = txtName.Text;
                        CustContact = txtTav.Text;
                        DelAddress = txtStreet.Text;
                        DelZip = txtZipcode.Text;
                        DelCity = txtCity.Text;
                        DelCountry = drpCountry.SelectedValue.ToString();
                    }
                    int updateId = UpdateOrderForDelivery(OrderID, DeliveryDate, ShippingMethod, CustName, CustContact, DelAddress, DelZip, DelCity, DelCountry);
                }
            }
            Response.Redirect("Confirmation.aspx");
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    private int UpdateOrderForCollect(string OrderId, DateTime DeliveryDate, string ShippingMethod)
    {
        WebOrders wsOrder = new WebOrders();
        wsOrder.OrderID = int.Parse(OrderId);
        wsOrder.DeliveryDate = DeliveryDate;
        wsOrder.ShippingMethod = ShippingMethod;
        int upItem = new FacadeManager().UpdateOrderforCollect(wsOrder);        
        return upItem;
    }

    private int UpdateOrderForDelivery(string OrderId, DateTime DeliveryDate, string ShippingMethod, string CustName, string CustContact,
           string DeliveryAddress, string DeliveryZipCode, string DeliveryCity, string DeliveryCountry)
    {
        WebOrders wsOrder = new WebOrders();
        wsOrder.OrderID = int.Parse(OrderId);
        wsOrder.DeliveryDate = DeliveryDate;
        wsOrder.ShippingMethod = ShippingMethod;
        wsOrder.CustName = CustName;
        wsOrder.CustContact = CustContact;
        wsOrder.DeliveryAddress = DeliveryAddress;
        wsOrder.DeliveryZipcode = DeliveryZipCode;
        wsOrder.DeliveryCity = DeliveryCity;
        wsOrder.DeliveryCountry = DeliveryCountry;
        int upItem = new FacadeManager().UpdateOrderforDelivery(wsOrder);
        return upItem;
    }

    protected void rboCollect_OnCheckedChanged(object sender, EventArgs e)
    {
        txtCollectDate.Enabled = true;
        CalExtenderCollect.Enabled = true;
    }
    protected void rboDeliverDate_CheckedChanged(object sender, EventArgs e)
    {
        txtDeliveryDate.Enabled = true;
        txtName.Enabled = true;
        txtTav.Enabled = true;
        txtZipcode.Enabled = true;
        txtCity.Enabled = true;
        txtStreet.Enabled = true;
        drpCountry.Enabled = true;
        CalendarExtenderDelivery.Enabled = true;
    }
}
