using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace ProjekatProba2
{
    static class RadSlika
    {
        public static bool dodajSLiku(string tip, string IDBR)
        {
            if (IDBR == "")
            {
                MessageBox.Show("Odaberite " + (tip == "korisnik" ? "korisnika " : "automobil "));
                return false;
            }
            else
            {
                string imeSlike = tip + IDBR + ".jpg";
                OpenFileDialog pronadjiSliku = new OpenFileDialog();
                pronadjiSliku.Filter = "JPG(*.JPG)|*.jpg";
                if (pronadjiSliku.ShowDialog() == DialogResult.OK)
                {
                    File.Delete("slike/" + imeSlike);
                    string putanjaNoveSlike = pronadjiSliku.FileName;
                    Image slika = Image.FromFile(putanjaNoveSlike);
                    slika.Save("slike/" + imeSlike);
                    MessageBox.Show("Slika dodata");
                    return true;
                }
            }
            return false;
        }
        public static bool obrisiSliku(string tip, string IDBR)
        {
            if (IDBR != "")
            {
                File.Delete("slike/" + tip + IDBR + ".jpg");
                return true;
            }
            else return false;
        }
        public static bool promenaIDBRaSlika(string tip, string stariIDBR, string noviIDBR)
        {
            string staroImeSlike = tip + stariIDBR + ".jpg";
            string novoImeSlike = tip + noviIDBR + ".jpg";
            if (File.Exists("slike/" + staroImeSlike))
            {
                File.Move("slike/" + staroImeSlike, "slike/" + novoImeSlike);
                return true;
            }
            return false;
        }
        public static bool prikaziSliku(PictureBox pbSlika, string tip, string IDBR)
        {
            if (File.Exists("slike/" + tip + IDBR + ".jpg"))
            {

                FileStream slika = new FileStream("slike/" + tip + IDBR + ".jpg", FileMode.Open, FileAccess.Read);
                pbSlika.Image = Image.FromStream(slika);
                slika.Close();
                return true;
            }
            else
            {
                FileStream slika = new FileStream("slike/default" + tip + ".jpg", FileMode.Open, FileAccess.Read);
                pbSlika.Image = Image.FromStream(slika);
                slika.Close();
                return false;
            }
        }
        public static bool postaviSliku(Button b, string tip, string IDBR)
        {
            b.BackgroundImageLayout = ImageLayout.Stretch;
            if (File.Exists("slike/" + tip + IDBR + ".jpg"))
            {

                FileStream slika = new FileStream("slike/" + tip + IDBR + ".jpg", FileMode.Open, FileAccess.Read);
                b.BackgroundImage = Image.FromStream(slika);
                slika.Close();
                return true;
            }
            else
            {
                FileStream slika = new FileStream("slike/default" + tip + ".jpg", FileMode.Open, FileAccess.Read);
                b.BackgroundImage = Image.FromStream(slika);
                slika.Close();
                return false;
            }
        }
    }
}
