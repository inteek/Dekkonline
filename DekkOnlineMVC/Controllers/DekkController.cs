
using System;
using System.Linq;
using System.Web.Mvc;
using DekkOnlineMVC.Models;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Security;

namespace DekkOnlineMVC.Controllers
{
    public class DekkController : Controller
    {

        public enum ProductsCardsSortMode { LowestHighest, Highestlowest };



        public ActionResult Products()
        {
            string path = Request.Url.AbsolutePath;
            ViewBag.ReturnUrl = path;

            GetFilters();

            ViewBag.IsCardView = false;
            ViewBag.SortMode = ProductsCardsSortMode.LowestHighest;


            var resultModel = GetProducts(null, null, null, null, null, false, ProductsCardsSortMode.LowestHighest);

            resultModel = resultModel.OrderBy(c => c.Price).ToList();

            //resultModel.Clear();

            return View(resultModel);
        }

        public ActionResult ProductsFilters(int? sizeWidth, int? profile, int? sizeDiameter, int? idCategory, Guid? idBrand, bool? isCardView, ProductsCardsSortMode? sortMode)
        {
            GetFilters();

            ViewBag.IsCardView = false;
            ViewBag.SortMode = ProductsCardsSortMode.LowestHighest;


            var resultModel = GetProducts(sizeWidth, profile, sizeDiameter, idCategory, idBrand, false, ProductsCardsSortMode.LowestHighest);

            resultModel = resultModel.OrderBy(c => c.Price).ToList();

            ViewBag.SizeWidth = sizeWidth;
            ViewBag.Profile = profile;
            ViewBag.SizeDiameter = sizeDiameter;
            ViewBag.IdCategory = idCategory;
            ViewBag.IdBrand = idBrand;


            return View("Products", resultModel);
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
                                   CategoryName = GetNameCategory(p.CatId), //p.CategoryName,
                                   Brand = string.IsNullOrEmpty(p.Brand) ? "GENERAL": p.Brand.ToUpper(),
                                   BrandImage = p.BrandImage,
                                   Name = p.Name,
                                   Description = p.Description,
                                   Width = p.Width,
                                   Profile = p.Profile,
                                   Diameter = p.Diameter,
                                   TyreSize = p.TyreSize,
                                   Fuel = string.IsNullOrEmpty(p.Fuel)? "&emsp;": p.Fuel,
                                   Wet = string.IsNullOrEmpty(p.Wet) ? "&emsp;" : p.Wet,
                                   Noise = string.IsNullOrEmpty(p.Noise) ? "&emsp;" : p.Noise,
                                   Price = p.Price,
                                   Stock = p.Stock,
                                   SpeedIndex = p.SpeedIndex,
                                   LoadIndex = p.LoadIndex
                               }
                               ).ToList();


            if (sortMode == ProductsCardsSortMode.LowestHighest) resultModel = resultModel.OrderBy(c => c.Price).ToList();
            else resultModel = resultModel.OrderByDescending(c => c.Price).ToList();

           


            return resultModel;

        }


        [HttpPost]
        public ActionResult AddToCart(int idPro, int quantity)
        {
            Framework.ShoppingCart shoppingCart = new Framework.ShoppingCart();


            dynamic product = new JObject();
            product.IdPro = idPro;
            product.Quantity = quantity;


            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            if (usuario1 == "")
            {
                product.Success = shoppingCart.AddToCart(idUser, idPro, quantity);
            }
            else
            {
                var id = shoppingCart.User(usuario1);
                if (id != null)
                {
                    product.Success = shoppingCart.AddToCart(id, idPro, quantity);
                    shoppingCart.UpdateShoppingCart(id, idUser);
                    
                }
               
            }
            

            return Content(JsonConvert.SerializeObject(product), "application/json");
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
        
        private string GetNameCategory(int? idCat)
        {
            switch (idCat)
            {
                case 1:
                    return "summer";
                case 2:
                    return "winter";
                case 3:
                    return "motors";
                case 4:
                    return "ATV";
                default:
                    return "summer"; //"about:blank";
            }
        }




    }
}