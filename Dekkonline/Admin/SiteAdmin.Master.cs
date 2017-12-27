using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DekkOnline.Admin
{
    public partial class SiteAdmin : System.Web.UI.MasterPage
    {

        public string username = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtCurrentPage.Text = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                try
                {
                    username = System.Web.Security.Membership.GetUser().UserName;
                }
                catch { }
            }
        }

        protected void LinkButtonMyadminAcc_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/my_user_details.aspx?parms=" + Page.User.Identity.Name.ToString() + "");
        }
    }
}