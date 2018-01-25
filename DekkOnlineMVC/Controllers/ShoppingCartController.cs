﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
           
            ShoppingCart sh = new ShoppingCart();
          var pro =  sh.ProductsInCart("3f619083-b218-41e8-8693-1a93ecd82fdf");//8eb14cb4-c1d5-4e00-94fd-ca458532ac92
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

        public PartialViewResult Step2()
        {
            return PartialView();
        }

        public PartialViewResult Step3()
        {
            return PartialView();
        }

        public PartialViewResult Step4()
        {
            return PartialView();
        }






	}
}