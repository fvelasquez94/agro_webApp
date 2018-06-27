using agro_webApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace agro_webApp.Controllers
{
    public class ConfiguracionController : Controller
    {
        // SECCION : LOTES
        private agro_appEntities db = new agro_appEntities();

        // GET: lotes
        public ActionResult Lotes()
        {
            if (Session["id_usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int IDfinca = Convert.ToInt32(Session["ID_finca"]);

                ViewBag.modulo = "Administracion";
                ViewBag.seccion = "Lotes";
                var lotes = db.lotes.Where(x => x.id_Finca == IDfinca).Include(l => l.finca).ToList();

                if (lotes.Count == 0) {
                    TempData["info"] = "Al parecer no tienes creado ningun lote.";
                }

                return View(lotes);
            }



        }

        // GET: lotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lotes lotes = db.lotes.Find(id);
            if (lotes == null)
            {
                return HttpNotFound();
            }
            return View(lotes);
        }

        // GET: lotes/Create
        public ActionResult Create()
        {
            ViewBag.id_Finca = new SelectList(db.finca, "id_Finca", "nombre");
            return View();
        }

        // POST: lotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Lote,codigo,nombre,extension,id_Finca,prioridad,descripcion")] lotes lotes)
        {
            if (ModelState.IsValid)
            {
                db.lotes.Add(lotes);
                db.SaveChanges();
                return RedirectToAction("Lotes");
            }

            ViewBag.id_Finca = new SelectList(db.finca, "id_Finca", "nombre", lotes.id_Finca);
            return View(lotes);
        }

        // GET: lotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lotes lotes = db.lotes.Find(id);
            if (lotes == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Finca = new SelectList(db.finca, "id_Finca", "nombre", lotes.id_Finca);
            return View(lotes);
        }

        // POST: lotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Lote,codigo,nombre,extension,id_Finca,prioridad,descripcion")] lotes lotes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lotes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Lotes");
            }
            ViewBag.id_Finca = new SelectList(db.finca, "id_Finca", "nombre", lotes.id_Finca);
            return View(lotes);
        }

        // GET: lotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lotes lotes = db.lotes.Find(id);
            if (lotes == null)
            {
                return HttpNotFound();
            }
            return View(lotes);
        }

        // POST: lotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lotes lotes = db.lotes.Find(id);
            db.lotes.Remove(lotes);
            db.SaveChanges();
            return RedirectToAction("Lotes");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // FIN SECCION : LOTES

        // SECCION : DATOS DE FINCA
        public ActionResult DatosFinca()
        {
            if (Session["id_usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int IDfinca = Convert.ToInt32(Session["ID_finca"]);

                ViewBag.modulo = "Administracion";
                ViewBag.seccion = "Datos de la finca";

                finca datosfinca = new finca();

                datosfinca = db.finca.Where(x => x.id_Finca == IDfinca).FirstOrDefault();
                return View(datosfinca);
            }



        }

        // FIN SECCION : DATOS DE FINCA

    }
}