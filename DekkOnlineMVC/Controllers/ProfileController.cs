using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using Framework;
using Framework.Libraies;
using static DekkOnlineMVC.Controllers.ManageController;

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
                var userworkshop = us.ValidateUserworkshop(id);
                if (userworkshop == true)
                {
                    return RedirectToAction("Index", "Workshop");
                }
                else {
                UserData = us.dataUser(id);
                if (UserData != null)
                {
                    foreach (var item in UserData)
                    {
                        pro = new Framework.Libraies.ResultDataUser
                        {
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
            }
            else
            {
                return View();
            }


            return View(pro);

        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return RedirectToAction("Index", new { Message = "Upload Failed" });
            }
            ShoppingCart sh = new ShoppingCart();
            Users us = new Users();
            var usuario1 = User.Identity.Name;
            var id = sh.User(usuario1);
            if (ModelState.IsValid)
            {
                var originalFilename = Path.GetFileName(file.FileName);
                string fileId = Guid.NewGuid().ToString().Replace("-", "");
                string userId = id;
                if (originalFilename.ToLower().EndsWith(".png") || originalFilename.ToLower().EndsWith(".jpg"))
                {
                    //var path = Path.Combine(Server.MapPath("~/Content/Uploads/Photo/"), userId, fileId);
                    //file.SaveAs(path);
                    var fnm = fileId + ".png";
                    var filePath = HostingEnvironment.MapPath("~/Content/Uploads/Photo/") + fnm;
                    var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/Uploads/Photo/"));
                    if (directory.Exists == false)
                    {
                        directory.Create();
                    }
                    ViewBag.FilePath = filePath.ToString();
                    file.SaveAs(filePath);
                    var userupdate = us.UpdateUserImage(filePath, id);
                    if (userupdate != null || userupdate.Length > 1)
                    {
                        return Json(new { userupdate, JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { Message = "Upload Failed" });
                        //return Json(new { x, JsonRequestBehavior.AllowGet });
                    }

                }
                else
                {
                    return RedirectToAction("Index", new { Message = "Upload Success" });
                }
            }
            return Json(new { Success = true });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile()
        {
            ShoppingCart sh = new ShoppingCart();
            Users us = new Users();
            var usuario1 = User.Identity.Name;
            var id = sh.User(usuario1);

            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);
                    if (fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".jpg"))
                    {

                    }
                    else
                    {
                        return RedirectToAction("Index", new { Message = "Upload Success" });
                    }
                    _imgname = Guid.NewGuid().ToString().Replace("-", "");
                    var _comPath = Server.MapPath("~/Content/Uploads/Photo/") + _imgname + _ext;

                    ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(_comPath);

                    if (img.Width > 130)
                        img.Resize(130, 130);
                    img.Save(_comPath);
                    // end resize
                    var userupdate = us.UpdateUserImage(_comPath, id);
                    if (userupdate != null || userupdate.Length > 1)
                    {
                        return Json(userupdate, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return RedirectToAction("Index", new { Message = "Upload Failed" });
                        //return Json(new { x, JsonRequestBehavior.AllowGet });
                    }

                }
                else
                {
                    return RedirectToAction("Index", new { Message = "Upload Failed" });
                }
            }
            else
            {
                return RedirectToAction("Index", new { Message = "Upload Failed" });
            }
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

        [HttpPost]
        public JsonResult ValidateEmail(string email)
        {
            try
            {
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                Users us = new Users();
                var emailvalidate = us.ValidateUserEmail(email, idUser);
                if (emailvalidate == false)
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
        public JsonResult UpdateUserData(string zipcore, string name, string lastname, string address, string email, string mobile)
        {
            try
            {
                List<ResultDataUser> user = null;
                var b = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Users us = new Users();
                    var update = us.UpdateDataUser(zipcore, name, lastname, address, email, mobile, idUser);
                    if (update == true)
                    {
                        user = us.dataUser(idUser);
                        foreach (var item in user)
                        {
                            b = new Framework.Libraies.ResultDataUser { ZipCode = item.ZipCode, FirstName = item.FirstName, LastName = item.LastName, Address = item.Address, Email = item.Email, Phone = item.Phone };
                        }
                        return Json(b, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Success = false });
                    }
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
        public JsonResult InfoWorkShop(int Orden)
        {
            List<ResultWorkshop> lista = null;
            bool error = false;
            var resultado = (dynamic)null;
            try
            {
                Workshop workshop = new Workshop();

                lista = workshop.infoWorkShop(Orden);

                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        resultado = new Framework.Libraies.ResultWorkshop { IdWorkshop = item.IdWorkshop, Name = item.Name, Address = item.Address, Phone = item.Phone, Email = item.Email, WorkImage = item.WorkImage };
                    }
                    error = false;
                }
                else
                {
                    error = true;
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return Json(new { error, resultado });

        }

    }
}