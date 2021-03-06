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
     [Route("Admin/Users/")]
     public class UsersController : Controller
     {
          AppDbContext db = new AppDbContext();

          public async Task<IActionResult> Index()
          {
               return View(await db.AppUsers.Include(e => e.Patient).Include(e => e.Professional).ToListAsync());
          }

          protected override void Dispose(bool disposing)
          {
               db.Dispose();
               base.Dispose(disposing);
          }
     }
}