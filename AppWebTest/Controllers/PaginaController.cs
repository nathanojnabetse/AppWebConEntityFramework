using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class PaginaController : Controller
    {
        // GET: Página
        public ActionResult Index()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.BHABILITADO == 1
                              select new PaginaCLS
                              {
                                  iidpagina = pagina.IIDPAGINA,
                                  mensaje = pagina.MENSAJE,
                                  controlador = pagina.CONTROLADOR,
                                  accion = pagina.ACCION
                              }).ToList();
            }
                return View(listaPagina);
        }

        public ActionResult Filtrar(PaginaCLS oPaginaCLS)
        {
            string mensaje = oPaginaCLS.mensaje;
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if(mensaje == null)
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                }
                else
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   && pagina.MENSAJE.Contains(mensaje)
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                }
            }

            return PartialView("_TablaPagina",listaPagina);
        }

        public int Guardar(PaginaCLS oPaginaCLS, int titulo)
        {
            int rpta = 0;//n de registros afectados
            using (var bd = new BDPasajeEntities())
            {
                if(titulo ==1)
                {
                    Pagina oPagina = new Pagina();
                    oPagina.MENSAJE = oPaginaCLS.mensaje;
                    oPagina.ACCION = oPaginaCLS.accion;
                    oPagina.CONTROLADOR = oPaginaCLS.controlador;
                    oPagina.BHABILITADO = 1;
                    bd.Pagina.Add(oPagina);
                    rpta = bd.SaveChanges();

                }                
            }
            return rpta;
        }
    }
}