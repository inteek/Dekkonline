using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;

namespace DekkOnlineMVC
{
    public static class UserName
    {
        public static string Username { get; set; }
    }

    public static class Getname
    {
        public static string name()
        {
            string data;
            string email = HttpContext.Current.User.Identity.Name;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var iduser = db.AspNetUsers.Where(s => s.UserName == email).FirstOrDefault();
                    if (iduser != null)
                    {
                        var userdata = db.UserAddress.Where(s => s.IdUser == iduser.Id).FirstOrDefault();
                        if (userdata != null)
                        {
                            data = userdata.FirstName + " " + userdata.LastName;
                        }
                        else
                        {
                            data = iduser.UserName;
                        }
                        return data;
                    }
                    else
                    {
                        data = "User";
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}