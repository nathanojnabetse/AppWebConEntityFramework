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
                                    teléfono = sucursal.TELEFONO,
                                    email = sucursal.EMAIL
                                }).ToList();
            }
            return View(listaSucursal);//PAsar la lista a la vista
        }
    }
}