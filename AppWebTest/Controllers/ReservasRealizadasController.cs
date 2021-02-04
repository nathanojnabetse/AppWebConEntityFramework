using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class ReservasRealizadasController : Controller
    {
        // GET: ReservasRealizadas
        public ActionResult Index()
        {
            //obtener el id del usuario logeado
            Usuario oUsuario = (Usuario)Session["Usuario"];
            int iidUdsuario = oUsuario.IIDUSUARIO;
            List<ReservasRealizadaCLS> listaReserva = new List<ReservasRealizadaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaReserva = (from venta in bd.VENTA
                                where venta.BHABILITADO == 1
                                && venta.IIDUSUARIO == iidUdsuario
                                select new ReservasRealizadaCLS
                                {
                                    iidventa = venta.IIDVENTA,
                                    fechaVenta = (DateTime)venta.FECHAVENTA,
                                    total =(decimal)venta.TOTAL
                                }).ToList();

            }

                return View(listaReserva);
        }
    }
}