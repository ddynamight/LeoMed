using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeoMed.Model;
using Microsoft.EntityFrameworkCore;

namespace LeoMed.Web.Areas.Patients.Controllers
{
     [Area("Patients")]
     [Route("Patients/Histories/")]
     public class HistoriesController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.Histories.Where(e => e.Patient.AppUser.UserName == User.Identity.Name).Include(e => e.Professional).ThenInclude(e => e.AppUser).ToListAsync());
          }

          [HttpGet("Details/{tag}")]
          public async Task<IActionResult> Details(int tag)
          {
               return View(await db.Histories.Include(e => e.Professional).ThenInclude(e => e.AppUser).Include(e => e.Patient).ThenInclude(e => e.AppUser).SingleAsync(e => e.Id == tag));
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}