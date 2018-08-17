using System;
using System.Collections.Generic;
using System.Text;

namespace LeoMed.Model
{
     public class News
     {
          public int Id { get; set; }
          public string Title { get; set; }
          public string Description { get; set; }
          public string Article { get; set; }
          public DateTime Date { get; set; }
          public string Image { get; set; }
          public string Status { get; set; }
          public string Tagname { get; set; }
     }
}
