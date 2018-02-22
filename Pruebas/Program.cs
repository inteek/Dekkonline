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
            //List<Framework.Libraies.ResultPurchaseOrder> product = null;
            ////product = orders.loadOrderPast("3f619083-b218-41e8-8693-1a93ecd82fdf");

            //foreach (var item in product)
            //{
            //    Console.WriteLine("ProductImage: " + item.ProductImage);
            //    Console.WriteLine("IdOrderDetail: " + item.IdOrderDetail);
            //    Console.WriteLine("ProductName: " + item.ProductName);
            //    Console.WriteLine("Price: " + item.Price);
            //    Console.WriteLine("Quantity: " + item.Quantity);
            //    Console.WriteLine("DeliveredDate: " + item.DeliveredDate);
            //    Console.WriteLine("TotalPrice1: " + item.TotalPrice1 + "\n");
            //}
            Uri uriAddress1 = new Uri("C:/Productos/DekkOnline20022018/DekkOnlineMVC/Content/Uploads/Photo/0d3410419f314dbeb0ff579bd4fdd59d.png");
            Console.WriteLine("The parts are {0}, {1}, {2}", uriAddress1.Segments[0], uriAddress1.Segments[1], uriAddress1.Segments[4]);
            Console.ReadLine();

        }
    }
}
