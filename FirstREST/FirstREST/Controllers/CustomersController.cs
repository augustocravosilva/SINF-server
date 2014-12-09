using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;
using System.Web.Routing;
using System.Web.Optimization;

namespace FirstREST.Controllers
{
    public class CustomersController : ApiController
    {

        // GET: api/Customers/
        public IEnumerable<Lib_Primavera.Model.Cliente> Get()
        {
            return Lib_Primavera.Comercial.ListaClientes();
        }

        public class LogInParams
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public string Login(LogInParams id)
        {
            string res = Lib_Primavera.Comercial.ValidaCliente(id.email, id.password);
            if (res != null && !res.Equals(""))
                return res;
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound)); 
        }

        // GET api/Customers/:id    
        public Cliente Get(string id)
        {
            Lib_Primavera.Model.Cliente cliente = Lib_Primavera.Comercial.GetCliente(id);
            if (cliente == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return cliente;
            }
        }

        // POST api/Customers
        [HttpPost]
        public HttpResponseMessage Post(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = Lib_Primavera.Comercial.InsereClienteObj(cliente);
            
            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }

        // PUT api/Customers/:id
        [HttpPut]
        public HttpResponseMessage Put(string id, Lib_Primavera.Model.Cliente cliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();

            try
            {
                erro = Lib_Primavera.Comercial.UpdCliente(id,cliente);
                if (erro.Erro == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
                }
            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, exc.ToString());
            }
        }



        public HttpResponseMessage Delete(string id)
        {


            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();

            try
            {

                erro = Lib_Primavera.Comercial.DelCliente(id);

                if (erro.Erro == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
                }

            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);

            }

        }


    }
}
