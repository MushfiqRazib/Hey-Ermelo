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

public partial class WebShopAccount : System.Web.UI.Page
{
    string OrderID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["orderid"] != null)
        {
            OrderID = Session["orderid"].ToString();
        }        
    }
    protected void btnAccount_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtEmail.Text))
            {
                if (chkAccount.Checked)
                {
                    EmailInfo eInfo = new EmailInfo();
                    eInfo.CompanyName = txtCompanyName.Text.Trim();
                    eInfo.ContactPerson = txtContactPerson.Text.Trim();
                    eInfo.PhoneNo = txtPhoneNo.Text.Trim();
                    eInfo.Email = txtEmail.Text.Trim();
                    lblStatus.Visible = false;
                    bool success = new FacadeManager().SendMailToUser(eInfo);
                    if (success)
                    {
                        RefreshAccountControl();
                        lblStatus.Visible = true;
                        lblStatus.Text = "Mail has sent.";
                        Session.Add("anonymousmail", txtEmail.Text.Trim());
                        Response.Redirect("Confirmation.aspx");
                    }
                    else
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Error: Mail is failed to deliver.";
                    }
                }
                else
                {
                    Session.Add("anonymousmail", txtEmail.Text.Trim());
                    Response.Redirect("Confirmation.aspx");
                }
            }
            else
            {
                lblStatus.Visible = true;
                lblStatus.Text = "No mail address is given.";
            }
        }
        catch (Exception ex)
        { }       
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
            CustomerObject = new FacadeManager().GetCustomerInfoByLogin(txtUserName.Text.Trim(), txtPassword.Text.Trim());
            if (CustomerObject != null)
            {
                lblMsg.Visible = false;
                RefreshLoginControl();
                Session.Add("userrole", CustomerObject);
                // update ws_order table if order_id exists...
                if (!String.IsNullOrEmpty(OrderID))
                {
                    WebOrders wsOrder = new WebOrders();
                    wsOrder.OrderID = int.Parse(OrderID);
                    wsOrder.CustCode = CustomerObject.RelCode;
                    wsOrder.SessionId = CustomerObject.Email;
                    if (CustomerObject.RelCode.Length > 4)
                    {
                        wsOrder.OrderCode = CustomerObject.RelCode.Substring(0, 4) + DateTime.Now.Date.ToString("yyMMdd");
                    }
                    else
                    {
                        wsOrder.OrderCode = CustomerObject.RelCode + DateTime.Now.Date.ToString("yyMMdd");
                    }
                    int update = new FacadeManager().UpdateWSOrder(wsOrder);
                }
                Response.Redirect("Shipping.aspx");               
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Gebruikersnaamen/of wachtwoord is onjuist.";
            }
        }
        catch (Exception ex)
        {}
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
