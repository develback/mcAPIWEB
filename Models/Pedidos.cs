//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MundoCanjeWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedidos
    {
        public int Id { get; set; }
        public Nullable<int> IdProductoInteres { get; set; }
        public Nullable<int> IdUsuarioSolicita { get; set; }
        public Nullable<int> IdPedido_Estado { get; set; }
        public Nullable<System.DateTime> FechaPedido { get; set; }
        public Nullable<System.DateTime> FechaEntrega { get; set; }
        public string Comentarios { get; set; }
        public Nullable<decimal> Importe { get; set; }
        public Nullable<int> IdUsuarioRecibe { get; set; }
        public string TipoMatch { get; set; }
    
        public virtual Pedidos_Estados Pedidos_Estados { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}
