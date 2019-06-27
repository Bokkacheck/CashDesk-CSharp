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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = !checkedListBox1.Visible;
        }
        class Student
        {
            public int id;
            public List<string> grupa;
            public Student(int i,List<string>s)
            {
                id = i;
                grupa = s;
            }
        }
        Student s1 = new Student(1, new List<string>() { "prva", "druga" });
        Student s2 = new Student(2, new List<string>() { "prva" });
        Student s3 = new Student(3, new List<string>() { "treca", "cetvrta" });
        List<Student> studenti = new List<Student>();

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            studenti.Clear();
            studenti.Add(s1);
            studenti.Add(s2);
            studenti.Add(s3);
            button1.Text = "";
            string grupe = "";
            foreach (object o in checkedListBox1.CheckedItems)
            {
                button1.Text += " " + o;
                grupe += " " + o;
            }
            var upit = studenti.Where(s=>s.grupa.Any( g => grupe.Contains(g)));
            MessageBox.Show(checkedListBox1.SelectedItems + "");
            MessageBox.Show(upit.Count() + "");
            foreach (Student s in upit)
            {
                MessageBox.Show(s.id + "");
            }
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }
    }
}
