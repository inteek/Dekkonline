
using DekkOnlineMVC.Models;
using System.Web.Mvc;



namespace DekkOnlineMVC.Controllers
{
    public class DekkController : Controller
    {
        public enum ProductsCardsSortMode { LowestHighest, Highestlowest };



        public ActionResult Products()
        {
            GetFilters();
            ViewBag.IsCardView = true;
            ViewBag.SortMode = ProductsCardsSortMode.LowestHighest;
            return View(HeadphonesDataProvider.Headphones);
        }

        public ActionResult ProductsPartial(string sizeWidth, string profile, string sizeDiameter, string idCategory, string idBrand, bool? isCardView, ProductsCardsSortMode? sortMode)
        {
            ViewBag.IsCardView = isCardView ?? true;
            ViewBag.SortMode = sortMode ?? ProductsCardsSortMode.LowestHighest;

            return PartialView("ProductsPartial", HeadphonesDataProvider.Headphones);
        }


        private void GetFilters() {

            Framework.Articles ar = new Framework.Articles();
            ViewBag.BagWidth = ar.loadDimensionWidth();
            ViewBag.BagProfile = ar.loadDimensionProfile();
            ViewBag.BagDiameter = ar.loadDimensionDiameter();
            ViewBag.BagCategories = ar.loadCategories();
            ViewBag.BagBrands = ar.loadBrands();

        }

    }
}