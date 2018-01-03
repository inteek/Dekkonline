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
    public class Workshop
    {
        public string detailWorkshopAddress(int idWorkshop)
        {
            var delail = (dynamic)null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    delail = (from work in db.Workshop where work.IdWorkshop == idWorkshop select new { idWorkshop = work.IdWorkshop, name = work.Name, address = work.Address, phone = work.Phone, zipCode = work.ZipCode }).FirstOrDefault().ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return delail;
        }

        //DE-14 4
        public List<ResultTypesServices> detailWorkshopServices(int idWorkshop)
        {
            List<ResultTypesServices> services = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    services = (from ser in db.TypesServices
                                join ws in db.WorkshopServices on ser.IdService equals ws.IdService
                                where ws.IdWorkshop == idWorkshop
                                select new ResultTypesServices
                                {
                                    IdService = ser.IdService,
                                    Description = ser.Description,
                                    Price = ws.Price
                                }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return services;
        }

        //DE-14 4
        public List<ResultAppointments> detailWorkshopAppointments(int idWorkshop)
        {
            List<ResultAppointments> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from a in db.Appointments
                              join wa in db.WorkshopAppointment on a.IdAppointment equals wa.IdAppointment
                              where wa.IdWorkshop == idWorkshop
                              select new ResultAppointments
                              {
                                  IdAppointment = a.IdAppointment,
                                  Schedule = a.Schedule
                              }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        //DE-14 5
        public bool addToWorkshop(string name, string address, string phone, int zipCode, string latitude, string length)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var addWorkshop = new Entity.Workshop();
                    addWorkshop.Name = name;
                    addWorkshop.Address = address;
                    addWorkshop.Phone = phone;
                    addWorkshop.ZipCode = zipCode;
                    addWorkshop.Latitude = latitude;
                    addWorkshop.Length = length;
                    db.Workshop.Add(addWorkshop);
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

        //DE-14 3
        public List<ResultWorkshop> loadWorkshopAddress(int zipCode)
        {
            List<ResultWorkshop> result = null;
            int rango1 = zipCode - 100;
            int rango2 = zipCode + 100;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from address in db.Workshop
                              where (address.ZipCode >= rango1 && address.ZipCode <= rango2)
                              select new ResultWorkshop
                              {
                                  Address = address.Address,
                                  ZipCode = address.ZipCode,
                                  Latitude = address.Latitude,
                                  Length = address.Length
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        //DE-26 1
        public List<ResultWorkshop> loadWorkshop(int workshop)
        {
            List<ResultWorkshop> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from address in db.Workshop
                              where (address.IdWorkshop >= workshop)
                              select new ResultWorkshop
                              {
                                  Address = address.Address,
                                  ZipCode = address.ZipCode,
                                  Phone = address.Phone,
                                  Latitude = address.Latitude,
                                  Length = address.Length
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
       
    }
}
