using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;

namespace AppWebTest.Controllers
{
    public class MarcaController : Controller
    {
        // GET: Marca
        public ActionResult Index(MarcaCLS oMarcaCLS)//Recibe el modelo
        {
            string nombreMArca = oMarcaCLS.nombre;
            List<MarcaCLS> listaMarca = null;


            using (var bd = new BDPasajeEntities())
            {
                if(oMarcaCLS.nombre == null)//si es nulo muestra todo
                {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS
                                  {
                                      idMarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                }
                else //aqui se pone un filtro con nombreMArca, el filtrado es con un boton
                {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  && marca.NOMBRE .Contains(nombreMArca)
                                  select new MarcaCLS
                                  {
                                      idMarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                }
                
            }
         return View(listaMarca);
        }

        //Genera la vista html
        public ActionResult Agregar()
        {
            return View();
        }

        //Hacer la insercion de datos por  medio del metodo post (Html.BEgin form) Agregar.csHtml
        [HttpPost]
        public ActionResult Agregar(MarcaCLS oMarcaCLS)//Recivbe el modelo
        {
            int numeroDeRegistrosEncontrados = 0;
            string nombreMarca = oMarcaCLS.nombre;
            //eso para evitar cosas repetidas como la marca
            using(var bd = new BDPasajeEntities())
            {
                numeroDeRegistrosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca)).Count();            
            }

            //////////////
            if(!ModelState.IsValid || numeroDeRegistrosEncontrados>=1)
            {
                if(numeroDeRegistrosEncontrados >=1)
                {
                    oMarcaCLS.mensajeError = "El nombre marca ya existe";
                }
                Console.WriteLine("no es valido");
                return View(oMarcaCLS);
                
            }
            else
            {
                Console.WriteLine("es valido");
                using (var bd = new BDPasajeEntities())
                {

                    //MArca sin cls es de Entity, el CLS es que yo cree
                    Marca oMarca = new Marca();
                    oMarca.NOMBRE = oMarcaCLS.nombre;
                    oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                    oMarca.BHABILITADO = 1;
                    bd.Marca.Add(oMarca);
                    bd.SaveChanges();
                }                
            }
            return RedirectToAction("Index");
        }
        

        public ActionResult Editar(int id)
        {

            MarcaCLS oMarcaCLS = new MarcaCLS();
            using(var bd = new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarcaCLS.idMarca = oMarca.IIDMARCA;
                oMarcaCLS.nombre = oMarca.NOMBRE;
                oMarcaCLS.descripcion = oMarca.DESCRIPCION;              
            }
            
            return View(oMarcaCLS);
        }

        [HttpPost]
        public ActionResult Editar(MarcaCLS oMarcaCLS)
        {

            int nResgistradosEncontrados = 0;
            string nombreMarca = oMarcaCLS.nombre;
            int iidmarca = oMarcaCLS.idMarca;
            using (var bd = new BDPasajeEntities())
            {
                nResgistradosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca) && !p.IIDMARCA.Equals(iidmarca) ).Count();
            }
                if (!ModelState.IsValid || nResgistradosEncontrados >=1)
                {
                    if(nResgistradosEncontrados >= 1)
                    {
                    oMarcaCLS.mensajeError = "Ya se encuentra registrada la marca";
                    }
                    return View(oMarcaCLS);

                }

            int idMarca = oMarcaCLS.idMarca;       

            using (var bd = new BDPasajeEntities())
            {
                Marca oMArca = bd.Marca.Where(p => p.IIDMARCA.Equals(idMarca)).First();
                oMArca.NOMBRE = oMarcaCLS.nombre;
                oMArca.DESCRIPCION = oMarcaCLS.descripcion;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            //deshabilitando, para eliminar eliminacio logica
            using (var bd =new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarca.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}