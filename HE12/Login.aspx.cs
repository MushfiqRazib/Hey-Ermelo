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

public partial class Login : System.Web.UI.Page
{
    string sessionid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["usersession"];
        if (cookie != null)
        {
            sessionid = cookie.Value.ToString();
            DataTable dt = new DataTable();
            dt = new FacadeManager().CheckOrderExist(sessionid); // step 1: if any order exists for this anonymous user 
            if (dt.Rows.Count > 0)  // step 2: if order exists, get the order id and total item number for this anonymous user
            {
                int OrderID = int.Parse(dt.Rows[0]["order_id"].ToString());
                if (Session["orderid"] == null)
                {
                    Session.Add("orderid", OrderID.ToString());
                }
                int TotalItem = new FacadeManager().TotalItemofOrder(OrderID);
                if (TotalItem > 0)
                {                    
                    if (Session["itemno"] == null)
                    {
                        Session.Add("itemno", TotalItem.ToString());
                    }
                } 
            }
        }        
    }
    protected void btnAccount_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtEmail.Text))
            {
                EmailInfo eInfo = new EmailInfo();
                eInfo.CompanyName = txtCompanyName.Text.Trim();
                eInfo.ContactPerson = txtContactPerson.Text.Trim();
                eInfo.PhoneNo = txtPhoneNo.Text.Trim();
                eInfo.Email = txtEmail.Text.Trim();
                lblStatus.Visible = false;

                bool success = new FacadeManager().SendMailToUser(eInfo); // sending mail to Admin and getting confirmation of mail
                if (success)
                {
                    RefreshAccountControl();
                    lblStatus.Visible = true;
                    lblStatus.Text = "Mail has sent.";
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Error: Mail is failed to deliver.";
                }
            }
            else
            {
                lblStatus.Visible = true;
                lblStatus.Text = "No mail address is given.";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void RefreshAccountControl()
    {
        txtEmail.Text = "";
        txtContactPerson.Text = "";
        txtCompanyName.Text = "";
        txtPhoneNo.Text = "";
    }
    public void RefreshLoginControl()
    {
        txtPassword.Text = "";
        txtUserName.Text = "";
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            HCustomers CustomerObject = new HCustomers();
            // get the customer information.
            CustomerObject = new FacadeManager().GetCustomerInfoByLogin(txtUserName.Text.Trim(), txtPassword.Text.Trim());
            if (CustomerObject != null)
            {
                lblMsg.Visible = false;
                RefreshLoginControl();
                Session.Add("userrole", CustomerObject);  // user session for logged in customer               
                UserControl ucl;
                DataTable orderDT = new DataTable();
                orderDT = new FacadeManager().CheckREGOrADMOrderExist(CustomerObject.RelCode); // check if any Order exists for this Register or Admin user.
                if (orderDT.Rows.Count > 0)
                {
                    int OrderID = int.Parse(orderDT.Rows[0]["order_id"].ToString());
                    if (Session["orderid"] == null)
                    {
                        Session.Add("orderid", OrderID.ToString());
                    }
                    else
                    {
                        Session["orderid"] = OrderID.ToString();
                    }
                    int TotalItem = new FacadeManager().TotalItemofOrder(OrderID);  // Get the toal item number of the existing order.
                    if (TotalItem > 0)  // if item exists, a session is created/updated for store the current item number.
                    {                        
                        if (Session["itemno"] == null)
                        {
                            Session.Add("itemno", TotalItem.ToString());
                        }
                        else
                        {
                            Session["itemno"] = TotalItem.ToString();
                        }
                    }                    
                }           
    
                // After successful log in, the page will redirect to the FilterItem.aspx page and the first subitem
                // of the first item of the menu will be selected and shows the image and filter pane of that selected Subitem.
                // The below code get the first subitem value i.e. code and description from the master page navigator control.
                // And the page will redirect with necessary querystring to the FilterItem.aspx page.
                AjaxControlToolkit.Accordion accordMenu = null;
                ucl = (UserControl)Master.FindControl("NavigatorControl1");               
                if (ucl != null)
                {
                    accordMenu = (AjaxControlToolkit.Accordion)ucl.FindControl("accordMenu");
                }
                LinkButton lnk = accordMenu.Panes[0].ContentContainer.Controls[0].Controls[0] as LinkButton;
                if (lnk.CommandName == "GetItemCode")
                {
                    string[] values = lnk.CommandArgument.ToString().Split(new char[] { ',' });
                    Response.Redirect("FilterItem.aspx?code=" + values[0] + "&description=" + values[1]);
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Gebruikersnaamen/of wachtwoord is onjuist.";
            }
        }
        catch (Exception ex)
        {
            string errorMsg = ex.ToString();
        }
    }
    protected void lnkForgetPassword_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtUserName.Text))
            {
                EmailInfo eInfo = new EmailInfo();
                eInfo.Email = txtUserName.Text.Trim();
                lblMsg.Visible = false;
                bool success = new FacadeManager().SendPasswordRecoveryMail(eInfo);
                if (success)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Uw wachtwoord is verstuurd naar het opgegeven e-mailadres.";
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Het opgegeven e-mailadres is onbekend in ons systeem.";
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Het opgegeven e-mailadres is onbekend in ons systeem.";
            }
        }
        catch (Exception ex)
        { }
    }
}
