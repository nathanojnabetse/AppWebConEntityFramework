using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult Index()
        {
            List<RolCLS> listarol = new List<RolCLS>();

            using (var bd =new BDPasajeEntities())
            {
                listarol = (from rol in bd.Rol
                            where rol.BHABILITADO == 1
                            select new RolCLS
                            {
                                iidRol = rol.IIDROL,
                                nombre = rol.NOMBRE,
                                descripcion = rol.DESCRIPCION
                            }).ToList();
            }

                return View(listarol);
        }

        public ActionResult Filtro(string nombrerol)
        {
            List<RolCLS> listarol = new List<RolCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if(nombrerol == null)
                {
                    listarol = (from rol in bd.Rol
                                where rol.BHABILITADO == 1
                                select new RolCLS
                                {
                                    iidRol = rol.IIDROL,
                                    nombre = rol.NOMBRE,
                                    descripcion = rol.DESCRIPCION
                                }).ToList();
                }
                else
                {
                    listarol = (from rol in bd.Rol
                                where rol.BHABILITADO == 1
                                && rol.NOMBRE.Contains(nombrerol)
                                select new RolCLS
                                {
                                    iidRol = rol.IIDROL,
                                    nombre = rol.NOMBRE,
                                    descripcion = rol.DESCRIPCION
                                }).ToList();
                }
                return PartialView("_TablaRol", listarol);
            }
        }

        public string Guardar(RolCLS oRolCLS, int titulo)
        {
            string respuesta = "";// bumero de registros afectados
            try
            {                
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values//valores
                                 from error in state.Errors//mensajes
                                 select error.ErrorMessage).ToList();
                    respuesta += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        respuesta += "<li class='list-group-item'>" + item + "</li>";
                    }
                    respuesta += "</ul>";
                }
                else
                {//devuleve 1 es correcto
                    using (var bd = new BDPasajeEntities())
                    {
                        if (titulo.Equals(-1))//guardar
                        {
                            Rol oRol = new Rol();
                            oRol.NOMBRE = oRolCLS.nombre;
                            oRol.DESCRIPCION = oRolCLS.descripcion;
                            oRol.BHABILITADO = 1;
                            bd.Rol.Add(oRol);
                            respuesta = bd.SaveChanges().ToString();
                            if (respuesta == "0")//no se agrego nada
                            {
                                respuesta = "";
                            }
                        }
                        else//editar
                        {
                            //obtener todo el registro por id
                            Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First();
                            oRol.NOMBRE = oRolCLS.nombre;
                            oRol.DESCRIPCION = oRolCLS.descripcion;
                            respuesta = bd.SaveChanges().ToString();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                respuesta = "";
            }
            
            
            return respuesta;
        }

        public JsonResult recuperarDatos(int titulo)
        {
            RolCLS oRolCLS = new RolCLS();
            using (var bd  = new BDPasajeEntities())
            {
                Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First();
                oRolCLS.nombre = oRol.NOMBRE;
                oRolCLS.descripcion = oRol.DESCRIPCION;
            }
            return Json(oRolCLS,JsonRequestBehavior.AllowGet);//objetoserializado, no se puedde enviar asi no mas un objeto a la vista
        }
    }
}