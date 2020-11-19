using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class EmpleadoCLS
    {
        [Display(Name = "Id Empleado")]        
        public int iidEmpleado { get; set; }
        [StringLength(100,ErrorMessage ="Long max 100")]
        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [StringLength(200, ErrorMessage = "Long max 200")]
        [Required]
        [Display(Name = "Apellido Paterno")]
        public string apPaterno { get; set; }
        [StringLength(200, ErrorMessage = "Long max 200")]
        [Required]
        [Display(Name = "Apellido Materno")]
        public string apMaterno { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Fecha Contrato")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaContrato { get; set; }
        [Required]
        [Display(Name = "Tipo Usuario")]
        public int idtipoUsuario { get; set; }
        [Required]
        [Display(Name = "Tipo contrato")]
        public int idtipoContrato { get; set; }
        [Required]
        [Display(Name = "Sexo")]
        public int iidSexo { get; set; }
        public int bhabilitado { get; set; }
        [Display(Name = "Sueldo")]
        [Required]
        [Range(0,100000,ErrorMessage ="Fuera de rango")]
        public decimal sueldo { get; set; }

        //propiedades adicionales
        [Display(Name ="Tipo Contrato")]
        public string nombreTipoContrato { get; set; }//esto se muesttra en la vista
        [Display(Name = "Tipo Usuario")]
        public string nombreTipoUsuario { get; set; }
        //otra
        public string mensajeError { get; set; }

    }
}