using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class RolPaginaController : Controller
    {
        // GET: RolPagina
        public ActionResult Index()
        {
            listarComboRol();
            listarComboPagina();
            List<RolPaginaCLS> listaRol = new List<RolPaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaRol = (from rolpagina in bd.RolPagina
                            join rol in bd.Rol
                            on rolpagina.IIDROL equals rol.IIDROL
                            join pagina in bd.Pagina
                            on rolpagina.IIDPAGINA equals
                            pagina.IIDPAGINA
                            where rolpagina.BHABILITADO==1
                            select new RolPaginaCLS
                            {
                                iidrolpagina = rolpagina.IIDROLPAGINA,
                                nombreRol = rol.NOMBRE,
                                nombreMensaje = pagina.MENSAJE
                            }).ToList();
            }
                return View(listaRol);
        }

        public void listarComboRol()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.Rol
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDROL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaRol = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarComboPagina()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.Pagina
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.MENSAJE,
                             Value = item.IIDPAGINA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaPagina = lista; //ViewBag pasa info del controles a la vista
            }
        }


        public int Guardar(RolPaginaCLS oRolPaginaCLS, int titulo)
        {
            int rpta = 0;

            using (var bd = new BDPasajeEntities())
            {
                if(titulo ==1)
                {
                    RolPagina oRolPagina = new RolPagina();
                    oRolPagina.IIDROL = oRolPaginaCLS.iidrol;
                    oRolPagina.IIDPAGINA = oRolPaginaCLS.iidpagina;
                    oRolPagina.BHABILITADO = 1;
                    bd.RolPagina.Add(oRolPagina);
                    rpta = bd.SaveChanges();//numero de registros afectado
                }
            }
                return rpta;
        }
        public ActionResult Filtrar(int? iidrol)
        {
            listarComboRol();
            List<RolPaginaCLS> listaRol = new List<RolPaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (iidrol == null)
                {
                    listaRol = (from rolpagina in bd.RolPagina
                                join rol in bd.Rol
                                on rolpagina.IIDROL equals rol.IIDROL
                                join pagina in bd.Pagina
                                on rolpagina.IIDPAGINA equals
                                pagina.IIDPAGINA
                                where rolpagina.BHABILITADO == 1
                                select new RolPaginaCLS
                                {
                                    iidrolpagina = rolpagina.IIDROLPAGINA,
                                    nombreRol = rol.NOMBRE,
                                    nombreMensaje = pagina.MENSAJE
                                }).ToList();
                }
                else
                {
                    listaRol = (from rolpagina in bd.RolPagina
                                join rol in bd.Rol
                                on rolpagina.IIDROL equals rol.IIDROL
                                join pagina in bd.Pagina
                                on rolpagina.IIDPAGINA equals
                                pagina.IIDPAGINA
                                where rolpagina.BHABILITADO == 1
                                && rolpagina.IIDROL == iidrol
                                select new RolPaginaCLS
                                {
                                    iidrolpagina = rolpagina.IIDROLPAGINA,
                                    nombreRol = rol.NOMBRE,
                                    nombreMensaje = pagina.MENSAJE
                                }).ToList();
                }
                
            }
            return PartialView("_TableRolPagina",listaRol);
        }
    }
}