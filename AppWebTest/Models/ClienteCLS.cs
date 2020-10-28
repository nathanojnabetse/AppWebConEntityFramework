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
        [Required]
        [StringLength(100, ErrorMessage = "Longitud Mmaxima 100")]
        public string nombre { get; set; }
        [StringLength(150, ErrorMessage = "Longitud Mmaxima 150")]
        [Display(Name = "Apellido paterno")]
        [Required]        
        public string appaterneno { get; set; }
        [Display(Name = "Apellido materno")]
        [Required]
        [StringLength(150, ErrorMessage = "Longitud Mmaxima 150")]
        public string apmaterno { get; set; }
        [Required]
        [Display(Name = "Email")]
        [StringLength(200, ErrorMessage = "Longitud Mmaxima 200")]
        [EmailAddress(ErrorMessage ="Ingrese un email valido")]
        public string email { get; set; }
        [Required]
        [Display(Name = "Dirección")]
        [StringLength(200, ErrorMessage = "Longitud Mmaxima 200")]
        public string direccion { get; set; }
        [Required]
        [Display(Name = "Sexo")]
        public int iidsexo { get; set; }
        [StringLength(10, ErrorMessage = "Longitud Mmaxima 10")]
        [Display(Name = "Telefono Fijo")]
        [Required]
        public string telefonoFijo { get; set; }
        [Required]
        [Display(Name = "Telefono Celular")]
        [StringLength(10, ErrorMessage = "Longitud Mmaxima 10")]
        public string telefonoCelular { get; set; }
        public string bhabilitado { get; set; }
        public string bTieneUsuario { get; set; }
        public string tipoUsuario { get; set; }

    }
}