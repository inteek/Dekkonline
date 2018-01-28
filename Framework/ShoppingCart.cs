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

        //ADD TO CART DE-5 TASK 3
        public bool AddToCart(string idUser, int id_dekk, int id_quantity)
        {
            try
            {

                using (var db = new dekkOnlineEntities())
                {
                    var productPrice = db.products.Where(s => s.proId == id_dekk).Select(x => x.proSuggestedPrice).FirstOrDefault();
                    decimal price = Convert.ToDecimal(productPrice) * id_quantity;

                    Entity.ShoppingCart productShoppingCart = db.ShoppingCart.Where(s => s.proId == id_dekk && s.IdUser.Equals(idUser)).FirstOrDefault();

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
                    else {
                        productShoppingCart.quantity = id_quantity;
                        productShoppingCart.Price = price;
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
        public bool DeleteProductFromCart(string idcart)
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

        public bool IncreaseProductFromCart(string idcart, int qty)
        {
            Framework.encryptdecrypt en = new encryptdecrypt();
            try
            {
                int id = Convert.ToInt32(en.DesEncriptar(idcart));
                using (var db = new dekkOnlineEntities())
                {
                    var d = db.ShoppingCart.Where(x => x.Id == id).FirstOrDefault();
                    var e = db.products.Where(x => x.proId == d.proId).FirstOrDefault();
                    if (e.proInventory > qty)
                    {
                        if (d != null)
                        {
                            d.Price = ((d.Price / d.quantity) * qty);
                            d.quantity = qty;
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (d != null)
                        {
                            d.Price = ((d.Price / d.quantity) * e.proInventory);
                            d.quantity = e.proInventory;
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                return false;
            }

        }
        //PRODUCTS IN CART DE-11 TASK 1 cambios
        public List<ResultAllCart> ProductsInCart(string User)
        {
            List<ResultShoppingCartProduct> products = null;
            List<ResultAllCart> allcart = null;
            try
            {

                using (var db = new dekkOnlineEntities())
                {
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
                                   totalpriceprod = cart.Price,
                                    cartid = cart.Id.ToString()
                                }).ToList();


                    var promocode1 = ""; //LoadPromoCodeFomUser(User);
                    var points1 = 0; //LoadPointsPerUser(User);


                    decimal? subtotal1 = products.Select(p => p.totalpriceprod).Sum();
                    subtotal1 = subtotal1 == null ? 00M : subtotal1;

                    //foreach (var item in products)
                    //{
                    //    subtotal1 += Convert.ToDecimal(item.totalpriceprod);
                    //}

                     allcart = new List<ResultAllCart> {
                        new ResultAllCart{
                        cart = products,
                        subtotal = (decimal)subtotal1,
                        promocode = promocode1,
                        points = points1,
                        total = 0 }
                    };
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
                    promocode = (from p in db.PromotionCodeUser
                                 where p.IdUser == idUser
                                 select (p.IdCode)).FirstOrDefault().ToString();
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
        public string ValidatePromoCode(string code, decimal totalprice, string idUser)
        {
            var promocodediscount = (dynamic)null;
            var promocodevalidate = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    DateTime DateToday = DateTime.Now;
                    promocodediscount = db.PromotionCode.Where(p => p.IdCode == code && p.DateStart <= DateToday && p.DateEnd >= DateToday).Select(p => p.PercentCode).FirstOrDefault();
                    promocodevalidate = db.PurchaseOrder.Where(pv => pv.UsedPromo == code && pv.IdUser == idUser).FirstOrDefault();
                    if (promocodevalidate == null)
                    {
                        if (promocodediscount != null)
                        {
                            promocodediscount = promocodediscount / 100;
                            decimal totalpricepromocode = promocodediscount * totalprice;
                            totalprice = totalprice - totalpricepromocode;
                            return totalprice.ToString();
                        }
                        else
                        {
                            var PromoCodeNoValid = "The promotion code is not valid.";
                            return PromoCodeNoValid;
                        }
                    }
                    else
                    {
                        var PromoAlreadyUsed = "The promotion code was already used";
                        return PromoAlreadyUsed;
                    }
                }
            }
            catch (Exception)
            {

                throw;
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

        //VALIDATE POINTS

    }
}
