using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class ImagesController : ApiController
    {

        public HttpResponseMessage Get(string id)
        {
            try
            {
                string[] filePaths = Directory.GetFiles(@"C:\Program Files\PRIMAVERA\SG800\Dados\LP\ANEXOS\", id + ".*");
                var path = filePaths[0];
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(path, FileMode.Open);
                result.Content = new StreamContent(stream);
                if(path.EndsWith("jpg",true,null))
                 result.Content.Headers.ContentType = new MediaTypeHeaderValue("Image/JPG");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = id + path.Substring(path.LastIndexOf("."))
                };
                return result;
            }


            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }


        }
    }
}
