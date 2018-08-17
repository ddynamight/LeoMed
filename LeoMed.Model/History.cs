using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoMed.Model
{
     public class History
     {
          public int Id { get; set; }
          public DateTime Date { get; set; }
          public string Type { get; set; }
          public string Details { get; set; }
          public string PatientAppUserId { get; set; }
          public string ProfessionalAppUserId { get; set; }

          public virtual Patient Patient { get; set; }
          public virtual Professional Professional { get; set; }
     }

}
