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
    
    public class CalificacionesController : ApiController
    {
        private MundoCanjeDBEntities db = new MundoCanjeDBEntities();

        public List<Models.CalificacionesViewModel> GetCalificaciones()
        {
            List<Calificaciones> listaCalif = db.Calificaciones.ToList();

            if (listaCalif == null)
            {
                return null;
            }

            List<CalificacionesViewModel> listVM = new List<CalificacionesViewModel>();
            foreach (var item in listaCalif)
            {
                var UsCalificador = db.Usuarios.Where(x => x.Id == item.UsuarioCalificador).FirstOrDefault();
                var UsCalificado = db.Usuarios.Where(x => x.Id == item.UsuarioCalificado).FirstOrDefault();

                listVM.Add(new CalificacionesViewModel
                {
                    Id = item.Id,
                    UsuarioCalificador = item.UsuarioCalificador,
                    UsuarioCalificado = item.UsuarioCalificado,
                    Puntuacion = item.Puntuacion,
                    Comentario = item.Comentario,
                    FechaAlta = (item.FechaAlta!=null) ? item.FechaAlta.Value: DateTime.MinValue,
                    ImgUsuarioCalificador = UsCalificador.Imagen,
                    ImgUsuarioCalificado = UsCalificado.Imagen,
                    Nombre_UsuarioCalificador = UsCalificador.Nombre,
                    Nombre_UsuarioCalificado = UsCalificado.Nombre
                });

            }

            return listVM;
        }

        // GET: api/Calificaciones/5
        [ResponseType(typeof(Calificaciones))]
        public IHttpActionResult GetCalificaciones(int id)
        {
            Calificaciones calif = db.Calificaciones.Find(id);
            if (calif == null)
            {
                return NotFound();
            }

            return Ok(calif);
        }

        [HttpGet]
        [Route("api/Calificaciones/ByUsuarioCalificado/{idUsuario}")]
        public List<Models.CalificacionesViewModel> GetByUsuarioCalificado(int idUsuario)
        {
            List<Calificaciones> listaCalif = db.Calificaciones.Where(x=>x.UsuarioCalificado== idUsuario).ToList();

            if (listaCalif == null)
            {
                return null;
            }

            List<CalificacionesViewModel> listVM = new List<CalificacionesViewModel>();
            foreach (var item in listaCalif)
            {
                var UsCalificador = db.Usuarios.Where(x => x.Id == item.UsuarioCalificador).FirstOrDefault();
                var UsCalificado = db.Usuarios.Where(x => x.Id == item.UsuarioCalificado).FirstOrDefault();

                listVM.Add(new CalificacionesViewModel
                {
                    Id = item.Id,
                    UsuarioCalificador = item.UsuarioCalificador,
                    UsuarioCalificado = item.UsuarioCalificado,
                    Puntuacion = item.Puntuacion,
                    Comentario = item.Comentario,
                    FechaAlta = (item.FechaAlta != null) ? item.FechaAlta.Value : DateTime.MinValue,
                    ImgUsuarioCalificador = UsCalificador.Imagen,
                    ImgUsuarioCalificado = UsCalificado.Imagen,
                    Nombre_UsuarioCalificador = UsCalificador.Nombre,
                    Nombre_UsuarioCalificado = UsCalificado.Nombre
                });

            }

            return listVM;
        }
        [HttpGet]
        [Route("api/Calificaciones/MiPromedioCalificado/{idUsuario}")]
        public double MiPromedioCalificado(int idUsuario)
        {
            double MiPuntaje = 0;
            var dbMiCalificacion = db.Calificaciones.Where(x => x.UsuarioCalificado == idUsuario).ToList();
            if (dbMiCalificacion.Count > 0)
            {
                MiPuntaje = dbMiCalificacion.Average(i => i.Puntuacion);
            }
            else
            {
                return 0;
            }
            
            return Math.Round(MiPuntaje, 2); ;
        }


        // PUT: api/Localidades/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCalificaciones(int id, Calificaciones calificaciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calificaciones.Id)
            {
                return BadRequest();
            }

            db.Entry(calificaciones).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalificacionesExists(id))
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

        // POST: api/Calificaciones
        [ResponseType(typeof(Calificaciones))]
        public IHttpActionResult PostCalificaciones(Calificaciones calificaciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            calificaciones.FechaAlta = DateTime.Now;
            db.Calificaciones.Add(calificaciones);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = calificaciones.Id }, calificaciones);
        }

        // DELETE: api/Calificaciones/5
        [ResponseType(typeof(Calificaciones))]
        public IHttpActionResult DeleteCalificaciones(int id)
        {
            Calificaciones calificaciones = db.Calificaciones.Find(id);
            if (calificaciones == null)
            {
                return NotFound();
            }

            db.Calificaciones.Remove(calificaciones);
            db.SaveChanges();

            return Ok(calificaciones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalificacionesExists(int id)
        {
            return db.Calificaciones.Count(e => e.Id == id) > 0;
        }
    }
}