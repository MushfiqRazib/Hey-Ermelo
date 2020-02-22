using System;
using System.Collections;
using System.Collections.Generic;
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
using Hey.Business;
using Hey.Common.Objects;

public partial class Controls_Header : System.Web.UI.UserControl
{
    protected void Page_Init(Object sender, EventArgs e)
    {       
    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);        
    }

    //protected override void OnPreRender(EventArgs e)
    //{
    //    base.OnPreRender(e);
    //    if (Session["itemno"] != null)
    //    {
    //        lblItemNo.Text = Session["itemno"].ToString();
    //    }
    //   // Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "SetCurrentItemNumber();", true);
    //}
 

    protected void Page_Load(object sender, EventArgs e)
    {
        txtMainSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + 
            txtMainSearch.ClientID + "').click();return false;}} else {return true}; ");
        if (!Page.IsPostBack)
        {
            if (Session["searchstring"] != null)
            {
                txtMainSearch.Text = Session["searchstring"].ToString();
                GetSearchResult();
            }
            else
            {
                Session.Remove("searchstring");               
                SearchResultPane.Visible = false;
                EmptyDiv.Visible = true;
            }
            Label1.Text = txtMainSearch.Text;               
        }
        if (Session["itemno"] != null)
        {
            lblItemNo.Text = Session["itemno"].ToString();
        }
        
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "SetCurrentItemNumber();", true);
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        SearchResultPane.Visible = false;
        Response.Redirect("Login.aspx");
    }
    protected void SearchMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
        int index = int.Parse(e.Item.Value);
        MultiViewSearchResult.ActiveViewIndex = index;
        if(MultiViewSearchResult.ActiveViewIndex == 0)
        {

        }
        if (MultiViewSearchResult.ActiveViewIndex == 1)
        {

        }
    }
    public void RemoveChildLinks()
    {        
        dlProducten.DataSource = null;
        dlProducten.DataBind();
    }   
    
    protected void dlProducten_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("ShowItem"))
        {
            string[] values = e.CommandArgument.ToString().Split(new char[] { '|' });
            Response.Redirect("FilterItem.aspx?code=" + values[0] + "&description=" + values[1] + "&s=" + txtMainSearch.Text.Trim());
        }
    }
    protected void imgMainsearch_Click(object sender, ImageClickEventArgs e)
    {
        if (txtMainSearch.Text.Length == 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert", "<script>alert('To search you need to enter your search key!');</script>");
        }
        else
        {
            SearchResultPane.Visible = true;
            EmptyDiv.Visible = false;
            if (Session["searchstring"] != null)
            {
                Session["searchstring"] = txtMainSearch.Text;
            }
            else
            {
                Session.Add("searchstring", txtMainSearch.Text);
            }
            GetSearchResult();
        }
    }
    public void GetSearchResult()
    {
        if (Session["searchstring"] != null)
        {
            SearchResultPane.Visible = true;
            EmptyDiv.Visible = false;
            string inputString = Session["searchstring"].ToString();
            List<MaterialGroup> groups = new FacadeManager().GetSearchFilterData(inputString);
            dlProducten.DataSource = groups;
            dlProducten.DataBind();
        }
    } 

    protected void btnWShop_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShoppingCart.aspx");
    }

   
   
}
