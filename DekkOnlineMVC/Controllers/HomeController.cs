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
            ViewBag.BagWidth = ar.loadDimensionWidth();
            ViewBag.BagProfile = ar.loadDimensionProfile();
            ViewBag.BagDiameter = ar.loadDimensionDiameter();
            ViewBag.BagCategories = ar.loadCategories();
           
            return View();
        }

       

        [HttpPost]
        public JsonResult Validate(string user, string pass)
        {
            Users classUser = new Users();
            var result = classUser.validaLogin(user, pass);

            if (result == true)
            {
                return Json(new { error = false, noError = 0, msg = "Sesion iniciada", page = Url.Action("Index", "Home", new { username = user, password = pass }) });
            }
            else
            {
                return Json(new { error = true, noError = 0, msg = "Usuario y/o contraseña no validos", page = "" });
            }
        }
    }
}