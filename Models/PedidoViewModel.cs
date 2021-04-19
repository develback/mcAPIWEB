using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MundoCanjeWeb.Models
{
    public class PedidoViewModel
    {
        public int Id { get; set; }
        public int IdProductoInteres { get; set; }
        public int IdUsuarioSolicita { get; set; }
        public int IdUsuarioInteres { get; set; }
        public int IdPedido_Estado { get; set; }
        public string Desc_Estado { get; set; }
        public string NombreProductoInteres { get; set; }
        public string ImgProductoInteres { get; set; }
        public string DescProductoInteres { get; set; }
        public string ImgUsuarioInteres { get; set; }
        public string ImgUsuarioSolicita { get; set; }
        public string Nombre_UsuarioInteres { get; set; }
        public string Nombre_UsuarioSolicita { get; set; }
        public string FechaPedido { get; set; }
        public string Fecha_Entrega { get; set; }
        public Decimal Importe { get; set; }
        public string CodigoDescuento { get; set; }
        public string Img_Comercio { get; set; }
        public string Nombre_Comercio { get; set; }
        public string Comentarios { get; set; }
        public int Ult_Dias { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public bool FueCalificado { get; set; }
        public string TipoMatch { get; set; }
    }
}