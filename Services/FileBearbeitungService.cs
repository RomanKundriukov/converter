using converter.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Aspose.Pdf;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static converter.ViewModels.ViewModelBase;
using Aspose.Email.Clients.Activity;


namespace converter.Services
{
    public class FileBearbeitungService : FileBearbeitungInterface
    {
        // alle unterschtützte Formate
        private ObservableCollection<Format> _formats;

        //Downloadsdirectory
        private readonly string downloadDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        public FileBearbeitungService()
        {
            //Formats Initialisieren
            _formats = new ObservableCollection<Format>(
                Enum.GetValues(typeof(Format)).Cast<Format>()
            );
        }

        /// <summary>
        /// Prüft ob Dateiformat unterschtützt ist
        /// </summary>
        /// <param name="filePath">Dateipfad</param>
        /// <returns>bool</returns>
        public bool GetFileFormat(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();

           if(extension.StartsWith(".")) 
                extension = extension.Substring(1);

            return _formats.Any(format => format.ToString().ToLower() == extension);
            //return System.IO.Path.GetFileName(filePath);
        }

        /// <summary>
        /// Switcht zwischen den Formaten
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileFormat"></param>
        public string KonvertFile(string filePath, string formatTo) 
        {
            string ergebnis = "Erfolgreich!";

            var extension = Path.GetExtension(filePath).ToLower();

            if(extension == ".pdf" && formatTo == "Docx")
            {
                PdfToDocx(filePath);
                return ergebnis;
            }
            else if (extension == ".docx" && formatTo == "Pdf")
            {
                DocxToPdf(filePath);
                return ergebnis;
            }
            else if (extension == ".xlsx" && formatTo == "Pdf")
            {
                XlsxToPdf(filePath);
                return ergebnis;
            }
            else if (extension == ".xlsx" && formatTo == "Docx")
            {
                XlsxToDocx(filePath);
                return ergebnis;
            }
            else
            {
                ergebnis = "Dateiformat nicht unterstützt!";
                return ergebnis;
            }

        }

        /// <summary>
        /// Konvertiert DOCX in PDF
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DocxToPdf(string filePath)
        {
            Aspose.Words.Document docxDocument = new Aspose.Words.Document(filePath);

            string outputFilePath = GetNewFilePath(filePath: filePath, fileFormat: ".pdf");

            docxDocument.Watermark.Remove();

            docxDocument.Save(outputFilePath, Aspose.Words.SaveFormat.Pdf);
        }

        /// <summary>
        /// Konvertiert PDF in DOCX
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void PdfToDocx(string filePath)
        {
            Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(filePath);

            string outputFilePath = GetNewFilePath(filePath: filePath, fileFormat: ".docx");

            pdfDocument.DisableFontLicenseVerifications = true;

            pdfDocument.Save(outputFilePath, Aspose.Pdf.SaveFormat.DocX);

        }

        /// <summary>
        /// Konvertiert XLSX in DOCX
        /// </summary>
        /// <param name="filePath"></param>
        public void XlsxToDocx(string filePath)
        {

            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(filePath);

            string tempPdfPath = Path.ChangeExtension(filePath, ".temp.pdf");
            workbook.Save(tempPdfPath, Aspose.Cells.SaveFormat.Pdf);

            // 2. PDF -> DOCX
            var pdfDocument = new Aspose.Pdf.Document(tempPdfPath);
            string outputFilePath = GetNewFilePath(filePath: filePath, fileFormat: ".docx");
            pdfDocument.Save(outputFilePath, Aspose.Pdf.SaveFormat.DocX);

            // Optional: temporäre PDF löschen
            File.Delete(tempPdfPath);

           
        }

        /// <summary>
        /// Konvertiert XLSX in PDF
        /// </summary>
        /// <param name="filePath"></param>
        public void XlsxToPdf(string filePath)
        {
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(filePath);
            string outputFilePath = GetNewFilePath(filePath: filePath, fileFormat: ".pdf");
            workbook.Save(outputFilePath, Aspose.Cells.SaveFormat.Pdf);
        }

        /// <summary>
        /// Get New File Path
        /// </summary>
        /// <param name="filePath""></param>
        public string GetNewFilePath(string filePath, string fileFormat)
        {
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            var outputFileName = fileNameWithoutExt + fileFormat;
            var outputFilePath = Path.Combine(downloadDirectory, outputFileName);
            return outputFilePath;
        }

       
    }

}
