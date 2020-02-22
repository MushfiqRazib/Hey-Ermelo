<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/WebShopMaster.master"
    AutoEventWireup="true" CodeFile="ShoppingCart.aspx.cs" Inherits="ShoppingCart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="contentHeader" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript">
        function IndexChanged() {
            document.getElementById("ctl00_ContentPlaceHolder1_ButtonIndexChanged").click();
        }

    </script>
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
                <label>------------------------------------------------------------------------------------------</label>
            </div>
        </div>
    </div>
    <div id="FilterPage" class="filterpage">
        <div id="FilterPane" class="filterpane" style="height: 650px">
            <asp:UpdatePanel ID="udpShoppingCart" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div id="FilterHeader" class="filterheader" style="font-size: 12px;">
                        De door u gekozen producten
                    </div>
                    <div id="FilterPanelContent" class="filterpanelcontent">
                        <asp:GridView ID="gvSpCart" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            PageSize="10" Width="670px" EmptyDataText="No item added in the cart" GridLines="Both"
                            HeaderStyle-CssClass="headerstyle" RowStyle-CssClass="rowstyle" AlternatingRowStyle-CssClass="alternaterowstyle"
                            EnableSortingAndPagingCallbacks="false" DataKeyNames="OrderID,ItemID" OnRowDeleting="gvSpCart_RowDeleting"
                            OnPageIndexChanging="gvSpCart_PageIndexChanging" OnRowDataBound="gvStdCart_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Width="70px" />
                                    <ItemTemplate>
                                        <div>
                                            <asp:ImageButton ID="btnSpecialItem" CommandArgument="<%# Container.DataItemIndex  %>"
                                                runat="server" ImageUrl="/he12/images/special_item.png" OnClick="btnSpecialItem_Click" />
                                            <asp:ImageButton ID="btnStandardItem" runat="server" ImageUrl="/he12/images/standard_tem.png"
                                                CommandArgument="<%# Container.DataItemIndex  %>" OnClick="btnStandardGridItemEdit_Click" />
                                            <asp:ImageButton CommandName="Delete" ImageUrl="/he12/images/delete.png" runat="server"
                                                ID="btnImgDelete" OnClientClick="return confirm('Are you sure to off-load this item from cart ?');" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="Omschrijving">
                                    <ControlStyle Width="220px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Opmerking">
                                    <ItemStyle Width="180px" />
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtRemark" Width="95%" Text='<%#Eval("Remark") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aantal">
                                    <ItemStyle Width="50px" />
                                    <ItemTemplate>
                                        <div style="float: left">
                                            <div style="float: left">
                                                <asp:TextBox ValidationGroup="requiredValidator" Style="text-align: right" runat="server"
                                                    ID="txtAantal" Text='<%#Eval("Quantity") %>' Width="75%" /></div>
                                            <div style="float: right">
                                                <asp:RequiredFieldValidator ID="requiredValidator" ControlToValidate="txtAantal"
                                                    runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ErrorMessage="" ControlToValidate="txtAantal" ID="RegularExpressionValidator1"
                                                    runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                    <div id="dvBottomSummery">
                        <div id="dvAdminSpecialGrid" class="filterpanelcontent" runat="server" style="width: 670px;
                            overflow: auto;">
                            <asp:GridView ID="gvShoppingSpecial" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                PageSize="10" Width="670px" EmptyDataText="No item added in the cart" GridLines="Both"
                                HeaderStyle-CssClass="headerstyle" RowStyle-CssClass="rowstyle" AlternatingRowStyle-CssClass="alternaterowstyle"
                                EnableSortingAndPagingCallbacks="true" DataKeyNames="OrderID,ItemID" OnRowDeleting="gvSpecial_RowDeleting"
                                OnRowDataBound="gvSpRow_dataBound" OnPageIndexChanging="gvSpecial_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Width="70px" />
                                        <ItemTemplate>
                                            <div>
                                                <asp:ImageButton ID="btnStandardItemAdminEdit" CommandArgument="<%# Container.DataItemIndex  %>"
                                                    runat="server" ImageUrl="/he12/images/standard_tem.png" OnClick="btnSpecialGridItemEdit_Click" />
                                                <asp:ImageButton CommandName="Delete" ImageUrl="/he12/images/delete.png" runat="server"
                                                    ID="btnImgDeleteSpecial" OnClientClick="return confirm('Are you sure to off-load this item from cart ?');" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Description" HeaderText="Omschrijving">
                                        <ControlStyle Width="220px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Opmerking">
                                        <ItemStyle Width="180px" />
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtRemark" Width="95%" Text='<%#Eval("Remark") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aantal">
                                        <ItemStyle Width="50px" />
                                        <ItemTemplate>
                                            <div style="float: left">
                                                <div>
                                                    <asp:TextBox ValidationGroup="spGridValidation" Style="text-align: right" runat="server"
                                                        ID="txtAantalSp" Text='<%#Eval("Quantity") %>' Width="75%" /></div>
                                                <div>
                                                    <asp:RequiredFieldValidator ValidationGroup="spGridValidation" ID="requiredValidatorSp"
                                                        ControlToValidate="txtAantalSp" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ErrorMessage="" ControlToValidate="txtAantalSp" ID="rfvNumeric"
                                                        ValidationGroup="spGridValidation" runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                            <div style="float: left; width: 62%">
                                <asp:Button ID="btnRemoveall" runat="server" Text="Winkelwagen legen" OnClick="btnRemoveall_Click" />
                                <asp:Button ID="btnUpdate" runat="server" Text="Bijwerken" OnClick="btnUpdate_Click" />
                                <asp:Button ID="btnAddSpecialItem" runat="server" Text="Special toevoegen" OnClick="btnAddSpecialItem_Click" />
                                <asp:Button ID="btnEditInfo" runat="server" Text="Overige gegevens..." OnClick="btnEditInfo_Click" />
                            </div>
                            <div style="float: right; width: 38%">
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
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                    <asp:PostBackTrigger ControlID="gvSpCart" />
                    <asp:PostBackTrigger ControlID="gvShoppingSpecial" />
                    <asp:PostBackTrigger ControlID="btnStandardPopOk" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="ResultPane" runat="server" class="resultpane" style="padding-top: 15px;
        padding-left: 9px">
        <div style="float: left; width: 461px;">
            <div>
                <div style="margin-top: 10px">
                    U kunt hier een referentie opgegeven voor uw bestelling (deze wordt op de factuur
                    afgedrukt) :</div>
                <div>
                    <asp:TextBox ID="txtCustRefCode" Width="200px" MaxLength="50" runat="server"></asp:TextBox>
                </div>
            </div>
            <div>
                <div style="margin-top: 10px">
                    Geef hier eventuele opmerkingen over de bestelling op:</div>
                <div>
                    <asp:TextBox ID="txtOrderRemark" Rows="4" Columns="60" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            PageSize="2" Width="461px" EmptyDataText="No attached files" GridLines="Horizontal"
                            DataKeyNames="fileURI" HeaderStyle-CssClass="headerstyle" RowStyle-CssClass="rowstyle"
                            AlternatingRowStyle-CssClass="alternaterowstyle" EnableSortingAndPagingCallbacks="true"
                            ShowFooter="true" OnRowDeleting="gvFiles_RowDeleting" OnPageIndexChanging="gvFiles_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Bijlagen">
                                    <ItemStyle Width="10px" />
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="Delete"
                                            ImageUrl="/he12/images/delete.png" runat="server" ID="btnImgDelete" OnClientClick="return confirm('Are you sure to delete this file ?');" />
                                        <asp:Label runat="server" Text='<%#Eval("fileName") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button ID="btnUploadHelper" runat="server" OnClick="btnShowUpload_Click" Text="Bijlage toevoegen" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle ForeColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="alternaterowstyle" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="float: right;">
            <div id="Div1" class="filterpane" style="width: 200px">
                <div id="Div2" class="filterheader" style="width: 200px; text-align: center">
                    Afronden
                </div>
                <div style="padding-top: 100px; text-align: center">
                    <div>
                        <asp:Label ID="lblBedrag" runat="server" Text=""></asp:Label>
                    </div>
                    <div>
                        <asp:Button ID="btnSubmit" runat="server" Text="Afronden" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div id="dvModalSpecial">
            <asp:Button ID="btnHiddenSpecialItem" runat="Server" Style="display: none" />
            <asp:ModalPopupExtender ID="mpeSpecialItem" TargetControlID="btnHiddenSpecialItem"
                PopupControlID="specialitempanel" CancelControlID="imgCancel" Drag="true" PopupDragHandleControlID="specialheader"
                runat="server" BackgroundCssClass="specialitemcontainer">
            </asp:ModalPopupExtender>
            <asp:Panel ID="specialitempanel" runat="server" CssClass="specialitempanelbackgroundcolor">
                <div class="modalcontainerlayout specialitemcontainer">
                    <div class="modalheader">
                        <asp:Panel ID="specialheader" runat="server">
                            <div class="modalheaderstyle">
                                Standaard Artikel - Verbijzonderen
                            </div>
                            <div class="modalclose">
                                <asp:ImageButton ID="imgCancel" runat="server" ImageUrl="/he12/images/special_close.png" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modalcontent">
                        <div class="modalcontentcontrols">
                            <div class="modalcontentrow">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Afgeleid van</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ReadOnly="true" ID="txtVan" Width="200px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Omschrijving</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtItemDescCustomise" Width="300px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Kostprijs</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <div class="floatleft modalinnercol1">
                                        <asp:TextBox ID="txtCostPrice" Width="60px" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtCostPrice"
                                            ID="rfvNumeric1" runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="modalinnercol2 floatleft">
                                        <div class="floatleft modallabelcolor">
                                            <label>
                                                Verkoopprijs</label>
                                        </div>
                                        <div class="floatright modalinnertextbox">
                                            <asp:TextBox ID="txtSellPrice" Width="60px" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtSellPrice"
                                                ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Aantal</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <div class="floatleft modalinnercol1 modallabelcolor">
                                        <asp:TextBox ID="txtQuanityCustomise" Width="60px" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblTotalQuantityCustomise" ValidationGroup="numericValidationCustomise"
                                            runat="server" Text=""></asp:Label>
                                        <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtQuanityCustomise"
                                            ID="rfvNumeric" ValidationGroup="numericValidationCustomise" runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="modalinnercol2 floatleft">
                                        <div class="floatleft modallabelcolor">
                                            <label>
                                                Eenheid</label>
                                        </div>
                                        <div class="floatright modalinnertextbox">
                                            <asp:TextBox ID="txtUnit" Width="60px" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modalcontentrowmultitxtbox modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Opmerkingen</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtRemarkCustomise" Width="300px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrowmultitxtbox modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Productie-aantekeningen</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtProdNodesCustomise" Width="300px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <asp:CheckBox ID="chkToEdm" runat="server" Text="Toevoegen aan EDM" />
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <asp:CheckBox ID="chkToProduct" runat="server" Text="Toevoegen aan Artikelenbestand" />
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <div class="modalbutton">
                                        <asp:Button ID="btnCustomise" runat="server" Text="Ok" OnClick="btnCustomise_Click" />
                                        <asp:Button ID="Button2" runat="server" Text="Annuleren" />
                                        <asp:HiddenField ID="hdnItemIDCustomise" runat="server" />
                                        <asp:HiddenField ID="hdnCurrentGridforCustom" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="dvModalStandard">
            <asp:Button ID="btnHiddenStandardItem" runat="Server" Style="display: none" />
            <asp:ModalPopupExtender ID="mpeStandardItem" TargetControlID="btnHiddenStandardItem"
                PopupControlID="standardItemPanel" CancelControlID="imgCancelStandard" Drag="true"
                PopupDragHandleControlID="standardHeader" runat="server" BackgroundCssClass="specialitemcontainer">
            </asp:ModalPopupExtender>
            <asp:Panel ID="standardItemPanel" runat="server" CssClass="specialitempanelbackgroundcolor">
                <div class="modalcontainerlayout specialitemcontainer" style="height: 365px">
                    <div class="modalheader">
                        <asp:Panel ID="standardHeader" runat="server">
                            <div class="modalheaderstyle">
                                Standaard Artikel - bewerken
                            </div>
                            <div class="modalclose">
                                <asp:ImageButton ID="imgCancelStandard" runat="server" ImageUrl="/he12/images/special_close.png" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modalcontent" style="height: 340px">
                        <div class="modalcontentcontrols">
                            <div class="modalcontentrow">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Materiaalcode</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ReadOnly="true" ID="txtStdMatCode" Width="120px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Omschrijving</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ReadOnly="true" ID="txtStdDescription" Width="300px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Aantal</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <div class="modalinnercol1">
                                        <asp:TextBox ID="txtStdQuantiry" ValidationGroup="editStdPop" Width="60px" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ControlToValidate="txtStdQuantiry" ID="rfvNumericEdit"
                                            ErrorMessage="*" ValidationGroup="editStdPop" runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin" style="padding-top: 10px">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Opmerkingen</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtStdRemark" Width="300px" Height="60px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin" style="padding-top: 10px">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Productie-aantekeningen</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtStdProdNote" Width="300px" Height="60px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin" style="padding-top: 10px">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <div class="modalbutton">
                                        <asp:Button ID="btnStandardPopOk" ValidationGroup="editStdPop" CssClass="button"
                                            runat="server" Text="Ok" OnClick="SaveItem_Click" />
                                        <asp:Button ID="btnStandardPopCancel" CssClass="button" runat="server" Text="Annuleren" />
                                        <asp:HiddenField runat="server" ID="hdnItemID" />
                                        <asp:HiddenField runat="server" ID="hdnCurrentGrid" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="dvModalAttachment">
            <asp:Button ID="btnHiddenAttachment" runat="Server" Style="display: none" />
            <asp:ModalPopupExtender ID="modalPopupAttachment" TargetControlID="btnHiddenAttachment"
                PopupControlID="attachmentPanel" CancelControlID="imgAttachmentClose" Drag="true"
                PopupDragHandleControlID="attachmentHeader" runat="server" BackgroundCssClass="specialitemcontainer">
            </asp:ModalPopupExtender>
            <asp:Panel ID="attachmentPanel" runat="server" CssClass="specialitempanelbackgroundcolor">
                <div class="modalcontainerlayout specialitemcontainer" style="height: 130px">
                    <div class="modalheader">
                        <asp:Panel ID="attachmentHeader" runat="server">
                            <div class="modalheaderstyle">
                                Bijlage toevoegen
                            </div>
                            <div class="modalclose">
                                <asp:ImageButton ID="imgAttachmentClose" runat="server" ImageUrl="/he12/images/special_close.png" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modalcontent" style="height: 100px">
                        <div class="modalcontentcontrols" style="padding-left: 20px">
                            <label>
                                Bestand</label>
                            <asp:FileUpload ID="fileUploadsCart" runat="server" Size="50" />
                            <asp:RequiredFieldValidator ID="rfvFileAttachment" ValidationGroup="fileAttachment"
                                runat="server" ControlToValidate="fileUploadsCart" ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                            <div style="width: 456px; text-align: right; padding-top: 15px;">
                                <asp:Button ValidationGroup="fileAttachment" ID="btnUpload" runat="server" Text="Versturen"
                                    OnClick="UploadFile_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Annuleren" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="divModalAddSpecial">
            <asp:Button ID="btnHidAddSpecialItem" runat="Server" Style="display: none" />
            <asp:ModalPopupExtender ID="mpeAddSpecialItem" TargetControlID="btnHidAddSpecialItem"
                PopupControlID="addspecialitempanel" CancelControlID="imgaddCancel" Drag="true"
                PopupDragHandleControlID="addspecialheader" runat="server" BackgroundCssClass="specialitemcontainer">
            </asp:ModalPopupExtender>
            <asp:Panel ID="addspecialitempanel" runat="server" CssClass="specialitempanelbackgroundcolor">
                <div class="modalcontainerlayout specialitemcontainer">
                    <div class="modalheader">
                        <asp:Panel ID="addspecialheader" runat="server">
                            <div class="modalheaderstyle">
                                Speciaal Artikel - Toevoegen
                            </div>
                            <div class="modalclose">
                                <asp:ImageButton ID="imgaddCancel" runat="server" ImageUrl="/he12/images/special_close.png" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modalcontent">
                        <div class="modalcontentcontrols">
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Omschrijving</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAddSpDescription" Width="300px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Kostprijs</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <div class="floatleft modalinnercol1">
                                        <asp:TextBox ID="txtAddSpCost" Width="60px" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtAddSpCost"
                                            ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="modalinnercol2 floatleft">
                                        <div class="floatleft modallabelcolor">
                                            <label>
                                                Verkoopprijs</label>
                                        </div>
                                        <div class="floatright modalinnertextbox">
                                            <asp:TextBox ID="txtAddSpSell" Width="60px" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtAddSpSell"
                                                ID="RegularExpressionValidator3" runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Aantal</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <div class="floatleft modalinnercol1 modallabelcolor">
                                        <asp:TextBox ID="txtAddSpAntal" Width="60px" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblAddTotalQuantity" ValidationGroup="numericValidationCustomise"
                                            runat="server" Text=""></asp:Label>
                                        <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtAddSpAntal"
                                            ID="RegularExpressionValidator4" ValidationGroup="numericValidationCustomise"
                                            runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="modalinnercol2 floatleft">
                                        <div class="floatleft modallabelcolor">
                                            <label>
                                                Eenheid</label>
                                        </div>
                                        <div class="floatright modalinnertextbox">
                                            <asp:TextBox ID="txtAddEenhd" Width="60px" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modalcontentrowmultitxtbox modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Opmerkingen</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAddOpmerk" Width="300px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrowmultitxtbox modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Productie-aantekeningen</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAddProductie" Width="300px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <asp:CheckBox ID="chkAddSpEDM" runat="server" Text="Toevoegen aan EDM" />
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <asp:CheckBox ID="chkAddSpArtikel" runat="server" Text="Toevoegen aan Artikelenbestand" />
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <div class="modalbutton">
                                        <asp:Button ID="btnAddSpOK" runat="server" Text="Ok" OnClick="btnAddSpOK_Click" />
                                        <asp:Button ID="btnAddSpCancel" runat="server" Text="Annuleren" OnClick="btnAddSpCancel_Click" />
                                        <asp:HiddenField ID="hdnItemIdAdding" runat="server" />
                                        <asp:HiddenField ID="hdnCurrentgridForAdding" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="divModalEditInfo">
            <asp:Button ID="btnAdditionalEditInfo" runat="Server" Style="display: none" />
            <asp:ModalPopupExtender ID="mpeEditInfo" TargetControlID="btnAdditionalEditInfo"
                PopupControlID="EditInfoPanel" CancelControlID="imgEditCancel" Drag="true" PopupDragHandleControlID="editinfoheader"
                runat="server" BackgroundCssClass="specialitemcontainer">
            </asp:ModalPopupExtender>
            <asp:Panel ID="EditInfoPanel" runat="server" CssClass="specialitempanelbackgroundcolor">
                <div class="modalcontainerlayout specialitemcontainer">
                    <div class="modalheader">
                        <asp:Panel ID="editinfoheader" runat="server">
                            <div class="modalheaderstyle">
                                Projectgegevens
                            </div>
                            <div class="modalclose">
                                <asp:ImageButton ID="imgEditCancel" runat="server" ImageUrl="/he12/images/special_close.png" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="modalcontent">
                        <div class="modalcontentcontrols">
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Klant</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:DropDownList ID="drpKlant" Width="300px" runat="server" onchange="IndexChanged();" EnableViewState="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Projectcode</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAddInfoPrjCode" Width="150px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Omschrijving</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAdditionalInfoDescription" Width="300px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Verzendkosten</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAddInfoVerzend" Width="150px" Text="#,##" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Totaalprijs</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAddInfoTotalPrc" Width="150px" Text="#,##" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrowmultitxtbox modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    <label>
                                        Productie-aantekeningen</label>
                                </div>
                                <div class="modalcontentcolumn2">
                                    <asp:TextBox ID="txtAddInfoProductie" Width="300px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modalcontentrow modalcontentrowmargin">
                                <div class="modalcontentcolumn1 modallabelcolor">
                                    &nbsp;
                                </div>
                                <div class="modalcontentcolumn2 modallabelcolor">
                                    <div class="modalbutton">
                                        <asp:Button ID="btnAddInfoOK" runat="server" Text="Ok" OnClick="btnAddInfoOK_Click" />
                                        <asp:Button ID="btnAddInfoCancel" runat="server" Text="Annuleren" OnClick="btnAddInfoCancel_Click" />
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
