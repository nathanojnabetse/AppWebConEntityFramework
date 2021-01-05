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

        public ActionResult Filtro(string nombre)
        {
            List<RolCLS> listarol = new List<RolCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if(nombre == null)
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
                                && rol.NOMBRE.Contains(nombre)
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

        public int Guardar(RolCLS oRolCLS, int titulo)
        {
            int respuesta = 0;// bumero de registros afectados
            using (var bd = new BDPasajeEntities())
            {
                if(titulo.Equals(1))
                {
                    Rol oRol = new Rol();
                    oRol.NOMBRE = oRolCLS.nombre;
                    oRol.DESCRIPCION = oRolCLS.descripcion;
                    oRol.BHABILITADO = 1;
                    bd.Rol.Add(oRol);
                    respuesta = bd.SaveChanges();
                }
            }
            return respuesta;
        }
    }
}