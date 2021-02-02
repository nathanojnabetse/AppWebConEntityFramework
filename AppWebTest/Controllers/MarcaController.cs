using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebTest.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using AppWebTest.Filters;

namespace AppWebTest.Controllers
{
    [Acceder]
    public class MarcaController : Controller
    {
        // GET: Marca
        public ActionResult Index(MarcaCLS oMarcaCLS)//Recibe el modelo
        {
            string nombreMArca = oMarcaCLS.nombre;
            List<MarcaCLS> listaMarca = null;


            using (var bd = new BDPasajeEntities())
            {
                if(oMarcaCLS.nombre == null)//si es nulo muestra todo
                {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS
                                  {
                                      idMarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    //guardar lista amrca en un session para los pdfs
                    Session["lista"] = listaMarca;
                }
                else //aqui se pone un filtro con nombreMArca, el filtrado es con un boton
                {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  && marca.NOMBRE .Contains(nombreMArca)
                                  select new MarcaCLS
                                  {
                                      idMarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    Session["lista"] = listaMarca;
                }
                
            }
         return View(listaMarca);
        }

        //Genera la vista html
        public ActionResult Agregar()
        {
            return View();
        }

        //Hacer la insercion de datos por  medio del metodo post (Html.BEgin form) Agregar.csHtml
        [HttpPost]
        public ActionResult Agregar(MarcaCLS oMarcaCLS)//Recivbe el modelo
        {
            int numeroDeRegistrosEncontrados = 0;
            string nombreMarca = oMarcaCLS.nombre;
            //eso para evitar cosas repetidas como la marca
            using(var bd = new BDPasajeEntities())
            {
                numeroDeRegistrosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca)).Count();            
            }

            //////////////
            if(!ModelState.IsValid || numeroDeRegistrosEncontrados>=1)
            {
                if(numeroDeRegistrosEncontrados >=1)
                {
                    oMarcaCLS.mensajeError = "El nombre marca ya existe";
                }
                Console.WriteLine("no es valido");
                return View(oMarcaCLS);
                
            }
            else
            {
                Console.WriteLine("es valido");
                using (var bd = new BDPasajeEntities())
                {

                    //MArca sin cls es de Entity, el CLS es que yo cree
                    Marca oMarca = new Marca();
                    oMarca.NOMBRE = oMarcaCLS.nombre;
                    oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                    oMarca.BHABILITADO = 1;
                    bd.Marca.Add(oMarca);
                    bd.SaveChanges();
                }                
            }
            return RedirectToAction("Index");
        }
        

        public ActionResult Editar(int id)
        {

            MarcaCLS oMarcaCLS = new MarcaCLS();
            using(var bd = new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarcaCLS.idMarca = oMarca.IIDMARCA;
                oMarcaCLS.nombre = oMarca.NOMBRE;
                oMarcaCLS.descripcion = oMarca.DESCRIPCION;              
            }
            
            return View(oMarcaCLS);
        }

        [HttpPost]
        public ActionResult Editar(MarcaCLS oMarcaCLS)
        {

            int nResgistradosEncontrados = 0;
            string nombreMarca = oMarcaCLS.nombre;
            int iidmarca = oMarcaCLS.idMarca;
            using (var bd = new BDPasajeEntities())
            {
                nResgistradosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca) && !p.IIDMARCA.Equals(iidmarca) ).Count();
            }
                if (!ModelState.IsValid || nResgistradosEncontrados >=1)
                {
                    if(nResgistradosEncontrados >= 1)
                    {
                    oMarcaCLS.mensajeError = "Ya se encuentra registrada la marca";
                    }
                    return View(oMarcaCLS);

                }

            int idMarca = oMarcaCLS.idMarca;       

            using (var bd = new BDPasajeEntities())
            {
                Marca oMArca = bd.Marca.Where(p => p.IIDMARCA.Equals(idMarca)).First();
                oMArca.NOMBRE = oMarcaCLS.nombre;
                oMArca.DESCRIPCION = oMarcaCLS.descripcion;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            //deshabilitando, para eliminar eliminacio logica
            using (var bd =new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarca.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public FileResult generarPDF()
        {
            Document doc = new Document();
            byte[] buffer;

            using(MemoryStream ms = new MemoryStream())
            {
                PdfWriter.GetInstance(doc, ms);//guardar el doc en memoria
                doc.Open();
                Paragraph title = new Paragraph("Lista Marca");
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);//añadido al documento 

                Paragraph espacio = new Paragraph("Espacio");
                doc.Add(espacio);

                //Columnas
                PdfPTable table = new PdfPTable(3);//tabla de 3 col
                float[] values = new float[3] { 30, 40, 80 }; //anchos de col
                table.SetWidths(values);//anchos asignados a la tabla
                //creando las Celdas 
                //creando celdas y poniendo color ademas dealinear el contenido al centro
                PdfPCell celda1 = new PdfPCell(new Phrase("id Marca"));
                celda1.BackgroundColor = new BaseColor(130, 130, 130);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(celda1);

                PdfPCell celda2 = new PdfPCell(new Phrase("Nombre"));
                celda2.BackgroundColor = new BaseColor(130, 130, 130);
                celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(celda2);

                PdfPCell celda3 = new PdfPCell(new Phrase("Descripcion"));
                celda3.BackgroundColor = new BaseColor(130, 130, 130);
                celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(celda3);

                List<MarcaCLS> lista = (List<MarcaCLS>)Session["lista"];
                int nregistros = lista.Count;
                for(int i=0; i<nregistros;i++)
                {
                    table.AddCell(lista[i].idMarca.ToString());
                    table.AddCell(lista[i].nombre);
                    table.AddCell(lista[i].descripcion);
                }


                doc.Add(table);
                doc.Close();
                //usar un sesion es una variable (suoper global)que vive en la app y se puede llamardesde cualquier controlados



                buffer = ms.ToArray();//obtenerlo para usarlo en el buffer


            }

            return File(buffer, "application/pdf");            
        }

        public FileResult generarExcel()
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                //Todo el documento
                ExcelPackage ep = new ExcelPackage();
                //una hoja asociaada al doc
                ep.Workbook.Worksheets.Add("Reporte");
                //ExcelWorksheet ew = ep.Workbook.Worksheets[1];
                var currentSheet = ep.Workbook.Worksheets;

                var ew = currentSheet.First();
                //nombre de las columnas
                ew.Cells[1, 1].Value = "Id Marca";
                ew.Cells[1, 2].Value = "Nombre";
                ew.Cells[1, 3].Value = "Descripcion";

                ew.Column(1).Width = 20;
                ew.Column(2).Width = 40;
                ew.Column(3).Width = 180;

                using (var range = ew.Cells[1,1,1,3])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkRed);

                }
                List<MarcaCLS> lista = (List<MarcaCLS>)Session["lista"];
                int nregistros = lista.Count;

                for(int i = 0; i < nregistros; i++ )
                {
                    ew.Cells[i + 2, 1].Value = lista[i].idMarca;
                    ew.Cells[i + 2, 2].Value = lista[i].nombre;
                    ew.Cells[i + 2, 3].Value = lista[i].descripcion;
                }

                ep.SaveAs(ms);

                buffer = ms.ToArray();
            }

                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}