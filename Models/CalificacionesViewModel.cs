using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MundoCanjeWeb.Models
{
    public class CalificacionesViewModel
    {
        public int Id { get; set; }
        public int UsuarioCalificador { get; set; }
        public int UsuarioCalificado { get; set; }        
        public int Puntuacion { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaAlta { get; set; }
        public string ImgUsuarioCalificador { get; set; }
        public string ImgUsuarioCalificado { get; set; }
        public string Nombre_UsuarioCalificador { get; set; }
        public string Nombre_UsuarioCalificado { get; set; }
    }
    




}