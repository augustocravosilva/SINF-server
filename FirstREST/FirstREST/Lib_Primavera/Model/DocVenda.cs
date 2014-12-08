using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class DocVendaForList
    {
        public string id
        {
            get;
            set;
        }


        public DateTime date
        {
            get;
            set;
        }

        public double total
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }
    }

    public class DocVenda
    {

        public string id
        {
            get;
            set;
        }

        public string customer
        {
            get;
            set;
        }


        public DateTime date
        {
            get;
            set;
        }

        public double total
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }

        public List<Model.LinhaDocVenda> lines

        {
            get;
            set;
        }



        public string delivery_adress { get; set; }

        public string delivery_city { get; set; }

        public string delivery_zip { get; set; }
    }
}