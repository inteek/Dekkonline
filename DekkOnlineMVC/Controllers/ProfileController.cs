using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DekkOnlineMVC.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        public PartialViewResult Index()
        {
            return PartialView();
        }
	}
}