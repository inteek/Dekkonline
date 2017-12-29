using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Framework.Libraies;
using System.Data.Entity;

namespace Framework
{
    public class Orders
    {
        public bool AddToPurchaseOrder(string idUser, string products, decimal totalPrice, string paymentmethod, DateTime orderDate, bool oderStatus, int idDelivery, DateTime deliveredDate, string usedPromo, string comments)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var addPurchaseOrder = new Entity.PurchaseOrder();
                    addPurchaseOrder.IdUser = idUser;
                    addPurchaseOrder.Products = products;
                    addPurchaseOrder.TotalPrice = totalPrice;
                    addPurchaseOrder.Paymentmethod = paymentmethod;
                    addPurchaseOrder.OrderDate = orderDate;
                    addPurchaseOrder.Orderstatus = oderStatus;
                    addPurchaseOrder.IdDelivery = idDelivery;
                    addPurchaseOrder.DeliveredDate = deliveredDate;
                    addPurchaseOrder.UsedPromo = usedPromo;
                    addPurchaseOrder.Comments = comments;

                    db.PurchaseOrder.Add(addPurchaseOrder);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //_Error = ex;
                return false;
            }
        }

    }
}
