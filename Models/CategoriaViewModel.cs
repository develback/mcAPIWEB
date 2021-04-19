using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MundoCanjeWeb.Models
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string Logo { get; set; }
        public int CantProductos { get; set; }
    }
    public class TotalesCategoriaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CantUsuarios { get; set; }
        public int CantProductos { get; set; }
        public int CantHombres { get; set; }
        public int CantMujeres { get; set; }
        public int CantNinios { get; set; }
        public int CantDescuentos { get; set; }
    }



}