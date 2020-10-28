using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class ViajeCLS
    {
        [Display(Name ="Id viaje")]
        public int iidViaje { get; set; }
        [Display(Name = "Lugar Origen")]
        [Required]
        public int iidLugarOrigen { get; set; }
        [Display(Name = "Lugar destino")]
        [Required]
        public int iidLugarDestino { get; set; }
        [Display(Name = "Precio")]
        [Required]
        public double precio { get; set; }
        [Display(Name = "Fecha Viaje")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaViaje { get; set; }
        [Display(Name = "Bus")]
        [Required]
        public int iidBus { get; set; }
        [Display(Name = "Numero de ASientos Disponibles")]
        [Required]
        public int numero { get; set; }
        //Propiedades adicionales
        [Display(Name ="Lugar Origen")]
        public string nombreLugarOrigen { get; set; }
        [Display(Name = "Lugar destino")]
        public string nombreLugarDestino { get; set; }
        [Display(Name = "Nombre bus")]
        public string nombreBus { get; set; }
    }
}