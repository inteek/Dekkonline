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

        public bool IncreaseProductFromCart(string idcart, int qty, string idUser)
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
                        }
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


        //PRODUCTS IN CART DE-11 TASK 1 cambios
        public List<ResultAllCart> ProductsInCart(string User)//
        {

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
                                    totalpriceprod = Math.Round((double)cart.Price, 2),
                                    cartid = cart.Id.ToString()
                                }).ToList();


                    //var promocode1 = ""; //LoadPromoCodeFomUser(User);
                    var promocode1 = LoadPromoCodeFomUser(User);
                    var points1 = 0; //LoadPointsPerUser(User);

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
                        allcart = new List<ResultAllCart> {
                        new ResultAllCart{
                        cart = products,
                        subtotal = (decimal)subtotal1,
                        promocode = promocode1,
                        points = points1,
                        total = (decimal)subtotal1,
                        promocodeapp = false} };
                    }
                    else
                    {
                        allcart = new List<ResultAllCart> {
                        new ResultAllCart{
                        cart = products,
                        subtotal = (decimal)subtotal1,
                        promocode = promocodeused.PromoCode,
                        points = points1,
                        total = (decimal)promocodeused.TotalPriceFinal,
                        promocodeapp = true} };

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
        public string LoadPromoCodeFomUser(string idUser)
        {
            var promocode = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    promocode = db.PromotionCodeUser.Where(s => s.IdUser == idUser).Select(s => s.IdCode).FirstOrDefault();
                    if (promocode != null)
                    {
                        return promocode;
                    }
                    else
                    {
                        return promocode = "";
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        //VALIDATE PROMO CODE DE-11 cambios
        public bool ValidatePromoCode(string code, string idUser)
        {
            var promocodediscount = (dynamic)null;
            List<ResultShoppingCartProduct> products = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    DateTime DateToday = DateTime.Now;
                    promocodediscount = db.PromotionCode.Where(p => p.IdCode == code && p.DateStart <= DateToday && p.DateEnd >= DateToday).Select(p => p.PercentCode).FirstOrDefault();
                    if (promocodediscount != null)
                    {
                        var usercode = db.PromoCodeUsed.Where(s => s.idUser == idUser && s.PromoCode == code && s.Used == true).FirstOrDefault();
                        if (usercode != null)
                        {
                            return false;
                        }
                        products = (from pro in db.products
                                    join cart in db.ShoppingCart on pro.proId equals cart.proId
                                    where cart.IdUser.Equals(idUser) && cart.Status == false
                                    select new ResultShoppingCartProduct
                                    {
                                        totalpriceprod = Math.Round((double)cart.Price, 2),
                                    }).ToList();
                        double? totalprice = products.Select(p => p.totalpriceprod).Sum();
                        double? totalprice2 = products.Select(p => p.totalpriceprod).Sum();
                        promocodediscount = promocodediscount / 100;
                        decimal totalpricepromocode = promocodediscount * (decimal)totalprice;
                        totalprice = totalprice - (double)totalpricepromocode;
                        Entity.PromoCodeUsed addpromo = new Entity.PromoCodeUsed();
                        addpromo.idUser = idUser;
                        addpromo.PromoCode = code;
                        addpromo.TotalPrice = (decimal)totalprice2;
                        addpromo.TotalPriceFinal = (decimal)totalprice;
                        addpromo.Used = false;
                        db.PromoCodeUsed.Add(addpromo);
                        db.SaveChanges();
                        return true;
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
                    NowPointsLoad = db.DetailUserPoints.Where(s => s.IdUser == idUser && s.StatusofPromo == true).Select(s => s.PointsEarned).Sum().ToString();

                    if (NowPointsLoad == null || NowPointsLoad == "")
                    {
                        NowPointsLoad = 0;
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
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public bool UpdateShoppingCart(string IdUser1, string idUserCookies)
        {
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

                    var promocodeusedus = db.PromoCodeUsed.Where(s => s.idUser == IdUser1 && s.Used == false).FirstOrDefault();
                    if (promocodeusedck != null)
                    {
                        price = 0;
                        var percent = db.PromotionCode.Where(s => s.IdCode == promocodeusedck.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                        var user1 = db.ShoppingCart.Where(s => s.IdUser == IdUser1 && s.Status == false).ToList();
                        foreach (var item in user1)
                        {
                            price = item.Price;
                        }
                        promocodeusedck.idUser = IdUser1;
                        percent = percent / 100;
                        promocodeusedck.TotalPrice = promocodeusedck.TotalPrice + price;
                        promocodeusedck.TotalPriceFinal = promocodeusedck.TotalPrice - (percent * promocodeusedck.TotalPrice);
                        if (promocodeusedus != null)
                        {
                            db.PromoCodeUsed.Remove(promocodeusedus);

                        }
                    }
                   else if (promocodeusedus != null)
                    {
                        var percent = db.PromotionCode.Where(s => s.IdCode == promocodeusedus.PromoCode).Select(s => s.PercentCode).FirstOrDefault();
                        percent = percent / 100;
                        promocodeusedus.TotalPrice = promocodeusedus.TotalPrice + price;
                        promocodeusedus.TotalPriceFinal = promocodeusedus.TotalPrice - (percent * promocodeusedus.TotalPrice);
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
        //VALIDATE POINTS

    }
}
