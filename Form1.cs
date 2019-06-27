using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace ProjekatProba2
{
    public delegate void PrikazArtikla();
    public partial class Form1 : Form
    {
        private DataTable dt = new DataTable();
        private DataTable dtRacun = new DataTable();
        private List<RacunStavka> stavkeRacuna = new List<RacunStavka>();
        public PrikazArtikla prikazArtikla;
        private double osnovnaUkupnaCena = 0;
        private DataSet ds = new DataSet();
        private DataTable dtGrupe = new DataTable();
        Task t;
        object locker = new object();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RadBaza.NapuniDataSet(ds, ref dt);
            RadBaza.NapuniTabelu(dtGrupe, "select naziv, idgrupa from grupa");
            //timer1.Start();
            //timer1.Interval = 50;
            dpDatumDo.MinDate = dpDatumOd.Value;
            prikazArtikla = PromenjenaTabela;
            UcitavanjeNastavak();
        }
        private void UcitajIzBazePocetak()
        {
            lock (locker)
            {

            }
        }
        private void TajmerPocetak(object sender, EventArgs e)
        {
            //if (t.IsCompleted)
            //{
            //    UcitavanjeNastavak();
            //    timer1.Interval = 1000;
            //    timer1.Tick -= TajmerPocetak;
            //    timer1.Tick += TajmerNastavak;
            //    return;
            //}
        }
        private void UcitavanjeNastavak()
        {
            GrupePrikaz.KreirajGrupe(pnlGrupe, ds, dt, ref prikazArtikla);
            clbGrupe.DataSource = dtGrupe;
            clbGrupe.DisplayMember = "naziv";
            panel1.Visible = false;
        }
        private void TajmerNastavak(object sender,EventArgs e)
        {
            lblDatumRacun.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }
        private void promenaRadnje(object sender, EventArgs e)
        {
            pnlArhiva.Visible = false;
            pnlDodavanje.Visible = false;
            pnlRacunIzdavanje.Visible = false;
            if ((sender as RadioButton).Checked)
            {
                if ((sender as RadioButton).Name == "rb1Racun") pnlRacunIzdavanje.Visible = true;
                else if ((sender as RadioButton).Name == "rb2Artikal") pnlDodavanje.Visible = true;
                else
                {
                    pnlArhiva.Visible = true;
                    RadBaza.NapuniTabelu(dtRacun, "select * from racun");
                }
            }
            AzuriranjePodataka();
            dt.Clear();
            PromenjenaTabela();
            ObrisiSliku();
        }
        private void PromenjenaTabela()
        {
            lbArtikalRacun.SelectedIndex = -1;
            lbArtikalRacun.Items.Clear();
            lbPostojeciArtikli.SelectedIndex = -1;
            lbPostojeciArtikli.Items.Clear();
            foreach (DataRow red in dt.Rows)
            {
                osnovnaUkupnaCena = double.Parse(red["cena"] + "") * ((100 - int.Parse(red["popust"] + "")) / 100.0);
                lbArtikalRacun.Items.Add(red["naziv"] + "  Cena: " + red["cena"] + " Popust: " + red["popust"] + "% Ukupno: " + osnovnaUkupnaCena);
                lbPostojeciArtikli.Items.Add(red["naziv"] + "  Cena: " + red["cena"] + " Popust: " + red["popust"] + "% Ukupno: " + osnovnaUkupnaCena);
            }
        }
        private void PretragaNaziv(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text == "")
                dt.Clear();
            else
                RadBaza.FilterIme(ds, dt,(sender as TextBox).Text);
            PromenjenaTabela();
        }
        private void PromenjenaGrupa(object sender, EventArgs e)
        {
            nudKolicina.Value = 1;
            txtUkupnaCena.Text = "";
            pbSlikaArtikal.Image = null;
            if (lbArtikalRacun.SelectedIndex != -1)
            {
                osnovnaUkupnaCena = double.Parse(dt.Rows[lbArtikalRacun.SelectedIndex]["cena"] + "") * ((100 - int.Parse(dt.Rows[lbArtikalRacun.SelectedIndex]["popust"] + "")) / 100.0);
                txtUkupnaCena.Text = osnovnaUkupnaCena + "";
                RadSlika.prikaziSliku(pbSlikaArtikal, "artikal", dt.Rows[lbArtikalRacun.SelectedIndex]["idartikal"] + "");
            }
        }
        private void btnDodajRacun_Click(object sender, EventArgs e)
        {
            if (lbArtikalRacun.SelectedIndex != -1)
            {
                RacunStavka.DodajStavku(stavkeRacuna, dt.Rows[lbArtikalRacun.SelectedIndex], (int)nudKolicina.Value, osnovnaUkupnaCena);
                RacunStavka.IzlistajStavke(btnObirisiSliku,stavkeRacuna, lblCenaRacun);
            }
        }
        private void btnUkloniRacun_Click(object sender, EventArgs e)
        {
            if (stavkeRacuna.Count == 0)
            {
                MessageBox.Show("Račun je već prazan");
                return;
            }
            if (btnObirisiSliku.SelectedIndex != -1)
                RacunStavka.ObrisiStavku(btnObirisiSliku, stavkeRacuna, lblCenaRacun, btnObirisiSliku.SelectedIndex);
            else
                MessageBox.Show("Odaberite prvo stavku za brisanje");
        }
        private void nudKolicina_ValueChanged(object sender, EventArgs e)
        {
            txtUkupnaCena.Text = ((int)nudKolicina.Value * osnovnaUkupnaCena) + "";
        }
        private void btnStampajRacun_Click(object sender, EventArgs e)
        {
            if (stavkeRacuna.Count != 0)
            {
                if(FormaZaPotvrdu.provera("Da li ste sigurni da želite da završite račun ?"))
                    if (RadBaza.DodajRed("insert into racun (cena,datumVreme) values (" + double.Parse(lblCenaRacun.Text) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')"))
                    {
                        MessageBox.Show("Račun uspešno upisan");
                        stavkeRacuna = new List<RacunStavka>();
                        RacunStavka.IzlistajStavke(btnObirisiSliku, stavkeRacuna, lblCenaRacun);
                        dt.Clear();
                        PromenjenaTabela();
                    }
            }
            else MessageBox.Show("Upis nije moguć za prazan račun");
        }
        private void btnPonitiRacun_Click(object sender, EventArgs e)
        {
            if (stavkeRacuna.Count != 0)
            {
                if (FormaZaPotvrdu.provera("Da li ste sigurni da želite da poništite račun ?"))
                {
                    stavkeRacuna = new List<RacunStavka>();
                    RacunStavka.IzlistajStavke(btnObirisiSliku, stavkeRacuna, lblCenaRacun);
                    dt.Clear();
                    PromenjenaTabela();
                }
                return;
            }
            MessageBox.Show("Račun je već prazan");
        }
        private void btnDodajArtikal_Click(object sender, EventArgs e)
        {
            if(RadBaza.DodajUds(ds, dt, txtNaziv, txtCena, txtPopust, clbGrupe))
            {
                ObrisiSliku();
                txtNaziv.Text = "";
            }
        }
        private void SakriGrupe(object sender, EventArgs e)
        {
            clbGrupe.Visible = false;
        }
        private void btnGrupePrikaz_Click(object sender, EventArgs e)
        {
            clbGrupe.Visible = !clbGrupe.Visible;
        }
        private void clbGrupe_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGrupePrikaz.Text = "";
            foreach (object o in clbGrupe.CheckedItems)
                btnGrupePrikaz.Text += " " + (o as DataRowView)["Naziv"];
            dt.Clear();
            if (clbGrupe.CheckedItems.Count!=0)
            {
                dt = RadBaza.FilterGrupe(ds, dt, btnGrupePrikaz.Text);
            }
            txtNaziv.Text = "";

            PromenjenaTabela();
        }
        private void btnDodajSliku_Click(object sender, EventArgs e)
        {
            RadSlika.dodajSLiku("artikal", "1");
            RadSlika.prikaziSliku(pictureBox1, "artikal", "1");
        }
        private void btnObrisiSliku_Click(object sender, EventArgs e)
        {
            ObrisiSliku();
        }
        private void ObrisiSliku()
        {
            RadSlika.obrisiSliku("artikal", "1");
            pictureBox1.Image = null;
        }
        private void PromenaDatumaRacun(object sender, EventArgs e)
        {
            dpDatumDo.MinDate = dpDatumOd.Value;
            IEnumerable<DataRow> odabrani = dtRacun.AsEnumerable().Where(x => dpDatumOd.Value <= DateTime.Parse((x["datumVreme"] + "")) && dpDatumDo.Value >= DateTime.Parse((x["datumVreme"] + "")));
            if (odabrani.Count() > 0)
            {
                dataGridView3.DataSource = odabrani.CopyToDataTable();
            }
            else
            {
                MessageBox.Show("Nema računa u odabranom periodu");
                dataGridView3.DataSource = null;
            }
        }
        private void dpDatumOd_ValueChanged(object sender, EventArgs e)
        {
            dpDatumDo.MinDate = dpDatumDo.Value;
        }
        private void AzuriranjePodataka()
        {
            if (ds.HasChanges())
            {
                if (FormaZaPotvrdu.provera("Da li želite da sačuvate unete promene ?"))
                {
                    RadBaza.AzurirajPodatke(ds);
                }
                else
                {
                    ds.RejectChanges();
                }
            }
        }
        private void btnSacuvaj_Click(object sender, EventArgs e)
        {
            AzuriranjePodataka();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AzuriranjePodataka();
        }
    }
}
