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
    public class Users
    {
        //DE-14 1
        public string loadAddressUser(int idUser)
        {
            var UserAddress = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    UserAddress = (from user in db.UserAddress where user.IdUser == idUser select new { adreess = user.Address, latitude = user.Latitude, length = user.Length });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return UserAddress;
        }

        //DE-14 2
        public bool updateAddressUser(int idUser, string firstName, string lastName, string address, string phone, int zipCode, string latitude, string length)
        {
            bool Result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var result = db.UserAddress.Find(idUser);
                    if (result != null)
                    {
                        result.FirstName = firstName;
                        result.LastName = lastName;
                        result.Address = address;
                        result.Phone = phone;
                        result.ZipCode = zipCode;
                        result.Latitude = latitude;
                        result.Length = length;

                        db.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }

        //DE-14 5
        public bool addToUserAddress(int idUser, string firstName, string lastName, string address, string phone, int zipCode, string latitude, string length)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var user = db.UserAddress.Where(s => s.IdUser == idUser).FirstOrDefault();
                    if (user == null)
                    {
                        var addUserAddress = new Entity.UserAddress();
                        addUserAddress.IdUser = idUser;
                        addUserAddress.FirstName = firstName;
                        addUserAddress.LastName = lastName;
                        addUserAddress.Address = address;
                        addUserAddress.Phone = phone;
                        addUserAddress.ZipCode = zipCode;
                        addUserAddress.Latitude = latitude;
                        addUserAddress.Length = length;
                        db.UserAddress.Add(addUserAddress);
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        user.FirstName = firstName;
                        user.LastName = lastName;
                        user.Address = address;
                        user.Phone = phone;
                        user.ZipCode = zipCode;
                        user.Latitude = latitude;
                        user.Length = length;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        //DE-23 1
        public string dataUser(int idUser)
        {
            var user = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    user = (from us in db.UserAddress where us.IdUser == idUser select new { idUsuario = us.IdUser, firstName = us.FirstName, lastName = us.LastName, address = us.Address, phone = us.Phone, zipCode = us.ZipCode, latitude = us.Latitude, length = us.Length });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }
    }
}
