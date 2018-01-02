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
        public bool AddToCart(string User, int id_dekk, int id_quantity)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var product = db.products.Where(s => s.proId == id_dekk).Select(x => x.proSuggestedPrice).FirstOrDefault();
                    decimal price = Convert.ToDecimal(product) * id_quantity;
                    var addCart = new Entity.ShoppingCart();
                    addCart.IdUser = User;
                    addCart.proId = id_dekk;
                    addCart.quantity = id_quantity;
                    addCart.Price = price;
                    addCart.Status = true;
                    db.ShoppingCart.Add(addCart);
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
        public bool DeleteProductFromCart(int idcart)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var addCart = new ShoppingCart();
                    var d = db.ShoppingCart.Where(x => x.Id == idcart).FirstOrDefault();
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

        //PRODUCTS IN CART DE-11 TASK 1
        public List<ResultProduct> ProductsInCart(string User)
        {
            List<ResultProduct> products = null;
            try
            {

                using (var db = new dekkOnlineEntities())
                {
                    products = (from pro in db.products
                                join
                                    cart in db.ShoppingCart on pro.proId equals cart.proId
                                where cart.IdUser == User
                                select new ResultProduct
                                {
                                    proId = pro.proId,
                                    proImage = pro.proImage,
                                    proName = pro.proName,
                                    proDescription = pro.proDescription,
                                    proSuggestedPrice = pro.proSuggestedPrice,
                                    proInventory = pro.proInventory
                                }).ToList();
                }

            }
            catch (Exception)
            {

                return products;
            }
            return products;
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
                                 select new
                                 {
                                     IdCode = p.IdCode
                                 }).FirstOrDefault().ToString();
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
                        return NowPointsLoad;
                    }
                    else
                    {
                        UserPoints.Points = Convert.ToInt32(NowPointsLoad);
                        db.Entry(UserPoints).State = EntityState.Modified;
                        db.SaveChanges();
                        return NowPointsLoad;

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
