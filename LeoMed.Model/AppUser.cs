using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeoMed.Model
{
     public class AppUser : IdentityUser
     {
          public string Title { get; set; }
          public string Firstname { get; set; }
          public string Middlename { get; set; }
          public string Lastname  { get; set; }
          public string Sex { get; set; }
          public DateTime DateOfBirth { get; set; }


          public virtual Patient Patient { get; set; }
          public virtual Professional Professional { get; set; }
     }
}
