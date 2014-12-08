using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class LinhaDocVenda
    {
          public string product_id { get; set; }

          public double unit_price { get; set; }

          public double quatity { get; set; }

          public double total { get; set; }

          public string color { get; set; }

          public string size { get; set; }
    }
}