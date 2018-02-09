﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DekkOnlineMVC.Models;
using Framework;
using Framework.Libraies;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DekkOnlineMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        public static string zip;
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            string path = Request.Url.AbsolutePath;
            ViewBag.ReturnUrl = path;
            var b = (dynamic)null;
            ShoppingCart sh = new ShoppingCart();
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            var pro = (dynamic)null;
            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
                sh.UpdateShoppingCart(id, idUser);
                pro = sh.ProductsInCart(id);//8eb14cb4-c1d5-4e00-94fd-ca458532ac92
            }
            else
            {
                pro = sh.ProductsInCart(idUser);//8eb14cb4-c1d5-4e00-94fd-ca458532ac92
            }


            if (pro != null)
            {
               foreach (var item in pro)
                {
                     b = new Framework.Libraies.ResultAllCart { cart = item.cart, subtotal = item.subtotal, promocode = item.promocode, points = item.points, total = item.total, promocodeapp = item.promocodeapp };
                   
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
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            try
            {

                ShoppingCart sh = new ShoppingCart();
                var a = (dynamic)null;
                if (usuario1 !="")
                {
                    var id = sh.User(usuario1);
                    a = sh.DeleteProductFromCart(idcart, id);
                }
                else
                {
                    a = sh.DeleteProductFromCart(idcart, idUser);
                }
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
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            try
            {
                var a = (dynamic)null;
                ShoppingCart sh = new ShoppingCart();
                if (usuario1 != "")
                {
                    var id = sh.User(usuario1);
                    a = sh.IncreaseProductFromCart(idcart, qty, id);
                }
                else
                {
                        a = sh.IncreaseProductFromCart(idcart, qty, idUser);
                } 
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

        [HttpPost]
        public ActionResult ValidatePromo(string Code)
        {
            var idUser = Security.GetIdUser(this);
            try
            {
                Code = Code.Trim();
                if (Code =="")
                {
                    Code = null;
                }
                if (Code == null || Code == "")
                {
                    return Json(new { Success = false });
                }
                else
                {
                    ShoppingCart sh = new ShoppingCart();
                    var usuario1 = User.Identity.Name;
                    var a = (dynamic)null;
                    if (usuario1 != "")
                    {
                        var id = sh.User(usuario1);
                        a = sh.ValidatePromoCode(Code, id);
                    }
                    else
                    {
                        a = sh.ValidatePromoCode(Code, idUser);
                    }

                    if (a == true)
                    {
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false });
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult UndoPromo(string id1)
        {
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            try
            {
                ShoppingCart sh = new ShoppingCart();
                var a = (dynamic)null;
                if (usuario1 != "")
                {
                    var id = sh.User(usuario1);
                    a = sh.UndoPromoCode(id);
                }
                else
                {
                    a= a = sh.UndoPromoCode(idUser);
                }
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



        public ActionResult Step2()
        {
            string path = Request.Url.AbsolutePath;
            ViewBag.ReturnUrl = path;

            List<ResulUserWorkShop> list = null;
            List<ResultWorkshop> workshop = null;

            Workshop workShop = new Workshop();

            var b = (dynamic)null;

            try
            {
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;

                if (idUser == null || idUser == "")
                {
                    string codeZip = zip;

                    if (codeZip != null)
                    {
                        workshop = workShop.loadWorkshopAddress(Convert.ToInt32(codeZip));

                        b = new Framework.Libraies.ResulUserWorkShop { workshop = workshop, zipcode = Convert.ToInt32(null), firstName = null, lastName = null, address = null, email = null, mobile = null };
                    }
                    else
                    {
                        b = new Framework.Libraies.ResulUserWorkShop { workshop = workshop, zipcode = Convert.ToInt32(null), firstName = null, lastName = null, address = null, email = null, mobile = null };
                    }
                }
                else
                {
                    Users users = new Users();
                    ShoppingCart shoppingCart = new ShoppingCart();

                    //Actualizar la cookie por el idUser
                    var usercookie = Security.GetIdUser(this);
                    shoppingCart.UpdateShoppingCart(idUser, usercookie);

                    list = users.infoStep2(idUser);

                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            b = new Framework.Libraies.ResulUserWorkShop { workshop = item.workshop, zipcode = item.zipcode, firstName = item.firstName, lastName = item.lastName, address = item.address, email = item.email, mobile = item.mobile };
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return View(b);
        }




        [HttpPost]
        public JsonResult loadWorkShop(string zipCode)
        {
            zip = zipCode;
            return Json(new { error = false, noError = 0, msg = "" });
        }


        //public PartialViewResult Step2()
        //{
        //    return PartialView();
        //}

        public ActionResult Step3()
        {
            var b = (dynamic)null;
            Orders sh = new Orders();
            ShoppingCart sh2 = new ShoppingCart();
            var usuario1 = User.Identity.Name;
            if (usuario1 == "")
            {
                return RedirectToAction("Index", "Home");
            }
            var id = sh2.User(usuario1);
            var pro = sh.ObtainProductsConfirmed(id);//8eb14cb4-c1d5-4e00-94fd-ca458532ac92   3f619083-b218-41e8-8693-1a93ecd82fdf
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
                        Comments = item.Comments,
                        Total = item.Total
                    };

                }
                return View(b);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ConfirmPay(string tar, string cn, string edm, string edy, string sc, string chn)
        {
            try
            {
                Orders or = new Orders();
                ShoppingCart sh = new ShoppingCart();
                var usuario1 = User.Identity.Name;
                var a = (dynamic)null;
                if (usuario1 != "")
                {
                    var id = sh.User(usuario1);
                    a = or.addToPurchaseOrder(id, Convert.ToInt32(tar), cn, edm, edy, Convert.ToInt32(sc), chn);
                }
                else
                {
                    a = false;
                }
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

        public ActionResult Step4()
        {
            var b = (dynamic)null;
            Orders sh = new Orders();
            ShoppingCart sh2 = new ShoppingCart();
            var usuario1 = User.Identity.Name;
            if (usuario1 == "")
            {
                return RedirectToAction("Index", "Home");
            }
            var id = sh2.User(usuario1);
            var pro = sh.ObtainProductsPaid(id);//8eb14cb4-c1d5-4e00-94fd-ca458532ac92   3f619083-b218-41e8-8693-1a93ecd82fdf
            if (pro != null)
            {
                foreach (var item in pro)
                {
                    b = new Framework.Libraies.ResultPaidProducts
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
                        Comments = item.Comments,
                        Total = item.Total,
                        TypeTarget = item.TypeTarget,
                        Number = item.Number,
                        Expire = item.Expire,
                        Order = item.Order
                    
                    };

                }
                return View(b);
            }
            else
            {
                return View();
            }

        }

        //[HttpPost]
        //public JsonResult Validate(string user, string pass)
        //{
        //    Users classUser = new Users();
        //    var result = classUser.dataUser(user, pass);

        //    if (result != null && result.Count != 0)
        //    {
        //        return Json(new { error = false, noError = 0, msg = "Sesion iniciada", page = Url.Action("Step2", "ShoppingCart"), resultado = result });
        //    }
        //    else
        //    {
        //        return Json(new { error = true, noError = 0, msg = "Usuario y/o contraseña no validos", page = "" });
        //    }
        //}

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
        public async Task<ActionResult> Next(string zipCode, string firstName, string lastName, string mobile, string address, string email, string choose, string date, string comments, string dateMapa, string timeMapa, string commentsMapa, int IdWorkshop, int radio, string latitude, string longitude)
        {
            bool error = false;
            int noError = 0;
            string msg = "";
            string page = "";
            Users users = new Users();
            Workshop Workshop = new Workshop();

            string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;

            try
            {
                if (idUser == null || idUser == "")
                {
                    string pass = users.GeneratePassword();

                    RegisterViewModel model = new RegisterViewModel();

                    model.Email1 = email;
                    model.Password1 = pass;

                    AccountController account = new AccountController(this);
                    await account.Register(model);


                    error = false;
                    noError = 0;
                    msg = "Registro Exitoso";
                    page = Url.Action("Step3", "ShoppingCart");
                }
                else
                {
                    bool resultUpdate = users.updateAddressUser(idUser, firstName, lastName, address, mobile, Convert.ToInt32(zipCode), latitude, longitude);

                    if (resultUpdate == true)
                    {
                        if (radio == 1)
                        {
                            bool result = Workshop.addDeliveryType(1, idUser, 0, 0, 0, dateMapa, timeMapa, commentsMapa, address);
                            if (result == true)
                            {
                                error = false;
                                noError = 0;
                                msg = "Registro Exitoso";
                                page = Url.Action("Step3", "ShoppingCart");
                            }
                            else
                            {
                                error = true;
                                noError = 1;
                                msg = "Registro incorrecto metodo addDeliveryType";
                                page = "";
                            }
                        }
                    }
                    else
                    {
                        error = true;
                        noError = 1;
                        msg = "Registro incorrecto metodo updateAddressUser";
                        page = "";
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = true, noError = 0, msg = "Error", page = "" });
            }

            return Json(new { error, noError, msg, page });
        }

        [HttpPost]
        public JsonResult MakeApponitment(int fecha, int servicio, string date, string time, string comments, int workshop, int idWorkShop, string address)
        {
            Workshop Workshop = new Workshop();
            string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;

            try
            {
                bool result = Workshop.addDeliveryType(workshop, idUser, idWorkShop, servicio, fecha, date, time, comments, address);

                if (result == true)
                {
                    return Json(new { error = false, noError = 0, msg = "Registro Correcto" });
                }
                else
                {
                    return Json(new { error = true, noError = 0, msg = "Registro Incorrecto" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex, noError = 0, msg = "Registro Incorrecto" });
            }
        }

  public ActionResult CountProductCart()
        {

            List<Framework.Libraies.ResultAllCart> pro;
            ShoppingCart sh = new ShoppingCart();
            string idUser = Security.GetIdUser(this);
            string usuario1 = User.Identity.Name;

            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
                //sh.UpdateShoppingCart(id, idUser);
                pro = sh.ProductsInCart(id);//8eb14cb4-c1d5-4e00-94fd-ca458532ac92
            }
            else
            {
                pro = sh.ProductsInCart(idUser);//8eb14cb4-c1d5-4e00-94fd-ca458532ac92
            }


            return Content(JsonConvert.SerializeObject(pro.Select(x=>x.cart.Sum(y=>y.quantity))), "application/json");
        }

		
    }
}