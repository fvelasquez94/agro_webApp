using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using agro_webApp.Models;

namespace agro_webApp.Controllers
{
    public class usuariosController : Controller
    {
        private agro_appEntities db = new agro_appEntities();

        // GET: usuarios
        public ActionResult Index()
        {
            var usuarios = db.usuarios.Include(u => u.tipo_Usuario);
            return View(usuarios.ToList());
        }

        // GET: usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: usuarios/Create
        public ActionResult Create()
        {
            ViewBag.id_Tipo = new SelectList(db.tipo_Usuario, "id_Tipo", "descripcion");
            return View();
        }

        // POST: usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Usuario,correo,contrasena,id_Tipo,activo")] usuarios usuarios, string Nombre_completo, string fecha_nacimiento, string pais, string profesion_oficio, string nombreFinca, string ubicacion, string extension, string descripcion)
        {
            //[Bind(Include = "id_Usuario,correo,contrasena,id_Tipo")] usuarios usuarios, [Bind(Include = "id_datos,id_Usuario,Nombre_completo,fecha_nacimiento,pais,profesion_oficio")] datos_Usuario datos_usuario 
            usuarios.activo = true;
            if (ModelState.IsValid)
            {
                //Primero guardamos el binding de usuario que viene desde la vista
                db.usuarios.Add(usuarios);
                db.SaveChanges();
                Session["id_usuario"] = usuarios.id_Usuario;
                //Luego guardamos los datos del usuario instanciando la clase datos_usuario 
                DateTime fecha = DateTime.Parse(fecha_nacimiento);
                var datos_usuario = new datos_Usuario();
                datos_usuario.id_Usuario = usuarios.id_Usuario;
                datos_usuario.Nombre_completo = Nombre_completo;
                datos_usuario.fecha_nacimiento = fecha;
                datos_usuario.pais = pais;
                datos_usuario.profesion_oficio = profesion_oficio;
                
                db.datos_Usuario.Add(datos_usuario);
                db.SaveChanges();
                Session["nombre_usuario"] = datos_usuario.Nombre_completo;
                //luego guardamos los datos de la finca
                var datos_finca = new finca();
                datos_finca.nombre = nombreFinca;
                datos_finca.ubicacion = ubicacion;
                datos_finca.extension = extension;
                datos_finca.descripcion = descripcion;
                datos_finca.id_Usuario = usuarios.id_Usuario;
                
                db.finca.Add(datos_finca);
                db.SaveChanges();
                Session["nombre_finca"] = datos_finca.nombre;
                //y por ultimo relacionamos la finca con el usuario
                return RedirectToAction("Index","Home");
            }

            ViewBag.id_Tipo = new SelectList(db.tipo_Usuario, "id_Tipo", "descripcion", usuarios.id_Tipo);
            return View(usuarios);
        }

        // GET: usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Tipo = new SelectList(db.tipo_Usuario, "id_Tipo", "descripcion", usuarios.id_Tipo);
            return View(usuarios);
        }

        // POST: usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Usuario,correo,contrasena,id_Tipo,activo")] usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_Tipo = new SelectList(db.tipo_Usuario, "id_Tipo", "descripcion", usuarios.id_Tipo);
            return View(usuarios);
        }

        // GET: usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuarios usuarios = db.usuarios.Find(id);
            db.usuarios.Remove(usuarios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
