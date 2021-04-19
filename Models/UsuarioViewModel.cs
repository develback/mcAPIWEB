using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MundoCanjeWeb.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public int Telefono { get; set; }
        public int Whatsapp { get; set; }
        public string Mail { get; set; }
        public string Direccion { get; set; }
        public string token { get; set; }
        public int Estado { get; set; }
        public int IdTipo { get; set; }
        public string Cuit { get; set; }
        public string Razon_Social { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public int Puntuacion { get; set; }
        public string Imagen { get; set; }
        public int IdPlan { get; set; }
        public int IdLocalidad { get; set; }
        public DateTime Fecha_Alta { get; set; }
        public string Localidad { get; set; }
        public string RubroUsuario { get; set; }
        

    }

    public class UsuarioCategoriasViewModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public bool Checked { get; set; }
    }

    public class UsuarioNotificacionesVM
    {
        public int IdNotificacion { get; set; }
        public int IdPedido { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }        
        public string ImgUsuario { get; set; }        
        public int PuntajeUsuario { get; set; }
        public string Observacion { get; set; }
        public string Tipo { get; set; }
        

    }


}