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
        public ActionResult Index(EmpleadoCLS oEmpleadoCLS)
        {
            int idTipoUsuario = oEmpleadoCLS.idtipoUsuario;
            List<EmpleadoCLS> listaEmpleados = null;
            listarComboTipoUsuario();

            using (var bd = new BDPasajeEntities())
            {
                if(idTipoUsuario == 0)
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
                else
                {
                    listaEmpleados = (from empleado in bd.Empleado
                                      join tipousuario in bd.TipoUsuario
                                      on empleado.IIDTIPOUSUARIO equals tipousuario.IIDTIPOUSUARIO
                                      join tipoContrato in bd.TipoContrato
                                      on empleado.IIDTIPOCONTRATO equals tipoContrato.IIDTIPOCONTRATO
                                      where empleado.BHABILITADO == 1
                                      &&empleado.IIDTIPOUSUARIO == idTipoUsuario
                                      select new EmpleadoCLS
                                      {
                                          iidEmpleado = empleado.IIDEMPLEADO,
                                          nombre = empleado.NOMBRE,
                                          apPaterno = empleado.APPATERNO,
                                          nombreTipoUsuario = tipousuario.NOMBRE,
                                          nombreTipoContrato = tipoContrato.NOMBRE
                                      }).ToList();
                }
                
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
            int nregistrosAfectados = 0;
            string nombre = oEmpleadoCLS.nombre;
            string apPaterno = oEmpleadoCLS.apPaterno;
            string apMaterno = oEmpleadoCLS.apMaterno;

            using (var bd = new BDPasajeEntities())
            {
                nregistrosAfectados = bd.Empleado.Where(
                    p => p.NOMBRE.Equals(nombre) &&
                    p.APPATERNO.Equals(apPaterno) &&
                    p.APMATERNO.Equals(apMaterno)).Count();
            }
                if (!ModelState.IsValid || nregistrosAfectados >=1)
                {
                    if(nregistrosAfectados >=1)
                    {
                    oEmpleadoCLS.mensajeError = "El empleado ya existe";
                    }
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

        public ActionResult Editar(int id)
        {
            listarCombos();
            EmpleadoCLS oEmpleadoCLS = new EmpleadoCLS();

            using (var bd = new BDPasajeEntities())
            {
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(id)).First();
                oEmpleadoCLS.iidEmpleado = oEmpleado.IIDEMPLEADO;
                oEmpleadoCLS.nombre = oEmpleado.NOMBRE;
                oEmpleadoCLS.apPaterno = oEmpleado.APPATERNO;
                oEmpleadoCLS.apMaterno = oEmpleado.APMATERNO;
                oEmpleadoCLS.fechaContrato = (DateTime)oEmpleado.FECHACONTRATO;
                oEmpleadoCLS.sueldo = (decimal)oEmpleado.SUELDO;
                oEmpleadoCLS.idtipoUsuario = (int)oEmpleado.IIDTIPOUSUARIO;
                oEmpleadoCLS.idtipoContrato = (int)oEmpleado.IIDTIPOCONTRATO;
                oEmpleadoCLS.iidSexo = (int)oEmpleado.IIDSEXO;
            }

                return View(oEmpleadoCLS);
        }
        [HttpPost]
        public ActionResult Editar(EmpleadoCLS oEmpleadoCLS)
        {
            int nregistrosAfectados = 0;
            int idEmpleado = oEmpleadoCLS.iidEmpleado;
            string nombre = oEmpleadoCLS.nombre;
            string apPaterno = oEmpleadoCLS.apPaterno;
            string apmaterno = oEmpleadoCLS.apMaterno;

            using (var bd = new BDPasajeEntities())
            {
                nregistrosAfectados = bd.Empleado.Where(
                    p => p.NOMBRE.Equals(nombre) &&
                    p.APPATERNO.Equals(apPaterno) &&
                    p.APMATERNO.Equals(apmaterno) &&
                    !p.IIDEMPLEADO.Equals(idEmpleado)
                    ).Count();
            }

                if (!ModelState.IsValid || nregistrosAfectados>=1)
                {
                    if(nregistrosAfectados >=1 )
                    {
                        oEmpleadoCLS.mensajeError = "ya existe el empleado";
                    }

                listarCombos();
                    return View();

                }

            using (var bd = new BDPasajeEntities())
            {
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(idEmpleado)).First();
                oEmpleado.NOMBRE = oEmpleadoCLS.nombre;
                oEmpleado.APPATERNO = oEmpleadoCLS.apPaterno;
                oEmpleado.APMATERNO = oEmpleadoCLS.apMaterno;
                oEmpleado.FECHACONTRATO = oEmpleadoCLS.fechaContrato;
                oEmpleado.SUELDO = oEmpleadoCLS.sueldo;
                oEmpleado.IIDTIPOCONTRATO = oEmpleadoCLS.idtipoContrato;
                oEmpleado.IIDTIPOUSUARIO = oEmpleadoCLS.idtipoUsuario;
                oEmpleado.IIDSEXO = oEmpleadoCLS.iidSexo;

                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Eliminar(int txtIdEmpleado)
        {
            using (var bd = new BDPasajeEntities())
            {
                Empleado emp = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(txtIdEmpleado)).First();
                emp.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}