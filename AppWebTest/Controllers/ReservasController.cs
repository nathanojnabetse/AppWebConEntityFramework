using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;
namespace AppWebTest.Controllers
{
    public class ReservasController : Controller
    {
        // GET: Reservas
        public ActionResult Index()
        {
            using (var bd =new BDPasajeEntities())
            {
                listarLugar();

                var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];
                if(pasajesId != null)
                {
                    ViewBag.listaId = pasajesId.Value;
                    ViewBag.listaCantidad = pasajesId.Value;
                }

                var reserva = (from viaje in bd.Viaje
                               join lugar in bd.Lugar
                               on viaje.IIDLUGARORIGEN equals lugar.IIDLUGAR
                               join bus in bd.Bus
                               on viaje.IIDBUS equals bus.IIDBUS
                               join lugarDes in bd.Lugar
                               on viaje.IIDLUGARDESTINO equals lugarDes.IIDLUGAR
                               where viaje.BHABILITADO == 1
                               select new ReservaCLS
                               {
                                   iidViaje=viaje.IIDVIAJE,
                                   nombreArchivo=viaje.nombrefoto,
                                   foto=viaje.FOTO,
                                   lugarDestino = lugar.NOMBRE,
                                   lugarOrigen = lugarDes.NOMBRE,
                                   precio=(decimal)viaje.PRECIO,
                                   fechaviaje=(DateTime)viaje.FECHAVIAJE,
                                   nombreBus=bus.PLACA,
                                   descripcionBus=bus.DESCRIPCION,
                                   totoalAsientos=(int)bus.NUMEROCOLUMNAS * (int)bus.NUMEROFILAS,
                                   asientosdisponibles = (int)viaje.NUMEROASIENTOSDISPONIBLES,
                                   iidBus=bus.IIDBUS
                               }).ToList();
                return View(reserva);
            }               
        }

        //lugar

        public string AgregarCookie(string idViaje, string cantidad, string fechaViaje, string lugarOrigen, string lugarDestino, string precio, int idBus)
        {
            string rpta = "";
            try
            {
                var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];
                if(pasajesId!=null && pasajesCantidad!=null && pasajesCantidad.Value != "" && pasajesId.Value != "")
                {
                    //se crea por segunda vez
                    string idCookie = pasajesId.Value + "{" + idViaje;
                    string cantidadCookie = pasajesCantidad.Value+ "{"+ cantidad + "*" + fechaViaje + "*" + lugarOrigen + "*" + lugarDestino + "*" + precio + "*" + idBus;

                    HttpCookie cookieId = new HttpCookie("pasajesId", idCookie);
                    HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", cantidadCookie);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                }
                else
                {
                    //pasajesCantidad toda la data menos el idviaje
                    string formatoCadena = cantidad + "*" + fechaViaje + "*" + lugarOrigen + "*" + lugarDestino + "*" + precio + "*" + idBus;
                    HttpCookie cookieId = new HttpCookie("pasajesId", idViaje);
                    HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", formatoCadena);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                    //cookies oslo string no arrays
                    //se crea por primera vez
                }
                rpta = "OK";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                rpta = "";
            }
            return rpta;

        }

        public string Quitarcookie(String idViaje)
        {
            string rpta = "";
            try
            {
                var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];
                string valorId = pasajesId.Value;
                string valorCantidad = pasajesCantidad.Value;
                string[] arrayID = valorId.Split('{');
                int indiceID = Array.IndexOf(arrayID, idViaje);

                //6{7{9{2 replace(cadena , valor)
                string nuevoId;
                if (valorId.Contains("{" + idViaje))
                {
                    nuevoId = valorId.Replace("{" + idViaje, "");
                }
                else if (valorId.Contains(idViaje + "{"))
                {
                    nuevoId = valorId.Replace(idViaje+ "{", "");
                }
                else
                {
                    nuevoId = valorId.Replace(idViaje , "");
                }

                List<string> valor = valorCantidad.Split('{').ToList();
                valor.RemoveAt(indiceID);
                string[] arrayCantidad = valor.ToArray();
                string nuevaCantidad = String.Join("{", arrayCantidad);

                HttpCookie cookieId = new HttpCookie("pasajesId", nuevoId);
                HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", nuevaCantidad);
                ControllerContext.HttpContext.Response.SetCookie(cookieId);
                ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);

                rpta = "OK";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                rpta = "";
            }


            return rpta;
        }

        public void listarLugar()
        {

            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.Lugar
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDLUGAR.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaLugar = lista; //ViewBag pasa info del controles a la vista
            }
        }


    }
}