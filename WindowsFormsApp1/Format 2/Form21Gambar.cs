﻿using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CsvHelper;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using PictureBox = System.Windows.Forms.PictureBox;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;

namespace WindowsFormsApp1.Format_2
{
    public partial class Form21Gambar : Form
    {
        string splitTahun, splitBulan, tanggal,  noRM, id, jam, gabung1, gabung,tanggalDatabase, dataHasilPemeriksaan, code,getIdPasien, getIdDokter, ambilDaerah, action, Date, tanggalLahir, umur, alamat, dokterNama;
        string logoValue, jenisValue;

        string dirRtf = @"D:\ZeusEndoscope\FileRTF\";
        string dirLogo = @"D:\ZeusEndoscope\LogoKOP\";
        string csvFilePath = "D:\\ZeusEndoscope\\Database\\dataPasien\\dataDefault.csv";

        //string dir = @"D:\";

        public delegate void TransfDelegate(String value);
        public event TransfDelegate TEFormat2; 
        public event TransfDelegate TEViewC2Gambar;
        public event TransfDelegate TEViewC4Gambar;
        public event TransfDelegate TEViewC6Gambar;
        public event TransfDelegate TEViewC21G;
        int result;

        private Dictionary<PictureBox, PictureBoxControls> pictureBoxControls = new Dictionary<PictureBox, PictureBoxControls>();

        public Form21Gambar()
        {
            InitializeComponent();
            InitializePrinterComboBox();
            FillListBox();
            /*PopulatePrinterComboBox*//*();*/ // Call to populate printers
            comboBox1.SelectedIndex = -1; // Ensure no printer is selected by default
            InitializeThumbnails();
            InitializeThumbnailsForToday();
            InitializeMainPictureBoxes();
            InitializeComboBox();
            InitializeComboBoxNow();

            comboBox1.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
            comboBox2.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
        }


        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; // Mencegah karakter yang diketik ditampilkan di ComboBox
        }

        private void InitializePrinterComboBox()
        {
            try
            {
                comboBox1.Items.AddRange(PrinterSettings.InstalledPrinters.Cast<string>().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan saat memuat daftar printer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public class PictureBoxControls
        {
            public Control CloseControl { get; set; }
            public Control AddControl { get; set; }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }

        public class ComboBoxItem
        {

            public string FolderPath { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                // Tampilkan hanya DisplayText
                return DisplayText;
            }
        }

        private void cbx_baru_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cbx_baru.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void InitializeComboBox()
        {
            // Menambahkan placeholder item
            cbx_baru.Items.Add("-PILIH TANGGAL-");
            //MessageBox.Show("Placeholder added: " + cbx_baru.Items.Count);  // Debugging

            // Menyimpan index placeholder
            int placeholderIndex = cbx_baru.Items.Count - 1;

            // Set dropdown style untuk mencegah pengeditan
            cbx_baru.DropDownStyle = ComboBoxStyle.DropDownList;

            // Event handler untuk memastikan placeholder hilang ketika item dipilih
            cbx_baru.SelectedIndexChanged += (s, e) =>
            {
                // Jika placeholder dipilih, reset kembali ke placeholder
                if (cbx_baru.SelectedIndex == placeholderIndex)
                {
                    // Pilih item placeholder kembali
                    cbx_baru.SelectedIndex = placeholderIndex;
                }
            };

            // Set placeholder lagi jika pengguna tidak memilih item valid
            this.Load += (s, e) => EnsurePlaceholder(placeholderIndex);
        }

        // Metode terpisah untuk memastikan placeholder
        private void EnsurePlaceholder(int placeholderIndex)
        {
            if (cbx_baru.SelectedIndex == -1 || cbx_baru.SelectedIndex == placeholderIndex)
            {
                cbx_baru.SelectedIndex = placeholderIndex;
            }
        }

        private void InitializeComboBoxNow()
        {
            cbx_now.DropDownStyle = ComboBoxStyle.DropDownList;

            // Mengurutkan item berdasarkan tanggal terbaru
            if (cbx_now.Items.Count > 1)
            {
                var sortedItems = cbx_now.Items.Cast<ComboBoxItem>()
                    .OrderByDescending(item =>
                    {
                        DateTime parsedDate;
                        // Menggunakan kultur bahasa Indonesia agar sesuai dengan format bulan (misalnya "September")
                        var culture = new CultureInfo("id-ID");

                        // Coba parsing tanggal
                        if (DateTime.TryParse(item.DisplayText, culture, DateTimeStyles.None, out parsedDate))
                        {
                            return parsedDate;
                        }
                        else
                        {
                            // Jika gagal diparsing, kembalikan DateTime.MinValue agar item tersebut berada di urutan paling bawah
                            return DateTime.MinValue;
                        }
                    })
                    .ToList();

                // Bersihkan item ComboBox dan tambahkan kembali item yang sudah diurutkan
                cbx_now.Items.Clear();

                foreach (var item in sortedItems)
                {
                    cbx_now.Items.Add(item);
                }

                // Pilih item terbaru (item pertama setelah diurutkan)
                cbx_now.SelectedIndex = 0;
            }

            // Placeholder handling
            int placeholderIndex = cbx_now.Items.Count - 1;

            cbx_now.SelectedIndexChanged += (s, e) =>
            {
                // Jika placeholder dipilih, biarkan pengguna memilih item lain
                if (cbx_now.SelectedIndex == placeholderIndex)
                {
                    // Tidak melakukan apa-apa, biarkan pengguna memilih
                }
            };

            // Memastikan placeholder dipilih jika tidak ada item valid
            this.Load += (s, e) =>
            {
                if (cbx_now.SelectedIndex == -1 || cbx_now.SelectedIndex == placeholderIndex)
                {
                    cbx_now.SelectedIndex = placeholderIndex; // Pilih placeholder jika tidak ada item valid
                }
            };
        }
        private void EnsurePlaceholderNow(int placeholderIndex)
        {
            // Pastikan indeks yang dipilih valid atau kembali ke placeholder
            if (cbx_now.SelectedIndex == -1 || cbx_now.SelectedIndex == placeholderIndex)
            {
                cbx_now.SelectedIndex = placeholderIndex; // Pilih placeholder
            }
        }



        private void InitializeThumbnails()
        {
            // Call the method to read data from the CSV file
            ReadDataFromCSV(csvFilePath);

            tanggal = DateTime.Now.ToString("ddMMyyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];

            // Gabungkan string menjadi bagian dari folder path yang akan dicari
            string searchPattern = $@"{gabung}\Image";

            // Root folder
            string rootPath = @"D:\ZeusEndoscope";

            // Bersihkan ComboBox dan FlowLayoutPanel sebelum memulai
            cbx_baru.Items.Clear();
            flowLayoutPanel1.Controls.Clear();

            // Format untuk mendapatkan nama bulan
            var culture = new System.Globalization.CultureInfo("id-ID");

            // Loop untuk mencari semua subfolder di rootPath
            foreach (string yearFolder in Directory.GetDirectories(rootPath))
            {
                foreach (string monthFolder in Directory.GetDirectories(yearFolder))
                {
                    foreach (string dayFolder in Directory.GetDirectories(monthFolder))
                    {
                        foreach (string patientFolder in Directory.GetDirectories(dayFolder))
                        {
                            // Gabungkan folder dengan folder Image
                            string folderPath = Path.Combine(patientFolder, "Image");

                            // Cek apakah folder saat ini adalah folder yang sesuai
                            if (patientFolder.EndsWith(gabung) && Directory.Exists(folderPath))
                            {
                                // Pastikan folder tersebut memiliki file gambar
                                string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                                    .Where(file => file.ToLower().EndsWith(".jpg") ||
                                                   file.ToLower().EndsWith(".jpeg") ||
                                                   file.ToLower().EndsWith(".png") ||
                                                   file.ToLower().EndsWith(".bmp") ||
                                                   file.ToLower().EndsWith(".gif"))
                                    .ToArray();

                                if (imageFiles.Length > 0)
                                {

                                    string day = Path.GetFileName(dayFolder);

                                    // Memisahkan hari, bulan, dan tahun
                                    string dayPart = day.Substring(0, 2);
                                    string monthPart = day.Substring(2, 2);
                                    string yearPart = day.Substring(4, 4);

                                    // Deklarasikan variabel untuk hasil parsing bulan
                                    int monthNumber;

                                    // Coba konversi bulan menjadi nomor bulan dan ambil nama bulannya
                                    if (int.TryParse(monthPart, out monthNumber) && monthNumber >= 1 && monthNumber <= 12)
                                    {
                                        // Mendapatkan nama bulan berdasarkan culture
                                        monthPart = culture.DateTimeFormat.GetMonthName(monthNumber);
                                    }

                                    // Format tanggal sesuai dengan format yang diinginkan: "dd-MM-yyyy"
                                    string formattedDate = $"{dayPart} - {monthPart} - {yearPart}";

                                    // Tambahkan formattedDate ke ComboBox
                                    cbx_baru.Items.Add(new ComboBoxItem
                                    {
                                        FolderPath = folderPath,
                                        DisplayText = formattedDate
                                    });
                                }
                            }
                        }
                    }
                }
            }

            // Event handler ketika pilihan pada ComboBox berubah
            cbx_baru.SelectedIndexChanged += (s, e) =>
            {
                // Hapus semua thumbnail yang ada di FlowLayoutPanel
                flowLayoutPanel1.Controls.Clear();

                // Dapatkan folder path dari pilihan yang dipilih
                ComboBoxItem selectedItem = cbx_baru.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string selectedFolder = selectedItem.FolderPath;

                    // Ambil semua file gambar dari folder yang dipilih
                    string[] imageFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(file => file.ToLower().EndsWith(".jpg") ||
                                       file.ToLower().EndsWith(".jpeg") ||
                                       file.ToLower().EndsWith(".png") ||
                                       file.ToLower().EndsWith(".bmp") ||
                                       file.ToLower().EndsWith(".gif"))
                        .ToArray();

                    // Tampilkan gambar sebagai thumbnail
                    foreach (string file in imageFiles)
                    {
                        try
                        {
                            Image image = Image.FromFile(file);
                            PictureBox thumbnail = new PictureBox
                            {
                                Image = ResizeImage(image, 271, 134),
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Size = new Size(232, 134),
                                Margin = new Padding(5),
                                Tag = file
                            };

                            // Tambahkan event handler untuk menangani klik pada thumbnail
                            thumbnail.MouseDown += Thumbnail_MouseDown;
                            flowLayoutPanel1.Controls.Add(thumbnail);  // Tambahkan thumbnail ke FlowLayoutPanel
                        }
                        catch (Exception ex)
                        {
                            // Jika ada kesalahan saat memuat gambar, tampilkan pesan error
                            MessageBox.Show($"Error loading image {file}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            // Jika tidak ada folder yang cocok
            if (cbx_baru.Items.Count == 0)
            {
                //MessageBox.Show("Tidak ditemukan folder yang sesuai dengan gabungan NORM dan Nama.", "Folder Tidak Ditemukan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void InitializeThumbnailsForToday()
        {
            // Call the method to read data from the CSV file
            ReadDataFromCSV(csvFilePath);

            tanggal = DateTime.Now.ToString("ddMMyyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];

            // Gabungkan string menjadi bagian dari folder path yang akan dicari
            string searchPattern = $@"{gabung}\Image";

            // Root folder
            string rootPath = @"D:\ZeusEndoscope";

            // Bersihkan ComboBox dan FlowLayoutPanel sebelum memulai
            cbx_now.Items.Clear();
            flowLayoutPanel2.Controls.Clear();

            // Format untuk mendapatkan nama bulan
            var culture = new System.Globalization.CultureInfo("id-ID");

            // Loop untuk mencari semua subfolder di rootPath
            foreach (string yearFolder in Directory.GetDirectories(rootPath))
            {
                foreach (string monthFolder in Directory.GetDirectories(yearFolder))
                {
                    foreach (string dayFolder in Directory.GetDirectories(monthFolder))
                    {
                        foreach (string patientFolder in Directory.GetDirectories(dayFolder))
                        {
                            // Gabungkan folder dengan folder Image
                            string folderPath = Path.Combine(patientFolder, "Image");

                            // Cek apakah folder saat ini adalah folder yang sesuai
                            if (patientFolder.EndsWith(gabung) && Directory.Exists(folderPath))
                            {
                                // Pastikan folder tersebut memiliki file gambar
                                string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                                    .Where(file => file.ToLower().EndsWith(".jpg") ||
                                                   file.ToLower().EndsWith(".jpeg") ||
                                                   file.ToLower().EndsWith(".png") ||
                                                   file.ToLower().EndsWith(".bmp") ||
                                                   file.ToLower().EndsWith(".gif"))
                                    .ToArray();

                                if (imageFiles.Length > 0)
                                {
                                    // Misal 'day' memiliki format seperti '30092024'
                                    string day = Path.GetFileName(dayFolder);

                                    // Memisahkan hari, bulan, dan tahun
                                    string dayPart = day.Substring(0, 2);  // '30'
                                    string monthPart = day.Substring(2, 2); // '09'
                                    string yearPart = day.Substring(4, 4);  // '2024'

                                    // Deklarasikan variabel untuk hasil parsing bulan
                                    int monthNumber;

                                    // Coba konversi bulan menjadi nomor bulan dan ambil nama bulannya
                                    if (int.TryParse(monthPart, out monthNumber) && monthNumber >= 1 && monthNumber <= 12)
                                    {
                                        // Mendapatkan nama bulan berdasarkan culture
                                        monthPart = culture.DateTimeFormat.GetMonthName(monthNumber);
                                    }

                                    // Format tanggal sesuai dengan format yang diinginkan: "dd-MM-yyyy"
                                    string formattedDate = $"{dayPart} - {monthPart} - {yearPart}";

                                    // Tambahkan formattedDate ke ComboBox
                                    cbx_now.Items.Add(new ComboBoxItem
                                    {
                                        FolderPath = folderPath,
                                        DisplayText = formattedDate
                                    });
                                }


                            }
                        }
                    }
                }
            }

            // Event handler ketika pilihan pada ComboBox berubah
            cbx_now.SelectedIndexChanged += (s, e) =>
            {
                // Hapus semua thumbnail yang ada di FlowLayoutPanel
                flowLayoutPanel2.Controls.Clear();

                // Dapatkan folder path dari pilihan yang dipilih
                ComboBoxItem selectedItem = cbx_now.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string selectedFolder = selectedItem.FolderPath;

                    // Ambil semua file gambar dari folder yang dipilih
                    string[] imageFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(file => file.ToLower().EndsWith(".jpg") ||
                                       file.ToLower().EndsWith(".jpeg") ||
                                       file.ToLower().EndsWith(".png") ||
                                       file.ToLower().EndsWith(".bmp") ||
                                       file.ToLower().EndsWith(".gif"))
                        .ToArray();

                    // Tampilkan gambar sebagai thumbnail
                    foreach (string file in imageFiles)
                    {
                        try
                        {
                            Image image = Image.FromFile(file);
                            PictureBox thumbnail = new PictureBox
                            {
                                Image = ResizeImage(image, 271, 134),
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Size = new Size(232, 134),
                                Margin = new Padding(5),
                                Tag = file
                            };

                            // Tambahkan event handler untuk menangani klik pada thumbnail
                            thumbnail.MouseDown += Thumbnail_MouseDown;
                            flowLayoutPanel2.Controls.Add(thumbnail);  // Tambahkan thumbnail ke FlowLayoutPanel
                        }
                        catch (Exception ex)
                        {
                            // Jika ada kesalahan saat memuat gambar, tampilkan pesan error
                            MessageBox.Show($"Error loading image {file}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            // Jika tidak ada folder yang cocok
            if (cbx_now.Items.Count == 0)
            {
                //MessageBox.Show("Tidak ditemukan folder yang sesuai dengan gabungan NORM dan Nama.", "Folder Tidak Ditemukan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //kode paling terakhir setalah kode yg di atas
        //private void InitializeThumbnails()
        //{
        //    // Call the method to read data from the CSV file
        //    ReadDataFromCSV(csvFilePath);

        //    // Ambil tanggal saat ini
        //    tanggal = DateTime.Now.ToString("ddMMyyyy");
        //    string text = DateTime.Now.ToString("Y");
        //    string[] arr = text.Split(' ');
        //    splitBulan = arr[0];
        //    splitTahun = arr[1];

        //    // Gabungkan string menjadi path lengkap
        //    string folderPath = $@"D:\ZeusEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Image";

        //    if (Directory.Exists(folderPath))
        //    {
        //        // Dapatkan semua file gambar dari folder
        //        string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
        //                                     .Where(file => file.ToLower().EndsWith(".jpg") ||
        //                                                    file.ToLower().EndsWith(".jpeg") ||
        //                                                    file.ToLower().EndsWith(".png") ||
        //                                                    file.ToLower().EndsWith(".bmp") ||
        //                                                    file.ToLower().EndsWith(".gif"))
        //                                     .ToArray();

        //        // Cek jika tidak ada file gambar di dalam direktori
        //        if (imageFiles.Length == 0)
        //        {
        //            MessageBox.Show("Tidak ada file gambar di dalam folder.", "Folder Kosong", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            foreach (string file in imageFiles)
        //            {
        //                try
        //                {
        //                    Image image = Image.FromFile(file);
        //                    PictureBox thumbnail = new PictureBox
        //                    {
        //                        Image = ResizeImage(image, 271, 134),
        //                        SizeMode = PictureBoxSizeMode.StretchImage,
        //                        Size = new Size(232, 134),
        //                        Margin = new Padding(5),
        //                        Tag = file
        //                    };

        //                    // Tambahkan event handler untuk mengubah visibilitas


        //                    thumbnail.MouseDown += Thumbnail_MouseDown;
        //                    flowLayoutPanel1.Controls.Add(thumbnail);
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show($"Error loading image {file}: {ex.Message}");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Tidak ada file gambar di dalam folder.", "Folder Kosong", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }

        //}


        private void Thumbnail_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox thumbnail = sender as PictureBox;
            if (thumbnail != null)
            {
                thumbnail.DoDragDrop(thumbnail.Tag, DragDropEffects.Copy);
            }
        }



        private void InitializeMainPictureBoxes()
        {
            // Daftar semua PictureBox yang akan digunakan
            PictureBox[] pictureBoxes = { pictureBox1 };

            // Inisialisasi kontrol untuk setiap PictureBox
            for (int i = 0; i < pictureBoxes.Length; i++)
            {
                PictureBox pictureBox = pictureBoxes[i];

                // Temukan kontrol close dan add dengan nama yang sesuai
                var closeControl = this.Controls.Find($"close{i + 1}", true).FirstOrDefault();
                var addControl = this.Controls.Find($"add{i + 1}", true).FirstOrDefault();

                pictureBoxControls[pictureBox] = new PictureBoxControls
                {
                    CloseControl = closeControl,
                    AddControl = addControl
                };

                pictureBox.AllowDrop = true;
                pictureBox.DragEnter += PictureBox_DragEnter;
                pictureBox.DragDrop += PictureBox_DragDrop;
            }
        }


        private void PictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string filePath = e.Data.GetData(DataFormats.StringFormat) as string;
                    if (filePath != null && File.Exists(filePath))
                    {
                        PictureBoxControls controls;
                        if (pictureBoxControls.TryGetValue(pictureBox, out controls))
                        {
                            if (controls.CloseControl != null) controls.CloseControl.Visible = true;
                            if (controls.AddControl != null) controls.AddControl.Visible = false;

                            btn_Delete.Visible = true;
                            pictureBox.Image = Image.FromFile(filePath);
                        }
                    }
                }
            }
        }


        void FillListBox()
        {
            // Clear existing items
            comboBox1.Items.Clear();

            // Loop through all installed printers
            foreach (var p in PrinterSettings.InstalledPrinters)
            {
                // Convert the printer name to a string
                string printerName = p.ToString();

                // Check if the printer is not the one you want to exclude
                if (printerName != "Canon SELPHY CP1300")
                {
                    // Add the printer to the ComboBox
                    comboBox1.Items.Add(printerName);
                }
            }
        }

        //public Form21Gambar()
        //{
        //    InitializeComponent(); 
        //    // Inisialisasi ComboBox dengan daftar printer yang tersedia
        //    foreach (string printer in PrinterSettings.InstalledPrinters)
        //    {
        //        comboBox1.Items.Add(printer);
        //    } 
        //} 

        private void button6_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Foto diisi Dahulu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxKlinis.Text))
            {
                MessageBox.Show("Klinis Belum diisi", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("Hasil Pemeriksaan Belum diisi", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Pilih printer terlebih dahulu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Pilih profil terlebih dahulu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedPrinter = comboBox1.SelectedItem.ToString();
            PrintDocument pd = new PrintDocument
            {
                PrinterSettings = { PrinterName = selectedPrinter },
                DefaultPageSettings =
        {
            PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180),
            Landscape = false
        }
            };

            if (comboBox2.Text == "Default")
            {
                pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            }
            else if (comboBox2.Text == "Adjust Brightness")
            {
                pd.PrintPage += new PrintPageEventHandler(printDocument2_PrintPage);
            }

            pd.Print();

            ResetFormState();
            MessageBox.Show("Dokumen berhasil diprint.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResetFormState()
        {
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            FillListBox();
            panel7.Size = new Size(0, 0);
            HistoryPrintA4(comboBox2.Text);
            clearTextboxPemeriksaan();
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            buttobDeleteFalse();
            buttonCancel.PerformClick();
            TEViewC21G("4");
        }



        private void buttobDeleteFalse()
        {
            btn_Delete.Visible = false; 
        }

        public static class printer
        {
            [DllImport("winspool.drv",
              CharSet = CharSet.Auto,
              SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean SetDefaultPrinter(String name);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Periksa apakah ada item yang dipilih
            if (comboBox1.SelectedItem != null)
            {
                string Pname = comboBox1.SelectedItem.ToString();

                // Set default printer
                printer.SetDefaultPrinter(Pname);
            }
        }
        
        private void HistoryPrintA4(string profile)
        {
            string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\History Print\Format-2" + @"\1-Gambar\";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }            

            string existingPathName = dir;
            string notExistingFileName;

            if (profile == "Default")
            {
                notExistingFileName = dir + gabung1 + ".pdf";
            }
            else if (profile == "Adjust Brightness")
            {
                notExistingFileName = dir + gabung1 + "_Adjust_Brightness.pdf";
            }
            else
            {
                // Handle the case where the profile is not recognized
                MessageBox.Show("Profile tidak dikenali", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Directory.Exists(existingPathName) && !File.Exists(notExistingFileName))
            {
                PrintDocument pdoc = new PrintDocument();
                pdoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                pdoc.PrinterSettings.PrintFileName = notExistingFileName;
                pdoc.PrinterSettings.PrintToFile = true;
                pdoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
                pdoc.DefaultPageSettings.Landscape = false; 
                pdoc.PrintPage += printDocument12_PrintPage;
                pdoc.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(picLogo1.Image, 10, 5, 100, 100);

            if (logoValue == "1")
            {
                //AdjustPictureBoxSize(e.Graphics, "Persegi");
                //e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);

                if (jenisValue == "Persegi Panjang")
                {
                    // Handle case when logoValue is "1" and jenisValue is "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, jenisValue);
                    e.Graphics.DrawImage(picLogo2.Image, 30, 3, picLogo2.Width, picLogo2.Height);

                    //MessageBox.Show("Persegi Panjang");
                }
                else
                {
                    // Handle case when logoValue is "1" and jenisValue is not "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, "Persegi");
                    e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);
                    //MessageBox.Show("Persegi");
                }
            }
            else if (logoValue == "2")
            {
                AdjustPictureBoxSize(e.Graphics, jenisValue); // Adjust the size based on jenisValue
                e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);

                // Adjust the coordinates based on jenisValue
                if (jenisValue == "Persegi")
                {
                    e.Graphics.DrawImage(picLogo2.Image, 690, 3, picLogo2.Width, picLogo2.Height);
                }
                else if (jenisValue == "Persegi Panjang")
                {
                    e.Graphics.DrawImage(picLogo2.Image, 640, 3, picLogo2.Width, picLogo2.Height);
                }
            }


            StringFormat SF1 = new StringFormat();
            SF1.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(richTextBoxNRS.Text, new Font("Montserrat", 16, FontStyle.Bold), Brushes.Black, 400, 18, SF1);
            e.Graphics.DrawString(richTextBoxBE.Text, new Font("Montserrat", 14, FontStyle.Bold), Brushes.Black, 400, 45, SF1);
            e.Graphics.DrawString(richTextBoxJalan.Text, new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, 400, 70, SF1);
            e.Graphics.DrawString(richTextBoxEmail.Text, new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, 400, 85, SF1);

            ////kotak poliklinik
            //Color red = Color.Black;
            //Pen redPen = new Pen(red);
            //redPen.Width = 1;
            //e.Graphics.DrawRectangle(redPen, 30, 110, 760, 35);
            //e.Graphics.DrawString(richTextBox2.Text, new Font("Montserrat", 14, FontStyle.Bold), Brushes.Black, 320, 115);


            //kotak poliklinik
            Color red = Color.Black;
            Pen redPen = new Pen(red);
            redPen.Width = 1;
            e.Graphics.DrawRectangle(redPen, 5, 110, 791, 35);

            Font font = new Font("Montserrat", 14, FontStyle.Bold);

            SizeF textSize = e.Graphics.MeasureString(richTextBox2.Text, font);

            float centerX = 5 + (791 - textSize.Width) / 2;
            float centerY = 110 + (35 - textSize.Height) / 2;

            e.Graphics.DrawString(richTextBox2.Text, font, Brushes.Black, centerX, centerY);

            //kotak data pasien
            Color redd = Color.Black;
            Pen redPenn = new Pen(redd);
            redPenn.Width = 1;
            e.Graphics.DrawRectangle(redPenn, 5, 145, 791, 90);
            e.Graphics.DrawString("Nama", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 8, 150);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 150);
            e.Graphics.DrawString(labelNama.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 150);

            e.Graphics.DrawString("Tanggal Lahir", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 8, 180);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 180);
            e.Graphics.DrawString(labelTglUmur.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 180);

            e.Graphics.DrawString("No RM", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 8, 195);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 195);
            e.Graphics.DrawString(labelNoMR.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 195);

            e.Graphics.DrawString("Jenis Pemeriksaan", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 150);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 150);
            e.Graphics.DrawString(labelJenisPemeriksaan.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 150);

            //e.Graphics.DrawString("Keterangan Klinis", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 165);
            //e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 165);
            //e.Graphics.DrawString(textBoxKlinis.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 165);

            //mulai
            // Mengambil teks dari textBoxKlinis
            string klinisText = textBoxKlinis.Text;

            // Maksimum karakter per baris
            int maxCharsPerLine = 33;

            // Font yang digunakan untuk teks
            Font regularFont = new Font("Montserrat", 9, FontStyle.Regular);
            Font boldFont = new Font("Montserrat", 9, FontStyle.Bold);

            // Posisi awal untuk teks
            float labelX = 370; // X untuk label "Keterangan Klinis"
            float colonX = 505; // X untuk tanda ":"
            float startX = 515; // X untuk teks klinis, buat ini konsisten
            float startY = 165; // Posisi vertikal untuk baris pertama
            float lineHeight = regularFont.GetHeight(e.Graphics);

            // Cetak label "Keterangan Klinis"
            e.Graphics.DrawString("Keterangan Klinis", boldFont, Brushes.Black, labelX, startY);
            e.Graphics.DrawString(":", boldFont, Brushes.Black, colonX, startY);

            // Memecah teks menjadi kata-kata
            string[] words = klinisText.Split(' ');
            string currentLine = "";
            List<string> lines = new List<string>();

            // Membagi teks berdasarkan kata, bukan karakter
            foreach (var word in words)
            {
                if (currentLine.Length + word.Length + 1 <= maxCharsPerLine)
                {
                    // Tambahkan kata ke baris saat ini
                    currentLine += (currentLine == "" ? "" : " ") + word;
                }
                else
                {
                    // Simpan baris dan mulai baris baru
                    lines.Add(currentLine);
                    currentLine = word;
                }
            }

            // Tambahkan baris terakhir
            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            // Cetak setiap baris teks, dengan X yang sama untuk semua baris
            for (int i = 0; i < lines.Count; i++)
            {
                // Cetak baris teks di posisi X yang sama (startX) dan Y yang menurun setiap baris
                e.Graphics.DrawString(lines[i], regularFont, Brushes.Black, startX, startY + (i * lineHeight));
            }
            //berakhir


            e.Graphics.DrawString("Alamat", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 195);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 195);
            e.Graphics.DrawString(labelAlamat.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 195);

            //kotak foto
            Color reddd = Color.Black;
            Pen redddPen = new Pen(reddd);
            redddPen.Width = 1;
            e.Graphics.DrawRectangle(redddPen, 5, 235, 791, 680);
            e.Graphics.DrawString("Hasil Foto", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 8, 240);
            e.Graphics.DrawString(":", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 120, 240);

            e.Graphics.DrawImage(pictureBox1.Image, 24, 262, 752, 396);

            //kotak hasil pemeriksaan
            Color redddd = Color.Black;
            Pen reddddPen = new Pen(redddd);
            reddddPen.Width = 1;
            e.Graphics.DrawRectangle(reddddPen, 5, 915, 395, 160);

            //kotak tanda tangan
            Pen redddddPen = new Pen(reddd);
            redddddPen.Width = 1;
            e.Graphics.DrawRectangle(redddddPen, 400, 915, 396, 160);

            e.Graphics.DrawString("Hasil Pemeriksaan", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 8, 920);
            e.Graphics.DrawString(":", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 161, 920);

            // Set the RichTextBox text with line breaks if needed
            string hasilPemeriksaan = AddNewlinesIfTooLong(richTextBox1.Text, 55);
            e.Graphics.DrawString(hasilPemeriksaan, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 8, 940);



            e.Graphics.DrawString(labelLokTgl.Text, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 599, 920, SF1);
            e.Graphics.DrawString(label30.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 599, 934, SF1);
            StringFormat sff = new StringFormat();
            sff.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(labelNamaDokter.Text, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 599, 1055, sff);
        }

        private void printDocument12_PrintPage(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(picLogo1.Image, 10, 5, 100, 100);

            if (logoValue == "1")
            {
                //AdjustPictureBoxSize(e.Graphics, "Persegi");
                //e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);

                if (jenisValue == "Persegi Panjang")
                {
                    // Handle case when logoValue is "1" and jenisValue is "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, jenisValue);
                    e.Graphics.DrawImage(picLogo2.Image, 30, 3, picLogo2.Width, picLogo2.Height);

                    //MessageBox.Show("Persegi Panjang");
                }
                else
                {
                    // Handle case when logoValue is "1" and jenisValue is not "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, "Persegi");
                    e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);
                    //MessageBox.Show("Persegi");
                }
            }
            else if (logoValue == "2")
            {
                AdjustPictureBoxSize(e.Graphics, jenisValue); // Adjust the size based on jenisValue
                e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);

                // Adjust the coordinates based on jenisValue
                if (jenisValue == "Persegi")
                {
                    e.Graphics.DrawImage(picLogo2.Image, 690, 3, picLogo2.Width, picLogo2.Height);
                }
                else if (jenisValue == "Persegi Panjang")
                {
                    e.Graphics.DrawImage(picLogo2.Image, 640, 3, picLogo2.Width, picLogo2.Height);
                }
            }


            StringFormat SF1 = new StringFormat();
            SF1.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(richTextBoxNRS.Text, new Font("Montserrat", 16, FontStyle.Bold), Brushes.Black, 424, 18, SF1);
            e.Graphics.DrawString(richTextBoxBE.Text, new Font("Montserrat", 14, FontStyle.Bold), Brushes.Black, 424, 45, SF1);
            e.Graphics.DrawString(richTextBoxJalan.Text, new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, 424, 70, SF1);
            e.Graphics.DrawString(richTextBoxEmail.Text, new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, 424, 85, SF1);

            ////kotak poliklinik
            //Color red = Color.Black;
            //Pen redPen = new Pen(red);
            //redPen.Width = 1;
            //e.Graphics.DrawRectangle(redPen, 30, 110, 760, 35);
            //e.Graphics.DrawString(richTextBox2.Text, new Font("Montserrat", 14, FontStyle.Bold), Brushes.Black, 320, 115);


            //kotak poliklinik
            Color red = Color.Black;
            Pen redPen = new Pen(red);
            redPen.Width = 1;
            e.Graphics.DrawRectangle(redPen, 21, 110, 805, 35);

            Font font = new Font("Montserrat", 14, FontStyle.Bold);

            SizeF textSize = e.Graphics.MeasureString(richTextBox2.Text, font);

            float centerX = 21 + (805 - textSize.Width) / 2;
            float centerY = 110 + (35 - textSize.Height) / 2;

            e.Graphics.DrawString(richTextBox2.Text, font, Brushes.Black, centerX, centerY);

            //kotak data pasien
            Color redd = Color.Black;
            Pen redPenn = new Pen(redd);
            redPenn.Width = 1;
            e.Graphics.DrawRectangle(redPenn, 21, 145, 805, 90);
            e.Graphics.DrawString("Nama", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 24, 150);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 150);
            e.Graphics.DrawString(labelNama.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 150);

            e.Graphics.DrawString("Tanggal Lahir", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 24, 180);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 180);
            e.Graphics.DrawString(labelTglUmur.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 180);

            e.Graphics.DrawString("No RM", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 24, 195);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 195);
            e.Graphics.DrawString(labelNoMR.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 195);

            e.Graphics.DrawString("Jenis Pemeriksaan", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 150);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 150);
            e.Graphics.DrawString(labelJenisPemeriksaan.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 150);

            //e.Graphics.DrawString("Keterangan Klinis", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 165);
            //e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 165);
            //e.Graphics.DrawString(textBoxKlinis.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 165);

            //mulai
            // Mengambil teks dari textBoxKlinis
            string klinisText = textBoxKlinis.Text;

            // Maksimum karakter per baris
            int maxCharsPerLine = 33;

            // Font yang digunakan untuk teks
            Font regularFont = new Font("Montserrat", 9, FontStyle.Regular);
            Font boldFont = new Font("Montserrat", 9, FontStyle.Bold);

            // Posisi awal untuk teks
            float labelX = 370; // X untuk label "Keterangan Klinis"
            float colonX = 505; // X untuk tanda ":"
            float startX = 515; // X untuk teks klinis, buat ini konsisten
            float startY = 165; // Posisi vertikal untuk baris pertama
            float lineHeight = regularFont.GetHeight(e.Graphics);

            // Cetak label "Keterangan Klinis"
            e.Graphics.DrawString("Keterangan Klinis", boldFont, Brushes.Black, labelX, startY);
            e.Graphics.DrawString(":", boldFont, Brushes.Black, colonX, startY);

            // Memecah teks menjadi kata-kata
            string[] words = klinisText.Split(' ');
            string currentLine = "";
            List<string> lines = new List<string>();

            // Membagi teks berdasarkan kata, bukan karakter
            foreach (var word in words)
            {
                if (currentLine.Length + word.Length + 1 <= maxCharsPerLine)
                {
                    // Tambahkan kata ke baris saat ini
                    currentLine += (currentLine == "" ? "" : " ") + word;
                }
                else
                {
                    // Simpan baris dan mulai baris baru
                    lines.Add(currentLine);
                    currentLine = word;
                }
            }

            // Tambahkan baris terakhir
            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            // Cetak setiap baris teks, dengan X yang sama untuk semua baris
            for (int i = 0; i < lines.Count; i++)
            {
                // Cetak baris teks di posisi X yang sama (startX) dan Y yang menurun setiap baris
                e.Graphics.DrawString(lines[i], regularFont, Brushes.Black, startX, startY + (i * lineHeight));
            }
            //berakhir

            e.Graphics.DrawString("Alamat", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 195);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 195);
            e.Graphics.DrawString(labelAlamat.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 195);

            //kotak foto
            Color reddd = Color.Black;
            Pen redddPen = new Pen(reddd);
            redddPen.Width = 1;
            e.Graphics.DrawRectangle(redddPen, 21, 235, 805, 680);
            e.Graphics.DrawString("Hasil Foto", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 24, 240);
            e.Graphics.DrawString(":", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 120, 240);

            e.Graphics.DrawImage(pictureBox1.Image, 40, 262, 768, 396);

            //kotak hasil pemeriksaan
            Color redddd = Color.Black;
            Pen reddddPen = new Pen(redddd);
            reddddPen.Width = 1;
            e.Graphics.DrawRectangle(reddddPen, 21, 915, 402, 160);

            //kotak tanda tangan
            Pen redddddPen = new Pen(reddd);
            redddddPen.Width = 1;
            e.Graphics.DrawRectangle(redddddPen, 423, 915, 403, 160);

            e.Graphics.DrawString("Hasil Pemeriksaan", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 24, 920);
            e.Graphics.DrawString(":", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 161, 920);

            // Set the RichTextBox text with line breaks if needed
            string hasilPemeriksaan = AddNewlinesIfTooLong(richTextBox1.Text, 55);
            e.Graphics.DrawString(hasilPemeriksaan, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 24, 940);
            e.Graphics.DrawString(labelLokTgl.Text, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 624, 920, SF1);
            e.Graphics.DrawString(label30.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 624, 934, SF1);
            StringFormat sff = new StringFormat();
            sff.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(labelNamaDokter.Text, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 624, 1055, sff);
        }

        private void AdjustPictureBoxSize(Graphics graphics, string jenis)
        {
            // Check the value of jenis and adjust the size of PictureBox controls accordingly
            if (jenis == "Persegi")
            {
                picLogo1.Size = new Size(100, 100);
                picLogo2.Size = new Size(100, 100);
            }
            else if (jenis == "Persegi Panjang")
            {
                picLogo1.Size = new Size(150, 100);
                picLogo2.Size = new Size(150, 100);
            }
            // Add more conditions as needed
        }

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(picLogo1.Image, 10, 5, 100, 100); 

            if (logoValue == "1")
            {
                //AdjustPictureBoxSize(e.Graphics, "Persegi");
                //e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);

                if (jenisValue == "Persegi Panjang")
                {
                    // Handle case when logoValue is "1" and jenisValue is "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, jenisValue);
                    e.Graphics.DrawImage(picLogo2.Image, 30, 3, picLogo2.Width, picLogo2.Height);
                    //MessageBox.Show("Persegi Panjang");
                }
                else
                {
                    // Handle case when logoValue is "1" and jenisValue is not "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, "Persegi");
                    e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);
                    //MessageBox.Show("Persegi");
                }
            }
            else if (logoValue == "2")
            {
                AdjustPictureBoxSize(e.Graphics, jenisValue); // Adjust the size based on jenisValue
                e.Graphics.DrawImage(picLogo1.Image, 30, 3, picLogo1.Width, picLogo1.Height);

                // Adjust the coordinates based on jenisValue
                if (jenisValue == "Persegi")
                {
                    e.Graphics.DrawImage(picLogo2.Image, 690, 3, picLogo2.Width, picLogo2.Height);
                }
                else if (jenisValue == "Persegi Panjang")
                {
                    e.Graphics.DrawImage(picLogo2.Image, 640, 3, picLogo2.Width, picLogo2.Height);
                }
            }

            StringFormat SF1 = new StringFormat();
            SF1.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(richTextBoxNRS.Text, new Font("Montserrat", 16, FontStyle.Bold), Brushes.Black, 400, 18, SF1);
            e.Graphics.DrawString(richTextBoxBE.Text, new Font("Montserrat", 14, FontStyle.Bold), Brushes.Black, 400, 45, SF1);
            e.Graphics.DrawString(richTextBoxJalan.Text, new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, 400, 70, SF1);
            e.Graphics.DrawString(richTextBoxEmail.Text, new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, 400, 85, SF1);

            ////kotak poliklinik
            //Color red = Color.Black;
            //Pen redPen = new Pen(red);
            //redPen.Width = 1;
            //e.Graphics.DrawRectangle(redPen, 30, 110, 760, 35);
            //e.Graphics.DrawString(richTextBox2.Text, new Font("Montserrat", 14, FontStyle.Bold), Brushes.Black, 320, 115);

            //kotak poliklinik
            Color red = Color.Black;
            Pen redPen = new Pen(red);
            redPen.Width = 1;
            e.Graphics.DrawRectangle(redPen, 5, 110, 791, 35);

            Font font = new Font("Montserrat", 14, FontStyle.Bold);

            SizeF textSize = e.Graphics.MeasureString(richTextBox2.Text, font);

            float centerX = 5 + (791 - textSize.Width) / 2;
            float centerY = 110 + (35 - textSize.Height) / 2;

            e.Graphics.DrawString(richTextBox2.Text, font, Brushes.Black, centerX, centerY);

            //kotak data pasien
            Color redd = Color.Black;
            Pen redPenn = new Pen(redd);
            redPenn.Width = 1;
            e.Graphics.DrawRectangle(redPenn, 5, 145, 791, 90);
            e.Graphics.DrawString("Nama", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 8, 150);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 150);
            e.Graphics.DrawString(labelNama.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 150);

            e.Graphics.DrawString("Tanggal Lahir", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 8, 180);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 180);
            e.Graphics.DrawString(labelTglUmur.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 180);

            e.Graphics.DrawString("No RM", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 8, 195);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 135, 195);
            e.Graphics.DrawString(labelNoMR.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 145, 195);

            e.Graphics.DrawString("Jenis Pemeriksaan", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 150);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 150);
            e.Graphics.DrawString(labelJenisPemeriksaan.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 150);

            //e.Graphics.DrawString("Keterangan Klinis", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 165);
            //e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 165);
            //e.Graphics.DrawString(textBoxKlinis.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 165);

            //mulai
            // Mengambil teks dari textBoxKlinis
            string klinisText = textBoxKlinis.Text;

            // Maksimum karakter per baris
            int maxCharsPerLine = 33;

            // Font yang digunakan untuk teks
            Font regularFont = new Font("Montserrat", 9, FontStyle.Regular);
            Font boldFont = new Font("Montserrat", 9, FontStyle.Bold);

            // Posisi awal untuk teks
            float labelX = 370; // X untuk label "Keterangan Klinis"
            float colonX = 505; // X untuk tanda ":"
            float startX = 515; // X untuk teks klinis, buat ini konsisten
            float startY = 165; // Posisi vertikal untuk baris pertama
            float lineHeight = regularFont.GetHeight(e.Graphics);

            // Cetak label "Keterangan Klinis"
            e.Graphics.DrawString("Keterangan Klinis", boldFont, Brushes.Black, labelX, startY);
            e.Graphics.DrawString(":", boldFont, Brushes.Black, colonX, startY);

            // Memecah teks menjadi kata-kata
            string[] words = klinisText.Split(' ');
            string currentLine = "";
            List<string> lines = new List<string>();

            // Membagi teks berdasarkan kata, bukan karakter
            foreach (var word in words)
            {
                if (currentLine.Length + word.Length + 1 <= maxCharsPerLine)
                {
                    // Tambahkan kata ke baris saat ini
                    currentLine += (currentLine == "" ? "" : " ") + word;
                }
                else
                {
                    // Simpan baris dan mulai baris baru
                    lines.Add(currentLine);
                    currentLine = word;
                }
            }

            // Tambahkan baris terakhir
            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            // Cetak setiap baris teks, dengan X yang sama untuk semua baris
            for (int i = 0; i < lines.Count; i++)
            {
                // Cetak baris teks di posisi X yang sama (startX) dan Y yang menurun setiap baris
                e.Graphics.DrawString(lines[i], regularFont, Brushes.Black, startX, startY + (i * lineHeight));
            }
            //berakhir

            e.Graphics.DrawString("Alamat", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 370, 195);
            e.Graphics.DrawString(":", new Font("Montserrat", 9, FontStyle.Bold), Brushes.Black, 505, 195);
            e.Graphics.DrawString(labelAlamat.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 515, 195);

            //kotak foto
            Color reddd = Color.Black;
            Pen redddPen = new Pen(reddd);
            redddPen.Width = 1;
            e.Graphics.DrawRectangle(redddPen, 5, 235, 791, 680);
            e.Graphics.DrawString("Hasil Foto", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 8, 240);
            e.Graphics.DrawString(":", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 120, 240);

            //float contrast = 1.41f;
            float contrast = 1.00f;
            float gamma = 0.715f;
            float reed = 0.56f;
            float green = 0.35f;
            float blue = 0.28f;

            ImageAttributes ia = new ImageAttributes();
            float[][] ptsarray = {
                        new float[] { contrast+reed, 0f, 0f, 0f, 0f},
                        new float[] { 0f, contrast+green, 0f, 0f, 0f},
                        new float[] { 0f, 0f, contrast+blue, 0f, 0f},
                        new float[] { 0f, 0f,       0f, 1f, 0f},
                        new float[] {   0, 0,        0, 1f, 1f},
                                };
            ia.ClearColorMatrix();
            ia.SetColorMatrix(new ColorMatrix(ptsarray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            ia.SetGamma(gamma, ColorAdjustType.Bitmap);
            e.Graphics.DrawImage(pictureBox1.Image, new Rectangle(24, 262, 752, 396), 0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height, GraphicsUnit.Pixel, ia);
            ia.Dispose();

            //kotak hasil pemeriksaan
            Color redddd = Color.Black;
            Pen reddddPen = new Pen(redddd);
            reddddPen.Width = 1;
            e.Graphics.DrawRectangle(reddddPen, 5, 915, 395, 160);

            //kotak tanda tangan
            Pen redddddPen = new Pen(reddd);
            redddddPen.Width = 1;
            e.Graphics.DrawRectangle(redddddPen, 400, 915, 396, 160);

            e.Graphics.DrawString("Hasil Pemeriksaan", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 8, 920);
            e.Graphics.DrawString(":", new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 161, 920);

            // Set the RichTextBox text with line breaks if needed
            string hasilPemeriksaan = AddNewlinesIfTooLong(richTextBox1.Text, 55);
            e.Graphics.DrawString(hasilPemeriksaan, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 8, 940);
            e.Graphics.DrawString(labelLokTgl.Text, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 599, 920, SF1);
            e.Graphics.DrawString(label30.Text, new Font("Montserrat", 9, FontStyle.Regular), Brushes.Black, 599, 934, SF1);
            StringFormat sff = new StringFormat();
            sff.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(labelNamaDokter.Text, new Font("Montserrat", 10, FontStyle.Regular), Brushes.Black, 599, 1055, sff);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.icon;
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            FillListBox();
            int kondisi1 = 7;
            TEViewC6Gambar(kondisi1.ToString());
            this.Close();
        }

        private void FormFormat3_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label10;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            string dirlogo1 = dirLogo + "logo1.png";
            btn_Delete.Visible = false;
            //string dirlogo1 = dir + "1160358.png";
            if (!Directory.Exists(dirlogo1))
            {
                picLogo1.Image = Image.FromFile(dirLogo + "logo1.png");
                picLogo2.Image = Image.FromFile(dirLogo + "logo2.png");
            }



            LoadAndSetValues(); 


        }

        private void button1_Click(object sender, EventArgs e)
        { 
                        pictureBox1.Image = Properties.Resources.icon; 
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            FillListBox();
            int kondisi1 = 5;
            TEViewC2Gambar(kondisi1.ToString()); 
            this.Close(); 
        }

        private void button7_Click(object sender, EventArgs e)
        { 
            pictureBox1.Image = Properties.Resources.icon; 
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            FillListBox();
            int kondisi1 = 6;
            TEViewC4Gambar(kondisi1.ToString()); 
            this.Close(); 
        } 

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            btn_Delete.Visible = false;

        }

        private void buttonExportPdf_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Foto diisi Dahulu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (textBoxKlinis.Text == "")
                {
                    MessageBox.Show("Klinis Belum diisi ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    if (richTextBox1.Text == "")
                    {
                        MessageBox.Show("Hasil Pemeriksaan Belum diisi ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\EksporPDF\Format-2\1-Gambar\";
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        string existingPathName = dir;
                        string notExistingFileName = dir + gabung1 + ".pdf";

                        if (Directory.Exists(existingPathName) && !File.Exists(notExistingFileName))
                        {
                            PrintDocument pdoc = new PrintDocument();
                            pdoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                            pdoc.PrinterSettings.PrintFileName = notExistingFileName;
                            pdoc.PrinterSettings.PrintToFile = true;
                            pdoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
                            pdoc.DefaultPageSettings.Landscape = false;
                            pdoc.PrintPage += printDocument12_PrintPage;
                            pdoc.Print();

                            buttonCancel.PerformClick();

                            int kondisi1 = 4;
                            TEViewC21G(kondisi1.ToString());
                            MessageBox.Show("Proses ekspor file berhasil diselesaikan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                }
            }

        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {  
            pictureBox1.Image = Properties.Resources.icon;  
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null; 
            clearTextboxPemeriksaan();
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            FillListBox();
            int kondisi = 9;
            TEFormat2(kondisi.ToString()); 
            this.Close(); 
        } 

        private void clearTextboxPemeriksaan()
        { 
            textBoxKlinis.Clear();
            richTextBox1.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            { 
                ReadDataFromCSV(csvFilePath);
                LoadAndSetValues();
                textBox2.Clear();
            }
        }

        private void LoadAndSetValues()
        {
            string filePath = @"D:\ZeusEndoscope\LogoKOP\logo.xml";

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Load the XML document from the file
                    XDocument xd = XDocument.Load(filePath);

                    // Retrieve the logo value from the XML document
                    logoValue = xd.Element("userdata")?.Element("logo")?.Value;


                    // Set the logo value to the TextBox
                    //textBoxLogo.Text = logoValue;

                    // Retrieve the jenis value from the XML document
                    jenisValue = xd.Element("userdata")?.Element("jenis")?.Value;


                    // Check the logoValue and show/hide PictureBox controls accordingly
                    if (logoValue == "1")
                    {
                        picLogo1.Visible = true;
                        picLogo2.Visible = false;
                    }
                    else if (logoValue == "2")
                    {
                        picLogo1.Visible = true;
                        picLogo2.Visible = true;
                    }
                    else
                    {
                        // Handle other cases if needed
                        picLogo1.Visible = false;
                        picLogo2.Visible = false;
                    }

                    // Check the jenisValue and adjust the size of PictureBox controls accordingly
                    if (jenisValue == "Persegi")
                    {
                        picLogo1.Size = new Size(100, 100);
                        picLogo2.Size = new Size(100, 100);

                        richTextBoxNRS.Size = new Size(644, 20);
                        richTextBoxNRS.Location = new Point(109, 12); 

                        richTextBoxBE.Size = new Size(644, 20);
                        richTextBoxBE.Location = new Point(109, 34);

                        richTextBoxJalan.Size = new Size(644, 18);
                        richTextBoxJalan.Location = new Point(109, 55);

                        richTextBoxEmail.Size = new Size(644, 18);
                        richTextBoxEmail.Location = new Point(109, 71);

                        //label1.Size = new Size(538, 23);
                        //label1.Location = new Point(164, 27);

                        //label2.Size = new Size(613, 23);
                        //label2.Location = new Point(127, 50);
                    }
                    else if (jenisValue == "Persegi Panjang")
                    {
                        picLogo1.Size = new Size(150, 100);
                        picLogo2.Size = new Size(150, 100);
                        picLogo2.Location = new Point(705, 5);

                        richTextBoxNRS.Size = new Size(544, 20);
                        richTextBoxBE.Size = new Size(544, 20);
                        richTextBoxJalan.Size = new Size(544, 18);
                        richTextBoxEmail.Size = new Size(544, 18);

                        richTextBoxNRS.Location = new Point(159, 12);
                        richTextBoxBE.Location = new Point(159, 34);
                        richTextBoxJalan.Location = new Point(159, 55);
                        richTextBoxEmail.Location = new Point(159, 71);


                        //label1.Size = new Size(343, 23);
                        //label1.Location = new Point(262, 27);

                        //label2.Size = new Size(418, 23);
                        //label2.Location = new Point(225, 50);
                    }
                }
                else
                {
                    MessageBox.Show("The XML file does not exist.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ReadDataFromCSV(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    // Baca record dari file CSV satu per satu
                    while (csv.Read())
                    {
                        // Ambil data dari record saat ini
                        var noRM = csv.GetField<string>("Rm");
                        var name = csv.GetField<string>("Nama");
                        var action = csv.GetField<string>("Jenis Pemeriksaan");
                        var date = csv.GetField<string>("Tanggal Kunjungan");
                        var tanggalLahir = csv.GetField<string>("Tanggal Lahir");
                        var umur = csv.GetField<string>("Umur");
                        var alamat = csv.GetField<string>("Alamat");
                        var dokterNama = csv.GetField<string>("Dokter");
                        gabung = noRM + "-" + name;

                        DateTime today = DateTime.Now;
                        jam = today.ToString("hhmmss");
                        gabung1 = noRM + "-" + name + "-" + jam;

                        string combinedText = name;
                        labelNama.Text = AddNewlinesIfTooLong(combinedText, 30);

                        string tgl_lahir, tglKunjungan;
                        tgl_lahir = tanggalLahir;
                        labelTglUmur.Text = tgl_lahir + " - " + umur;
                        labelNoMR.Text = noRM;
                        labelJenisPemeriksaan.Text = action;
                        tglKunjungan = date;

                        string combinedAlamat = alamat;
                        labelAlamat.Text = AddNewlinesIfTooLong(combinedAlamat, 40);

                        richTextBoxNRS.LoadFile(dirRtf + "RtfFile.rtf", RichTextBoxStreamType.RichText);
                        richTextBoxBE.LoadFile(dirRtf + "RtfFile1.rtf", RichTextBoxStreamType.RichText);
                        richTextBoxJalan.LoadFile(dirRtf + "RtfFile2.rtf", RichTextBoxStreamType.RichText);
                        richTextBoxEmail.LoadFile(dirRtf + "RtfFile3.rtf", RichTextBoxStreamType.RichText);
                        richTextBox2.LoadFile(dirRtf + "RtfFile5.rtf", RichTextBoxStreamType.RichText);
                        richTextBox5.LoadFile(dirRtf + "RtfFile4.rtf", RichTextBoxStreamType.RichText);

                        ambilDaerah = richTextBox5.Text;
                        labelLokTgl.Text = ambilDaerah + ", " + tglKunjungan;

                        string namaDokter = dokterNama;
                        labelNamaDokter.Text = "(" + namaDokter + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tidak ada data yang tersedia. Mohon isi data Pasien terlebih dahulu.", "Informasi!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string AddNewlinesIfTooLong(string inputText, int maxLineLength)
        {
            StringBuilder result = new StringBuilder();
            string[] words = inputText.Split(' ');  // Memecah teks menjadi kata-kata
            int currentLineLength = 0;

            foreach (string word in words)
            {
                // Jika menambahkan kata akan melebihi batas, maka pindah ke baris berikutnya
                if (currentLineLength + word.Length + 1 > maxLineLength)
                {
                    result.AppendLine();  // Menambahkan newline
                    currentLineLength = 0; // Reset panjang baris saat ini
                }

                // Menambahkan kata ke baris dan memperbarui panjang baris
                if (currentLineLength > 0)
                {
                    result.Append(" ");  // Menambahkan spasi jika bukan kata pertama di baris
                    currentLineLength++;  // Menambah 1 untuk spasi
                }

                result.Append(word);  // Menambahkan kata
                currentLineLength += word.Length;  // Menambah panjang kata ke panjang baris
            }

            return result.ToString();
        }

        //private string AddNewlinesIfTooLong(string inputText, int maxLineLength)
        //{
        //    StringBuilder result = new StringBuilder();
        //    string[] words = inputText.Split(' ');

        //    int currentLineLength = 0;

        //    foreach (string word in words)
        //    {
        //        if (currentLineLength + word.Length + 1 <= maxLineLength)
        //        {
        //            result.Append(word + " ");
        //            currentLineLength += word.Length + 1;
        //        }
        //        else
        //        {
        //            result.AppendLine();  
        //            result.Append(word + " ");
        //            currentLineLength = word.Length + 1;
        //        }
        //    }

        //    return result.ToString();
        //} 

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            tanggal = DateTime.Now.ToString("ddMMyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];

            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            of.InitialDirectory = "D:\\ZeusEndoscope\\" + splitTahun + "\\" + splitBulan + "\\" + tanggal + "\\" + gabung + "\\Image";

            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = of.FileName;
                btn_Delete.Visible = true;
            }
        }  
    }
}
