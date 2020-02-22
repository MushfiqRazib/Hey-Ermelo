using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hey.DataAccess;
using Hey.Common.Objects;
using System.Web.UI.WebControls;
using System.Data;

namespace Hey.Business
{
    public class FacadeManager
    {
        public FacadeManager()
        {
        }

        public HCustomers GetCustomerInfoByLogin(string username, string password)
        {
            return BusinessObjectManager.GetCustomerDataByLogin(username, password);
        }

        public List<MaterialGroup> GetMenuItems()
        {
            return BusinessObjectManager.GetMenuItems();
        }
        public bool SendMailToUser(EmailInfo info)
        {
            return BusinessObjectManager.SendRequestMailToUser(info);
        }
        public bool SendPasswordRecoveryMail(EmailInfo info)
        {
            return BusinessObjectManager.SendPasswordRecoveryMail(info);
        }

        public List<MaterialGroupFilter> GetNumberOfFilterCombo(string code)
        {
            return BusinessObjectManager.GetNumberOfFilterCombos(code);
        }
        public DropDownList CreteFilterCombo(MaterialGroupFilter mgCombo)
        {
            return BusinessObjectManager.CreateFilterCombo(mgCombo);
        }

        public List<BaseMaterial> GetFilterData(string code, string fieldnames, string fieldvalues)
        {
            return BusinessObjectManager.GetFilterData(code, fieldnames, fieldvalues);
        }
        public List<MaterialGroup> GetSearchFilterData(string input)
        {
            return BusinessObjectManager.GetSearchFilterData(input);
        }
        public List<BaseMaterial> GetSubgroupFilterSearchData(string code)
        {
            return BusinessObjectManager.GetSubgroupFilterSearchData(code);
        }

        public List<BaseMaterial> FilterPanelSearchData(string code, string searchstring)
        {
            return BusinessObjectManager.FilterPanelSearchData(code, searchstring);
        }


        public int InsertShoppingCartItem(WebOrderItems woItem)
        {
            return BusinessObjectManager.InsertShoppingCartItem(woItem);
        }
        public int InsertShoppingOrder(WebOrders webOrder)
        {
            return BusinessObjectManager.InsertShoppingOrder(webOrder);
        }

        public DataTable GetDiscountForCustomer(string CustCode)
        {
            return BusinessObjectManager.GetDiscountForCustomer(CustCode);
        }
        public DataTable CheckOrderExist(string SessionId)
        {
            return BusinessObjectManager.CheckOrderExist(SessionId);
        }

        public DataTable CheckREGOrADMOrderExist(string CustCode)
        {
            return BusinessObjectManager.CheckREGOrADMOrderExist(CustCode);
        }

        public DataTable CheckOrderItemExist(int OrderId, string ItemCode)
        {
            return BusinessObjectManager.CheckOrderItemExist(OrderId, ItemCode);
        }

        public int UpdateShoppingCartItem(WebOrderItems woItem)
        {
            return BusinessObjectManager.UpdateShoppingCartItem(woItem);
        }

        public int UpdateShoppingCartSpecialItem(WebOrderItems woItem)
        {
            return BusinessObjectManager.UpdateShoppingCartSpecialItem(woItem);
        }


        public List<IShoppingCart> GetShoppingCart(string sessionid, ItemTypeEnum iType, UserType uType)
        {
            ShoppingCartManager shoppingMag = new ShoppingCartManager();
            return shoppingMag.GetShoppingCart(sessionid, iType, uType);
        }

        public void UpdateShoppingCart(IShoppingCart shoppingCart)
        {
            shoppingCart.UpdateItems();
        }
        public void UpdateShoppingCartItem(IShoppingCart shoppingCart)
        {
            shoppingCart.UpdateItem();
        }

        public void RemoveShoppingCartItem(IShoppingCart shoppingCart)
        {
            shoppingCart.DeleteItem();
        }

        public void RemoveShoppingCart(IShoppingCart shoppingCart)
        {
            shoppingCart.DeleteCart();
        }

        public List<UploadedFile> GetUploadedFiles(string userDirectory)
        {
            return UploadedFile.GetFileList(userDirectory);
        }

        public void UploadFile(FileUpload fileControl, string userDirectory)
        {
            UploadedFile.UploadFile(fileControl, userDirectory);
        }

        public DataTable ShopOrderCostDetail(string order_id)
        {
            ShoppingCartManager spCart = new ShoppingCartManager();
            return spCart.ShopOrderCostDetail(order_id);
        }

        public IShoppingCart GetACartItem(string item_id)
        {
            ShoppingCartManager spCart = new ShoppingCartManager();
            return spCart.GetACartItem(item_id);
        }

        public void UpdateShopOrderData(string order_id, string cust_ref, string remarks)
        {
            ShoppingCartManager spCart = new ShoppingCartManager();
            spCart.UpdateShopOrderData(order_id, cust_ref, remarks);
        }

        public string CheckCustomItemExists(ShoppingCartManager shoppingCart)
        {
            return shoppingCart.CheckCustomItemExists();
        }

        public void UpdateToCustomItem(ShoppingCartManager shoppingCart)
        {
            shoppingCart.UpdateToCustomItem();
        }
        public void UpdateCustomItem(ShoppingCartManager shoppingCart)
        {
            shoppingCart.UpdateCustomItem();
        }
        public void ReduceStandardItemQuanity(ShoppingCartManager shoppingCart)
        {
            shoppingCart.ReduceStandardItemQuanity();
        }
        public void ConvertItemToCustomized(ShoppingCartManager shoppingCart)
        {
            shoppingCart.ConvertItemToCustomized();
        }
        public void CreateNewCustomizedItem(ShoppingCartManager shoppingCart)
        {
            shoppingCart.CreateNewCustomizedItem();
        }


        public void ConfirmOrder(ShoppingCartManager shoppingCart,string status)
        {
            shoppingCart.CofirmOrder(status);
        }

         public bool OrderConfirmationMailToAdmin(EmailInfo eInfo, string adminMail, string message)
        {
            return BusinessObjectManager.OrderConfirmationToAdmin( eInfo,  adminMail,  message);
        }

        public bool OrderFinalizeMailToCustomer(EmailInfo eInfo, string adminMail, string message)
        {
            return BusinessObjectManager.OrderFinalizeToCustomer(eInfo, adminMail, message);
        }

         public bool OrderConfirmationToCustomer(EmailInfo eInfo, string adminMail, string message)
         {
             return BusinessObjectManager.OrderConfirmationToCustomer(eInfo, adminMail, message);
         }

        public DataTable GetCountryList()
        {
            return BusinessObjectManager.GetCountryList();
        }

        public int UpdateOrderforCollect(WebOrders wsOrder)
        {
            return BusinessObjectManager.UpdateOrderforCollect(wsOrder);
        }

        public int UpdateOrderforDelivery(WebOrders wsOrder)
        {
            return BusinessObjectManager.UpdateOrderforDelivery(wsOrder);
        }

        public int UpdateWSOrder(WebOrders wsOrder)
        {
            return BusinessObjectManager.UpdateWSOrder(wsOrder);
        }

        public int TotalItemofOrder(int OrderID)
        {
            return BusinessObjectManager.TotalItemofOrder(OrderID);
        }

        public int InsertEDM(EDM edm)
        {
            return BusinessObjectManager.InsertEDM(edm);
        }

        public int InsertBaseMaterial(BaseMaterial bm)
        {
            return BusinessObjectManager.InsertBaseMaterial(bm);
        }

        public DataTable GetAllItemForOrder(int OrderId)
        {
            return BusinessObjectManager.GetAllItemForOrder(OrderId);
        }

        public DataTable GetAllCustomers()
        {
            return BusinessObjectManager.GetAllCustomers();
        }

        public int UpdateAdditionalInfo(WebOrders wsOrder)
        {
            return BusinessObjectManager.UpdateAdditionalInfo(wsOrder);
        }

        public string GetKLSPCode()
        {
            return BusinessObjectManager.GetKLSPCode();
        }

        public string GetEDMCode(string OCode)
        {
            return BusinessObjectManager.GetEDMCode(OCode);
        }

    }

}
