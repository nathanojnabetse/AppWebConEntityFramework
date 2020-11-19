using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class SucursalController : Controller
    {
        // GET: Sucursal
        public ActionResult Index()
        {
            List<SucursalCLS> listaSucursal = null;
            using (var bd = new BDPasajeEntities())
            {
                listaSucursal = (from sucursal in bd.Sucursal
                                 where sucursal.BHABILITADO==1
                                select new SucursalCLS
                                {
                                    idsucursal = sucursal.IIDSUCURSAL,
                                    nombre = sucursal.NOMBRE,
                                    telefono = sucursal.TELEFONO,
                                    email = sucursal.EMAIL
                                }).ToList();
            }
            return View(listaSucursal);//PAsar la lista a la vista
        }


        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(SucursalCLS oSucursalCLS)
        {
            int nRegistrosEncontrados = 0;
            string nombreSucursal = oSucursalCLS.nombre;
            using (var bd = new BDPasajeEntities())
            {
                nRegistrosEncontrados = bd.Sucursal.Where(p => p.NOMBRE.Equals(nombreSucursal)).Count();
            }
            
            if (!ModelState.IsValid || nRegistrosEncontrados >= 1)
            {
                if(nRegistrosEncontrados >= 1)
                {
                    oSucursalCLS.mensajeError = "Ya existe la sucursal a ingresar";
                }
                return View(oSucursalCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Sucursal oSucursal = new Sucursal();
                oSucursal.NOMBRE = oSucursalCLS.nombre;
                oSucursal.DIRECCION = oSucursalCLS.direccion;
                oSucursal.TELEFONO = oSucursalCLS.telefono;
                oSucursal.EMAIL = oSucursalCLS.email;
                oSucursal.FECHAAPERTURA = oSucursalCLS.fechaApertura;
                oSucursal.BHABILITADO = 1;
                bd.Sucursal.Add(oSucursal);
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Editar(int id)
        {
            SucursalCLS oSucurcalCLS = new SucursalCLS();
                 
            using(var bd =new BDPasajeEntities())
            {
                Sucursal oSucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
                oSucurcalCLS.idsucursal = oSucursal.IIDSUCURSAL;
                oSucurcalCLS.nombre = oSucursal.NOMBRE;
                oSucurcalCLS.direccion = oSucursal.DIRECCION;
                oSucurcalCLS.telefono = oSucursal.TELEFONO;
                oSucurcalCLS.email = oSucursal.EMAIL;
                oSucurcalCLS.fechaApertura = (DateTime)oSucursal.FECHAAPERTURA;
            }

            return View(oSucurcalCLS);
        }
       
        [HttpPost]
        public ActionResult Editar(SucursalCLS oSucursalCLS)
        {
            int nregistrosAfecados = 0;
            int idSucursal = oSucursalCLS.idsucursal;
            string nombreSucursal = oSucursalCLS.nombre;

            using (var bd = new BDPasajeEntities())
            {
                nregistrosAfecados = bd.Sucursal.Where(p => p.NOMBRE.Equals(nombreSucursal) && !p.IIDSUCURSAL.Equals(idSucursal)).Count();
            }

            if (!ModelState.IsValid || nregistrosAfecados >= 1)
            {
                if(nregistrosAfecados >= 1)
                {
                    oSucursalCLS.mensajeError = "Ya existe la sucursal";
                }
                return View(oSucursalCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Sucursal oSucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(idSucursal)).First();

                oSucursal.NOMBRE = oSucursalCLS.nombre;
                oSucursal.DIRECCION = oSucursalCLS.direccion;
                oSucursal.TELEFONO = oSucursalCLS.telefono;
                oSucursal.EMAIL = oSucursalCLS.email;
                oSucursal.FECHAAPERTURA = oSucursalCLS.fechaApertura;

                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }
        
        public ActionResult Eliminar(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Sucursal oSucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
                oSucursal.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}