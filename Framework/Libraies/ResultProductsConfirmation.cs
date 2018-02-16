using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultProductsConfirmation
    {
        public List<ResultShoppingCartProduct> cart { get; set; }
        public string ZipCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Promo { get; set; }
        public string WorkshopName { get; set; }
        public string WorkshopAddress { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Comments { get; set; }
        public string Rating { get; set; }
        public string Image { get; set; }
        public int Total { get; set; }
    }
}
