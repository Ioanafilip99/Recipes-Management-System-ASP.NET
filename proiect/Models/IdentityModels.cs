using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace proiect.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeType> RecipeTypes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Cook> Cooks { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }


        public static void addIntoDb(ApplicationDbContext ctx)
        {

            RecipeType type1 = new RecipeType { RecipeTypeId = 1, Name = "Antreu" };
            RecipeType type2 = new RecipeType { RecipeTypeId = 2, Name = "Fel principal" };
            RecipeType type3 = new RecipeType { RecipeTypeId = 3, Name = "Desert" };

            ctx.RecipeTypes.Add(type1);
            ctx.RecipeTypes.Add(type2);
            ctx.RecipeTypes.Add(type3);

            ctx.Recipes.Add(new Recipe
            {
                Title = "Cheesecake cu capsuni",
                Description = "Acest cheesecake cu capsuni fara coacere este extrem de fin si de cremos si foarte aromat.",
                Steps = "Baza tortului am facut-o din piscoturi. Am aliniat piscoturile intr-o forma de tort cu fund detasabil de 28 cm si le-am decupat dupa masura acesteia. Piscoturile nu le-am insiropat pentru ca ele se vor inmuia de la crema (vor trage umiditatea din ea).Fructele le-am avut congelate. Le-am lasat un pic sa se dezghete si le-am pus in blender. Am folosit fragute si capsuni. Am facut o pasta fina pe care am dat-o printr-o sita pentru a opri semintele mai mari. Fragutele au seminte mici de tot asa ca oricum nu putem sa le scoatem pe toate. Intr-un alt castron punem mascarponele, crema de branza (cream cheese) si smantana pentru frisca. Batem totul deodata si suntem cu ochii pe mixer ca nu cumva sa se taie crema. In final adaugam si zaharul pudra si vanilia si mai mixam foarte putin (cateva secunde)",
                CookTime = 45,
                Cook = new Cook
                {
                    Name = "Cristina",
                    ContactInfo = new ContactInfo
                    {
                        PhoneNumber = "0712345678",
                        Email = "cristina@yahoo.com",
                        Age = 40,
                        GenderType = Gender.Female
                    }
                },
                RecipeTypeId = type3.RecipeTypeId,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Name = "biscuiti"
                    },
                    new Ingredient
                    {
                        Name = "oua"
                    },
                    new Ingredient
                    {
                        Name = "unt"
                    },
                    new Ingredient
                    {
                        Name = "crema de branza"
                    },
                    new Ingredient
                    {
                        Name = "capsuni"
                    },
                    new Ingredient
                    {
                        Name = "smantana"
                    },
                    new Ingredient
                    {
                        Name = "zahar"
                    }
                }
            });

            ctx.Recipes.Add(new Recipe
            {
                Title = "Sarmale",
                Description = "Aceste sarmale traditionale nu lipsesc de pe mesele de sarbatoare ale romanilor.",
                Steps = "Condimentele tipice pe care le folosim la sarmale sunt sarea, piperul, cimbrul uscat si putina paprika (boia dulce). Pe langa sarmale mai punem in oala si bucatele de carne de porc mai grasa (piept sau costite).Intr-o cratita am asezat 1/2 de ceapa tocata si o lingura de ulei si am calit-o la foc mediu (cu putina sare) pana a devenit translucida, fara sa se rumeneasca. Am adaugat in cratita si orezul (spalat in cateva ape) si l-am calit si pe acesta 2-3 minute, amestecand continuu ca sa nu se prinda. Am stins focul si am lasat cratita la racorit.Am asezat carnea tocata de porc intr-un bol incapator si am asezonat-o cu sare, piper negru macinat, boia dulce si cimbru uscat. Am adaugat si orezul calit cu ceapa si am framantat bine compozitia. Boiaua ii da culoare si gust.",
                CookTime = 45,
                Cook = new Cook
                {
                    Name = "Mihaela",
                    ContactInfo = new ContactInfo
                    {
                        PhoneNumber = "0787654321",
                        Email = "mihaela@yahoo.com",
                        Age = 44,
                        GenderType = Gender.Female
                    }
                },
                RecipeTypeId = type3.RecipeTypeId,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Name = "carne"
                    },
                    new Ingredient
                    {
                        Name = "orez"
                    },
                    new Ingredient
                    {
                        Name = "foi de varza"
                    },
                    new Ingredient
                    {
                        Name = "ceapa"
                    },
                    new Ingredient
                    {
                        Name = "piper"
                    },
                    new Ingredient
                    {
                        Name = "sare"
                    },
                    new Ingredient
                    {
                        Name = "boia"
                    }
                }
            });

            ctx.SaveChanges();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public class Initp : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext ctx)
            {
                addIntoDb(ctx);
                base.Seed(ctx);
            }
        }

    }

}