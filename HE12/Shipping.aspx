<%@ Page Language="C#" MasterPageFile="~/MasterPage/WebShopMaster.master" AutoEventWireup="true" CodeFile="Shipping.aspx.cs" Inherits="Shipping" Title="Web Shop Module Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
     <ContentTemplate>
        <div id="AccountPage" class="accountpage">
        <div id="AccountHeader" class="accountheader">
            <div class="loginicondiv">
                <img src="/he12/images/login_icon.png" />
            </div>
            <div class="loginheaderdiv">
                 <div class="loginheaderlabel">
                     <div class="loginheaderposition">
                        <label>Verzendwijze</label> 
                     </div>                     
                 </div>
                 <div class="loginheaderdottedline">
                    <label>-----------------------------------------------------------------------------------------</label>
                 </div>  
            </div>
        </div>
        <div id="AccLogContainer" class="accountlogcontainer">
            <div id="AccountPanel" class="shippingpaneldiv accpanelmargin" >
                <div id="AccPanelHeader" class="panelheader">
                  <label>Afhalen</label>
                </div> 
                <div id="AccPanelContent" class="panelcontentcolor">
                    <div class="panelcontentmargin">
                        <div class="panelcontentheader">
                           <asp:RadioButton ID="rboCollectDate" Text="Afhalen in Ermelo (Gratis), zonder extra kosten uw bestelling afhalen bij ons filliaal in Ermelo." 
                           runat="server" OnCheckedChanged="rboCollect_OnCheckedChanged" 
                                AutoPostBack="True" />
                        </div>
                        <div class="panelcontrolscontainer">
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Afhaaldatum</label>
                                </div>
                                <div class="panelcontroltextbox" >
                                    <asp:TextBox ID="txtCollectDate" runat="server"  MaxLength="80" Enabled="false"></asp:TextBox>
                                    <img id="imgCal" src="/he12/images/Calendar.png" alt="Icon" />
                                    <asp:CalendarExtender ID="CalExtenderCollect" TargetControlID="txtCollectDate" 
                                    PopupButtonID="imgCal" runat="server" Enabled="false" >
                                    </asp:CalendarExtender>
                                </div>
                            </div>                           
                        </div>                                          
                        <div class="accbtn">
                            <asp:Button ID="btnCollect" runat="server" Text="Verder met afronden" Width="146px"
                                onclick="btnCollect_Click" ValidationGroup="AccountValidation" />                      
                        </div>
                    </div>
                </div>             
            </div>
            <div id="LoginPanel" class="shippingpaneldiv logpanelmargin">
                <div id="LogPHeader" class="panelheader">
                  <label>Bezorgen</label>
                </div> 
                 
                <div id="LogPanelContent" class="panelcontentcolor">
                    <div class="panelcontentmargin">
                        <div class="panelcontentheader">
                          <asp:RadioButton ID="rboDeliverDate" 
                                Text="Bezorgen, geef hieronder het adres waar u de bestelling bezorgd wilt hebben (geen postbus|)." 
                                runat="server" oncheckedchanged="rboDeliverDate_CheckedChanged" 
                                AutoPostBack="True" />
                        </div>                 
                        <div class="panelcontrolscontainer logincontrolpadding">
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Afleverdatum</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtDeliveryDate" runat="server"  MaxLength="80" Enabled="false"></asp:TextBox>
                                    <img id="imgDelCal" src="/he12/images/Calendar.png" alt="Icon" />
                                    <asp:CalendarExtender ID="CalendarExtenderDelivery" TargetControlID="txtDeliveryDate" 
                                    PopupButtonID="imgDelCal" runat="server"  Enabled="false">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Naam</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtName" runat="server"  MaxLength="255" Enabled="false"></asp:TextBox>
                                    <label>*</label>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtName"
                                     WatermarkText="Default name" WatermarkCssClass="watermarked">
                                    </asp:TextBoxWatermarkExtender> 
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                        EnableClientScript="False" ErrorMessage="please input naam." Display="None" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtName" ValidationGroup="DeliveryValidation">
                                        </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>T.a.v.</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtTav" runat="server"  MaxLength="20" Enabled="false"></asp:TextBox>                                   
                                </div>
                            </div>
                            
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Straat + Huisnr.</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtStreet" runat="server"  MaxLength="255" Enabled="false"></asp:TextBox>
                                    <label>*</label>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtStreet"
                                     WatermarkText="Default address" WatermarkCssClass="watermarked">
                                    </asp:TextBoxWatermarkExtender> 
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        EnableClientScript="False" ErrorMessage="please street." Display="None" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtStreet" ValidationGroup="DeliveryValidation">
                                        </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Postcode</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtZipcode" runat="server"  MaxLength="255" Enabled="false"></asp:TextBox>
                                    <label>*</label>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtZipcode"
                                     WatermarkText="Default zipcode" WatermarkCssClass="watermarked">
                                    </asp:TextBoxWatermarkExtender> 
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                        EnableClientScript="False" ErrorMessage="please postcode." Display="None" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtZipcode" ValidationGroup="DeliveryValidation">
                                        </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Plaats</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtCity" runat="server"  MaxLength="255" Enabled="false"></asp:TextBox>
                                    <label>*</label>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtCity"
                                     WatermarkText="Default city" WatermarkCssClass="watermarked">
                                    </asp:TextBoxWatermarkExtender> 
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                        EnableClientScript="False" ErrorMessage="please plaats." Display="None" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtCity" ValidationGroup="DeliveryValidation">
                                        </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            
                             <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Land</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:DropDownList ID="drpCountry"  runat="server" Enabled="false">
                                    </asp:DropDownList>
                                    <label>*</label>                                   
                                </div>
                            </div>
                                                  
                        </div>
                    
                         <div class="validationsummary">
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List"
                             ValidationGroup="DeliveryValidation" ShowSummary="true" ShowMessageBox="false" />
                        </div>                                           
                        <div class="logbtn">                            
                            <asp:Button ID="btnDelivery" runat="server" Text="Verder met afronden" Width="146px"
                                onclick="btnDelivery_Click" ValidationGroup="DeliveryValidation" OnClientClick="" />                      
                        </div>
                    </div>
                </div>    
            </div>        
        </div>
    </div>        
     </ContentTemplate>
     <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnCollect"  EventName="Click"/>
        <asp:AsyncPostBackTrigger ControlID="btnDelivery" EventName="Click" />
     </Triggers>
    </asp:UpdatePanel>
   
</asp:Content>

