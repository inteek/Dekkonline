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
        public string loadAddressUser(string idUser)
        {
            var UserAddress = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    UserAddress = (from user in db.UserAddress where user.IdUser == idUser select new { adreess = user.Address, latitude = user.Latitude, length = user.Length }).FirstOrDefault().ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return UserAddress;
        }

        //DE-14 2
        public bool updateAddressUser(string idUser, string firstName, string lastName, string address, string phone, int zipCode, string latitude, string length)
        {
            bool Result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var result = db.UserAddress.Where(s => s.IdUser == idUser).FirstOrDefault();
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
            catch (Exception ex)
            {
                throw;
            }
            return Result;
        }

        //DE-14 5
        public bool addToUserAddress(string idUser, string firstName, string lastName, string address, string phone, int zipCode, string latitude, string length)
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
        public List<ResultDataUser> dataUser(string username, string password)
        {

            var user = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var idUser = db.AspNetUsers.Where(s => s.Email == username && s.PasswordHash == password).Select(s => s.Id).FirstOrDefault();
                    user = (from us in db.UserAddress
                            join aspuser in db.AspNetUsers on us.IdUser equals aspuser.Id
                            where us.IdUser == idUser
                            select new ResultDataUser
                            {
                                IdUser = us.IdUser,
                                FirstName = us.FirstName,
                                LastName = us.LastName,
                                Address = us.Address,
                                Phone = us.Phone,
                                ZipCode = us.ZipCode.ToString(),
                                Latitude = us.Latitude,
                                length = us.Length,
                                Image = us.Image,
                                Email = aspuser.Email
                            }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        public bool validaLogin(string user, string pass)
        {
            bool valUser = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var result = db.AspNetUsers.Where(s => s.UserName == user && s.PasswordHash == pass).FirstOrDefault();

                    if (result != null)
                    {
                        valUser = true;
                    }
                    else
                    {
                        valUser = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return valUser;
        }
    }
}
