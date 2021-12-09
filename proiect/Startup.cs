using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using proiect.Models;

[assembly: OwinStartupAttribute(typeof(proiect.Startup))]
namespace proiect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminAndUserRoles();
        }
        private void CreateAdminAndUserRoles()
        {
            var ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(ctx));
            // adaugam rolurile pe care le poate avea un utilizator
            // din cadrul aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // adaugam rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                var adminCreated = userManager.Create(user, "Admin2020!");
                if (adminCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
            // ATENTIE !!! Pentru proiecte, pentru a adauga un rol nou trebuie sa adaugati secventa:
            if (!roleManager.RoleExists("Cook"))
            {
                // adaugati rolul specific aplicatiei voastre
                var role = new IdentityRole();
                role.Name = "Cook";
                roleManager.Create(role);
                // se adauga utilizatorul bucatar
                var user = new ApplicationUser();
                user.UserName = "cook@cook.com";
                user.Email = "cook@cook.com";
                
                var cookCreated = userManager.Create(user, "Cook2020!");
                if (cookCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Cook");
                }
            }
        }
    }
}
