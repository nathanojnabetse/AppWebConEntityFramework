using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class TipoUsuarioCLS
    {
        [Display(Name ="Id tipo Usuario")]
        public int iidtipousuario { get; set; }
        
        [Display(Name = "Nombre tipo Usuario")]
        [Required]
        [StringLength(150,ErrorMessage ="Longitud Maxima 150")]
        public string nombre {get; set;}

        [Display(Name = "Descripcion")]
        [Required]
        [StringLength(250, ErrorMessage = "Longitud Maxima 250")]
        public string descripcion { get; set; }

        public int bhabiliado { get; set; }

    }
}