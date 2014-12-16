using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;


namespace FirstREST.Controllers
{
    public class ProductsController : ApiController
    {

        public class ProductAnswer
        {
           public Artigo product { get; set; }
        }

        public class ProductsAnswer
        {
            public List<Artigo> products { get; set; }
        }

        //
        // GET: /Artigos/

        public ProductsAnswer Get()
        {
            ProductsAnswer answer = new ProductsAnswer();
            answer.products = Lib_Primavera.Comercial.ListaArtigos();
            return answer;
        }

        [HttpGet]
        public ProductsAnswer ByCategory(string id)
        {
            ProductsAnswer answer = new ProductsAnswer();
            answer.products = Lib_Primavera.Comercial.ListaArtigos(id);
            return answer;
        }

        [HttpGet]
        public ProductsAnswer hot(string id)
        {
            ProductsAnswer answer = new ProductsAnswer();
            answer.products = Lib_Primavera.Comercial.ListaArtigosMaisVendidos(id);
            return answer;
        }

        [HttpGet]
        public ProductsAnswer fast(string id)
        {
            ProductsAnswer answer = new ProductsAnswer();
            answer.products = Lib_Primavera.Comercial.ListaMaisStock(id);
            return answer;
        }

        // GET api/artigo/5    
        public ProductAnswer Get(string id)
        {
            Lib_Primavera.Model.Artigo artigo = Lib_Primavera.Comercial.GetArtigo(id);
            if (artigo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                ProductAnswer pa = new ProductAnswer();
                pa.product = artigo;
                return pa;
            }
        }

    }
}

