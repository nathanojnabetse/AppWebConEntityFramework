﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;


namespace AppWebTest.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Index()
        {
            List<EmpleadoCLS> listaEmpleados = null;
            using (var bd = new BDPasajeEntities())
            {
                listaEmpleados = (from empleado in bd.Empleado
                                  join tipousuario in bd.TipoUsuario
                                  on empleado.IIDTIPOUSUARIO equals tipousuario.IIDTIPOUSUARIO
                                  join tipoContrato in bd.TipoContrato
                                  on empleado.IIDTIPOCONTRATO equals tipoContrato.IIDTIPOCONTRATO
                                  where empleado.BHABILITADO == 1
                                  select new EmpleadoCLS
                                  {
                                      iidEmpleado = empleado.IIDEMPLEADO,
                                      nombre = empleado.NOMBRE,
                                      apPaterno = empleado.APPATERNO,
                                      nombreTipoUsuario = tipousuario.NOMBRE,
                                      nombreTipoContrato = tipoContrato.NOMBRE
                                  }).ToList();
            }
            return View(listaEmpleados);
        }

        public void listarComboSexo()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from sexo in bd.Sexo
                         where sexo.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = sexo.NOMBRE,
                             Value = sexo.IIDSEXO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaSexo = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarTipoContrato()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from tipoContrato in bd.TipoContrato
                         where tipoContrato.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = tipoContrato.NOMBRE,
                             Value = tipoContrato.IIDTIPOCONTRATO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoContrato = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarComboTipoUsuario()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from tipoUsuario in bd.TipoUsuario
                         where tipoUsuario.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = tipoUsuario.NOMBRE,
                             Value = tipoUsuario.IIDTIPOUSUARIO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoUsuario = lista; //ViewBag pasa info del controles a la vista
            }
        }
    
        public void listarCombos()
        {
            listarComboTipoUsuario();
            listarTipoContrato();
            listarComboSexo();
        }

        public ActionResult Agregar()
        {
            listarCombos();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(EmpleadoCLS oEmpleadoCLS)
        {
            if(!ModelState.IsValid)
            {
                listarCombos();
                return View(oEmpleadoCLS);
            }
            
                using (var bd = new BDPasajeEntities())
                {
                    Empleado oEmpleado = new Empleado();
                    oEmpleado.NOMBRE = oEmpleadoCLS.nombre;
                    oEmpleado.APPATERNO = oEmpleadoCLS.apPaterno;
                    oEmpleado.APMATERNO = oEmpleadoCLS.apMaterno;
                    oEmpleado.FECHACONTRATO = oEmpleadoCLS.fechaContrato;
                    oEmpleado.SUELDO = oEmpleadoCLS.sueldo;
                    oEmpleado.IIDTIPOUSUARIO = oEmpleadoCLS.idtipoUsuario;
                    oEmpleado.IIDTIPOCONTRATO = oEmpleadoCLS.idtipoContrato;
                    oEmpleado.IIDSEXO = oEmpleadoCLS.iidSexo;
                    oEmpleado.BHABILITADO = 1;

                    bd.Empleado.Add(oEmpleado);
                    bd.SaveChanges();
                }
                return RedirectToAction("Index");
            
        }
    }
}