using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class SucursalCLS
    {
        [Display (Name ="Id Sucursal")]
        public int idsucursal { get; set; }
        [Display(Name = "Nombre Sucursal")]
        public string nombre { get; set; }
        public string dirección { get; set; }
        [Display(Name = "Teléfono Sucursal")]
        public string teléfono { get; set; }
        [Display(Name = "Email Sucursal")]
        public string email { get; set; }
        public DateTime fechaApertura { get; set; }
    }
}