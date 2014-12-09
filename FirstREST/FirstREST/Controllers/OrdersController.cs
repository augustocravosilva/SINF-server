using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using FirstREST.Lib_Primavera.Model;
using FirstREST.Lib_Primavera;


namespace FirstREST.Controllers
{
    public class OrdersController : ApiController
    {

        public class OrdersAnswer
        {
            public List<DocVendaForList> orders { get; set; }
        }

        [System.Web.Http.HttpGet]
        public OrdersAnswer WithCustomer([FromUri] string customer)
        {
            OrdersAnswer o = new OrdersAnswer();
            o.orders = Comercial.Encomendas_List(customer);
            return o;
        }
        

        public Lib_Primavera.Model.DocVenda Get(string id)
        {
            Lib_Primavera.Model.DocVenda docvenda = Lib_Primavera.Comercial.Encomenda_Get(id);
            if (docvenda == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return docvenda;
            }
        }
        
        
        public HttpResponseMessage Post(Lib_Primavera.Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.Comercial.Encomendas_New(dv);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created,erro.Descricao);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,erro.Descricao);
            }

        }

        /*
        public HttpResponseMessage Put(int id, Lib_Primavera.Model.Cliente cliente)
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }
        */


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
