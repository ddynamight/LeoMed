using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeoMed.Model;
using Microsoft.EntityFrameworkCore;

namespace LeoMed.Web.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Route("Admin/Professionals/")]
     public class ProfessionalsController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.Professionals.Include(e => e.Histories).Include(e => e.AppUser).ToListAsync());
          }

          [HttpGet("Details/{tag}")]
          public async Task<IActionResult> Details(string tag)
          {
               return View(await db.Professionals.Include(e => e.Histories).ThenInclude(e => e.Patient).ThenInclude(e => e.AppUser).Include(e => e.AppUser).SingleAsync(e => e.AppUser.UserName == tag));
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}