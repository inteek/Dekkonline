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

            DekkOnline2.engine2.dekkpro.updateProductStock(1);




            ViewBag.ReturnUrl = "/Profile/Index";
            Articles ar = new Articles();
            ViewBag.BagWidth = ar.loadDimensionWidth();
            ViewBag.BagProfile = ar.loadDimensionProfile();
            ViewBag.BagDiameter = ar.loadDimensionDiameter();
            ViewBag.BagCategories = ar.loadCategories();
            

            ViewBag.SizeWidth = System.Configuration.ConfigurationManager.AppSettings["SizeWidth"];
            ViewBag.Profile = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            ViewBag.SizeDiameter = System.Configuration.ConfigurationManager.AppSettings["SizeDiameter"];


            if (DateTime.Now.Month == 11 || DateTime.Now.Month == 12 || DateTime.Now.Month == 1 || DateTime.Now.Month == 2) {
                //ViewBag.IdCategory = 2; //WINTER
                ViewBag.IdCategory = 1;
            }
            else{
                ViewBag.IdCategory = 1; //SOMMERDEKK
            }

            //ViewBag.IdBagCategories = System.Configuration.ConfigurationManager.AppSettings["SizeWidth"];

            
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