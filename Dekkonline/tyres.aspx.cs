using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DekkOnline
{
    public partial class tyres : System.Web.UI.Page
    {
        dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                loadType();
                loadBrands();
                loadSize();


                string type = "";
                try { type = Request.GetFriendlyUrlSegments()[0].ToString(); } catch { }
                string size = "";
                try { size = Request.GetFriendlyUrlSegments()[1].ToString(); } catch { }
                string brand = "";
                try { brand = Request.GetFriendlyUrlSegments()[2].ToString(); } catch { }

                if(type != "")
                {
                    try { cmbCategory.Items.FindByText(type.ToUpper()).Selected = true; } catch { }
                }

                if(size != "")
                {
                    try { cmbWidth.Value = size.Substring(0, 3); } catch { }
                    try { cmbProfile.Value = size.Substring(3, 2); } catch { }
                    try { cmbDiameter.Value=size.Substring(5, 2); } catch { }
                }

                loadProducts();
            }
            else
            {
                xgvProducts.DataSource = Session["products"];
                xgvProducts.DataBind();
            }
        }

        void loadType()
        {
            var cats = (from cat in db.categories where cat.catStatus == true select new { catId = cat.catId, catName = cat.catName.ToUpper() });
            cmbCategory.DataSource = cats;
            cmbCategory.ValueField = "catId";
            cmbCategory.TextField = "catName";
            cmbCategory.DataBind();
        }

        void loadSize()
        {

            var ws = from pro in db.products where pro.proDimensionWidth.HasValue select new { Id = pro.proDimensionWidth.Value.ToString(), Size = pro.proDimensionWidth.Value.ToString() };
            var ps = from pro in db.products where pro.proDimensionProfile.HasValue select new { Id = pro.proDimensionProfile.Value.ToString(), Size = pro.proDimensionProfile.Value.ToString() };
            var ds = from pro in db.products where pro.proDimensionDiameter.HasValue select new { Id = pro.proDimensionDiameter.Value.ToString(), Size = pro.proDimensionDiameter.Value.ToString() };

            initDropSize(ref cmbWidth, ws.ToList());
            initDropSize(ref cmbProfile, ps.ToList());
            initDropSize(ref cmbDiameter, ds.ToList());
        }

        void initDropSize(ref DevExpress.Web.ASPxComboBox cmb, object ds)
        {
            cmb.DataSource = ds;
            cmb.TextField = "Size";
            cmb.ValueField = "Id";
            cmb.DataBind();
        }

        void loadBrands()
        {
            var bran = (from bra in db.brands where bra.products.Count > 0 select bra);
            cmbBrand.DataSource = bran;
            cmbBrand.ValueField = "braId";
            cmbBrand.TextField = "braName";
            cmbBrand.ImageUrlField = "braImage";
            cmbBrand.DataBind();
        }


        void loadProducts()
        {

            int catId = 0;
            try { int.TryParse(cmbCategory.Value.ToString(), out catId); } catch { }

            int width = 0;
            int profile = 0;
            int diameter = 0;

            try { int.TryParse(cmbWidth.Value.ToString(), out width); } catch { }
            try { int.TryParse(cmbProfile.Value.ToString(), out profile); } catch { }
            try { int.TryParse(cmbDiameter.Value.ToString(), out diameter); } catch { }


            Guid? braId = null;

            try { braId = Guid.Parse(cmbBrand.Value.ToString()); } catch { }

            var products = (from pro in db.products
                            where pro.proStatus == true
                            && pro.categoriesDP.cdpStatus == true
                            && (catId == 0 || pro.catId == catId)
                            && (width == 0 || pro.proDimensionWidth == width)
                            && (profile == 0 || pro.proDimensionProfile == profile)
                            && (diameter == 0 || pro.proDimensionDiameter == diameter)
                            && (braId.HasValue == false || pro.braId == braId)
                            select new
                            {
                                Id = pro.proId,
                                Image = pro.proImage,
                                CategoryImage = pro.category.catImage,
                                Brand = pro.brand.braName,
                                BrandImage = pro.brand.braImage,
                                Name = pro.proName,
                                Width = pro.proDimensionWidth,
                                Profile = pro.proDimensionProfile,
                                Diameter = pro.proDimensionDiameter,
                                TyreSize = pro.proTyreSize,
                                Fuel = pro.proFuel,
                                Wet = pro.proWet,
                                Noise = pro.proNoise,
                                Price = pro.proSuggestedPrice,
                                Stock = pro.proInventory
                            });

            if (cmbPrice.SelectedIndex == 0) products = products.OrderBy(c => c.Price);
            else products = products.OrderByDescending(c => c.Price);

            Session["products"] = products;
            xgvProducts.DataSource = products;
            xgvProducts.DataBind();

            
        }

        protected void xcpProducts_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            loadProducts();
        }
    }
}