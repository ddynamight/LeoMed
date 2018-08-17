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
     public class HomeController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               #region Counter Stuff

               ViewData["historyCounter"] = await db.Histories.CountAsync();
               ViewData["blogCounter"] = await db.Blogs.CountAsync();
               ViewData["newsCounter"] = await db.News.CountAsync();
               ViewData["patientCounter"] = await db.Patients.CountAsync();
               ViewData["profCounter"] = await db.Professionals.CountAsync();


               #endregion

               return View();
          }


          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}