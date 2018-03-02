using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DekkOnline.engine;

namespace DekkOnline
{
    /// <summary>
    /// Summary description for updateData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class updateData : System.Web.Services.WebService
    {
        
        [WebMethod]
        public bool updateAllData(bool overwrite)
        {
            try
            {
                dekkpro.updateData(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
