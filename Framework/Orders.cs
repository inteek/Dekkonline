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
        public bool addToPurchaseOrder(string idUser, string products, decimal totalPrice, string paymentmethod, DateTime orderDate, bool oderStatus, DateTime deliveredDate, string usedPromo, string comments)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var idDelivery = (from d in db.DeliveryType
                              where (d.IdUser == idUser)
                              orderby d.IdUser descending
                              select d.IdDelivery);

                    if (idDelivery != null)
                    {
                        var addPurchaseOrder = new Entity.PurchaseOrder();
                        addPurchaseOrder.IdUser = idUser;
                        addPurchaseOrder.Products = products;
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
                         string[] Products;
                        Products = products.Split(',');
                        foreach (var item in Products)
                        {
                            var productquantity = (from a in db.ShoppingCart
                                                   where a.proId.ToString() == item && a.IdUser == idUser
                                                   orderby a.Id descending
                                                   select new { quantity1 = a.quantity }).FirstOrDefault();
                            var StockProductFinal = db.products.Where(s => s.proId.ToString() == item).FirstOrDefault();
                            var productcartchange = (from a in db.ShoppingCart
                                                     where a.proId.ToString() == item && a.IdUser == idUser && a.Status == true
                                                     orderby a.Id descending select a).FirstOrDefault();
                            StockProductFinal.proInventory = StockProductFinal.proInventory - productquantity.quantity1;
                            db.products.Add(StockProductFinal);
                            productcartchange.Status = false;
                            db.ShoppingCart.Add(productcartchange);
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
            List<ResultPurchaseOrder> AllProducts = null;
            List<ResultPurchaseOrder> orders = null;
            var EachProductDetail = (dynamic)null;
            string[] product = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    orders = (from or in db.PurchaseOrder
                              where or.IdUser == idUser && or.Orderstatus == false
                              select new ResultPurchaseOrder
                              {
                                  IdOrderDetail = or.IdOrderDetail,
                                  Products = or.Products,
                                  TotalPrice = or.TotalPrice,
                                  OrderDate = or.OrderDate,
                                  Orderstatus = or.Orderstatus,
                                  DeliveredDate = or.DeliveredDate,
                              }).ToList();

                    foreach (var item in orders)
                    {
                        product = item.Products.Split(',');

                        foreach (var item2 in product)
                        {
                            EachProductDetail = (from prod in db.products
                                                     join shpro in db.ShoppingCart on prod.proId equals shpro.proId
                                                     where shpro.IdUser == idUser && shpro.Status == true && shpro.proId.ToString() == item2
                                                     orderby shpro.Id descending
                                                     select new ResultPurchaseOrder
                                                     {
                                                         ProductImage = prod.proImage,
                                                         IdOrderDetail = item.IdOrderDetail,
                                                         ProductName = prod.proName,
                                                         Price = shpro.Price,
                                                         Quantity = shpro.quantity,
                                                         TotalPrice1 = item.TotalPrice1
                                                     });

                            if (EachProductDetail != null)
                            {
                                AllProducts.AddRange(EachProductDetail);
                            }

                        }
                    }

                    

                }


            }
            catch (Exception ex)
            {
                throw;
            }
            return orders;
        }

        //ORDER CONFIRMATION DE-20 TASK 1
        public List<ResultProductsConfirmation> ObtainProductsConfirmed(string idUser)
        {
            List<ResultProductsConfirmation> AllProducts = null;
            var EachProductDetail = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var OrderInfo = (from pro in db.PurchaseOrder
                                     join del in db.DeliveryType on pro.IdDelivery equals del.IdDelivery
                                     where pro.IdUser == idUser
                                     orderby pro.IdOrderDetail descending
                                     select new { ProductsOrder = pro.Products, TotalPrice1 = pro.TotalPrice, IdOrder = pro.IdOrderDetail }
                      ).FirstOrDefault();

                    string[] Products;
                    Products = OrderInfo.ProductsOrder.Split(',');
                    foreach (var item in Products)
                    {
                        EachProductDetail = (from prod in db.products
                                             join shpro in db.ShoppingCart on prod.proId equals shpro.proId
                                             where shpro.IdUser == idUser && shpro.Status == true && shpro.proId.ToString() == item
                                             orderby shpro.Id descending
                                             select new ResultProductsConfirmation
                                             {
                                                 IdOrderDetail = OrderInfo.IdOrder,
                                                 ProductImage = prod.proImage,
                                                 ProductName = prod.proName,
                                                 Price = shpro.Price,
                                                 Quantity = shpro.quantity,
                                                 TotalPrice1 = OrderInfo.TotalPrice1
                                             }).FirstOrDefault();
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
                                    select new {Deliverytype = del.DeliveryType1, Appointmentworkshop = del.IdAppointmentsWorkshop}
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
                              ).FirstOrDefault();
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
                            ).FirstOrDefault();
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
                            ).FirstOrDefault();
                        return OrderConf;
                    }


                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
