using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Framework.Libraies;

namespace Framework
{
    public class Articles
    {
        public void loadBrands()
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var bran = (from bra in db.brands where bra.products.Count > 0 select bra);
                }
            }
            catch (Exception)
            {                
                throw;
            }
            
        }

        public void loadType()
        {
            try
            {
                using(var db = new dekkOnlineEntities())
                {
                    var cats = (from cat in db.categories where cat.catStatus == true select new { catId = cat.catId, catName = cat.catName.ToUpper() });
                }
            }
            catch (Exception)
            {                
                throw;
            }            
        }

        public void loadSize()
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var ws = from pro in db.products where pro.proDimensionWidth.HasValue select new { Id = pro.proDimensionWidth.Value.ToString(), Size = pro.proDimensionWidth.Value.ToString() };
                    var ps = from pro in db.products where pro.proDimensionProfile.HasValue select new { Id = pro.proDimensionProfile.Value.ToString(), Size = pro.proDimensionProfile.Value.ToString() };
                    var ds = from pro in db.products where pro.proDimensionDiameter.HasValue select new { Id = pro.proDimensionDiameter.Value.ToString(), Size = pro.proDimensionDiameter.Value.ToString() };     
                }
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public void loadProducts()
        {
            int catId = 0;
            int width = 0;
            int profile = 0;
            int diameter = 0;
           
            Guid? braId = null;

            try
            {
                using (var db = new dekkOnlineEntities())
                {                 
                    var products = (from pro in db.products
                                    where pro.proStatus == true
                                    && pro.categoriesDP.cdpStatus == true
                                    && (catId == 0 || pro.catId == catId)
                                    && (width == 0 || pro.proDimensionWidth == width)
                                    && (profile == 0 || pro.proDimensionProfile == profile)
                                    && (diameter == 0 || pro.proDimensionDiameter == diameter)
                                    && (braId.HasValue == false || pro.braId == braId)
                                    select new
                                    {
                                        Id = pro.proId,
                                        Image = pro.proImage,
                                        CategoryImage = pro.categories.catImage,
                                        Brand = pro.brands.braName,
                                        BrandImage = pro.brands.braImage,
                                        Name = pro.proName,
                                        Width = pro.proDimensionWidth,
                                        Profile = pro.proDimensionProfile,
                                        Diameter = pro.proDimensionDiameter,
                                        TyreSize = pro.proTyreSize,
                                        Fuel = pro.proFuel,
                                        Wet = pro.proWet,
                                        Noise = pro.proNoise,
                                        Price = pro.proSuggestedPrice,
                                        Stock = pro.proInventory
                                    });                    
                }
            }
            catch (Exception)
            {
                throw;
            }                       
        }

        //LOAD SYZES PER DEKK
        public List<ResultProduct> SizesperDekk(string dekk)
        {
            List<ResultProduct> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from x in db.products
                              where (x.proName == dekk && x.proInventory > 0)
                              select new ResultProduct
                              {
                                  proId = x.proId,
                                  proDimensionProfile = x.proDimensionProfile,
                                  proDimensionWidth = x.proDimensionWidth,
                                  proDimensionDiameter = x.proDimensionDiameter,
                                  proInventory = x.proInventory

                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        //ADD TO CART
        public bool AddToCart(string User, int id_dekk, int id_quantity)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {

                    var addCart = new ShoppingCart();
                    addCart.IdUser = User;
                    addCart.proId = id_dekk;
                    addCart.quantity = id_quantity;
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

        //DELETE FROM CART
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

        //PRODUCTS IN CART
        public void ProductsInCart(string User)
        {
            try
            {

                using (var db = new dekkOnlineEntities())
                {
                    var products = (from pro in db.products
                                    join
                                        cart in db.ShoppingCart on pro.proId equals cart.proId
                                    where cart.IdUser == User
                                    select new
                                    {
                                        Id = pro.proId,
                                        Image = pro.proImage,
                                        Name = pro.proName,
                                        Description = pro.proDescription,
                                        Price = pro.proSuggestedPrice,
                                        Stock = pro.proInventory
                                    });
                }

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
