<%@ Page Language="C#" MasterPageFile="~/MasterPage/WebShopMaster.master" AutoEventWireup="true" CodeFile="WebShopAccount.aspx.cs" Inherits="WebShopAccount" Title="Shopping Module Account" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">

function validate(){
      if(document.getElementById("<%=txtUserName.ClientID %>").value=="") {
          // alert("Email id can not be blank");
           document.getElementById("<%=txtUserName.ClientID %>").focus();
           return false;
      }
      //var emailPat = /^(\".*\"|[A-Za-z]\w*)@(\[\d{1,3}(\.\d{1,3}){3}]|[A-Za-z]\w*(\.[A-Za-z]\w*)+)$/;
      //var rgexpr = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ;
      var email = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
      var emailAddress=document.getElementById("<%=txtUserName.ClientID %>").value;
      //var matchArray = emailid.match(rgexpr);
      if (!email.test(emailAddress)){
          // alert("Your email address seems incorrect. Please try again.");
           document.getElementById("<%=txtUserName.ClientID %>").focus();           
           document.getElementById("fpwddiv").innerHTML = "Het opgegeven e-mailadres is onbekend in ons systeem";
           return false;
     }


 return true;
}
</script>
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
                        <label>Uw gegevens</label> 
                     </div>                     
                 </div>
                 <div class="loginheaderdottedline">
                    <label>-----------------------------------------------------------------------------------------</label>
                 </div>  
            </div>
        </div>
        <div id="AccLogContainer" class="accountlogcontainer">
            <div id="AccountPanel" class="accountpaneldiv accpanelmargin" style="height: 370px;">
                <div id="AccPanelHeader" class="panelheader">
                  <label>Ik heb geen account</label>
                </div> 
                <div id="AccPanelContent" class="panelcontentcolor">
                    <div class="panelcontentmargin">
                        <div class="panelcontentheader">
                            <label>Geef hier uw gegevens op indien u geen account heeft.</label>
                        </div>
                        <div class="panelcontentheader">
                            <label>Geef alleen een bedrijfsnaam of -pand op indien van toepassing.</label>
                        </div>
                        
                        <div class="panelcontrolscontainer">
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Bedrijfsnaam</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="50"></asp:TextBox>
                                     <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" TargetControlID="txtCompanyName"
                                     WatermarkText="Company name" WatermarkCssClass="watermarked"></asp:TextBoxWatermarkExtender>
                                </div>
                            </div>
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Contactpersoon</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="80"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtContactPerson"
                                     WatermarkText="Contact person" WatermarkCssClass="watermarked"></asp:TextBoxWatermarkExtender>
                                    <label class="requirecolor">*</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="requirecolor" 
                                        ControlToValidate="txtContactPerson" ErrorMessage="De rood gemarkeerde velden zijn verplicht"   
                                        Display="None"  ValidationGroup="AccountValidation" />
                                </div>
                            </div>
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Telefoonnummer</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtPhoneNo" MaxLength="15" runat="server"></asp:TextBox>
                                     <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtPhoneNo"
                                     WatermarkText="Phone number" WatermarkCssClass="watermarked"></asp:TextBoxWatermarkExtender>
                                    <label class="requirecolor">*</label>  
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="requirecolor" 
                                        ControlToValidate="txtPhoneNo" ErrorMessage="De rood gemarkeerde velden zijn verplicht"   
                                        Display="None"  ValidationGroup="AccountValidation" />                                     
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="requirecolor"
                                        ControlToValidate="txtPhoneNo" ErrorMessage="De rood gemarkeerde velden zijn verplicht" Display="None"
                                        ValidationGroup="AccountValidation" ValidationExpression="^\d+$"  
                                        >*</asp:RegularExpressionValidator>
                                       
                                </div>
                            </div>
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>E-mailadres</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="255"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtEmail"
                                     WatermarkText="Email Address" WatermarkCssClass="watermarked"></asp:TextBoxWatermarkExtender>
                                    <label class="requirecolor">*</label>                                       
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  CssClass="requirecolor"
                                        EnableClientScript="False" ErrorMessage="De rood gemarkeerde velden zijn verplicht" Display="None" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtEmail" ValidationGroup="AccountValidation"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            
                             <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>&nbsp;</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:CheckBox ID="chkAccount" Text="Ik wil graag een account aanvragen" runat="server" />
                                </div>
                            </div>
                        
                        </div>
                        
                        <div class="validationsummary">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                             ValidationGroup="AccountValidation" ShowSummary="true" ShowMessageBox="false" />
                        </div>
                        <div class="labelmsg">
                                <asp:Label ID="lblStatus" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                        <div class="accbtn">
                            <asp:Button ID="btnAccount" runat="server" Text="Prijsaanvraag" Width="146px"
                                onclick="btnAccount_Click" ValidationGroup="AccountValidation" />                      
                        </div>
                    </div>
                </div>             
            </div>
            <div id="LoginPanel" class="accountpaneldiv logpanelmargin" style="height: 370px;">
                <div id="LogPHeader" class="panelheader">
                  <label>Ik heb al een account</label>
                </div> 
                 
                <div id="LogPanelContent" class="panelcontentcolor">
                    <div class="panelcontentmargin">
                        <div class="panelcontentheader">
                            <label>Geef hier de gebruikersnaam en het wachtwoord  op van het<br /> 
                              account waarop u de order wilt ontvangen.</label>
                        </div>                 
                        <div class="panelcontrolscontainer logincontrolpadding">
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Gebruikersnaam</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="255"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtUserName"
                                     WatermarkText="Username" WatermarkCssClass="watermarked">
                                    </asp:TextBoxWatermarkExtender> 
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                        EnableClientScript="False" ErrorMessage="Gebruikersnaamen/of wachtwoord is onjuist." Display="None" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtUserName" ValidationGroup="LoginValidation">
                                        </asp:RegularExpressionValidator>
                                        
                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="txtUserName" ErrorMessage="Gebruikersnaamen/of wachtwoord is onjuist." 
                                        Display="None" ValidationGroup="LoginValidation">
                                        </asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                            <div class="panelcontroldiv">
                                <div class="panelcontrollabel">
                                    <label>Wachtwoord</label>
                                </div>
                                <div class="panelcontroltextbox">
                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="20" 
                                        TextMode="Password"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtPassword"
                                     WatermarkText="Password" WatermarkCssClass="watermarked"></asp:TextBoxWatermarkExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                        ControlToValidate="txtPassword" ErrorMessage="Gebruikersnaamen/of wachtwoord is onjuist." Display="None"
                                        ValidationGroup="LoginValidation"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="panelcontroldiv">
                                <div class="panelhyperlink">
                                    <asp:LinkButton ID="lnkForgetPassword" runat="server"
                                     OnClientClick="return validate()" onclick="lnkForgetPassword_Click"
                                    >Wachtwoord vergeten?</asp:LinkButton>                                   
                                </div>
                            </div>                            
                        </div>
                    
                         <div class="validationsummary">
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List"
                             ValidationGroup="LoginValidation" ShowSummary="true" ShowMessageBox="false" />
                        </div>
                        <div class="labelmsg">
                                <asp:Label ID="lblMsg" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                        <div id="fpwddiv" class="labelmsg"></div>                        
                        <div class="logbtn">                            
                            <asp:Button ID="btnLogin" runat="server" Text="Inloggen" Width="146px"
                                onclick="btnLogin_Click" ValidationGroup="LoginValidation" OnClientClick="" />                      
                        </div>
                    </div>
                </div>    
            </div>        
        </div>
    </div>        
     </ContentTemplate>
     <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnLogin"  EventName="Click"/>
        <asp:AsyncPostBackTrigger ControlID="btnAccount" EventName="Click" />
     </Triggers>
    </asp:UpdatePanel>
   
</asp:Content>

