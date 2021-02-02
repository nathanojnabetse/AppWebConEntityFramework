using AppWebTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AppWebTest.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }



        public string Login(UsuarioCLS oUsuario)
        {
            string mensaje="";
            //ERROR
            if(!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values//valores
                             from error in state.Errors//mensajes
                             select error.ErrorMessage).ToList();
                mensaje += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    mensaje += "<li class='list-group-item'>" + item + "</li>";
                }
                mensaje += "</ul>";
            }
            else
            {
                string nombreUsuario = oUsuario.nombreusuario;
                string password = oUsuario.contra;
                //Cifrar y comparar con lo de la bdd
                SHA256Managed sha = new SHA256Managed();
                byte[] byteContra = Encoding.Default.GetBytes(password);
                byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                using (var bd = new BDPasajeEntities())
                {
                    int numeroVeces = bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario && p.CONTRA == cadenaContraCifrada).Count();
                    mensaje = numeroVeces.ToString();
                    if (mensaje == "0")
                    {
                        mensaje = "Usuario o conraseña incorrecta";
                    }
                    else
                    {
                        Usuario ousuario = bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario && p.CONTRA == cadenaContraCifrada).First();
                        //todo el objeto usuario
                        Session["Usuario"] = ousuario;

                        if(ousuario.TIPOUSUARIO == "C")
                        {
                            Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE == ousuario.IID).First();
                            Session["nombreCompleto"]=oCliente.NOMBRE+" "+oCliente.APPATERNO + " " + oCliente.APMATERNO; 
                        }
                        else
                        {
                            Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO == ousuario.IID).First();
                            Session["nombreCompleto"] = oEmpleado.NOMBRE + " " + oEmpleado.APPATERNO + " " + oEmpleado.APMATERNO;
                        }

                        List<MenuCLS> listaMenu = (from usuario in bd.Usuario
                                                   join rol in bd.Rol
                                                   on usuario.IIDROL equals rol.IIDROL
                                                   join rolpagina in bd.RolPagina
                                                   on rol.IIDROL equals rolpagina.IIDROL
                                                   join pagina in bd.Pagina
                                                   on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                                   where rol.IIDROL == ousuario.IIDROL && rolpagina.IIDROL == ousuario.IIDROL && usuario.IIDUSUARIO==ousuario.IIDUSUARIO//&& rolpagina.BHABILITADO ==1
                                                   select new MenuCLS
                                                   {
                                                       nombreAccion = pagina.ACCION,
                                                       nombreControlador = pagina.CONTROLADOR,
                                                       mensaje = pagina.MENSAJE
                                                   }).ToList();


                                         Session["Rol"] = listaMenu;
                    }
                }
            }            
            return mensaje;
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index");
        }
    }
}