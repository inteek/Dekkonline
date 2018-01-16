using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DekkOnlineMVC.Controllers
{
    public class DekkController : Controller
    {
        // GET: Dekk
        public ActionResult Products()
        {
            return View();
        }
    }
}