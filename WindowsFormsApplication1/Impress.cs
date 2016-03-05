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
        /// <summary>
        /// Déclaration des variables
        /// </summary>
        public const bool DEBUG = true;
        /// <summary>
        /// Chaine de texte qui sera imprimée
        /// </summary>
        public String text { get; set; }
        private String docName { get; set; }
        private String docPath { get; set; }
        private String printer { get; set; }
        private String path { get; set; }
        private String stringToPrint { get; set; }
        public PrintDocument docToPrint { get; set; }

        public Font Font { get; set; }

        /// <summary>
        /// Constructeur le plus simple
        /// </summary>
        /// <param name="text">Chaine de texte qui sera imprimée</param>
        public Impress(string text)
        {
            this.text = text;
            this.docToPrint = new PrintDocument();
            docToPrint.PrintPage += docToPrint_PrintPage;
            this.Font = new Font("Arial", 10);
        }
        /// <summary>
        /// Constructeur avec choix de la police d'impression
        /// </summary>
        /// <param name="text">Chine de texte qui sera imprimée</param>
        /// <param name="font">Paramètres de police d'impression</param>
        public Impress(string text, Font font)
        {
            this.text = text;
            this.docToPrint = new PrintDocument();
            docToPrint.PrintPage += docToPrint_PrintPage;
            this.Font = font;
        }
        /// <summary>
        /// Evènement appelé lors du déclenchement de l'impression
        /// </summary>
        private void docToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            stringToPrint = stringToPrint.Substring(charactersOnPage);

            e.HasMorePages = (stringToPrint.Length > 0);
        }
        /// <summary>
        /// Transformation de la chaine de texte en fichier texte temporaire, puis lecture du fichier pour impression
        /// </summary>
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
        /// <summary>
        /// Paramètres d'impression, bientôt plus ici ;)s
        /// </summary>
        private void SetSettings()
        {
            if (DEBUG)
                MessageBox.Show(printer);
            docToPrint.PrinterSettings.PrinterName = printer;
        }
        /// <summary>
        /// Méthode à appeler pour lancer l'impression
        /// </summary>
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
