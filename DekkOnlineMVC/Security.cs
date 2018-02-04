
using System;
using System.Linq;
using System.Web.Mvc;
using DekkOnlineMVC.Models;
using System.Collections.Generic;
using System.Web;
using System.Security.Claims;

namespace DekkOnlineMVC
{

    public static class Security
    {
        public static string GetSessionIdUser(Controller ctr)
        {
            if (ctr.Session["UserInfo"] != null)
            {
                return ctr.Session["UserInfo"].ToString();
            }
            else
            {
                return null;
            }

        }

        public static string GetIdUser(Controller ctr)
        {
            Framework.encryptdecrypt Frame = new Framework.encryptdecrypt();

            if (GetSessionIdUser(ctr) == null)
            {
                if (ctr.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UserInfo") == false)
                {
                    ctr.ControllerContext.HttpContext.Response.Cookies.Clear();

                    //CREAR COOKIES
                    Random rnd = new Random();
                    int Id = rnd.Next();
                    var enid = Frame.Encriptar(Id.ToString());

                    HttpCookie cookie = new HttpCookie("UserInfo");
                    cookie.Value = enid;
                    cookie.Expires = DateTime.Now.AddYears(1);
                    ctr.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    return enid;
                }
                else
                {
                    return ctr.ControllerContext.HttpContext.Request.Cookies["UserInfo"].Value.ToString();
                }
            }
            else
            {
                //OBRENER ID DE SESSION
                return GetSessionIdUser(ctr);
            }
        }
    }
}