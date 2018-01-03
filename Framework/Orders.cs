using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Framework.Libraies;
using System.Data.Entity;

namespace Framework
{
    public class Orders
    {
        public bool addToDelivery(bool deliveryType, string idUser, int idWorkshop, int idServiceWorkshop, int idAppointmentsWorkshop, DateTime date, string time, string comments)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    if (idAppointmentsWorkshop != null)
                    {
                        var addDelivery = new Entity.DeliveryType();
                        addDelivery.DeliveryType1 = deliveryType;
                        addDelivery.IdUser = idUser;
                        addDelivery.IdWorkshop = idWorkshop;
                        addDelivery.IdServiceWorkshop = idServiceWorkshop;
                        addDelivery.IdAppointmentsWorkshop = idAppointmentsWorkshop;
                        addDelivery.Date = null;
                        addDelivery.Time = null;
                        addDelivery.Comments = null;
                        db.DeliveryType.Add(addDelivery);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        var addDelivery = new Entity.DeliveryType();
                        addDelivery.DeliveryType1 = deliveryType;
                        addDelivery.IdUser = idUser;
                        addDelivery.IdWorkshop = idWorkshop;
                        addDelivery.IdServiceWorkshop = idServiceWorkshop;
                        addDelivery.IdAppointmentsWorkshop = null;
                        addDelivery.Date = date;
                        addDelivery.Time = time;
                        addDelivery.Comments = comments;
                        db.DeliveryType.Add(addDelivery);
                        db.SaveChanges();
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                //_Error = ex;
                return false;
            }
        }

        //DE-8 2
        public bool addToPurchaseOrder(string idUser, string products, string carts, decimal totalPrice, string paymentmethod, DateTime orderDate, bool oderStatus, DateTime? deliveredDate, string usedPromo, string comments)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var idDelivery = (from d in db.DeliveryType
                                      where (d.IdUser == idUser)
                                      orderby d.IdUser descending
                                      select d.IdDelivery).FirstOrDefault(); ;

                    if (idDelivery != null)
                    {
                        var addPurchaseOrder = new Entity.PurchaseOrder();
                        addPurchaseOrder.IdUser = idUser;
                        addPurchaseOrder.Products = products;
                        addPurchaseOrder.Shoppingcarts = carts;
                        addPurchaseOrder.TotalPrice = totalPrice;
                        addPurchaseOrder.Paymentmethod = paymentmethod;
                        addPurchaseOrder.OrderDate = orderDate;
                        addPurchaseOrder.Orderstatus = oderStatus;
                        addPurchaseOrder.IdDelivery = Convert.ToInt32(idDelivery);
                        addPurchaseOrder.DeliveredDate = deliveredDate;
                        addPurchaseOrder.UsedPromo = usedPromo;
                        addPurchaseOrder.Comments = comments;

                        db.PurchaseOrder.Add(addPurchaseOrder);
                        db.SaveChanges();
                        string[] shoppingcartid;
                        shoppingcartid = products.Split(',');
                        foreach (var item in shoppingcartid)
                        {
                            var prodid = db.ShoppingCart.Where(s => s.Id.ToString() == item).Select(s => s.proId).FirstOrDefault();
                            var productquantity = (from a in db.ShoppingCart
                                                   where a.proId == prodid && a.IdUser == idUser
                                                   orderby a.Id descending
                                                   select new { quantity1 = a.quantity }).FirstOrDefault();
                            var StockProductFinal = db.products.Where(s => s.proId == prodid).FirstOrDefault();
                            var productcartchange = (from a in db.ShoppingCart
                                                     where a.proId == prodid && a.IdUser == idUser && a.Status == true
                                                     orderby a.Id descending
                                                     select a).FirstOrDefault();
                            StockProductFinal.proInventory = StockProductFinal.proInventory - productquantity.quantity1;
                            db.Entry(StockProductFinal).State = EntityState.Modified;
                            productcartchange.Status = false;
                            db.Entry(productcartchange).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                //_Error = ex;
                return false;
            }
        }

        //DE-25 1
        public List<ResultPurchaseOrder> loadOrderPending(string idUser)
        {
            List<ResultPurchaseOrder> AllProducts = new List<ResultPurchaseOrder>();
            List<ResultPurchaseOrder> orders = null;
            List<ResultPurchaseOrder> EachProductDetail = (dynamic)null;
            string[] product = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    orders = (from or in db.PurchaseOrder
                              where or.IdUser == idUser && or.Orderstatus == false
                              orderby or.IdOrderDetail descending
                              select new ResultPurchaseOrder
                              {
                                  IdOrderDetail = or.IdOrderDetail,
                                  Products = or.Products,
                                  TotalPrice = or.TotalPrice,
                                  OrderDate = or.OrderDate,
                                  Orderstatus = or.Orderstatus,
                                  DeliveredDate = or.DeliveredDate,
                                  ShoppingCarts = or.Shoppingcarts
                              }).ToList();

                    foreach (var item in orders)
                    {
                        product = item.ShoppingCarts.Split(',');

                        foreach (var item2 in product)
                        {
                            EachProductDetail = (from prod in db.products
                                                 join shpro in db.ShoppingCart on prod.proId equals shpro.proId
                                                 where shpro.IdUser == idUser && shpro.Status == false && shpro.Id.ToString() == item2
                                                 orderby shpro.Id descending
                                                 select new ResultPurchaseOrder
                                                 {
                                                     ProductImage = prod.proImage,
                                                     IdOrderDetail = item.IdOrderDetail,
                                                     ProductName = prod.proName,
                                                     Price = shpro.Price,
                                                     Quantity = shpro.quantity,
                                                     TotalPrice1 = item.TotalPrice
                                                 }).ToList();

                            if (EachProductDetail != null)
                            {
                                AllProducts.AddRange(EachProductDetail);
                            }

                        }
                    }
                }
                return AllProducts;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //DE-29 1
        public List<ResultPurchaseOrder> loadOrderPast(string idUser)
        {
            List<ResultPurchaseOrder> AllProducts = new List<ResultPurchaseOrder>();
            List<ResultPurchaseOrder> orders = null;
            List<ResultPurchaseOrder> EachProductDetail = (dynamic)null;
            string[] product = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    orders = (from or in db.PurchaseOrder
                              where or.IdUser == idUser && or.Orderstatus == true
                              orderby or.IdOrderDetail descending
                              select new ResultPurchaseOrder
                              {
                                  IdOrderDetail = or.IdOrderDetail,
                                  Products = or.Products,
                                  TotalPrice = or.TotalPrice,
                                  OrderDate = or.OrderDate,
                                  Orderstatus = or.Orderstatus,
                                  DeliveredDate = or.DeliveredDate,
                                  ShoppingCarts = or.Shoppingcarts
                              }).ToList();
                    
                    foreach (var item in orders)
                    {
                        product = item.ShoppingCarts.Split(',');

                        foreach (var item2 in product)
                        {
                            EachProductDetail = (from prod in db.products
                                                 join shpro in db.ShoppingCart on prod.proId equals shpro.proId
                                                 where shpro.IdUser == idUser && shpro.Status == false && shpro.Id.ToString() == item2
                                                 orderby shpro.Id descending
                                                 select new ResultPurchaseOrder
                                                 {
                                                     ProductImage = prod.proImage,
                                                     IdOrderDetail = item.IdOrderDetail,
                                                     ProductName = prod.proName,
                                                     Price = shpro.Price,
                                                     Quantity = shpro.quantity,
                                                     DeliveredDate = item.DeliveredDate,
                                                     TotalPrice1 = item.TotalPrice
                                                 }).ToList();

                            if (EachProductDetail != null)
                            {
                                AllProducts.AddRange(EachProductDetail);
                            }

                        }
                    }
                }
                return AllProducts;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //ORDER CONFIRMATION DE-20 TASK 1
        public List<ResultProductsConfirmation> ObtainProductsConfirmed(string idUser)
        {
            List<ResultProductsConfirmation> AllProducts = new List<ResultProductsConfirmation>();
            List<ResultProductsConfirmation> EachProductDetail = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var OrderInfo = (from pro in db.PurchaseOrder
                                     join del in db.DeliveryType on pro.IdDelivery equals del.IdDelivery
                                     where pro.IdUser == idUser
                                     orderby pro.IdOrderDetail descending
                                     select new { ProductsOrder = pro.Products, ShoppingCartItems = pro.Shoppingcarts, TotalPrice1 = pro.TotalPrice, IdOrder = pro.IdOrderDetail }
                      ).FirstOrDefault();

                    string[] shoppingcart;
                    shoppingcart = OrderInfo.ShoppingCartItems.Split(',');
                    foreach (var item in shoppingcart)
                    {
                        var prodid = db.ShoppingCart.Where(s => s.Id.ToString() == item).Select(s => s.proId).FirstOrDefault();
                        EachProductDetail = (from prod in db.products
                                             join shpro in db.ShoppingCart on prod.proId equals shpro.proId
                                             where shpro.IdUser == idUser && shpro.Status == false && shpro.Id.ToString() == item
                                             orderby shpro.Id descending
                                             select new ResultProductsConfirmation
                                             {
                                                 IdOrderDetail = OrderInfo.IdOrder,
                                                 ProductImage = prod.proImage,
                                                 ProductName = prod.proName,
                                                 Price = shpro.Price,
                                                 Quantity = shpro.quantity,
                                                 TotalPrice1 = OrderInfo.TotalPrice1
                                             }).ToList();
                        if (EachProductDetail != null)
                        {
                            AllProducts.AddRange(EachProductDetail);
                        }

                    }
                }
                return AllProducts;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //ORDER CONFIRMATION DE-20 TASK 1
        public string ObtainConfirmation(string idUser)
        {
            var OrderConf = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var DeliveryType = (from pro in db.PurchaseOrder
                                        join del in db.DeliveryType on pro.IdDelivery equals del.IdDelivery
                                        where pro.IdUser == idUser
                                        orderby pro.IdOrderDetail descending
                                        select new { Deliverytype = del.DeliveryType1, Appointmentworkshop = del.IdAppointmentsWorkshop }
                                     ).FirstOrDefault();



                    if (DeliveryType.Deliverytype == false)
                    {
                        if (DeliveryType.Appointmentworkshop != null)
                        {


                            OrderConf = (from pro in db.PurchaseOrder
                                         join del in db.DeliveryType on pro.IdDelivery equals del.IdDelivery
                                         join user in db.UserAddress on del.IdUser equals user.IdUser
                                         join tUser in db.AspNetUsers on pro.IdUser equals tUser.Id
                                         join twork in db.Workshop on del.IdWorkshop equals twork.IdWorkshop
                                         where pro.IdUser == idUser
                                         orderby pro.IdOrderDetail descending
                                         select new
                                         {
                                             Idorderdetail = pro.IdOrderDetail,
                                             zipcode = user.ZipCode,
                                             Firstname = user.FirstName,
                                             Lastname = user.LastName,
                                             Adress = user.Address,
                                             Emailuser = tUser.Email,
                                             Phonenumber = user.Phone,
                                             DateWorkshop = del.Date,
                                             TimeWorkshop = del.Time,
                                             WorkshopComments = del.Comments,
                                             WorkshopName = twork.Name,
                                             WorkshopAdress = twork.Address,
                                             WorkshopLatitude = twork.Latitude,
                                             WorkshopLength = twork.Length
                                         }
                                  ).FirstOrDefault().ToString();
                            return OrderConf;
                        }
                        else
                        {
                            OrderConf = (from pro in db.PurchaseOrder
                                         join del in db.DeliveryType on pro.IdDelivery equals del.IdDelivery
                                         join user in db.UserAddress on del.IdUser equals user.IdUser
                                         join tUser in db.AspNetUsers on pro.IdUser equals tUser.Id
                                         join twork in db.Workshop on del.IdWorkshop equals twork.IdWorkshop
                                         join awork in db.WorkshopAppointment on del.IdAppointmentsWorkshop equals awork.Id
                                         where pro.IdUser == idUser
                                         orderby pro.IdOrderDetail descending
                                         select new
                                         {
                                             Idorderdetail = pro.IdOrderDetail,
                                             zipcode = user.ZipCode,
                                             Firstname = user.FirstName,
                                             Lastname = user.LastName,
                                             Adress = user.Address,
                                             Emailuser = tUser.Email,
                                             Phonenumber = user.Phone,
                                             DateWorkshop = awork.Date,
                                             TimeWorkshop = awork.Time,
                                             WorkshopComments = del.Comments,
                                             WorkshopName = twork.Name,
                                             WorkshopAdress = twork.Address,
                                             WorkshopLatitude = twork.Latitude,
                                             WorkshopLength = twork.Length
                                         }
                            ).FirstOrDefault().ToString();
                            return OrderConf;
                        }
                    }
                    else
                    {

                        OrderConf = (from pro in db.PurchaseOrder
                                     join del in db.DeliveryType on pro.IdDelivery equals del.IdDelivery
                                     join user in db.UserAddress on del.IdUser equals user.IdUser
                                     join tUser in db.AspNetUsers on pro.IdUser equals tUser.Id
                                     join twork in db.Workshop on del.IdWorkshop equals twork.IdWorkshop
                                     where pro.IdUser == idUser
                                     orderby pro.IdOrderDetail descending
                                     select new
                                     {
                                         Idorderdetail = pro.IdOrderDetail,
                                         zipcode = user.ZipCode,
                                         Firstname = user.FirstName,
                                         Lastname = user.LastName,
                                         Adress = user.Address,
                                         Emailuser = tUser.Email,
                                         Phonenumber = user.Phone,
                                         DateUser = del.Date,
                                         TimeUser = del.Time,
                                         UserComments = del.Comments,
                                         UserName = user.FirstName,
                                         UserAdress = user.Address,
                                         UserLatitude = user.Latitude,
                                         UserLength = user.Length
                                     }
                            ).FirstOrDefault().ToString();
                        return OrderConf;
                    }


                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
