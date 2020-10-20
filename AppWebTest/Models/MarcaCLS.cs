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
        [Display(Name = "Nombre Marca")]
        public string nombre { get; set; }
        [Display(Name = "Descripción Marca")]
        public string descripcion { get; set; }
        public int nhabilitado { get; set; }

    }
}