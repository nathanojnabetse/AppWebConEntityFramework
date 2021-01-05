using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class RolCLS
    {
        [Display(Name = "Id Rol")]
        public int iidRol { get; set; }
        [Required]
        [Display(Name = "Nombre Rol")]
        public string nombre { get; set; }
        [Required]
        [Display(Name = "Descripción Rol")]
        public string descripcion { get; set; }
        public int bhabilitado { get; set; }
    }
}