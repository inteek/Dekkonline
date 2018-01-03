using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Framework;

namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            Framework.Users user = new Framework.Users();
            Framework.Articles articles = new Framework.Articles();
            Framework.Workshop work = new Framework.Workshop();
            Framework.Orders orders = new Framework.Orders();
            Framework.ShoppingCart shoppin = new Framework.ShoppingCart();


            //Console.WriteLine(shoppin.LoadPointsPerUser("3f619083-b218-41e8-8693-1a93ecd82fdff"));
            //Console.WriteLine(user.updateAddressUser("3f619083-b218-41e8-8693-1a93ecd82fd2","Alfredo","Escobar","Apodaca #2334","4444444444", 3333, null, null));
            List<Framework.Libraies.ResultPurchaseOrder> product = null;
            product = orders.loadOrderPast("3f619083-b218-41e8-8693-1a93ecd82fdf");

            foreach (var item in product)
            {
                Console.WriteLine("ProductImage: " + item.ProductImage);
                Console.WriteLine("IdOrderDetail: " + item.IdOrderDetail);
                Console.WriteLine("ProductName: " + item.ProductName);
                Console.WriteLine("Price: " + item.Price);
                Console.WriteLine("Quantity: " + item.Quantity);
                Console.WriteLine("DeliveredDate: " + item.DeliveredDate);
                Console.WriteLine("TotalPrice1: " + item.TotalPrice1 + "\n");
            }
            Console.ReadLine();

        }
    }
}
