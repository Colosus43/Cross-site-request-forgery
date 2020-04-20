using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSRF.Controllers
{
    public class CSRFController : Controller
    {
        // GET: CSRF
        public ActionResult Login()
        {
            return View();
        }

        //POST: Login
        //Este action result verificaremos al usuario y si tiene permiso le dejamos acceder
        //y si es asi crearemos una SESSION para el.
        [HttpPost]
        public ActionResult Login(String usuario, String pass)
        {
            usuario = usuario.ToLower();
            //comprobaremos si el nombre de usuario y la contraseña son correctas 
            if (usuario == "cliente" && pass == "cliente")
            {
                //si son correctas creamos la session del usuario
                Session["CLIENTE"] = usuario;
                return RedirectToAction("Productos");

            }
            else
            {
                //si el usuario no existe le mostramos un mensaje y la session no se crea
                ViewBag.Mensaje = "Usuario - Contraseña Incorrectos";
                return View();
            }
        }

        //GET: Productos
        public ActionResult Productos()
        {
            //comprobamos si el usuario a iniciado session
            if (Session["CLIENTE"] == null)
            {
                //si no ha iniciado le mandamos a la pagina del Login
                return RedirectToAction("Login");
            }
            else
            {
                //si esta validado le dejamos entrar en la pagina de productos
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Productos(String[] producto)
        {
            //almacenamos los productos elegidos por el usuario en una variable temporal
            TempData["PRODUCTOS"] = producto;

            //mandamos al usuario a la pagina donde se vera el resultado de su compra
            return RedirectToAction("Compra");
        }

        public ActionResult Compra()
        {
            
            String[] productos = TempData["PRODUCTOS"] as String[];
            return View(productos);
        }
    }
}