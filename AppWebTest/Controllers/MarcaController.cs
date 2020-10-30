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
            
            if(!ModelState.IsValid)
            {
                
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

        
    }
}