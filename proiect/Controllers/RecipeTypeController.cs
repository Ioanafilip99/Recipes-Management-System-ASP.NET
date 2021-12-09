using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class RecipeTypeController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        
        // toti pot vedea tipurile de retete
        public ActionResult Index()
        {
            ViewBag.RecipeTypes = ctx.RecipeTypes.ToList();
            return View();
        }

        // userii logati, admin, cook pot crea un tip nou
        [Authorize]
        public ActionResult New()
        {
            RecipeType recipeType = new RecipeType();
            return View(recipeType);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(RecipeType recipeTypeRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ctx.RecipeTypes.Add(recipeTypeRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(recipeTypeRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(recipeTypeRequest);
            }
        }

        // admin si cook pot edita
        [Authorize(Roles = "Admin, Cook")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                RecipeType recipeType = ctx.RecipeTypes.Find(id);
                if (recipeType == null)
                {
                    return HttpNotFound("Couldn't find the recipe type with id " + id.ToString() + "!");
                }
                return View(recipeType);
            }
            return HttpNotFound("Couldn't find the recipe type with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin, Cook")]
        [HttpPut]
        public ActionResult Edit(int id, RecipeType recipeTypeRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RecipeType recipeType = ctx.RecipeTypes.Find(id);
                    if (TryUpdateModel(recipeType))
                    {
                        recipeType.Name = recipeTypeRequest.Name;
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(recipeTypeRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(recipeTypeRequest);
            }
        }

        // admin poate sterge
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                RecipeType recipeType = ctx.RecipeTypes.Find(id);
                if (recipeType != null)
                {
                    ctx.RecipeTypes.Remove(recipeType);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Couldn't find the recipe type with id " + id.ToString() + "!");
            }
            return HttpNotFound("Recipe type id parameter is missing!");
        }
    }
}