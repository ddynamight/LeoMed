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
     [Route("Patients/News/")]
     public class NewsController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.News.ToListAsync());
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}