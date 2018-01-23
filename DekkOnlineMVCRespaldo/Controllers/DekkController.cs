
using System;
using System.Linq;
using System.Web.Mvc;
using DekkOnlineMVC.Models;
using System.Collections.Generic;

namespace DekkOnlineMVC.Controllers
{
    public class DekkController : Controller
    {
        public enum ProductsCardsSortMode { LowestHighest, Highestlowest };



        public ActionResult Products()
        {
            GetFilters();

            ViewBag.IsCardView = false;
            ViewBag.SortMode = ProductsCardsSortMode.LowestHighest;


            var resultModel = GetProducts(null, null, null, null, null, true, ProductsCardsSortMode.LowestHighest);

            resultModel = resultModel.OrderBy(c => c.Price).ToList();



            return View(resultModel);
        }

        public ActionResult ProductsPartial(int? sizeWidth, int? profile, int? sizeDiameter, int? idCategory, Guid? idBrand, bool? isCardView, ProductsCardsSortMode? sortMode)
        {
            isCardView = isCardView ?? true;
            sortMode = sortMode ?? ProductsCardsSortMode.LowestHighest;
            
            var resultModel = GetProducts(sizeWidth, profile, sizeDiameter, idCategory, idBrand, isCardView, sortMode);
            

            ViewBag.IsCardView = isCardView;
            ViewBag.SortMode = sortMode;

            return PartialView("ProductsPartial", resultModel);
        }


        private void GetFilters() {

            Framework.Articles ar = new Framework.Articles();
            ViewBag.BagWidth = ar.loadDimensionWidth();
            ViewBag.BagProfile = ar.loadDimensionProfile();
            ViewBag.BagDiameter = ar.loadDimensionDiameter();
            ViewBag.BagCategories = ar.loadCategories();
            ViewBag.BagBrands = ar.loadBrands();

        }


        private List<Models.Products> GetProducts(int? sizeWidth, int? profile, int? sizeDiameter, int? idCategory, Guid? idBrand, bool? isCardView, ProductsCardsSortMode? sortMode)
        {

            Framework.Articles articles = new Framework.Articles();
            var articlesResult = articles.loadProducts(idCategory, sizeWidth, profile, sizeDiameter, idBrand);

            var resultModel = (from p in articlesResult
                               select new Models.Products
                               {
                                   Id = p.Id,
                                   Image = p.Image,
                                   CatId = p.CatId,
                                   CategoryImage = GetImageCategory(p.CatId), //p.CategoryImage,
                                   Brand = string.IsNullOrEmpty(p.Brand) ? "GENERAL": p.Brand.ToUpper(),
                                   BrandImage = p.BrandImage,
                                   Name = p.Name,
                                   Width = p.Width,
                                   Profile = p.Profile,
                                   Diameter = p.Diameter,
                                   TyreSize = p.TyreSize,
                                   Fuel = string.IsNullOrEmpty(p.Fuel)? "&emsp;": p.Fuel,
                                   Wet = string.IsNullOrEmpty(p.Wet) ? "&emsp;" : p.Wet,
                                   Noise = string.IsNullOrEmpty(p.Noise) ? "&emsp;" : p.Noise,
                                   Price = p.Price,
                                   Stock = p.Stock
                               }
                               ).ToList();


            if (sortMode == ProductsCardsSortMode.LowestHighest) resultModel = resultModel.OrderBy(c => c.Price).ToList();
            else resultModel = resultModel.OrderByDescending(c => c.Price).ToList();



            return resultModel;

        }


        private string GetImageCategory(int? idCat) {
            switch (idCat) {
                case 1:
                    return "~/Content/imgs/Summer_red-01.png";
                case 2:
                    return "~/Content/imgs/winter_red-01.png";
                case 3:
                    return "~/Content/imgs/moto_red-01.png";
                case 4:
                    return "~/Content/imgs/ATV_red-01.png";
                default:
                    return "~/Content/imgs/Summer_red-01.png"; //"about:blank";
            }
        }


    }
}