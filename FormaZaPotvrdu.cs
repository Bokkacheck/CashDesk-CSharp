using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjekatProba2
{
    public partial class FormaZaPotvrdu : Form
    {
        public FormaZaPotvrdu()
        {
            InitializeComponent();
        }
        public FormaZaPotvrdu(string pitanje)
        {
            InitializeComponent();
            label1.Text = pitanje;
        }
        public static bool provera(string pitanje)
        {
            FormaZaPotvrdu frmProvera = new FormaZaPotvrdu(pitanje);
            if (frmProvera.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
