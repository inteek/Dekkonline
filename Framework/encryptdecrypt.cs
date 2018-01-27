using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework
{
  public  class encryptdecrypt
    {


        /// Encripta una cadena
        public string Encriptar(string cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public string DesEncriptar(string cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        public bool IsSessionTimedOut()
        {
            HttpContext ctx = HttpContext.Current;
            
            if (ctx == null)
                throw new Exception("Este método sólo se puede usar en una aplicación Web");

            //Comprobamos que haya sesión en primer lugar 
            //(por ejemplo si por ejemplo EnableSessionState=false)
            if (ctx.Session == null)
                return false;   //Si no hay sesión, no puede caducar
                                //Se comprueba si se ha generado una nueva sesión en esta petición
            if (!ctx.Session.IsNewSession)
                return false;   //Si no es una nueva sesión es que no ha caducado

            //HttpCookie objCookie = ctx.Request.Cookies["UserInfo"];
            ////Esto en teoría es imposible que pase porque si hay una 
            ////nueva sesión debería existir la cookie, pero lo compruebo porque
            ////IsNewSession puede dar True sin ser cierto (más en el post)
            //if (objCookie == null)
            //    return false;

            ////Si hay un valor en la cookie es que hay un valor de sesión previo, pero como la sesión 
            ////es nueva no debería estar, por lo que deducimos que la sesión anterior ha caducado
            //if (!string.IsNullOrEmpty(objCookie.Value))
            //    return true;
            //else


            return false;
        }











    }





}
