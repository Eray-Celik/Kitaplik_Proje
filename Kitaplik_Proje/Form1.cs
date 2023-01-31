using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace Kitaplik_Proje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=kitaplik.mdb");

        void listele()
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("Select * From Kitaplar", baglanti);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();  
            //liste(); 
            //priate void Form_load(object sender, EventArgs  e)
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            listele();
        }
        string durum = "";
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (TxtKitapAd.Text.Length<1 &&TxtKitapYazar.Text.Length<3)
            {
                MessageBox.Show("Boş alan bırakmayınız !!!");
                baglanti.Close();
                return;
            }
            OleDbCommand komut = new OleDbCommand("insert into Kitaplar (KitapAd,Yazar,Tur,Sayfa,Durum) values (@p1,@p2,@p3,@p4,@5)", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtKitapYazar.Text);
            komut.Parameters.AddWithValue("@p3", CmbTur.Text);
            komut.Parameters.AddWithValue("@p4", TxtKitapSayfa.Text);
            komut.Parameters.AddWithValue("@p5", durum);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kitap Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();


        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            durum = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            durum = "1";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtKitapid.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtKitapAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtKitapYazar.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            CmbTur.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtKitapSayfa.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            if (dataGridView1.Rows[secilen].Cells[5].Value.ToString() == "True")
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Delete From Kitaplar where Kitapid=@1", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtKitapid.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kitap Listeden Silindi","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            listele();

        }
        
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                if (TxtKitapid.Text == "")
                {
                    throw new Exception();
                }
                OleDbCommand komut = new OleDbCommand("update kitaplar set KitapAd=@p1,Yazar=@p2,Tur=@p3,Sayfa=@p4,Durum=@p5 where Kitapid=@p6", baglanti);
                komut.Parameters.AddWithValue("@p1", TxtKitapAd.Text);
                komut.Parameters.AddWithValue("@p2", TxtKitapYazar.Text);
                komut.Parameters.AddWithValue("@p3", CmbTur.Text);
                komut.Parameters.AddWithValue("@p4", TxtKitapSayfa.Text);
                if (radioButton1.Checked == true)
                {
                    komut.Parameters.AddWithValue("@p5", durum);
                }
                if (radioButton2.Checked == true)
                {
                    komut.Parameters.AddWithValue("@p5", durum);
                }
                komut.Parameters.AddWithValue("@p6", TxtKitapid.Text);
                komut.ExecuteNonQuery();
                MessageBox.Show("Kayıt Güncellendi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
            }
            catch (Exception)
            {
                MessageBox.Show("Güncelleme Hatası !!!");
            }
            finally {
                baglanti.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Select * From Kitaplar where kitapAd like'%"+TxtKitapBul.Text+"%' ", baglanti);
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();

            


        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKitapid.Clear();
            TxtKitapAd.Clear();
            TxtKitapBul.Clear();
            TxtKitapSayfa.Clear();
            TxtKitapYazar.Clear();
            CmbTur.Text = "";

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
