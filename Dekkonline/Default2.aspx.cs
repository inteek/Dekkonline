using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DekkOnline
{
    public partial class _Default : Page
    {
        dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Login?ReturnUrl=%2Fadmin%2F");
            //loadProducts();
            //if (!Page.IsCallback && !Page.IsPostBack)
            //{
            //    loadType();
            //    loadSize();
            //}
        }

        /// <summary>
        /// Testing deployment
        /// </summary>
        void loadType()
        {
            var cats = (from cat in db.categories where cat.catStatus == true select cat);
            cmbCategory.DataSource = cats;
            cmbCategory.ValueField = "catId";
            cmbCategory.TextField = "catName";
            cmbCategory.DataBind();
        }

        void loadSize()
        {

            var ws = (from pro in db.products where pro.proDimensionWidth.HasValue select new { Id = pro.proDimensionWidth.Value, Size = pro.proDimensionWidth.Value.ToString() }).Distinct().OrderBy(c=>c.Id).ToList();
            var ps = (from pro in db.products where pro.proDimensionProfile.HasValue select new { Id = pro.proDimensionProfile.Value, Size = pro.proDimensionProfile.Value.ToString() }).Distinct().OrderBy(c => c.Id).ToList();
            var ds = (from pro in db.products where pro.proDimensionDiameter.HasValue select new { Id = pro.proDimensionDiameter.Value, Size = pro.proDimensionDiameter.Value.ToString() }).Distinct().OrderBy(c => c.Id).ToList();

            initDropSize(ref cmbWidth, ws.OrderBy(c=>c.Size).ToList());
            initDropSize(ref cmbProfile, ps.OrderBy(c=>c.Size).ToList());
            initDropSize(ref cmbDiameter, ds.OrderBy(c => c.Size).ToList());
        }

        void initDropSize(ref DevExpress.Web.ASPxComboBox cmb, object ds)
        {
            cmb.DataSource = ds;
            cmb.TextField = "Size";
            cmb.ValueField = "Id";
            cmb.DataBind();
        }
    }
}