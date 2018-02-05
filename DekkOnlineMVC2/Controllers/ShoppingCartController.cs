using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DekkOnlineMVC.Models;
using Framework;

namespace DekkOnlineMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var b = (dynamic)null;

            var idUser = Security.GetIdUser(this);

            ShoppingCart sh = new ShoppingCart();
          var pro =  sh.ProductsInCart(idUser);//8eb14cb4-c1d5-4e00-94fd-ca458532ac92
            if (pro != null)
            {
               foreach (var item in pro)
                {
                     b = new Framework.Libraies.ResultAllCart { cart = item.cart, subtotal = item.subtotal, promocode = item.promocode, points = item.points, total = item.total };
                   
                }
                return View(b);
            }
            else
            {
                return View();
            }
           
            
            
        }

        [HttpPost]
        public ActionResult DeleteFromCart(string idcart)
        {
            try
            {
                ShoppingCart sh = new ShoppingCart();
                var a = sh.DeleteProductFromCart(idcart);
                if (a == true)
                {
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false });
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult IncreaseProductFromCart(string idcart, int qty)
        {
            try
            {
                ShoppingCart sh = new ShoppingCart();
                var a = sh.IncreaseProductFromCart(idcart, qty);
                if (a == true)
                {
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { error = false });
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public ActionResult Step2()
        {
            Workshop workshop = new Workshop();
            var work = (dynamic)null;

            try
            {
                work = workshop.loadWorkshopAddress(1100);
            }
            catch (Exception)
            {

                throw;
            }

            return View(work);
        }

        //public PartialViewResult Step2()
        //{
        //    return PartialView();
        //}

        public ActionResult Step3()
        {
            var b = (dynamic)null;
            Orders sh = new Orders();
            var pro = sh.ObtainProductsConfirmed("3f619083-b218-41e8-8693-1a93ecd82fdf");//8eb14cb4-c1d5-4e00-94fd-ca458532ac92
            if (pro != null)
            {
                foreach (var item in pro)
                {
                    b = new Framework.Libraies.ResultProductsConfirmation
                    {
                        cart = item.cart,
                        ZipCode = item.ZipCode.ToString(),
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Address = item.Address,
                        Email = item.Email,
                        Mobile = item.Mobile,
                        Promo = item.Promo,
                        WorkshopName = item.WorkshopName,
                        WorkshopAddress = item.WorkshopAddress,
                        Image = item.Image,
                        Rating = "as",
                        Date = item.Date.ToString(),
                        Time = item.Time,
                        Comments = item.Comments
                    };

                }
                return View(b);
            }
            else
            {
                return View();
            }
        }

        public PartialViewResult Step4()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Validate(string user, string pass)
        {
            Users classUser = new Users();
            var result = classUser.dataUser(user, pass);

            if (result != null && result.Count != 0)
            {
                return Json(new { error = false, noError = 0, msg = "Sesion iniciada", page = Url.Action("Step2", "ShoppingCart"), resultado = result });
            }
            else
            {
                return Json(new { error = true, noError = 0, msg = "Usuario y/o contraseña no validos", page = "" });
            }
        }

        [HttpPost]
        public JsonResult NewUser(string idUser, string firstName, string lastName, string address, string email, string phone, int zipCode, string latitude, string length)
        {
            Users users = new Users();

            users.addNetUser(email);

            users.addToUserAddress(idUser, firstName, lastName, address, phone, zipCode, latitude, length);

            //Cookies
            encryptdecrypt en = new encryptdecrypt();
            var cookie = Request.QueryString["UserInfo"];
            var descookie = en.DesEncriptar(cookie);



            //if (result != null && result.Count != 0)
            //{
            //    return Json(new { error = false, noError = 0, msg = "Sesion iniciada", page = Url.Action("Step2", "ShoppingCart"), resultado = result });
            //}
            //else
            //{
            //    return Json(new { error = true, noError = 0, msg = "Usuario y/o contraseña no validos", page = "" });
            //}
            return Json(new { error = true });
        }

        [HttpPost]
        public ActionResult Next(string zipCode, string firstName, string lastName, string address, string email, string choose, string date, string comments)
        {
            try
            {
                Users users = new Users();
                string pass = users.CreateRandomPassword(7);

                RegisterViewModel model = new RegisterViewModel();

                model.UserName = email;
                model.Password = pass;

                AccountController account = new AccountController();

                account.Register(model);

                return Json(new { error = false, noError = 0, msg = "Registro exitoso", page = Url.Action("Step3", "ShoppingCart") });

            }
            catch (Exception)
            {
                return Json(new { error = true, noError = 0, msg = "Error", page = "" });
            }
        }
        }
}