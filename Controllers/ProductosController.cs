using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MundoCanjeWeb.Models;
using System.Web.Http.Cors;

namespace MundoCanjeWeb.Controllers
{
    [EnableCors(origins: "https://mundocanjeapp.tk,http://localhost:51199,http://localhost:8100,http://localhost:8000", headers: "*", methods: "*")]
    //[EnableCors(origins: "http://wi200484.ferozo.com", headers: "*", methods: "*")]
    public class ProductosController : ApiController
    {
        private MundoCanjeDBEntities db = new MundoCanjeDBEntities();

        // GET: api/Productos
        public IQueryable<Productos> GetProductos()
        {
            return db.Productos;
        }

        // GET: api/Productos/5
        //[ResponseType(typeof(Productos))]
        //public IHttpActionResult GetProductos(int id)
        //{
        //    Productos productos = db.Productos.Find(id);
        //    if (productos == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(productos);
        //}
        public ItemVM GetProductos(int id)
        {
            Productos objProd = db.Productos.Find(id);
            int nroSolicitudes = db.Pedidos.Where(x => x.IdProductoInteres == id).Count();//objProd.Pedidos.Count;
            if (objProd == null)
            {
                return null;
            }

            ItemVM listVM = new ItemVM()
            {
                Id = objProd.Id,
                Nombre = objProd.Nombre,
                Descripcion = string.IsNullOrEmpty(objProd.Descripcion) ? "Palermo. Buenos Aires" : objProd.Descripcion,
                IdTipo = objProd.IdTipo,
                Fecha_Publicacion = objProd.Fecha_Publicacion,
                Ult_Dias = (int)DateTime.Now.Subtract(objProd.Fecha_Publicacion.Value).TotalDays,
                Imagen = objProd.Imagen,
                Categoria = objProd.Categorias.Nombre,
                IdCategoria = objProd.IdCategoria.ToString(),
                Importe = objProd.Importe.ToString(),
                lat = string.IsNullOrEmpty(objProd.lat) ? "0" : objProd.lat,
                lng = string.IsNullOrEmpty(objProd.lng) ? "0" : objProd.lng,
                NroSolicitudes = nroSolicitudes.ToString(),
                Usuario = objProd.Usuarios.Nombre,
                UsuarioImagen = objProd.Usuarios.Imagen,
                IdProductoUsuario = objProd.Usuarios.Id.ToString()
            };
            

            return listVM;
        }

        [HttpGet]
        [Route("api/productos/ProductsByUser/{idUsuario}")]
        public List<ItemVM> ProductsByUser(string idUsuario)
        {
            List<Productos> listaProductos = db.Productos.Where(x => x.IdUsuario.ToString().Contains(idUsuario)).ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<ItemVM> listVM = new List<ItemVM>();
            foreach (var item in listaProductos)
            {
                /*
               listVM.Add(new ProductoViewModel
               {
                   Id = item.Id,
                   Nombre = item.Nombre,
                   Descripcion = item.Descripcion,
                   IdTipo = item.IdTipo,
                   IdEstado = item.IdEstado,
                   Importe = item.Importe,
                   Fecha_Publicacion = item.Fecha_Publicacion,
                   TipoDespublicacion = item.TipoDespublicacion,
                   IdCategoria = item.IdCategoria,
                   IdUsuario = item.IdUsuario,
                   Cantidad = item.Cantidad
               });
               */
                listVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = string.IsNullOrEmpty(item.Descripcion) ? "Palermo. Buenos Aires" : item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen
                });

            }

            return listVM;
        }

        [HttpGet]
        [Route("api/productos/ProductsByName/{Nombre}")]
        public List<ItemVM> ProductsByName(string Nombre)
        {
            List<Productos> listaProductos = db.Productos.Where(x => x.IdTipo==1 && x.Nombre.ToUpper().ToString().Contains(Nombre.ToUpper())).ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<ItemVM> listVM = new List<ItemVM>();
            foreach (var item in listaProductos)
            {
               
                listVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = string.IsNullOrEmpty(item.Descripcion) ? "Palermo. Buenos Aires" : item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    IdUsuario = item.IdUsuario ?? 0,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen
                });

            }

            return listVM;
        }

        [HttpGet]
        [Route("api/productos/ProductsByIdProd/{IdProducto}")]
        public ItemVM ProductsByIdProd(int IdProducto)
        {
            Productos listaProductos = db.Productos.Where(x => x.Id== IdProducto).FirstOrDefault();

            if (listaProductos == null)
            {
                return null;
            }
            ItemVM listVM = new ItemVM()
            {
                Id = listaProductos.Id,
                Nombre = listaProductos.Nombre,
                Descripcion = string.IsNullOrEmpty(listaProductos.Descripcion) ? "Palermo. Buenos Aires" : listaProductos.Descripcion,
                IdTipo = listaProductos.IdTipo,
                Fecha_Publicacion = listaProductos.Fecha_Publicacion,
                Ult_Dias = (int)DateTime.Now.Subtract(listaProductos.Fecha_Publicacion.Value).TotalDays,
                Imagen = listaProductos.Imagen,
                Categoria = listaProductos.Categorias.Nombre,
                IdCategoria = listaProductos.IdCategoria.ToString(),
                Importe = listaProductos.Importe.ToString(),
                lat = string.IsNullOrEmpty(listaProductos.lat) ? "0" : listaProductos.lat,
                lng = string.IsNullOrEmpty(listaProductos.lng) ? "0" : listaProductos.lng,
                IdUsuario = listaProductos.IdUsuario ?? 0,
                Usuario = listaProductos.Usuarios.Nombre,
                UsuarioImagen = listaProductos.Usuarios.Imagen,
                CodigoDescuento = listaProductos.CodigoDescuento,
                Cantidad = listaProductos.Cantidad??0,
                FechaVencimiento =listaProductos.FechaVencimiento,
                PorcentajeDesc = listaProductos.PorcentajeDescuento,
                DireccionUsuario = listaProductos.Usuarios.Direccion
            };
            
            return listVM;
        }


        
        [HttpGet]
        [Route("api/productos/HomeApp/")]
        public HomeViewModel HomeApp()
        {
            List<Productos> listaProductos = db.Productos.ToList();

            if (listaProductos == null)
            {
                return null;
            }

            //List<Parametros> listaBanner = db.Parametros.Where(z=>z.Key== "home_banner").ToList();
            List<Productos> listaCanjes = listaProductos.Where(x => x.IdTipo == 1).OrderByDescending(x=> x.Id).Take(4).ToList();
            List<Productos> listaDescuentos = listaProductos.Where(x => x.IdTipo == 2).OrderByDescending(x => x.Id).Take(4).ToList();
            List<Usuarios> listaUsuarios =db.Usuarios.Where(x => x.IdTipo == 1).OrderByDescending(x => x.Id).Take(4).ToList();
            

            List<ItemVM> listItemBannerVM = new List<ItemVM>();
            List<ItemVM> listItemCanjeVM = new List<ItemVM>();
            List<ItemVM> listItemDescVM = new List<ItemVM>();
            List<PerfilesVM> ListPerfilesVM = new List<PerfilesVM>();

            HomeViewModel homeVM = new HomeViewModel();

            //foreach (var item in listaBanner)
            //{
            //    listItemBannerVM.Add(new ItemVM
            //    {
            //        Id = item.Id,
            //        Nombre = item.Key,
            //        Descripcion = "",
            //        IdTipo = 0,
            //        Fecha_Publicacion = DateTime.Now,
            //        Ult_Dias = 1,
            //        Imagen = item.Value
            //    });

            //}
            foreach (var item in listaCanjes)
            {
                listItemCanjeVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    //Descripcion = item.Descripcion,
                    Descripcion = "Palermo. Buenos Aires",
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    IdUsuario = item.IdUsuario??0,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen=item.Usuarios.Imagen
                });

            }
            foreach (var item in listaDescuentos)
            {
                listItemDescVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen,
                    PorcentajeDesc = item.PorcentajeDescuento,
                    FechaVencimiento = item.FechaVencimiento
                });

            }
            foreach (var item in listaUsuarios)
            {
                ListPerfilesVM.Add(new PerfilesVM
                {

                    Id = item.Id,
                    Nombre = item.Nombre,
                    Mail = item.Mail,
                    Imagen = item.Imagen,
                    Distancia = "1 km",
                    CantCanjes = item.Productos.Count()
                });

            }

            homeVM.Banners = listItemBannerVM;
            homeVM.Canjes = listItemCanjeVM;
            homeVM.Descuentos = listItemDescVM;
            homeVM.Usuarios = ListPerfilesVM;

            return homeVM;
        }

        [HttpGet]
        [Route("api/productos/MapApp/")]
        public HomeViewModel MapApp()
        {
            List<Productos> listaProductos = db.Productos.ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<Productos> listaCanjes = listaProductos.Where(x => x.IdTipo == 1).ToList();
            List<Productos> listaDescuentos = listaProductos.Where(x => x.IdTipo == 2).ToList();

            List<ItemVM> listItemCanjeVM = new List<ItemVM>();
            List<ItemVM> listItemDescVM = new List<ItemVM>();
            HomeViewModel homeVM = new HomeViewModel();

            
            foreach (var item in listaCanjes)
            {
                listItemCanjeVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = "",
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    IdUsuario = item.IdUsuario??0,
                    Usuario = item.Usuarios.Nombre
                    //UsuarioImagen = item.Usuarios.Imagen
                });

            }
            foreach (var item in listaDescuentos)
            {
                listItemDescVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Imagen = item.Imagen,
                    IdUsuario = item.IdUsuario ??0,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen,
                    PorcentajeDesc = item.PorcentajeDescuento,
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng
                });

            }
            homeVM.Canjes = listItemCanjeVM;
            homeVM.Descuentos = listItemDescVM;


            return homeVM;
        }


        [HttpGet]
        [Route("api/productos/GetPerfilesByCateg/{IdCateg}")]
        public List<PerfilesVM> GetPerfilesByCateg(int IdCateg)
        {
            var rows =(from p in db.Productos
                       where p.IdCategoria== IdCateg && p.IdTipo==1
                       join o in db.Usuarios on p.IdUsuario equals o.Id
                       group p.Id by o into grp
                       select new
                         {
                           NombreUsuario=grp.Key.Nombre,
                           Mail = grp.Key.Mail,
                           IdUsuario = grp.Key.Id,
                           ImgUsuario = grp.Key.Imagen,
                           Distancia = "1.5 km",
                           CantidadCanjes = grp.Count()
                         }
                        ).ToList();
            List<PerfilesVM> ListPerfiles = new List<PerfilesVM>();
            foreach (var item in rows)
            {
                ListPerfiles.Add(new PerfilesVM
                {
                    
                    Id = item.IdUsuario,
                    Nombre = item.NombreUsuario,
                    Mail = item.Mail,
                    Imagen = item.ImgUsuario,
                    Distancia = item.Distancia,
                    CantCanjes = item.CantidadCanjes
                });

            }

            return ListPerfiles;
        }
        [HttpGet]
        [Route("api/productos/GetAllPerfiles")]
        public List<PerfilesVM> GetAllPerfiles()
        {
            var ListUsuarios = db.Usuarios.Where(x => x.IdTipo == 1).ToList();
            List<PerfilesVM> ListPerfiles = new List<PerfilesVM>();
            foreach (var item in ListUsuarios)
            {
                var objCantCanjes = db.Productos.Where(x => x.IdUsuario == item.Id).Count();
                ListPerfiles.Add(new PerfilesVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Mail = item.Mail,
                    Imagen = item.Imagen,
                    Distancia = "1.5 km",
                    CantCanjes = objCantCanjes
                });

            }
            //////////

            //var rows = (from p in db.Productos
            //            where p.IdTipo == 1
            //            join o in db.Usuarios on p.IdUsuario equals o.Id
            //            group p.Id by o into grp
            //            select new
            //            {
            //                NombreUsuario = grp.Key.Nombre,
            //                Mail = grp.Key.Mail,
            //                IdUsuario = grp.Key.Id,
            //                ImgUsuario = grp.Key.Imagen,
            //                Distancia = "1.5 km",
            //                CantidadCanjes = grp.Count()
            //            }
            //            ).ToList();
            //List<PerfilesVM> ListPerfiles = new List<PerfilesVM>();
            //foreach (var item in rows)
            //{
            //    ListPerfiles.Add(new PerfilesVM
            //    {

            //        Id = item.IdUsuario,
            //        Nombre = item.NombreUsuario,
            //        Mail = item.Mail,
            //        Imagen = item.ImgUsuario,
            //        Distancia = item.Distancia,
            //        CantCanjes = item.CantidadCanjes
            //    });

            //}

            return ListPerfiles;
        }
        

        [HttpGet]
        [Route("api/productos/GetProductosByIdTipo/{idTipo}")]
        public List<ItemVM> GetProductosByIdTipo(int idTipo)
        {
            List<Productos> listaProductos = db.Productos.Where(x=> x.IdTipo== idTipo).ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<ItemVM> listItemProducVM = new List<ItemVM>();
           
            foreach (var item in listaProductos)
            {
                listItemProducVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = string.IsNullOrEmpty(item.Descripcion) ? "Palermo. Buenos Aires": item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,                    
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen,
                    PorcentajeDesc = item.PorcentajeDescuento,
                    FechaVencimiento = item.FechaVencimiento
                });

            }
            

            return listItemProducVM;
        }
        [HttpGet]
        [Route("api/productos/GetDescuentosByIdCategoria/{idCategoria}")]
        public List<ItemVM> GetDescuentosByIdCategoria(int idCategoria)
        {
            List<Productos> listaProductos = db.Productos.Where(x => x.IdTipo == 2 && x.IdCategoria== idCategoria).ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<ItemVM> listItemProducVM = new List<ItemVM>();

            foreach (var item in listaProductos)
            {
                listItemProducVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = string.IsNullOrEmpty(item.Descripcion) ? "Palermo. Buenos Aires" : item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen,
                    PorcentajeDesc = item.PorcentajeDescuento,
                    FechaVencimiento = item.FechaVencimiento
                });

            }


            return listItemProducVM;
        }

        [HttpGet]
        [Route("api/productos/GetCanjesByIdCategoria/{idCategoria}")]
        public List<ItemVM> GetCanjesByIdCategoria(int idCategoria)
        {
            List<Productos> listaProductos = db.Productos.Where(x => x.IdTipo==1 && x.IdCategoria == idCategoria).ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<ItemVM> listItemProducVM = new List<ItemVM>();

            foreach (var item in listaProductos)
            {
                listItemProducVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = string.IsNullOrEmpty(item.Descripcion) ? "Palermo. Buenos Aires" : item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen,
                    PorcentajeDesc = item.PorcentajeDescuento,
                    FechaVencimiento = item.FechaVencimiento
                });

            }


            return listItemProducVM;
        }

        [HttpGet]
        [Route("api/productos/GetCanjesByIdUsuario/{idUsuario}")]
        public List<ItemVM> GetCanjesByIdUsuario(int idUsuario)
        {
            List<Productos> listaProductos = db.Productos.Where(x => x.IdTipo == 1 && x.IdUsuario == idUsuario).ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<ItemVM> listItemProducVM = new List<ItemVM>();

            foreach (var item in listaProductos)
            {
                listItemProducVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = string.IsNullOrEmpty(item.Descripcion) ? "Palermo. Buenos Aires" : item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen,
                    PorcentajeDesc = item.PorcentajeDescuento,
                    FechaVencimiento = item.FechaVencimiento
                });

            }


            return listItemProducVM;
        }

        [HttpGet]
        [Route("api/productos/GetCanjesBySexo/{Sexo}")]
        public List<ItemVM> GetCanjesBySexo(string Sexo)
        {
            List<Productos> listaProductos = db.Productos.Where(x => x.IdTipo == 1 && x.Usuarios.Sexo == Sexo).ToList();

            if (listaProductos == null)
            {
                return null;
            }

            List<ItemVM> listItemProducVM = new List<ItemVM>();

            foreach (var item in listaProductos)
            {
                listItemProducVM.Add(new ItemVM
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = string.IsNullOrEmpty(item.Descripcion) ? "Palermo. Buenos Aires" : item.Descripcion,
                    IdTipo = item.IdTipo,
                    Fecha_Publicacion = item.Fecha_Publicacion,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                    Imagen = item.Imagen,
                    Categoria = item.Categorias.Nombre,
                    IdCategoria = item.IdCategoria.ToString(),
                    Importe = item.Importe.ToString(),
                    lat = string.IsNullOrEmpty(item.lat) ? "0" : item.lat,
                    lng = string.IsNullOrEmpty(item.lng) ? "0" : item.lng,
                    Usuario = item.Usuarios.Nombre,
                    UsuarioImagen = item.Usuarios.Imagen,
                    PorcentajeDesc = item.PorcentajeDescuento,
                    FechaVencimiento = item.FechaVencimiento
                });

            }


            return listItemProducVM;
        }


        // PUT: api/Productos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProductos(int id, Productos productos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productos.Id)
            {
                return BadRequest();
            }

            db.Entry(productos).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Productos
        [ResponseType(typeof(Productos))]
        public IHttpActionResult PostProductos(Productos productos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Productos.Add(productos);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductosExists(productos.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = productos.Id }, productos);
        }

        // DELETE: api/Productos/5
        [ResponseType(typeof(Productos))]
        public IHttpActionResult DeleteProductos(int id)
        {
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return NotFound();
            }

            db.Productos.Remove(productos);
            db.SaveChanges();

            return Ok(productos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductosExists(int id)
        {
            return db.Productos.Count(e => e.Id == id) > 0;
        }
    }
}