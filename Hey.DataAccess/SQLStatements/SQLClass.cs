using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.DataAccess.SQLStatements
{
    public class SQLClass
    {
        #region Menu

        //public const string GET_GROUPS = @"SELECT  code,description,list_position,webshop from product_groups where webshop = 1 order by list_position ASC, description ASC ";

        public const string GET_GROUPS = @"SELECT mg1.code, mg1.description, substring(mg1.code,1,1) as parentitem,
                                           mg1.list_position, mg1.webshop
                                           from product_groups mg1, product_groups mg2 
                                           where mg1.code = mg2.code and mg1.webshop = 1 
                                           order by mg1.list_position ASC, mg1.description ASC ";

        #endregion

        #region Account/Login
        public const string GET_CUSTOMERINFO = @"SELECT * from h_customers_v 
                                                 WHERE LOWER(email)= LOWER(:email) 
                                                 AND webshop_pwd = :webshop_pwd 
                                                 AND webshop_role > 0 ";

        public const string GET_FORGETUSERPASSWORD = @" SELECT * from h_customers_v 
                                                        WHERE LOWER(email)= LOWER(:email)
                                                        AND webshop_role > 0";
        #endregion

        #region Filter Section
        public const string GET_NUMBEROFFIILTERCOMBO = @"SELECT * FROM product_filters WHERE UPPER(group_code) = :group_code 
                                                       ORDER BY list_position ASC";

        public const string LOAD_FILTERCOMBOS = @"SELECT DISTINCT @field_name FROM @table_name ORDER BY @field_name ";

        public const string FILTER_QUERY = @" SELECT matcode, description, unit, sellprice, purchaseprice FROM basematerial ";

        //string query = "SELECT SUBSTR(m.matcode, 1, 4) AS matgroup, g.description, COUNT(SUBSTR(m.matcode, 1, 4)) FROM basematerial m " + 
        //               "LEFT JOIN product_groups g ON SUBSTR(m.matcode, 1, 4) = g.code WHERE (m.keywords ~* '"+ input.ToLower() +
        //               "') AND m.webshop = 1 GROUP BY matgroup, g.description ORDER BY g.description";

        //string query = "SELECT matcode,substring(matcode,1,4) as subgroup, description, unit, sellprice  FROM basematerial WHERE LOWER(keywords) LIKE '%" +
        //    input.ToLower() + "%' AND webshop = 1 ";

        public const string SEARCHQUERY = @"SELECT SUBSTR(m.matcode, 1, 4) AS matgroup, g.description, COUNT(SUBSTR(m.matcode, 1, 4)) 
                                            FROM basematerial m 
                                            LEFT JOIN product_groups g 
                                            ON SUBSTR(m.matcode, 1, 4) = g.code 
                                            WHERE (m.keywords ~* :searchstring) 
                                            AND m.webshop = 1 GROUP BY matgroup, g.description ORDER BY g.description";

        public const string FILTERSEARCHQUERY = @"select matcode, description, unit, sellprice , purchaseprice
                                                  FROM basematerial 
                                                  WHERE (matcode ~* :searchstring AND webshop = 1 ) 
                                                  AND (matcode ~* :searchstring 
                                                  OR description ~* :searchstring 
                                                  OR unit ~* :searchstring
                                                  OR to_char(sellprice, '9999.99') ~* :searchstring)";

        public const string ITEMGROUPDATAQUERY = @" SELECT matcode, description, unit, sellprice , purchaseprice
                                                    FROM basematerial 
                                                    WHERE matcode ~* :code 
                                                    AND webshop = 1 
                                                    ORDER BY matcode ";
        #endregion

        #region Make ShoppingCart

        public const string INSERT_INTO_WEB_ORDER_ITEMS = @"INSERT INTO ws_order_items(order_id,item_type,
                                                        item_code, derived_from, description, suppl_code, quantity, units,
                                                        price_cost, price_sell, price_total, discount, remarks, prodnotes,
                                                        to_edm, to_products) 
                                                        VALUES(:order_id,:item_type,:item_code,:derived_from,
                                                        :description,:suppl_code,:quantity,:units,:price_cost,:price_sell,
                                                        :price_total,:discount,:remarks,:prodnotes,:to_edm,:to_products)";

        public const string UPDATE_ITEM = @" UPDATE ws_order_items set 
                                            item_type = :item_type, 
                                            item_code = :item_code,
                                            description = :description, 
                                            quantity = :quantity, 
                                            units = :units,
                                            price_cost = :price_cost, 
                                            price_sell =:price_sell, 
                                            price_total = :price_total, 
                                            discount = :discount, 
                                            remarks = :remarks, 
                                            prodnotes = :prodnotes, 
                                            to_edm = :to_edm, 
                                            to_products = :to_products 
                                            WHERE order_id = :order_id 
                                            AND item_code ~* :item_code ";

        public const string INSERT_INTO_WEB_ORDER = @" INSERT INTO ws_orders(order_code,order_type,order_descr,order_date,order_status,cust_code,
                                                      cust_name,cust_contact,cust_phone,cust_email,cust_ref,delivery_date,delivery_address,
                                                      delivery_zipcode,delivery_city,delivery_country,shipping_method,shipping_cost,total_items,
                                                      discount_items,discount_other,total_vat,total_order,total_order_with_vat,remarks,prodnotes,
                                                      session_id,replicated)
                                                      VALUES(:order_code,:order_type,:order_descr,:order_date,:order_status,:cust_code,
                                                      :cust_name,:cust_contact,:cust_phone,:cust_email,:cust_ref,:delivery_date,:delivery_address,
                                                      :delivery_zipcode,:delivery_city,:delivery_country,:shipping_method,:shipping_cost,:total_items,
                                                      :discount_items,:discount_other,:total_vat,:total_order,:total_order_with_vat,:remarks,:prodnotes,
                                                      :session_id,:replicated)";

        public const string GET_DISCOUNT_FOR_CUSTOMER = @"select cust_code, prod_code, discount from  product_discount  where cust_code ~* :cust_code ";

        public const string IS_ORDER_EXIST = @" select * from ws_orders where session_id ~* :session_id AND order_status= '-' ";

        public const string IS_REG_ADM_ORDER_EXIST = @" select * from ws_orders where cust_code ~* :cust_code AND order_status= '-' ";

        public const string IS_ORDER_ITEM_EXIST = @" select * from ws_order_items where order_id = :order_id AND item_code ~* :item_code ";

        public const string GET_ALL_ITEM_FOR_ORDER = @" select * from ws_order_items where order_id = :order_id";



        public const string MAX_ORDER_ID = @"select MAX(order_id) as orderid from ws_orders ";

        #endregion

        #region Shopping Cart

        public const string SHOPPING_ITEMS_QUERY = @"SELECT order_id,item_id,item_code,description,remarks,quantity,price_sell,
                                                     price_total as Total,prodNotes  
                                                     FROM ws_order_items WHERE item_type = 'S' 
                                                     AND order_id = (SELECT order_id from ws_orders 
                                                                where order_status = '-' 
                                                                and session_id=:sessionid)";

        public const string SHOPPING_ITEMS_REG_QUERY = @"SELECT order_id,item_id,item_code,description,remarks,quantity,price_sell,
                                                     price_total as Total,prodNotes  
                                                     FROM ws_order_items WHERE item_type = 'S' 
                                                     AND order_id = (SELECT order_id from ws_orders 
                                                                where order_status = '-' 
                                                                and cust_code=:cust_code
                                                                 AND order_status = '-')";

        public const string SHOPPING_ITEMS_ADM_QUERY = @"SELECT order_id,item_id,item_code,description,remarks,quantity,price_sell,
                                                     price_total as Total,prodNotes  
                                                     FROM ws_order_items WHERE item_type = 'S' 
                                                     AND order_id = (SELECT order_id from ws_orders 
                                                                where cust_code=:cust_code
                                                                AND order_status = '-')";

        public const string SHOPPING_ITEMS_QUERY_SPECIAL = @"SELECT order_id,item_id,item_code,description,remarks,quantity,price_sell,
                                                             price_total as Total,prodNotes  
                                                             FROM ws_order_items 
                                                             WHERE item_type <> 'S' AND 
                                                             order_id = (SELECT order_id from ws_orders 
                                                                         where cust_code=:cust_code
                                                                         AND order_status = '-')";

        public const string SHOPPING_ITEMS_UPDATE = @"UPDATE  ws_order_items set quantity = :quantity,remarks = :remark,price_total = :quantity*price_sell
                                                      WHERE item_id = :item_id";
        public const string SHOPPING_ITEMS_DELETE = @"DELETE from ws_order_items WHERE item_id = :item_id";
        public const string SHOPPING_ITEMS_DELETE_ALL = @"DELETE from ws_orders WHERE order_id = :order_id";

//        public const string SHOPPING_ITEMS_QUERY_SPECIAL = @"SELECT order_id,item_id,item_code,description,remarks,quantity,price_sell,
//                                                             price_total as Total,prodNotes  
//                                                             FROM ws_order_items 
//                                                             WHERE item_type <> 'S' AND 
//                                                             order_id = (SELECT order_id from ws_orders 
//                                                                         where order_status = '-' 
//                                                                         and session_id=:sessionid)";
       
        public const string SHOPPING_ORDER = @"Select total_order, shipping_cost,discount_items as discount, discount_other as other_discount,remarks,cust_ref,
                                               total_vat,total_order_with_vat from ws_orders where order_id = :order_id";
        public const string SHOPPING_ITEM = "Select order_id,item_code,item_type,item_code,description,remarks,quantity,prodNotes,units, price_cost, price_sell ,to_edm,  to_products From ws_order_items where item_id = :item_id";
        public const string SHOPPING_ITEM_UPDATE = "update ws_order_items set remarks = :remarks ,quantity=:quantity,prodNotes=:prodNotes,price_total = :quantity*price_sell where item_id = :item_id";
        public const string SHOPPING_ORDER_UPDATE = "update ws_orders set cust_ref = :customer_ref, remarks = :remarks where order_id= :order_id";

        public const string CHECK_CUSTOM_ITEM_EXIST = "SELECT item_id from ws_order_items where order_id=:order_id and item_code=:item_code and item_type ~* 'C'";

        public const string UPDATE_TO_CUSTOM_ITEM = @"UPDATE ws_order_items set quantity = (quantity + :quantity),price_total = (quantity + :quantity) * price_sell where item_id=:item_id";

        public const string REDUCE_STANDARD_ITEM_QUNATITY = @"UPDATE ws_order_items set quantity = (quantity - :quantity),price_total = (quantity - :quantity) * price_sell  where item_id=:item_id";

        public const string INSERT_CUSTOM_ITEM = @"INSERT into ws_order_items(order_id,item_code,item_type,derived_from,description,suppl_code,quantity,units,price_cost,price_sell,
                                                    discount,remarks,prodnotes,to_edm,to_products)
                                                    Select order_id,item_code,'C',derived_from,:description,suppl_code,:quantity,:units,:price_cost,:price_sell,
                                                    discount,:remarks,:prodnotes, :to_edm,  :to_products
                                                    from ws_order_items where item_id =:item_id";
        public const string GET_ORDER_ITEM_LAST_ID = @"SELECT currval('ws_order_items_seq')";
//        public const string INSERT_CUSTOM_ITEM = @"INSERT into ws_order_items(order_id,item_code,item_type,derived_from,description,suppl_code,quantity,units,price_cost,price_sell,price_total)
//                                                    Select order_id,item_code,'C',derived_from,:description,suppl_code,:quantity,:units,:price_cost,:price_sell, :quantity * :price_sell
//                                                    from ws_order_items where item_id =:item_id";

        public const string CONVERT_TO_CUSTOM = @"update ws_order_items set item_type = 'C',
                                                  description = :description,price_cost = :price_cost, price_sell = :price_sell,
                                                  units = :units, remarks = :remarks,prodNotes=:prodNotes,quantity = :quantity,
                                                  to_edm = :to_edm,  to_products = :to_products
                                                  where item_id = :item_id";

        //        public const string CONVERT_TO_CUSTOM = @"update ws_order_items set item_type = 'C', 
        //                                                  description = :description,price_cost = :price_cost, price_sell = :price_sell,
        //                                                  units = :units, remarks = :remarks,prodNotes=:prodNotes,
        //                                                  to_edm = :to_edm,  to_products = :to_products
        //                                                  where item_id = :item_id";

        public const string UPDATE_CUSTOM_ITEM = @"update ws_order_items set description = :description,price_cost = :price_cost, price_sell = :price_sell,
                                                  units = :units, remarks = :remarks,prodNotes=:prodNotes,quantity = :quantity,
                                                  to_edm = :to_edm,  to_products = :to_products
                                                  where item_id = :item_id";
        public const string UPDATE_CUSTOM_ITEM_TOTAL_PRICE = "UPDATE ws_order_items set price_total = (quantity * price_sell ) where item_id = :item_id";

        public const string WSHOP_ORDER_UPDATE = @" UPDATE ws_orders set 
                                                   cust_code = :cust_code, 
                                                   session_id = :session_id, 
                                                   order_code = :order_code 
                                                   where order_id= :order_id";



        #endregion

        #region Shipping

        public const string COUNTRY_LIST = @" SELECT * FROM list_country WHERE list_show = 1 ORDER BY list_position";

        public const string UPDATE_ORDER_FOR_COLLECT = @" UPDATE ws_orders set 
                                            shipping_method = :shipping_method, 
                                            delivery_date = :delivery_date                                            
                                            WHERE order_id = :order_id ";

        public const string UPDATE_ORDER_FOR_Delivery = @" UPDATE ws_orders set 
                                            shipping_method = :shipping_method, 
                                            delivery_date = :delivery_date,
                                            cust_name = :cust_name, 
                                            cust_contact = :cust_contact,  
                                            delivery_address = :delivery_address, 
                                            delivery_zipcode = :delivery_zipcode,  
                                            delivery_city = :delivery_city, 
                                            delivery_country = :delivery_country 
                                                                                      
                                            WHERE order_id = :order_id ";

         
        #endregion

        #region Confirmation

        public const string ORDER_CONFIRMATION = @"Update ws_orders set 
                                                   order_status = :order_status,
                                                   session_id = ''
                                                   where order_id = :order_id";

        #endregion

        public const string INSERT_INTO_EDM = @"INSERT INTO edm_rc(edm_code,description,revision,
                                                cust_code,format,scale,first_published,last_published,
                                                remarks,author,last_edited,last_edited_by,deleted)
	                                            VALUES(:edm_code,:description,:revision,:cust_code,:format,
                                                :scale,:first_published,:last_published,:remarks,:author,
                                                :last_edited,:last_edited_by,:deleted)";

        public const string INSERT_INTO_BASEMATERIAL = @"INSERT INTO basematerial(matcode,description,orderdescription,suppcode,prjcode,edmcode,unit,packunit,purchaseprice,
			 discount1,discount2,freight,addon1,addon2,netprice,sellprice,sellperfect,orderdate,deliverydate,
			 instock,minstock,maxstock,minorder,stockloc,sparepart,webshop,remark,mergecode,derivedfrom,remarkeng,
			 keywords,filter1,filter2,filter3,filter4,filter5,new_suppcode,cust_code,matcode_suppl,deleted)
                  VALUES(:matcode,:description,:orderdescription,:suppcode,:prjcode,:edmcode,:unit,:packunit,:purchaseprice,
			 :discount1,:discount2,:freight,:addon1,:addon2,:netprice,:sellprice,:sellperfect,:orderdate,:deliverydate,
			 :instock,:minstock,:maxstock,:minorder,:stockloc,:sparepart,:webshop,:remark,:mergecode,:derivedfrom,:remarkeng,
			 :keywords,:filter1,:filter2,:filter3,:filter4,:filter5,:new_suppcode,:cust_code,:matcode_suppl,:deleted)";

        public const string TOTAL_ITEM_OF_AN_ORDER = @"select count(*) from ws_order_items where order_id = :order_id ";

        public const string GET_TOTAL_CUSTOMERS = @"SELECT rel_code, rel_name FROM h_customers_v ORDER BY rel_name";

        public const string UPDATE_ADDITIONAL_INFO = @"UPDATE ws_orders set 
                                                      order_code = :order_code,
                                                      order_descr = :order_descr,
                                                      total_order_with_vat  = :total_order_with_vat,
                                                      total_order = :total_order,
                                                      discount_other = (shipping_cost + total_items) - discount_items - :total_order,
                                                      prodnotes=:prodnotes
                                                      where order_id = :order_id";

        public const string GET_MAX_KLSP = @"SELECT ('KLSP' || TRIM(TO_CHAR(MAX(SUBSTR(matcode, 5))::integer + 1, '00000'))) AS new_code 
                                             FROM basematerial WHERE matcode LIKE 'KLSP%' AND ISNUMERIC(SUBSTR(matcode, 5))";

        public const string IS_ORDER_CODE_EXISTS = @"select order_code from ws_orders where order_code  ~* :order_code";

    }
}
