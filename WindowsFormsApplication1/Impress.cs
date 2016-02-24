using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Impress
    {
        public const bool DEBUG = true;
        public String text { get; set; }
        private String docName { get; set; }
        private String docPath { get; set; }
        private String printer { get; set; }
        private String path { get; set; }
        private String stringToPrint { get; set; }
        public PrintDocument docToPrint { get; set; }

        public Font Font { get; set; }

        public Impress(string text)
        {
            this.text = text;
            this.docToPrint = new PrintDocument();
            docToPrint.PrintPage += docToPrint_PrintPage;
            this.Font = new Font("Arial", 10);
        }

        void docToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);
        }

        private void ReadFile()
        {
            string docName = "TEMPORARY.txt";
            string docPath = @"C:\Users\Dussert Cyril\Desktop";

            if (!Directory.Exists(docPath))
                Directory.CreateDirectory(docPath);
            path = Path.Combine(docPath, docName);
            File.WriteAllText(path, text);

            docToPrint.DocumentName = "Programme CNC";
            using (FileStream stream = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                stringToPrint = reader.ReadToEnd();
            }
        }

        private void SetSettings()
        {
            if (DEBUG)
                MessageBox.Show(printer);
            docToPrint.PrinterSettings.PrinterName = printer;
        }

        public void Start()
        {
            try
            {
                PrinterChoice choice = new PrinterChoice();
                if (choice.ShowDialog() == DialogResult.OK)
                {
                    printer = choice.printer;
                    ReadFile();
                    SetSettings();
                    docToPrint.Print();
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
