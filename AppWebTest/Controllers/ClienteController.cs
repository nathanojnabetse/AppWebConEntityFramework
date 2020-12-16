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
        public ActionResult Index(ClienteCLS oClienteCLS)
        {
            List<ClienteCLS> listaCliente = null;
            int iidsexo = oClienteCLS.iidsexo;
            llenarSexo();
            ViewBag.lista = listaSexo;

            using (var bd = new BDPasajeEntities())
            {
                if(oClienteCLS.iidsexo==0)
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    select new ClienteCLS
                                    {
                                        idCliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        appaterneno = cliente.APPATERNO,
                                        apmaterno = cliente.APMATERNO,
                                        telefonoFijo = cliente.TELEFONOFIJO
                                    }).ToList();
                }
                else
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    && cliente.IIDSEXO == iidsexo
                                    select new ClienteCLS
                                    {
                                        idCliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        appaterneno = cliente.APPATERNO,
                                        apmaterno = cliente.APMATERNO,
                                        telefonoFijo = cliente.TELEFONOFIJO
                                    }).ToList();
                }
                
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
            //
            int nregistrosEncontrados = 0;
            string nombre = oClienteCLS.nombre;
            string apPaterno = oClienteCLS.appaterneno;
            string apmaterno = oClienteCLS.apmaterno;
            using (var bd = new BDPasajeEntities())
            {
                nregistrosEncontrados = bd.Cliente.Where(p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(apPaterno) && p.APMATERNO.Equals(apmaterno)).Count();
            }
            //
            using (var bd = new BDPasajeEntities())
            {

                if (!ModelState.IsValid || nregistrosEncontrados >= 1)
                {
                    if(nregistrosEncontrados>=1)
                    {
                        oClienteCLS.mensajeErrorr = "Ya rxiste cliente registrado";
                    }
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
        
        //cuando el modelo no sea valido en agragar y editar hay que llenar los combos de nuevo en todos los formularios donde se valide el formulario

        [HttpPost]
        public ActionResult Editar(ClienteCLS oClienteCLS)
        {
            int nregistrosEncontrados = 0;
            int idCliente = oClienteCLS.idCliente;
            string nombre = oClienteCLS.nombre;
            string apPaterno = oClienteCLS.appaterneno;
            string apMaterno = oClienteCLS.apmaterno;

            using (var bd = new BDPasajeEntities())
            {
                nregistrosEncontrados = bd.Cliente.Where(
                    p => p.NOMBRE.Equals(nombre) && 
                    p.APPATERNO.Equals(apPaterno) && 
                    p.APMATERNO.Equals(apMaterno) && 
                    !p.IIDCLIENTE.Equals(idCliente)
                    ).Count();
            }
            
            if (!ModelState.IsValid || nregistrosEncontrados >= 1)
            {
                if(nregistrosEncontrados >=1)
                {
                    oClienteCLS.mensajeErrorr = "Ya existe el cliente";
                }
                llenarSexo();

                return View(oClienteCLS);

            }


            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(idCliente)).First();
                
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.appaterneno;
                oCliente.APMATERNO = oClienteCLS.apmaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.IIDSEXO = oClienteCLS.iidsexo;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefonoCelular;
                oCliente.TELEFONOFIJO = oClienteCLS.telefonoFijo;
                
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int idcliente)
        {
            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(idcliente)).First();
                oCliente.BHABILITADO = 0;
                bd.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}