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
                    delail = (from work in db.Workshop where work.IdWorkshop == idWorkshop select new { idWorkshop = work.IdWorkshop, name = work.Name, address = work.Address, phone = work.Phone, zipCode = work.ZipCode });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return delail;
        }

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
                                    Description = ser.Description
                                }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return services;
        }

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

        public bool AddToWorkshop(string name, string address, string phone, int zipCode, string latitude, string length)
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

        public bool AddToDelivery(bool deliveryType, int idUser, int idWorkshop, int idServiceWorkshop, int idAppointmentsWorkshop, DateTime date, string time, string comments)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    if (idAppointmentsWorkshop != null)
                    {
                        var addDelivery = new Entity.DeliveryType();
                        addDelivery.DeliveryType1 = deliveryType;
                        addDelivery.IdUser = idUser;
                        addDelivery.IdWorkshop = idWorkshop;
                        addDelivery.IdServiceWorkshop = idServiceWorkshop;
                        addDelivery.IdAppointmentsWorkshop = idAppointmentsWorkshop;
                        addDelivery.Date = null;
                        addDelivery.Time = null;
                        addDelivery.Comments = null;
                        db.DeliveryType.Add(addDelivery);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        var addDelivery = new Entity.DeliveryType();
                        addDelivery.DeliveryType1 = deliveryType;
                        addDelivery.IdUser = idUser;
                        addDelivery.IdWorkshop = idWorkshop;
                        addDelivery.IdServiceWorkshop = idServiceWorkshop;
                        addDelivery.IdAppointmentsWorkshop = null;
                        addDelivery.Date = date;
                        addDelivery.Time = time;
                        addDelivery.Comments = comments;
                        db.DeliveryType.Add(addDelivery);
                        db.SaveChanges();
                        return true;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                //_Error = ex;
                return false;
            }
        }

        public List<ResultWorkshop> loadWorkshopAddress()
        {
            List<ResultWorkshop> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from address in db.Workshop
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
    }
}
