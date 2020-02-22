<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Controls_Header" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type="text/javascript">
    function SetCurrentItemNumber() {

        var num = '<%= Session["itemno"] %>';
        alert(num);
        document.getElementById("<%=lblItemNo.ClientID %>").innerHTML = num;
        alert(num);
        return true;
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="HeaderContent" class="headercontent">
          <div id="ControlPane" class="controlpane"> 
             <div id="HeaderText" class="headertext"> 
                <label>Main Module</label>
                 <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
             </div>
             <div id="LoginPane" class="loginpane">
                 <asp:Button ID="btnLogin" runat="server" Text="Inloggen" Height="26px" Width="100px"
                     onclick="btnLogin_Click" />
                 <asp:Label ID="lblItemNo" runat="server" Text="0" Height="26px" Width="30px"></asp:Label>
                 <asp:Button ID="btnWShop" runat="server" Height="26px" Width="130px" 
                     Text="Winkelwagentje" onclick="btnWShop_Click" />
             </div>
             <div id="MainSearch" class="mainsearchdiv">
                 <div style="float:left;">
                    <asp:TextBox ID="txtMainSearch" runat="server" Width="155px" Height="20px"></asp:TextBox>                
                     <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                      TargetControlID="txtMainSearch"  WatermarkText="zoeken..." WatermarkCssClass="searchwatermarked">
                     </asp:TextBoxWatermarkExtender>
                 </div>
                 <div style="float:left;width:20px;">
                     <asp:ImageButton ID="imgMainsearch" ImageUrl="/he12/images/searchbtn.png" 
                         Height="24px" Width="20px" runat="server" onclick="imgMainsearch_Click" />
                 </div>
                                 
                 <%-- <asp:Button ID="btnSearch" runat="server" Text="" CssClass="searchbtnimage" 
                     Width="16" Height="16" onclick="btnSearch_Click" />--%>
             </div>                  
          </div> 
          <div id="EmptyDiv" runat="server" class="EmptyDiv">
            
          </div>
          <div id="SearchResultPane" runat="server" class="SearchResultPane">
            <div id="container" class="SearchContentDiv">
                <asp:Menu ID="SearchMenu" runat="server" Orientation="Horizontal" 
                    onmenuitemclick="SearchMenu_MenuItemClick">
                    <Items>
                        <asp:MenuItem Selected="true" Value="0" Text="content">
                        </asp:MenuItem>
                        <asp:MenuItem Value="1" Text="producten">
                        </asp:MenuItem>
                    </Items>
                </asp:Menu>
                <asp:MultiView ID="MultiViewSearchResult" runat="server" ActiveViewIndex="1" >
                    <asp:View ID="ContentView" runat="server">
                     <div id="cViewContent">
                          content here
                     </div>
                    </asp:View>
                    <asp:View ID="ProductenView" runat="server">
                      <div id="pViewContent" class="searchresultcontainerdiv">
                         <div class="searchlabel">
                            <label>Uw zoekopdracht heeft de volgende produkten opgeleverd: </label>
                         </div>
                         <div class="searchresultcontroldiv">                         
                             <asp:DataList ID="dlProducten" runat="server" RepeatColumns="5" RepeatDirection="Vertical" onitemcommand="dlProducten_ItemCommand">
                               <ItemTemplate>
                                    <asp:LinkButton ID="lnkGroup" CommandName="ShowItem" Width="165px"
                                    CommandArgument='<%# Eval("Code") +"|"+Eval("Description")%>' runat="server">
                                    <%# Eval("Description")%> &nbsp(<%# Eval("ItemCount") %>)
                                    </asp:LinkButton> 
                               </ItemTemplate>
                             </asp:DataList>
                                
                          <%--</div>      --%>           
                         </div>
   
                      </div>
                    </asp:View>
                </asp:MultiView>              
             </div>          
          </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnLogin" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
