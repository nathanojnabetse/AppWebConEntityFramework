using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class ViajeController : Controller
    {
        // GET: Viaje
        public ActionResult Index()
        {
            listarCombos();
            List<ViajeCLS> listaViaje = null;
            using (var bd = new BDPasajeEntities() )
            {
                listaViaje = (from viaje in bd.Viaje
                              join lugarOrigen in bd.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                              join lugarDestino in bd.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                              join bus in bd.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              where viaje.BHABILITADO == 1
                              select new ViajeCLS
                              {
                                  iidViaje = viaje.IIDVIAJE,
                                  nombreBus = bus.PLACA,
                                  nombreLugarOrigen = lugarOrigen.NOMBRE,
                                  nombreLugarDestino = lugarDestino.NOMBRE,

                              }).ToList();
            }
                return View(listaViaje);
        }

        public ActionResult Agregar()
        {
            listarCombos();
            return View();
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

        public void listarBus()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())//llamando al modelo
            {
                lista = (from item in bd.Bus
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.PLACA,
                             Value = item.IIDBUS.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaBus = lista; //ViewBag pasa info del controles a la vista
            }
        }

        public void listarCombos()
        {
            listarBus();
            listarLugar();
        }

        public string Guardar(ViajeCLS oViajeCLS, HttpPostedFileBase foto, int titulo)
        {
            string mensaje = "";
            try
            {
                if (!ModelState.IsValid || (foto == null &&titulo==-1))
                {
                    var query = (from state in ModelState.Values//valores
                                 from error in state.Errors//mensajes
                                 select error.ErrorMessage).ToList();
                    

                    if(foto == null && titulo == -1)
                    {
                        oViajeCLS.mensaje = "la foto es obligatoria";
                        mensaje += "<ul><li> Debe ingresar la foto </li></ul>";
                    }

                    mensaje += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        mensaje += "<li class='list-group-item'>" + item + "</li>";
                    }
                    mensaje += "</ul>";
                }
                else
                {
                    byte[] fotoBD = null;
                    if(foto!=null)
                    {
                        BinaryReader lector = new BinaryReader(foto.InputStream);
                        fotoBD = lector.ReadBytes((int)foto.ContentLength);


                    }
                    using (var bd = new BDPasajeEntities())
                    {
                        if(titulo ==-1)
                        {
                            Viaje oViaje = new Viaje();
                            oViaje.IIDBUS = oViajeCLS.iidBus;
                            oViaje.IIDLUGARDESTINO = oViajeCLS.iidLugarDestino;
                            oViaje.IIDLUGARORIGEN = oViajeCLS.iidLugarOrigen;
                            oViaje.PRECIO = oViajeCLS.precio;
                            oViaje.FECHAVIAJE = oViajeCLS.fechaViaje;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.numeroAsientosDisponibles;
                            oViaje.FOTO = fotoBD;
                            oViaje.nombrefoto = oViajeCLS.nombreFoto;
                            oViaje.BHABILITADO = 1;
                            bd.Viaje.Add(oViaje);

                            mensaje = bd.SaveChanges().ToString();
                            if(mensaje=="0")
                            {
                                mensaje = "";
                            }
                        }
                        else
                        {
                            Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == titulo).First();
                            oViaje.IIDLUGARDESTINO = oViajeCLS.iidLugarDestino;
                            oViaje.IIDLUGARORIGEN = oViajeCLS.iidLugarOrigen;
                            oViaje.PRECIO = oViajeCLS.precio;
                            oViaje.FECHAVIAJE = oViajeCLS.fechaViaje;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.numeroAsientosDisponibles;
                            if (foto != null)
                            {
                                oViaje.FOTO = fotoBD;
                            }
                            mensaje = bd.SaveChanges().ToString();
                            
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                mensaje = "";
            }
            return mensaje;
        }

        public ActionResult Filtrar(int? lugarDestinoParametro)
        {
            List<ViajeCLS> listaViaje = new List<ViajeCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if(lugarDestinoParametro == null)
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO==1
                                  select new ViajeCLS
                                  {
                                      iidViaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarOrigen.NOMBRE,
                                      nombreLugarDestino = lugarDestino.NOMBRE,

                                  }).ToList();
                }
                else
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  && viaje.IIDLUGARDESTINO==lugarDestinoParametro
                                  select new ViajeCLS
                                  {
                                      iidViaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarOrigen.NOMBRE,
                                      nombreLugarDestino = lugarDestino.NOMBRE,

                                  }).ToList();
                }
            }
            return PartialView("_TablaViaje", listaViaje);
        }

        public JsonResult recuperarInformacion(int idViaje)
        {
            ViajeCLS oViajeCLS = new ViajeCLS();
            using (var bd = new BDPasajeEntities())
            {
                Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                oViajeCLS.iidViaje = oViaje.IIDVIAJE;
                oViajeCLS.iidBus = (int)oViaje.IIDBUS;
                oViajeCLS.iidViaje = oViaje.IIDVIAJE;
                oViajeCLS.iidLugarDestino = (int)oViaje.IIDLUGARDESTINO;
                oViajeCLS.iidLugarOrigen = (int)oViaje.IIDLUGARORIGEN;
                oViajeCLS.precio = (decimal)oViaje.PRECIO;
                //año-mes-dia pide
                //bd (dd-mm-yy)

                oViajeCLS.fechaViajeCadena = ((DateTime)oViaje.FECHAVIAJE).ToString("yyyy-MM-dd");
                oViajeCLS.numeroAsientosDisponibles = (int)oViaje.NUMEROASIENTOSDISPONIBLES;
                oViajeCLS.nombreFoto = oViaje.nombrefoto;
                oViajeCLS.extension = Path.GetExtension(oViaje.nombrefoto);
                oViajeCLS.fotoRecuperarCadena = Convert.ToBase64String(oViaje.FOTO);
            }

            return Json(oViajeCLS, JsonRequestBehavior.AllowGet);
        }

        public int EliminarViaje(int idViaje)
        {
            int nregistrosAfectados = 0;

            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                    oViaje.BHABILITADO = 0;
                    nregistrosAfectados = bd.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }
        
    }
}