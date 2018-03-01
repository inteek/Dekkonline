using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework;

namespace DekkOnlineMVC.Controllers
{
    public class PromoValidateController : Controller
    {
        // GET: ValidatePromo
        public ActionResult Index()
        {
            bool result = false;
            string code = RouteData.Values["id"] + Request.Url.Query;
            ShoppingCart sh = new ShoppingCart();
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
              result =  sh.confirmPromocode(code, id, idUser);
            }
            else
            {
              result =  sh.confirmPromocode(code, "-1", idUser);
            }

            if (result == true)
            {
                Session["PromoCode"] = code;
                ViewBag.Title = "Valid promotion code";
                ViewBag.Description = "Redirecting to Home...";
            }
            else
            {
                ViewBag.Title = "Promotion code not validated";
                ViewBag.Description = "Redirecting to Home...";
            }
            return View();
        }
    }
}