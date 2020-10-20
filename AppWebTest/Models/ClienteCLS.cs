using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class ClienteCLS
    {
        
        [Display (Name = "Id Cliente")]
        public int idCliente { get; set; }
        [Display (Name = "Nombre Cliente")]
        public string nombre { get; set; }
        [Display(Name = "Apellido paterno")]
        public string appaterneno { get; set; }
        [Display(Name = "Apellido materno")]
        public string apmaterno { get; set; }
        public string email { get; set; }
        public string iddireccion { get; set; }
        [Display(Name = "Telefono Fijo")]
        public string telefonoFijo { get; set; }
        public string telefonoCelular { get; set; }
        public string bhabilitado { get; set; }
        public string bTieneUsuario { get; set; }
        public string tipoUsuario { get; set; }

    }
}