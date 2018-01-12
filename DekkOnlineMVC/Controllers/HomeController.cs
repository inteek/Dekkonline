using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework;

namespace DekkOnlineMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Articles ar = new Articles();
            ViewBag.BagCategories = new SelectList(ar.loadCategories(), "catId", "catName");
            ViewBag.BagWidth = new SelectList(ar.loadDimensionWidth().OrderBy(c => Convert.ToInt32(c.Id)), "Size", "Size");
            ViewBag.BagProfile = new SelectList(ar.loadDimensionProfile().OrderBy(c => Convert.ToInt32(c.Id)), "Size", "Size");
            ViewBag.BagDiameter = new SelectList(ar.loadDimensionDiameter().OrderBy(c => Convert.ToInt32(c.Id)), "Size", "Size");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult Validate(string user, string pass)
        {
            Users classUser = new Users();
            var result = classUser.validaLogin(user, pass);

            if (result == true)
            {
                return Json(new { error = false, noError = 0, msg = "Sesion iniciada", page = Url.Action("Index", "Profile") });
            }
            else
            {
                return Json(new { error = true, noError = 0, msg = "Usuario y/o contraseña no validos", page = "" });
            }
        }
    }
}