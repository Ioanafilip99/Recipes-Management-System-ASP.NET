using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
   
    public class CookController : Controller
    {
        
        private ApplicationDbContext ctx = new ApplicationDbContext();
        
        // toti pot vedea bucatarii
        public ActionResult Index()
        {
            List<Cook> cooks = ctx.Cooks.ToList();
            ViewBag.Cooks = cooks;

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Cook cook = ctx.Cooks.Find(id);
                if (cook != null)
                {
                    return View(cook);
                }
                return HttpNotFound("Couldn't find the cook with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing cook id parameter!");
        }

        // userii logati, admin, cook pot crea un bucatar nou
        [Authorize]
        [HttpGet]
        public ActionResult New()
        {
            CookContactViewModel cc = new CookContactViewModel();
            ViewBag.GenderList = GetAllGenderTypes();
                        
            return View(cc);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(CookContactViewModel ccViewModel)
        {
            ViewBag.GenderList = GetAllGenderTypes();
            try
            {
                if (ModelState.IsValid)
                {
                    ContactInfo contact = new ContactInfo
                    {
                        PhoneNumber = ccViewModel.PhoneNumber,
                        Email = ccViewModel.Email,
                        Age = ccViewModel.Age,
                        GenderType = ccViewModel.GenderType
                    };
                    // vom lua din baza de date ambele obiecte
                    ctx.ContactInfos.Add(contact);

                    Cook cook = new Cook
                    {
                        Name = ccViewModel.Name,
                        ContactInfo = contact
                    };
                    ctx.Cooks.Add(cook);

                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(ccViewModel);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(ccViewModel);
            }
        }
        
        // admin, cook pot edita
        [Authorize(Roles = "Admin, Cook")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Cook cook = ctx.Cooks.Find(id);

                if (cook == null)
                {
                    return HttpNotFound("Coludn't find the cook with id " + id.ToString() + "!");
                }
                return View(cook);
            }
            return HttpNotFound("Missing cook id parameter!");
        }

        [Authorize(Roles = "Admin, Cook")]
        [HttpPut]
        public ActionResult Edit(int id, Cook cookRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cook cook = ctx.Cooks.Find(id);
                    ContactInfo contact = ctx.ContactInfos.Find(cook.ContactInfo.ContactInfoId);
                    if (TryUpdateModel(cook))
                    {
                        cook.Name = cookRequest.Name;
                        contact.PhoneNumber = cookRequest.ContactInfo.PhoneNumber;
                        contact.Email = cookRequest.ContactInfo.Email;
                        contact.Age = cookRequest.ContactInfo.Age;
                        contact.GenderType = cookRequest.ContactInfo.GenderType;

                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(cookRequest);
            }
            catch (Exception)
            {
                return View(cookRequest);
            }
        }

        // admin poate sterge
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Cook cook = ctx.Cooks.Find(id);
            ContactInfo contact = ctx.ContactInfos.Find(cook.ContactInfo.ContactInfoId);

            if (cook != null)
            {
                ctx.Cooks.Remove(cook);
                ctx.ContactInfos.Remove(contact);
                var list = ctx.Recipes.Where(t => t.Cook.CookId == cook.CookId);
                foreach (var r in list)
                {
                    ctx.Recipes.Remove(r);
                }
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the cook with id " + id.ToString() + "!");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllGenderTypes()
        {
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem
            {
                Value = Gender.Male.ToString(),
                Text = "Male"
            });

            selectList.Add(new SelectListItem
            {
                Value = Gender.Female.ToString(),
                Text = "Female"
            });

            return selectList;
        }


    }
}