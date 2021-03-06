﻿using System;
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
                                    idWorkshop = ws.Id,
                                    IdService = ser.IdService,
                                    Description = ser.Description,
                                    Price = (double)ws.Price
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
                              join zipwork in db.WorkshopZipCode on address.IdWorkshop equals zipwork.IdWorkshop
                              where (zipwork.IdKommune >= rango1 && zipwork.IdKommune <= rango2)
                              select new ResultWorkshop
                              {
                                  IdWorkshop = address.IdWorkshop,
                                  Name = address.Name,
                                  Address = address.Address,
                                  ZipCode = address.ZipCode,
                                  Latitude = address.Latitude,
                                  Length = address.Length,
                                  WorkImage = address.WorkImage
                              }).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public List<ResultWorkshop> loadWorkshopreco(int zipCode, int filter)
        {
            List<ResultWorkshop> result = null;
            int rango1 = zipCode - 100;
            int rango2 = zipCode + 100;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    if (filter == 1)
                    {
                        result = (from address in db.Workshop
                                      //where (address.ZipCode >= rango1 && address.ZipCode <= rango2)
                                  join zipwork in db.WorkshopZipCode on address.IdWorkshop equals zipwork.IdWorkshop
                                  where (zipwork.IdKommune >= rango1 && zipwork.IdKommune <= rango2)
                                  orderby address.Average descending
                                  select new ResultWorkshop
                                  {
                                      IdWorkshop = address.IdWorkshop,
                                      Name = address.Name,
                                      Address = address.Address,
                                      ZipCode = zipwork.IdKommune,
                                      Latitude = address.Latitude,
                                      Length = address.Length,
                                      WorkImage = address.WorkImage,
                                      Average = (int)address.Average
                                  }).Distinct().ToList();
                        result = result.GroupBy(x => x.IdWorkshop).Select(x => x.FirstOrDefault()).ToList();
                        result = result.OrderByDescending(s => s.Average).Distinct().ToList();
                        

                    }
                    else if (filter == 2)
                    {
                        result = (from address in db.Workshop
                                  join zipwork in db.WorkshopZipCode on address.IdWorkshop equals zipwork.IdWorkshop
                                  where (zipwork.IdKommune >= rango1 && zipwork.IdKommune <= rango2)
                                  orderby zipwork.IdKommune descending
                                  select new ResultWorkshop
                                  {
                                      IdWorkshop = address.IdWorkshop,
                                      Name = address.Name,
                                      Address = address.Address,
                                      ZipCode = zipwork.IdKommune,
                                      Latitude = address.Latitude,
                                      Length = address.Length,
                                      WorkImage = address.WorkImage
                                  }).Distinct().ToList();
                        result = result.GroupBy(x => x.IdWorkshop).Select(x => x.FirstOrDefault()).ToList();
                        result = result.OrderByDescending(s => s.ZipCode).Distinct().ToList();
                    }
                    else
                    {
                        result = (from address in db.Workshop
                                  join zipwork in db.WorkshopZipCode on address.IdWorkshop equals zipwork.IdWorkshop
                                  where (zipwork.IdKommune >= rango1 && zipwork.IdKommune <= rango2)
                                  select new ResultWorkshop
                                  {
                                      IdWorkshop = address.IdWorkshop,
                                      Name = address.Name,
                                      Address = address.Address,
                                      ZipCode = zipwork.IdKommune,
                                      Latitude = address.Latitude,
                                      Length = address.Length,
                                      WorkImage = address.WorkImage
                                  }).Distinct().ToList();
                        result = result.GroupBy(x => x.IdWorkshop).Select(x => x.FirstOrDefault()).ToList();
                    }

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

        public bool addDeliveryType(int workshop, string idUser, int idWorkShop, string[] servicio, int fecha, string date, string time, string comments, string address)
        {
            bool result = false;
            bool work = false;

            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var order = db.Orders.Where(s => s.idUser == idUser).OrderByDescending(s => s.id).Select(s => s.DeliveryAddress).FirstOrDefault();
                    var delivery = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).Select(s => s.IdDelivery).FirstOrDefault();

                    if (workshop == 0) work = false;
                    else work = true;

                    if (order == delivery || (order == null && delivery == 0))
                    {
                        if (fecha != 0)
                        {
                            var hora = db.WorkshopAppointment.Where(s => s.Id == fecha).FirstOrDefault();
                            TimeSpan timeSpan = (TimeSpan)hora.Time;
                            string hora2 = timeSpan.ToString(@"hh\:mm");

                            var addwork = new Entity.DeliveryType();
                            addwork.DeliveryType1 = work;
                            addwork.IdUser = idUser;
                            addwork.IdWorkshop = idWorkShop;
                            addwork.IdServiceWorkshop = 0;
                            addwork.IdAppointments = fecha;
                            addwork.Date = Convert.ToDateTime(date);
                            addwork.Time = hora2;
                            addwork.Comments = comments;
                            addwork.Address = null;
                            db.DeliveryType.Add(addwork);
                            db.SaveChanges();

                            var delivery2 = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).Select(s => s.IdDelivery).FirstOrDefault();
                            var deleteservicelistp = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                            if (deleteservicelistp != null || deleteservicelistp.Count != 0)
                            {
                                foreach (var item2 in deleteservicelistp)
                                {
                                    var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                    db.DeliveryServices.Remove(deleteservice);
                                    db.SaveChanges();
                                }
                            }
                            if (servicio.Length>0)
                            {
                                foreach (var item in servicio)
                                {
                                    if (item == "0")
                                    {
                                        var deleteservicelist = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                                        if (deleteservicelist != null || deleteservicelist.Count != 0)
                                        {
                                            foreach (var item2 in deleteservicelist)
                                            {
                                                var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                                db.DeliveryServices.Remove(deleteservice);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var serviceadd = new Entity.DeliveryServices();
                                        serviceadd.idDelivery = delivery2;
                                        serviceadd.idService = Convert.ToInt32(item);
                                        db.DeliveryServices.Add(serviceadd);
                                        db.SaveChanges();
                                    }
                                  
                                }
                                
                            }
                            result = true;
                        }
                        else
                        {
                            var addwork = new Entity.DeliveryType();
                            addwork.DeliveryType1 = work;
                            addwork.IdUser = idUser;
                            addwork.IdWorkshop = idWorkShop;
                            addwork.IdServiceWorkshop = 0;
                            addwork.IdAppointments = fecha;
                            addwork.Date = Convert.ToDateTime(date);
                            addwork.Time = time;
                            addwork.Comments = comments;
                            addwork.Address = address;
                            db.DeliveryType.Add(addwork);
                            db.SaveChanges();
                            var delivery2 = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).Select(s => s.IdDelivery).FirstOrDefault();
                            var deleteservicelistp = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                            if (deleteservicelistp != null || deleteservicelistp.Count != 0)
                            {
                                foreach (var item2 in deleteservicelistp)
                                {
                                    var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                    db.DeliveryServices.Remove(deleteservice);
                                    db.SaveChanges();
                                }
                            }
                            if (servicio.Length > 0)
                            {
                                foreach (var item in servicio)
                                {
                                    if (item == "0")
                                    {
                                        var deleteservicelist = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                                        if (deleteservicelist != null || deleteservicelist.Count != 0)
                                        {
                                            foreach (var item2 in deleteservicelist)
                                            {
                                                var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                                db.DeliveryServices.Remove(deleteservice);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var serviceadd = new Entity.DeliveryServices();
                                        serviceadd.idDelivery = delivery2;
                                        serviceadd.idService = Convert.ToInt32(item);
                                        db.DeliveryServices.Add(serviceadd);
                                        db.SaveChanges();
                                    }

                                }

                            }
                            result = true;
                        }
                    }
                    else
                    {
                        var update = db.DeliveryType.Where(s => s.IdDelivery == delivery).FirstOrDefault();

                        if (fecha != 0)
                        {
                            var hora = db.WorkshopAppointment.Where(s => s.Id == fecha).FirstOrDefault();
                            TimeSpan timeSpan = (TimeSpan)hora.Time;
                            string hora2 = timeSpan.ToString(@"hh\:mm");

                            if (update != null)
                            {
                                update.DeliveryType1 = work;
                                update.IdWorkshop = idWorkShop;
                                update.IdServiceWorkshop = 0;
                                update.IdAppointments = fecha;
                                update.Date = Convert.ToDateTime(date);
                                update.Time = hora2;
                                update.Comments = comments;
                                update.Address = address;

                                db.Entry(update).State = EntityState.Modified;
                                db.SaveChanges();
                                var delivery2 = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).Select(s => s.IdDelivery).FirstOrDefault();
                                var deleteservicelistp = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                                if (deleteservicelistp != null || deleteservicelistp.Count != 0)
                                {
                                    foreach (var item2 in deleteservicelistp)
                                    {
                                        var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                        db.DeliveryServices.Remove(deleteservice);
                                        db.SaveChanges();
                                    }
                                }
                                if (servicio.Length > 0)
                                {
                                    foreach (var item in servicio)
                                    {
                                        if (item == "0")
                                        {
                                            var deleteservicelist = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                                            if (deleteservicelist != null || deleteservicelist.Count != 0)
                                            {
                                                foreach (var item2 in deleteservicelist)
                                                {
                                                    var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                                    db.DeliveryServices.Remove(deleteservice);
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                        else
                                        {

                                            var serviceadd = new Entity.DeliveryServices();
                                            serviceadd.idDelivery = delivery2;
                                            serviceadd.idService = Convert.ToInt32(item);
                                            db.DeliveryServices.Add(serviceadd);
                                            db.SaveChanges();
                                        }

                                    }

                                }
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (update != null)
                            {
                                update.DeliveryType1 = work;
                                update.IdWorkshop = idWorkShop;
                                update.IdServiceWorkshop = 0;
                                update.IdAppointments = fecha;
                                update.Date = Convert.ToDateTime(date);
                                update.Time = time;
                                update.Comments = comments;
                                update.Address = address;

                                db.Entry(update).State = EntityState.Modified;
                                db.SaveChanges();
                                var delivery2 = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).Select(s => s.IdDelivery).FirstOrDefault();
                                var deleteservicelistp = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                                if (deleteservicelistp != null || deleteservicelistp.Count != 0)
                                {
                                    foreach (var item2 in deleteservicelistp)
                                    {
                                        var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                        db.DeliveryServices.Remove(deleteservice);
                                        db.SaveChanges();
                                    }
                                }
                                if (servicio.Length > 0)
                                {
                                    foreach (var item in servicio)
                                    {
                                        if (item == "0")
                                        {
                                            var deleteservicelist = db.DeliveryServices.Where(s => s.idDelivery == delivery2).ToList();
                                            if (deleteservicelist != null || deleteservicelist.Count != 0)
                                            {
                                                foreach (var item2 in deleteservicelist)
                                                {
                                                    var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                                    db.DeliveryServices.Remove(deleteservice);
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var serviceadd = new Entity.DeliveryServices();
                                            serviceadd.idDelivery = delivery2;
                                            serviceadd.idService = Convert.ToInt32(item);
                                            db.DeliveryServices.Add(serviceadd);
                                            db.SaveChanges();
                                        }

                                    }

                                }
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }


                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool updateDeliveryappointmentmap(string idUser)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var order = db.Orders.Where(s => s.idUser == idUser).OrderByDescending(s => s.id).FirstOrDefault();
                    var delivery = db.DeliveryType.Where(s => s.IdUser == idUser).OrderByDescending(s => s.IdDelivery).FirstOrDefault();
                    if (order.DeliveryAddress != delivery.IdDelivery || (order == null && delivery != null))
                    {

                        var deleteservicelistp = db.DeliveryServices.Where(s => s.idDelivery == delivery.IdDelivery).ToList();
                        if (deleteservicelistp != null || deleteservicelistp.Count != 0)
                        {
                            foreach (var item2 in deleteservicelistp)
                            {
                                var deleteservice = db.DeliveryServices.Where(s => s.idDelivery == item2.idDelivery).FirstOrDefault();
                                db.DeliveryServices.Remove(deleteservice);
                                db.SaveChanges();
                            }
                            result = true;
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }


        public List<ResultWorkshopAppointment> loadAppoinment(int idWorkShop)
        {
            List<ResultWorkshopAppointment> result = null;
            try
            {

                using (var db = new dekkOnlineEntities())
                {
                    result = (from date in db.WorkshopAppointment
                              where (date.IdWorkshop >= idWorkShop)
                              select new ResultWorkshopAppointment
                              {
                                  Id = date.Id,
                                  Date = (DateTime)date.Date,
                                  Time = (TimeSpan)date.Time,
                                  Comments = date.Comments
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public List<ResultWorkshopDateAppointment> dateWorkshop(int Workshop)
        {
            DateTime fecha_actual = DateTime.Today;
            fecha_actual = fecha_actual.AddDays(5);
            string nombreDia = "";
            string fecha = "";
            string Nuevafecha = "";
            List<ResultWorkshopDateAppointment> result = null;
            List<ResultWorkshopDateAppointment> result2 = new List<ResultWorkshopDateAppointment>();


            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var diasTaller = (from wsa in db.WorkshopAppointment
                                      where wsa.IdWorkshop == Workshop
                                      select new
                                      {
                                          Id = (int)wsa.Id,
                                          DayAppointment = (int)wsa.DayAppointment,
                                          Time = wsa.Time

                                      }).ToList();



                    if (diasTaller != null)
                    {
                        foreach (var item in diasTaller)
                        {
                            TimeSpan timeSpan = (TimeSpan)item.Time;
                            string hora = timeSpan.ToString(@"hh\:mm");

                            int fechaCercana = ((int)item.DayAppointment - (int)fecha_actual.DayOfWeek + 7) % 7;
                            DateTime nextTuesday = fecha_actual.AddDays(fechaCercana);
                            Nuevafecha = nextTuesday.ToString("d");
                            nombreDia = nextTuesday.ToString("dddd");
                            fecha = nextTuesday.ToString("M");
                            string fechaCompleta = nombreDia + " " + fecha + " - " + hora;

                            result = new List<ResultWorkshopDateAppointment>
                            {
                                new ResultWorkshopDateAppointment
                                {
                                    IdAppointment = item.Id,
                                    DateGet = fechaCompleta,
                                    Date = Nuevafecha,
                                    Time = item.Time.ToString()
                                }
                            };

                            result2.AddRange(result);
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result2;
        }

        public List<ResultWorkshop> Workshopinfo(string idUser)
        {
            List<ResultWorkshop> result = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    result = (from ws in db.Workshop
                              join wus in db.WorkshopUser on ws.IdWorkshop equals wus.idWorkshop
                              where wus.idUser == idUser
                              select new ResultWorkshop
                              {
                                  IdWorkshop = ws.IdWorkshop,
                                  Name = ws.Name,
                                  Address = ws.Address,
                                  ZipCode = ws.ZipCode,
                                  WorkImage = ws.WorkImage,
                                  Phone = ws.Phone
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool UpdateWorkshopData(string zipcore, string name, string address, string mobile, string idUser1, string idwork)
        {
            bool result = false;
            try
            {
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var workshopdata = db.Workshop.Where(s => s.IdWorkshop.ToString() == idwork).FirstOrDefault();
                    if (workshopdata != null)
                    {
                        workshopdata.Name = name;
                        workshopdata.Address = address;
                        workshopdata.Phone = mobile;
                        workshopdata.ZipCode = Convert.ToInt32(zipcore);
                        db.Entry(workshopdata).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public List<ResultWorkshop> dataworkshop(string idUser)
        {

            List<ResultWorkshop> user = null;
            try
            {
                using (var db = new dekkOnlineEntities())
                {

                    user = (from ws in db.Workshop
                            join wus in db.WorkshopUser on ws.IdWorkshop equals wus.idWorkshop
                            where wus.idUser == idUser
                            select new ResultWorkshop
                            {
                                Name = ws.Name,
                                Address = ws.Address,
                                Phone = ws.Phone,
                                ZipCode = ws.ZipCode,
                                WorkImage = ws.WorkImage
                            }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }


        public List<ResultWorkshopDateAppointment> SchedulesWorkshop(string workshop)
        {
            List<ResultWorkshopDateAppointment> result = null;
            List<ResultWorkshopDateAppointment> result2 = new List<ResultWorkshopDateAppointment>();
            try
            {
                using (var db = new dekkOnlineEntities())
                {


                    var user = (from ws in db.WorkshopAppointment
                                where ws.IdWorkshop.ToString() == workshop
                                select new
                                {
                                    IdAppointment = ws.Id,
                                    Time = ws.Time,
                                    Date = ws.DayAppointment.ToString(),
                                    Dayint = ws.DayAppointment.ToString(),
                                    TimeEnd = ws.TimeEnd
                                }).ToList();
                    if (user != null)
                    {
                        foreach (var item in user)
                        {
                            TimeSpan time1 = (TimeSpan)item.Time;
                            TimeSpan timeend1 = (TimeSpan)item.TimeEnd;
                            string dia = Enum.GetName(typeof(DayOfWeek), Convert.ToInt32(item.Date));
                            string time2 = time1.ToString(@"hh\:mm");
                            string timeend2 = timeend1.ToString(@"hh\:mm");
                            result = new List<ResultWorkshopDateAppointment> {
                                new ResultWorkshopDateAppointment{
                                    IdAppointment = item.IdAppointment,
                                    Time = time2,
                                    Date = dia,
                                    Dayint = item.Dayint,
                                    TimeEnd = timeend2
                                }
                            };
                            result2.AddRange(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result2;

        }

        public bool UpdateWorkshopSchedule(int idschedule, string time, string dayend, int dayint, int idwo)
        {
            bool result = false;
            try
            {
                TimeSpan time2 = TimeSpan.Parse(time);
                TimeSpan time3 = TimeSpan.Parse(dayend);
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var workshopdata = db.WorkshopAppointment.Where(s => s.IdWorkshop == idwo && s.Id == idschedule).FirstOrDefault();
                    if (workshopdata != null)
                    {
                        workshopdata.Time = time2;
                        workshopdata.TimeEnd = time3;
                        workshopdata.DayAppointment = dayint;
                        db.Entry(workshopdata).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public bool DeleteWorkshopSchedule(int idschedule)
        {
            bool result = false;
            try
            {
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var workshopdata = db.WorkshopAppointment.Where(s => s.Id == idschedule).FirstOrDefault();
                    if (workshopdata != null)
                    {
                        db.WorkshopAppointment.Remove(workshopdata);
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public bool AddWorkshopSchedule(string time, string timeend, int dayint, int idwo)
        {
            bool result = false;
            try
            {
                TimeSpan time2 = TimeSpan.Parse(time);
                TimeSpan time3 = TimeSpan.Parse(timeend);
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    WorkshopAppointment workshopdata = new WorkshopAppointment();
                    workshopdata.Time = time2;
                    workshopdata.DayAppointment = dayint;
                    workshopdata.IdWorkshop = idwo;
                    workshopdata.TimeEnd = time3;
                    db.WorkshopAppointment.Add(workshopdata);
                    db.SaveChanges();
                    result = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = false;
                return result;
                throw;
            }
        }

        public List<ResultTypesServices> ServiceWorkshop(string workshop)
        {
            List<ResultTypesServices> result = null;
            List<ResultTypesServices> result2 = new List<ResultTypesServices>();
            try
            {
                using (var db = new dekkOnlineEntities())
                {


                    var user = (from ws in db.WorkshopServices
                                join ser in db.TypesServices on ws.IdService equals ser.IdService
                                where ws.IdWorkshop.ToString() == workshop
                                select new
                                {
                                    IdService = ws.Id,
                                    Name = ser.Name,
                                    Description = ser.Description,
                                    Price = Math.Truncate((double)ws.Price)
                                }).ToList();
                    if (user != null)
                    {
                        foreach (var item in user)
                        {
                            result = new List<ResultTypesServices> {
                                new ResultTypesServices{
                                   IdService = item.IdService,
                                   Name = item.Name,
                                   Description = item.Description,
                                   Price = (double)item.Price
                                }
                            };
                            result2.AddRange(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result2;

        }

        public List<ResultTypesServices> ServiceWorkshopasociar(int idwork)
        {
            List<ResultTypesServices> result = null;
            List<ResultTypesServices> result2 = new List<ResultTypesServices>();
            try
            {
                using (var db = new dekkOnlineEntities())
                {


                    var user = (from ws in db.WorkshopServices
                                join ser in db.TypesServices on ws.IdService equals ser.IdService
                                where ws.IdWorkshop == idwork
                                select new
                                {
                                    IdService = ser.IdService,
                                    Name = ser.Name,
                                    Description = ser.Description,
                                    Price = 0,
                                    service = ser.IdService
                                }).ToList();
                    var user2 = (from ser in db.TypesServices
                                 select new
                                 {
                                     IdService = ser.IdService,
                                     Name = ser.Name,
                                     Description = ser.Description,
                                     Price = 0,
                                     service = ser.IdService
                                 }).ToList();
                    var serviciosnoeneltaller = user2.Except(user).ToList();
                    if (serviciosnoeneltaller != null)
                    {
                        foreach (var item in serviciosnoeneltaller)
                        {
                            result = new List<ResultTypesServices> {
                                new ResultTypesServices{
                                   IdService = item.IdService,
                                   Name = item.Name,
                                   Description = item.Description,
                                   Price = item.Price
                                }
                            };
                            result2.AddRange(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result2;

        }

        public bool ServiceWorkshopcreate(int idwork, string service, string desc, int price)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {

                    TypesServices serv = new TypesServices();
                    serv.Name = service;
                    serv.Description = desc;
                    db.TypesServices.Add(serv);
                    db.SaveChanges();

                    var servicio = db.TypesServices.Where(s => s.Name == service && s.Description == desc).OrderByDescending(s => s.IdService).FirstOrDefault();
                    if (servicio != null)
                    {
                        WorkshopServices work = new WorkshopServices();
                        work.IdService = servicio.IdService;
                        work.IdWorkshop = idwork;
                        work.Price = (decimal)Math.Truncate((double)price);
                        db.WorkshopServices.Add(work);
                        db.SaveChanges();
                        result = true;
                        return result;
                    }
                    else
                    {
                        result = false;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                return result;
                throw;
            }

        }

        public bool ServiceWorkshopasociradd(int idwork, int service)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var servicio = db.TypesServices.Where(s => s.IdService == service).OrderByDescending(s => s.IdService).FirstOrDefault();
                    if (servicio != null)
                    {
                        WorkshopServices work = new WorkshopServices();
                        work.IdService = servicio.IdService;
                        work.IdWorkshop = idwork;
                        db.WorkshopServices.Add(work);
                        db.SaveChanges();
                        result = true;
                        return result;
                    }
                    else
                    {
                        result = false;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                return result;
                throw;
            }

        }

        public bool DeleteWorkshopService(int idserv)
        {
            bool result = false;
            try
            {
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var workshopdata = db.WorkshopServices.Where(s => s.Id == idserv).FirstOrDefault();
                    if (workshopdata != null)
                    {
                        db.WorkshopServices.Remove(workshopdata);
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public bool UpdateWorkshopService(int idservice, string Name, string Desc, int price)
        {
            bool result = false;
            try
            {
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var workshopdataserv = db.WorkshopServices.Where(s => s.Id == idservice).FirstOrDefault();
                    if (workshopdataserv != null)
                    {
                        var service = db.TypesServices.Where(s => s.IdService == workshopdataserv.IdService).FirstOrDefault();
                        service.Name = Name;
                        service.Description = Desc;
                        workshopdataserv.Price = (decimal)Math.Truncate((double)price);
                        db.Entry(workshopdataserv).State = EntityState.Modified;
                        db.Entry(service).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //DE-25 1
        public List<ResultPendingOrderWorkshop> loadOrderPendingdata(string idWork, int orderid)
        {
            try
            {
                var products = (dynamic)null;
                List<ResultPendingOrderWorkshop> order = null;
                List<ResultPendingOrderWorkshop> order2 = new List<ResultPendingOrderWorkshop>();
                using (var db = new dekkOnlineEntities())
                {
                    var userdata2 = (from us in db.AspNetUsers
                                     join us2 in db.UserAddress on us.Id equals us2.IdUser
                                     join or in db.Orders on us.Id equals or.idUser
                                     join del in db.DeliveryType on or.DeliveryAddress equals del.IdDelivery
                                     where del.IdWorkshop.ToString() == idWork && or.id == orderid
                                     select new
                                     {
                                         FirstName = us2.FirstName,
                                         LastName = us2.LastName,
                                         Email = us.Email
                                     }).FirstOrDefault();
                    products = (from pro in db.products
                                join ord in db.OrdersDetail on pro.proId equals ord.proId
                                join or in db.Orders on ord.OrderMain equals or.id
                                where or.Delivered == false && or.DeliveredDate == null && or.id == orderid
                                select new ResultOrderProductsUser
                                {
                                    proId = ord.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = ord.quantity,
                                    totalpriceprod = Math.Truncate((double)ord.price),
                                    orders = or.id.ToString(),
                                    estimated1 = (DateTime)or.EstimatedDate,
                                    orderdte1 = (DateTime)or.DateS
                                }).ToList();
                    foreach (var item in products)
                    {
                        var dateE = item.estimated1;
                        dateE = dateE.ToString("D");
                        var dateO = item.orderdte1;
                        dateO = dateO.ToString("D");
                        item.estimated2 = dateE;
                        item.orderdte2 = dateO;
                    }
                    foreach (var item in products)
                    {
                        order = new List<ResultPendingOrderWorkshop>
                        {
                            new ResultPendingOrderWorkshop{
                            FirstName = userdata2.FirstName,
                            LastName = userdata2.LastName,
                            Email = userdata2.Email,
                            proId = item.proId,
                            Name = item.Name,
                            Description = item.Description,
                            quantity = item.quantity,
                            totalpriceprod = item.totalpriceprod,
                            orders = item.orders }
                        };
                        order2.AddRange(order);
                    }

                    return order2;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //DE-25 1
        public List<ResultOrders> loadOrderPendingmain(string idWork)
        {
            try
            {
                var products = (dynamic)null;
                List<ResultDataUser> userdata = new List<ResultDataUser>();
                using (var db = new dekkOnlineEntities())
                {

                    products = (from or in db.Orders
                                join ord in db.OrdersDetail on or.id equals ord.OrderMain
                                join del in db.DeliveryType on or.DeliveryAddress equals del.IdDelivery
                                where del.IdWorkshop.ToString() == idWork && or.Delivered == false && or.DeliveredDate == null

                                select new ResultOrders
                                {
                                    IdOrderDetail = or.id,
                                    UsedPromo = or.PromoCode,
                                    TotalPrice = (int)or.Total,
                                    datesale = or.DateS,
                                    dateest = or.EstimatedDate

                                }).ToList();
                    foreach (var item in products)
                    {
                        var dateE = item.dateest;
                        dateE = dateE.ToString("D");
                        var dateO = item.datesale;
                        dateO = dateO.ToString("D");
                        item.dateest2 = dateE;
                        item.datesale2 = dateO;
                    };
                    return products;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //DE-29 1
        public List<ResultUserOrder> loadOrderPast(string idUser)
        {
            try
            {
                var products = (dynamic)null;
                List<ResultDataUser> userdata = null;
                using (var db = new dekkOnlineEntities())
                {
                    userdata = (from us in db.AspNetUsers
                                join us2 in db.UserAddress on us.Id equals us2.IdUser
                                where (us.Id == idUser)
                                select new ResultDataUser
                                {
                                    FirstName = us2.FirstName,
                                    LastName = us2.LastName,
                                    Email = us.Email
                                }).ToList();
                    products = (from pro in db.products
                                join ord in db.OrdersDetail on pro.proId equals ord.proId
                                join or in db.Orders on ord.OrderMain equals or.id
                                join del in db.DeliveryType on or.DeliveryAddress equals del.IdDelivery
                                where or.idUser.Equals(idUser) && or.DeliveredDate != null && or.Delivered == true
                                select new ResultOrderProductsUser
                                {
                                    IdUser = or.idUser,
                                    proId = ord.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = ord.quantity,
                                    totalpriceprod = Math.Truncate((double)ord.price),
                                    orders = or.id.ToString(),
                                    estimated1 = (DateTime)or.EstimatedDate,
                                    orderdte1 = (DateTime)or.DateS,
                                    Datedelivered1 = (DateTime)or.DeliveredDate
                                }).ToList();
                    foreach (var item in products)
                    {
                        var dateE = item.estimated1;
                        dateE = dateE.ToString("D");
                        var dateO = item.orderdte1;
                        dateO = dateO.ToString("D");
                        var dateD = item.Datedelivered1;
                        dateD = dateD.ToString("D");
                        item.estimated2 = dateE;
                        item.orderdte2 = dateO;
                        item.Datedelivered2 = dateD;
                    }

                    List<ResultUserOrder> order = new List<ResultUserOrder>()
                    {
                        new ResultUserOrder{
                                product = products,
                                user = userdata
                        }
                    };
                    return order;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public List<ResultWorkshop> infoWorkShop(int Orden)
        {
            var infoWork = (dynamic)null;
            List<ResultWorkshop> result = null;

            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var Delevery = db.Orders.Where(s => s.id == Orden).Select(s => s.DeliveryAddress).FirstOrDefault();

                    var idWork = db.DeliveryType.Where(s => s.IdDelivery == Delevery).Select(s => s.IdWorkshop).FirstOrDefault();

                    if (idWork != 0)
                    {
                        infoWork = db.Workshop.Where(s => s.IdWorkshop == idWork).FirstOrDefault();
                    }

                    if (infoWork != null)
                    {
                        result = new List<ResultWorkshop>
                            {
                                new ResultWorkshop
                                {
                                    IdWorkshop = infoWork.IdWorkshop,
                                    Name = infoWork.Name,
                                    Address = infoWork.Address,
                                    Phone = infoWork.Phone,
                                    Email = infoWork.Email,
                                    WorkImage = infoWork.WorkImage
                                }
                            };
                    }

                }

            }
            catch (Exception ex)
            {

            }

            return result;

        }


        //DE-25 1
        public List<ResultPendingOrderWorkshop> loadOrderPastdata(string idWork, int orderid)
        {
            try
            {
                var products = (dynamic)null;
                List<ResultPendingOrderWorkshop> order = null;
                List<ResultPendingOrderWorkshop> order2 = new List<ResultPendingOrderWorkshop>();
                using (var db = new dekkOnlineEntities())
                {
                    var userdata2 = (from us in db.AspNetUsers
                                     join us2 in db.UserAddress on us.Id equals us2.IdUser
                                     join or in db.Orders on us.Id equals or.idUser
                                     join del in db.DeliveryType on or.DeliveryAddress equals del.IdDelivery
                                     where del.IdWorkshop.ToString() == idWork && or.id == orderid
                                     select new
                                     {
                                         FirstName = us2.FirstName,
                                         LastName = us2.LastName,
                                         Email = us.Email
                                     }).FirstOrDefault();
                    products = (from pro in db.products
                                join ord in db.OrdersDetail on pro.proId equals ord.proId
                                join or in db.Orders on ord.OrderMain equals or.id
                                where or.Delivered == true && or.DeliveredDate != null && or.id == orderid
                                select new ResultOrderProductsUser
                                {
                                    proId = ord.proId,
                                    Image = pro.proImage,
                                    Name = pro.proName,
                                    Description = pro.proDescription,
                                    quantity = ord.quantity,
                                    totalpriceprod = Math.Truncate((double)ord.price),
                                    orders = or.id.ToString(),
                                    estimated1 = (DateTime)or.DeliveredDate,
                                    orderdte1 = (DateTime)or.DateS
                                }).ToList();
                    foreach (var item in products)
                    {
                        var dateE = item.estimated1;
                        dateE = dateE.ToString("D");
                        var dateO = item.orderdte1;
                        dateO = dateO.ToString("D");
                        item.estimated2 = dateE;
                        item.orderdte2 = dateO;
                    }
                    foreach (var item in products)
                    {
                        order = new List<ResultPendingOrderWorkshop>
                        {
                            new ResultPendingOrderWorkshop{
                            FirstName = userdata2.FirstName,
                            LastName = userdata2.LastName,
                            Email = userdata2.Email,
                            proId = item.proId,
                            Name = item.Name,
                            Description = item.Description,
                            quantity = item.quantity,
                            totalpriceprod = item.totalpriceprod,
                            orders = item.orders }
                        };
                        order2.AddRange(order);
                    }

                    return order2;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //DE-25 1
        public List<ResultOrders> loadOrderPastmain(string idWork)
        {
            try
            {
                var products = (dynamic)null;
                List<ResultDataUser> userdata = new List<ResultDataUser>();
                using (var db = new dekkOnlineEntities())
                {

                    products = (from or in db.Orders
                                join ord in db.OrdersDetail on or.id equals ord.OrderMain
                                join del in db.DeliveryType on or.DeliveryAddress equals del.IdDelivery
                                where del.IdWorkshop.ToString() == idWork && or.Delivered == true && or.DeliveredDate != null

                                select new ResultOrders
                                {
                                    IdOrderDetail = or.id,
                                    UsedPromo = or.PromoCode,
                                    TotalPrice = (int)or.Total,
                                    datesale = or.DateS,
                                    dateest = or.DeliveredDate

                                }).ToList();
                    foreach (var item in products)
                    {
                        var dateE = item.dateest;
                        dateE = dateE.ToString("D");
                        var dateO = item.datesale;
                        dateO = dateO.ToString("D");
                        item.dateest2 = dateE;
                        item.datesale2 = dateO;
                    };
                    return products;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public List<ResultZipCode> GetZipCodesInWorkshop(int work)
        {
            List<ResultZipCode> zipc1 = null;
            List<ResultZipCode> zipc2 = new List<ResultZipCode>();
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var zipcodes = (from zpc in db.kommuner
                                    join fyl in db.fylker on zpc.fylkeID equals fyl.fylkeID
                                    join wsz in db.WorkshopZipCode on zpc.kommuneID equals wsz.IdKommune
                                    where wsz.IdWorkshop == work
                                    select new ResultZipCode
                                    {
                                        kommuneID = zpc.kommuneID,
                                        KommuneNavn = zpc.kommuneNavn,
                                        fylker = fyl.fylkeNavn
                                    }).ToList();
                    foreach (var item in zipcodes)
                    {
                        zipc1 = new List<ResultZipCode>
                        {
                            new ResultZipCode
                            {
                                kommuneID = item.kommuneID,
                                KommuneNavn = item.KommuneNavn,
                                fylker = item.fylker
                            }
                        };
                        zipc2.AddRange(zipc1);
                    }
                    return zipc2;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<ResultZipCode> GetZipCodesNoInWorkshop(int work)
        {
            List<ResultZipCode> zipc1 = null;
            List<ResultZipCode> zipc2 = new List<ResultZipCode>();
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var zipcodes = (from zpc in db.kommuner
                                    join fyl in db.fylker on zpc.fylkeID equals fyl.fylkeID
                                    join wsz in db.WorkshopZipCode on zpc.kommuneID equals wsz.IdKommune
                                    where wsz.IdWorkshop == work
                                    select new ResultZipCode
                                    {
                                        kommuneID = zpc.kommuneID,
                                        KommuneNavn = zpc.kommuneNavn,
                                        fylker = fyl.fylkeNavn
                                    }).ToList();
                    var zipcodes2 = (from zpc in db.kommuner
                                    join fyl in db.fylker on zpc.fylkeID equals fyl.fylkeID
                                    select new ResultZipCode
                                    {
                                        kommuneID = zpc.kommuneID,
                                        KommuneNavn = zpc.kommuneNavn,
                                        fylker = fyl.fylkeNavn
                                    }).ToList();
                    var zeipcodesin = new HashSet<int>(zipcodes.Select(x => x.kommuneID));
                    var zips = zipcodes2.Where(s=> !zeipcodesin.Contains(s.kommuneID));
                    foreach (var item in zips)
                    {
                        zipc1 = new List<ResultZipCode>
                        {
                            new ResultZipCode
                            {
                                kommuneID = item.kommuneID,
                                KommuneNavn = item.KommuneNavn,
                                fylker = item.fylker
                            }
                        };
                        zipc2.AddRange(zipc1);
                    }
                    return zipc2;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public bool emailWorkshop(string idUser, int idWorkshop, string mensaje)
        {
            bool result = false;
            var mail = new Mail();

            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var emailWorkshop = db.Workshop.Where(s => s.IdWorkshop == idWorkshop).Select(s => s.Email).FirstOrDefault();
                    var emailUser = db.AspNetUsers.Where(s => s.Id == idUser).Select(s => s.Email).FirstOrDefault();

                    mail.sendEmailWorkshop(emailUser, emailWorkshop, mensaje);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool DeleteZipcode(int idwo, int zipcode)
        {
            bool result = false;
            try
            {
                using (dekkOnlineEntities db = new dekkOnlineEntities())
                {
                    var zipcodeWorkshop = db.WorkshopZipCode.Where(s => s.IdWorkshop == idwo && s.IdKommune == zipcode).FirstOrDefault();
                    var zipcodevalidate = db.kommuner.Where(s => s.kommuneID == (short)zipcode).FirstOrDefault();
                    if (zipcodevalidate != null)
                    {
                        if (zipcodeWorkshop != null)
                        {
                            db.WorkshopZipCode.Remove(zipcodeWorkshop);
                            db.SaveChanges();
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public bool Workshopzipcodeadd(int idwork, int zipcode)
        {
            bool result = false;
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var zipwork = db.WorkshopZipCode.Where(s => s.IdWorkshop == idwork && s.IdKommune==zipcode).FirstOrDefault();
                    var zipcodevalidate = db.kommuner.Where(s => s.kommuneID == (short)zipcode).FirstOrDefault();
                    if (zipcodevalidate != null)
                    {
                            if (zipwork == null)
                            {
                                WorkshopZipCode workz = new WorkshopZipCode();
                                workz.IdWorkshop = idwork;
                                workz.IdKommune = (short)zipcode;
                                db.WorkshopZipCode.Add(workz);
                                db.SaveChanges();
                                result = true;
                                return result;
                            }
                            else
                            {
                                result = false;
                                return result;
                            }
                    }
                    else
                    {
                        result = false;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                return result;
                throw;
            }

        }


        public string UpdateWorkshopImage(string path, string idUser1)
        {
            try
            {
                using (var db = new dekkOnlineEntities())
                {
                    var UserInfo = db.UserAddress.Where(s => s.IdUser == idUser1).FirstOrDefault();
                    if (UserInfo != null)
                    {
                        UserInfo.Image = path;
                        db.Entry(UserInfo).State = EntityState.Modified;
                        db.SaveChanges();
                        var image = db.UserAddress.Where(s => s.IdUser == idUser1).Select(s => s.Image).FirstOrDefault();
                        return image;
                    }
                    else if (UserInfo == null)
                    {
                        UserAddress us2 = new UserAddress();
                        us2.IdUser = idUser1;
                        us2.Image = path;
                        db.UserAddress.Add(us2);
                        db.SaveChanges();
                        var image = db.UserAddress.Where(s => s.IdUser == idUser1).Select(s => s.Image).FirstOrDefault();
                        return image;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }

    }
    }
