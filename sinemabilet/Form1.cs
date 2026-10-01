using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sinemabilet
{
    public partial class Form1 : Form
    {
        private readonly string dosyayolu = @"C:\Users\omera\source\repos\bilet.txt";
        string secilenKoltuk = "";
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            this.Resize += Form1_Resize;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (groupBox1 != null)
            {
                groupBox1.Left = (this.ClientSize.Width - groupBox1.Width) / 2;
                groupBox1.Top = (this.ClientSize.Height - groupBox1.Height) / 2 + 40;
            }
            if (label1 != null)
            {
                label1.Left = (this.ClientSize.Width - label1.Width) / 2;
                label1.Top = (groupBox1.Top - label1.Height - 30);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            comboBox1.Items.AddRange(items: new string[] { "G.O.R.A", "Issız Adam", "Eyvah Eyvah", "Joker","Dağ","Herşey Çok Güzel Olacak" });
            comboBox2.Items.AddRange(items: new string[] { "12:00", "15:00", "18:00", "21:00" });
            
            
            Form1_Resize(null, null);
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string film = comboBox1.SelectedItem?.ToString();

            if (film == "G.O.R.A") label7.Text = "SALON 1";
            else if (film == "Issız Adam") label7.Text = "SALON 2";
            else if (film == "Eyvah Eyvah") label7.Text = "SALON 3";
            else if (film == "Joker") label7.Text = "SALON 4";
            else if (film == "Dağ") label7.Text = "SALON 5";
            else if (film == "Herşey Çok Güzel Olacak") label7.Text = "SALON 6";
            



        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(secilenKoltuk) ||
                string.IsNullOrEmpty(textBox1.Text) ||
            comboBox1.SelectedItem == null ||
            comboBox2.SelectedItem == null || string.IsNullOrEmpty(textBox2.Text) ||
            string.IsNullOrEmpty(textBox3.Text) ||(!radioButton1.Checked && !radioButton2.Checked))
{
    MessageBox.Show("Lütfen tüm alanları (Ad, Film, Seans, Koltuk) doldurun!");
    return;
}
            string adSoyad = textBox1.Text;
            string tcNo = textBox2.Text;
            string bilettipi = radioButton1.Checked ? "Tam" : "Öğrenci";
            string ucret = radioButton1.Checked ? "199.99 TL" : "149.99 TL";
            string film = comboBox1.SelectedItem.ToString();
            string seans = comboBox2.SelectedItem.ToString();
            string salon = label7.Text;
            string tarih = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            string kayit = $"{secilenKoltuk};{textBox2.Text};{textBox1.Text};{comboBox1.SelectedItem};{label7.Text};{comboBox2.SelectedItem};{tarih};{bilettipi};{ucret}";

            try
            {
               File.AppendAllText(dosyayolu, kayit + Environment.NewLine);
                MessageBox.Show($"Ödeme Alındı: {ucret} TL\nBiletiniz Hazır!");

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                label8.Text = "Koltuk No: ";
                secilenKoltuk = "";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        
        
       

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen önce Film ve Seans seçiniz!");
                return;
            }
            string secilenFilm = comboBox1.SelectedItem.ToString();
            string secilenSeans = comboBox2.SelectedItem.ToString();
            string secilenTarih = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            using (Form3 frm0 = new Form3(secilenFilm, secilenSeans, secilenTarih))
            {
                if(frm0.ShowDialog() == DialogResult.OK)
                {
                    secilenKoltuk = frm0.secilenKoltuk;
                    label8.Text = "Koltuk No: " + secilenKoltuk;
                    MessageBox.Show($"Seçilen Koltuk: {secilenKoltuk}");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string arananTC = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(arananTC))
            {
                MessageBox.Show("Lütfen sorgulamak için bir TC Kimlik No girin!");
                return;
            }

            listBox1.Items.Clear();
            bool biletBulundu = false;

            if (File.Exists(dosyayolu))
            {
                string[] satirlar = File.ReadAllLines(dosyayolu);

                foreach (string satir in satirlar)
                {
                    if (string.IsNullOrWhiteSpace(satir)) continue;

                    string[] p = satir.Split(';');

                    if (p.Length >= 7)
                    {
                        string dosyaTC = p[1].Trim();

                        if (dosyaTC == arananTC)
                        {
                            biletBulundu = true;
                            string sonuc = $"FİLM: {p[3]} | TARİH: {p[6]} | SAAT: {p[5]} | KOLTUK: {p[0]} | SALON: {p[4]}";
                            listBox1.Items.Add(sonuc);
                        }
                    }
                }
            }

            if (!biletBulundu)
            {
                MessageBox.Show("Bu TC numarasına ait herhangi bir bilet kaydı bulunamadı.");
            }
            else
            {
                MessageBox.Show($"{listBox1.Items.Count} adet bilet bulundu.");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
