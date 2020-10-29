using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class BusController : Controller
    {
        // GET: Bus
        public ActionResult Index()
        {
            List<BusCLS> listaBus = null;
            using (var bd = new BDPasajeEntities())
            {
                listaBus = (from bus in bd.Bus
                            join sucursal in bd.Sucursal
                            on bus.IIDSUCURSAL equals sucursal.IIDSUCURSAL
                            join TipoBus in bd.TipoBus
                            on bus.IIDTIPOBUS equals TipoBus.IIDTIPOBUS
                            join tipoModelo in bd.Modelo
                            on bus.IIDMODELO equals tipoModelo.IIDMODELO
                            where bus.BHABILITADO == 1
                            select new BusCLS
                            {
                                iidBus = bus.IIDBUS,
                                placa = bus.PLACA,
                                nombreModelo = tipoModelo.NOMBRE,
                                nombreSucursal = sucursal.NOMBRE,
                                nombreTipoBus = TipoBus.NOMBRE
                            }).ToList();
            }

            return View(listaBus);
      
        }

        public ActionResult Agregar()
        {
            listarCombos();

            return View();
        }

        [HttpPost]
        public ActionResult Agregar(BusCLS oBusCLS)
        {
            if(!ModelState.IsValid)
            {
                return View(oBusCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                Bus oBus = new Bus();
                oBus.BHABILITADO = 1;
                oBus.IIDSUCURSAL = oBusCLS.iidSucursal;
                oBus.IIDTIPOBUS = oBusCLS.iidTipoBus;
                oBus.PLACA = oBusCLS.placa;
                oBus.FECHACOMPRA = oBusCLS.fechaCompra;
                oBus.IIDMODELO = oBusCLS.iidModelo;
                oBus.NUMEROFILAS = oBusCLS.numeroFilas;
                oBus.NUMEROCOLUMNAS = oBusCLS.numeroColumnas;
                oBus.OBSERVACION = oBusCLS.observacion;
                oBus.DESCRIPCION = oBusCLS.descripcion;
                oBus.IIDMARCA = oBusCLS.iidmarca;

                bd.Bus.Add(oBus);
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }


        //nombre modelo nombre marca . nombre tipo bus
        public void listarTipoBus()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.TipoBus
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDTIPOBUS.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoBus = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarMarca()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.Marca
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDMARCA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaMarca = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarModelo()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.Modelo
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDMODELO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaModelo = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarSucursal()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.Sucursal
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDSUCURSAL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaSucursal = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarCombos()
        {
            listarMarca();
            listarModelo();
            listarTipoBus();
            listarSucursal();
        }

        public ActionResult Editar(int id)
        {
            listarCombos();
            BusCLS oBusCLS = new BusCLS();
            using (var bd = new BDPasajeEntities())
            {
                Bus oBus = bd.Bus.Where(p => p.IIDBUS.Equals(id)).First();
                oBusCLS.iidBus = oBus.IIDBUS;
                oBusCLS.iidSucursal = (int)oBus.IIDSUCURSAL;
                oBusCLS.iidTipoBus = (int)oBus.IIDTIPOBUS;
                oBusCLS.placa = oBus.PLACA;
                oBusCLS.fechaCompra = (DateTime)oBus.FECHACOMPRA;
                oBusCLS.iidModelo = (int)oBus.IIDMODELO;
                oBusCLS.numeroFilas = (int)oBus.NUMEROFILAS;
                oBusCLS.numeroColumnas = (int)oBus.NUMEROCOLUMNAS;
                oBusCLS.descripcion = oBus.DESCRIPCION;
                oBusCLS.observacion = oBus.OBSERVACION;
                oBusCLS.iidmarca = (int)oBus.IIDMARCA;
            }

                return View(oBusCLS);
        }
    }
}