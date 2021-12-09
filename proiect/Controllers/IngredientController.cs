using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class IngredientController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        
        // toti pot vedea ingredientele
        public ActionResult Index()
        {
            ViewBag.Ingredients = ctx.Ingredients.ToList();
            return View();
        }

        // userii logati, admin, cook pot crea un ingredient nou
        [Authorize]
        public ActionResult New()
        {
            Ingredient ingredient = new Ingredient();
            return View(ingredient);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(Ingredient ingredientTypeRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ctx.Ingredients.Add(ingredientTypeRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(ingredientTypeRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(ingredientTypeRequest);
            }
        }

        // admin, cook pot edita
        [Authorize(Roles = "Admin, Cook")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Ingredient ingredient = ctx.Ingredients.Find(id);
                if (ingredient == null)
                {
                    return HttpNotFound("Couldn't find the ingredient with id " + id.ToString() + "!");
                }
                return View(ingredient);
            }
            return HttpNotFound("Couldn't find the ingredient with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin, Cook")]
        [HttpPut]
        public ActionResult Edit(int id, Ingredient ingredientTypeRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ingredient ingredient = ctx.Ingredients.Find(id);

                    if (TryUpdateModel(ingredient))
                    {
                        ingredient.Name = ingredientTypeRequest.Name;
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(ingredientTypeRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(ingredientTypeRequest);
            }
        }

        // admin poate sterge
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                Ingredient ingredient = ctx.Ingredients.Find(id);
                if (ingredient != null)
                {
                    ctx.Ingredients.Remove(ingredient);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the ingredient with id " + id.ToString() + "!");
            }
            return HttpNotFound("Ingredient id parameter is missing!");
        }
    }
}