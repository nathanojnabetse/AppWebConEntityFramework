using AppWebTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AppWebTest.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            List<ClienteCLS> listaCliente = null;

            using (var bd = new BDPasajeEntities())
            {
                listaCliente = (from cliente in bd.Cliente
                                select new ClienteCLS
                                {
                                    idCliente = cliente.IIDCLIENTE,
                                    nombre = cliente.NOMBRE,
                                    appaterneno=cliente.APPATERNO,
                                    apmaterno = cliente.APMATERNO,
                                    telefonoFijo = cliente.TELEFONOFIJO
                                }).ToList();
            }
                return View(listaCliente);
        }

        List<SelectListItem> listaSexo;//lista para el combobox del sexo
        private void llenarSexo()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaSexo = (from sexo in bd.Sexo
                             where sexo.BHABILITADO == 1
                             select new SelectListItem
                             {
                                 Text = sexo.NOMBRE,//lo que el user v a aver, seleccionar (campo viible)
                                 Value = sexo.IIDSEXO.ToString()//value (valor interno), id

                             }).ToList();
                listaSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
        }
        public ActionResult Agregar()
        {
            llenarSexo();
            ViewBag.lista = listaSexo; //pasar info del controller a la vista


            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClienteCLS)
        {
            using (var bd=new BDPasajeEntities())
            {

                if(!ModelState.IsValid)
                {
                    llenarSexo();
                    ViewBag.lista = listaSexo;
                    return View(oClienteCLS);
                }
                else
                {
                    Cliente oCliente = new Cliente(); //cliente es clase de EF y lalleno con mi clase
                    oCliente.NOMBRE = oClienteCLS.nombre;
                    oCliente.APPATERNO = oClienteCLS.appaterneno;
                    oCliente.APMATERNO = oClienteCLS.apmaterno;
                    oCliente.EMAIL = oClienteCLS.email;
                    oCliente.DIRECCION = oClienteCLS.direccion;
                    oCliente.IIDSEXO = oClienteCLS.iidsexo;
                    oCliente.TELEFONOCELULAR = oClienteCLS.telefonoCelular;
                    oCliente.TELEFONOFIJO = oClienteCLS.telefonoFijo;
                    oCliente.BHABILITADO = 1;
                    bd.Cliente.Add(oCliente);
                    bd.SaveChanges();
                }
                
            }
            return RedirectToAction("Index");
        }

        //recupera info
        public ActionResult Editar(int id)
        {
            ClienteCLS oClienteCLS = new ClienteCLS();
            
            using (var bd = new BDPasajeEntities())
            {
                //esto por que al revesitR TAMBIEN NECESITA COMBOBOX
                llenarSexo();
                //desde la lista editar
                ViewBag.lista = listaSexo; //pasar info del controller a la vista

                //campos a recuperar y mostrar para editar
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(id)).First();
                oClienteCLS.idCliente = oCliente.IIDCLIENTE;
                oClienteCLS.nombre = oCliente.NOMBRE;
                oClienteCLS.appaterneno = oCliente.APPATERNO;
                oClienteCLS.apmaterno = oCliente.APMATERNO;
                oClienteCLS.direccion = oCliente.DIRECCION;
                oClienteCLS.email = oCliente.EMAIL;
                oClienteCLS.iidsexo = (int)oCliente.IIDSEXO;
                oClienteCLS.email = oCliente.EMAIL;
                oClienteCLS.telefonoCelular = oCliente.TELEFONOCELULAR;
                oClienteCLS.telefonoFijo = oCliente.TELEFONOFIJO;
            }
                return View(oClienteCLS);
        }
    }
}