using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoMed.Model
{
     public class Patient
     {
          public Patient()
          {
               this.Histories = new HashSet<History>();
          }

          public string AppUserId { get; set; }
          public string PatientNo { get; set; }
          public string Location { get; set; }
          public string State { get; set; }
          public string Country { get; set; }


          public IEnumerable<History> Histories { get; set; }
          public virtual AppUser AppUser { get; set; }
     }
}
