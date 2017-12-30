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
                              where (pro.proDimensionWidth.HasValue)
                              select new ResultSize
                              {
                                  Id = pro.proDimensionWidth.Value.ToString(),
                                  Size = pro.proDimensionWidth.Value.ToString()

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
        public List<ResultSize> loadDimensionProfile()
        {
            List<ResultSize> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from pro in db.products
                              where (pro.proDimensionProfile.HasValue)
                              select new ResultSize
                              {
                                  Id = pro.proDimensionProfile.Value.ToString(),
                                  Size = pro.proDimensionProfile.Value.ToString()

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
        public List<ResultSize> loadDimensionDiameter()
        {
            List<ResultSize> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from pro in db.products
                              where (pro.proDimensionDiameter.HasValue)
                              select new ResultSize
                              {
                                  Id = pro.proDimensionDiameter.Value.ToString(),
                                  Size = pro.proDimensionDiameter.Value.ToString()

                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        
        //DE-2 2
        public List<ResultProduct> loadProducts()
        {
            List<ResultProduct> result = null;
            try
            {
                int catId = 0;
                int width = 0;
                int profile = 0;
                int diameter = 0;

                Guid? braId = null;
                using (var db = new dekkOnlineEntities())
                {
                    result = (from pro in db.products
                              where (pro.proStatus == true
                                    && pro.categoriesDP.cdpStatus == true
                                    && (catId == 0 || pro.catId == catId)
                                    && (width == 0 || pro.proDimensionWidth == width)
                                    && (profile == 0 || pro.proDimensionProfile == profile)
                                    && (diameter == 0 || pro.proDimensionDiameter == diameter)
                                    && (braId.HasValue == false || pro.braId == braId))
                              select new ResultProduct
                              {
                                  proId = pro.proId,
                                  proImage = pro.proImage,
                                  categories = pro.categories.catImage,
                                  Brand = pro.brands.braName,
                                  BrandImage = pro.brands.braImage,
                                  proName = pro.proName,
                                  proDimensionWidth = pro.proDimensionWidth,
                                  proDimensionProfile = pro.proDimensionProfile,
                                  proDimensionDiameter = pro.proDimensionDiameter,
                                  proTyreSize = pro.proTyreSize,
                                  proFuel = pro.proFuel,
                                  proWet = pro.proWet,
                                  proNoise = pro.proNoise,
                                  proSuggestedPrice = pro.proSuggestedPrice,
                                  proInventory = pro.proInventory

                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
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


    }
}
