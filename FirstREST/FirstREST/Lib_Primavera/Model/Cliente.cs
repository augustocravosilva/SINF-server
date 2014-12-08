using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Cliente
    {
        public string id { get; set; }

        public string name { get; set; }

        public string tax_id { get; set; }

        public string email { get; set; }

        public string street { get; set; }

        public string city { get; set; }

        public string zip_code1 { get; set; }

        public string zip_code2 { get; set; }
    }
}