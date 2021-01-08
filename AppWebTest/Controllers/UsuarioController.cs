using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace AppWebTest.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            listaPersonas();
            listarRol();
            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                List<UsuarioCLS> listaUsuarioCiente = (from usuario in bd.Usuario
                                                       join cliente in bd.Cliente
                                                       on usuario.IID equals
                                                       cliente.IIDCLIENTE
                                                       join rol in bd.Rol
                                                       on usuario.IIDROL equals rol.IIDROL
                                                       where usuario.bhabilitado == 1
                                                       && usuario.TIPOUSUARIO == "C"
                                                       select new UsuarioCLS
                                                       {
                                                           iidusuario = usuario.IIDUSUARIO,
                                                           nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                           nombreusuario = usuario.NOMBREUSUARIO,
                                                           nombreRol = rol.NOMBRE,
                                                           nombreTipoEmpleado = "Cliente"
                                                       }).ToList();

                List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                       join empleado in bd.Empleado
                                                       on usuario.IID equals
                                                       empleado.IIDEMPLEADO
                                                       join rol in bd.Rol
                                                       on usuario.IIDROL equals rol.IIDROL
                                                       where usuario.bhabilitado == 1
                                                       && usuario.TIPOUSUARIO == "E"
                                                       select new UsuarioCLS
                                                       {
                                                           iidusuario = usuario.IIDUSUARIO,
                                                           nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                           nombreusuario = usuario.NOMBREUSUARIO,
                                                           nombreRol = rol.NOMBRE,
                                                           nombreTipoEmpleado = "Empleado"
                                                       }).ToList();
                
                listaUsuario.AddRange(listaUsuarioCiente);
                listaUsuario.AddRange(listaUsuarioEmpleado);
                listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();

            }

                return View(listaUsuario);
        }

        public void listaPersonas()
        {
            List<SelectListItem> listaPersonas = new List<SelectListItem>();
            using (var bd = new BDPasajeEntities())
            {
                List<SelectListItem> listaCliente = (from item in bd.Cliente
                                                     where item.BHABILITADO == 1
                                                     && item.bTieneUsuario == 1
                                                     select new SelectListItem
                                                     {
                                                         Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO+" (C)",
                                                         Value = item.IIDCLIENTE.ToString()
                                                     }).ToList();

                List<SelectListItem> listaEmpleados = (from item in bd.Empleado
                                                     where item.BHABILITADO == 1
                                                     && item.bTieneUsuario == 1
                                                     select new SelectListItem
                                                     {
                                                         Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO + " (E)",
                                                         Value = item.IIDEMPLEADO.ToString()
                                                     }).ToList();

                listaPersonas.AddRange(listaCliente);
                listaPersonas.AddRange(listaEmpleados);

                listaPersonas = listaPersonas.OrderBy(p => p.Text).ToList();
                listaPersonas.Insert(0, new SelectListItem { Text = "--Seleecione--", Value = "" });
                ViewBag.listaPersona = listaPersonas;



            }
        }

        public void listarRol()
        {
            List<SelectListItem> listaRol;
            using (var bd = new BDPasajeEntities())
            {
                listaRol = (from item in bd.Rol
                            where item.BHABILITADO == 1
                            select new SelectListItem
                            {
                                Text = item.NOMBRE ,
                                Value = item.IIDROL.ToString()
                            }).ToList();

                ViewBag.listaRol = listaRol;
            }
        }

        public int Guardar(UsuarioCLS oUsuraioCLS, int titulo)
        {
            int rpta = 0;
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (titulo == 1)
                        {
                            Usuario oUsuario = new Usuario();
                            oUsuario.NOMBREUSUARIO = oUsuraioCLS.nombreusuario;
                            // cifrado
                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(oUsuraioCLS.contra);
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                            oUsuario.CONTRA = cadenaContraCifrada;
                            oUsuario.TIPOUSUARIO = oUsuraioCLS.nombrePersona.Substring(oUsuraioCLS.nombrePersona.Length - 2, 1);
                            oUsuario.IID = oUsuraioCLS.iid;
                            oUsuario.IIDROL = oUsuraioCLS.iidrol;
                            oUsuario.bhabilitado = 1;
                            bd.Usuario.Add(oUsuario);
                            if(oUsuario.TIPOUSUARIO.Equals("C"))
                            {
                                Cliente ocliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(oUsuraioCLS.iid)).First();
                                ocliente.bTieneUsuario = 1;

                            }
                            else
                            {
                                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(oUsuraioCLS.iid)).First();
                                oEmpleado.bTieneUsuario = 1;
                            }
                            rpta = bd.SaveChanges();
                            transaccion.Complete();
                        }
                    }

                        
                }
            }
            catch(Exception ex)
            {
                rpta = 0;
            }            

            return rpta;
        }

        public ActionResult Filtrar(UsuarioCLS oUsuarioCLS)
        {
            string nombrepersona = oUsuarioCLS.nombrePersona;
            listaPersonas();
            listarRol();
            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if(oUsuarioCLS.nombrePersona==null)
                {
                    List<UsuarioCLS> listaUsuarioCiente = (from usuario in bd.Usuario
                                                           join cliente in bd.Cliente
                                                           on usuario.IID equals
                                                           cliente.IIDCLIENTE
                                                           join rol in bd.Rol
                                                           on usuario.IIDROL equals rol.IIDROL
                                                           where usuario.bhabilitado == 1
                                                           && usuario.TIPOUSUARIO == "C"
                                                           select new UsuarioCLS
                                                           {
                                                               iidusuario = usuario.IIDUSUARIO,
                                                               nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                               nombreusuario = usuario.NOMBREUSUARIO,
                                                               nombreRol = rol.NOMBRE,
                                                               nombreTipoEmpleado = "Cliente"
                                                           }).ToList();

                    List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                             join empleado in bd.Empleado
                                                             on usuario.IID equals
                                                             empleado.IIDEMPLEADO
                                                             join rol in bd.Rol
                                                             on usuario.IIDROL equals rol.IIDROL
                                                             where usuario.bhabilitado == 1
                                                             && usuario.TIPOUSUARIO == "E"
                                                             select new UsuarioCLS
                                                             {
                                                                 iidusuario = usuario.IIDUSUARIO,
                                                                 nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                                 nombreusuario = usuario.NOMBREUSUARIO,
                                                                 nombreRol = rol.NOMBRE,
                                                                 nombreTipoEmpleado = "Empleado"
                                                             }).ToList();

                    listaUsuario.AddRange(listaUsuarioCiente);
                    listaUsuario.AddRange(listaUsuarioEmpleado);
                    listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();

                }
                else
                {
                    List<UsuarioCLS> listaUsuarioCiente = (from usuario in bd.Usuario
                                                           join cliente in bd.Cliente
                                                           on usuario.IID equals
                                                           cliente.IIDCLIENTE
                                                           join rol in bd.Rol
                                                           on usuario.IIDROL equals rol.IIDROL
                                                           where usuario.bhabilitado == 1
                                                           &&(cliente.NOMBRE.Contains(nombrepersona)
                                                           || cliente.APPATERNO.Contains(nombrepersona)
                                                           || cliente.APPATERNO.Contains(nombrepersona))
                                                           && usuario.TIPOUSUARIO == "C"
                                                           select new UsuarioCLS
                                                           {
                                                               iidusuario = usuario.IIDUSUARIO,
                                                               nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                               nombreusuario = usuario.NOMBREUSUARIO,
                                                               nombreRol = rol.NOMBRE,
                                                               nombreTipoEmpleado = "Cliente"
                                                           }).ToList();

                    List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                             join empleado in bd.Empleado
                                                             on usuario.IID equals
                                                             empleado.IIDEMPLEADO
                                                             join rol in bd.Rol
                                                             on usuario.IIDROL equals rol.IIDROL
                                                             where usuario.bhabilitado == 1
                                                             && usuario.TIPOUSUARIO == "E"
                                                             && (empleado.NOMBRE.Contains(nombrepersona)
                                                             || empleado.APPATERNO.Contains(nombrepersona)
                                                             || empleado.APPATERNO.Contains(nombrepersona))
                                                             select new UsuarioCLS
                                                             {
                                                                 iidusuario = usuario.IIDUSUARIO,
                                                                 nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                                 nombreusuario = usuario.NOMBREUSUARIO,
                                                                 nombreRol = rol.NOMBRE,
                                                                 nombreTipoEmpleado = "Empleado"
                                                             }).ToList();

                    listaUsuario.AddRange(listaUsuarioCiente);
                    listaUsuario.AddRange(listaUsuarioEmpleado);
                    listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
                }

            }

            return PartialView("_TablaUsuario",listaUsuario);
        }
    }      
       
}