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


            Console.WriteLine(user.loadAddressUser("3f619083-b218-41e8-8693-1a93ecd82fdf"));
            Console.ReadLine();

        }
    }
}
