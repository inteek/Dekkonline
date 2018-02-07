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
        public bool addToDelivery(bool deliveryType, string idUser, int idWorkshop, int? idServiceWorkshop, int? idAppointmentsWorkshop, DateTime date, string time, string comments)
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
                        //addDelivery.IdAppointmentsWorkshop = idAppointmentsWorkshop;
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
                        //addDelivery.IdAppointmentsWorkshop = null;
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

        //DE-8 2 cambios
        public bool addToPurchaseOrder(string idUser, int tar, string cn, string edm, string edy, int sc, string chn)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    DbContextTransaction transaction = db.Database.BeginTransaction();
                    try
                    {

                        var idDelivery = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).FirstOrDefault();
                        var Address = db.UserAddress.Where(s => s.IdUser == idUser).OrderByDescending(s => s.Id).FirstOrDefault();
                        var product = db.ShoppingCart.Where(s => s.IdUser == idUser && s.Status == false).ToList();
                        var Procmocode = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).FirstOrDefault();
                        var prom = (dynamic)null;
                        var total = (dynamic)null;
                        var addTarget = new Entity.Payment();
                        if (tar == 1)
                        {
                          cn =  cn.Substring(cn.Length - 4);
                            addTarget.Number = Convert.ToInt32(cn);
                            addTarget.Expire = edm + "/" + edy;
                            addTarget.TargetType = "vpps";
                            addTarget.idUser = idUser;
                            db.Payment.Add(addTarget);
                            db.SaveChanges();
                        }
                        else
                        {
                           cn = cn.Substring(cn.Length - 4);
                            addTarget.Number = Convert.ToInt32(cn);
                            addTarget.Expire = edm + "/" + edy;
                            addTarget.TargetType = "Mastercard/Visa";
                            addTarget.idUser = idUser;
                            db.Payment.Add(addTarget);
                            db.SaveChanges();
                        }
                        if (Procmocode != null)
                        {
                            prom = Procmocode.PromoCode;
                            total = Procmocode.TotalPriceFinal;
                        }
                        else
                        {
                            prom = "No promo";
                            total = product.Select(p => p.Price).Sum();
                        }
                        var payment = db.Payment.Where(s => s.idUser == idUser).OrderByDescending(s => s.id).FirstOrDefault();
                        var addOrder = new Entity.Orders();
                        addOrder.idUser = idUser;
                        addOrder.Payment = payment.id;
                        addOrder.DeliveryAddress = idDelivery.IdDelivery;
                        addOrder.PromoCode = prom;
                        addOrder.Total = total;
                        addOrder.DateS = DateTime.Now;
                        db.Orders.Add(addOrder);
                        db.SaveChanges();
                        var addOrderdetail = new Entity.OrdersDetail();
                        var ordermain = db.Orders.Where(s => s.idUser == idUser).OrderByDescending(s => s.id).FirstOrDefault();
                        foreach (var item in product)
                        {
                            addOrderdetail.proId = item.proId;
                            addOrderdetail.quantity = item.quantity;
                            addOrderdetail.price = item.Price;
                            addOrderdetail.OrderMain = ordermain.id;
                            item.Status = true;
                            if (Procmocode != null)
                            {
                            Procmocode.Used = true;
                            }
                            db.OrdersDetail.Add(addOrderdetail);
                            db.SaveChanges();

                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {

                        transaction.Rollback();
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

        ////ORDER CONFIRMATION DE-20 TASK 1
        //public List<ResultProductsConfirmation> ObtainProductsConfirmed(string idUser)
        //{
        //    List<ResultProductsConfirmation> AllProducts = new List<ResultProductsConfirmation>();
        //    List<ResultProductsConfirmation> EachProductDetail = (dynamic)null;
        //    try
        //    {
        //        using (var db = new dekkOnlineEntities())
        //        {
        //            var OrderInfo = (from pro in db.PurchaseOrder
        //                             join del in db.DeliveryType on pro.IdDelivery equals del.IdDelivery
        //                             where pro.IdUser == idUser
        //                             orderby pro.IdOrderDetail descending
        //                             select new { ProductsOrder = pro.Products, ShoppingCartItems = pro.Shoppingcarts, TotalPrice1 = pro.TotalPrice, IdOrder = pro.IdOrderDetail }
        //              ).FirstOrDefault();

        //            string[] shoppingcart;
        //            shoppingcart = OrderInfo.ShoppingCartItems.Split(',');
        //            foreach (var item in shoppingcart)
        //            {
        //                var prodid = db.ShoppingCart.Where(s => s.Id.ToString() == item).Select(s => s.proId).FirstOrDefault();
        //                EachProductDetail = (from prod in db.products
        //                                     join shpro in db.ShoppingCart on prod.proId equals shpro.proId
        //                                     where shpro.IdUser == idUser && shpro.Status == false && shpro.Id.ToString() == item
        //                                     orderby shpro.Id descending
        //                                     select new ResultProductsConfirmation
        //                                     {
        //                                         IdOrderDetail = OrderInfo.IdOrder,
        //                                         ProductImage = prod.proImage,
        //                                         ProductName = prod.proName,
        //                                         Price = shpro.Price,
        //                                         Quantity = shpro.quantity,
        //                                         TotalPrice1 = OrderInfo.TotalPrice1
        //                                     }).ToList();
        //                if (EachProductDetail != null)
        //                {
        //                    AllProducts.AddRange(EachProductDetail);
        //                }

        //            }
        //        }
        //        return AllProducts;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //ORDER CONFIRMATION DE-20 TASK 1

        //ORDER CONFIRMATION DE-20 TASK 1YY
        public List<ResultProductsConfirmation> ObtainProductsConfirmed(string idUser)
        {
            List<ResultProductsConfirmation> AddressWorkshop = (dynamic)null;
            List<ResultShoppingCartProduct> products = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    products = (from pro in db.products
                                join cart in db.ShoppingCart on pro.proId equals cart.proId
                                where cart.IdUser.Equals(idUser) && cart.Status == false
                                select new ResultShoppingCartProduct
                                {
                                    IdUser = cart.IdUser,
                                    proId = cart.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = cart.quantity,
                                    totalpriceprod = Math.Round((double)cart.Price, 2),
                                    cartid = cart.Id.ToString()
                                }).ToList();
                    var promocodeused2 = (from promo in db.PromotionCode
                                          join promou in db.PromoCodeUsed on promo.IdCode equals promou.PromoCode
                                          where (promo.DateEnd < DateTime.Now && promou.Used == false && promou.idUser == idUser)
                                          select new { promo.IdCode }).FirstOrDefault();
                    if (promocodeused2 != null)
                    {
                        var promo1 = db.PromoCodeUsed.Where(s => s.PromoCode == promocodeused2.IdCode && s.idUser == idUser).FirstOrDefault();
                        db.PromoCodeUsed.Remove(promo1);
                        db.SaveChanges();
                    }
                    var promocodeused = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).FirstOrDefault();
                    var userAddress = (from us in db.AspNetUsers
                                       join us2 in db.UserAddress on us.Id equals us2.IdUser
                                       where us.Id == idUser
                                       select new
                                       {
                                           ZipCode = us2.ZipCode.ToString(),
                                           FirstName = us2.FirstName,
                                           LastName = us2.LastName,
                                           Address = us2.Address,
                                           Email = us.Email,
                                           Mobile = us2.Phone,
                                       }).FirstOrDefault();
                    double? Total2 = products.Select(p => p.totalpriceprod).Sum();
                    var Delivery = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).FirstOrDefault();
                    var workshop = db.Workshop.Where(s => s.IdWorkshop == Delivery.IdWorkshop).FirstOrDefault();
                    if (promocodeused == null)
                    {
                        if (Delivery.DeliveryType1 == false)
                        {
                            var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                            if (Delivery.IdAppointments == 0)
                            {
                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = null,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = Delivery.Time,
                                    Comments = Delivery.Comments,
                                   Total = (decimal)Total2}
                                };
                            }
                            else
                            {
                                var appointmentpre = db.Appointments.Where(s => s.IdAppointment == appointment.IdAppointment).FirstOrDefault();
                                DateTime dat = (DateTime)appointmentpre.Schedule;
                                DateTime dat2 = (DateTime)appointmentpre.Schedule;
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = null,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = dat2.ToString("t"),
                                    Comments = Delivery.Comments,
                                   Total = (decimal)Total2}
                                };
                            }

                        }
                        else
                        {
                            DateTime dat = (DateTime)Delivery.Date;
                            AddressWorkshop = new List<ResultProductsConfirmation>()
                        {
                           new ResultProductsConfirmation{ cart = products,
                            ZipCode = userAddress.ZipCode.ToString(),
                            FirstName = userAddress.FirstName,
                            LastName = userAddress.LastName,
                            Address = userAddress.Address,
                            Email = userAddress.Email,
                            Mobile = userAddress.Mobile,
                            Promo = null,
                            WorkshopName = null,
                            WorkshopAddress = Delivery.Address,
                            Image = null,
                            Rating = "as",
                            Date = dat.ToString("D"),
                            Time = Delivery.Time,
                            Comments = Delivery.Comments,
                           Total = (decimal)Total2 }
                        };
                        }
                    }////promocode null
                    else
                    {
                        var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                        if (Delivery.DeliveryType1 == false)
                        {
                            if (Delivery.IdAppointments == 0)
                            {
                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = promocodeused.PromoCode,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = Delivery.Time,
                                    Comments = Delivery.Comments,
                                   Total = promocodeused.TotalPriceFinal}
                                };
                            }
                            else
                            {
                                var appointmentpre = db.Appointments.Where(s => s.IdAppointment == appointment.IdAppointment).FirstOrDefault();
                                DateTime dat = (DateTime)appointmentpre.Schedule;
                                DateTime dat2 = (DateTime)appointmentpre.Schedule;
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = promocodeused.PromoCode,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = dat2.ToString("t"),
                                    Comments = Delivery.Comments,
                                   Total = promocodeused.TotalPriceFinal}
                                };
                            }
                        }
                        else
                        {
                            DateTime dat = (DateTime)Delivery.Date;
                            AddressWorkshop = new List<ResultProductsConfirmation>()
                        {
                           new ResultProductsConfirmation{ cart = products,
                            ZipCode = userAddress.ZipCode.ToString(),
                            FirstName = userAddress.FirstName,
                            LastName = userAddress.LastName,
                            Address = userAddress.Address,
                            Email = userAddress.Email,
                            Mobile = userAddress.Mobile,
                            Promo = promocodeused.PromoCode,
                            WorkshopName = null,
                            WorkshopAddress = Delivery.Address,
                            Image =null,
                            Rating = "as",
                            Date = dat.ToString("D"),
                            Time = Delivery.Time,
                            Comments = Delivery.Comments,
                           Total = promocodeused.TotalPriceFinal }
                        };
                        }
                    }
                }
                return AddressWorkshop;
            }
            catch (Exception ex)
            {

                throw;
            }

        }



        public List<ResultPaidProducts> ObtainProductsPaid(string idUser)
        {
            List<ResultPaidProducts> AddressWorkshop = (dynamic)null;
            List<ResultShoppingCartProduct> products = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var order = db.Orders.Where(s => s.idUser == idUser).OrderByDescending(s => s.id).Select(s => s.id).FirstOrDefault();
                    products = (from pro in db.products
                                join cart in db.OrdersDetail on pro.proId equals cart.proId
                                join or in db.Orders on cart.OrderMain equals or.id
                                where or.idUser.Equals(idUser) && or.id == order
                                select new ResultShoppingCartProduct
                                {
                                    IdUser = or.idUser,
                                    proId = cart.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = cart.quantity,
                                    totalpriceprod = Math.Round((double)cart.price, 2),
                                    cartid = cart.id.ToString()
                                }).ToList();
                    var promocodeused = (from prom in db.PromoCodeUsed
                                         join or in db.Orders on prom.PromoCode equals or.PromoCode
                                         where (prom.idUser == idUser && prom.Used == true && or.id == order)
                                         select new { prom.PromoCode,
                                         prom.TotalPriceFinal}).FirstOrDefault();
                    var userAddress = (from us in db.AspNetUsers
                                       join us2 in db.UserAddress on us.Id equals us2.IdUser
                                       where us.Id == idUser
                                       select new
                                       {
                                           ZipCode = us2.ZipCode.ToString(),
                                           FirstName = us2.FirstName,
                                           LastName = us2.LastName,
                                           Address = us2.Address,
                                           Email = us.Email,
                                           Mobile = us2.Phone,
                                       }).FirstOrDefault();
                    double? Total2 = products.Select(p => p.totalpriceprod).Sum();
                    var Delivery = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).FirstOrDefault();
                    var workshop = db.Workshop.Where(s => s.IdWorkshop == Delivery.IdWorkshop).FirstOrDefault();
                    var payment = db.Payment.Where(s => s.idUser == idUser).OrderByDescending(s => s.id).FirstOrDefault();
                    if (promocodeused == null)
                    {
                        if (Delivery.DeliveryType1 == false)
                        {
                            var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                            if (Delivery.IdAppointments == 0)
                            {
                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = null,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = Delivery.Time,
                                    Comments = Delivery.Comments,
                                   Total = (decimal)Total2,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order}
                                  
                                };
                            }
                            else
                            {
                                var appointmentpre = db.Appointments.Where(s => s.IdAppointment == appointment.IdAppointment).FirstOrDefault();
                                DateTime dat = (DateTime)appointmentpre.Schedule;
                                DateTime dat2 = (DateTime)appointmentpre.Schedule;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = null,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = dat2.ToString("t"),
                                    Comments = Delivery.Comments,
                                   Total = (decimal)Total2,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order}
                                };
                            }

                        }
                        else
                        {
                            DateTime dat = (DateTime)Delivery.Date;
                            AddressWorkshop = new List<ResultPaidProducts>()
                        {
                           new ResultPaidProducts{ cart = products,
                            ZipCode = userAddress.ZipCode.ToString(),
                            FirstName = userAddress.FirstName,
                            LastName = userAddress.LastName,
                            Address = userAddress.Address,
                            Email = userAddress.Email,
                            Mobile = userAddress.Mobile,
                            Promo = null,
                            WorkshopName =null,
                            WorkshopAddress = Delivery.Address,
                            Image = null,
                            Rating = "as",
                            Date = dat.ToString("D"),
                            Time = Delivery.Time,
                            Comments = Delivery.Comments,
                           Total = (decimal)Total2,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order}
                        };
                        }
                    }////promocode null
                    else
                    {
                        var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                        if (Delivery.DeliveryType1 == false)
                        {
                            if (Delivery.IdAppointments == 0)
                            {
                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = promocodeused.PromoCode,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = Delivery.Time,
                                    Comments = Delivery.Comments,
                                   Total = promocodeused.TotalPriceFinal,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order}
                                };
                            }
                            else
                            {
                                var appointmentpre = db.Appointments.Where(s => s.IdAppointment == appointment.IdAppointment).FirstOrDefault();
                                DateTime dat = (DateTime)appointmentpre.Schedule;
                                DateTime dat2 = (DateTime)appointmentpre.Schedule;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                    ZipCode = userAddress.ZipCode.ToString(),
                                    FirstName = userAddress.FirstName,
                                    LastName = userAddress.LastName,
                                    Address = userAddress.Address,
                                    Email = userAddress.Email,
                                    Mobile = userAddress.Mobile,
                                    Promo = promocodeused.PromoCode,
                                    WorkshopName = workshop.Name,
                                    WorkshopAddress = workshop.Address,
                                    Image = workshop.WorkImage,
                                    Rating = "as",
                                    Date = dat.ToString("D"),
                                    Time = dat2.ToString("t"),
                                    Comments = Delivery.Comments,
                                   Total = promocodeused.TotalPriceFinal,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order}
                                };
                            }
                        }
                        else
                        {
                            DateTime dat = (DateTime)Delivery.Date;
                            AddressWorkshop = new List<ResultPaidProducts>()
                        {
                           new ResultPaidProducts{ cart = products,
                            ZipCode = userAddress.ZipCode.ToString(),
                            FirstName = userAddress.FirstName,
                            LastName = userAddress.LastName,
                            Address = userAddress.Address,
                            Email = userAddress.Email,
                            Mobile = userAddress.Mobile,
                            Promo = promocodeused.PromoCode,
                            WorkshopName = null,
                            WorkshopAddress = Delivery.Address,
                            Image = null,
                            Rating = "as",
                            Date = dat.ToString("D"),
                            Time = Delivery.Time,
                            Comments = Delivery.Comments,
                           Total = promocodeused.TotalPriceFinal,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order}
                        };
                        }
                    }
                }
                return AddressWorkshop;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


    }
}
