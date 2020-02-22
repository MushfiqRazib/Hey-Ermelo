<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/WebShopMaster.master"
    AutoEventWireup="true" CodeFile="Confirmation.aspx.cs" Inherits="Confirmation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="contentHeader" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="AccountHeader" class="accountheader">
        <div class="loginicondiv">
            <img src="/he12/images/cartheader.png" />
        </div>
        <div class="loginheaderdiv">
            <div class="loginheaderlabel">
                <div class="loginheaderposition">
                    <label>
                        Winkelwagen</label>
                </div>
            </div>
            <div class="loginheaderdottedline">
                <label>
                    ------------------------------------------------------------------------------------------</label>
            </div>
        </div>
    </div>
    <div id="FilterPage" class="filterpage">
        <div id="FilterPane" class="filterpane" style="height: 650px">
            <asp:UpdatePanel ID="udpShoppingCart" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div id="FilterHeader" class="filterheader" style="font-size: 10px;">
                        Hier ziet u het totaaloverzicht van uw bestelling. Door op "Bestelling versturen"
                        te klikken, bevestigt u de bestelling definitief.
                    </div>
                    <div id="FilterPanelContent" class="filterpanelcontent" style="height: 200px; width: 672px;
                        overflow: auto">
                        <asp:GridView ID="gvSpCart" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="alternaterowstyle"
                            AutoGenerateColumns="False" DataKeyNames="OrderID,ItemID" EmptyDataText="No item added in the cart"
                            EnableSortingAndPagingCallbacks="false" GridLines="Both" HeaderStyle-CssClass="headerstyle"
                            OnPageIndexChanging="gvSpCart_PageIndexChanging" OnRowDataBound="gvStdCart_RowDataBound"
                            PageSize="10" RowStyle-CssClass="rowstyle" Width="670px">
                            <Columns>
                                <asp:BoundField DataField="Description" HeaderText="Omschrijving">
                                    <ControlStyle Width="300px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Remark" HeaderText="Opmerking">
                                    <ControlStyle Width="300px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Quantity" HeaderText="Aantal">
                                    <ControlStyle Width="50px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemTemplate>
                                        <div>
                                            €</div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UnitPrice" HeaderText="Per stuk" ItemStyle-HorizontalAlign="Right">
                                    <ControlStyle Width="35px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemTemplate>
                                        <div>
                                            €</div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Total" HeaderText="Totaal" ItemStyle-HorizontalAlign="Right">
                                    <ControlStyle Width="40px" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle ForeColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="alternaterowstyle" />
                        </asp:GridView>
                    </div>
                    <div id="dvBottomSummery">
                        <div id="dvAdminSpecialGrid" class="filterpanelcontent" runat="server" style="height: 200px;
                            width: 672px; overflow: auto;">
                            <asp:GridView ID="gvShoppingSpecial" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                PageSize="10" Width="670px" EmptyDataText="No item added in the cart" GridLines="Both"
                                HeaderStyle-CssClass="headerstyle" RowStyle-CssClass="rowstyle" AlternatingRowStyle-CssClass="alternaterowstyle"
                                EnableSortingAndPagingCallbacks="true" DataKeyNames="OrderID,ItemID" OnRowDataBound="gvSpRow_dataBound"
                                OnPageIndexChanging="gvSpecial_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Description" HeaderText="Omschrijving">
                                        <ControlStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderText="Opmerking">
                                        <ControlStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Quantity" HeaderText="Aantal">
                                        <ControlStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <div>
                                                €</div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UnitPrice" HeaderText="Per stuk" ItemStyle-HorizontalAlign="Right">
                                        <ControlStyle Width="35px" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <div>
                                                €</div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Total" HeaderText="Totaal" ItemStyle-HorizontalAlign="Right">
                                        <ControlStyle Width="40px" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle ForeColor="Black" Wrap="True" />
                                <AlternatingRowStyle CssClass="alternaterowstyle" />
                            </asp:GridView>
                        </div>
                        <hr />
                        <div style="width: 100%">
                            <div style="width: 55%; float: left">
                                &nbsp;</div>
                            <div style="float: left; width: 45%">
                                <div runat="server" id="costdetailContainer">
                                    <table class="shpCartCostDetail shpCartCostDetailfont">
                                        <tr>
                                            <td>
                                                Totaal producten
                                            </td>
                                            <td>
                                                <div>
                                                    €</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Korting producten
                                            </td>
                                            <td>
                                                <div>
                                                    €</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:Label ID="lblProductDiscout" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Korting overig
                                            </td>
                                            <td>
                                                <div>
                                                    €</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:Label ID="lblOtherDiscount" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Verzendkosten
                                            </td>
                                            <td>
                                                <div>
                                                    €</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:Label ID="lblShippingCost" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                BTW(19%)
                                            </td>
                                            <td>
                                                <div>
                                                    €</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:Label ID="lblTotalVat" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                            </td>
                                        </tr>
                                        <tr style="font-weight: bold">
                                            <td>
                                                Totaal incl. BTW
                                            </td>
                                            <td>
                                                <div>
                                                    €</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <asp:Label ID="lblTotalIncludingVat" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="ResultPane" runat="server" class="resultpane" style="padding-top: 15px;
        padding-left: 5px">
        <div style="float: left">
            <div>
                <div style="margin-top: 10px">
                    Uw referentie :</div>
                <div>
                    <asp:TextBox ID="txtCustRefCode" Width="200px" ReadOnly="true" MaxLength="50" runat="server"></asp:TextBox>
                </div>
            </div>
            <div>
                <div style="margin-top: 10px">
                    Opmerkingen:</div>
                <div>
                    <asp:TextBox ID="txtOrderRemark" Rows="4" Columns="60" ReadOnly="true" TextMode="MultiLine"
                        runat="server"></asp:TextBox>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            PageSize="2" Width="390px" EmptyDataText="No attached files" GridLines="Horizontal"
                            DataKeyNames="fileURI" HeaderStyle-CssClass="headerstyle" RowStyle-CssClass="rowstyle"
                            AlternatingRowStyle-CssClass="alternaterowstyle" EnableSortingAndPagingCallbacks="true"
                            ShowFooter="true" OnPageIndexChanging="gvFiles_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="fileName" HeaderText="Bijlagen">
                                    <ControlStyle Width="200px" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle ForeColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="alternaterowstyle" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="float: right; padding-right: 0px">
            <div class="filterpane" style="width: 270px; height: 280px">
                <div class="filterheader" style="width: 100%; text-align: center;">
                    Afronden
                </div>
                <div style="text-align: center; padding-top: 15px;font-size:10px;">
                    <div>
                        Door op 'Bestelling versturen' te klikken gaat u
                    </div>
                    <div>
                        akkoord met onze</div>
                    <div>
                        <a target="_blank" href="Instruction.pdf" runat="server" id="lnkPdfInstruction">Algemene
                            Voorwaarden</a>
                        <img src="/he12/images/pdf.jpg" />
                    </div>
                </div>
                <div id="divAno" runat="server" style="padding-top: 70px; text-align: center;">
                    <div>
                        <asp:Button ID="btnPriceRequest" runat="server" Text="Prijsaanvraag versturen" OnClick="btnPriceRequest_Click" />
                    </div>
                </div>
                <div id="divReg" runat="server" style="padding-top: 70px; text-align: center;">
                    <div>
                        <asp:Button ID="btnSendOffer" runat="server" Text="Offerte versturen" OnClick="btnSendOffer_Click" />
                        <asp:Button ID="btnPlaceOffer" runat="server" Text="Bestelling versturen" OnClick="btnPlaceOffer_Click" />
                    </div>
                </div>
                <div id="divAdm" runat="server" style="padding-top: 70px; text-align: center;">
                    <div>
                        <asp:CheckBox ID="chkConfirmation" runat="server" Text="Verstuur bevestiging" />
                    </div>
                    <div>
                        <asp:Button ID="btnAdmOffer" runat="server" Text="Offerte versturen" OnClick="btnAdmOffer_Click" />
                        <asp:Button ID="btnAdmOrder" runat="server" Text="Bestelling versturen" OnClick="btnAdmOrder_Click" />
                    </div>
                    <div>
                        <asp:Button ID="btnNaarProd" runat="server" Text="Naar Productie" OnClick="btnNaarProd_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
