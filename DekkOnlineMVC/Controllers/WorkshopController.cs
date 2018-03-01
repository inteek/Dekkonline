using Framework;
using Framework.Libraies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DekkOnlineMVC.Controllers
{
    public class WorkshopController : Controller
    {
        // GET: Workshop
        public ActionResult Index()
        {
            string path = Request.Url.AbsolutePath;
            ViewBag.ReturnUrl = path;
            Users us = new Users();
            ShoppingCart sh = new ShoppingCart();
            Workshop ws = new Workshop();
            var UserData = (dynamic)null;
            var idUser = Security.GetIdUser(this);
            var usuario1 = User.Identity.Name;
            var pro = (dynamic)null;
            if (usuario1 != "")
            {
                var id = sh.User(usuario1);
                UserData = ws.Workshopinfo(id);
                if (UserData != null)
                {
                    foreach (var item in UserData)
                    {
                        pro = new Framework.Libraies.ResultWorkshop
                        {
                            Name = item.Name,
                            Address = item.Address,
                            Phone = item.Phone,
                            ZipCode = item.ZipCode,
                            WorkImage = item.WorkImage,
                            IdWorkshop = item.IdWorkshop
                        };
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(pro);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public JsonResult UpdateWorkshopData(string zipcore, string name, string address, string mobile, string idwo)
        {
            try
            {
                List<ResultWorkshop> user = null;
                var b = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    var update = us.UpdateWorkshopData(zipcore, name, address, mobile, idUser, idwo);
                    if (update == true)
                    {
                        user = us.dataworkshop(idUser);
                        foreach (var item in user)
                        {
                            b = new Framework.Libraies.ResultWorkshop { ZipCode = item.ZipCode, Name = item.Name, Address = item.Address, Phone = item.Phone };
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



        public PartialViewResult Schedule()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult ScheduleWorkshop(string idwo)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.SchedulesWorkshop(idwo);
                    if (user != null || user.Count > 1)
                    {
                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult ScheduleWorkshopUpdate(string idschedule, string time, string dayint, string dayend, string idwo)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.UpdateWorkshopSchedule(Convert.ToInt32(idschedule), time, dayend, Convert.ToInt32(dayint), Convert.ToInt32(idwo));
                    if (user != null || user.Count >= 0)
                    {
                        return Json(user, JsonRequestBehavior.AllowGet);
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
                return Json(new { Success = false });
                throw;
            }

        }

        [HttpPost]
        public JsonResult DeleteWorkshopUpdate(string idschedule)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.DeleteWorkshopSchedule(Convert.ToInt32(idschedule));
                    if (user != null || user.Count > 1)
                    {
                        return Json(user, JsonRequestBehavior.AllowGet);
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
                return Json(new { Success = false });
                throw;
            }

        }

        [HttpPost]
        public JsonResult ScheduleWorkshopAdd(string time, string dayint, string dayend, string idwo)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.AddWorkshopSchedule(time, dayend, Convert.ToInt32(dayint), Convert.ToInt32(idwo));
                    if (user != null || user.Count > 1)
                    {
                        return Json(user, JsonRequestBehavior.AllowGet);
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
                return Json(new { Success = false });
                throw;
            }

        }


        public PartialViewResult Services()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult ServiceWorkshop(string idwo)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.ServiceWorkshop(idwo);
                    if (user != null || user.Count > 1)
                    {
                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult ServiceWorkshopasociarlist(string idwo)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.ServiceWorkshopasociar(Convert.ToInt32(idwo));
                    if (user != null || user.Count > 1)
                    {
                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult ServiceWorkshopasociar(string idwo, string service)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.ServiceWorkshopasociradd(Convert.ToInt32(idwo), Convert.ToInt32(service));
                    if (user == true)
                    {
                        return Json(new { Success = true });
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
        public JsonResult ServiceWorkshopcreate(string idwo, string name, string desc, string price)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.ServiceWorkshopcreate(Convert.ToInt32(idwo), name, desc, Convert.ToInt32(price));
                    if (user == true)
                    {
                        return Json(new { Success = true });
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
        public JsonResult DeleteWorkshopservice(string idservice)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.DeleteWorkshopService(Convert.ToInt32(idservice));
                    if (user != null || user.Count > 1)
                    {
                        return Json(new { Success = true });
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
                return Json(new { Success = false });
                throw;
            }

        }


        [HttpPost]
        public JsonResult ServiceWorkshopUpdate(string idservice, string Name, string Desc, string price)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.UpdateWorkshopService(Convert.ToInt32(idservice), Name, Desc, Convert.ToInt32(price));
                    if (user != null || user.Count >= 0)
                    {
                        return Json(user, JsonRequestBehavior.AllowGet);
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
                return Json(new { Success = false });
                throw;
            }

        }


        public PartialViewResult Pending()
        {
            return PartialView();
        }


        [HttpPost]
        public JsonResult ResultPendingOrderWorkshop(string idwo, string order)
        {
            try
            {
                var user = (dynamic)null;
                var user2 = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.loadOrderPendingdata(idwo, Convert.ToInt32(order));
                    if (user != null || user.Count >= 0)
                    {

                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult ResultPendingOrderWorkshopmain(string idwo)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.loadOrderPendingmain(idwo);
                    if (user != null || user.Count >= 0)
                    {

                        return Json(user, JsonRequestBehavior.AllowGet);
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

        public PartialViewResult Past()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult ResultPastOrderWorkshop(string idwo, string order)
        {
            try
            {
                var user = (dynamic)null;
                var user2 = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.loadOrderPastdata(idwo, Convert.ToInt32(order));
                    if (user != null || user.Count >= 0)
                    {

                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult ResultPastOrderWorkshopmain(string idwo)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    user = us.loadOrderPastmain(idwo);
                    if (user != null || user.Count >= 0)
                    {

                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult ZipCodes(string idwo, string isWorkshop)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();
                    
                    if (isWorkshop == "1")
                    {
                        user = us.GetZipCodesInWorkshop(Convert.ToInt32(idwo));
                    }
                    else
                    {
                        user = us.GetZipCodesNoInWorkshop(Convert.ToInt32(idwo));
                    }
                    if (user != null || user.Count >= 0)
                    {

                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult zipcodeinworkshopdelete(string idwo, string zipcode)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();

                    user = us.DeleteZipcode(Convert.ToInt32(idwo), Convert.ToInt32(zipcode));
                    if (user != null || user.Count >= 0)
                    {

                        return Json(user, JsonRequestBehavior.AllowGet);
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
        public JsonResult zipcodeinworkshopadd(string idwo, string zipcode)
        {
            try
            {
                var user = (dynamic)null;
                string idUser = System.Web.HttpContext.Current.Session["SessionUser"] as String;
                if (idUser != null && idUser != "")
                {
                    Workshop us = new Workshop();

                    user = us.Workshopzipcodeadd(Convert.ToInt32(idwo), Convert.ToInt32(zipcode));
                    if (user != null || user.Count >= 0)
                    {

                        return Json(user, JsonRequestBehavior.AllowGet);
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

        [AcceptVerbs(HttpVerbs.Post)]//enviar tambien el id del taller
        public ActionResult UploadFile()
        {
            ShoppingCart sh = new ShoppingCart();
            Workshop ws = new Workshop();
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
                    var userupdate = ws.UpdateWorkshopImage(realpath, id);
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


    }
}