﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using CsvHelper;
using PictureBox = System.Windows.Forms.PictureBox;

namespace WindowsFormsApp1
{
    public partial class Form1Print : Form
    {
        string dirLogo = @"D:\ZeusEndoscope\LogoKOP\";
        //string dir = @"D:\";
        string csvFilePath = "D:\\ZeusEndoscope\\Database\\dataPasien\\dataDefault.csv";

        public delegate void TransfDelegate(String value);
        public event TransfDelegate TransfEventt;
        public event TransfDelegate TransfEvenPrint4;
        public event TransfDelegate TransfEvenPrint6;
        public event TransfDelegate TransfEventPrint1G;
        string jam, Name, Code, Date, action, tanggal, id, gabung1, gabung, splitBulan, splitTahun, noRM, nameFix;

        string logoValue, jenisValue;

        // Deklarasikan matchingFolders sebagai variabel kelas
        private List<string> matchingFolders = new List<string>();

        private Dictionary<PictureBox, PictureBoxControls> pictureBoxControls = new Dictionary<PictureBox, PictureBoxControls>();
        public Form1Print()
        {
            InitializeComponent();
            LoadPrinters();
            PopulatePrinterComboBox(); // Call to populate printers
            comboBox1.SelectedIndex = -1; // Ensure no printer is selected by default
            InitializeThumbnails();
            InitializeThumbnailsForToday();
            InitializeMainPictureBoxes();
            InitializeComboBox();
            InitializeComboBoxNow();


            comboBox1.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
            comboBox2.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
            comboBox3.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
        }
        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; // Mencegah karakter yang diketik ditampilkan di ComboBox
        }

        private void LoadPrinters()
        {
            try
            {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(PrinterSettings.InstalledPrinters.Cast<string>().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading printers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class PictureBoxControls
        {
            public Control CloseControl { get; set; }
            public Control AddControl { get; set; }
        }


        private void PopulatePrinterComboBox()
        {
            // Clear the existing items before populating
            comboBox1.Items.Clear();

            // Get the list of installed printers
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                // Add each printer to the comboBox1
                comboBox1.Items.Add(printer);
            }

            // Optionally, set default selection (e.g., -1 to not select any printer)
            comboBox1.SelectedIndex = -1;
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



        //kode sebelumnya 01-10-2024
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

        private void cbx_baru_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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
                                Image = ResizeImage(image, 271, 134),  // Resize gambar ke ukuran thumbnail
                                SizeMode = PictureBoxSizeMode.StretchImage,  // Sesuaikan gambar dengan PictureBox
                                Size = new Size(255, 134),  // Ukuran PictureBox
                                Margin = new Padding(5),  // Spasi antar thumbnail
                                Tag = file  // Simpan path file di Tag untuk referensi
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
                                Image = ResizeImage(image, 271, 134),  // Resize gambar ke ukuran thumbnail
                                SizeMode = PictureBoxSizeMode.StretchImage,  // Sesuaikan gambar dengan PictureBox
                                Size = new Size(255, 134),  // Ukuran PictureBox
                                Margin = new Padding(5),  // Spasi antar thumbnail
                                Tag = file  // Simpan path file di Tag untuk referensi
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



        //private void InitializeThumbnails()
        //{
        //    // Call the method to read data from the CSV file
        //    ReadDataFromCSV(csvFilePath);

        //    // Gabungkan string menjadi bagian dari folder path yang akan dicari
        //    string searchPattern = $@"{gabung}\Image";

        //    // Root folder
        //    string rootPath = @"D:\ZeusEndoscope";

        //    // Bersihkan ComboBox dan FlowLayoutPanel sebelum memulai
        //    cbx_baru.Items.Clear();
        //    flowLayoutPanel1.Controls.Clear();

        //    // Mengumpulkan semua path folder yang sesuai
        //    List<string> matchingFolders = new List<string>();

        //    var culture = new System.Globalization.CultureInfo("id-ID");

        //    // Loop untuk mencari semua subfolder di rootPath
        //    foreach (string yearFolder in Directory.GetDirectories(rootPath))
        //    {
        //        foreach (string monthFolder in Directory.GetDirectories(yearFolder))
        //        {
        //            foreach (string dayFolder in Directory.GetDirectories(monthFolder))
        //            {
        //                foreach (string patientFolder in Directory.GetDirectories(dayFolder))
        //                {
        //                    // Gabungkan folder dengan folder Image
        //                    string folderPath = Path.Combine(patientFolder, "Image");

        //                    // Cek apakah folder saat ini adalah folder yang sesuai
        //                    if (patientFolder.EndsWith(gabung) && Directory.Exists(folderPath))
        //                    {
        //                        // Pastikan folder tersebut memiliki file gambar
        //                        string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
        //                            .Where(file => file.ToLower().EndsWith(".jpg") ||
        //                                           file.ToLower().EndsWith(".jpeg") ||
        //                                           file.ToLower().EndsWith(".png") ||
        //                                           file.ToLower().EndsWith(".bmp") ||
        //                                           file.ToLower().EndsWith(".gif"))
        //                            .ToArray();

        //                        if (imageFiles.Length > 0)
        //                        {
        //                            // Jika folder memiliki gambar, tambahkan ke matchingFolders
        //                            matchingFolders.Add(folderPath);

        //                            // Format tanggal dari folder dan tambahkan ke ComboBox
        //                            string formattedDate = $"{Path.GetFileName(yearFolder)} - {Path.GetFileName(monthFolder)} - {Path.GetFileName(dayFolder)}";
        //                            cbx_baru.Items.Add(new { FolderPath = folderPath, DisplayText = formattedDate });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Event handler ketika pilihan pada ComboBox berubah
        //    cbx_baru.SelectedIndexChanged += (s, e) =>
        //    {
        //        // Hapus semua thumbnail yang ada di FlowLayoutPanel
        //        flowLayoutPanel1.Controls.Clear();

        //        // Dapatkan folder path dari pilihan yang dipilih
        //        dynamic selectedItem = cbx_baru.SelectedItem;
        //        if (selectedItem != null)
        //        {
        //            string selectedFolder = selectedItem.FolderPath;

        //            // Ambil semua file gambar dari folder yang dipilih
        //            string[] imageFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.TopDirectoryOnly)
        //                .Where(file => file.ToLower().EndsWith(".jpg") ||
        //                               file.ToLower().EndsWith(".jpeg") ||
        //                               file.ToLower().EndsWith(".png") ||
        //                               file.ToLower().EndsWith(".bmp") ||
        //                               file.ToLower().EndsWith(".gif"))
        //                .ToArray();

        //            // Tampilkan gambar sebagai thumbnail
        //            foreach (string file in imageFiles)
        //            {
        //                try
        //                {
        //                    Image image = Image.FromFile(file);
        //                    PictureBox thumbnail = new PictureBox
        //                    {
        //                        Image = ResizeImage(image, 271, 134),  // Resize gambar ke ukuran thumbnail
        //                        SizeMode = PictureBoxSizeMode.StretchImage,  // Sesuaikan gambar dengan PictureBox
        //                        Size = new Size(255, 134),  // Ukuran PictureBox
        //                        Margin = new Padding(5),  // Spasi antar thumbnail
        //                        Tag = file  // Simpan path file di Tag untuk referensi
        //                    };

        //                    // Tambahkan event handler untuk menangani klik pada thumbnail
        //                    thumbnail.MouseDown += Thumbnail_MouseDown;
        //                    flowLayoutPanel1.Controls.Add(thumbnail);  // Tambahkan thumbnail ke FlowLayoutPanel
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Jika ada kesalahan saat memuat gambar, tampilkan pesan error
        //                    MessageBox.Show($"Error loading image {file}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //        }
        //    };

        //    // Jika tidak ada folder yang cocok
        //    if (matchingFolders.Count == 0)
        //    {
        //        MessageBox.Show("Tidak ditemukan folder yang sesuai dengan gabungan NORM dan Nama.", "Folder Tidak Ditemukan", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}





        //kode sebelumnya paling terakhir dari kode di atas
        //private void InitializeThumbnails()
        //{
        //    // Call the method to read data from the CSV file
        //    ReadDataFromCSV(csvFilePath);

        //    // Gabungkan string menjadi bagian dari folder path yang akan dicari
        //    string searchPattern = $@"{gabung}\Image";

        //    // Root folder
        //    string rootPath = @"D:\ZeusEndoscope";

        //    // Mengumpulkan semua path folder yang sesuai
        //    List<string> matchingFolders = new List<string>();

        //    // Loop untuk mencari semua subfolder di rootPath
        //    foreach (string yearFolder in Directory.GetDirectories(rootPath))
        //    {
        //        foreach (string monthFolder in Directory.GetDirectories(yearFolder))
        //        {
        //            foreach (string dayFolder in Directory.GetDirectories(monthFolder))
        //            {
        //                foreach (string patientFolder in Directory.GetDirectories(dayFolder))
        //                {
        //                    // Gabungkan folder dengan folder Image
        //                    string folderPath = Path.Combine(patientFolder, "Image");

        //                    // Cek apakah folder saat ini adalah folder yang sesuai
        //                    if (patientFolder.EndsWith(gabung) && Directory.Exists(folderPath))
        //                    {
        //                        matchingFolders.Add(folderPath);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Jika ditemukan folder yang sesuai
        //    if (matchingFolders.Count > 0)
        //    {
        //        // Ambil gambar dari semua folder yang sesuai
        //        foreach (string folder in matchingFolders)
        //        {
        //            // Ambil semua file gambar dari folder
        //            string[] imageFiles = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly)
        //                .Where(file => file.ToLower().EndsWith(".jpg") ||
        //                               file.ToLower().EndsWith(".jpeg") ||
        //                               file.ToLower().EndsWith(".png") ||
        //                               file.ToLower().EndsWith(".bmp") ||
        //                               file.ToLower().EndsWith(".gif"))
        //                .ToArray();

        //            // Cek jika tidak ada file gambar di dalam direktori
        //            if (imageFiles.Length == 0)
        //            {
        //                MessageBox.Show($"Tidak ada file gambar di dalam folder {folder}.", "Folder Kosong", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            else
        //            {
        //                foreach (string file in imageFiles)
        //                {
        //                    try
        //                    {
        //                        Image image = Image.FromFile(file);
        //                        PictureBox thumbnail = new PictureBox
        //                        {
        //                            Image = ResizeImage(image, 271, 134),  // Resize gambar ke ukuran thumbnail
        //                            SizeMode = PictureBoxSizeMode.StretchImage,  // Sesuaikan gambar dengan PictureBox
        //                            Size = new Size(255, 134),  // Ukuran PictureBox
        //                            Margin = new Padding(5),  // Spasi antar thumbnail
        //                            Tag = file  // Simpan path file di Tag untuk referensi
        //                        };

        //                        // Tambahkan event handler untuk menangani klik pada thumbnail
        //                        thumbnail.MouseDown += Thumbnail_MouseDown;
        //                        flowLayoutPanel1.Controls.Add(thumbnail);  // Tambahkan thumbnail ke FlowLayoutPanel
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        // Jika ada kesalahan saat memuat gambar, tampilkan pesan error
        //                        MessageBox.Show($"Error loading image {file}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Tidak ditemukan folder yang sesuai dengan gabungan NORM dan Nama.", "Folder Tidak Ditemukan", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}



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

            //string folderPath = $@"D:\ZeusEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Image";
            //string folderPath = $@"D:\ZeusEndoscope\{gabung}\Image";



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
            //                        Image = ResizeImage(image, 271, 134),  // Resize gambar ke ukuran thumbnail
            //                        SizeMode = PictureBoxSizeMode.StretchImage,  // Sesuaikan gambar dengan PictureBox
            //                        Size = new Size(255, 134),  // Ukuran PictureBox
            //                        Margin = new Padding(5),  // Spasi antar thumbnail
            //                        Tag = file  // Simpan path file di Tag untuk referensi
            //                    };

            //                    // Tambahkan event handler untuk menangani klik pada thumbnail
            //                    thumbnail.MouseDown += Thumbnail_MouseDown;
            //                    flowLayoutPanel1.Controls.Add(thumbnail);  // Tambahkan thumbnail ke FlowLayoutPanel
            //                }
            //                catch (Exception ex)
            //                {
            //                    // Jika ada kesalahan saat memuat gambar, tampilkan pesan error
            //                    MessageBox.Show($"Error loading image {file}: {ex.Message}");
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //MessageBox.Show("Ambil gambar terlebih dahulu.", "Gambar tidak ada", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //public Form1Print()
        //{
        //    InitializeComponent();

        //    // Inisialisasi ComboBox dengan daftar printer yang tersedia
        //    InitializePrinterComboBox();
        //}

        //private void InitializePrinterComboBox()
        //{
        //    try
        //    {
        //        // Mengambil daftar printer dan menambahkannya ke ComboBox sekaligus
        //        comboBox1.Items.AddRange(PrinterSettings.InstalledPrinters.Cast<string>().ToArray());
        //    }
        //    catch (System.ComponentModel.Win32Exception winEx)
        //    {
        //        // Menangani masalah terkait dengan akses sistem operasi atau printer
        //        MessageBox.Show($"Kesalahan akses printer: {winEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Menangani pengecualian umum lainnya
        //        MessageBox.Show($"Terjadi kesalahan saat memuat daftar printer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        private void button4_MouseMove(object sender, MouseEventArgs e)
        {
            button4.BackColor = Color.FromArgb(255, 153, 153);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Mengatur ComboBox
            ResetComboBoxes();

            // Mengatur PictureBox
            DisposeImage(pictureBox1);

            // Melakukan tindakan tambahan
            TransfEventt(9.ToString());

            // Menutup form
            Close();
        }

        private void ResetComboBoxes()
        {
            // Mengatur nilai SelectedIndex dan membersihkan item pada ComboBox
            comboBox2.SelectedIndex = -1;
            comboBox1.Items.Clear();
            comboBox1.ResetText();
        }

        private void DisposeImage(System.Windows.Forms.PictureBox pictureBox)
        {
            // Mengatur dan menghapus gambar pada PictureBox jika ada
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
        }

        


        private void button5_Click(object sender, EventArgs e)
        {
            int kondisi = 3;
            TransfEventt(kondisi.ToString());
        }

        public static class printer
        {
            [DllImport("winspool.drv",
              CharSet = CharSet.Auto,
              SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean SetDefaultPrinter(String name);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            string Pname = comboBox1.SelectedItem.ToString();
            printer.SetDefaultPrinter(Pname);

            if (comboBox1.Text == "Canon SELPHY CP1300")
            {
                //if (comboBox2.Items.Contains("A4"))
                //{
                //    comboBox2.Items.Remove("A4");
                //}

                // Sembunyikan elemen-elemen UI tertentu
                picLogo1.Visible = false;
                pictureBox2.Visible = false;
                label1.Visible = false;
                label2.Visible = false;

                int print1 = 1;
                textBox2.Text = print1.ToString();
            }
            else
            {
                //if (!comboBox2.Items.Contains("A4"))
                //{
                //    comboBox2.Items.Add("A4");
                //}
                // Tampilkan kembali elemen-elemen UI tersebut jika pilihan printer berbeda
                picLogo1.Visible = true;
                label1.Visible = true;
                label2.Visible = true;

                int print2 = 2;
                textBox2.Text = print2.ToString();
            }
        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        //    string Pname = comboBox1.SelectedItem.ToString();
        //    printer.SetDefaultPrinter(Pname);

        //    if (comboBox1.Text == "Canon SELPHY CP1300")
        //    {
        //        int print1 = 1;
        //        textBox2.Text = print1.ToString();
        //    } 
        //    else
        //    {  
        //        int print2 = 2;
        //        textBox2.Text = print2.ToString(); 
        //    }
        //    //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        //}

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            if (comboBox2.Text == "4R")
            {
                int print1 = 1;
                textBox6.Text = print1.ToString();
            }
            else
            {
                int print2 = 2;
                textBox6.Text = print2.ToString();
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null; 
            btn_Delete.Visible = false;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            if (panel4.BorderStyle == BorderStyle.FixedSingle)
            {
                int thickness = 2;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(Color.Black, thickness))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(halfThickness, halfThickness, panel4.ClientSize.Width - thickness, panel4.ClientSize.Height - thickness));
                }
            }
        } 

        //private void buttonAdd_Click(object sender, EventArgs e)
        //{
        //    comboBox2.SelectedIndex = -1;

        //    tanggal = DateTime.Now.ToString("ddMMyyy");
        //    string text = DateTime.Now.ToString("Y");
        //    string[] arr = text.Split(' ');
        //    splitBulan = arr[0];
        //    splitTahun = arr[1];

        //    OpenFileDialog of = new OpenFileDialog();
        //    of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
        //    of.InitialDirectory = "D:\\ZeusEndoscope\\" + splitTahun + "\\" + splitBulan + "\\" + tanggal + "\\" + gabung + "\\Image";
        //    if (of.ShowDialog() == DialogResult.OK)
        //    {
        //        pictureBox1.ImageLocation = of.FileName;
        //        btn_Delete.Visible = true; 

        //    }
        //}

        private void button8_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Foto diisi Dahulu ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (comboBox2.Text == "4R" || comboBox2.Text == "A4")
                {
                    int s = 1;
                    int r = 2;
                    if (textBox6.Text == s.ToString())
                    {
                        savePDF4R();
                        textBox6.Clear();
                        comboBox2.SelectedIndex = -1;
                        comboBox2.DropDownStyle = ComboBoxStyle.DropDownList; 
                        buttobDeleteFalse(); 

                        button4.PerformClick();
                        int kondisi1 = 1;
                        TransfEventPrint1G(kondisi1.ToString());
                        MessageBox.Show("Proses ekspor file berhasil diselesaikan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (textBox6.Text == r.ToString())
                    {
                        savePDFA4();
                        textBox6.Clear();
                        comboBox2.SelectedIndex = -1;
                        comboBox2.DropDownStyle = ComboBoxStyle.DropDownList; 
                        buttobDeleteFalse(); 

                        button4.PerformClick();
                        int kondisi1 = 1;
                        TransfEventPrint1G(kondisi1.ToString());
                        MessageBox.Show("Proses ekspor file berhasil diselesaikan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                } 
                else
                {
                    MessageBox.Show("Pilih ukuran kertas", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HandleButtonClick(2, TransfEvenPrint4);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            HandleButtonClick(3, TransfEvenPrint6);
        }

        // Helper method to handle common button click operations
        private void HandleButtonClick(int kondisi, TransfDelegate action)
        {
            // Mengatur ComboBox
            comboBox2.SelectedIndex = -1;
            comboBox1.Items.Clear();
            comboBox1.ResetText();

            // Mengatur PictureBox
            DisposeImage(pictureBox1);

            // Melakukan tindakan tambahan
            action(kondisi.ToString());

            // Menutup form
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            if (textBox3.Text != "")
            {

                // Call the method to read data from the CSV file
                ReadDataFromCSV(csvFilePath);
                LoadAndSetValues();
                textBox3.Clear();

                //MessageBox.Show(logoValue);

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
                        pictureBox2.Visible = false;
                    }
                    else if (logoValue == "2")
                    {
                        picLogo1.Visible = true;
                        pictureBox2.Visible = true;
                    }
                    else
                    {
                        // Handle other cases if needed
                        picLogo1.Visible = false;
                        pictureBox2.Visible = false;
                    }

                    // Check the jenisValue and adjust the size of PictureBox controls accordingly
                    if (jenisValue == "persegi")
                    {
                        picLogo1.Size = new Size(100, 100);
                        pictureBox2.Size = new Size(100, 100);

                        label1.Size = new Size(538, 23);
                        label1.Location = new Point(164, 27);

                        label2.Size = new Size(613,23);
                        label2.Location = new Point(127, 50);

                    }
                    else if (jenisValue == "Persegi Panjang")
                    {
                        picLogo1.Size = new Size(200, 100);
                        pictureBox2.Size = new Size(200, 100); 
                        pictureBox2.Location = new Point(647,20);

                        label1.Size = new Size(343,23);
                        label1.Location = new Point(262,27);

                        label2.Size = new Size(418,23);
                        label2.Location = new Point(225,50);
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
                        var noRMIndex = csv.GetField<string>("Rm");
                        var nameIndex = csv.GetField<string>("Nama");
                        var actionIndex = csv.GetField<string>("Jenis Pemeriksaan");
                        var dateIndex = csv.GetField<string>("Tanggal Kunjungan");
                        var tanggalLahir = csv.GetField<string>("Tanggal Lahir");
                        var umur = csv.GetField<string>("Umur");
                        var alamat = csv.GetField<string>("Alamat");
                        var dokterNama = csv.GetField<string>("Dokter");



                        DateTime today = DateTime.Now;
                        jam = today.ToString("hhmmss");
                        nameFix = GetFirstTwoWords(nameIndex);
                        string varia = nameFix + " - " + noRMIndex + " - " + dateIndex;
                        label1.Text = actionIndex;
                        label2.Text = varia;
                        gabung = noRMIndex + "-" + nameIndex;
                        gabung1 = noRMIndex + "-" + nameIndex + "-" + jam;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tidak ada data yang tersedia. Mohon isi data Pasien terlebih dahulu.", "Informasi!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private string GetFirstTwoWords(string inputText)
        {
            string[] words = inputText.Split(' ');

            if (words.Length >= 2)
            {
                return words[0] + " " + words[1];
            }
            else if (words.Length == 1)
            {
                return words[0];
            }
            else
            {
                return string.Empty;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        void otherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LoadAndSetValues();
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    // Validasi kondisi awal
        //    if (pictureBox1.Image == null)
        //    {
        //        MessageBox.Show("Foto diisi Dahulu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    if (comboBox1.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Pilih printer terlebih dahulu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    if (comboBox3.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Pilih profil terlebih dahulu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Setup PrintDocument
        //    PrintDocument pd = new PrintDocument();

        //    // Set PrintPage event handler based on conditions
        //    SetPrintPageEventHandler(pd);

        //    // Print document
        //    pd.Print();

        //    // Perform post-print actions
        //    PerformPostPrintActions();

        //    // Notify user
        //    MessageBox.Show("Dokumen berhasil diprint.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        private void SetPrintPageEventHandler(PrintDocument pd)
        {
            if (textBox2.Text == "1")
            {
                if (comboBox3.Text == "Default")
                    pd.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);
                else if (comboBox3.Text == "Adjust Brightness")
                    pd.PrintPage += new PrintPageEventHandler(this.printDocument4_PrintPage);
            }
            else
            {
                pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);

                if (comboBox3.Text == "Default")
                    pd.PrintPage += new PrintPageEventHandler(this.printDocument2_PrintPage);
                else if (comboBox3.Text == "Adjust Brightness")
                    pd.PrintPage += new PrintPageEventHandler(this.printDocument3_PrintPage);
            }
        }

        private void PerformPostPrintActions()
        {
            // Clear and reset controls
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            DisposeImage(pictureBox1);

            // Manage button states
            buttobDeleteFalse(); 
            button4.PerformClick();

            // Call event
            int kondisi1 = 1;
            TransfEventPrint1G(kondisi1.ToString());
        }

        private void cbx_now_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       


        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Foto diisi Dahulu ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                comboBox2.SelectedIndex = -1;
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Pilih printer terlebih dahulu ", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (comboBox3.SelectedIndex == -1)
                    {
                        MessageBox.Show("Pilih profil terlebih dahulu ", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        PrintDocument pd = new PrintDocument();
                        if (textBox2.Text == "1")
                        {
                            // Setting up for the specific profile
                            if (comboBox3.Text == "Default")
                            {
                                pd.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);
                            }
                            else if (comboBox3.Text == "Adjust Brightness")
                            {
                                pd.PrintPage += new PrintPageEventHandler(this.printDocument4_PrintPage);
                            }
                            pd.Print();
                            HistoryPrint4R(comboBox3.Text);
                            //printPreviewDialog1.Document = pd;
                            //printPreviewDialog1.ShowDialog();
                        }
                        else
                        {
                            pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
                            // Setting up for the specific profile
                            if (comboBox3.Text == "Default")
                            {
                                pd.PrintPage += new PrintPageEventHandler(this.printDocument2_PrintPage);
                            }
                            else if (comboBox3.Text == "Adjust Brightness")
                            {
                                pd.PrintPage += new PrintPageEventHandler(this.printDocument3_PrintPage);
                            }
                            pd.Print();
                            HistoryPrintA4(comboBox3.Text);
                            //printPreviewDialog1.Document = pd;
                            //printPreviewDialog1.ShowDialog();
                        }

                        // Common post-print actions
                        comboBox1.Items.Clear();
                        comboBox1.ResetText();
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                        buttobDeleteFalse();
                        button4.PerformClick();
                        int kondisi1 = 1;
                        TransfEventPrint1G(kondisi1.ToString());
                        MessageBox.Show("Dokumen berhasil diprint.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (pictureBox1.Image == null)
        //    {
        //        MessageBox.Show("Foto diisi Dahulu ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    else
        //    {
        //        comboBox2.SelectedIndex = -1;
        //        if (comboBox1.SelectedIndex == -1)
        //        {
        //            MessageBox.Show("Pilih printer terlebih dahulu ", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        else
        //        {
        //            int print1 = 1;
        //            int print2 = 2;

        //            if (textBox2.Text == print1.ToString())
        //            {
        //                if (comboBox3.SelectedIndex == -1)
        //                {
        //                    MessageBox.Show("Pilih profil terlebih dahulu ", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                }
        //                PrintDocument pd = new PrintDocument();
        //                if (comboBox3.Text == "Default")
        //                {
        //                    pd.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);
        //                    pd.Print();
        //                    HistoryPrint4R();
        //                    comboBox1.Items.Clear();
        //                    comboBox1.ResetText(); 
        //                    pictureBox1.Image.Dispose();
        //                    pictureBox1.Image = null;
        //                    buttobDeleteFalse();
        //                    buttobAddTrue();
        //                    button4.PerformClick();
        //                    int kondisi1 = 1;
        //                    TransfEventPrint1G(kondisi1.ToString());
        //                    MessageBox.Show("Dokumen berhasil diprint.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                else if (comboBox3.Text == "Adjust Brightness")
        //                {
        //                    pd.PrintPage += new PrintPageEventHandler(this.printDocument4_PrintPage);
        //                    pd.Print();
        //                    HistoryPrint4R();
        //                    comboBox1.Items.Clear();
        //                    comboBox1.ResetText(); 
        //                    pictureBox1.Image.Dispose();
        //                    pictureBox1.Image = null;
        //                    buttobDeleteFalse();
        //                    buttobAddTrue();
        //                    button4.PerformClick();
        //                    int kondisi1 = 1;
        //                    TransfEventPrint1G(kondisi1.ToString());
        //                    MessageBox.Show("Dokumen berhasil diprint.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //            }
        //            else
        //            {
        //                if(comboBox3.SelectedIndex == -1)
        //                {
        //                    MessageBox.Show("Pilih profil terlebih dahulu ", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                }
        //                PrintDocument pd = new PrintDocument();
        //                pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);

        //                if(comboBox3.Text == "Default")
        //                {
        //                    pd.PrintPage += new PrintPageEventHandler(this.printDocument2_PrintPage);
        //                    pd.Print();
        //                    //printPreviewDialog1.Document = pd;
        //                    //printPreviewDialog1.ShowDialog();
        //                    HistoryPrintA4();
        //                    comboBox1.Items.Clear();
        //                    comboBox1.ResetText(); 
        //                    pictureBox1.Image.Dispose();
        //                    pictureBox1.Image = null;
        //                    buttobDeleteFalse();
        //                    buttobAddTrue();
        //                    button4.PerformClick();
        //                    int kondisi1 = 1;
        //                    TransfEventPrint1G(kondisi1.ToString());
        //                    MessageBox.Show("Dokumen berhasil diprint.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                else if(comboBox3.Text == "Adjust Brightness")
        //                {
        //                    pd.PrintPage += new PrintPageEventHandler(this.printDocument3_PrintPage);
        //                    pd.Print();
        //                    //printPreviewDialog1.Document = pd;
        //                    //printPreviewDialog1.ShowDialog();
        //                    HistoryPrintA4();
        //                    comboBox1.Items.Clear();
        //                    comboBox1.ResetText(); 
        //                    pictureBox1.Image.Dispose();
        //                    pictureBox1.Image = null;
        //                    buttobDeleteFalse();
        //                    buttobAddTrue();
        //                    button4.PerformClick();
        //                    int kondisi1 = 1;
        //                    TransfEventPrint1G(kondisi1.ToString());
        //                    MessageBox.Show("Dokumen berhasil diprint.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //            }
        //        }

        //    }
        //}

        private void buttobDeleteFalse()
        {
            btn_Delete.Visible = false; 
        }
         

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            //StringFormat sf = new StringFormat(); 
            //sf.Alignment = StringAlignment.Center;
            //e.Graphics.DrawString(label1.Text, new Font("Montserrat", 12, FontStyle.Bold), Brushes.Black, 414, 35, sf);
            //StringFormat sff = new StringFormat(); 
            //sff.Alignment = StringAlignment.Center;
            //e.Graphics.DrawString(label2.Text, new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 415, 60, sff);
            //e.Graphics.DrawImage(picLogo1.Image, 5, 0, 100, 100);
            //e.Graphics.DrawImage(pictureBox1.Image, 5, 120, 790, 433);  

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            e.Graphics.DrawString(label1.Text, new Font("Montserrat", 12, FontStyle.Bold), Brushes.Black, 414, 35, sf);

            StringFormat sff = new StringFormat();
            sff.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(label2.Text, new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 415, 60, sff);
             

            // Check the value of logoValue
            if (logoValue == "1")
            {
                //AdjustPictureBoxSize(e.Graphics, "Persegi");
                //e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);

                if (jenisValue == "Persegi Panjang")
                {
                    // Handle case when logoValue is "1" and jenisValue is "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, jenisValue);
                    e.Graphics.DrawImage(pictureBox2.Image, 5, 0, pictureBox2.Width, pictureBox2.Height);

                    //MessageBox.Show("Persegi Panjang");
                }
                else
                {
                    // Handle case when logoValue is "1" and jenisValue is not "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, "Persegi");
                    e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);
                    //MessageBox.Show("Persegi");
                }


            }
            else if (logoValue == "2")
            {
                AdjustPictureBoxSize(e.Graphics, jenisValue); // Adjust the size based on jenisValue
                e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);

                // Adjust the coordinates based on jenisValue
                if (jenisValue == "Persegi")
                {
                    e.Graphics.DrawImage(pictureBox2.Image, 695, 0, pictureBox2.Width, pictureBox2.Height);
                }
                else if (jenisValue == "Persegi Panjang")
                { 
                    e.Graphics.DrawImage(pictureBox2.Image, 595, 0, pictureBox2.Width, pictureBox2.Height);
                }
            }


            e.Graphics.DrawImage(pictureBox1.Image, 7, 120, 790, 433);
        }

        private void printDocument12_PrintPage(object sender, PrintPageEventArgs e)
        {
            //StringFormat sf = new StringFormat(); 
            //sf.Alignment = StringAlignment.Center;
            //e.Graphics.DrawString(label1.Text, new Font("Montserrat", 12, FontStyle.Bold), Brushes.Black, 414, 35, sf);
            //StringFormat sff = new StringFormat(); 
            //sff.Alignment = StringAlignment.Center;
            //e.Graphics.DrawString(label2.Text, new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 415, 60, sff);
            //e.Graphics.DrawImage(picLogo1.Image, 5, 0, 100, 100);
            //e.Graphics.DrawImage(pictureBox1.Image, 5, 120, 790, 433);  

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            e.Graphics.DrawString(label1.Text, new Font("Montserrat", 12, FontStyle.Bold), Brushes.Black, 414, 35, sf);

            StringFormat sff = new StringFormat();
            sff.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(label2.Text, new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 415, 60, sff);


            // Check the value of logoValue
            if (logoValue == "1")
            {
                //AdjustPictureBoxSize(e.Graphics, "Persegi");
                //e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);

                if (jenisValue == "Persegi Panjang")
                {
                    // Handle case when logoValue is "1" and jenisValue is "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, jenisValue);
                    e.Graphics.DrawImage(pictureBox2.Image, 5, 0, pictureBox2.Width, pictureBox2.Height);

                    //MessageBox.Show("Persegi Panjang");
                }
                else
                {
                    // Handle case when logoValue is "1" and jenisValue is not "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, "Persegi");
                    e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);
                    //MessageBox.Show("Persegi");
                }


            }
            else if (logoValue == "2")
            {
                AdjustPictureBoxSize(e.Graphics, jenisValue); // Adjust the size based on jenisValue
                e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);

                // Adjust the coordinates based on jenisValue
                if (jenisValue == "Persegi")
                {
                    e.Graphics.DrawImage(pictureBox2.Image, 695, 0, pictureBox2.Width, pictureBox2.Height);
                }
                else if (jenisValue == "Persegi Panjang")
                {
                    e.Graphics.DrawImage(pictureBox2.Image, 595, 0, pictureBox2.Width, pictureBox2.Height);
                }
            }


            e.Graphics.DrawImage(pictureBox1.Image, 25, 120, 790, 433);
        }

        private void AdjustPictureBoxSize(Graphics graphics, string jenis)
        {
            // Check the value of jenis and adjust the size of PictureBox controls accordingly
            if (jenis == "persegi")
            {
                picLogo1.Size = new Size(100, 100);
                pictureBox2.Size = new Size(100, 100);
            }
            else if (jenis == "persegi panjang")
            {
                picLogo1.Size = new Size(200, 100);
                pictureBox2.Size = new Size(200, 100);
            }
            // Add more conditions as needed
        }

        private void printDocument3_PrintPage(object sender, PrintPageEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(label1.Text, new Font("Montserrat", 12, FontStyle.Bold), Brushes.Black, 414, 35, sf);
            StringFormat sff = new StringFormat();
            sff.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(label2.Text, new Font("Montserrat", 10, FontStyle.Bold), Brushes.Black, 415, 60, sff); 

            // Check the value of logoValue
            if (logoValue == "1")
            {
                //AdjustPictureBoxSize(e.Graphics, "persegi");
                //e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);

                if (jenisValue == "Persegi Panjang")
                {
                    // Handle case when logoValue is "1" and jenisValue is "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, jenisValue);
                    e.Graphics.DrawImage(pictureBox2.Image, 5, 0, pictureBox2.Width, pictureBox2.Height);

                    //MessageBox.Show("Persegi Panjang");
                }
                else
                {
                    // Handle case when logoValue is "1" and jenisValue is not "Persegi Panjang"
                    AdjustPictureBoxSize(e.Graphics, "Persegi");
                    e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);
                    //MessageBox.Show("Persegi");
                }

            }
            else if (logoValue == "2")
            {
                AdjustPictureBoxSize(e.Graphics, jenisValue); // Adjust the size based on jenisValue
                e.Graphics.DrawImage(picLogo1.Image, 5, 0, picLogo1.Width, picLogo1.Height);

                // Adjust the coordinates based on jenisValue
                if (jenisValue == "Persegi")
                {
                    e.Graphics.DrawImage(pictureBox2.Image, 695, 0, pictureBox2.Width, pictureBox2.Height);
                }
                else if (jenisValue == "Persegi Panjang")
                {
                    e.Graphics.DrawImage(pictureBox2.Image, 595, 0, pictureBox2.Width, pictureBox2.Height);
                }
            }

            //float contrast = 1.41f;
            float contrast = 1.00f;
            float gamma = 0.715f;
            float red = 0.56f;
            float green = 0.35f;
            float blue = 0.28f;

            ImageAttributes ia = new ImageAttributes();
            float[][] ptsarray = {
                        new float[] { contrast+red, 0f, 0f, 0f, 0f},
                        new float[] { 0f, contrast+green, 0f, 0f, 0f},
                        new float[] { 0f, 0f, contrast+blue, 0f, 0f},
                        new float[] { 0f, 0f,       0f, 1f, 0f},
                        new float[] {   0, 0,        0, 1f, 1f},
                };
            ia.ClearColorMatrix();
            ia.SetColorMatrix(new ColorMatrix(ptsarray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            ia.SetGamma(gamma, ColorAdjustType.Bitmap);
            e.Graphics.DrawImage(pictureBox1.Image, new Rectangle(7, 120, 790, 433), 0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height, GraphicsUnit.Pixel, ia);
            ia.Dispose();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 14, 43, 548, 312);
        }

        private void printDocument4_PrintPage(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(pictureBox1.Image, 14, 43, 548, 312);

            //Config 1

            //float contrast = 1.02f;
            //float gamma = 0.78f;

            //Config 2

            //float contrast = 1.41f;
            float contrast = 1.00f;
            float gamma = 0.715f;
            float red = 0.56f;
            float green = 0.35f;
            float blue = 0.28f;

            ImageAttributes ia = new ImageAttributes();
            float[][] ptsarray = {
                        new float[] { contrast+red, 0f, 0f, 0f, 0f},
                        new float[] { 0f, contrast+green, 0f, 0f, 0f},
                        new float[] { 0f, 0f, contrast+blue, 0f, 0f},
                        new float[] { 0f, 0f,       0f, 1f, 0f},
                        new float[] {   0, 0,        0, 1f, 1f},
                };
            ia.ClearColorMatrix();
            ia.SetColorMatrix(new ColorMatrix(ptsarray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            ia.SetGamma(gamma, ColorAdjustType.Bitmap);
            e.Graphics.DrawImage(pictureBox1.Image, new Rectangle(14, 43, 548, 312), 0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height, GraphicsUnit.Pixel, ia);
            ia.Dispose();
        }

        private void Form1Print_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label5;
            string dirlogo1 = dirLogo + "logo1.png";
            //string dirlogo1 = dir + "1160358.png";
            if (!Directory.Exists(dirlogo1))
            {
                picLogo1.Image = Image.FromFile(dirLogo + "logo1.png");
                pictureBox2.Image = Image.FromFile(dirLogo + "logo2.png");
            }

            buttobDeleteFalse();
        }

        private void savePDF4R()
        {
            string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\EksporPDF\Format-1\1-Gambar\4R\";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string existingPathName = dir;
            string notExistingFileName = dir + gabung1 + "_4R.pdf";
            if (Directory.Exists(existingPathName) && !File.Exists(notExistingFileName))
            {
                PrintDocument pdoc = new PrintDocument();
                pdoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                pdoc.PrinterSettings.PrintFileName = notExistingFileName;
                pdoc.PrinterSettings.PrintToFile = true;
                pdoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
                pdoc.DefaultPageSettings.Landscape = true;
                pdoc.PrintPage += printDocument1_PrintPage;
                pdoc.Print();
            }
        }

        private void HistoryPrint4R(string profile)
        {
            string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\History Print\Format-1\1-Gambar\4R\";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string existingPathName = dir;
            string notExistingFileName;

            if (profile == "Default")
            {
                notExistingFileName = dir + gabung1 + "_4R.pdf";
            }
            else if (profile == "Adjust Brightness")
            {
                notExistingFileName = dir + gabung1 + "_Adjust_Brightness_4R.pdf";
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
                pdoc.DefaultPageSettings.Landscape = true;
                pdoc.PrintPage += printDocument1_PrintPage;
                pdoc.Print();
            }
        }


        //private void HistoryPrint4R(string profile)
        //{
        //    string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\History Print\Format-1\1-Gambar\4R\";

        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }


        //    string existingPathName = dir;
        //    string notExistingFileName = dir + gabung1 + "_4R.pdf";
        //    if (Directory.Exists(existingPathName) && !File.Exists(notExistingFileName))
        //    {
        //        PrintDocument pdoc = new PrintDocument();
        //        pdoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
        //        pdoc.PrinterSettings.PrintFileName = notExistingFileName;
        //        pdoc.PrinterSettings.PrintToFile = true;
        //        pdoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        //        pdoc.DefaultPageSettings.Landscape = true;
        //        pdoc.PrintPage += printDocument1_PrintPage;
        //        pdoc.Print();
        //    }
        //}

        private void savePDFA4()
        {
            string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\EksporPDF\Format-1\1-Gambar\A4\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string existingPathName = dir;
            string notExistingFileName = dir + gabung1 + "_A4.pdf";

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

       
        private void HistoryPrintA4(string profile)
        {
            string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\History Print\Format-1\1-Gambar\A4\";
            

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string existingPathName = dir;
            string notExistingFileName;

            if (profile == "Default")
            {
                notExistingFileName = dir + gabung1 + "_A4.pdf";
            }
            else if (profile == "Adjust Brightness")
            {
                notExistingFileName = dir + gabung1 + "_Adjust_Brightness_A4.pdf";
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


        //private void HistoryPrintA4()
        //{
        //    string dir = @"D:\ZeusEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\History Print\Format-1\1-Gambar\A4\";
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }
        //    string existingPathName = dir;
        //    string notExistingFileName = dir + gabung1 + "_A4.pdf";

        //    if (Directory.Exists(existingPathName) && !File.Exists(notExistingFileName))
        //    {
        //        PrintDocument pdoc = new PrintDocument();
        //        pdoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
        //        pdoc.PrinterSettings.PrintFileName = notExistingFileName;
        //        pdoc.PrinterSettings.PrintToFile = true;
        //        pdoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        //        pdoc.DefaultPageSettings.Landscape = false;
        //        pdoc.PrintPage += printDocument2_PrintPage;
        //        pdoc.Print();
        //    }
        //}
    }
}
