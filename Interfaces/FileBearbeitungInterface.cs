using Avalonia.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace converter.Interfaces
{
    interface FileBearbeitungInterface 
    {
        bool GetFileFormat(string filePath);

        void PdfToDocx(string filePath);

        void DocxToPdf(string filePath);

        void XlsxToPdf(string filePath);
        void XlsxToDocx(string filePath);

        string KonvertFile(string filePath, string formatTo);

        string GetNewFilePath(string filePath, string fileFormat);
    }
}
