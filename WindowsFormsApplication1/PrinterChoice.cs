using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class PrinterChoice : Form
    {
        public string printer { get; set; }
        public PrinterChoice()
        {
            InitializeComponent();
        }

        private void PrinterChoice_Load(object sender, EventArgs e)
        {
            foreach (String printer in PrinterSettings.InstalledPrinters)
            {
                cbPrinters.Items.Add(printer.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(cbPrinters.SelectedItem.ToString()))
            {
                this.printer = cbPrinters.SelectedItem.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            }
            this.Close();
        }
    }
}
