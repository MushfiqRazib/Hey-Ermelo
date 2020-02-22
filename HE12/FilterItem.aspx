<%@ Page Language="C#" MasterPageFile="~/MasterPage/WebShopMaster.master" AutoEventWireup="true" CodeFile="FilterItem.aspx.cs" Inherits="FilterItem" Title="Web Shop Module: Filter Item" %>

<%@ MasterType VirtualPath="~/MasterPage/WebShopMaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
       <ContentTemplate>
            <div id="FilterPage" class="filterpage">
                <div id="FilterPane" class="filterpane width676px">
                    <div id="FilterHeader" class="filterheader width676px">
                      Filter <label id="ItemName" runat="server"></label> op
                    </div> 
                    <div id="FilterPanelContent" class="filterpanelcontent width676px">
                       <div class="filterpanelcontainer"> 
                           <div id="ImgContainer" class="imgcontainer" >
                                <div> 
                                    <asp:Image ID="groupImg" Height="85px" Width="85px" runat="server" />                                   
                                </div>
                           </div>  
                           <div id="FilterControlsContainer" class="filtercontrolscontainer"> 
                                <div id="ComboContainer" class="combocontainer">
                                    <asp:Panel ID="ComboPanel" runat="server">                                            
                                    </asp:Panel>
                                </div>
                                <div id="ButtonContainer" class="buttoncontainer"> 
                                    <div id="FilterBtn" class="filterbtn">
                                        <asp:Button ID="btnFilter" Width="100px" runat="server" Text="Toon Selectie" 
                                            onclick="btnFilter_Click" />
                                    </div>
                                     <div id="FilterClearBtn" class="filterclearbtn">
                                        <asp:Button ID="btnClearBtn" Width="100px" runat="server" 
                                             Text="Verwijder Filter" onclick="btnClearBtn_Click" />
                                    </div>
                                    <div id="SearchBtn" class="filtersearchbtndiv">
                                        <div class="searchtxtbox">
                                            <asp:TextBox ID="txtFilterSearch" runat="server" Width="130px" Height="18px" MaxLength="5"></asp:TextBox>
                                            
                                            <asp:TextBoxWatermarkExtender
                                                ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtFilterSearch" WatermarkCssClass="searchwatermarked"
                                                 WatermarkText="zoeken...">
                                            </asp:TextBoxWatermarkExtender>
                                        </div>
                                        <div class="searchimgbtn">
                                            <asp:ImageButton ID="imgFilterSearch" ImageUrl="/he12/images/searchbtn.png" 
                                                Height="22px" Width="20px" runat="server" onclick="imgFilterSearch_Click" />
                                        </div>
                                        
                                    </div>
                                </div>
                           </div>   
                       </div>
                    </div>
                </div>
            </div> 
           
            <div id="ResultPane" runat="server" class="resultpane resultpanehide">
                <div id="ResultGrid" class="resultgrid">
                    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" 
                        AllowPaging="True" PageSize="40" Width="676px" EmptyDataText="NO DATA" 
                        GridLines="None" HeaderStyle-CssClass="headerstyle" 
                        RowStyle-CssClass="rowstyle" AlternatingRowStyle-CssClass="alternaterowstyle" 
                        onpageindexchanging="gvResult_PageIndexChanging" EnableSortingAndPagingCallbacks="true" 
                        DataKeyNames="Matcode" OnRowDataBound="gvResult_OnRowDataBound" Font-Size="12px">
                        <RowStyle CssClass="rowstyle" />
                        <Columns>
                            <asp:BoundField DataField="Matcode" HeaderText="Artikelnr" >
                                <ControlStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Omschrijving" >
                            
                             <ControlStyle Width="400px" />
                              </asp:BoundField>
                            <asp:BoundField DataField="Unit" HeaderText="Eenheid" >
                             <ControlStyle Width="150px" />
                              </asp:BoundField>
                               <asp:BoundField DataField="UnitSign" HeaderText=""  >
                             <ControlStyle Width="30px" />
                              </asp:BoundField>
                            <asp:BoundField DataField="SellPrice" HeaderText="Prijs"   >
                             <ControlStyle Width="150px"/>
                              </asp:BoundField>                           
                            
                            <asp:TemplateField ShowHeader="false" SortExpression="false" >
                            
                                <ItemTemplate>
                                    <div class="templatestyle">
                                        <asp:TextBox ID="txtQuantity" runat="server" Width="70px" ></asp:TextBox>
                                        <asp:RegularExpressionValidator ErrorMessage="" ControlToValidate="txtQuantity" ID="rfvNumeric"
                                        runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                        <asp:ImageButton ID="btnShoppingCart"  runat="server" ImageUrl="/he12/images/shoppping_cart.png"
                                          OnClick="btnShoppingCart_Click" ToolTip="Shopping Cart" />
                                        <asp:ImageButton ID="btnSpecialItem" runat="server" ImageUrl="/he12/images/special_item.png" 
                                         OnClick="btnSpecialItem_Click" ToolTip="Add Special Item" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                             
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPurchasePrice" runat="server" Text='<%# Eval("PurchasePrice") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                                
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSellPrice" runat="server" Text='<%# Eval("SellPrice") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                        <HeaderStyle ForeColor="Black" Wrap="True" />
                        <AlternatingRowStyle CssClass="alternaterowstyle" />
                    </asp:GridView>
                    <asp:Button ID="btnHiddenSpecialItem" runat="Server" Style="display: none" /> 
                    <asp:ModalPopupExtender ID="mpeSpecialItem" 
                         TargetControlID="btnHiddenSpecialItem"
                         PopupControlID="specialitempanel" 
                         CancelControlID="imgCancel" Drag="true" 
                         PopupDragHandleControlID="specialheader" 
                         runat="server" 
                         BackgroundCssClass="specialitemcontainer">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="specialitempanel" runat="server" CssClass="specialitempanelbackgroundcolor">
                        <div class="modalcontainerlayout specialitemcontainer">
                            <div class="modalheader">
                                <asp:Panel ID="specialheader" runat="server">
                                    <div class="modalheaderstyle">
                                    Speciaal product - Toevoegen  
                                    </div>
                                    <div class="modalclose">
                                        <asp:ImageButton ID="imgCancel"  runat="server" 
                                            ImageUrl="/he12/images/special_close.png" 
                                        
                                        />
                                    </div>
                                </asp:Panel>
                            </div> 
                            
                            <div class="modalcontent">
                                <div class="modalcontentcontrols">
                                  <div class="modalcontentrow">
                                        <div class="modalcontentcolumn1 modallabelcolor">
                                          <label>Afgeleid van</label>
                                        </div>
                                        <div class="modalcontentcolumn2">
                                            <asp:TextBox ID="txtVan" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                  </div>
                                  <div class="modalcontentrow modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor">
                                          <label>Omschrijving</label>
                                        </div>
                                       <div class="modalcontentcolumn2">
                                            <asp:TextBox ID="txtOms" Width="300px" runat="server"></asp:TextBox>
                                        </div>
                                  </div>
                                  
                                   <div class="modalcontentrow modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor">
                                          <label>Kostprijs</label>
                                        </div>
                                        <div class="modalcontentcolumn2">
                                            <div class="floatleft modalinnercol1">
                                                <asp:TextBox ID="txtKost" Width="60px" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtKost" ID="rfvNumeric1"
                                        runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="modalinnercol2 floatleft">
                                                <div class="floatleft modallabelcolor">
                                                   <label>Verkoopprijs</label>
                                                </div>
                                                <div class="floatright modalinnertextbox">
                                                    <asp:TextBox ID="txtVerk" Width="60px" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtVerk" ID="RegularExpressionValidator1"
                                        runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                  </div>
                                  
                                   <div class="modalcontentrow modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor">
                                          <label>Aantal</label>
                                        </div>
                                        <div class="modalcontentcolumn2">
                                            <div class="floatleft modalinnercol1 modallabelcolor">
                                                <asp:TextBox ID="txtAntal" Width="60px" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ErrorMessage="*" ControlToValidate="txtAntal" ID="RegularExpressionValidator2"
                                        runat="server" ValidationExpression="^[-]?\d*\,?\d*$"></asp:RegularExpressionValidator>
                                                <label>/ </label> <asp:Label ID="lblDefaultAantal" runat="server" Width="30px" Text="10"></asp:Label>
                                            </div>
                                            <div class="modalinnercol2 floatleft">
                                                <div class="floatleft modallabelcolor">
                                                   <label>Eenheid</label>
                                                </div>
                                                <div class="floatright modalinnertextbox">
                                                    <asp:TextBox ID="txtEnhd" Width="60px" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                  </div>
                                  
                                   <div class="modalcontentrowmultitxtbox modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor">
                                          <label>Opmerkingen</label>
                                        </div>
                                       <div class="modalcontentcolumn2">
                                            <asp:TextBox ID="txtOpmerk" Width="300px" runat="server" TextMode="MultiLine" ></asp:TextBox>
                                        </div>
                                  </div>
                                  
                                  
                                   <div class="modalcontentrowmultitxtbox modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor">
                                          <label>Productie-aantekeningen</label>
                                        </div>
                                       <div class="modalcontentcolumn2">
                                            <asp:TextBox ID="txtProdNotes" Width="300px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                  </div>
                                  
                                   <div class="modalcontentrow modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor"> 
                                         &nbsp;                                        
                                        </div>
                                       <div class="modalcontentcolumn2 modallabelcolor">
                                           <asp:CheckBox ID="chkEDM" runat="server"  Text="Toevoegen aan EDM"/>
                                        </div>
                                  </div>
                                  
                                   <div class="modalcontentrow modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor">  
                                         &nbsp;                                          
                                        </div>
                                       <div class="modalcontentcolumn2 modallabelcolor">
                                           <asp:CheckBox ID="chkArtikel" runat="server"  Text="Toevoegen aan Artikelenbestand"/>
                                        </div>
                                  </div>
                                  
                                   <div class="modalcontentrow modalcontentrowmargin">
                                        <div class="modalcontentcolumn1 modallabelcolor">  
                                         &nbsp;                                          
                                        </div>
                                       <div class="modalcontentcolumn2 modallabelcolor">    
                                          <div class="modalbutton">                                    
                                           <asp:Button ID="btnOK" runat="server" Text="Ok" onclick="btnOK_Click" />
                                           <asp:Button ID="btnCancel" runat="server" Text="Annuleren" 
                                                  onclick="btnCancel_Click" />
                                          </div>
                                        </div>
                                  </div>
                                  
                                  

                                </div>
                            </div>                       
                        </div>
                    </asp:Panel>   
                </div>            
            </div>
       </ContentTemplate>  
       <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
          <asp:AsyncPostBackTrigger ControlID="btnClearBtn" EventName="Click" />
       </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
           <div style="position: relative; top: 30%; text-align: center;"> 
              <img src="/he12/images/loader.gif" style="vertical-align:middle" alt="Processing"/>
                 Processing ... 
           </div> 
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>

