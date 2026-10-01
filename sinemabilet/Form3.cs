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
    public partial class Form3 : Form
    {
        public string secilenKoltuk = "";
        public string gelenFilm = "", gelenSeans = "",gelenTarih = "";
        string dosyaYolu = @"C:\Users\omera\source\repos\bilet.txt";
        public Form3(string film, string seans, string tarih)
        {
            InitializeComponent();
            this.gelenFilm = film;
            this.gelenSeans = seans;
            this.gelenTarih = tarih;
            this.Load += Form3_Load; 
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = $"{gelenFilm} - {gelenSeans} - {gelenTarih} Koltuk Seçimi";
            this.StartPosition = FormStartPosition.CenterScreen;
            
            KoltuklariOlustur();
        }

        private void KoltuklariOlustur()
        {
            List<string> doluKoltuklar = new List<string>();

            if (File.Exists(dosyaYolu))
            {
                string[] satirlar = File.ReadAllLines(dosyaYolu);
                foreach (string satir in satirlar)
                {
                    if (string.IsNullOrWhiteSpace(satir)) continue;
                    string[] p = satir.Split(';');

                    if (p.Length >= 7)
                    {
                        string dKoltuk = p[0].Trim();
                        string dFilm = p[3].Trim();
                        string dSalon = p[4].Trim();
                        string dSeans = p[5].Trim();
                        string dTarihMetin = p[6].Trim();

                        DateTime dosyaTarihi, seciliTarih;
                        bool t1 = DateTime.TryParse(dTarihMetin, out dosyaTarihi);
                        bool t2 = DateTime.TryParse(gelenTarih, out seciliTarih);

                        if (t1 && t2 && dosyaTarihi.Date == seciliTarih.Date)
                        {
                            if (dosyaTarihi.Date == seciliTarih.Date &&
                                dFilm.Equals(gelenFilm.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                dSeans.Equals(gelenSeans.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                doluKoltuklar.Add(dKoltuk);
                            }
                        }
                    }
                }
            }

            int cols = 6;
            int rows = 5;
            int hGap = 20; 
            int vGap = 20;
            int btnWidth = 120;
            int btnHeight = 80;

            int gridWidth = (cols * btnWidth) + ((cols - 1) * hGap);
            int gridHeight = (rows * btnHeight) + ((rows - 1) * vGap);

            int startX = (this.ClientSize.Width - gridWidth) / 2;
            int startY = (this.ClientSize.Height - gridHeight) / 2;

            int sayac = 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Button btn = new Button();
                    btn.Text = sayac.ToString();
                    btn.Size = new Size(btnWidth, btnHeight);
                    
                   
                    int xPos = startX + (j * (btnWidth + hGap));
                    int yPos = startY + (i * (btnHeight + vGap));
                    
                    btn.Location = new Point(xPos, yPos);
                    
                
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

                    if (doluKoltuklar.Contains(btn.Text))
                    {
                        btn.BackColor = Color.Crimson;
                        btn.ForeColor = Color.White;
                        btn.Enabled = false; 
                    }
                    else
                    {
                        btn.BackColor = Color.MediumSeaGreen;
                        btn.ForeColor = Color.White;
                        
                        btn.MouseEnter += (s, ev) => { btn.BackColor = Color.SeaGreen; };
                        btn.MouseLeave += (s, ev) => { btn.BackColor = Color.MediumSeaGreen; };

                        btn.Click += (s, ev) =>
                        {
                            secilenKoltuk = btn.Text;
                            this.DialogResult = DialogResult.OK;
                        };
                    }
                    this.Controls.Add(btn);
                    sayac++;
                }
            }
        }
    }
}
