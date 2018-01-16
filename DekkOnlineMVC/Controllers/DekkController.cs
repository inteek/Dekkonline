using DekkOnlineMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DekkOnlineMVC.Controllers
{
    public class DekkController : Controller
    {


        public enum HeadphoneCardsSortMode { Recomended, Discount, LowPrice, HighPrice };
        
        public ActionResult Products()
        {
            ViewBag.IsCardView = true;
            ViewBag.SortMode = HeadphoneCardsSortMode.Recomended;
            return View(HeadphonesDataProvider.Headphones);
        }
        public ActionResult ProductsPartial(bool? isCardView, HeadphoneCardsSortMode? sortMode)
        {
            ViewBag.IsCardView = isCardView ?? true;
            ViewBag.SortMode = sortMode ?? HeadphoneCardsSortMode.Recomended;
            return PartialView("ProductsPartial", HeadphonesDataProvider.Headphones);
        }


    }
}