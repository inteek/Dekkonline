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

        public ActionResult Index()
        {
            string path = Request.Url.AbsolutePath;
            ViewBag.ReturnUrl = path;
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
            

            return View(pro);

        }

        //public PartialViewResult Index()
        //{
        //    return PartialView();
        //}

        public PartialViewResult Pending()
        {
            Orders or = new Orders();
            ShoppingCart sh = new ShoppingCart();
            var pending = (dynamic)null;
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            var pro = (dynamic)null;
            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
                pending = or.loadOrderPending(id);
                if (pending != null)
                {
                    foreach (var item in pending)
                    {
                        pro = new Framework.Libraies.ResultUserOrder
                        {
                            product = item.product,
                        user = item.user
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

        public PartialViewResult Past()
        {
            Orders or = new Orders();
            ShoppingCart sh = new ShoppingCart();
            var pending = (dynamic)null;
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            var pro = (dynamic)null;
            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
                pending = or.loadOrderPast(id);
                if (pending != null)
                {
                    foreach (var item in pending)
                    {
                        pro = new Framework.Libraies.ResultUserOrder
                        {
                            product = item.product,
                            user = item.user
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

        public PartialViewResult Promos()
        {
            return PartialView();
        }

        //public JsonResult ValidateEmail()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //public JsonResult UpdateUserData(string zipcore, string name, string lastname, string address, string email, string mobile)
        //{
        //    try
        //    {
        //        string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
        //        if (idUser != null && idUser != "")
        //        {

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
	}
}