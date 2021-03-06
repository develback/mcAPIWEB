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
    public class CategoriasController : ApiController
    {
        private MundoCanjeDBEntities db = new MundoCanjeDBEntities();

        // GET: api/Categorias
        //public IQueryable<Categorias> GetCategorias()
        //{
        //    return db.Categorias;
        //}
        public List<Models.CategoriaViewModel> GetCategorias()
        {
            List<Categorias> listaCateg = db.Categorias.ToList();

            if (listaCateg == null)
            {
                return null;
            }

            List<CategoriaViewModel> listVM = new List<CategoriaViewModel>();
            foreach (var item in listaCateg)
            {
                int CantProd = item.Productos.Count();
                listVM.Add(new CategoriaViewModel
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Imagen = item.Imagen,
                    Logo = item.Logo,
                    CantProductos = CantProd
                });

            }

            return listVM;
        }
        [HttpGet]
        [Route("api/Categorias/GetCategoriasHome")]
        public List<Models.CategoriaViewModel> GetCategoriasHome()
        {
            List<Categorias> listaCateg = db.Categorias.Take(12).ToList();

            if (listaCateg == null)
            {
                return null;
            }

            List<CategoriaViewModel> listVM = new List<CategoriaViewModel>();
            foreach (var item in listaCateg)
            {
                int CantProd = item.Productos.Count();
                listVM.Add(new CategoriaViewModel
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Imagen = item.Imagen,
                    Logo = item.Logo,
                    CantProductos = CantProd
                });

            }

            return listVM;
        }

        [HttpGet]
        [Route("api/Categorias/GetTotalesById/{IdCateg}")]
        public TotalesCategoriaViewModel GetTotalesById(int IdCateg)
        {
            //var listaCanjes = db.Productos.Where(x => x.IdTipo == 1 && x.IdCategoria == IdCateg).ToList();
            var listaProd = db.Productos.Where(x => x.IdCategoria == IdCateg).ToList();
            var listaCanjes = listaProd.Where(x => x.IdTipo == 1).ToList();
            var listaDescuentos = listaProd.Where(x => x.IdTipo == 2).ToList();
            int cantUsuarios = listaCanjes.Select(x => x.IdUsuario).Distinct().Count();
            int cantProductos = listaCanjes.Count();
            int cantHombres = listaCanjes.Where(x => x.Usuarios.Sexo == "Hombre").Count();
            int cantMujeres = listaCanjes.Where(x => x.Usuarios.Sexo == "Mujer").Count();
            int cantDescuentos = listaDescuentos.Count();

            TotalesCategoriaViewModel totVM = new TotalesCategoriaViewModel()
            {
                Id = IdCateg,
                Nombre = (listaCanjes.Count>0) ?listaCanjes[0].Categorias.Nombre : "",
                CantUsuarios = cantUsuarios,
                CantProductos = cantProductos,
                CantHombres = cantHombres,
                CantMujeres = cantMujeres,
                CantNinios = 0,
                CantDescuentos = cantDescuentos
            };


            return totVM;
        }


        // GET: api/Categorias/5
        [ResponseType(typeof(Categorias))]
        public IHttpActionResult GetCategorias(int id)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        
        // PUT: api/Categorias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategorias(int id, Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorias.Id)
            {
                return BadRequest();
            }

            db.Entry(categorias).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriasExists(id))
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

        // POST: api/Categorias
        [ResponseType(typeof(Categorias))]
        public IHttpActionResult PostCategorias(Categorias categorias)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categorias.Add(categorias);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CategoriasExists(categorias.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = categorias.Id }, categorias);
        }

        // DELETE: api/Categorias/5
        [ResponseType(typeof(Categorias))]
        public IHttpActionResult DeleteCategorias(int id)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return NotFound();
            }

            db.Categorias.Remove(categorias);
            db.SaveChanges();

            return Ok(categorias);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriasExists(int id)
        {
            return db.Categorias.Count(e => e.Id == id) > 0;
        }
    }
}