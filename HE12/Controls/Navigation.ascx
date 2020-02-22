<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Navigation.ascx.cs" Inherits="Controls_Navigation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:UpdatePanel ID="UpdatePanelMenu" runat="server" UpdateMode="Conditional">
   <ContentTemplate>
     <div>        
        <asp:Accordion ID="accordMenu" runat="server" CssClass="">
        </asp:Accordion>       
     </div>
   </ContentTemplate>
   
</asp:UpdatePanel>
