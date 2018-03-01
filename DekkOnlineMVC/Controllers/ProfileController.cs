using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                    sh.UpdateShoppingCart(id, idUser);
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
                        return RedirectToAction("Index", new { Message = "invalid Upload" });
                    }
                    _imgname = Guid.NewGuid().ToString().Replace("-", "");
                    _imgname = _imgname.Replace(_imgname, id);
                    var _comPath = Server.MapPath("~/Content/Uploads/Photo/") + _imgname + _ext;

                    ViewBag.Msg = _comPath;
                    var path = _comPath;
                    string pathvalidate1 = _imgname + _ext;
                    pathvalidate1 = pathvalidate1.Remove(pathvalidate1.Length - 4);
                    pathvalidate1 = pathvalidate1 + ".png";
                    string pathvalidate2 = _imgname + _ext;
                    pathvalidate2 = pathvalidate2.Remove(pathvalidate2.Length - 4);
                    pathvalidate2 = pathvalidate2 + ".jpg";
                    // Saving Image in Original Mode
                    string fullPath1 = Request.MapPath("~/Content/Uploads/Photo/" + pathvalidate1);
                    string fullPath2 = Request.MapPath("~/Content/Uploads/Photo/" + pathvalidate2);
                    if (System.IO.File.Exists(fullPath1))
                    {
                        System.IO.File.Delete(fullPath1);
                        pic.SaveAs(path);
                    }
                    else if (System.IO.File.Exists(fullPath2))
                    {
                        System.IO.File.Delete(fullPath2);
                        pic.SaveAs(path);
                    }
                    else
                    {
                        pic.SaveAs(path);
                    }

                    // resizing image
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(_comPath);

                    if (img.Width > 130)
                        img.Resize(130, 130);
                    img.Save(_comPath);
                    // end resize
                    var realpath = "/Content/Uploads/Photo/" + _imgname + _ext;
                    var userupdate = us.UpdateUserImage(realpath, id);
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



        //protected bool FTPUploadimg(object sender, EventArgs e)
        //{
        //    bool result = false;
        //    //FTP Server URL.
        //    string ftp = "ftp://dekkonline.sone.mx/";

        //    //FTP Folder name. Leave blank if you want to upload to root folder.
        //    string ftpFolder = "Content/Uploads/Photo/";

        //    byte[] fileBytes = null;

        //    //Read the FileName and convert it to Byte array.
        //    string fileName = Path.GetFileName(FileUpload1.FileName);
        //    using (StreamReader fileStream = new StreamReader(FileUpload1.PostedFile.InputStream))
        //    {
        //        fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
        //        fileStream.Close();
        //    }

        //    try
        //    {
        //        //Create FTP Request.
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
        //        request.Method = WebRequestMethods.Ftp.UploadFile;

        //        //Enter FTP Server credentials.
        //        request.Credentials = new NetworkCredential("dekkonline.sone.mx|dekkonline", "Tr75bv5d84");
        //        request.ContentLength = fileBytes.Length;
        //        request.UsePassive = true;
        //        request.UseBinary = true;
        //        request.ServicePoint.ConnectionLimit = fileBytes.Length;
        //        request.EnableSsl = false;

        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(fileBytes, 0, fileBytes.Length);
        //            requestStream.Close();
        //        }

        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //        response.Close();
        //        result = true;
        //        return result;
        //    }
        //    catch (WebException ex)
        //    {
        //        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        //    }
        //}



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
            Orders or = new Orders();
            ShoppingCart sh = new ShoppingCart();
            var promos = (dynamic)null;
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            var lista = (dynamic)null;
            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
                promos = or.loadPromos(id);

                if (promos != null)
                {
                    foreach (var item in promos)
                    {
                        lista = new Framework.Libraies.ResultUserPromo
                        {
                            promo = item.promo,
                            user = item.user
                        };
                    }
                }
                else
                {
                    ;
                }
            }
            else
            {
                Response.Redirect("~/Home/Index");
            }
            return PartialView(lista);
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
        [HttpPost]
        public JsonResult emailWorkshop(int idWorkshop, string mensaje)
        {
            Workshop workshop = new Workshop();
            string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
            try
            {
                bool email = workshop.emailWorkshop(idUser, idWorkshop, mensaje);

                if (email == true)
                {
                    return Json(new { error = false });
                }
                else
                {
                    return Json(new { error = true });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult emailPromo(string correos, string mensaje)
        {
            Users users = new Users();
            string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
            try
            {
                bool email = users.emailPromo(idUser, correos, mensaje);
                if (email == true)
                {
                    return Json(new { error = false });
                }
                else
                {
                    return Json(new { error = true });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}