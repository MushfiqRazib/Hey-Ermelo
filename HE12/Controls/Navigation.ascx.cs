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

public partial class Controls_Navigation : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
            LoadNavigationContainer();
        //}
    }

    public void LoadNavigationContainer()
    {
        List<MaterialGroup> itemGroups = new List<MaterialGroup>();
        itemGroups = new FacadeManager().GetMenuItems();

        foreach (MaterialGroup item in itemGroups)
        {
            AjaxControlToolkit.AccordionPane pane = new AjaxControlToolkit.AccordionPane();
            //create unique pane id...
            pane.ID = "Pane_" + item.Code;
            //pane.CssClass = "MenuHeaderDiv";
            // pane Header section ...
            HyperLink headerLink = new HyperLink();
            headerLink.Text = item.Description;
            headerLink.CssClass = "MenuHeader";
            HtmlGenericControl img = new HtmlGenericControl("img");
            img.Attributes["src"] = "images/menu_arrow.png";
            img.Attributes["class"] = "imgposition";

            pane.HeaderContainer.CssClass = "MenuHeaderDiv";
            pane.HeaderCssClass = "MenuHeaderDiv";

            pane.HeaderContainer.Controls.Add(headerLink);
            pane.HeaderContainer.Controls.Add(img);


            //creating list of children...
            if (item.ChildGroupItems.Count > 1)
            {
                // Pane Content section...
                HtmlGenericControl list;
                int i = 0;
                foreach (MaterialGroup mg in item.ChildGroupItems)
                {
                    i++;
                    list = new HtmlGenericControl("li");
                    LinkButton linkBtn = new LinkButton();
                    linkBtn.ID = i + "_" + mg.Code;
                    linkBtn.Attributes["runat"] = "server";
                    linkBtn.Text = mg.Description;
                    linkBtn.CommandArgument = mg.Code + "," + mg.Description;
                    linkBtn.CommandName = "GetItemCode";
                    linkBtn.CssClass = "MenuList";
                    linkBtn.Click += new EventHandler(linkBtn_Click);
                   
                    list.Controls.Add(linkBtn);
                    pane.ContentCssClass = "MenuList";
                    pane.ContentContainer.Controls.Add(list);
                }
            }
            accordMenu.Panes.Add(pane);
        }
    }

    protected void linkBtn_Click(Object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        if (lnk.CommandName == "GetItemCode")
        {
            string[] values = lnk.CommandArgument.ToString().Split(new char[] { ','});
            if (Session["searchstring"] != null)
            {
                Session.Remove("searchstring");
                //Session.Clear();
                //Session.Abandon();                
            }
            Response.Redirect("FilterItem.aspx?code=" + values[0]+ "&description=" + values[1]);
        }        
    }
}
