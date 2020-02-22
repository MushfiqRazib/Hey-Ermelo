using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.Common.Objects
{
    public enum UserType
    {
        ANONYMOUS,
        REGISTERED,
        ADMIN
    }
    public class HCustomers
    {
        public string RelCode {get;set;}
        public string OldCode {get;set;}
        public string CompanyType {get;set;}
        public string Remarks {get;set;}
        public string Suspect {get;set;}
        public string WebShop {get;set;}
        public string Reference {get;set;}
        public string Password {get;set;}
        public string Archive {get;set;}
        public string UserPrivilege { get; set; }
        public string RelName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string CellPhone { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public int WebShopRole { get; set; }

        public string DeliveryAddress { get; set; }
        public string DeliveryZipcode { get; set; }
        public string VisitAddress { get; set; }
        public string VisitZipcode { get; set; }

        public string CurrentShoppingCartItemNumber { get; set; }
    }
}
