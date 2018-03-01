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
      
        //DE-8 2 cambios
        public bool addToPurchaseOrder(string idUser, int tar, string cn, string edm, string edy, int sc, string chn)
        {
            var ivapor = System.Configuration.ConfigurationManager.AppSettings["ivapre"];
            int ivpre = Convert.ToInt32(ivapor);
            double ivapor2 = Convert.ToDouble((double)ivpre / 100);
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    DbContextTransaction transaction = db.Database.BeginTransaction();
                    try
                    {
                        List<ResultTypesServices> services1 = null;
                        var totaliva = (dynamic)null;
                        var idDelivery = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).FirstOrDefault();
                        var Address = db.UserAddress.Where(s => s.IdUser == idUser).OrderByDescending(s => s.Id).FirstOrDefault();
                        var product = db.ShoppingCart.Where(s => s.IdUser == idUser && s.Status == false).ToList();
                        var Procmocode = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).FirstOrDefault();
                        var prom = (dynamic)null;
                        var total = (dynamic)null;
                        var tax = 0;
                        var addTarget = new Entity.Payment();
                        if (tar == 1)
                        {
                            cn = cn.Substring(cn.Length - 4);
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
                            totaliva = Procmocode.TotalPriceFinal;
                        }
                        else
                        {
                            prom = "No promo";
                            total = product.Select(p => p.Price).Sum();
                            totaliva = total;
                            var iva = (double)totaliva * ivapor2;//TAX
                            totaliva = (double)totaliva + iva;
                            totaliva = (int)Math.Floor((decimal)totaliva);
                        }
                        var payment = db.Payment.Where(s => s.idUser == idUser).OrderByDescending(s => s.id).FirstOrDefault();
                        var addOrder = new Entity.Orders();
                        var deliverydate = (dynamic)null;
                        var deliverydate2 = (dynamic)null;
                        if (idDelivery.IdAppointments != 0 && idDelivery.IdAppointments != null)
                        {
                            deliverydate = (from iap in db.WorkshopAppointment
                                            where iap.IdWorkshop == idDelivery.IdWorkshop && iap.Id == idDelivery.IdAppointments
                                            select new { Date = iap.Date, iap.IdAppointment, iap.Time }).FirstOrDefault();
                        }
                        else if (idDelivery.IdAppointments == 0)
                        {
                            DateTime date2 = (DateTime)idDelivery.Date;
                            deliverydate2 = date2.ToString("d") + " " + idDelivery.Time;
                        }
                        if (deliverydate == null)
                        {
                            addOrder.EstimatedDate = Convert.ToDateTime(deliverydate2);
                        }
                        else
                        {
                            DateTime f = (DateTime)idDelivery.Date;

                            string a = f.ToString("d");
                            a = a + " " + idDelivery.Time;

                            //DateTime newDateTime = fecha.Add(TimeSpan.Parse(deliverydate.Time));
                            addOrder.EstimatedDate = Convert.ToDateTime(a);
                        }
                        //Obtener los servicios si es que contrato
                        services1 = (from wtps in db.WorkshopServices
                                     join ws in db.Workshop on wtps.IdWorkshop equals ws.IdWorkshop
                                     join tps in db.TypesServices on wtps.IdService equals tps.IdService
                                     join dps in db.DeliveryServices on wtps.Id equals dps.idService
                                     join dela in db.DeliveryType on dps.idDelivery equals dela.IdDelivery
                                     where dela.IdDelivery == idDelivery.IdDelivery
                                     select new ResultTypesServices
                                     {
                                         IdService = (int)wtps.Id,
                                         Name = tps.Name,
                                         WorkshopImage = ws.WorkImage,
                                         WorkshopName = ws.Name,
                                         Price = Math.Truncate((double)wtps.Price),
                                         TaxIva = Math.Truncate((double)wtps.Price * ivapor2)
                                     }
                            ).ToList();
                        //
                        //Obtener el total de los servicios con el iva agregado
                        double? totalserviceswithtax = 0;
                        double? totalservices = 0;
                        double? totalservicestax = 0;
                        if (services1.Count > 0 || services1 != null)
                        {
                             totalservices = services1.Select(p => (double?)p.Price).Sum();
                            totalservicestax = services1.Select(p => p.TaxIva).Sum();
                            totalservices = (int)Math.Floor((decimal)totalservices);
                            totalservicestax = (int)Math.Floor((decimal)totalservicestax);
                            totalserviceswithtax = (int)Math.Floor((decimal)totalservices + (decimal)totalservicestax);
                        }
                        //
                        //Agregar la compra a la tabla de orden
                        var totalfinal = 0;
                        if (Procmocode == null)
                        {
                            tax = (int)(ivapor2 * ((int)total + (int)totalservices));
                            totalfinal = (int)total + (int)totalservices;
                            totalfinal = totalfinal + (int)(ivapor2 * ((int)total + (int)totalservices));
                        }
                        else
                        {
                            var promocodeperc = (dynamic)null;
                                promocodeperc = db.PromotionCode.Where(s => s.IdCode == Procmocode.PromoCode).FirstOrDefault();

                            var promocodedisc = promocodeperc.PercentCode;
                            promocodedisc = promocodedisc / 100;
                            var nototalpromo = Procmocode.TotalPrice;
                            var promototal = (double)nototalpromo * (double)promocodedisc;
                            promototal = (int)Math.Floor((decimal)promototal);
                            nototalpromo = nototalpromo - (decimal)promototal;
                            nototalpromo = (int)Math.Floor((decimal)nototalpromo);

                            tax = (int)(ivapor2 * ((int)nototalpromo + (int)totalservices));
                            totalfinal = (int)nototalpromo + (int)totalservices;
                            var ivagood = ivapor2 * ((int)nototalpromo + (int)totalservices);
                            ivagood = (int)Math.Floor(ivagood);
                            totalfinal = totalfinal + (int)ivagood;
                        }


                        addOrder.idUser = idUser;
                        addOrder.Payment = payment.id;
                        addOrder.DeliveryAddress = idDelivery.IdDelivery;
                        addOrder.PromoCode = prom;
                        addOrder.Total = Convert.ToDecimal(totalfinal);
                        addOrder.DateS = DateTime.Now;
                        addOrder.Delivered = false;
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
                                Procmocode.Points = 10;
                            }
                            db.OrdersDetail.Add(addOrderdetail);
                            db.SaveChanges();

                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
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
        public List<ResultUserOrder> loadOrderPending(string idUser)
        {
            try
            {
                var products = (dynamic)null;
                List<ResultDataUser> userdata = null;
                using (var db = new dekkOnlineEntities())
                {
                    userdata = (from us in db.AspNetUsers
                                join us2 in db.UserAddress on us.Id equals us2.IdUser
                                where (us.Id == idUser)
                                select new ResultDataUser
                                {
                                    FirstName = us2.FirstName,
                                    LastName = us2.LastName,
                                    Email = us.Email,
                                    Image = us2.Image
                                }).ToList();
                    if (userdata.Count == 0)
                    {
                        userdata = (from us in db.AspNetUsers
                                    where us.Id == idUser
                                    select new ResultDataUser
                                    {
                                        Email = us.Email
                                    }).ToList();
                    }
                    products = (from pro in db.products
                                join ord in db.OrdersDetail on pro.proId equals ord.proId
                                join or in db.Orders on ord.OrderMain equals or.id
                                join de in db.DeliveryType on or.DeliveryAddress equals de.IdDelivery
                                where or.idUser.Equals(idUser) && or.Delivered == false && or.DeliveredDate == null
                                select new ResultOrderProductsUser
                                {
                                    IdUser = or.idUser,
                                    proId = ord.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = ord.quantity,
                                    totalpriceprod = Math.Truncate((double)ord.price),
                                    orders = or.id.ToString(),
                                    estimated1 = (DateTime)or.EstimatedDate,
                                    orderdte1 = (DateTime)or.DateS,
                                    idWorkShop = (int)de.IdWorkshop,
                                    Address = de.Address
                                }).ToList();
                    foreach (var item in products)
                    {
                        var dateE = item.estimated1;
                        dateE = dateE.ToString("D");
                        var dateO = item.orderdte1;
                        dateO = dateO.ToString("D");
                        item.estimated2 = dateE;
                        item.orderdte2 = dateO;
                    }

                    List<ResultUserOrder> order = new List<ResultUserOrder>()
                    {
                        new ResultUserOrder{
                                product = products,
                                user = userdata
                        }
                    };
                    return order;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //DE-29 1
        public List<ResultUserOrder> loadOrderPast(string idUser)
        {
            try
            {
                var products = (dynamic)null;
                List<ResultDataUser> userdata = null;
                using (var db = new dekkOnlineEntities())
                {
                    userdata = (from us in db.AspNetUsers
                                join us2 in db.UserAddress on us.Id equals us2.IdUser
                                where (us.Id == idUser)
                                select new ResultDataUser
                                {
                                    FirstName = us2.FirstName,
                                    LastName = us2.LastName,
                                    Email = us.Email,
                                    Image = us2.Image
                                }).ToList();
                    if (userdata.Count == 0)
                    {
                        userdata = (from us in db.AspNetUsers
                                where us.Id == idUser
                                select new ResultDataUser
                                {
                                    Email = us.Email
                                }).ToList();
                    }
                    products = (from pro in db.products
                                join ord in db.OrdersDetail on pro.proId equals ord.proId
                                join or in db.Orders on ord.OrderMain equals or.id
                                where or.idUser.Equals(idUser) && or.DeliveredDate != null && or.Delivered == true
                                select new ResultOrderProductsUser
                                {
                                    IdUser = or.idUser,
                                    proId = ord.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = ord.quantity,
                                    totalpriceprod = Math.Truncate((double)ord.price),
                                    orders = or.id.ToString(),
                                    estimated1 = (DateTime)or.EstimatedDate,
                                    orderdte1 = (DateTime)or.DateS,
                                    Datedelivered1 = (DateTime)or.DeliveredDate
                                }).ToList();
                    foreach (var item in products)
                    {
                        var dateE = item.estimated1;
                        dateE = dateE.ToString("D");
                        var dateO = item.orderdte1;
                        dateO = dateO.ToString("D");
                        var dateD = item.Datedelivered1;
                        dateD = dateD.ToString("D");
                        item.estimated2 = dateE;
                        item.orderdte2 = dateO;
                        item.Datedelivered2 = dateD;
                    }

                    List<ResultUserOrder> order = new List<ResultUserOrder>()
                    {
                        new ResultUserOrder{
                                product = products,
                                user = userdata
                        }
                    };
                    return order;
                }

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

        //ORDER CONFIRMATION DE-20 TASK 1YY

        public List<ResultProductsConfirmation> ObtainProductsConfirmed(string idUser)
        {
            var ivapor = System.Configuration.ConfigurationManager.AppSettings["ivapre"];
            int ivpre = Convert.ToInt32(ivapor);
            var tax = 0;
            double ivapor2 = Convert.ToDouble((double)ivpre / 100);
            List<ResultProductsConfirmation> AddressWorkshop = (dynamic)null;
            List<ResultShoppingCartProduct> products = null;
            List<ResultTypesServices> services1 = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {

                    //obtener productos del carrito
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
                                    totalpriceprod = Math.Truncate((double)cart.Price),
                                    UnitPrice = Math.Truncate((double)pro.proSuggestedPriceDP),
                                    cartid = cart.Id.ToString(),
                                    taxproduct = (int)Math.Truncate((double)cart.Price * ivapor2),
                                    proDimensionprofile = pro.proDimensionProfileDP.ToString(),
                                    proDimensionWidth = pro.proDimensionWidthDP.ToString(),
                                    proDimensionDiameter = pro.proDimensionDiameterDP.ToString()
                                }).ToList();
                    //
                    //validar si hay codigo de promocion activo
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
                    var promocodeperc = (dynamic)null;
                    if (promocodeused != null)
                    {
                        promocodeperc = db.PromotionCode.Where(s => s.IdCode == promocodeused.PromoCode).FirstOrDefault();
                    }
                    
                    //
                    //obtener los datos del cliente
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
                    //
                    //Obtener el total de los produtos
                    double? Total2 = products.Select(p => p.totalpriceprod).Sum();
                    Total2 = (int)Math.Floor((decimal)Total2);
                    //
                    //Obtener los datos de la entrega del producto y datos del taller
                    var Delivery = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).FirstOrDefault();
                    var workshop = db.Workshop.Where(s => s.IdWorkshop == Delivery.IdWorkshop).FirstOrDefault();
                    //
                    //Obtener los servicios si es que contrato
                    services1 = (from wtps in db.WorkshopServices
                                join ws in db.Workshop on wtps.IdWorkshop equals ws.IdWorkshop
                                join tps in db.TypesServices on wtps.IdService equals tps.IdService
                                join dps in db.DeliveryServices on wtps.Id equals dps.idService
                                join dela in db.DeliveryType on dps.idDelivery equals dela.IdDelivery
                                where dela.IdDelivery == Delivery.IdDelivery
                                select new ResultTypesServices
                                {
                                    IdService = (int)wtps.Id,
                                    Name = tps.Name,
                                    WorkshopImage = ws.WorkImage,
                                    WorkshopName = ws.Name,
                                    Price = Math.Truncate((double)wtps.Price),
                                    TaxIva = Math.Truncate((double)wtps.Price * ivapor2)
                                }
                        ).ToList();
                    //
                    //Obtener el total de los servicios con el iva agregado
                    double? totalserviceswithtax = 0;
                    double? totalservices = 0;
                    double? totalservicestax = 0;
                    if (services1.Count > 0 || services1 != null)
                    {
                        totalservices = services1.Select(p => (double?)p.Price).Sum();
                        totalservicestax = services1.Select(p => p.TaxIva).Sum();
                        totalservices = (int)Math.Floor((decimal)totalservices);
                        totalservicestax = (int)Math.Floor((decimal)totalservicestax);
                        totalserviceswithtax = (int)Math.Floor((decimal)totalservices + (decimal)totalservicestax);
                        tax = (int)totalservicestax;
                    }
                    //
                    //Si no hay codigo de promocion aplicado
                    if (promocodeused == null)
                    {
                        //Si se selecciono un taller para la entrega
                        if (Delivery.DeliveryType1 == false)
                        {
                            var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                            //Si el cliente selecciono una fecha y hora
                            if (Delivery.IdAppointments == 0)
                            {
                                DateTime dat = (DateTime)Delivery.Date;
                                var totalnodec = Total2;
                                var iva = totalnodec * ivapor2;//TAX
                                
                                totalnodec = totalnodec + iva;
                                totalnodec = (int)Math.Floor((decimal)totalnodec);
                                tax = (int)(ivapor2 * ((int)Total2 + (int)totalservices));
                                var totalfinal = (int)Total2 + (int)totalservices;
                                totalfinal = totalfinal + (int)(ivapor2*((int)Total2 + (int)totalservices));
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                   services = services1,
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
                                   Total = totalfinal,
                                   SubTotal = (int)Total2 + (int)totalservices,
                                   taxproduct = tax}
                                };
                            }
                            //Si el cliente selecciono una fecha y hora predeterminada por el taller
                            else
                            {
                                DateTime dat = (DateTime)Delivery.Date;
                                var totalnodec = Total2;
                                var iva = totalnodec * ivapor2;//TAX
                                tax = tax + (int)iva;
                                totalnodec = totalnodec + iva;
                                totalnodec = (int)Math.Floor((decimal)totalnodec);

                                tax = (int)(ivapor2 * ((int)Total2 + (int)totalservices));
                                var totalfinal = (int)Total2 + (int)totalservices;
                                totalfinal = totalfinal + (int)(ivapor2 * ((int)Total2 + (int)totalservices));
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                   services = services1,
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
                                   Total = totalfinal,
                                   SubTotal = (int)Total2 + (int)totalservices,
                                   taxproduct = tax}
                                };
                            }

                        }
                        //Si selecciono un lugar en el mapa para la entrega
                        else
                        {
                            DateTime dat = (DateTime)Delivery.Date;
                            var totalnodec = Total2;
                            var iva = totalnodec * ivapor2;//TAX
                            tax = tax + (int)iva;
                            totalnodec = totalnodec + iva;
                            totalnodec = (int)Math.Floor((decimal)totalnodec);
                            AddressWorkshop = new List<ResultProductsConfirmation>()
                        {
                           new ResultProductsConfirmation{ cart = products,
                            services = services1,
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
                           Total = (int)totalnodec,
                            SubTotal = (int)Total2 + (int)totalservices,
                            taxproduct = tax}
                        };
                        }
                    }////promocode null
                    //Si hay codigo de promocion aplicado
                    else
                    {
                        var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                        //Si wl cliente selecciono un taller para la entrega
                        if (Delivery.DeliveryType1 == false)
                        {
                            //Si el cliente registro una fecha y hora
                            if (Delivery.IdAppointments == 0)
                            {
                                var totalpromo = promocodeused.TotalPriceFinal;
                                totalpromo = (int)Math.Floor((decimal)totalpromo);


                                var promocodedisc = promocodeperc.PercentCode;
                                promocodedisc = promocodedisc / 100;
                                var nototalpromo = Total2;
                                var dissc = nototalpromo * (double)promocodedisc;
                                dissc = (int)Math.Floor((double)dissc);
                                nototalpromo = nototalpromo - (dissc);
                                nototalpromo = (int)Math.Floor((decimal)nototalpromo);

                                tax = (int)(ivapor2 * ((int)nototalpromo + (int)totalservices));
                                var totalfinal = (int)nototalpromo + (int)totalservices;
                                var ivagood = ivapor2 * ((int)nototalpromo + (int)totalservices);
                                ivagood = (int)Math.Floor(ivagood);
                                totalfinal = totalfinal + (int)ivagood;
                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                    services = services1,
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
                                   Total = totalfinal,
                                   SubTotal = (int)nototalpromo + (int)totalservices,
                                   taxproduct = tax}
                                };
                            }
                            //Si el cliente selecciono una hora y fecha configurada del taller
                            else
                            {
                                var totalpromo = promocodeused.TotalPriceFinal;
                                totalpromo = (int)Math.Floor((decimal)totalpromo);

                                var promocodedisc = promocodeperc.PercentCode;
                                promocodedisc = promocodedisc / 100;
                                var nototalpromo = Total2;
                                var dissc = nototalpromo * (double)promocodedisc;
                                dissc = (int)Math.Floor((double)dissc);
                                nototalpromo = nototalpromo - (dissc);
                                nototalpromo = (int)Math.Floor((decimal)nototalpromo);

                                tax = (int)(ivapor2 * ((int)nototalpromo + (int)totalservices));
                                var totalfinal = (int)nototalpromo + (int)totalservices;
                                var ivagood = ivapor2 * ((int)nototalpromo + (int)totalservices);
                                ivagood = (int)Math.Floor(ivagood);
                                totalfinal = totalfinal + (int)ivagood;

                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultProductsConfirmation>()
                                {
                                   new ResultProductsConfirmation{ cart = products,
                                    services = services1,
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
                                   Total = totalfinal,
                                   SubTotal = (int)nototalpromo + (int)totalservices,
                                   taxproduct = tax}
                                };
                            }
                        }
                        //Si el cliente selecciono un lugar en el mapa y no un taller
                        else
                        {
                            DateTime dat = (DateTime)Delivery.Date;
                            var totalpromo = promocodeused.TotalPriceFinal;
                            totalpromo = (int)Math.Floor((decimal)totalpromo);

                            var promocodedisc = promocodeperc.PercentCode;
                            promocodedisc = promocodedisc / 100;
                            var nototalpromo = Total2;
                            var dissc = nototalpromo * (double)promocodedisc;
                            dissc = (int)Math.Floor((double)dissc);
                            nototalpromo = nototalpromo - (dissc);
                            nototalpromo = (int)Math.Floor((decimal)nototalpromo);

                            tax = (int)(ivapor2 * ((int)nototalpromo + (int)totalservices));
                            var totalfinal = (int)nototalpromo + (int)totalservices;
                            var ivagood = ivapor2 * ((int)nototalpromo + (int)totalservices);
                            ivagood = (int)Math.Floor(ivagood);
                            totalfinal = totalfinal + (int)ivagood;

                            AddressWorkshop = new List<ResultProductsConfirmation>()
                        {
                           new ResultProductsConfirmation{ cart = products,
                            services = services1,
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
                           Total = (int)totalfinal,
                            SubTotal = (int)nototalpromo,
                            taxproduct = tax}
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
            var tax = 0;
            var ivapor = System.Configuration.ConfigurationManager.AppSettings["ivapre"];
            int ivpre = Convert.ToInt32(ivapor);
            double ivapor2 = Convert.ToDouble((double)ivpre / 100);
            List<ResultPaidProducts> AddressWorkshop = (dynamic)null;
            List<ResultShoppingCartProduct> products = null;
            List<ResultTypesServices> services1 = null;
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
                                    totalpriceprod = Math.Truncate((double)cart.price),
                                    taxproduct = (int)Math.Truncate((double)cart.price * ivapor2),
                                    UnitPrice = Math.Truncate((double)pro.proSuggestedPriceDP),
                                    cartid = cart.id.ToString(),
                                    proDimensionprofile = pro.proDimensionProfileDP.ToString(),
                                    proDimensionWidth = pro.proDimensionWidthDP.ToString(),
                                    proDimensionDiameter = pro.proDimensionDiameterDP.ToString()
                                }).ToList();
                    var promocodeused = (from prom in db.PromoCodeUsed
                                         join or in db.Orders on prom.PromoCode equals or.PromoCode
                                         where (prom.idUser == idUser && prom.Used == true && or.id == order)
                                         select new
                                         {
                                             prom.PromoCode,
                                             prom.TotalPriceFinal
                                         }).FirstOrDefault();
                    var promocodeperc = (dynamic)null;
                    if (promocodeused != null)
                    {
                        promocodeperc = db.PromotionCode.Where(s => s.IdCode == promocodeused.PromoCode).FirstOrDefault();
                    }
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
                    //Obtener los servicios si es que contrato
                    services1 = (from wtps in db.WorkshopServices
                                 join ws in db.Workshop on wtps.IdWorkshop equals ws.IdWorkshop
                                 join tps in db.TypesServices on wtps.IdService equals tps.IdService
                                 join dps in db.DeliveryServices on wtps.Id equals dps.idService
                                 join dela in db.DeliveryType on dps.idDelivery equals dela.IdDelivery
                                 where dela.IdDelivery == Delivery.IdDelivery
                                 select new ResultTypesServices
                                 {
                                     IdService = (int)wtps.Id,
                                     Name = tps.Name,
                                     WorkshopImage = ws.WorkImage,
                                     WorkshopName = ws.Name,
                                     Price = Math.Truncate((double)wtps.Price),
                                     TaxIva = Math.Truncate((double)wtps.Price * ivapor2)
                                 }
                        ).ToList();
                    //
                    //Obtener el total de los servicios con el iva agregado
                    double? totalserviceswithtax = 0;
                    double?  totalservices = 0;
                    double? totalservicestax = 0;
                    if (services1.Count > 0 || services1 != null)
                    {
                        totalservices = services1.Select(p => (double?)p.Price).Sum();
                        totalservicestax = services1.Select(p => p.TaxIva).Sum();
                        totalservices = (int)Math.Floor((decimal)totalservices);
                        totalservicestax = (int)Math.Floor((decimal)totalservicestax);
                        totalserviceswithtax = (int)Math.Floor((decimal)totalservices + (decimal)totalservicestax);
                        tax = (int)totalservicestax;
                    }
                    //
                    //Si el cliente no valido ningun codigo de promocion
                    if (promocodeused == null)
                    {
                        //si el cliente selecciono un taller en su entrega
                        if (Delivery.DeliveryType1 == false)
                        {
                            var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                            //Si el cliente ingreso una direccion y horario
                            if (Delivery.IdAppointments == 0)
                            {
                                var totalnodec = Total2;
                                var iva = totalnodec * ivapor2;//TAX
                                tax = tax + (int)iva;
                                totalnodec = totalnodec + iva;
                                totalnodec = (int)Math.Floor((decimal)totalnodec);

                                tax = (int)(ivapor2 * ((int)Total2 + (int)totalservices));
                                var totalfinal = (int)Total2 + (int)totalservices;
                                totalfinal = totalfinal + (int)(ivapor2 * ((int)Total2 + (int)totalservices));

                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                   services = services1,
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
                                   Total = totalfinal,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order,
                                   SubTotal = (int)Total2 + (int)totalservices,
                                   taxproduct = tax
                                   }

                                };
                            }
                            //Si el cliente selecciono una direccion y horario del taller
                            else
                            {
                                var totalnodec = Total2;
                                var iva = totalnodec * ivapor2;//TAX
                                tax = tax + (int)iva;
                                totalnodec = totalnodec + iva;
                                totalnodec = (int)Math.Floor((decimal)totalnodec);

                                tax = (int)(ivapor2 * ((int)Total2 + (int)totalservices));
                                var totalfinal = (int)Total2 + (int)totalservices;
                                totalfinal = totalfinal + (int)(ivapor2 * ((int)Total2 + (int)totalservices));


                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                   services = services1,
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
                                   Total = totalfinal,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order,
                                   SubTotal = (int)Total2 + (int)totalservices,
                                   taxproduct = tax}
                                };
                            }

                        }
                        //Si selecciono un mapa
                        else
                        {
                            var totalnodec = Total2;
                            var iva = totalnodec * ivapor2;//TAX
                            tax = tax + (int)iva;
                            totalnodec = totalnodec + iva;
                            totalnodec = (int)Math.Floor((decimal)totalnodec);
                            DateTime dat = (DateTime)Delivery.Date;
                            AddressWorkshop = new List<ResultPaidProducts>()
                        {
                           new ResultPaidProducts{ cart = products,
                           services = services1,
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
                           Total = (decimal)totalnodec ,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order,
                                   SubTotal = (int)Total2,
                                   taxproduct = tax}
                        };
                        }
                    }////promocode null
                    //
                    //Si el cliente utilizo un codigo de promocion
                    else
                    {
                        var appointment = db.WorkshopAppointment.Where(s => s.Id == Delivery.IdAppointments).FirstOrDefault();
                        //Si el cliente selecciono un taller
                        if (Delivery.DeliveryType1 == false)
                        {
                            //Si el cliente ingreso una fecha y horario de entrega
                            if (Delivery.IdAppointments == 0)
                            {
                                var promocodedisc = promocodeperc.PercentCode;
                                promocodedisc = promocodedisc / 100;
                                var nototalpromo = Total2;
                                var promototal = (double)nototalpromo * (double)promocodedisc;
                                promototal = (int)Math.Floor((decimal)promototal);
                                nototalpromo = nototalpromo - (double)promototal;
                                nototalpromo = (int)Math.Floor((decimal)nototalpromo);

                                tax = (int)(ivapor2 * ((int)nototalpromo + (int)totalservices));
                                var totalfinal = (int)nototalpromo + (int)totalservices;
                                var ivagood = ivapor2 * ((int)nototalpromo + (int)totalservices);
                                ivagood = (int)Math.Floor(ivagood);
                                totalfinal = totalfinal + (int)ivagood;

                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                   services = services1,
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
                                   Total = totalfinal, 
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order,
                                     SubTotal = (int)nototalpromo + (int)totalservices,
                                   taxproduct = tax}
                                };
                            }
                            //Si el cliente selecciono un horario y fecha de entrega del taller
                            else
                            {

                                var promocodedisc = promocodeperc.PercentCode;
                                promocodedisc = promocodedisc / 100;
                                var nototalpromo = Total2;
                                var promototal = (double)nototalpromo * (double)promocodedisc;
                                promototal = (int)Math.Floor((decimal)promototal);
                                nototalpromo = nototalpromo - (double)promototal;
                                nototalpromo = (int)Math.Floor((decimal)nototalpromo);

                                tax = (int)(ivapor2 * ((int)nototalpromo + (int)totalservices));
                                var totalfinal = (int)nototalpromo + (int)totalservices;
                                var ivagood = ivapor2 * ((int)nototalpromo + (int)totalservices);
                                ivagood = (int)Math.Floor(ivagood);
                                totalfinal = totalfinal + (int)ivagood;

                                DateTime dat = (DateTime)Delivery.Date;
                                AddressWorkshop = new List<ResultPaidProducts>()
                                {
                                   new ResultPaidProducts{ cart = products,
                                   services = services1,
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
                                   Total = totalfinal,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order,
                                     SubTotal = (int)nototalpromo + (int)totalservices,
                                   taxproduct = tax}
                                };
                            }
                        }
                        //Si el cliente ingreso una direccion en el mapa
                        else
                        {


                            var promocodedisc = promocodeperc.PercentCode;
                            promocodedisc = promocodedisc / 100;
                            var nototalpromo = Total2;
                            var dissc = nototalpromo * (double)promocodedisc;
                            dissc = (int)Math.Floor((double)dissc);
                            nototalpromo = nototalpromo - (dissc);
                            nototalpromo = (int)Math.Floor((decimal)nototalpromo);

                            tax = (int)(ivapor2 * ((int)nototalpromo + (int)totalservices));
                            var totalfinal = (int)nototalpromo + (int)totalservices;
                            var ivagood = ivapor2 * ((int)nototalpromo + (int)totalservices);
                            ivagood = (int)Math.Floor(ivagood);
                            totalfinal = totalfinal + (int)ivagood;

                            DateTime dat = (DateTime)Delivery.Date;
                            AddressWorkshop = new List<ResultPaidProducts>()
                        {
                           new ResultPaidProducts{ cart = products,
                           services = services1,
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
                           Total = totalfinal,
                                   TypeTarget = payment.TargetType,
                                   Number = (int)payment.Number,
                                   Expire = payment.Expire,
                                   Order = order,
                                  SubTotal = (int)nototalpromo,
                                   taxproduct = tax}
                        };
                        }
                    }
                }
                Mail mail = new Mail();

                mail.sendEmailConfirmation(AddressWorkshop);
                return AddressWorkshop;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public List<ResultUserPromo> loadPromos(string idUser)
        {
            try
            {
                List<Promos> datosTabla = null;
                List<Promos> promos = new List<Promos>();
                List<ResultDataUser> userdata = null;
                using (var db = new dekkOnlineEntities())
                {
                    var codigoPromo = (from pro in db.PromotionCode
                                       where (pro.IdUser == idUser)
                                       select new Promos
                                       {
                                           PromoCode = pro.IdCode
                                       }).FirstOrDefault();
                    userdata = (from us in db.AspNetUsers
                                join us2 in db.UserAddress on us.Id equals us2.IdUser
                                where (us.Id == idUser)
                                select new ResultDataUser
                                {
                                    FirstName = us2.FirstName,
                                    LastName = us2.LastName,
                                    Email = us.Email,
                                    Image = us2.Image,
                                    Promocode = codigoPromo.PromoCode
                                }).ToList();
                    if (userdata.Count == 0)
                    {
                        userdata = (from us in db.AspNetUsers
                                    where us.Id == idUser
                                    select new ResultDataUser
                                    {
                                        Email = us.Email,
                                        Promocode = codigoPromo.PromoCode
                                    }).ToList();
                    }
                    var promosActivas = (from pro in db.PromoCodeUsed
                                         where (pro.PromoCode == codigoPromo.PromoCode)
                                         select new Promos
                                         {
                                             idUser = pro.idUser,
                                             PromoCode = pro.PromoCode,
                                             Points = pro.Points,
                                             Used = pro.Used,
                                             Date = pro.DateUsed.ToString()
                                         }).ToList();
                        foreach (var item in promosActivas)
                        {

                            var userdata2 = (from us in db.AspNetUsers
                                             join us2 in db.UserAddress on us.Id equals us2.IdUser
                                             where (us.Id == item.idUser)
                                             select new
                                             {
                                                 FirstName = us2.FirstName,
                                                 LastName = us2.LastName,
                                                 Email = us.Email
                                             }).FirstOrDefault();
                            if (userdata2 != null)
                            {
                                datosTabla = new List<Promos>
                            {
                                new Promos
                                {
                                    UserName = userdata2.FirstName,
                                    Email = userdata2.Email,
                                    PromoCode = item.PromoCode,
                                    Points = item.Points,
                                    Date = item.Date,
                                    Used = item.Used
                                }
                            };
                                promos.AddRange(datosTabla);
                            }
                            else
                            {
                                datosTabla = new List<Promos>
                            {
                                new Promos
                                {
                                    UserName = null,
                                    Email = null,
                                    PromoCode = item.PromoCode,
                                    Points = item.Points,
                                    Date = item.Date,
                                    Used = item.Used
                                }
                            };
                                promos.AddRange(datosTabla);
                            }
                        }

                    List<ResultUserPromo> promosResult = new List<ResultUserPromo>()
                    {
                        new ResultUserPromo{
                                promo = promos,
                                user = userdata
                        }
                    };
                    return promosResult;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }
}
