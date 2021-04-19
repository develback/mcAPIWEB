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
    public class PedidosController : ApiController
    {
        private MundoCanjeDBEntities db = new MundoCanjeDBEntities();

        // GET: api/Pedidos
        public IQueryable<Pedidos> GetPedidos()
        {
            return db.Pedidos;
        }

        // GET: api/Pedidos/5
        [ResponseType(typeof(Pedidos))]
        public PedidoViewModel GetPedidos(int id)
        {
            Pedidos pedidos = db.Pedidos.Find(id);
            Usuarios objUsuarioRecibe = new Usuarios();
            Productos objProductos = new Productos();
            if (pedidos == null)
            {
                return null;
            }
            if(pedidos.IdUsuarioRecibe!=null)
            {
                objUsuarioRecibe = db.Usuarios.Where(x => x.Id == pedidos.IdUsuarioRecibe).FirstOrDefault();
            }
            if(pedidos.IdProductoInteres != null)
            {
                objProductos = db.Productos.Where(x => x.Id == pedidos.IdProductoInteres.Value).FirstOrDefault();
            }

            PedidoViewModel listVM = new PedidoViewModel()
            {
                Id = pedidos.Id,
                IdPedido_Estado = pedidos.IdPedido_Estado.Value,
                Desc_Estado = pedidos.Pedidos_Estados.Nombre,
                FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                Importe = pedidos.Importe ?? 0,
                Comentarios = pedidos.Comentarios,
                Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,                
                lat = (pedidos.IdProductoInteres != null) ? (objProductos.lat != null) ? objProductos.lat : "0" : "0",
                lng = (pedidos.IdProductoInteres != null) ? (objProductos.lng != null) ? objProductos.lng : "0" : "0",
                IdProductoInteres = pedidos.IdProductoInteres ?? 0,
                NombreProductoInteres = (pedidos.IdProductoInteres != null) ? objProductos.Nombre : "",
                ImgProductoInteres = (pedidos.IdProductoInteres != null) ? objProductos.Imagen : "",
                DescProductoInteres = (pedidos.IdProductoInteres != null) ? objProductos.Descripcion : "",

                IdUsuarioInteres = (pedidos.IdProductoInteres != null) ? objProductos.Usuarios.Id : pedidos.IdUsuarioRecibe ?? 0,
                ImgUsuarioInteres = (pedidos.IdProductoInteres != null) ? objProductos.Usuarios.Imagen : objUsuarioRecibe.Imagen,
                Nombre_UsuarioInteres = (pedidos.IdProductoInteres != null) ? objProductos.Usuarios.Nombre : objUsuarioRecibe.Nombre,

                IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
                ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
                Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre,
                TipoMatch = pedidos.TipoMatch
            };

            return listVM;
        }

        [HttpGet]
        [Route("api/Pedidos/PedidosByState/{idEstado}")]
        public List<PedidoViewModel> PedidosByState(int idEstado)
        {
            List<Pedidos> listaPedidos = db.Pedidos.Where(x => x.IdPedido_Estado == idEstado).ToList();

            if (listaPedidos == null)
            {
                return null;
            }

            List<PedidoViewModel> listVM = new List<PedidoViewModel>();
            foreach (var pedidos in listaPedidos)
            {
                Productos objProductos = new Productos();
                Usuarios objUsuarioRecibe = new Usuarios();
                if (pedidos.IdProductoInteres != null)
                {
                    objProductos = db.Productos.Where(x => x.Id == pedidos.IdProductoInteres.Value).FirstOrDefault();
                }
                if (pedidos.IdUsuarioRecibe != null)
                {
                    objUsuarioRecibe = db.Usuarios.Where(x => x.Id == pedidos.IdUsuarioRecibe).FirstOrDefault();
                }

                listVM.Add(new PedidoViewModel
                {
                    Id = pedidos.Id,
                    IdPedido_Estado = pedidos.IdPedido_Estado.Value,
                    Desc_Estado = pedidos.Pedidos_Estados.Nombre,
                    FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                    Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                    Importe = pedidos.Importe ?? 0,
                    Comentarios = pedidos.Comentarios,
                    Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,
                    lat = (pedidos.IdProductoInteres != null) ? (objProductos.lat != null) ? objProductos.lat : "0" : "0",
                    lng = (pedidos.IdProductoInteres != null) ? (objProductos.lng != null) ? objProductos.lng : "0" : "0",
                    IdProductoInteres = pedidos.IdProductoInteres ?? 0,
                    NombreProductoInteres = (pedidos.IdProductoInteres != null) ? objProductos.Nombre : "",
                    ImgProductoInteres = (pedidos.IdProductoInteres != null) ? objProductos.Imagen : "",
                    DescProductoInteres = (pedidos.IdProductoInteres != null) ? objProductos.Descripcion : "",

                    IdUsuarioInteres = (pedidos.IdProductoInteres != null) ? objProductos.Usuarios.Id : pedidos.IdUsuarioRecibe ?? 0,
                    ImgUsuarioInteres = (pedidos.IdProductoInteres != null) ? objProductos.Usuarios.Imagen : objUsuarioRecibe.Imagen,
                    Nombre_UsuarioInteres = (pedidos.IdProductoInteres != null) ? objProductos.Usuarios.Nombre : objUsuarioRecibe.Nombre,

                    IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
                    ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
                    Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre,
                    TipoMatch = pedidos.TipoMatch
                });

            }

            return listVM;
        }

        [HttpGet]
        [Route("api/Pedidos/CanjesRecibidosByUsuario/{idUsuario}")]
        public List<PedidoViewModel> CanjesRecibidosByUsuario(int idUsuario)
        {
            var query = from ped in db.Pedidos.AsEnumerable()
                        join prod in db.Productos.AsEnumerable() on ped.IdProductoInteres equals prod.Id
                        where ped.IdPedido_Estado==3 && prod.IdUsuario == idUsuario
                        select new PedidoViewModel
                        {
                            Id = ped.Id,
                            IdPedido_Estado = ped.IdPedido_Estado.Value,
                            Desc_Estado = ped.Pedidos_Estados.Nombre,
                            FechaPedido = (ped.FechaPedido != null) ? ped.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                            Fecha_Entrega = (ped.FechaEntrega != null) ? ped.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                            Importe = ped.Importe ?? 0,
                            Comentarios = ped.Comentarios,
                            Ult_Dias = (int)DateTime.Now.Subtract(ped.FechaPedido.Value).TotalDays,
                            lat = (prod.lat != null) ? prod.lat : "0",
                            lng = (prod.lng != null) ? prod.lng : "0",
                            IdProductoInteres = ped.IdProductoInteres.Value,
                            NombreProductoInteres = prod.Nombre,
                            ImgProductoInteres = prod.Imagen,
                            DescProductoInteres = prod.Descripcion,
                            IdUsuarioInteres = prod.Usuarios.Id,
                            ImgUsuarioInteres = prod.Usuarios.Imagen,
                            Nombre_UsuarioInteres = prod.Usuarios.Nombre,
                            IdUsuarioSolicita = ped.IdUsuarioSolicita ??0,
                            ImgUsuarioSolicita = (ped.IdUsuarioSolicita!=null)? ped.Usuarios.Imagen : "",
                            Nombre_UsuarioSolicita = (ped.IdUsuarioSolicita != null) ? ped.Usuarios.Nombre : "",
                        };
            return query.ToList();


            

            //List<Pedidos> listaPedidos = db.Pedidos.Where(x => x.IdPedido_Estado == 3 && x.Productos.IdUsuario== idUsuario).ToList();
            //if (listaPedidos == null)
            //{
            //    return null;
            //}

            //List<PedidoViewModel> listVM = new List<PedidoViewModel>();
            //foreach (var pedidos in listaPedidos)
            //{
            //    Productos objProductos = new Productos();
                
            //    listVM.Add(new PedidoViewModel
            //    {
            //        Id = pedidos.Id,
            //        IdPedido_Estado = pedidos.IdPedido_Estado.Value,
            //        Desc_Estado = pedidos.Pedidos_Estados.Nombre,
            //        FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
            //        Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
            //        Importe = pedidos.Importe ?? 0,
            //        Comentarios = pedidos.Comentarios,
            //        Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,
            //        lat = (objProductos.lat != null) ? objProductos.lat : "0",
            //        lng = (objProductos.lng != null) ? objProductos.lng : "0",
            //        IdProductoInteres = pedidos.IdProductoInteres.Value,
            //        NombreProductoInteres = pedidos.Productos.Nombre,
            //        ImgProductoInteres = pedidos.Productos.Imagen,
            //        DescProductoInteres = pedidos.Productos.Descripcion,
            //        IdUsuarioInteres = pedidos.Productos.Usuarios.Id,
            //        ImgUsuarioInteres = pedidos.Productos.Usuarios.Imagen,
            //        Nombre_UsuarioInteres = pedidos.Productos.Usuarios.Nombre,
            //        IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
            //        ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
            //        Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre
            //    }); 

            //}

            //return listVM;
        }
        [HttpGet]
        [Route("api/Pedidos/CanjesConfirmadosByUsuario/{idUsuario}")]
        public List<PedidoViewModel> CanjesConfirmadosByUsuario(int idUsuario)
        {
            //List<Pedidos> listaPedidos = db.Pedidos.Where(x => x.IdPedido_Estado >= 4 && x.Productos.IdUsuario == idUsuario).ToList();

            //if (listaPedidos == null)
            //{
            //    return null;
            //}

            //List<PedidoViewModel> listVM = new List<PedidoViewModel>();
            //foreach (var pedidos in listaPedidos)
            //{
            //    bool vFueCalificado = db.Calificaciones.Any(x => x.UsuarioCalificador == pedidos.Productos.Usuarios.Id && x.UsuarioCalificado == pedidos.IdUsuarioSolicita.Value);
            //    listVM.Add(new PedidoViewModel
            //    {
            //        Id = pedidos.Id,
            //        IdPedido_Estado = pedidos.IdPedido_Estado.Value,
            //        Desc_Estado = pedidos.Pedidos_Estados.Nombre,
            //        FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
            //        Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
            //        Importe = pedidos.Importe ?? 0,
            //        Comentarios = pedidos.Comentarios,
            //        Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,
            //        lat = (pedidos.Productos.lat != null) ? pedidos.Productos.lat : "0",
            //        lng = (pedidos.Productos.lng != null) ? pedidos.Productos.lng : "0",
            //        IdProductoInteres = pedidos.IdProductoInteres.Value,
            //        NombreProductoInteres = pedidos.Productos.Nombre,
            //        ImgProductoInteres = pedidos.Productos.Imagen,
            //        DescProductoInteres = pedidos.Productos.Descripcion,
            //        IdUsuarioInteres = pedidos.Productos.Usuarios.Id,
            //        ImgUsuarioInteres = pedidos.Productos.Usuarios.Imagen,
            //        Nombre_UsuarioInteres = pedidos.Productos.Usuarios.Nombre,
            //        IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
            //        ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
            //        Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre,
            //        FueCalificado = vFueCalificado
            //    }); 

            //}

            //return listVM;
            /////////////////////////////////
            var query = from ped in db.Pedidos.AsEnumerable()
                        join prod in db.Productos.AsEnumerable() on ped.IdProductoInteres equals prod.Id
                        where ped.IdPedido_Estado >= 4 && prod.IdUsuario == idUsuario
                        select new PedidoViewModel
                        {
                            Id = ped.Id,
                            IdPedido_Estado = ped.IdPedido_Estado.Value,
                            Desc_Estado = ped.Pedidos_Estados.Nombre,
                            FechaPedido = (ped.FechaPedido != null) ? ped.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                            Fecha_Entrega = (ped.FechaEntrega != null) ? ped.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                            Importe = ped.Importe ?? 0,
                            Comentarios = ped.Comentarios,
                            Ult_Dias = (int)DateTime.Now.Subtract(ped.FechaPedido.Value).TotalDays,
                            lat = (prod.lat != null) ? prod.lat : "0",
                            lng = (prod.lng != null) ? prod.lng : "0",
                            IdProductoInteres = ped.IdProductoInteres.Value,
                            NombreProductoInteres = prod.Nombre,
                            ImgProductoInteres = prod.Imagen,
                            DescProductoInteres = prod.Descripcion,
                            IdUsuarioInteres = prod.Usuarios.Id,
                            ImgUsuarioInteres = prod.Usuarios.Imagen,
                            Nombre_UsuarioInteres = prod.Usuarios.Nombre,
                            IdUsuarioSolicita = ped.IdUsuarioSolicita.Value,
                            ImgUsuarioSolicita = ped.Usuarios.Imagen,
                            Nombre_UsuarioSolicita = ped.Usuarios.Nombre,
                            FueCalificado = db.Calificaciones.Any(x => x.UsuarioCalificador == prod.Usuarios.Id && x.UsuarioCalificado == ped.IdUsuarioSolicita.Value)
                        };
            return query.ToList();
            //var query = (from ped in db.Pedidos.AsEnumerable()
            //             join prod in db.Productos.AsEnumerable() on ped.IdProductoInteres equals prod.Id
            //            where ped.IdPedido_Estado >= 4 && prod.IdUsuario == idUsuario
            //            select new PedidoViewModel
            //            {
            //                Id = ped.Id,
            //                IdPedido_Estado = ped.IdPedido_Estado.Value,
            //                Desc_Estado = ped.Pedidos_Estados.Nombre,
            //                FechaPedido = (ped.FechaPedido != null) ? ped.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
            //                Fecha_Entrega = (ped.FechaEntrega != null) ? ped.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
            //                Importe = ped.Importe ?? 0,
            //                Comentarios = ped.Comentarios,
            //                Ult_Dias = (int)DateTime.Now.Subtract(ped.FechaPedido.Value).TotalDays,
            //                lat = (prod.lat != null) ? prod.lat : "0",
            //                lng = (prod.lng != null) ? prod.lng : "0",
            //                IdProductoInteres = ped.IdProductoInteres.Value,
            //                NombreProductoInteres = prod.Nombre,
            //                ImgProductoInteres = prod.Imagen,
            //                DescProductoInteres = prod.Descripcion,
            //                IdUsuarioInteres = prod.Usuarios.Id,
            //                ImgUsuarioInteres = prod.Usuarios.Imagen,
            //                Nombre_UsuarioInteres = prod.Usuarios.Nombre,
            //                IdUsuarioSolicita = ped.IdUsuarioSolicita.Value,
            //                ImgUsuarioSolicita = ped.Usuarios.Imagen,
            //                Nombre_UsuarioSolicita = ped.Usuarios.Nombre,
            //                //FueCalificado = db.Calificaciones.Any(x => x.UsuarioCalificador == prod.Usuarios.Id && x.UsuarioCalificado == ped.IdUsuarioSolicita.Value)
            //            }).ToList();
            //return query;
        }

        [HttpGet]
        [Route("api/Pedidos/CanjesByState/{idEstado}")]
        public List<PedidoViewModel> CanjesByState(int idEstado)
        {
            List<PedidoViewModel> listVM = new List<PedidoViewModel>();
            if (idEstado == 1)
            {
                List<Productos> lisProductos = db.Productos.Where(x => x.IdEstado == idEstado && x.IdTipo == 1).OrderByDescending(y => y.Id).ToList();
                if (lisProductos == null)
                {
                    return null;
                }

                foreach (var item in lisProductos)
                {
                    listVM.Add(new PedidoViewModel
                    {
                        Id = item.Id,
                        IdPedido_Estado = item.IdEstado ??0,
                        Desc_Estado = item.Productos_Estados.Nombre,
                        FechaPedido = (item.Fecha_Publicacion != null) ? item.Fecha_Publicacion.Value.ToString("dd/MM/yyyy") : "",
                        Fecha_Entrega = (item.FechaVencimiento != null) ? item.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "",
                        Importe = item.Importe??0,
                        Comentarios = "",
                        Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                        lat = (item.lat != null) ? item.lat : "0",
                        lng = (item.lng != null) ? item.lng : "0",
                        IdProductoInteres = item.Id,
                        NombreProductoInteres = item.Nombre,
                        ImgProductoInteres = item.Imagen,
                        DescProductoInteres = item.Descripcion,
                        IdUsuarioInteres = item.Usuarios.Id,
                        ImgUsuarioInteres = item.Usuarios.Imagen,
                        Nombre_UsuarioInteres = item.Usuarios.Nombre,
                        IdUsuarioSolicita = 0,
                        ImgUsuarioSolicita = "",
                        Nombre_UsuarioSolicita = ""
                    });

                }
            }
            else
            {
                //List<Pedidos> listaPedidos = db.Pedidos.Where(x => x.IdPedido_Estado == idEstado && x.Productos.IdTipo == 1).ToList();

                //if (listaPedidos == null)
                //{
                //    return null;
                //}


                //foreach (var pedidos in listaPedidos)
                //{
                //    listVM.Add(new PedidoViewModel
                //    {
                //        Id = pedidos.Id,
                //        IdPedido_Estado = pedidos.IdPedido_Estado.Value,
                //        Desc_Estado = pedidos.Pedidos_Estados.Nombre,
                //        FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                //        Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                //        Importe = pedidos.Importe ??0,
                //        Comentarios = pedidos.Comentarios,
                //        Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,
                //        lat = (pedidos.Productos.lat != null) ? pedidos.Productos.lat : "0",
                //        lng = (pedidos.Productos.lng != null) ? pedidos.Productos.lng : "0",
                //        IdProductoInteres = pedidos.IdProductoInteres.Value,
                //        NombreProductoInteres = pedidos.Productos.Nombre,
                //        ImgProductoInteres = pedidos.Productos.Imagen,
                //        DescProductoInteres = pedidos.Productos.Descripcion,
                //        IdUsuarioInteres = pedidos.Productos.Usuarios.Id,
                //        ImgUsuarioInteres = pedidos.Productos.Usuarios.Imagen,
                //        Nombre_UsuarioInteres = pedidos.Productos.Usuarios.Nombre,
                //        IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
                //        ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
                //        Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre
                //    });

                //}
                var query = from ped in db.Pedidos
                            join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                            where ped.IdPedido_Estado == idEstado && prod.IdTipo == 1
                            select new PedidoViewModel
                            {
                                Id = ped.Id,
                                IdPedido_Estado = ped.IdPedido_Estado.Value,
                                Desc_Estado = ped.Pedidos_Estados.Nombre,
                                FechaPedido = (ped.FechaPedido != null) ? ped.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                                Fecha_Entrega = (ped.FechaEntrega != null) ? ped.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                                Importe = ped.Importe ?? 0,
                                Comentarios = ped.Comentarios,
                                Ult_Dias = (int)DateTime.Now.Subtract(ped.FechaPedido.Value).TotalDays,
                                lat = (prod.lat != null) ? prod.lat : "0",
                                lng = (prod.lng != null) ? prod.lng : "0",
                                IdProductoInteres = ped.IdProductoInteres.Value,
                                NombreProductoInteres = prod.Nombre,
                                ImgProductoInteres = prod.Imagen,
                                DescProductoInteres = prod.Descripcion,
                                IdUsuarioInteres = prod.Usuarios.Id,
                                ImgUsuarioInteres = prod.Usuarios.Imagen,
                                Nombre_UsuarioInteres = prod.Usuarios.Nombre,
                                IdUsuarioSolicita = ped.IdUsuarioSolicita.Value,
                                ImgUsuarioSolicita = ped.Usuarios.Imagen,
                                Nombre_UsuarioSolicita = ped.Usuarios.Nombre
                            };
                listVM=  query.ToList();
            }


            return listVM;
        }

        [HttpGet]
        [Route("api/Pedidos/UltimosCanjes/{Cantidad}")]
        public List<PedidoViewModel> UltimosCanjes(int Cantidad)
        {
            //List<Pedidos> listaPedidos = db.Pedidos.Where(x => x.Productos.IdTipo == 1).OrderByDescending(z=> z.FechaPedido).Take(Cantidad).ToList();

            //if (listaPedidos == null)
            //{
            //    return null;
            //}

            //List<PedidoViewModel> listVM = new List<PedidoViewModel>();
            //foreach (var pedidos in listaPedidos)
            //{
            //    listVM.Add(new PedidoViewModel
            //    {
            //        Id = pedidos.Id,
            //        IdPedido_Estado = pedidos.IdPedido_Estado.Value,
            //        Desc_Estado = pedidos.Pedidos_Estados.Nombre,
            //        FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
            //        Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
            //        Importe = pedidos.Importe ?? 0,
            //        Comentarios = pedidos.Comentarios,
            //        Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,
            //        lat = (pedidos.Productos.lat != null) ? pedidos.Productos.lat : "0",
            //        lng = (pedidos.Productos.lng != null) ? pedidos.Productos.lng : "0",
            //        IdProductoInteres = pedidos.IdProductoInteres.Value,
            //        NombreProductoInteres = pedidos.Productos.Nombre,
            //        ImgProductoInteres = pedidos.Productos.Imagen,
            //        DescProductoInteres = pedidos.Productos.Descripcion,
            //        IdUsuarioInteres = pedidos.Productos.Usuarios.Id,
            //        ImgUsuarioInteres = pedidos.Productos.Usuarios.Imagen,
            //        Nombre_UsuarioInteres = pedidos.Productos.Usuarios.Imagen,
            //        IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
            //        ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
            //        Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre
            //    });

            //}

            //return listVM;

            //var query = from ped in db.Pedidos
            //            join prod in db.Productos on ped.IdProductoInteres equals prod.Id
            //            where prod.IdTipo == 1 && ped.TipoMatch == "CANJE"
            //            orderby ped.FechaPedido descending
            //            select new PedidoViewModel
            //            {
            //                Id = ped.Id,
            //                IdPedido_Estado = ped.IdPedido_Estado.Value,
            //                Desc_Estado = ped.Pedidos_Estados.Nombre,
            //                FechaPedido = (ped.FechaPedido != null) ? ped.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
            //                Fecha_Entrega = (ped.FechaEntrega != null) ? ped.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
            //                Importe = ped.Importe ?? 0,
            //                Comentarios = ped.Comentarios,
            //                Ult_Dias = (int)DateTime.Now.Subtract(ped.FechaPedido.Value).TotalDays,
            //                lat = (prod.lat != null) ? prod.lat : "0",
            //                lng = (prod.lng != null) ? prod.lng : "0",
            //                IdProductoInteres = ped.IdProductoInteres.Value,
            //                NombreProductoInteres = prod.Nombre,
            //                ImgProductoInteres = prod.Imagen,
            //                DescProductoInteres = prod.Descripcion,
            //                IdUsuarioInteres = prod.Usuarios.Id,
            //                ImgUsuarioInteres = prod.Usuarios.Imagen,
            //                Nombre_UsuarioInteres = prod.Usuarios.Nombre,
            //                IdUsuarioSolicita = ped.IdUsuarioSolicita.Value,
            //                ImgUsuarioSolicita = ped.Usuarios.Imagen,
            //                Nombre_UsuarioSolicita = ped.Usuarios.Nombre
            //            };
            //return query.Take(Cantidad).ToList();


            List<Pedidos> listaPedidos = db.Pedidos.Where(x=> x.TipoMatch == "CANJE").OrderByDescending(z => z.FechaPedido).Take(Cantidad).ToList();
            List<PedidoViewModel> listVM = new List<PedidoViewModel>();
            if (listaPedidos == null)
            {
                return null;
            }
            foreach (var item in listaPedidos)
            {
                var objProd = db.Productos.Where(x => x.Id == item.IdProductoInteres).FirstOrDefault();
                listVM.Add(new PedidoViewModel
                {
                    Id = item.Id,
                    IdPedido_Estado = item.IdPedido_Estado.Value,
                    Desc_Estado = item.Pedidos_Estados.Nombre,
                    FechaPedido = (item.FechaPedido != null) ? item.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                    Fecha_Entrega = (item.FechaEntrega != null) ? item.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                    Importe = item.Importe ?? 0,
                    Comentarios = item.Comentarios,
                    Ult_Dias = (int)DateTime.Now.Subtract(item.FechaPedido.Value).TotalDays,
                    lat = (objProd.lat != null) ? objProd.lat : "0",
                    lng = (objProd.lng != null) ? objProd.lng : "0",
                    IdProductoInteres = item.IdProductoInteres?? 0,
                    NombreProductoInteres = objProd.Nombre,
                    ImgProductoInteres = objProd.Imagen,
                    DescProductoInteres = objProd.Descripcion,
                    IdUsuarioInteres = objProd.Usuarios.Id,
                    ImgUsuarioInteres = objProd.Usuarios.Imagen,
                    Nombre_UsuarioInteres = objProd.Usuarios.Nombre,
                    IdUsuarioSolicita = item.IdUsuarioSolicita??0 ,
                    ImgUsuarioSolicita = objProd.Usuarios.Imagen,
                    Nombre_UsuarioSolicita = objProd.Usuarios.Nombre
                });
            }
            return listVM;

        }

        [HttpGet]
        [Route("api/Pedidos/DescuentosByState/{idEstado}")]
        public List<PedidoViewModel> DescuentosByState(int idEstado)
        {

            List<PedidoViewModel> listVM = new List<PedidoViewModel>();
            if (idEstado == 1)
            {
                var ListaProd = db.Productos.Where(x => x.IdEstado == idEstado && x.IdTipo == 2).ToList();
                foreach (var item in ListaProd)
                {
                    listVM.Add(new PedidoViewModel
                    {
                        Id = item.Id,
                        IdPedido_Estado = item.IdEstado??0,
                        Desc_Estado = item.Productos_Estados.Nombre,
                        FechaPedido = (item.Fecha_Publicacion != null) ? item.Fecha_Publicacion.Value.ToString("dd/MM/yyyy") : "",
                        Fecha_Entrega = (item.FechaVencimiento != null) ? item.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "",
                        Importe = item.Importe?? 0,
                        Comentarios = "",
                        Ult_Dias = (int)DateTime.Now.Subtract(item.Fecha_Publicacion.Value).TotalDays,
                        lat = (item.lat != null) ? item.lat : "0",
                        lng = (item.lng != null) ? item.lng : "0",
                        IdProductoInteres = item.Id,
                        NombreProductoInteres = item.Nombre,
                        ImgProductoInteres = item.Imagen,
                        DescProductoInteres = item.Descripcion,
                        IdUsuarioInteres = item.Usuarios.Id,
                        ImgUsuarioInteres = item.Usuarios.Imagen,
                        Nombre_Comercio = item.Usuarios.Nombre,
                        IdUsuarioSolicita = 0,
                        ImgUsuarioSolicita = "",
                        Nombre_UsuarioSolicita = "",
                        CodigoDescuento = item.CodigoDescuento
                    });
                }
            }
            else
            {
                //List<Pedidos> listaPedidos = new List<Pedidos>();
                //listaPedidos = db.Pedidos.Where(x => x.IdPedido_Estado == idEstado && x.Productos.IdTipo == 2).ToList();

                //foreach (var pedidos in listaPedidos)
                //{
                //    listVM.Add(new PedidoViewModel
                //    {
                //        Id = pedidos.Id,
                //        IdPedido_Estado = pedidos.IdPedido_Estado.Value,
                //        Desc_Estado = pedidos.Pedidos_Estados.Nombre,
                //        FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                //        Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                //        Importe = pedidos.Importe ?? 0,
                //        Comentarios = pedidos.Comentarios,
                //        Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,
                //        lat = (pedidos.Productos.lat != null) ? pedidos.Productos.lat : "0",
                //        lng = (pedidos.Productos.lng != null) ? pedidos.Productos.lng : "0",
                //        IdProductoInteres = pedidos.IdProductoInteres.Value,
                //        NombreProductoInteres = pedidos.Productos.Nombre,
                //        ImgProductoInteres = pedidos.Productos.Imagen,
                //        DescProductoInteres = pedidos.Productos.Descripcion,
                //        IdUsuarioInteres = pedidos.Productos.Usuarios.Id,
                //        ImgUsuarioInteres = pedidos.Productos.Usuarios.Imagen,
                //        Nombre_Comercio = pedidos.Productos.Usuarios.Nombre,
                //        IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
                //        ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
                //        Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre,
                //        CodigoDescuento = pedidos.Productos.CodigoDescuento
                //    });
                //}
                var query = from ped in db.Pedidos
                            join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                            where ped.IdPedido_Estado == idEstado && prod.IdTipo == 2
                            select new PedidoViewModel
                            {
                                Id = ped.Id,
                                IdPedido_Estado = ped.IdPedido_Estado.Value,
                                Desc_Estado = ped.Pedidos_Estados.Nombre,
                                FechaPedido = (ped.FechaPedido != null) ? ped.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                                Fecha_Entrega = (ped.FechaEntrega != null) ? ped.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                                Importe = ped.Importe ?? 0,
                                Comentarios = ped.Comentarios,
                                Ult_Dias = (int)DateTime.Now.Subtract(ped.FechaPedido.Value).TotalDays,
                                lat = (prod.lat != null) ? prod.lat : "0",
                                lng = (prod.lng != null) ? prod.lng : "0",
                                IdProductoInteres = ped.IdProductoInteres.Value,
                                NombreProductoInteres = prod.Nombre,
                                ImgProductoInteres = prod.Imagen,
                                DescProductoInteres = prod.Descripcion,
                                IdUsuarioInteres = prod.Usuarios.Id,
                                ImgUsuarioInteres = prod.Usuarios.Imagen,
                                Nombre_Comercio = prod.Usuarios.Nombre,
                                IdUsuarioSolicita = ped.IdUsuarioSolicita.Value,
                                ImgUsuarioSolicita = ped.Usuarios.Imagen,
                                Nombre_UsuarioSolicita = ped.Usuarios.Nombre,
                                CodigoDescuento = prod.CodigoDescuento
                            };
                listVM = query.ToList();


            }
            return listVM;
        }

        [HttpGet]
        [Route("api/Pedidos/UltDescuentosDescargados/{Cantidad}")]
        public List<PedidoViewModel> UltimosDescuentos(int Cantidad)
        {

            //List<PedidoViewModel> listVM = new List<PedidoViewModel>();            
            //List<Pedidos> listaPedidos = new List<Pedidos>();
            //listaPedidos = db.Pedidos.Where(x => x.Productos.IdTipo == 2).OrderByDescending(z => z.FechaPedido).Take(Cantidad).ToList();

            //foreach (var pedidos in listaPedidos)
            //{
            //    listVM.Add(new PedidoViewModel
            //    {
            //        Id = pedidos.Id,
            //        IdPedido_Estado = pedidos.IdPedido_Estado.Value,
            //        Desc_Estado = pedidos.Pedidos_Estados.Nombre,
            //        FechaPedido = (pedidos.FechaPedido != null) ? pedidos.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
            //        Fecha_Entrega = (pedidos.FechaEntrega != null) ? pedidos.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
            //        Importe = pedidos.Importe ?? 0,
            //        Comentarios = pedidos.Comentarios,
            //        Ult_Dias = (int)DateTime.Now.Subtract(pedidos.FechaPedido.Value).TotalDays,
            //        lat = (pedidos.Productos.lat != null) ? pedidos.Productos.lat : "0",
            //        lng = (pedidos.Productos.lng != null) ? pedidos.Productos.lng : "0",
            //        IdProductoInteres = pedidos.IdProductoInteres.Value,
            //        NombreProductoInteres = pedidos.Productos.Nombre,
            //        ImgProductoInteres = pedidos.Productos.Imagen,
            //        DescProductoInteres = pedidos.Productos.Descripcion,
            //        IdUsuarioInteres = pedidos.Productos.Usuarios.Id,
            //        ImgUsuarioInteres = pedidos.Productos.Usuarios.Imagen,
            //        Nombre_Comercio = pedidos.Productos.Usuarios.Nombre,
            //        IdUsuarioSolicita = pedidos.IdUsuarioSolicita.Value,
            //        ImgUsuarioSolicita = pedidos.Usuarios.Imagen,
            //        Nombre_UsuarioSolicita = pedidos.Usuarios.Nombre,
            //        CodigoDescuento = pedidos.Productos.CodigoDescuento
            //    });
            //}

            //return listVM;
            var query = from ped in db.Pedidos
                        join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                        where prod.IdTipo == 2
                        orderby ped.FechaPedido descending
                        select new PedidoViewModel
                        {
                            Id = ped.Id,
                            IdPedido_Estado = ped.IdPedido_Estado.Value,
                            Desc_Estado = ped.Pedidos_Estados.Nombre,
                            FechaPedido = (ped.FechaPedido != null) ? ped.FechaPedido.Value.ToString("dd/MM/yyyy") : "",
                            Fecha_Entrega = (ped.FechaEntrega != null) ? ped.FechaEntrega.Value.ToString("dd/MM/yyyy") : "",
                            Importe = ped.Importe ?? 0,
                            Comentarios = ped.Comentarios,
                            Ult_Dias = (int)DateTime.Now.Subtract(ped.FechaPedido.Value).TotalDays,
                            lat = (prod.lat != null) ? prod.lat : "0",
                            lng = (prod.lng != null) ? prod.lng : "0",
                            IdProductoInteres = ped.IdProductoInteres.Value,
                            NombreProductoInteres = prod.Nombre,
                            ImgProductoInteres = prod.Imagen,
                            DescProductoInteres = prod.Descripcion,
                            IdUsuarioInteres = prod.Usuarios.Id,
                            ImgUsuarioInteres = prod.Usuarios.Imagen,
                            Nombre_Comercio = prod.Usuarios.Nombre,
                            IdUsuarioSolicita = ped.IdUsuarioSolicita.Value,
                            ImgUsuarioSolicita = ped.Usuarios.Imagen,
                            Nombre_UsuarioSolicita = ped.Usuarios.Nombre,
                            CodigoDescuento = prod.CodigoDescuento
                        };
            return query.Take(Cantidad).ToList();
        }

        [HttpGet]
        [Route("api/Pedidos/PedidosCount/")]
        public ContadoresProductos PedidosCount()
        {
            try
            {
                var joinCanjesCancel = from ped in db.Pedidos join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                                       where prod.IdTipo == 1 && ped.IdPedido_Estado == 2 select ped.Id;
                
                var joinCanjesIni = from ped in db.Pedidos  join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                                    where prod.IdTipo == 1 && ped.IdPedido_Estado == 3  select ped.Id;

                var joinCanjesConf = from ped in db.Pedidos  join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                                    where prod.IdTipo == 1 && ped.IdPedido_Estado == 4  select ped.Id;


                var CantCanjesPendientes = (from u in db.Productos where u.IdTipo==1 && u.IdEstado == 1 select u.Id).Count();
                //var CantCanjesCancelados = (from u in db.Pedidos where u.Productos.IdTipo == 1 && u.IdPedido_Estado == 2 select u.Id).Count();
                //var CantCanjesIniciados = (from u in db.Pedidos where u.Productos.IdTipo == 1 && u.IdPedido_Estado == 3 select u.Id).Count();
                //var CantCanjesConfirmados = (from u in db.Pedidos where u.Productos.IdTipo == 1 && u.IdPedido_Estado == 4 select u.Id).Count();

                var CantCanjesCancelados = joinCanjesCancel.Count();
                var CantCanjesIniciados = joinCanjesIni.Count();
                var CantCanjesConfirmados = joinCanjesConf.Count();

                var joinDescCancel = from ped in db.Pedidos
                                       join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                                       where prod.IdTipo == 2 && ped.IdPedido_Estado == 2
                                       select ped.Id;

                var joinDescIni = from ped in db.Pedidos
                                    join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                                    where prod.IdTipo == 2 && ped.IdPedido_Estado == 3
                                    select ped.Id;

                var joinDescConf = from ped in db.Pedidos
                                     join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                                     where prod.IdTipo == 2 && ped.IdPedido_Estado == 4
                                     select ped.Id;


                var CantDescuentosPendientes = (from u in db.Productos where u.IdTipo == 2 && u.IdEstado == 1 select u.Id).Count();
                //var CantDescuentosCancelados = (from u in db.Pedidos where u.Productos.IdTipo == 2 && u.IdPedido_Estado == 2 select u.Id).Count();
                //var CantDescuentosIniciados = (from u in db.Pedidos where u.Productos.IdTipo == 2 && u.IdPedido_Estado == 3 select u.Id).Count();
                //var CantDescuentosConfirmados = (from u in db.Pedidos where u.Productos.IdTipo == 2 && u.IdPedido_Estado == 4 select u.Id).Count();
                var CantDescuentosCancelados = joinDescCancel.Count();
                var CantDescuentosIniciados = joinDescIni.Count();
                var CantDescuentosConfirmados = joinDescConf.Count();


                var CantUsuariosTotales = (from u in db.Usuarios where u.IdTipo == 1 select u.Id).Count();
                var CantTermYCond = (from u in db.Terminos select u.Id).Count();
                var CantCategorias = (from u in db.Categorias select u.Id).Count();
                var CantComerciosTotales = (from u in db.Usuarios where u.IdTipo == 2 select u.Id).Count();
                var CantPregFrec = (from u in db.Preguntas_Frecuentes select u.Id).Count();
                var CantLocalidades = (from u in db.Localidades select u.Id).Count();
                var CantPaises = (from u in db.Paises select u.Id).Count();
                
                var CantNotifEnviadas = (from u in db.Notificaciones where u.Tipo=="S" select u.Id).Count();


                ContadoresProductos Lista = new ContadoresProductos()
                {
                    CanjesPendientes = CantCanjesPendientes.ToString(),
                    CanjesCancelados = CantCanjesCancelados.ToString(),
                    CanjesIniciados = CantCanjesIniciados.ToString(),
                    CanjesConfirmados = CantCanjesConfirmados.ToString(),
                    DescuentosPendientes = CantDescuentosPendientes.ToString(),
                    DescuentosCancelados = CantDescuentosCancelados.ToString(),
                    DescuentosIniciados = CantDescuentosIniciados.ToString(),
                    DescuentosConfirmados = CantDescuentosConfirmados.ToString(),
                    CantidadUsuarios = CantUsuariosTotales.ToString(),
                    CantTerminosYCondiciones = CantTermYCond.ToString(),
                    CantidadCategorias = CantCategorias.ToString(),
                    CantidadComercios = CantComerciosTotales.ToString(),
                    CantidadFAQ = CantPregFrec.ToString(),
                    CantidadLocalidades = CantLocalidades.ToString(),
                    CantidadPaises = CantPaises.ToString(),
                    CantNotificacEnviadas = CantNotifEnviadas.ToString()
                    
                };

                return Lista;

            }
            catch
            {
                return null;
            }


        }


        [HttpGet]
        [Route("api/Pedidos/HomeWebCount/")]
        public ContadoresHomeWeb HomeWebCount()
        {
            try
            {
                var CantPlanesVendidos = (from u in db.Usuarios where u.IdPlan != null && u.IdTipo == 1 select u.Id).Count();

                var joinCanjesConf = from ped in db.Pedidos
                                       join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                                       where prod.IdTipo == 1 && ped.IdPedido_Estado == 4
                                       select ped.Id;

                //var CantCanjesConfirmados = (from u in db.Pedidos where u.Productos.IdTipo == 1 && u.IdPedido_Estado == 4 select u.Id).Count();
                var CantCanjesConfirmados = joinCanjesConf.Count();
                var CantUsuariosActivos = (from u in db.Usuarios where u.Estado == 1 select u.Id).Count();


                ContadoresHomeWeb Lista = new ContadoresHomeWeb()
                {
                    PlanVendidoCant = CantPlanesVendidos.ToString(),
                    CanjesRealizadosCant = CantCanjesConfirmados.ToString(),
                    UsuariosActivosCant = CantUsuariosActivos.ToString()
                };


                return Lista;

            }
            catch
            {
                return null;
            }


        }

        
        // PUT: api/Pedidos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPedidos(int id, Pedidos pedidos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedidos.Id)
            {
                return BadRequest();
            }
            
            db.Entry(pedidos).State = EntityState.Modified;
            db.Entry(pedidos).Property(x => x.FechaPedido).IsModified = false;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidosExists(id))
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

        // POST: api/Pedidos
        [ResponseType(typeof(Pedidos))]
        public IHttpActionResult PostPedidos(Pedidos pedidos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            pedidos.FechaPedido = DateTime.Now;
            db.Pedidos.Add(pedidos);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PedidosExists(pedidos.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pedidos.Id }, pedidos);
        }

        // DELETE: api/Pedidos/5
        [ResponseType(typeof(Pedidos))]
        public IHttpActionResult DeletePedidos(int id)
        {
            Pedidos pedidos = db.Pedidos.Find(id);
            if (pedidos == null)
            {
                return NotFound();
            }

            db.Pedidos.Remove(pedidos);
            db.SaveChanges();

            return Ok(pedidos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PedidosExists(int id)
        {
            return db.Pedidos.Count(e => e.Id == id) > 0;
        }
    }
}