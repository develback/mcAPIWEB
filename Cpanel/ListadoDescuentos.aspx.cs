using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MundoCanjeWeb.Cpanel.Clases;
using System.Threading.Tasks;
using System.Web.Services;

namespace MundoCanjeWeb.Cpanel
{
    public partial class ListadoDescuentos : System.Web.UI.Page
    {
        public void IniciarControles()
        {
            HdnIdDescuento.Value = "0";
            #region CargarCombo

            ApiServices objApi = new ApiServices();
            string Request = "{}";
            HttpResponseMessage response = objApi.CallService("usuarios/GetUsuariosByTipo/2", Request, ApiServices.TypeMethods.GET).Result;

            if (response.IsSuccessStatusCode)
            {
                string Respuesta = response.Content.ReadAsStringAsync().Result;
                List<Models.Usuarios> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Usuarios>>(Respuesta);
                DlComercio.DataSource = obj;
                DlComercio.DataTextField = "Nombre";
                DlComercio.DataValueField = "Id";
                DlComercio.DataBind();
            }
            else
            {
                string RespuestaService = response.Content.ReadAsStringAsync().Result;
                ApiServices.Response obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiServices.Response>(RespuestaService);
                RespuestaService = response.StatusCode + " - " + obj.Error.message;
            }

            //////
            HttpResponseMessage response2 = objApi.CallService("categorias", Request, ApiServices.TypeMethods.GET).Result;

            if (response2.IsSuccessStatusCode)
            {
                string Respuesta = response2.Content.ReadAsStringAsync().Result;
                List<Models.Categorias> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Categorias>>(Respuesta);
                DlCategoria.DataSource = obj;
                DlCategoria.DataTextField = "Nombre";
                DlCategoria.DataValueField = "Id";
                DlCategoria.DataBind();
            }
            else
            {
                string RespuestaService2 = response2.Content.ReadAsStringAsync().Result;
                ApiServices.Response obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiServices.Response>(RespuestaService2);
                RespuestaService2 = response2.StatusCode + " - " + obj.Error.message;
            }
            #endregion

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["Admin"] == null)
            //{
            //    Response.Redirect("Login.aspx");
            //}
            //else
            //{
                if (!IsPostBack)
                {
                    try
                    {
                        string idEstado = Request.QueryString["Est"];
                        IniciarControles();

                        if (idEstado != null)
                        {
                            GetDetalleGrilla(Convert.ToInt32(idEstado));
                        }
                        else
                            GetDetalleGrilla(1);
                    }
                    catch (Exception ex)
                    {
                        //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
            //}
        }

        public void GetDetalleGrilla(int idEstado)
        {
            switch (idEstado)
            {
                case 1: //Pendiente                        
                    LblTituloGrilla.Text = "Listado de Descuentos Pendientes";
                    break;
                case 4: //Confirmados                        
                    LblTituloGrilla.Text = "Listado de Descuentos Confirmados";
                    break;
            }

            ApiServices objApi = new ApiServices();
            string Request = "{}";
            HttpResponseMessage response=objApi.CallService("Pedidos/DescuentosByState/" + idEstado, Request, ApiServices.TypeMethods.GET).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string Respuesta = response.Content.ReadAsStringAsync().Result;
                List<Models.PedidoViewModel> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.PedidoViewModel>>(Respuesta);
                string data = "";
                
                foreach (var item in obj)
                {
                    
                    data += "<tr> ";
                    data += "<td> <img src='" + item.ImgUsuarioSolicita + "'> " + item.Nombre_UsuarioSolicita + " </td>  ";

                    data += "<td> " + item.NombreProductoInteres + " </td> ";
                    data += "<td><label class='badge badge-gradient-success'>" + item.Desc_Estado+ "</label></td>  ";
                    data += "<td> " + item.FechaPedido + " </td> ";
                    data += "<td> " + item.Id + " </td> ";
                    data += "<td> " + item.CodigoDescuento + " </td> ";
                    data += "<td style='font-size: x-large'>  ";
                    if (idEstado > 1)
                        data += "<a style='cursor:pointer' onclick='VerDetalle(" + item.Id + ");return false' ><i class='mdi mdi-magnify'></i><span class='count-symbol bg-warning'></span></a> ";
                    if(idEstado==1)
                        data += "<a style='cursor:pointer' onclick='SetDeleteId(" + item.Id + ");return false' ><i class='mdi mdi-delete-outline'></i><span class='count-symbol bg-warning'></span></a> ";
                    
                         data += "<a style='cursor:pointer' onclick='GetEditId(" + item.Id + ");return false' ><i class='mdi mdi-pencil'></i><span class='count-symbol bg-warning'></span></a> ";
                    data += "</td></td>	</tr> ";

                }
                LitGrilla.Text = data;
            }
            else
            {
                string RespuestaService = response.Content.ReadAsStringAsync().Result;
                ApiServices.Response obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiServices.Response>(RespuestaService);
                RespuestaService = response.StatusCode + " - " + obj.Error.message;
            }
            //return ListaOrdenes;
        }
        [WebMethod]
        public static List<Models.ProductoEditVM> IniModalEdit(string Id)
        {
            List<Models.ProductoEditVM> lista = new List<Models.ProductoEditVM>();
            try
            {
                if (Id != "0")
                {
                    Int64 IdPedido = Convert.ToInt64(Id);
                    ApiServices objApi = new ApiServices();
                    string Request = "{}";
                    HttpResponseMessage response = objApi.CallService("productos/ProductsByIdProd/" + IdPedido, Request, ApiServices.TypeMethods.GET).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        //resp = await response.Content.ReadAsAsync();
                        string Respuesta = response.Content.ReadAsStringAsync().Result;
                        Models.ItemVM obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ItemVM>(Respuesta);
                        if (obj != null)
                        {
                            //lista.Add(new Models.Productos
                            //{
                            //    Id = obj.Id,
                            //    Nombre = obj.Nombre,
                            //    Descripcion = obj.Descripcion,
                            //    CodigoDescuento = obj.CodigoDescuento,
                            //    Cantidad = obj.Cantidad,
                            //    PorcentajeDescuento = obj.PorcentajeDesc,
                            //    FechaVencimiento = obj.FechaVencimiento,
                            //    Imagen = obj.Imagen,
                            //    IdUsuario = obj.IdUsuario
                            //}); 
                            lista.Add(new Models.ProductoEditVM
                            {
                                Id = obj.Id,
                                Nombre = obj.Nombre,
                                Descripcion = obj.Descripcion,
                                CodigoDescuento = obj.CodigoDescuento,
                                Cantidad = obj.Cantidad,
                                PorcentajeDescuento = obj.PorcentajeDesc,
                                FechaVencimiento = obj.FechaVencimiento,
                                Imagen = obj.Imagen,
                                IdUsuario = obj.IdUsuario,
                                IdCategoria = obj.IdCategoria,
                                FechaVenceAnio = (obj.FechaVencimiento!=null) ? obj.FechaVencimiento.Value.Year : 2020,
                                FechaVenceMes = (obj.FechaVencimiento != null) ? obj.FechaVencimiento.Value.Month : 12,
                                FechaVenceDia = (obj.FechaVencimiento != null) ? obj.FechaVencimiento.Value.Day.ToString().PadLeft(2,'0') : "31",
                            });
                        }

                    }

                }
            }
            catch
            {
                int sss = 0;
            }
            return lista;


        }


        [WebMethod]
        public static int Grabar(string Nombre, string Desc, string Codigo, string Imagen, string IdUsuario, string Cantidad, string PorcDesc,string FechaVence,int flagoEsNuevo,int IdProducto,int IdCategoria)
        {
            try
            {
                ////////////////////////////
                string LatComercio = "", LngComercio = "";
                #region Get Latitud y Longitud Comercio
                try
                {
                    ApiServices objApiUsuario = new ApiServices();                    
                    HttpResponseMessage respUsuario = objApiUsuario.CallService("usuarios/" + IdUsuario, "{}", ApiServices.TypeMethods.GET).Result;
                    if (respUsuario.IsSuccessStatusCode)
                    {
                        //resp = await response.Content.ReadAsAsync();
                        string Respuesta = respUsuario.Content.ReadAsStringAsync().Result;
                        Models.Usuarios obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Usuarios>(Respuesta);
                        if (obj != null)
                        {
                            LatComercio = obj.Lat;
                            LngComercio = obj.Long;
                        }

                    }
                }
                catch
                {

                }
                #endregion    


                ////////////////////////


                string CodDescuento = (string.IsNullOrEmpty(Codigo)) ? new Models.FuncGrales().GenerateRandom(8) : Codigo;
                Models.Productos objProd = new Models.Productos()
                {
                    Id = (flagoEsNuevo == 1)? 0: IdProducto,
                    Nombre = Nombre,
                    Descripcion = Desc,
                    CodigoDescuento = CodDescuento,
                    Imagen = Imagen,
                    IdUsuario = Convert.ToInt32(IdUsuario),
                    Cantidad = Convert.ToInt32(Cantidad),
                    IdTipo = 2,
                    IdEstado = 1,
                    Importe = 0,
                    Fecha_Publicacion = DateTime.Now,
                    TipoDespublicacion = 1,
                    IdCategoria = IdCategoria,
                    PorcentajeDescuento = PorcDesc,
                    FechaVencimiento = Convert.ToDateTime(FechaVence),
                    lat = LatComercio,
                    lng = LngComercio
                };

                ApiServices objApi = new ApiServices();
                HttpResponseMessage response = null;
                string Request = Newtonsoft.Json.JsonConvert.SerializeObject(objProd);
                if (flagoEsNuevo==1)
                {
                    response = objApi.CallService("productos", Request, ApiServices.TypeMethods.POST).Result;
                }
                else
                {                    
                    response = objApi.CallService("productos/"+ IdProducto, Request, ApiServices.TypeMethods.PUT).Result;
                }
                

                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return 0;
            }

        }

        [WebMethod]
        public static int Eliminar(int idDesc)
        {
            try
            {
                if (idDesc > 0)
                {
                    ApiServices objApi = new ApiServices();
                    HttpResponseMessage response = null;
                    string Request = "{}";
                    response = objApi.CallService("productos/" + idDesc, Request, ApiServices.TypeMethods.DELETE).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return 0;
            }

        }

        [WebMethod]
        public static int GrabarCambioEstado(int IdUsuario, int OnOff)
        {
            try
            {
                if (IdUsuario > 0)
                {
                    ApiServices objApi = new ApiServices();
                    string Request = "{}";
                    HttpResponseMessage response = objApi.CallService("usuarios/" + IdUsuario, Request, ApiServices.TypeMethods.GET).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string Respuesta = response.Content.ReadAsStringAsync().Result;
                        Models.Usuarios objUsuario = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Usuarios>(Respuesta);
                        if (objUsuario != null)
                        {
                            objUsuario.Estado = OnOff;
                            response = null;
                            
                            string Request2 = Newtonsoft.Json.JsonConvert.SerializeObject(objUsuario, new Newtonsoft.Json.JsonSerializerSettings()
                            {
                                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects,
                                Formatting = Newtonsoft.Json.Formatting.Indented
                            });
                            response = objApi.CallService("usuarios/" + objUsuario.Id, Request2, ApiServices.TypeMethods.PUT).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                return 1;
                            }                            
                        }
                        return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return 0;
            }

        }

        protected void DlComercio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}