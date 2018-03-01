using System;
using System.Linq;
using System.Collections.Generic;
using DekkOnlineMVC.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using DekkOnlineMVC.Models.HelperClasses;
using System.Web;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace DekkOnlineMVC.Engine
{
    public class DatabaseFunctions
    {
        ApplicationDbContext dbApp;
        public string strError;
        static string errorMsg = "Hubo un error al consultar la base de datos";

        public DatabaseFunctions()
        {
            dbApp = new ApplicationDbContext();
        }

        public DatabaseFunctions(ApplicationDbContext _dbApp)
        {
            this.dbApp = _dbApp;
        }

        public void SaveChanges()
        {
            dbApp.SaveChanges();
        }

        /// <summary>
        /// Regresa el id del usuario por su id de red social.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        public string getUserIdByProviderKey(string provider, string providerKey)
        {
            try
            {
                string sqlQuery = @"SELECT LOG.UserId FROM AspNetUserLogins LOG 
                                    WHERE LOG.LoginProvider = '" + provider + "'" +
                                    " AND LOG.ProviderKey = '" + providerKey + "'";
                return dbApp.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Global_Functions.saveErrors(ex.ToString(), false);
            }
            return null;
        }
        /// <summary>
        /// Obtiene los datos del usuario por su id
        /// </summary>
        /// <param name="usuId"></param>
        /// <returns></returns>
        public ApplicationUser getUserById(string usuId)
        {
            return dbApp.Users.Where(u => u.Id.Equals(usuId)).FirstOrDefault();
        }

        public DateTime DateTimeMX()
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            Debug.WriteLine("*************DEBUG************");
            var dateMx = TimeZoneInfo.ConvertTime(DateTime.Now, tz);
            Debug.WriteLine(TimeZoneInfo.ConvertTime(dateMx, tz).Date);
            return dateMx;
        }

        #region Users-Profile-Config

        /// <summary>
        /// Regresa las propiedades del rol por su nombre
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole getRoleByName(string roleName)
        {
            strError = "";
            try
            {
                return (from rol in dbApp.Roles
                        where rol.Name.Equals(roleName)
                        select rol).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.ToString();
                strError = errorMsg;
            }
            return null;
        }
        #endregion
    }
}