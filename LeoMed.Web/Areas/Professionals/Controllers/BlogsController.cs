using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeoMed.Model;
using Microsoft.EntityFrameworkCore;

namespace LeoMed.Web.Areas.Professionals.Controllers
{
     [Area("Professionals")]
     [Route("Professionals/Blogs/")]
     public class BlogsController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.Blogs.ToListAsync());
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}