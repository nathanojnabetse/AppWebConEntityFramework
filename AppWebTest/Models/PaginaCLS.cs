using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class PaginaCLS
    {
        [Display(Name = "id Pagina")]
        public int iidpagina { get; set; }
        [Required]
        [Display(Name ="titulo del link")]
        public string mensaje { get; set; }
        [Required]
        [Display(Name = "nombre de la Accion")]
        public string accion { get; set; }
        [Required]
        [Display(Name = "nombre del controlador")]
        public string controlador { get; set; }
        public int bhabilitado { get; set; }
        //propiedad adicional
        public string mensajeFiltro { get; set; }
    }
}