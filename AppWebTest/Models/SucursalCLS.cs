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
        [Required]
        [StringLength(100,ErrorMessage ="Longitud Maxima 100")]
        public string nombre { get; set; }
        [Required]
        public string direccion { get; set; }
        [Display(Name = "Teléfono Sucursal")]
        [Required]
        [StringLength(10, ErrorMessage = "Longitud Maxima 10")]
        public string telefono { get; set; }
        [Display(Name = "Email Sucursal")]
        [Required]
        [StringLength(10, ErrorMessage = "Longitud Maxima 10")]
        [EmailAddress(ErrorMessage ="INgrese un email valido")]
        public string email { get; set; }
        [Required]
        [Display(Name = "Fecha Apertura")]
        [DataType(DataType.Date)]//control fecha
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]//para que permita registrar en ese formato para evitar prblemas en la bdd
        public DateTime fechaApertura { get; set; }

        public int bhabilitado { get; set; }
    }
}