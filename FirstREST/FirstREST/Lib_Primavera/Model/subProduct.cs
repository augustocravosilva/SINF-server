using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class SubProduct
    {
        public string color { get; set; }

        public string size { get; set; }

        public int stock { get; set; }

        public string id { get; set; }

        public string[] stock_shops { get; set; }

    }
}