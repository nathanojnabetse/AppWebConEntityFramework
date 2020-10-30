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
            if (!ModelState.IsValid)
            {
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
            int idSucursal = oSucursalCLS.idsucursal;

            if(!ModelState.IsValid)
            {
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
        
    }
}