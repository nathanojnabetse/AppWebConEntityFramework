using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebTest.Models
{
    public class MarcaCLS
    {
        //modificar el nombre de las columnas
        //Display columnas que voy a mostrar
        [Display(Name = "Id Marca")]       
        public int idMarca { get; set; }

        // la etiqueta required lo hace obligatorio
        [Display(Name = "Nombre Marca")]
        [Required]
        [StringLength(100,ErrorMessage ="La longitud maxima es 100")]
        public string nombre { get; set; }
        
        [Display(Name = "Descripción Marca")]
        [Required]
        [StringLength(200, ErrorMessage = "La longitud maxima es 200")]
        public string descripcion { get; set; }

        public int nhabilitado { get; set; }

        //añado una propiedad (errores de validacion)

        public string mensajeError { get; set; }
    }
}