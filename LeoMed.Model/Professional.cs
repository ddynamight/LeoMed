using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoMed.Model
{
     public class Professional
     {
          public Professional()
          {
               this.Histories = new HashSet<History>();
          }

          public string AppUserId { get; set; }
          public string Description { get; set; }
          public string Profession { get; set; }
          public int Experience { get; set; }


          public IEnumerable<History> Histories { get; set; }
          public virtual AppUser AppUser { get; set; }
     }
}
