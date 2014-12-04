using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Artigo
    {

        public string id { get; set; }

        public string name { get; set; }

        public string category { get; set; }

        public string brand { get; set; }

        public double price { get; set; }

        public string material { get; set; }

        public string description { get; set; }

        public SubProduct[] subproducts { get; set; }

        public string[] image_links { get; set; }

        public string specs_link { get; set; }
    }
}