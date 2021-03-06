﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Framework.Libraies;
using System.Data.Entity;
namespace Framework
{
    public class ShoppingCart
    {
        public string User(string user)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {

                    var idUser = db.AspNetUsers.Where(s => s.Email == user).Select(s => s.Id).FirstOrDefault();
                    return idUser;

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //ADD TO CART DE-5 TASK 3
        public bool AddToCart(string idUser, int id_dekk, int id_quantity)
        {
            try
            {

                using (var db = new dekkOnlineEntities())
                {
                    var productPrice = db.products.Where(s => s.proId == id_dekk).Select(x => x.proSuggestedPrice).FirstOrDefault();
                    decimal price = Convert.ToDecimal(productPrice) * id_quantity;
                    var promocodeused = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).FirstOrDefault();
                    Entity.ShoppingCart productShoppingCart = db.ShoppingCart.Where(s => s.proId == id_dekk && s.IdUser.Equals(idUser) && s.Status == false).FirstOrDefault();
                    price = (int)Math.Floor(price);
                    if (productShoppingCart == null)
                    {
                        Entity.ShoppingCart addCart = new Entity.ShoppingCart();
                        addCart.IdUser = idUser;
                        addCart.proId = id_dekk;
                        addCart.quantity = id_quantity;
                        addCart.Price = price;
                        addCart.Status = false;
                        db.ShoppingCart.Add(addCart);
                    }
                    else
                    {
                        productShoppingCart.quantity = id_quantity;
                        productShoppingCart.Price = price;
                    }
                    if (promocodeused != null)
                    {
                        var percent = db.PromotionCode.Where(s => s.IdCode == promocodeused.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                        percent = percent / 100;
                        promocodeused.TotalPrice = promocodeused.TotalPrice + price;
                        promocodeused.TotalPriceFinal = promocodeused.TotalPrice - (percent * (promocodeused.TotalPrice));
                        db.Entry(promocodeused).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //_Error = ex;
                return false;
            }
        }

        //DELETE FROM CART DE-11 TASK 5
        public bool DeleteProductFromCart(string idcart, string idUser)
        {
            Framework.encryptdecrypt en = new encryptdecrypt();
            try
            {
                int id = Convert.ToInt32(en.DesEncriptar(idcart));
                using (var db = new dekkOnlineEntities())
                {
                    var addCart = new ShoppingCart();
                    var d = db.ShoppingCart.Where(x => x.Id == id).FirstOrDefault();
                    if (d != null)
                    {

                        var promocodeused = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).FirstOrDefault();
                        if (promocodeused != null)
                        {
                            var percent = db.PromotionCode.Where(s => s.IdCode == promocodeused.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                            percent = percent / 100;
                            promocodeused.TotalPrice = promocodeused.TotalPrice - d.Price;
                            promocodeused.TotalPriceFinal = promocodeused.TotalPrice - (percent * (promocodeused.TotalPrice));
                            db.Entry(promocodeused).State = EntityState.Modified;
                        }
                        db.ShoppingCart.Remove(d);
                        db.SaveChanges();
                        var user = db.ShoppingCart.Where(s => s.IdUser == idUser && s.Status == false).FirstOrDefault();
                        if (user != null)
                        {
                            return true;
                        }
                        else if (user == null)
                        {
                            var promocodeused2 = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).FirstOrDefault();
                            if (promocodeused2 != null)
                            {
                                db.PromoCodeUsed.Remove(promocodeused2);
                                db.SaveChanges();
                                return true;
                            }
                            else
                            {
                                return true;
                            }

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

        public string IncreaseProductFromCart(string idcart, int qty, string idUser)
        {
            Framework.encryptdecrypt en = new encryptdecrypt();
            try
            {
                int id = Convert.ToInt32(en.DesEncriptar(idcart));
                using (var db = new dekkOnlineEntities())
                {
                    var d = db.ShoppingCart.Where(x => x.Id == id).FirstOrDefault();
                    //var e = db.products.Where(x => x.proId == d.proId).FirstOrDefault();
                    //if (e.proInventory > qty)
                    //{
                    //    if (d != null)
                    //    {
                    //        d.Price = ((d.Price / d.quantity) * qty);
                    //        d.quantity = qty;
                    //        db.SaveChanges();
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    //    if (d != null)
                    //    {
                    //        d.Price = ((d.Price / d.quantity) * e.proInventory);
                    //        d.quantity = e.proInventory;
                    //        db.SaveChanges();
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        return false;
                    //    }
                    //}
                    if (d != null)
                    {
                        var final = ((d.Price / d.quantity) * qty);
                        d.Price = final;
                        d.quantity = qty;
                        var promocodeused = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).FirstOrDefault();
                        if (promocodeused != null)
                        {
                            var percent = db.PromotionCode.Where(s => s.IdCode == promocodeused.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                            percent = percent / 100;
                            promocodeused.TotalPrice = promocodeused.TotalPrice + (final - promocodeused.TotalPrice);
                            promocodeused.TotalPriceFinal = promocodeused.TotalPrice - (percent * (promocodeused.TotalPrice));
                            db.Entry(promocodeused).State = EntityState.Modified;
                            db.Entry(d).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        var qtyfinal = db.ShoppingCart.Where(s => s.Id == id).Select(s => s.Price).FirstOrDefault();
                        qtyfinal = (decimal)Math.Truncate((double)qtyfinal);
                        return qtyfinal.ToString();
                    }
                    else
                    {
                        return null;
                    }

                }

            }
            catch (Exception ex)
            {

                return null;
            }

        }


        //PRODUCTS IN CART DE-11 TASK 1 cambios
        public List<ResultAllCart> ProductsInCart(string User)//
        {
            var ivapor = System.Configuration.ConfigurationManager.AppSettings["ivapre"];
            int ivpre = Convert.ToInt32(ivapor);
            double ivapor2 = Convert.ToDouble((double)ivpre / 100);
            List<ResultShoppingCartProduct> products = null;
            List<ResultAllCart> allcart = null;
            try
            {

                using (var db = new dekkOnlineEntities())
                {
                    Framework.encryptdecrypt en = new encryptdecrypt();
                    products = (from pro in db.products
                                join cart in db.ShoppingCart on pro.proId equals cart.proId
                                where cart.IdUser.Equals(User) && cart.Status == false
                                select new ResultShoppingCartProduct
                                {
                                    IdUser = cart.IdUser,
                                    proId = cart.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = cart.quantity,
                                    totalpriceprod = Math.Truncate((double)cart.Price),
                                    cartid = cart.Id.ToString(),
                                    UnitPrice = Math.Truncate((double)pro.proSuggestedPrice),
                                    proDimensionprofile = pro.proDimensionProfileDP.ToString(),
                                    proDimensionWidth = pro.proDimensionWidthDP.ToString(),
                                    proDimensionDiameter = pro.proDimensionDiameterDP.ToString()
                                }).ToList();


                    //var promocode1 = ""; //LoadPromoCodeFomUser(User);
                    //var promocode1 = LoadPromoCodeFomUser(User);
                    var points1 = LoadPointsPerUser(User);

                    var promocodeused = db.PromoCodeUsed.Where(s => s.idUser == User && s.Used == false).FirstOrDefault();
                    double? subtotal1 = products.Select(p => p.totalpriceprod).Sum();
                    subtotal1 = subtotal1 == null ? 00 : subtotal1;

                    foreach (var item in products)
                    {
                        //subtotal1 += Convert.ToDecimal(item.totalpriceprod);
                        item.cartid = en.Encriptar(item.cartid);
                    }
                    if (promocodeused == null)
                    {
                        var subtotal2 = subtotal1;
                        var iva = subtotal2 * ivapor2;//TAX
                        subtotal2 = subtotal2 + iva;
                        subtotal2 = (int)Math.Floor((decimal)subtotal2);
                        allcart = new List<ResultAllCart> {
                        new ResultAllCart{
                        cart = products,
                        subtotal = (decimal)subtotal1,
                        //promocode = promocode1,
                        promocode = null,
                        points = points1,
                        total = (decimal)subtotal1,
                        promocodeapp = false,
                        tax = (Math.Truncate((double)iva)).ToString()} };
                    }
                    else
                    {
                        var promocode= db.PromotionCode.Where(s => s.IdCode == promocodeused.PromoCode).FirstOrDefault();
                        var promodis = promocode.PercentCode;
                        promodis = promodis / 100;
                        var disc = subtotal1 * (double)promodis;
                        disc = Math.Truncate((double)disc);
                        var dissub = (double)subtotal1 - (double)disc;
                        var subtotal2 = subtotal1;
                        var iva = subtotal2 * ivapor2;//TAX
                        subtotal2 = subtotal2 + iva;
                        allcart = new List<ResultAllCart> {
                        new ResultAllCart{
                        cart = products,
                        subtotal = (decimal)subtotal1,
                        promocode = promocodeused.PromoCode,
                        points = points1,
                        total = (decimal)Math.Truncate(dissub),
                        promocodeapp = true,
                        tax = (Math.Truncate((double)disc).ToString())//DESCUENTO
                        } };

                    }

                }


            }
            catch (Exception ex)
            {

                return allcart;
            }
            return allcart;
        }

        //LOAD PROMO CODE FROM USER DE-11 TASK 2 cambios
        //public string LoadPromoCodeFomUser(string idUser)
        //{
        //    var promocode = (dynamic)null;
        //    try
        //    {
        //        using (var db = new dekkOnlineEntities())
        //        {
        //            promocode = db.PromotionCodeUser.Where(s => s.IdUser == idUser).Select(s => s.IdCode).FirstOrDefault();
        //            if (promocode != null)
        //            {
        //                return promocode;
        //            }
        //            else
        //            {
        //                return promocode = "";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        
        //        throw;
        //    }

        //}


        //VALIDATE PROMO CODE DE-11 cambios
        public bool ValidatePromoCode(string code, string idUser)
        {
            var ivapor = System.Configuration.ConfigurationManager.AppSettings["ivapre"];
            int ivpre = Convert.ToInt32(ivapor);
            double ivapor2 = Convert.ToDouble((double)ivpre / 100);
            var promocodediscount = (dynamic)null;
            List<ResultShoppingCartProduct> products = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    DateTime DateToday = DateTime.Now;
                    promocodediscount = db.PromotionCode.Where(p => p.IdCode == code && p.DateStart <= DateToday && p.DateEnd >= DateToday).Select(p => p.PercentCode).FirstOrDefault();
                    var promocodeuservalidate = db.PromotionCode.Where(p => p.IdUser == idUser && p.IdCode == code).FirstOrDefault();
                    if (promocodediscount != null && promocodeuservalidate == null)
                    {
                        var usercode = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.PromoCode == code && s.Used == true).FirstOrDefault();
                        var usercodenotused = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false && s.PromoCode == code).FirstOrDefault();//aqui
                        if (usercode != null )
                        {
                            return false;
                        }
                        else if ( usercodenotused != null)
                        {
                            products = (from pro in db.products
                                        join cart in db.ShoppingCart on pro.proId equals cart.proId
                                        where cart.IdUser.Equals(idUser) && cart.Status == false
                                        select new ResultShoppingCartProduct
                                        {
                                            totalpriceprod = Math.Round((double)cart.Price, 2),
                                        }).ToList();
                            double? totalprice = products.Select(p => p.totalpriceprod).Sum();
                            totalprice = (int)Math.Floor((decimal)totalprice);
                            double? totalprice2 = products.Select(p => p.totalpriceprod).Sum();
                            promocodediscount = promocodediscount / 100;
                            decimal totalpricepromocode = promocodediscount * (decimal)totalprice;
                            totalpricepromocode = (int)Math.Floor((decimal)totalpricepromocode);
                            totalprice = totalprice - (double)totalpricepromocode;
                            //var iva = totalprice * ivapor2;//TAX
                            //totalprice = totalprice + iva; //Con esto comentado solo guardara el precio final de la orden sin el iva
                            totalprice = (int)Math.Floor((decimal)totalprice);
                            //totalprice2 = totalprice2 + iva;//Aqui guardara el precio de la orden sin validar el codigo de promocion y sin iva
                            usercodenotused.PromoCode = code;
                            usercodenotused.TotalPrice = (decimal)totalprice2;
                            usercodenotused.TotalPriceFinal = (decimal)totalprice;
                            usercodenotused.Used = false;
                            usercodenotused.DateUsed = DateTime.Now;
                            db.Entry(usercodenotused).State = EntityState.Modified;
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            products = (from pro in db.products
                                        join cart in db.ShoppingCart on pro.proId equals cart.proId
                                        where cart.IdUser.Equals(idUser) && cart.Status == false
                                        select new ResultShoppingCartProduct
                                        {
                                            totalpriceprod = Math.Round((double)cart.Price, 2),
                                        }).ToList();
                            double? totalprice = products.Select(p => p.totalpriceprod).Sum();
                            totalprice = (int)Math.Floor((decimal)totalprice);
                            double? totalprice2 = products.Select(p => p.totalpriceprod).Sum();
                            promocodediscount = promocodediscount / 100;
                            decimal totalpricepromocode = promocodediscount * (decimal)totalprice;
                            totalpricepromocode = (int)Math.Floor((decimal)totalpricepromocode);
                            totalprice = totalprice - (double)totalpricepromocode;
                            //var iva = totalprice * ivapor2;//TAX
                            //totalprice = totalprice + iva; //Con esto comentado solo guardara el precio final de la orden sin el iva
                            totalprice = (int)Math.Floor((decimal)totalprice);
                            //totalprice2 = totalprice2 + iva;//Aqui guardara el precio de la orden sin validar el codigo de promocion y sin iva
                            Entity.PromoCodeUsed addpromo = new Entity.PromoCodeUsed();
                            addpromo.idUser = idUser;
                            addpromo.PromoCode = code;
                            addpromo.TotalPrice = (decimal)totalprice2;
                            addpromo.TotalPriceFinal = (decimal)totalprice;
                            addpromo.Used = false;
                            addpromo.DateUsed = DateTime.Now;
                            addpromo.Points = 0;
                            db.PromoCodeUsed.Add(addpromo);
                            db.SaveChanges();
                            return true;
                        }
                     
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UndoPromoCode(string idUser)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var a = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.Used == false).OrderByDescending(s => s.id).FirstOrDefault();
                    if (a != null)
                    {
                        db.PromoCodeUsed.Remove(a);
                        db.SaveChanges();
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

                return false;
            }
        }

        //LOAD CURRENT POINTS PER USER DE-11 TASK 3
        public int LoadPointsPerUser(string idUser)
        {
            var NowPointsLoad = (dynamic)null;

            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var user = db.AspNetUsers.Where(s => s.Id == idUser).FirstOrDefault();
                    if (user != null)
                    {

                        NowPointsLoad = db.DetailUserPoints.Where(s => s.IdUser == idUser && s.StatusofPromo == true).Select(s => s.PointsEarned).Sum().ToString();

                        if (NowPointsLoad == null || NowPointsLoad == "")
                        {
                            NowPointsLoad = "0";
                        }

                        var UserPoints = db.UserPoints.Where(s => s.IdUser == idUser).FirstOrDefault();
                        if (UserPoints == null)
                        {
                            var points = new Entity.UserPoints();

                            points.IdUser = idUser;
                            points.Points = Convert.ToInt32(NowPointsLoad);
                            db.UserPoints.Add(points);
                            db.SaveChanges();
                            return Convert.ToInt32(NowPointsLoad);
                        }
                        else
                        {
                            UserPoints.Points = Convert.ToInt32(NowPointsLoad);
                            db.Entry(UserPoints).State = EntityState.Modified;
                            db.SaveChanges();
                            return Convert.ToInt32(NowPointsLoad);

                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public bool UpdateShoppingCart(string IdUser1, string idUserCookies)
        {
            var ivapor = System.Configuration.ConfigurationManager.AppSettings["ivapre"];
            int ivpre = Convert.ToInt32(ivapor);
            double ivapor2 = Convert.ToDouble((double)ivpre / 100);
            bool result = false;
            using (var db = new dekkOnlineEntities())
            {
                decimal? price = 0.00m;
                var user = db.ShoppingCart.Where(s => s.IdUser == idUserCookies && s.Status == false).ToList();
                var promocodeusedck = db.PromoCodeUsed.Where(s => s.idUser == idUserCookies && s.Used == false).FirstOrDefault();                
                if (user.Count > 0)
                {
                    foreach (var item in user)
                    {
                        item.IdUser = IdUser1;
                        price += item.Price;

                    }
                 db.SaveChanges();
                    var promocodeusedus = db.PromoCodeUsed.Where(s => s.idUser == IdUser1 && s.Used == false).FirstOrDefault();
                    if (promocodeusedck != null)
                    {
                        price = 0;
                        var validatepromocodeuser = db.PromotionCode.Where(s => s.IdCode == promocodeusedck.PromoCode && s.IdUser == IdUser1).FirstOrDefault();
                        var validatepromocodeusedbyuser = db.PromoCodeUsed.Where(s => s.PromoCode == promocodeusedck.PromoCode && s.idUser == IdUser1 && s.Used == true).FirstOrDefault();
                        if (validatepromocodeuser != null && validatepromocodeusedbyuser == null)
                        {

                        }
                        else
                        {
                            var percent = db.PromotionCode.Where(s => s.IdCode == promocodeusedck.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                            var user1 = db.ShoppingCart.Where(s => s.IdUser == IdUser1 && s.Status == false).ToList();
                            foreach (var item in user1)
                            {
                                price += item.Price;
                            }
                            promocodeusedck.idUser = IdUser1;
                            percent = percent / 100;
                            promocodeusedck.TotalPrice = promocodeusedck.TotalPrice + price;
                            var promo1 = (percent * promocodeusedus.TotalPrice);
                            promo1 = (int)Math.Floor((decimal)promo1);
                            var totalfinal = promocodeusedck.TotalPrice - promo1;
                            totalfinal = (int)Math.Floor((decimal)totalfinal);
                            //var iva = (double)totalfinal * ivapor2;//TAX
                            //totalfinal = totalfinal + (decimal)iva;
                            totalfinal = (int)Math.Floor((decimal)totalfinal);
                            promocodeusedck.TotalPriceFinal = (decimal)totalfinal;
                            if (promocodeusedus != null)
                            {
                                db.PromoCodeUsed.Remove(promocodeusedus);

                            }
                        }
                    }
                 else   if (promocodeusedus != null)
                    {
                        var percent = db.PromotionCode.Where(s => s.IdCode == promocodeusedus.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                        percent = percent / 100;
                        promocodeusedus.TotalPrice = promocodeusedus.TotalPrice + price;
                        var promo1 = (percent * promocodeusedus.TotalPrice);
                        promo1 = (int)Math.Floor((decimal)promo1);
                        var totalfinal = promocodeusedus.TotalPrice - promo1;
                        totalfinal = (int)Math.Floor((decimal)totalfinal);
                        //var iva = (double)totalfinal * ivapor2;//TAX
                        //totalfinal = totalfinal + (decimal)iva;
                        totalfinal = (int)Math.Floor((decimal)totalfinal);
                        promocodeusedus.TotalPriceFinal = (decimal)totalfinal;
                    }
                   
                   
                    db.SaveChanges();
                    result = true;
                }
                else if (user.Count == 0)
                {
                    var promocodeusedus = db.PromoCodeUsed.Where(s => s.idUser == IdUser1 && s.Used == false).FirstOrDefault();

                    if (promocodeusedck != null)
                    {
                        price = 0;
                        var validatepromocodeuser = db.PromotionCode.Where(s => s.IdCode == promocodeusedck.PromoCode && s.IdUser == IdUser1).FirstOrDefault();
                        var validatepromocodeusedbyuser = db.PromoCodeUsed.Where(s => s.PromoCode == promocodeusedck.PromoCode && s.idUser == IdUser1 && s.Used == true).FirstOrDefault();
                        if (validatepromocodeuser != null && validatepromocodeusedbyuser == null)
                        {
                            return false;
                        }
                        else
                        {
                            var percent = db.PromotionCode.Where(s => s.IdCode == promocodeusedck.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                            var user1 = db.ShoppingCart.Where(s => s.IdUser == IdUser1 && s.Status == false).ToList();
                            foreach (var item in user1)
                            {
                                price += item.Price;
                            }
                            promocodeusedck.idUser = IdUser1;
                            percent = percent / 100;
                            promocodeusedck.TotalPrice = price;
                            var promo = percent * price;
                            promo = (int)Math.Floor((decimal)promo);
                            var totalfinal = promocodeusedck.TotalPrice - promo;
                            totalfinal = (int)Math.Floor((decimal)totalfinal);
                            //var iva = (double)totalfinal * ivapor2;//TAX
                            //totalfinal = totalfinal + (decimal)iva;
                            totalfinal = (int)Math.Floor((decimal)totalfinal);
                            promocodeusedck.TotalPriceFinal = (decimal)totalfinal;
                            if (promocodeusedus != null)
                            {
                                db.PromoCodeUsed.Remove(promocodeusedus);

                            }
                        }
                    }
                   else if (promocodeusedus != null)
                    {
                        var percent = db.PromotionCode.Where(s => s.IdCode == promocodeusedus.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                        percent = percent / 100;
                        promocodeusedus.TotalPrice = promocodeusedus.TotalPrice + price;
                        var promo1 = (percent * promocodeusedus.TotalPrice);
                        promo1 = (int)Math.Floor((decimal)promo1);
                        var totalfinal = promocodeusedus.TotalPrice - promo1;

                        totalfinal = (int)Math.Floor((decimal)totalfinal);
                        //var iva = (double)totalfinal * ivapor2;//TAX
                        //totalfinal = totalfinal + (decimal)iva;
                        totalfinal = (int)Math.Floor((decimal)totalfinal);
                        promocodeusedus.TotalPriceFinal = (decimal)totalfinal;
                    }

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }


        public bool updateDeleveryType(string IdUser1, string idUserCookies)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var registro = db.DeliveryType.Where(s => s.IdUser == idUserCookies).OrderByDescending(s => s.IdUser).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.IdUser = IdUser1;
                        db.Entry(registro).State = EntityState.Modified;
                        db.SaveChanges();

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return result;
        }

        public bool confirmPromocode(string code, string idUser, string idUserCookie)
        {
            bool result = false;
            try
            {
                DateTime DateToday = DateTime.Now;
                var promocodediscountUser = (dynamic)null;
                var promocodediscountdates = (dynamic)null;
                using (var db = new dekkOnlineEntities())
                {
                    var userdata = db.AspNetUsers.Where(s=>s.Id == idUser).FirstOrDefault();//validar si se esta entrando a la url con un usuario o con cookie
                    if (userdata != null)
                    {
                        promocodediscountUser = db.PromotionCode.Where(p => p.IdCode == code && p.IdUser == userdata.Id).FirstOrDefault();//si entra un usuario logueado validar si no es su propio codigo de promocion
                        if (promocodediscountUser != null)
                        {
                            result = false;
                        }
                        else
                        {
                            promocodediscountdates = db.PromotionCode.Where(p => p.IdCode == code && p.DateStart <= DateToday && p.DateEnd >= DateToday).FirstOrDefault();//validar si el codigo de promocion esta vigente
                            if (promocodediscountdates != null)
                            {
                                var usercode = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.PromoCode == code && s.Used == true).FirstOrDefault();//validar que el usuario no haya utilizado antes el codigo de promocion
                                if (usercode != null)
                                {
                                    result = false;
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        promocodediscountdates = db.PromotionCode.Where(p => p.IdCode == code && p.DateStart <= DateToday && p.DateEnd >= DateToday).FirstOrDefault();//validar si el codigo de promocion esta vigente
                        if (promocodediscountdates != null)
                        {
                                var usercode2 = db.PromoCodeUsed.Where(s => s.idUser == idUserCookie  && s.Used == false).FirstOrDefault();//validar si es usuario con cookie, y su lo es, validar si tiene un codigo de promocion activo, si lo tiene que lo modifique por el nuevo
                                if (usercode2 != null)
                                {
                                var validate = UpdatePromo(code, idUserCookie);
                                result = validate;
                                }
                                else
                                {
                                PromoCodeUsed used = new PromoCodeUsed();
                                used.idUser = idUserCookie;
                                used.PromoCode = code;
                                used.TotalPrice = 0;
                                used.TotalPriceFinal = 0;
                                used.Used = false;
                                used.Points = 0;
                                used.DateUsed = DateTime.Now;
                                db.PromoCodeUsed.Add(used);
                                db.SaveChanges();
                                result = true;
                                }
                        }
                        else
                        {
                            result = false;
                        }

                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool UpdatePromo (string code, string idusercookie)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var registro = db.PromoCodeUsed.Where(s => s.idUser == idusercookie && s.Used == false).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.PromoCode = code;
                        db.Entry(registro).State = EntityState.Modified;
                        db.SaveChanges();

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
