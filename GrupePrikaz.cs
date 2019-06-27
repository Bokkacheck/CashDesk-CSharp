using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace ProjekatProba2
{
    static class GrupePrikaz
    {
        private static DataSet ds;
        private static DataTable dt = RadBaza.NapuniTabelu(new DataTable(), "select naziv, idgrupa from grupa");
        public static PrikazArtikla prikazArtikla;
        public static void KreirajGrupe(Panel grupa,DataSet dataset,DataTable tabela,ref PrikazArtikla delegat)
        {
            int brojDugmica = dt.Rows.Count;
            int sirinaDugmeta = (grupa.Width - 100) / 3;
            int visinaDugmeta = (int)((grupa.Height) / 1.8);
            int pocetnoGore = 0;
            prikazArtikla = delegat;
            for (int i = 0; i < brojDugmica; i++)
            {
                Button b = new Button();
                b.Width = sirinaDugmeta;
                b.Height = visinaDugmeta;
                b.Left = 20 + (sirinaDugmeta + 15) * (i % 3);
                b.Top = pocetnoGore;
                b.Text = dt.Rows[i]["naziv"] + "";
                b.Click += Klik;
                RadSlika.postaviSliku(b, "grupa", dt.Rows[i]["idGrupa"] + "");
                grupa.Controls.Add(b);
                pocetnoGore = i % 3 == 2 ? pocetnoGore + visinaDugmeta + 5 : pocetnoGore;
            }
            ds = dataset;
            dt = tabela;
        }
        private static void Klik(object sender,EventArgs e)
        {
            RadBaza.FilterGrupe(ds,dt, (sender as Button).Text);
            prikazArtikla();
        }
    }   
}
