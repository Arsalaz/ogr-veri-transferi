using System.Data;
using System.Data.OleDb;
using System.Xml;
using ClassLibrary1;
namespace kafi2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection baglan;
        OleDbDataAdapter verial;
        DataSet al;
        public void gor()
        {
            string baglanti = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\veri.accdb";
            string sorgu = "SELECT * FROM ogrenci";
            baglan = new OleDbConnection(baglanti);
            verial = new OleDbDataAdapter(sorgu, baglan);
            al = new DataSet();
            verial.Fill(al, "o");
            dataGridView1.DataSource = al.Tables[0];
        }
        public void sýrala()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("isim");
            listView1.Columns.Add("vize");
            listView1.Columns.Add("final");
            listView1.Columns.Add("ortalama");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gor();
            sýrala();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double vize, final, ortalama;
            vize = Convert.ToDouble(textBox2.Text);
            final = Convert.ToDouble(textBox3.Text);
            ortalama = new Class1().ortbul(vize, final, 0);
            textBox4.Text = ortalama.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sorgu = "insert into ogrenci (isim, vize, final, ortalama) values (@isim, @vize, @final, @ortalama)";
            OleDbCommand komut = new OleDbCommand(sorgu,baglan);
            komut.Parameters.AddWithValue("isim", textBox1.Text);
            komut.Parameters.AddWithValue("vize", Convert.ToDouble(textBox2.Text));
            komut.Parameters.AddWithValue("final", Convert.ToDouble(textBox3.Text));
            komut.Parameters.AddWithValue("oratlama", Convert.ToDouble(textBox4.Text));
            baglan.Open();
            komut.ExecuteNonQuery();
            baglan.Close();
            gor();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string xmlyol = Application.StartupPath + "\\liste.xml";//xml yolu tanýmladýk
            XmlDocument doc = new XmlDocument(); //doc nesnesi oluþtueduk
            doc.Load(xmlyol); //doc nesnesini xml yolumuzu kullanarak açtýk
            XmlElement ogrenci = doc.CreateElement("ogrenci"); //öðrenci elementini oluþturduk

            XmlElement isim = doc.CreateElement("isim");//isim elementi oluþturduk
            isim.InnerText = textBox1.Text;//isim elementine innertext özelliðini kullanarak textbox1 den aldýðýmýz veriyi girdik
            ogrenci.AppendChild(isim);//isim deðerini kaydattik

            XmlElement vize = doc.CreateElement("vize");
            vize.InnerText = textBox2.Text;
            ogrenci.AppendChild(vize);

            XmlElement final = doc.CreateElement("final");
            final.InnerText = textBox3.Text;
            ogrenci.AppendChild(final);

            XmlElement ortalama = doc.CreateElement("ortalama");
            ortalama.InnerText = textBox4.Text;
            ogrenci.AppendChild(ortalama);

            doc.DocumentElement?.AppendChild(ogrenci);//ogrenci elementini kapattýk
            doc.Save(xmlyol);//verileri xml e importladýk
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string xmlyol = Application.StartupPath + ("\\liste.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlyol);

            for (int i = 0; i < dataGridView1.Columns.Count - 1; i++)
            {
                XmlElement ogrenci = doc.CreateElement("ogrenci");

                XmlElement isim = doc.CreateElement("isim");
                isim.InnerText = dataGridView1.Rows[i].Cells["isim"].Value.ToString();
                ogrenci.AppendChild(isim);


                XmlElement vize = doc.CreateElement("vize");
                vize.InnerText = dataGridView1.Rows[i].Cells["vize"].Value.ToString();
                ogrenci.AppendChild(vize);

                XmlElement final = doc.CreateElement("final");
                final.InnerText = dataGridView1.Rows[i].Cells["final"].Value.ToString();
                ogrenci.AppendChild(final);

                XmlElement ortalama = doc.CreateElement("ortalama");
                ortalama.InnerText = dataGridView1.Rows[i].Cells["ortalama"].Value.ToString();
                ogrenci.AppendChild(ortalama);

                doc.DocumentElement?.AppendChild(ogrenci);
                doc.Save(xmlyol);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string xmlyol = Application.StartupPath + ("\\liste.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlyol);
            listView1.Items.Clear();
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string isim = node.SelectSingleNode("isim").InnerText;
                string vize = node.SelectSingleNode("vize").InnerText;
                string final = node.SelectSingleNode("final").InnerText;
                string ortalama = node.SelectSingleNode("ortalama").InnerText;
                ListViewItem item = new ListViewItem(new string[] { isim, vize, final, ortalama });
                listView1.Items.Add(item);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string xmlyol = Application.StartupPath + ("\\liste.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlyol);
            DataTable dt = new DataTable();
            dt.Columns.Add("isim");
            dt.Columns.Add("vize");
            dt.Columns.Add("final");
            dt.Columns.Add("ortalama");
            foreach(XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string isim = node.SelectSingleNode("isim").InnerText;
                string vize = node.SelectSingleNode("vize").InnerText;
                string final = node.SelectSingleNode("final").InnerText;
                string ortalama = node.SelectSingleNode("ortalama").InnerText;
                dt.Rows.Add(isim, vize, final, ortalama);
            }
            dataGridView2.DataSource = dt;
        }
    }
}
