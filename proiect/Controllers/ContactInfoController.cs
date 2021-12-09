using proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class ContactInfoController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        // userii logati, cook, admin pot vedea contactele
        [Authorize]
        public ActionResult Index()
        {
            List<ContactInfo> contactInfos = ctx.ContactInfos.ToList();
            ViewBag.ContactInfos = contactInfos;

            return View();
        }

        // admin si cook pot crea contacte noi
        [Authorize]
        [HttpGet]
        public ActionResult New()
        {
            ContactInfo contact = new ContactInfo();
            ViewBag.GenderList = GetAllGenderTypes();
            return View(contact);
        }

        [Authorize]
        [HttpPost]
        public ActionResult New(ContactInfo contactRequest)
        {
            ViewBag.GenderList = GetAllGenderTypes();
            try
            {
                if (ModelState.IsValid)
                {
                    ctx.ContactInfos.Add(contactRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "ContactInfo");
                }
                return View(contactRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(contactRequest);
            }
        }

        // admin si cook pot edita contactele
        [Authorize(Roles = "Admin, Cook")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                ContactInfo contactInfo = ctx.ContactInfos.Find(id);
                if (contactInfo == null)
                {
                    return HttpNotFound("Couldn't find the contact with id " + id.ToString() + "!");
                }
                return View(contactInfo);
            }
            return HttpNotFound("Couldn't find the contact with id " + id.ToString() + "!");
        }

        [Authorize(Roles = "Admin, Cook")]
        [HttpPut]
        public ActionResult Edit(int id, ContactInfo contactInfoRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactInfo contactInfo = ctx.ContactInfos.Find(id);
                    if (TryUpdateModel(contactInfo))
                    {
                        contactInfo.PhoneNumber = contactInfoRequest.PhoneNumber;
                        contactInfo.Email = contactInfoRequest.Email;
                        contactInfo.Age = contactInfoRequest.Age;
                        contactInfo.GenderType = contactInfoRequest.GenderType;
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index", "ContactInfo");
                }
                return View(contactInfoRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(contactInfoRequest);
            }
        }

        // admin poate sterge contactele
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                ContactInfo contactInfo = ctx.ContactInfos.Find(id);
                if (contactInfo != null)
                {
                    var list = ctx.Cooks.Where(t => t.ContactInfo.ContactInfoId == contactInfo.ContactInfoId);
                    foreach (var c in list)
                    {
                        ctx.Cooks.Remove(c);
                    }
                    ctx.ContactInfos.Remove(contactInfo);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "ContactInfo");
                }
                return HttpNotFound("Couldn't find the contact info with id " + id.ToString() + "!");
            }
            return HttpNotFound("Contact info id parameter is missing!");
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