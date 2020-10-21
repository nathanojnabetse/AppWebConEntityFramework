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
        public ActionResult Index()
        {
            List<MarcaCLS> listaMarca = null;

            using (var bd = new BDPasajeEntities())
            {
                listaMarca = (from marca in bd.Marca
                              where marca.BHABILITADO==1
                                             select new MarcaCLS
                                             {
                                                 idMarca = marca.IIDMARCA,
                                                 nombre = marca.NOMBRE,
                                                 descripcion = marca.DESCRIPCION
                                             }).ToList();
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
            if(!ModelState.IsValid)
            {
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
        
    }
}