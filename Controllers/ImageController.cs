using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Cors;


namespace MundoCanjeWeb.Controllers
{
    [EnableCors(origins: "https://mundocanjeapp.tk,http://localhost:51199,http://localhost:8100,http://localhost:8000", headers: "*", methods: "*")]
    //[EnableCors(origins: "http://wi200484.ferozo.com", headers: "*", methods: "*")]
    public class ImageController : ApiController
    {

        //[Route("PostUserImage")]
        //[AllowAnonymous]

        [HttpPost]
        [Route("api/image/CategoryImage/")]
        public async Task<HttpResponseMessage> CategoryImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                string PathReturn = "";
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            //var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/Categorias/" + postedFile.FileName + extension);
                            var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/Categorias/" + postedFile.FileName);
                            var urlHost = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                            PathReturn = urlHost + "/Imagenes/Categorias/" + postedFile.FileName;
                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully: "+ PathReturn);
                    return Request.CreateErrorResponse(HttpStatusCode.Created, PathReturn); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        [HttpPost]
        [Route("api/image/CategoryLogoImage/")]
        public async Task<HttpResponseMessage> CategoryLogoImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                string PathReturn = "";
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> {".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        System.Drawing.Image imageSize = System.Drawing.Image.FromStream(postedFile.InputStream);
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("La extensión del archivo debe ser .png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (imageSize.Width!=128 && imageSize.Height != 128)
                        {

                            var message = string.Format("Ingrese el tamaño correcto.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("El tamaño máximo del archivo debe ser 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            //var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/Categorias/" + postedFile.FileName + extension);
                            var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/Categorias/" + postedFile.FileName);
                            var urlHost = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                            PathReturn = urlHost + "/Imagenes/Categorias/" + postedFile.FileName;
                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully: " + PathReturn);
                    return Request.CreateErrorResponse(HttpStatusCode.Created, PathReturn); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpPost]
        [Route("api/image/SaveImage/{Tipo}")]
        public async Task<HttpResponseMessage> SaveImage(string Tipo)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                string PathReturn = "";
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    string PathTipo = "Categorias";
                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".png" };
                    int widthValid = 0, heightValid = 0;
                    switch (Tipo)
                    {
                        case "Usuarios":
                            PathTipo = "Usuarios";
                            //AllowedFileExtensions.Clear();
                            //AllowedFileExtensions.Add(".jpg");
                            //AllowedFileExtensions.Add(".png");
                            widthValid = 320; heightValid = 200;
                            break;
                        case "Productos":
                            PathTipo = "Productos";
                            widthValid = 600; heightValid = 400;
                            break;
                        default:
                            break;
                    }

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB                          
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        System.Drawing.Image imageSize = System.Drawing.Image.FromStream(postedFile.InputStream);

                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("La extensión del archivo debe ser .png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (imageSize.Width != widthValid && imageSize.Height != heightValid)
                        {
                            var message = string.Format("Ingrese el tamaño correcto.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("El tamaño máximo del archivo debe ser 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/"+ PathTipo + "/" + postedFile.FileName);
                            var urlHost = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                            PathReturn = urlHost + "/Imagenes/"+ PathTipo + "/" + postedFile.FileName;
                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully: " + PathReturn);
                    return Request.CreateErrorResponse(HttpStatusCode.Created, PathReturn); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

    }
}
