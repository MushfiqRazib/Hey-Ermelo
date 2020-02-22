using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using Hey.Common.Objects;
using Hey.Business;
using System.Web.Script.Services;

/// <summary>
/// Summary description for HeyWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class HeyWebService : System.Web.Services.WebService
{

    public HeyWebService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
    
    [WebMethod(EnableSession= true)]
    [ScriptMethod]
    public List<MaterialGroup> GetGroupItems()
    {
        List<MaterialGroup> groupList = new List<MaterialGroup>();
        groupList = new FacadeManager().GetMenuItems();
        return groupList;
    }

}

