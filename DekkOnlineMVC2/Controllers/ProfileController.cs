using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework;

namespace DekkOnlineMVC.Controllers
{
    public class ProfileController : Controller
    {
        // GET: /Profile/

        //public PartialViewResult Index(string username, string password)
        //{
        //    Users us = new Users();

        //    var Userdata = us.dataUser(username, password);

        //    return PartialView(Userdata);

        //}

        public PartialViewResult Index()
        {
            return PartialView();
        }

        public PartialViewResult Pending()
        {
            return PartialView();
        }

        public PartialViewResult Past()
        {
            return PartialView();
        }

        public PartialViewResult Promos()
        {
            return PartialView();
        }
	}
}