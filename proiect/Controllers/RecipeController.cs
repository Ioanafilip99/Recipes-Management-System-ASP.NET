using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class RecipeController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        // toti userii logati/nelogati pot vedea retetele
        public ActionResult Index()
        {
            List<Recipe> recipes = ctx.Recipes.ToList();
            ViewBag.Recipes = recipes;

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Recipe recipe = ctx.Recipes.Find(id);
                if (recipe != null)
                {
                    return View(recipe);
                }
                return HttpNotFound("Couldn't find the recipe with id " + id.ToString() + "!");

            }
            return HttpNotFound("Missing recipe id parameter!");
        }

        // userii logati, cook, admin pot crea retete noi
        [Authorize]
        [HttpGet] // nu este necesar, by default toate actiunile din controller sunt GET
        public ActionResult New()
        {
            Recipe recipe = new Recipe();

            recipe.RecipeTypesList = GetAllRecipeTypes();
            recipe.CooksList = GetAllCooks();
            recipe.IngredientsList = GetAllIngredients();
            recipe.Ingredients = new List<Ingredient>();

            return View(recipe);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(Recipe recipeRequest)
        {
            
             recipeRequest.RecipeTypesList = GetAllRecipeTypes();
             recipeRequest.CooksList = GetAllCooks();

            // memoram intr-o lista doar ingredientele care au fost selectate
            var selectedIngredients = recipeRequest.IngredientsList.Where(r => r.Checked).ToList();
            try
             {
                 if (ModelState.IsValid)
                 {
                    recipeRequest.Ingredients = new List<Ingredient>();
                    for (int i = 0; i < selectedIngredients.Count(); i++)
                    {
                        // retetei pe care vrem sa o adaugam in baza de date ii 
                        // asignam ingredientele selectate 
                        Ingredient ingredient = ctx.Ingredients.Find(selectedIngredients[i].Id);
                        recipeRequest.Ingredients.Add(ingredient);
                    }
                    ctx.Recipes.Add(recipeRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                 return View(recipeRequest);
             }
             catch (Exception e)
             {
                var msg = e.Message;
                return View(recipeRequest);
             }
        }

        // admin si cook pot edita
        [Authorize(Roles = "Admin, Cook")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Recipe recipe = ctx.Recipes.Find(id);
                recipe.RecipeTypesList = GetAllRecipeTypes();
                recipe.CooksList = GetAllCooks();
                recipe.IngredientsList = GetAllIngredients();


                foreach (Ingredient checkedIngredient in recipe.Ingredients)
                {   // iteram prin ingredientele care erau atribuite retetei inainte de momentul accesarii formularului
                    // si le selectam/bifam  in lista de checkbox-uri
                    recipe.IngredientsList.FirstOrDefault(i => i.Id == checkedIngredient.IngredientId).Checked = true;
                }

                if (recipe == null)
                {
                    return HttpNotFound("Couldn't find the recipe with id " + id.ToString());
                }
               
                return View(recipe);
            }
            return HttpNotFound("Missing recipe id parameter!");
        }

        [Authorize(Roles = "Admin, Cook")]
        [HttpPut]
        public ActionResult Edit(int id, Recipe recipeRequest)
        {
            recipeRequest.RecipeTypesList = GetAllRecipeTypes();
            recipeRequest.CooksList = GetAllCooks();

            // preluam reteta pe care vrem sa o modificam din baza de date
            Recipe recipe = ctx.Recipes.Include("Cook").Include("RecipeType")
                        .SingleOrDefault(r => r.RecipeId.Equals(id));

            // memoram intr-o lista doar ingredientele care au fost selectate din formular
            var selectedIngredients = recipeRequest.IngredientsList.Where(r => r.Checked).ToList();

            try
            {
                if (ModelState.IsValid)
                {
                   
                    if (TryUpdateModel(recipe))
                    {
                        recipe.Title = recipeRequest.Title;
                        recipe.Description = recipeRequest.Description;
                        recipe.Steps = recipeRequest.Steps;
                        recipe.CookTime = recipeRequest.CookTime;

                        recipe.Ingredients.Clear();
                        recipe.Ingredients = new List<Ingredient>();

                        for (int i = 0; i < selectedIngredients.Count(); i++)
                        {
                            // cartii pe care vrem sa o editam ii asignam genurile selectate 
                            Ingredient ingredient = ctx.Ingredients.Find(selectedIngredients[i].Id);
                            recipe.Ingredients.Add(ingredient);
                        }

                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(recipeRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(recipeRequest);
            }
        }

        // admin poate sterge
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Recipe recipe = ctx.Recipes.Find(id);
            if (recipe != null)
            {
                ctx.Recipes.Remove(recipe);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the recipe with id " + id.ToString());
        }

        [NonAction]
        public List<CheckBoxViewModel> GetAllIngredients()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach (var ingredient in ctx.Ingredients.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = ingredient.IngredientId,
                    Name = ingredient.Name,
                    Checked = false
                });
            }
            return checkboxList;
        }


        [NonAction] // specificam faptul ca nu este o actiune
        public IEnumerable<SelectListItem> GetAllRecipeTypes()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            foreach (var type in ctx.RecipeTypes.ToList())
            {
                // adaugam in lista elementele necesare pt dropdown
                selectList.Add(new SelectListItem
                {
                    Value = type.RecipeTypeId.ToString(),
                    Text = type.Name
                });
            }
            // returnam lista pentru dropdown
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCooks()
        {
            var selectList = new List<SelectListItem>();
            foreach (var cook in ctx.Cooks.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = cook.CookId.ToString(),
                    Text = cook.Name
                });
            }
            return selectList;
        }

    }
}