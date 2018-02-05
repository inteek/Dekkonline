using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DekkOnlineMVC.Models;
namespace DekkOnlineMVC.Models
{
    public class LoginRegisterViewModel
    {
        public Models.LoginViewModel Login { get; set; }
        public Models.RegisterViewModel Register { get; set; }
    }
}