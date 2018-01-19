
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

            var resultModel = GetProducts(null, null, null, null, null, true, ProductsCardsSortMode.LowestHighest);

            ViewBag.IsCardView = true;
            ViewBag.SortMode = ProductsCardsSortMode.LowestHighest;
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
                                   CategoryImage = p.CategoryImage,
                                   Brand = p.Brand,
                                   BrandImage = p.BrandImage,
                                   Name = p.Name,
                                   Width = p.Width,
                                   Profile = p.Profile,
                                   Diameter = p.Diameter,
                                   TyreSize = p.TyreSize,
                                   Fuel = p.Fuel,
                                   Wet = p.Wet,
                                   Noise = p.Noise,
                                   Price = p.Price,
                                   Stock = p.Stock
                               }
                               ).ToList();


            if (sortMode == ProductsCardsSortMode.LowestHighest) resultModel = resultModel.OrderBy(c => c.Price).ToList();
            else resultModel = resultModel.OrderByDescending(c => c.Price).ToList();



            return resultModel;

        }


    }
}