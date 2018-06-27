using agro_webApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace agro_webApp.Controllers
{
    public class HomeController : Controller
    {
        private agro_appEntities db = new agro_appEntities();
        public ActionResult Index()
        {
            if (Session["id_usuario"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.modulo = "Administracion";
                ViewBag.seccion = "Inicio";
                return View();
            }

        }
        public ActionResult Home()
        {

            return View();
        }
        public ActionResult cerrarSesion()
        {
            Session["id_usuario"] = null;
            Session["nombre_usuario"] = null;
            Session["nombre_finca"] = null;
            Session["ID_finca"] = null;
            Session.RemoveAll();
            return RedirectToAction("Home");
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult iniciarSesion(string correo, string contrasena)
        {

            if (correo != "" && contrasena != "")
            {

                var usuario = db.usuarios.Where(a => a.correo.Equals(correo) && a.contrasena.Equals(contrasena) && a.activo == true).FirstOrDefault();
                if (usuario != null)
                {
                    Session["id_usuario"] = usuario.id_Usuario.ToString();

                    var nombre_usuario = db.datos_Usuario.Where(a => a.id_Usuario.Equals(usuario.id_Usuario)).FirstOrDefault();
                    Session["nombre_usuario"] = nombre_usuario.Nombre_completo.ToString();

                    var nombre_finca = db.finca.Where(a => a.id_Usuario.Equals(usuario.id_Usuario)).FirstOrDefault();
                    Session["nombre_finca"] = nombre_finca.nombre;
                    Session["ID_finca"] = nombre_finca.id_Finca;

                    return RedirectToAction("Index");
                }
                //aqui mostramos alerta de que los datos son erroneos
                return RedirectToAction("Login");
            }//aqui mostramos alerta de que no escribio nada
            return RedirectToAction("Login");

        }
    }
}