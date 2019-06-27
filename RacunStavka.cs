using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace ProjekatProba2
{
    class RacunStavka
    {
        private int id;
        private string naziv;
        private double cena;
        private double ukupnaCena;
        private int kolicina;

        public RacunStavka(int id, string naziv, double cena, int kolicina)
        {
            this.id = id;
            this.naziv = naziv;
            this.cena = cena;
            this.kolicina = kolicina;
            ukupnaCena = RacunajCenu();
        }
        public Double RacunajCenu() {
            return kolicina * cena;
        }
        public static void DodajStavku(List<RacunStavka> stavkeRacuna,DataRow red, int kolicina,double cena)
        {
            for(int i = 0; i < stavkeRacuna.Count; i++){
                if (stavkeRacuna[i].id == int.Parse(red["idArtikal"] + "")) {
                    stavkeRacuna[i].Kolicina += kolicina;
                    return;
                }
            }
            stavkeRacuna.Add(new RacunStavka(int.Parse(red["idArtikal"] + ""),red["naziv"]+"",cena,kolicina));
        }
        public static void IzlistajStavke(ListBox lb,List<RacunStavka> stavkeRacuna,Label lbl)
        {
            double ukupnaCena = 0;
            lb.Items.Clear();
            for (int i = 0; i < stavkeRacuna.Count; i++)
            {
                lb.Items.Add(stavkeRacuna[i].ToString());
                ukupnaCena += stavkeRacuna[i].ukupnaCena;
            }
            lbl.Text = ukupnaCena+"";
        }
        public static void ObrisiStavku(ListBox lb, List<RacunStavka> stavkeRacuna, Label lbl,int indeks)
        {
            stavkeRacuna.RemoveAt(indeks);
            IzlistajStavke(lb, stavkeRacuna, lbl);
        }
        public override string ToString()
        {
            return naziv + " " + kolicina + " X " + cena + "= " + ukupnaCena;
        }
        public int Kolicina { get => kolicina; set{ kolicina = value; ukupnaCena = RacunajCenu(); } }
    }
}
