using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjekatProba2
{
    static class Validacije
    {
        public static bool DodavanjeArtikla(TextBox naziv, TextBox cena, TextBox popust, CheckedListBox grupa)
        {
            if (naziv.Text == "" || cena.Text == "" || popust.Text == "")
            {
                MessageBox.Show("Niste popunili sva polja! ");
                return false;
            }
            if (grupa.CheckedItems.Count == 0)
            {
                MessageBox.Show("Niste odabrali grupu! ");
                return false;
            }
            cena.Text = cena.Text.Replace(",", ".");
            string odgovor = "";
            bool provera = true;
            Match regularniIzraz = Regex.Match(naziv.Text, @"[\w\s]{3,30}$");
            if (!regularniIzraz.Success)
            {
                naziv.Clear();
                odgovor += "Naziv moze sadrzati brojeve i slova kao i razmake i biti dužine izmedđu 3 i 30 karaktera" + Environment.NewLine;
                provera = false;    
            }
            int p;
            if (int.TryParse(popust.Text, out p))
            {
                if (p < 0 || p > 100)
                {
                    popust.Clear();
                    odgovor += "Popust može biti ceo broj između 0 i 100" + Environment.NewLine;
                    provera = false;
                }
            }
            else
            {
                popust.Clear();
                odgovor += "Popust može biti ceo broj između 0 i 100" + Environment.NewLine;
                provera = false;
            }
            double c;
            if (double.TryParse(cena.Text, out c))
            {
                if (c < 1 || c > 999999)
                {
                    cena.Clear();
                    odgovor += "Cena može biti broj između 1.00 i 999999.00" + Environment.NewLine;
                    provera = false;
                }
            }
            else
            {
                cena.Clear();
                odgovor += "Cena može biti broj između 1.00 i 999999.00" + Environment.NewLine;
                provera = false;
            }
            if (!provera) MessageBox.Show(odgovor);
            return provera;
        }
    }
}
