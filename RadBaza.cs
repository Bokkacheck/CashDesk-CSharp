using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ProjekatProba2
{
    static class RadBaza
    {
        public static DataSet das;
        public static DataTable dat;
        public static string konekcijaString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Prodavnica.accdb";
        public static DataTable NapuniTabelu(DataTable dt,string upit)
        {
            try
            {
                dt.Clear();
                OleDbDataAdapter da = new OleDbDataAdapter(upit, konekcijaString);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                MessageBox.Show("Doslo je do greske prilikom dobavljanja podataka");
                return null;
            }
        }
        public static void NapuniDataSet(DataSet ds,ref DataTable artikli)
        {
            try
            {
                das = ds;
                das.Tables.Add("Artikal");
                das.Tables.Add("Grupa");
                das.Tables.Add("Grupisanje");
                dat = artikli;

                Parallel.For(0,3, citaj);

                artikli = ds.Tables["Artikal"].Clone();
                das.Tables["artikal"].Columns["idartikal"].AutoIncrement = true;
                das.Tables["artikal"].Columns["idartikal"].AutoIncrementSeed = int.Parse(das.Tables["artikal"].Rows[ds.Tables["artikal"].Rows.Count - 1]["idartikal"] + "") + 1;
                das.Tables["artikal"].Columns["idartikal"].AutoIncrementStep = 1;

            }
            catch (Exception e)
            {
                MessageBox.Show("Doslo je do greske prilikom dobavljanja podataka"+Environment.NewLine+e.Message);
            }
        }
        private static void citaj(int i)
        {
            OleDbConnection konekcija1 = new OleDbConnection(konekcijaString);
            OleDbDataAdapter da = new OleDbDataAdapter();
            if (i == 0)
            {
                da = new OleDbDataAdapter("select * from artikal;", konekcija1);
            }
            else if(i == 1)
            {
                da = new OleDbDataAdapter("select * from grupa;", konekcija1);
            }
            else if(i == 2)
            {
                
                da = new OleDbDataAdapter("select * from grupisanje;", konekcija1);
            }
            da.Fill(das.Tables[i]);
        }
        public static bool AzurirajPodatke(DataSet ds)
        {
            try
            {
                try
                {
                    OleDbDataAdapter da = new OleDbDataAdapter("select * from Artikal", konekcijaString);
                    OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                    da.UpdateCommand = cb.GetUpdateCommand();
                    da.Update(ds.Tables["Artikal"]);
                }
                catch
                {
                    MessageBox.Show("Ukoliko brišete neki zapis ponovite čuvanje još jednom");
                }
                OleDbDataAdapter da2 = new OleDbDataAdapter("select * from Grupisanje", konekcijaString);
                OleDbCommandBuilder cb2 = new OleDbCommandBuilder(da2);
                da2.UpdateCommand = cb2.GetUpdateCommand();
                da2.Update(ds.Tables["Grupisanje"]);
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Doslo je do greške" + Environment.NewLine + e.Message);
                return false;
            }
        }
        public static bool DodajRed(string naredba)
        {
            OleDbCommand komanda = new OleDbCommand(naredba, new OleDbConnection(konekcijaString));
            try
            {
                komanda.Connection.Open();
                komanda.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Doslo je do greske prilikom upisa racuna u bazu podataka" + Environment.NewLine + e.Message);
                return false;
            }
            finally
            {
                komanda.Connection.Close();
            }
        }
        public static bool DodajUds(DataSet ds,DataTable dt,TextBox txtNaziv,TextBox txtCena,TextBox txtPopust,CheckedListBox clbGrupe)
        {
            if (Validacije.DodavanjeArtikla(txtNaziv, txtCena, txtPopust, clbGrupe))
            {
                if ((dt.AsEnumerable().Where(x => (x["naziv"] + "").ToLower() == txtNaziv.Text.ToLower())).Count() == 0)
                {
                    DataRow red = ds.Tables["Artikal"].NewRow();
                    red["naziv"] = txtNaziv.Text;
                    red["cena"] = double.Parse(txtCena.Text);
                    red["popust"] = int.Parse(txtPopust.Text);
                    ds.Tables["Artikal"].Rows.Add(red);
                    foreach (object o in clbGrupe.CheckedItems)
                    {
                        DataRow grupaRed = ds.Tables["Grupisanje"].NewRow();
                        grupaRed["idArtikal"] = int.Parse(ds.Tables["Artikal"].Rows[ds.Tables["Artikal"].Rows.Count - 1]["idArtikal"] + "");
                        grupaRed["idgrupa"] = int.Parse((o as DataRowView)["idgrupa"] + "");
                        ds.Tables["Grupisanje"].Rows.Add(grupaRed);
                    }
                    RadSlika.promenaIDBRaSlika("artikal", "1", ds.Tables["Artikal"].Rows[ds.Tables["Artikal"].Rows.Count - 1]["idArtikal"] + "");
                    MessageBox.Show("Artikal uspešno dodat");
                    return true;
                }
                else
                {
                    MessageBox.Show("Već postoji ovakav artikal");
                    return false;
                }
            }
            return false;
        }
        public static DataTable FilterGrupe(DataSet ds, DataTable dt, string grupe)
        {
            dt.Clear();
            var upit = from p in ds.Tables["Artikal"].AsEnumerable()
                       join x in ds.Tables["Grupisanje"].AsEnumerable() on p.Field<int>("idartikal") equals x.Field<int>("idartikal")
                       join k in ds.Tables["Grupa"].AsEnumerable() on x.Field<int>("idgrupa") equals k.Field<int>("idgrupa")
                       where grupe.Contains(k.Field<string>("Naziv"))
                       select  p;
            foreach (var p in upit.Distinct())
                dt.ImportRow(p);
            return dt;
        }
        public static DataTable FilterIme(DataSet ds,DataTable dt,string tekst)
        {
            dt.Clear();
            var upit1 = from p in ds.Tables["Artikal"].AsEnumerable()
                       where p.Field<string>("Naziv").StartsWith(tekst)
                       select p;
            var upit = ds.Tables["Artikal"].AsEnumerable().Where(x => x.Field<string>("naziv").ToLower().StartsWith(tekst.ToLower()));
            foreach (var p in upit)
                dt.ImportRow(p);
            return dt;
        }
    }
}
