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
    public class Articles
    {
        //DE-2 1
        public List<ResultBrands> loadBrands()
        {
            List<ResultBrands> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from bra in db.brands
                              where (bra.products.Count > 0)
                              orderby bra.braName
                              select new ResultBrands
                              {
                                  braId = bra.braId,
                                  braName = bra.braName

                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        //DE-2 1
        public List<ResultCategories> loadType()
        {
            List<ResultCategories> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from cat in db.categories
                              where (cat.catStatus == true)
                              select new ResultCategories
                              {
                                  catId = cat.catId,
                                  catName = cat.catName.ToUpper()

                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        //DE-2 1
        public List<ResultSize> loadDimensionWidth()
        {
            List<ResultSize> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from pro in db.products
                              where (pro.proDimensionWidth.HasValue) && pro.proInventory != 0
                              select new ResultSize
                              {
                                  Id = pro.proDimensionWidth.Value.ToString(),
                                  Size = pro.proDimensionWidth.Value.ToString()

                              }).Distinct().OrderBy(c => c.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        //DE-2 1
        public List<ResultSize> loadDimensionProfile()
        {
            List<ResultSize> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from pro in db.products
                              where (pro.proDimensionProfile.HasValue) && pro.proInventory != 0
                              select new ResultSize
                              {
                                  Id = pro.proDimensionProfile.Value.ToString(),
                                  Size = pro.proDimensionProfile.Value.ToString()

                              }).Distinct().OrderBy(c => c.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        //DE-2 1
        public List<ResultSize> loadDimensionDiameter()
        {
            List<ResultSize> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from pro in db.products
                              where (pro.proDimensionDiameter.HasValue) && pro.proInventory != 0
                              select new ResultSize
                              {
                                  Id = pro.proDimensionDiameter.Value.ToString(),
                                  Size = pro.proDimensionDiameter.Value.ToString()

                              }).Distinct().OrderBy(c => c.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        //DE-2 2
        public List<ResultProduct> loadProducts(int? catId, int? width, int? profile, int? diameter, Guid? braId)
        {
            List<ResultProduct> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from pro in db.products
                              where (pro.proStatus == true
                                    //&& pro.categoriesDP.cdpStatus == true
                                    && (catId == null || pro.catId == catId)
                                    && (width == null || pro.proDimensionWidth == width)
                                    && (profile == null || pro.proDimensionProfile == profile)
                                    && (diameter == null || pro.proDimensionDiameter == diameter)
                                    && (braId.HasValue == false || pro.braId == braId))
                                    && (!pro.proNameDP.ToUpper().Contains("TEST"))
                                    && (!pro.proNameDP.ToUpper().Contains("TESET"))
                                    //&& pro.catId != null
                              //&& (!pro.proNameDP.Contains("Test"))
                              select new ResultProduct
                              {
                                  Id = pro.proId,
                                  Image = pro.proImage,
                                  CatId = pro.categories.catId,
                                  CategoryImage = pro.categories.catImage,
                                  CategoryName = pro.categories.catName,
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
                                  //Price = pro.proSuggestedPrice,
                                  Price = pro.proSuggestedPrice != null ? (int)Math.Floor((decimal)pro.proSuggestedPrice) : 0,
                                  Stock = pro.proInventory,
                                  SpeedIndex = pro.proSpeed,
                                  LoadIndex = pro.proLoadIndex
                              }).ToList();



                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        public string FormatPrice(string strPrice)
        {
            if (strPrice == null)
            {
                return "0";
            }
            else {
                strPrice = strPrice.Replace(strPrice.Substring(strPrice.IndexOf('.')), "");
                return strPrice;
            }
        }

        //LOAD SYZES PER DEKK DE-5 TASK 1 TASK 2
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
                                  Id = x.proId,
                                  Profile = x.proDimensionProfile,
                                  Width = x.proDimensionWidth,
                                  Diameter = x.proDimensionDiameter,
                                  Stock = x.proInventory

                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        //Load Categories
        public List<ResultCategories> loadCategories()
        {
            List<ResultCategories> Categories = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    Categories = (from cat in db.categories
                                  //where (cat.products.Count > 0)
                                  orderby cat.catName
                                  select new ResultCategories
                                  {
                                      catId = cat.catId,
                                      catName = cat.catName,
                                      catDescription = cat.catDescription,
                                      catStatus = cat.catStatus,
                                      catImage = cat.catImage

                                  }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Categories;
        }

    }
}
