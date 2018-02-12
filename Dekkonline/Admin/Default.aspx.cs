using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DekkOnline.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                loadOrders();
            }


            xgvOrders.DataSource = Session["dsOrders"];
            xgvOrders.DataBind();

            if (Session["dsOrderDetails"] != null)
            {
                xgvOrderDetails.DataSource = Session["dsOrderDetails"];
                xgvOrderDetails.DataBind();
            }

        }




        void loadOrders()
        {
            var orders = (from ord in db.Orders
                           where (ord.Delivered == false || ord.Delivered == null)
                           select new
                           {
                               IdOrder = ord.id,
                               DateOrder = ord.DateS,
                               Products = db.OrdersDetail.Where(od => od.OrderMain.Equals(ord.id)).Select(od => od.quantity).Sum(),
                               CodeProm = ord.PromoCode,
                               Total = ord.Total,
                               IdUser = ord.idUser,
                               NameUser = db.UserAddress.Where(u => u.IdUser.Equals(ord.idUser)).FirstOrDefault().FirstName + (db.UserAddress.Where(u => u.Equals(ord.idUser)).FirstOrDefault().LastName ?? ""),
                               EmailUser = db.AspNetUsers.Where(u => u.Id.Equals(ord.idUser)).FirstOrDefault().UserName
                           }).ToList();


            Session["dsOrders"] = orders;

        }
        

        protected void popOrder_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int orderId = int.Parse(e.Parameter.Split('|')[1]);
            loadOrderDetails(orderId);

            xgvOrderDetails.DataSource = Session["dsOrderDetails"];
            xgvOrderDetails.DataBind();
        }




        void loadOrderDetails(int idOrder)
        {
            var orderDetails = (from ord in db.OrdersDetail
                          where ord.OrderMain.Equals(idOrder)
                          select new
                          {
                              IdProduct = ord.proId,
                              Nmae = db.products.Where(p=>p.proId.Equals(ord.proId)).FirstOrDefault().proName,
                              Brand = db.products.Where(p => p.proId.Equals(ord.proId)).FirstOrDefault().brand.braName,
                              TyreSize = db.products.Where(p => p.proId.Equals(ord.proId)).FirstOrDefault().proTyreSize,
                              Quantity = ord.quantity,
                              Price = ord.price
                          }).ToList();


            Session["dsOrderDetails"] = orderDetails;


        }



    }
}