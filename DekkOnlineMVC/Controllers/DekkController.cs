
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
        const int rowsPageSize = 10;

        public enum ProductsCardsSortMode { LowestHighest, Highestlowest };



        public ActionResult Products()
        {
            string path = Request.Url.AbsolutePath;
            ViewBag.ReturnUrl = path;

            GetFilters();

            //ViewBag.IsCardView = false;
            //ViewBag.SortMode = ProductsCardsSortMode.LowestHighest;
            ViewBag.RowsPerPage = rowsPageSize;
            ViewBagSessionSearch();

            List<Models.Products> resultModel = GetProducts(ViewBag.SizeWidth, ViewBag.Profile, ViewBag.SizeDiameter, ViewBag.IdCategory, ViewBag.IdBrand, (bool)ViewBag.IsCardView, (ProductsCardsSortMode)ViewBag.SortMode);

           

            return View(resultModel);
        }

        public ActionResult ProductsFilters(int? sizeWidth, int? profile, int? sizeDiameter, int? idCategory, Guid? idBrand, bool? isCardView, ProductsCardsSortMode? sortMode)
        {
            GetFilters();
            ViewBag.RowsPerPage = rowsPageSize;

            ViewBag.IsCardView = false;
            ViewBag.SortMode = ProductsCardsSortMode.LowestHighest;
            ViewBag.SizeWidth = sizeWidth;
            ViewBag.Profile = profile;
            ViewBag.SizeDiameter = sizeDiameter;
            ViewBag.IdCategory = idCategory;
            ViewBag.IdBrand = idBrand;


            List<Models.Products> resultModel = GetProducts(sizeWidth, profile, sizeDiameter, idCategory, idBrand, false, ProductsCardsSortMode.LowestHighest);

            resultModel = resultModel.OrderBy(c => c.Price).ToList();
            



            return View("Products", resultModel);
        }

        public ActionResult ProductsPartial(int? sizeWidth, int? profile, int? sizeDiameter, int? idCategory, Guid? idBrand, bool? isCardView, ProductsCardsSortMode? sortMode)
        {
            isCardView = isCardView ?? true;
            sortMode = sortMode ?? ProductsCardsSortMode.LowestHighest;

            List<Models.Products> resultModel = GetProducts(sizeWidth, profile, sizeDiameter, idCategory, idBrand, isCardView, sortMode);
            

            ViewBag.IsCardView = isCardView;
            ViewBag.SortMode = sortMode;
            ViewBag.RowsPerPage = rowsPageSize;


            Session["IsCardView"] = isCardView;
            Session["SortMode"] = sortMode;
            Session["SizeWidth"] = sizeWidth;
            Session["Profile"] = profile;
            Session["SizeDiameter"] = sizeDiameter;
            Session["IdCategory"] = idCategory;
            Session["IdBrand"] = idBrand;

          
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


        private List<Models.Products> GetProducts(int? sizeWidth, int? profile, int? sizeDiameter, int? idCategory, Guid? idBrand, bool? isCardView, ProductsCardsSortMode? sortMode)
        {

            Framework.Articles articles = new Framework.Articles();
            var articlesResult = articles.loadProducts(idCategory, sizeWidth, profile, sizeDiameter, idBrand);

            var resultModel = (from p in articlesResult
                               select new Models.Products
                               {
                                   Id = p.Id,
                                   Image = validaImage(p.Image),
                                   CatId = p.CatId,
                                   CategoryImage = GetImageCategory(p.CatId), //p.CategoryImage,
                                   CategoryName = GetNameCategory(p.CatId), //p.CategoryName,
                                   Brand = string.IsNullOrEmpty(p.Brand) ? "GENERAL": p.Brand.ToUpper(),
                                   BrandImage = validaImage(p.BrandImage),
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

        private string validaImage(string image) {

            if (string.IsNullOrEmpty(image))
            {
               return System.Configuration.ConfigurationManager.AppSettings["UrlAdmin"] + "photos/noPhoto.png";
            }
            else {
                return System.Configuration.ConfigurationManager.AppSettings["UrlAdmin"] + image;
            }
            
        }

        private void ViewBagSessionSearch() {

            if (Session["SizeWidth"] != null)
            {
                ViewBag.SizeWidth = (int)Session["SizeWidth"];
            }
            else {
                ViewBag.SizeWidth = null;
            }

            if (Session["Profile"] != null)
            {
                ViewBag.Profile = (int)Session["Profile"];
            }
            else
            {
                ViewBag.Profile = null;
            }


            if (Session["SizeDiameter"] != null)
            {
                ViewBag.SizeDiameter = (int)Session["SizeDiameter"];
            }
            else
            {
                ViewBag.SizeDiameter = null;
            }


            if (Session["IdCategory"] != null)
            {
                ViewBag.IdCategory = (int)Session["IdCategory"];
            }
            else
            {
                ViewBag.IdCategory = null;
            }


            if (Session["IdBrand"] != null)
            {
                ViewBag.IdBrand = (Guid)Session["IdBrand"];
            }
            else
            {
                ViewBag.IdBrand = null;
            }


            if (Session["IsCardView"] != null)
            {
                ViewBag.IsCardView = (bool)Session["IsCardView"];
            }
            else
            {
                ViewBag.IsCardView = false;
            }

            if (Session["SortMode"] != null)
            {
                ViewBag.SortMode = (ProductsCardsSortMode)Session["SortMode"];
            }
            else
            {
                ViewBag.SortMode = ProductsCardsSortMode.LowestHighest; ;
            }


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