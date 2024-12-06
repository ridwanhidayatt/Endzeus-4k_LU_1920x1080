using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        string noRMDefault, namaDaerah;

        // Simpan path gambar saat ini
        private string currentImagePath, currentImagePath2;

        private string dirLogo = @"D:\GLEndoscope\LogoKOP\logo1.png";
        private string dirLogo2 = @"D:\GLEndoscope\LogoKOP\logo2.png";

        public Form1()
        {
            InitializeComponent();

            //LoadImage();
        }

        private void LoadImage()
        {
            // Use the default path that you specified
            currentImagePath = dirLogo;
            currentImagePath2 = dirLogo2;

            // Dispose of the existing image, if any
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }else if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
            }

            // Introduce a small delay before accessing the file
            System.Threading.Thread.Sleep(100);

            // Load the new image into PictureBox
            using (FileStream stream = new FileStream(currentImagePath, FileMode.Open, FileAccess.Read))
            {
                pictureBox1.Image = Image.FromStream(stream);
            }

            using (FileStream stream = new FileStream(currentImagePath2, FileMode.Open, FileAccess.Read))
            {
                pictureBox2.Image = Image.FromStream(stream);
            }

            string dirr = @"D:\GLEndoscope\FileRTF\";
            if (Directory.Exists(dirr))
            {
                richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
                richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
                richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
                richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
                richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);
                richTextBox6.LoadFile(dirr + "RtfFile5.rtf", RichTextBoxStreamType.RichText);
            }
            else
            {


            }


        }


        private void button5_Click(object sender, EventArgs e)
        {
            string dirr = @"D:\GLEndoscope\FileRTF\";

            string fileName = dirr + "RtfFile.rtf";
            richTextBox1.SaveFile(fileName, RichTextBoxStreamType.RichText);
            richTextBox1.Clear();


            string fileName1 = dirr + "RtfFile1.rtf";
            richTextBox2.SaveFile(fileName1, RichTextBoxStreamType.RichText);
            richTextBox2.Clear();


            string fileName2 = dirr + "RtfFile2.rtf";
            richTextBox3.SaveFile(fileName2, RichTextBoxStreamType.RichText);
            richTextBox3.Clear();


            string fileName3 = dirr + "RtfFile3.rtf";
            richTextBox4.SaveFile(fileName3, RichTextBoxStreamType.RichText);
            richTextBox4.Clear();

            string fileName4 = dirr + "RtfFile4.rtf";
            richTextBox5.SaveFile(fileName4, RichTextBoxStreamType.RichText);
            richTextBox5.Clear();

            string fileName5 = dirr + "RtfFile5.rtf";
            richTextBox6.SaveFile(fileName5, RichTextBoxStreamType.RichText);
            richTextBox6.Clear();



            richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
            richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
            richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
            richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
            richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);
            richTextBox6.LoadFile(dirr + "RtfFile5.rtf", RichTextBoxStreamType.RichText);


            //string dir = @"D:\GLEndoscope\LogoKOP";
            //string fileNameLogo = "logo1.png";
            //string filePath = Path.Combine(dir, fileNameLogo);

            //// Ensure the directory exists
            //if (!Directory.Exists(dir))
            //{
            //    Directory.CreateDirectory(dir);
            //}

            //try
            //{
            //    using (Image img = pictureBox1.Image.Clone() as Image)
            //    {
            //        if (img != null)
            //        {
            //            // Convert the image to PNG format before saving
            //            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            //            {
            //                img.Save(fs, ImageFormat.Png);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions
            //    MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    // Ensure resources are released
            //    pictureBox1.Image.Dispose();
            //} 

            string dir = @"D:\GLEndoscope\LogoKOP";
            string fileNameLogo1 = "logo1.png";
            string fileNameLogo2 = "logo2.png";

            // Ensure the directory exists
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                if (comboBox9.SelectedItem != null)
                {
                    int selectedValue = Convert.ToInt32(comboBox9.SelectedItem);

                    if (selectedValue == 1)
                    {
                        // Save pictureBox1 and logo1
                        SaveImage(pictureBox1.Image, Path.Combine(dir, fileNameLogo1));
                    }
                    else if (selectedValue == 2)
                    {
                        // Save pictureBox1, pictureBox2, logo1, and logo2
                        SaveImage(pictureBox1.Image, Path.Combine(dir, fileNameLogo1));
                        SaveImage(pictureBox2.Image, Path.Combine(dir, fileNameLogo2));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure resources are released
                pictureBox1.Image?.Dispose();
                pictureBox2.Image?.Dispose();
            }

            var xd = new XDocument( new XElement("userdata",new XElement("logo", comboBox9.Text),new XElement("jenis", comboBox1.Text)));

            xd.Save(@"D:\GLEndoscope\LogoKOP\logo.xml"); 

            richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
            richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
            richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
            richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
            richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);
            richTextBox6.LoadFile(dirr + "RtfFile5.rtf", RichTextBoxStreamType.RichText);

            MessageBox.Show("Data telah tersimpan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void SaveImage(Image image, string filePath)
        {
            try
            {
                using (Image img = image.Clone() as Image)
                {
                    if (img != null)
                    {
                        // Convert the image to PNG format before saving
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            img.Save(fs, ImageFormat.Png);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure resources are released
                image?.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dir = @"D:\GLEndoscope\LogoKOP";
            string fileNameLogo = "logo1.png";
            string filePath = Path.Combine(dir, fileNameLogo);

            // Ensure the directory exists
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                using (Image img = pictureBox1.Image.Clone() as Image)
                {
                    if (img != null)
                    {
                        // Convert the image to PNG format before saving
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            img.Save(fs, ImageFormat.Png);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure resources are released
                pictureBox1.Image.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadImage();
            button2.Enabled = false;
            button5.Enabled = true;
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Menyembunyikan semua PictureBox terlebih dahulu
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            button4.Visible = false;
            button3.Visible = false;

            // Memeriksa pilihan ComboBox dan menampilkan PictureBox yang sesuai
            switch (comboBox9.SelectedIndex)
            {
                case 0:
                    pictureBox1.Visible = true;
                    button4.Visible = true;
                    break;
                case 1:
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = true;
                    button4.Visible = true;
                    button3.Visible = true;
                    break; 
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Memeriksa pilihan ComboBox1 dan mengubah ukuran PictureBox sesuai
            if (comboBox1.SelectedIndex == 0) // Jika memilih persegi
            {
                pictureBox1.Size = new Size(147, 147);
                pictureBox2.Size = new Size(147, 147);
                pictureBox2.Location = new Point(1279, 12);

                richTextBox1.Size = new Size(1115, 41); 
                richTextBox1.Location = new Point(158, 12);

                richTextBox2.Size = new Size(1115, 41);
                richTextBox2.Location = new Point(158, 56);

                richTextBox3.Size = new Size(1115, 28);
                richTextBox3.Location = new Point(158, 100);

                richTextBox4.Size = new Size(1115, 28);
                richTextBox4.Location = new Point(158, 131);

            }
            else if (comboBox1.SelectedIndex == 1) // Jika memilih persegi panjang
            {
                pictureBox1.Size = new Size(290,147);
                richTextBox1.Size = new Size(833, 41);
                richTextBox1.Location = new Point(300, 12);
                richTextBox2.Size = new Size(833, 41);
                richTextBox2.Location = new Point(300, 56);
                richTextBox3.Size = new Size(833, 28);
                richTextBox3.Location = new Point(300, 100);
                richTextBox4.Size = new Size(833, 28);
                richTextBox4.Location = new Point(300, 131);
                pictureBox2.Size = new Size(294, 147);
                pictureBox2.Location = new Point(1138, 12);
            }
            // Tambahkan logika tambahan jika ada lebih banyak pilihan
            // ...
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Buka OpenFileDialog untuk memilih gambar baru
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = "C:\\";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Simpan path gambar yang baru dipilih
                    currentImagePath = openFileDialog.FileName;

                    // Ganti gambar di PictureBox
                    pictureBox2.Image = Image.FromFile(currentImagePath);
                }
            }
        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            // Buka OpenFileDialog untuk memilih gambar baru
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = "C:\\";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Simpan path gambar yang baru dipilih
                    currentImagePath = openFileDialog.FileName;

                    // Ganti gambar di PictureBox
                    pictureBox1.Image = Image.FromFile(currentImagePath);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label17;
            comboBox1.SelectedIndex = 0;
            //string dirLogo = @"D:\GLEndoscope\LogoKOP\";

            //string logoPath = Path.Combine(dirLogo, "logo1.png");

            //if (File.Exists(logoPath))
            //{
            //    try
            //    {
            //        pictureBox1.Image = Image.FromFile(logoPath);
            //    }
            //    catch (Exception ex)
            //    {
            //        // Tangani exception yang mungkin terjadi saat memuat gambar
            //        MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("File gambar tidak ditemukan.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}



            //string dirr = @"D:\GLEndoscope\FileRTF\";
            //if (Directory.Exists(dirr))
            //{
            //    richTextBox1.LoadFile(dirr + "RtfFile.rtf", RichTextBoxStreamType.RichText);
            //    richTextBox2.LoadFile(dirr + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
            //    richTextBox3.LoadFile(dirr + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
            //    richTextBox4.LoadFile(dirr + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
            //    richTextBox5.LoadFile(dirr + "RtfFile4.rtf", RichTextBoxStreamType.RichText);

            //}
            //else
            //{


            //}

            richTextBox1.BackColor = Color.White;
            richTextBox2.BackColor = Color.White;
            richTextBox3.BackColor = Color.White;
            richTextBox4.BackColor = Color.White;

            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox4.SelectionAlignment = HorizontalAlignment.Center; 

                //noRMDefault = KoneksiDatabase.mdr.GetString("no_rm");
                //Name = KoneksiDatabase.mdr.GetString("nama"); 
                //string tglKunjungan;
                //tglKunjungan = KoneksiDatabase.mdr.GetString("Tanggal Kunjungan");
                //namaDaerah = richTextBox5.Text; 




            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode, xmlnode1;
            int i = 0;
            FileStream fs = new FileStream(@"D:\GLEndoscope\LogoKOP\logo.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("logo");
            xmlnode1 = xmldoc.GetElementsByTagName("jenis");
            String dataXML = xmlnode[0].ChildNodes.Item(0).InnerText.Trim();
            String dataXML1 = xmlnode1[0].ChildNodes.Item(0).InnerText.Trim();



            comboBox9.Text = dataXML;
            comboBox1.Text = dataXML1;


            //FileStream fs = new FileStream(NameTb.Text + ".txt");
            //File.Create(fs);
            fs.Close();


            button5.Enabled = false;
        }


    }
}
