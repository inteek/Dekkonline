using Framework.Libraies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework
{
    public class Mail
    {
        private string path = System.Web.HttpContext.Current.Server.MapPath("");

        public bool sendEmailWorkshop(string emailUser, string emailWorkshop, string mensaje)
        {
            bool result = false;
            var archivo = "";
            var plantilla = "";

            //path = path.Split()

            string input = path;
            int index = input.LastIndexOf(@"\");
            if (index > 0)
                input = input.Substring(0, index); // or index + 1 to keep slash

            try
            {
                archivo = String.Format(@"{0}\Templates\EmailWorkshop.html", input);
                plantilla = getTexto_Plantilla(archivo);
                plantilla = plantilla.Replace("{MENSAJE}", mensaje);

                EnvioCorreo(emailUser, emailWorkshop, mensaje, plantilla, "Order information");

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        public bool sendEmailConfirmation(List<ResultPaidProducts> list)
        {
            bool result = false;
            var archivo = "";
            var archivo2 = "";
            var plantilla = "";
            string email = "";
            var htmldiv = "";
            var htmldivProducts = "";

            try
            {               
                string input = path;
                int index = input.LastIndexOf(@"\");
                if (index > 0)
                    input = input.Substring(0, index); // or index + 1 to keep slash

                archivo = String.Format(@"{0}\Templates\EmailConfirmation.html", input);
                archivo2 = String.Format(@"{0}\Templates\Products.html", input);
                plantilla = getTexto_Plantilla(archivo);


                foreach (var item in list)
                {
                    plantilla = plantilla.Replace("{NOMBRE}", item.FirstName);
                    plantilla = plantilla.Replace("{NUMORDEN}", item.Order.ToString());
                    plantilla = plantilla.Replace("{ZIPCODE}", item.ZipCode);
                    plantilla = plantilla.Replace("{FIRSTNAME}", item.FirstName);
                    plantilla = plantilla.Replace("{LASTNAME}", item.LastName);
                    plantilla = plantilla.Replace("{ADDRESS}", item.Address);
                    plantilla = plantilla.Replace("{EMAIL}", item.Email);
                    plantilla = plantilla.Replace("{MOBIL}", item.Mobile);

                    if (item.WorkshopName != null)
                    {
                        plantilla = plantilla.Replace("{IMAGE}", item.Image);
                        plantilla = plantilla.Replace("{WORKSHOPNAME}", item.WorkshopName);
                        plantilla = plantilla.Replace("{WORKSHOPADDRESS}", item.WorkshopAddress);
                    }
                    else
                    {
                        plantilla = plantilla.Replace("{WORKSHOPADDRESS}", item.WorkshopAddress);
                    }
                    
                    plantilla = plantilla.Replace("{DATE}", item.Date);
                    plantilla = plantilla.Replace("{TIME}", item.Time);
                    plantilla = plantilla.Replace("{COMMENTS}", item.Comments);
                    plantilla = plantilla.Replace("{TYPETARGET}", item.TypeTarget);
                    plantilla = plantilla.Replace("{NUMBER}", item.Number.ToString());
                    plantilla = plantilla.Replace("{EXPIRE}", item.Expire);
                    plantilla = plantilla.Replace("{FIRSTNAME}", item.FirstName);
                    plantilla = plantilla.Replace("{LASTNAME}", item.LastName);

                    foreach (var item2 in item.cart)
                    {
                        htmldiv = getTexto_Plantilla(archivo2);

                        htmldiv = htmldiv.Replace("{IMAGENPRODUCT}", item2.Image);
                        htmldiv = htmldiv.Replace("{NOMBREPRODUCTO}", item2.Name);
                        htmldiv = htmldiv.Replace("{PRECIOTOTAL}", item2.totalpriceprod.ToString());
                        htmldivProducts += htmldiv;
                    }

                    plantilla = plantilla.Replace("{PRODUCTS}", htmldivProducts);


                    email = item.Email;
                }

                emailConfirmation(email, plantilla, "Order confirmation");

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        private void EnvioCorreo(string emailUser, string emailWorkshop, string mensaje, string cuerpo, string asunto)
        {
            bool success = false;
            try
            {

                if (!string.IsNullOrEmpty(emailWorkshop))
                {
                    emailWorkshop = emailWorkshop.Replace(",", ";").Replace(" ", "");
                }
                else
                {
                    emailWorkshop = "";
                }

                ///INTEEK
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "inteekdev.com";//"inteekdev.com"
                smtp.Port = 587;//587
                smtp.Credentials = new System.Net.NetworkCredential("agalindo@inteekdev.com", "kPy-97w-Vxr-3ea");//"agalindo@inteekdev.com", "DeveloperNet.2016"


                MailMessage email2 = new MailMessage();
                email2.From = new MailAddress(emailUser);
                foreach (string lpCorreo in emailWorkshop.Split(';'))
                {
                    if (!string.IsNullOrEmpty(lpCorreo))
                    {
                        email2.To.Add(new MailAddress(lpCorreo));
                    }
                }

                if (email2.To.Count() > 0)
                {
                    email2.Subject = asunto;
                    email2.Body = cuerpo;
                    email2.IsBodyHtml = true;
                    email2.Priority = MailPriority.Normal;
                    smtp.Send(email2);
                }

                success = true;              

                if (success == false)
                {
                    throw new Exception("Chilkat no pudo enviar correo");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void emailConfirmation(string emailUser, string cuerpo, string asunto)
        {
            bool success = false;
            try
            {

                if (!string.IsNullOrEmpty(emailUser))
                {
                    emailUser = emailUser.Replace(",", ";").Replace(" ", "");
                }
                else
                {
                    emailUser = "";
                }

                ///INTEEK
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "inteekdev.com";//"inteekdev.com"
                smtp.Port = 587;//587
                smtp.Credentials = new System.Net.NetworkCredential("agalindo@inteekdev.com", "kPy-97w-Vxr-3ea");//"agalindo@inteekdev.com", "DeveloperNet.2016"


                MailMessage email2 = new MailMessage();
                email2.From = new MailAddress(emailUser);
                foreach (string lpCorreo in emailUser.Split(';'))
                {
                    if (!string.IsNullOrEmpty(lpCorreo))
                    {
                        email2.To.Add(new MailAddress(lpCorreo));
                    }
                }

                if (email2.To.Count() > 0)
                {
                    email2.Subject = asunto;
                    email2.Body = cuerpo;
                    email2.IsBodyHtml = true;
                    email2.Priority = MailPriority.Normal;
                    smtp.Send(email2);
                }

                success = true;

                if (success == false)
                {
                    throw new Exception("Chilkat no pudo enviar correo");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string getTexto_Plantilla(string archivo)
        {
            string result = string.Empty;
            try
            {
                using (var objFile = new StreamReader(archivo))
                {
                    result = objFile.ReadToEnd();
                    objFile.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
