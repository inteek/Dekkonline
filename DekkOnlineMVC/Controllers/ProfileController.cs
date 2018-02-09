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

        public PartialViewResult Index(string username, string password)
        {
            Users us = new Users();
            ShoppingCart sh = new ShoppingCart();
            var UserData = (dynamic)null;
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            var pro = (dynamic)null;
            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
                UserData = us.dataUser(id);
                if (UserData != null)
                {
                    foreach (var item in UserData)
                    {
                        pro = new Framework.Libraies.ResultDataUser {
                            IdUser = item.IdUser,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Address = item.Address,
                            Phone = item.Phone,
                            ZipCode = item.ZipCode,
                            Image = item.Image,
                            Email = item.Email
                        };
                    }
                }

            }
            else
            {
                Response.Redirect("~/Home/Index");
            }
            

            return PartialView(pro);

        }

        //public PartialViewResult Index()
        //{
        //    return PartialView();
        //}

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