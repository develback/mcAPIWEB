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
    public class Usuarios_CategoriasController : ApiController
    {
        private MundoCanjeDBEntities db = new MundoCanjeDBEntities();

        // GET: api/Usuarios_Categorias
        public IQueryable<Usuarios_Categorias> GetUsuarios_Categorias()
        {
            return db.Usuarios_Categorias;
        }

        // GET: api/Usuarios_Categorias/5
        [ResponseType(typeof(Usuarios_Categorias))]
        public IHttpActionResult GetUsuarios_Categorias(int id)
        {
            Usuarios_Categorias usuarios_Categorias = db.Usuarios_Categorias.Find(id);
            if (usuarios_Categorias == null)
            {
                return NotFound();
            }

            return Ok(usuarios_Categorias);
        }

        [HttpGet]
        [Route("api/usuarios_categorias/GetCategoriasByUsuario/{IdUsuario}")]
        public List<UsuarioCategoriasViewModel> GetCategoriasByUsuario(int IdUsuario)
        {
            var listUsuariosCat = db.Usuarios_Categorias.Where(x => x.IdUsuario == IdUsuario).ToList();
            var listCateg = db.Categorias.ToList();
            if (listUsuariosCat == null)
            {
                return null;
            }
            

            List<UsuarioCategoriasViewModel> listVM = new List<UsuarioCategoriasViewModel>();
            //foreach (var item in listUsuariosCat)
            //{

            //    listVM.Add(new UsuarioCategoriasViewModel
            //    {
            //        Id = item.Id,
            //        IdUsuario = item.IdUsuario,
            //        NombreUsuario = item.Usuarios.Nombre,
            //        IdCategoria = item.IdCategoria,
            //        NombreCategoria = item.Categorias.Nombre
            //    });

            //}
            foreach (var itemCateg in listCateg)
            {
                int idVM = 0, idUsuarioVM=0;
                string nombreUsuVM = "";
                bool checkVM = false;
                var LUsuario = listUsuariosCat.Where(x => x.IdCategoria == itemCateg.Id).ToList();
                if (LUsuario.Count >0)
                {
                    idVM = listUsuariosCat[0].Id;
                    idUsuarioVM = listUsuariosCat[0].IdUsuario;
                    nombreUsuVM = listUsuariosCat[0].Usuarios.Nombre;
                    checkVM = true;
                }

                listVM.Add(new UsuarioCategoriasViewModel
                {
                    Id = idVM,
                    IdUsuario = idUsuarioVM,
                    NombreUsuario = nombreUsuVM,
                    IdCategoria = itemCateg.Id,
                    NombreCategoria = itemCateg.Nombre,
                    Checked = checkVM
                });

            }


            return listVM;
        }

        // PUT: api/Usuarios_Categorias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuarios_Categorias(int id, Usuarios_Categorias usuarios_Categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarios_Categorias.Id)
            {
                return BadRequest();
            }

            db.Entry(usuarios_Categorias).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Usuarios_CategoriasExists(id))
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

        // POST: api/Usuarios_Categorias
        [ResponseType(typeof(Usuarios_Categorias))]
        public IHttpActionResult PostUsuarios_Categorias(Usuarios_Categorias usuarios_Categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuarios_Categorias.Add(usuarios_Categorias);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuarios_Categorias.Id }, usuarios_Categorias);
        }

        // DELETE: api/Usuarios_Categorias/5
        [ResponseType(typeof(Usuarios_Categorias))]
        public IHttpActionResult DeleteUsuarios_Categorias(int id)
        {
            //Usuarios_Categorias usuarios_Categorias = db.Usuarios_Categorias.Find(id);
            //if (usuarios_Categorias == null)
            //{
            //    return NotFound();
            //}

            //db.Usuarios_Categorias.Remove(usuarios_Categorias);
            //db.SaveChanges();

            //return Ok(usuarios_Categorias);
            var ListUsuCateg = db.Usuarios_Categorias.Where(x => x.IdUsuario == id).ToList();
            foreach (var item in ListUsuCateg)
            {
                db.Usuarios_Categorias.Remove(item);
                db.SaveChanges();
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Usuarios_CategoriasExists(int id)
        {
            return db.Usuarios_Categorias.Count(e => e.Id == id) > 0;
        }
    }
}