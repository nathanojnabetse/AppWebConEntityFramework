using AppWebTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            List<ClienteCLS> listaCliente = null;

            using (var bd = new BDPasajeEntities())
            {
                listaCliente = (from cliente in bd.Cliente
                                select new ClienteCLS
                                {
                                    idCliente = cliente.IIDCLIENTE,
                                    nombre = cliente.NOMBRE,
                                    appaterneno=cliente.APPATERNO,
                                    apmaterno = cliente.APMATERNO,
                                    telefonoFijo = cliente.TELEFONOFIJO
                                }).ToList();
            }
                return View(listaCliente);
        }
    }
}