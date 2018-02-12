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
                                    idWorkshop = ws.Id,
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
                                  IdWorkshop = address.IdWorkshop,
                                  Name = address.Name,
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
                                                  where (address.ZipCode >= rango1 && address.ZipCode <= rango2)
                                                  orderby address.Average descending
                                                  select new ResultWorkshop
                                                  {
                                                      IdWorkshop = address.IdWorkshop,
                                                      Name = address.Name,
                                                      Address = address.Address,
                                                      ZipCode = address.ZipCode,
                                                      Latitude = address.Latitude,
                                                      Length = address.Length
                                                  }).ToList();
                    }
                    else
                    {
                        result = (from address in db.Workshop
                                  where (address.ZipCode >= rango1 && address.ZipCode <= rango2)
                                  select new ResultWorkshop
                                  {
                                      IdWorkshop = address.IdWorkshop,
                                      Name = address.Name,
                                      Address = address.Address,
                                      ZipCode = address.ZipCode,
                                      Latitude = address.Latitude,
                                      Length = address.Length
                                  }).ToList();
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

        public bool addDeliveryType(int workshop, string idUser, int idWorkShop, int servicio, int fecha, string date, string time, string comments, string address)
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

                    if (order == delivery)
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
                            addwork.IdServiceWorkshop = servicio;
                            addwork.IdAppointments = fecha;
                            addwork.Date = Convert.ToDateTime(date);
                            addwork.Time = hora2;
                            addwork.Comments = comments;
                            addwork.Address = null;
                            db.DeliveryType.Add(addwork);
                            db.SaveChanges();

                            result = true;
                        }
                        else
                        {
                            var addwork = new Entity.DeliveryType();
                            addwork.DeliveryType1 = work;
                            addwork.IdUser = idUser;
                            addwork.IdWorkshop = idWorkShop;
                            addwork.IdServiceWorkshop = servicio;
                            addwork.IdAppointments = fecha;
                            addwork.Date = Convert.ToDateTime(date);
                            addwork.Time = time;
                            addwork.Comments = comments;
                            addwork.Address = address;
                            db.DeliveryType.Add(addwork);
                            db.SaveChanges();

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
                                update.IdServiceWorkshop = servicio;
                                update.IdAppointments = fecha;
                                update.Date = Convert.ToDateTime(date);
                                update.Time = hora2;
                                update.Comments = comments;
                                update.Address = address;

                                db.Entry(update).State = EntityState.Modified;
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
                            if (update != null)
                            {
                                update.DeliveryType1 = work;
                                update.IdWorkshop = idWorkShop;
                                update.IdServiceWorkshop = servicio;
                                update.IdAppointments = fecha;
                                update.Date = Convert.ToDateTime(date);
                                update.Time = time;
                                update.Comments = comments;
                                update.Address = address;

                                db.Entry(update).State = EntityState.Modified;
                                db.SaveChanges();
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
                                    Date = Nuevafecha
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

    }
}
