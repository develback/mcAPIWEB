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
    public class NotificacionesController : ApiController
    {
        private MundoCanjeDBEntities db = new MundoCanjeDBEntities();

        // GET: api/Notificaciones
        public IQueryable<Notificaciones> GetNotificaciones()
        {
            return db.Notificaciones;
        }

        // GET: api/Notificaciones/5
        [ResponseType(typeof(Notificaciones))]
        public IHttpActionResult GetNotificaciones(int id)
        {
            Notificaciones notificaciones = db.Notificaciones.Find(id);
            if (notificaciones == null)
            {
                return NotFound();
            }

            return Ok(notificaciones);
        }
        [HttpGet]
        [Route("api/Notificaciones/CantNotificacionesByUser/{idUsuario}")]
        public int CantNotificacionesByUser(int idUsuario)
        {
            //int NuevasNotif = 0; //Reemplazar por una tabla Notificacion que contenga tipos. Y cuando se produce un match, inserta ahi. Cuando se califica se updatea, y ahi va restando.
            //List<int> ListEstados = new List<int>() { 3, 4 };
            //List<Pedidos> listaPedidos = db.Pedidos.Where(x => ListEstados.Contains(x.IdPedido_Estado.Value) && x.Productos.IdUsuario == idUsuario).ToList();

            //if (listaPedidos == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    NuevasNotif = listaPedidos.Count;
            //}

            //return NuevasNotif;
            var joinNotif = from ped in db.Pedidos
                               join prod in db.Productos on ped.IdProductoInteres equals prod.Id
                               where (ped.IdPedido_Estado == 3 || ped.IdPedido_Estado == 4) && prod.IdUsuario == idUsuario
                            select ped.Id;

            return joinNotif.Count();
        }

        [HttpGet]
        [Route("api/Notificaciones/NotificacionesByUsuario/{idUsuario}")]
        public List<UsuarioNotificacionesVM> NotificacionesByUsuario(int idUsuario)
        {
            //List<int> ListEstados = new List<int>() { 3, 4 };
            //List<Pedidos> listaPedidos = db.Pedidos.Where(x => ListEstados.Contains(x.IdPedido_Estado.Value) && (x.Productos.IdUsuario == idUsuario || x.IdUsuarioRecibe== idUsuario)).ToList();




            //if (listaPedidos == null)
            //{
            //    return null;
            //}

            //List<UsuarioNotificacionesVM> listVM = new List<UsuarioNotificacionesVM>();
            //foreach (var pedidos in listaPedidos)
            //{
            //    double PromPuntaje = 0;
            //    string TipoNot = (pedidos.IdPedido_Estado == 3) ? "SOLICITUD" : "CALIFICACION";
            //    string ObsNot = (pedidos.IdPedido_Estado == 3) ? "Nueva Solicitud: "+pedidos.Comentarios : "Por favor califique al usuario "+ pedidos.Usuarios.Nombre;
            //    var dbCalificaciones = db.Calificaciones.Where(x => x.UsuarioCalificado == pedidos.IdUsuarioSolicita).ToList();
            //    if(dbCalificaciones.Count>0)
            //        PromPuntaje = dbCalificaciones.Average(i => i.Puntuacion);

            //    if (PromPuntaje.ToString().Contains(".") || PromPuntaje.ToString().Contains(","))
            //        PromPuntaje = Math.Truncate(PromPuntaje);

            //    listVM.Add(new UsuarioNotificacionesVM
            //    {
            //        IdNotificacion = 1,
            //        IdPedido = pedidos.Id,
            //        Tipo = TipoNot,
            //        FechaNotificacion = pedidos.FechaPedido ?? DateTime.Now,
            //        IdUsuario = pedidos.IdUsuarioSolicita ??0,
            //        NombreUsuario = pedidos.Usuarios.Nombre,
            //        ImgUsuario = pedidos.Usuarios.Imagen,
            //        PuntajeUsuario =  (int)PromPuntaje,
            //        Observacion = ObsNot
            //    });
            //}
            //return listVM;

            var query = from ped in db.Pedidos
                        join prod in db.Productos on ped.IdProductoInteres equals prod.Id into gj
                        from x in gj.DefaultIfEmpty()
                        where (ped.IdPedido_Estado==3 || ped.IdPedido_Estado == 4) && (x.IdUsuario == idUsuario || ped.IdUsuarioRecibe == idUsuario)
                        select new UsuarioNotificacionesVM
                        {
                            IdNotificacion = 1,
                            IdPedido = ped.Id,
                            Tipo = (ped.IdPedido_Estado == 3) ? "SOLICITUD" : "CALIFICACION",
                            FechaNotificacion = ped.FechaPedido ?? DateTime.Now,
                            IdUsuario = ped.IdUsuarioSolicita ?? 0,
                            NombreUsuario = ped.Usuarios.Nombre,
                            ImgUsuario = ped.Usuarios.Imagen,
                            //PuntajeUsuario =(ped.IdUsuarioSolicita!=null) ? (int)db.Calificaciones.Where(x => x.UsuarioCalificado == ped.IdUsuarioSolicita).ToList().Average(i => i.Puntuacion): 0,
                            PuntajeUsuario =(db.Calificaciones.Where(x => x.UsuarioCalificado == ped.IdUsuarioSolicita).ToList().Count>0) ? (int)db.Calificaciones.Where(x => x.UsuarioCalificado == ped.IdUsuarioSolicita).ToList().Average(i => i.Puntuacion): 0,
                            Observacion = (ped.IdPedido_Estado == 3) ? "Nueva Solicitud: " + ped.Comentarios : "Por favor califique al usuario " + ped.Usuarios.Nombre
                        };
            return query.ToList();
        }

        // PUT: api/Notificaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotificaciones(int id, Notificaciones notificaciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notificaciones.Id)
            {
                return BadRequest();
            }

            db.Entry(notificaciones).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificacionesExists(id))
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

        // POST: api/Notificaciones
        [ResponseType(typeof(Notificaciones))]
        public IHttpActionResult PostNotificaciones(Notificaciones notificaciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notificaciones.Add(notificaciones);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (NotificacionesExists(notificaciones.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = notificaciones.Id }, notificaciones);
        }

        // DELETE: api/Notificaciones/5
        [ResponseType(typeof(Notificaciones))]
        public IHttpActionResult DeleteNotificaciones(int id)
        {
            Notificaciones notificaciones = db.Notificaciones.Find(id);
            if (notificaciones == null)
            {
                return NotFound();
            }

            db.Notificaciones.Remove(notificaciones);
            db.SaveChanges();

            return Ok(notificaciones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificacionesExists(int id)
        {
            return db.Notificaciones.Count(e => e.Id == id) > 0;
        }
    }
}