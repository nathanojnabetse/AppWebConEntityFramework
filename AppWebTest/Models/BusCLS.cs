using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class BusCLS
    {
        [Display(Name ="Id Bus")]
        public int iidBus { get; set; }
        [Display(Name = "Nombre sucursal")]
        [Required]
        public int iidSucursal { get; set; }
        [Display(Name = "Tipo Bus")]
        [Required]
        public int iidTipoBus { get; set; }
        [Display(Name ="Placa")]
        [Required]
        [StringLength(100, ErrorMessage = "Long max 100")]
        public string placa { get; set; }
        [Display(Name = "Fecha Compra")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaCompra { get; set; }
        [Display(Name = "Nombre Modelo")]
        [Required]
        public int iidModelo { get; set; }
        [Display(Name = "Numero filas")]
        [Required]
        public int numeroFilas { get; set; }
        [Display(Name = "Numero columnas")]
        [Required]
        public int numeroColumnas { get; set; }
        public int bhabilitado { get; set; }
        [Display(Name = "Descripcion")]
        [Required]
        [StringLength(200, ErrorMessage = "Long max 200")]
        public string descripcion { get; set; }
        [Display(Name = "Observacion")]
        [StringLength(200, ErrorMessage = "Long max 200")]
        public string observacion { get; set; }
        [Display(Name = "Nombre Marca")]//al agregar y editar la info sale del combobox
        [Required]
        public int iidmarca { get; set; }
        //propiedades adicionales
        [Display(Name = "Nombre sucursal")]
        public string nombreSucursal { get; set; }
        [Display(Name = "Nombre tipo Bus")]
        public string nombreTipoBus { get; set; }
        [Display(Name = "Nombre modelo")]
        public string nombreModelo { get; set; }

        public string mensajeError { get; set; }
    }
}