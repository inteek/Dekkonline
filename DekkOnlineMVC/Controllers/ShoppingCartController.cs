using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DekkOnlineMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Step2()
        {
            return PartialView();
        }

        public PartialViewResult Step3()
        {
            return PartialView();
        }
	}
}