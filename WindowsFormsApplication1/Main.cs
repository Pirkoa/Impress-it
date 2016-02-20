using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        public const bool DEBUG = false;
        private string stringToPrint;
        private String path;
        private String printer;
        public Main()
        {
            InitializeComponent();
        }

        private void ReadFile()
        {
            string docName = "TEMPORARY.txt";
            string docPath = @"C:\Users\Dussert Cyril\Desktop";

            if (!Directory.Exists(docPath))
                Directory.CreateDirectory(docPath);
            path = Path.Combine(docPath, docName);
            File.WriteAllText(path, textBox1.Text);

            docToPrint.DocumentName = "Programme CNC";
            using (FileStream stream = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                stringToPrint = reader.ReadToEnd();
            }
        }

        private void SetSettings()
        {
            if(DEBUG)
                MessageBox.Show(printer);
            docToPrint.PrinterSettings.PrinterName = printer;
        }

        private void docToPrint_PrintPage(object sender, PrintPageEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
