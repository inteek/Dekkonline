using Microsoft.AspNet.Identity.EntityFramework;

namespace DekkOnlineMVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Data Source=91.207.159.138,4171;Initial Catalog=dekkOnline;User ID=dekk;Password=dekkOnline!?")
        {
        }
    }
}