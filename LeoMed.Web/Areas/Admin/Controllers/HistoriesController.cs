using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeoMed.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeoMed.Web.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Route("Admin/Histories/")]
     public class HistoriesController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               #region DDL Stuff

               var users = await db.Patients.ToListAsync();

               ViewData["patientDdl"] = new SelectList(await db.Patients.ToListAsync(), "AppUserId", "PatientNo");

               #endregion

               return View(await db.Histories.Include(e => e.Professional).ThenInclude(e => e.AppUser).Include(e => e.Patient).ThenInclude(e => e.AppUser).ToListAsync());
          }

          [HttpGet("Details/{tag}")]
          public async Task<IActionResult> Details(int tag)
          {
               return View(await db.Histories.Include(e => e.Professional).ThenInclude(e => e.AppUser).Include(e => e.Patient).ThenInclude(e => e.AppUser).SingleAsync(e => e.Id == tag));
          }

          [HttpPost]
          public async Task<IActionResult> Create(IFormCollection formCollection)
          {
               History history = new History();

               history.Professional = await db.Professionals.SingleAsync(e => e.AppUser.UserName.Equals(User.Identity.Name));

               history.Date = DateTime.Now;
               history.Type = formCollection["Type"];
               history.Details = formCollection["Details"];
               history.PatientAppUserId = formCollection["PatientAppUserId"];




               if (ModelState.IsValid)
               {
                    await db.Histories.AddAsync(history);
                    db.Entry(history).State = EntityState.Added;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
               }

               return NotFound();
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}