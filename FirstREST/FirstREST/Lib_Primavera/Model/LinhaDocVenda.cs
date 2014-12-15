using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class LinhaDocVenda
    {
         public LinhaDocVenda()
          {
              quantity = 0.0;
          }
          public string product_id { get; set; }

          public double unit_price { get; set; }

          public double quantity { get; set; }

          public double total { get; set; }

          public string color { get; set; }

          public string size { get; set; }
    }
}