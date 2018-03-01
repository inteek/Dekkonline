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
    public class ErroresController : Controller
    {
        // GET: Error
        public ActionResult Index(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "Esto es muy vergonzoso, trabajaremos para que esto no vuelva a pasar.";
                    break;

                case 404:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "La URL que está intentando ingresar no existe.";
                    break;

                default:
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "Algo salió muy mal :( trabajaremos para que esto no vuelva a pasar.";
                    break;
            }

            return View("~/views/errores/ErrorPage.cshtml");
        }
    }
}