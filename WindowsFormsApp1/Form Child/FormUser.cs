using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using WindowsFormsApp1.Form_Utama;
using CsvHelper.TypeConversion;
using System.Globalization;


namespace WindowsFormsApp1
{
    public partial class FormUser : Form
    {
        int Result, id, vCode;
        string sql, sql1, date, Code, dateDG, jk, rdbutton, idDokter, viewDokter;
        string str, str1, str2, str3, str4, str5, str6, str7, str8, str9;
        string strRm, strNama, strUmur, strNamaJalan, strDK, strKec, strKK, strRt, strRw, strTindakan, jam, tanggal;

        public delegate void TransfDelegate(String value);
        public event TransfDelegate TransfEvent;

        private bool isEditing = false;
        private bool isEditing1 = false;

        private bool anyChangesMade = false;

        private List<DataItem> dataList = new List<DataItem>();

        private FormDokter form2Instance; // Pastikan Anda memiliki instance Form2 di Form1

        // Simpan data terakhir ke file CSV
        string filePath1 = @"D:\GLEndoscope\Database\dataPasien\dataDefault.csv";

        private string filePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv";
        // Declare this at the top of your class, outside of any method
        private DataItem defaultPatient;




        public class DataItem
        {
            public int No { get; set; }
            public string Rm { get; set; }
            public string Nama { get; set; }
            public DateTime TanggalLahir { get; set; }
            public string Umur { get; set; }
            public string JenisKelamin { get; set; }
            public string Dokter { get; set; }
            public DateTime TanggalKunjungan { get; set; }
            public string Alamat { get; set; }
            public string JenisPemeriksaan { get; set; }
        }


       


        private void btn_DeleteForm_Click(object sender, EventArgs e)
        {
           // Memeriksa apakah txt_Rm telah diisi
            if (string.IsNullOrEmpty(txt_Rm.Text))
            {
                MessageBox.Show("Silakan pilih data yang ingin dihapus terlebih dahulu.", "Data Kosong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Keluar dari method karena data belum dipilih
            }

            string rmToDelete = txt_Rm.Text;


            // Ask for confirmation before deletion
            DialogResult result = MessageBox.Show("Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Find the index of the item with the matching RM
                int indexToDelete = dataList.FindIndex(item => item.Rm == rmToDelete);

                // Remove the item from the dataList
                dataList.RemoveAt(indexToDelete);

                UpdateNoValues();

                // Refresh the DataGridView
                RefreshDataGridView();

                // Clear the TextBoxes after deletion
                ClearTextBoxes();

                // Export to CSV
                ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");
                //ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv", dataList);

                // Set the editing flag to true
                isEditing1 = false;
            }
            else if (result == DialogResult.No)
            {
                // Set the editing flag to true
                isEditing1 = false;

                // Clear the TextBoxes after deletion
                ClearTextBoxes();
            } 

            txt_Rm.Enabled = true;
            //btn_Save.Enabled = true;
            DisableButtons();

           
        }


        private void UpdateNoValues()
        {
            // Iterate through the dataList and update the 'No' values
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].No = i + 1;
            }
        }

        public FormUser()
        {
            InitializeComponent();
            //txt_Rm.Validated += new EventHandler(textBox_Validated);
            //txt_Nama.Validated += new EventHandler(textBox_Validated);
            //txt_Tindakan.Validated += new EventHandler(textBox_Validated); 
            //comboBoxDokter.Validated += new EventHandler(combo1_Validated);

            // Menentukan kolom-kolom di DataGridView
            dataGridView1.Columns.Add("No", "No");
            dataGridView1.Columns.Add("Rm", "Rm");
            dataGridView1.Columns.Add("Nama", "Nama");
            dataGridView1.Columns.Add("Tanggal Lahir", "Tanggal Lahir");
            dataGridView1.Columns.Add("Umur", "Umur");
            dataGridView1.Columns.Add("JenisKelamin", "Jenis Kelamin");
            dataGridView1.Columns.Add("selectedDokter", "Dokter");
            dataGridView1.Columns.Add("Tanggal Kunjungan", "Tanggal Kunjungan");
            dataGridView1.Columns.Add("alamat", "Alamat");
            dataGridView1.Columns.Add("jenis Pemeriksaan", "Jenis Pemeriksaan");





            //txt_Rm.TextChanged += textBox_TextChanged; 
            //txt_Nama.TextChanged += textBox_TextChanged; ;
            //richTextBox1.TextChanged += textBox_TextChanged; ;
            //dateTimePicker1.ValueChanged += Control_ValueChanged;
            //dateTimePicker2.ValueChanged += Control_ValueChanged;
            //radioButtonPria.CheckedChanged += radioButton_CheckedChanged;
            //radioButtonWanita.CheckedChanged += radioButton_CheckedChanged;
            //comboBoxDokter.SelectedIndexChanged += Control_SelectedIndexChanged;




            //baru 

            // Tambahkan event handler untuk setiap kontrol
            txt_Rm.TextChanged += new EventHandler(AnyFieldChanged);
            txt_Nama.TextChanged += new EventHandler(AnyFieldChanged);
            dateTimePicker1.ValueChanged += new EventHandler(AnyFieldChanged);
            txt_Umur.TextChanged += new EventHandler(AnyFieldChanged);
            radioButtonPria.CheckedChanged += new EventHandler(AnyFieldChanged);
            radioButtonWanita.CheckedChanged += new EventHandler(AnyFieldChanged);
            comboBoxDokter.SelectedIndexChanged += new EventHandler(AnyFieldChanged);
            dateTimePicker2.ValueChanged += new EventHandler(AnyFieldChanged);
            richTextBox1.TextChanged += new EventHandler(AnyFieldChanged);
            txt_Tindakan.TextChanged += new EventHandler(AnyFieldChanged);

            // Awalnya tombol tidak aktif
            btn_Save.Enabled = false;
            btn_Cancel.Enabled = false;

            //batu 

            // Inisialisasi status tombol "Save"
            //UpdateSaveButtonStatus();

            // Menambahkan penangan acara CellDoubleClick
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

            // Menambahkan penangan acara ValueChanged pada DateTimePicker
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;

            // Inisialisasi instance Form2
            form2Instance = new FormDokter();

            // Subscribe ke event FormClosedEvent di Form2
            form2Instance.FormClosedEvent += Form2_FormClosedEvent;

            // Panggil fungsi ini pada saat aplikasi dimulai
            PopulateComboBox(" D:\\GLEndoscope\\Database\\dataDokter\\namaDokter.csv");

            comboBoxDokter.KeyPress += new KeyPressEventHandler(comboBoxDokter_KeyPress);

        }

        //baru
        private void AnyFieldChanged(object sender, EventArgs e)
        {
            if (!isEditing1) // Pastikan tidak sedang dalam mode edit dari double-click
            {
                EnableButtons1();
            }
        }

        private void EnableButtons1()
        {

            // Periksa apakah semua kontrol telah diisi
            bool allFieldsFilled = !string.IsNullOrWhiteSpace(txt_Rm.Text) &&
                                   !string.IsNullOrWhiteSpace(txt_Nama.Text) &&
                                   dateTimePicker1.Value != null &&
                                   !string.IsNullOrWhiteSpace(txt_Umur.Text) &&
                                   (radioButtonPria.Checked || radioButtonWanita.Checked) &&
                                   comboBoxDokter.SelectedItem != null &&
                                   dateTimePicker2.Value != null &&
                                   !string.IsNullOrWhiteSpace(richTextBox1.Text) &&
                                   !string.IsNullOrWhiteSpace(txt_Tindakan.Text);

            // Aktifkan atau nonaktifkan tombol berdasarkan hasil pengecekan
            btn_Save.Enabled = allFieldsFilled;
            btn_Cancel.Enabled = allFieldsFilled;
        }
        //baru

        private void Form2_FormClosedEvent()
        {
            // Panggil kembali fungsi untuk memperbarui ComboBox
            PopulateComboBox(" D:\\GLEndoscope\\Database\\dataDokter\\namaDokter.csv");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            DateTime tanggalLahir = dateTimePicker1.Value;
            string umur = HitungUmur(tanggalLahir);
            txt_Umur.Text = umur;

        }

        //private string HitungUmur(DateTime tanggalLahir)
        //{
        //    DateTime hariIni = DateTime.Today; 

        //    int umurTahun = hariIni.Year - tanggalLahir.Year;
        //    int umurBulan = 0;
        //    int umurHari = 0;

        //    if (hariIni.Month < tanggalLahir.Month || (hariIni.Month == tanggalLahir.Month && hariIni.Day < tanggalLahir.Day))
        //    {
        //        umurTahun--;
        //        umurBulan = 12 - tanggalLahir.Month + hariIni.Month - 1;

        //        if (hariIni.Day < tanggalLahir.Day)
        //        {
        //            DateTime lastMonth = hariIni.AddMonths(-1);
        //            umurHari = (int)(hariIni - lastMonth.AddMonths(-1)).TotalDays;
        //        }
        //    }
        //    else
        //    {
        //        umurBulan = hariIni.Month - tanggalLahir.Month;

        //        if (hariIni.Day >= tanggalLahir.Day)
        //        {
        //            umurHari = hariIni.Day - tanggalLahir.Day;
        //        }
        //        else
        //        {
        //            DateTime lastMonth = hariIni.AddMonths(-1);
        //            umurHari = (int)(hariIni - lastMonth.AddMonths(-1)).TotalDays;
        //        }
        //    }

        //    return $"{umurTahun}Th {umurBulan}Bln {umurHari}Hr";
        //}

        private string HitungUmur(DateTime tanggalLahir)
        {
            DateTime hariIni = DateTime.Today;

            // Hitung selisih tahun
            int umurTahun = hariIni.Year - tanggalLahir.Year;

            // Jika bulan hari ini kurang dari bulan tanggal lahir, kurangi satu tahun
            if (hariIni.Month < tanggalLahir.Month || (hariIni.Month == tanggalLahir.Month && hariIni.Day < tanggalLahir.Day))
            {
                umurTahun--;
            }

            // Hitung selisih bulan
            int umurBulan = hariIni.Month - tanggalLahir.Month;
            if (umurBulan < 0)
            {
                umurBulan += 12;
            }

            // Hitung selisih hari
            int umurHari = hariIni.Day - tanggalLahir.Day;
            if (umurHari < 0)
            {
                // Hitung jumlah hari dalam bulan sebelumnya
                DateTime bulanLalu = hariIni.AddMonths(-1);
                umurHari += DateTime.DaysInMonth(bulanLalu.Year, bulanLalu.Month);
            }

            return $"{umurTahun}Th {umurBulan}Bln {umurHari}Hr";
        }

        //private void SetUmur()
        //{
        //    DateTime tanggalLahir = dateTimePicker1.Value;
        //    DateTime tanggalSekarang = DateTime.Now;

        //    string umur;
        //    if (tanggalLahir > tanggalSekarang)
        //    {
        //        umur = "0 hari 0 bulan 0 tahun";
        //    }
        //    else
        //    {
        //        TimeSpan selisihWaktu = tanggalSekarang - tanggalLahir;
        //        int umurTahun = (int)(selisihWaktu.Days / 365.25);

        //        // Hitung umur dalam format hari, bulan, tahun
        //        int umurHari = selisihWaktu.Days % 365;
        //        int umurBulan = (int)((selisihWaktu.Days % 365.25) / 30.44);

        //        umur = string.Format("{0} hari {1} bulan {2} tahun", umurHari, umurBulan, umurTahun);
        //    }

        //    // Tampilkan umur di TextBox
        //    txt_Umur.Text = umur;
        //}

        //private void SetInitialUmur()
        //{
        //    DateTime tanggalLahir = dateTimePicker1.Value;
        //    DateTime tanggalSekarang = DateTime.Now;

        //    string umur;
        //    if (tanggalLahir > tanggalSekarang)
        //    {
        //        umur = "0 hari 0 bulan 0 tahun";
        //    }
        //    else
        //    {
        //        TimeSpan selisihWaktu = tanggalSekarang - tanggalLahir;
        //        int umurTahun = (int)(selisihWaktu.Days / 365.25);

        //        // Hitung umur dalam format hari, bulan, tahun
        //        int umurHari = selisihWaktu.Days % 365;
        //        int umurBulan = (int)((selisihWaktu.Days % 365.25) / 30.44);

        //        umur = string.Format("{0} hari {1} bulan {2} tahun", umurHari, umurBulan, umurTahun);
        //    }

        //    // Tampilkan umur di TextBox
        //    txt_Umur.Text = umur;
        //}




        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            int rowIndex = e.RowIndex;

            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[rowIndex];

                // Check if the required cells are not null
                object rmCellValue = selectedRow.Cells["Rm"].Value;
                object namaCellValue = selectedRow.Cells["Nama"].Value;
                object tanggalLahirCellValue = selectedRow.Cells["Tanggal Lahir"].Value;
                object umurCellValue = selectedRow.Cells["Umur"].Value;
                object jenisKelaminCellValue = selectedRow.Cells["JenisKelamin"].Value;
                object selectedDokterCellValue = selectedRow.Cells["selectedDokter"].Value;
                object tanggalKunjunganCellValue = selectedRow.Cells["Tanggal Kunjungan"].Value;
                object alamatCellValue = selectedRow.Cells["Alamat"].Value;
                object jenisPemeriksaanCellValue = selectedRow.Cells["Jenis Pemeriksaan"].Value;

                if (rmCellValue != null && namaCellValue != null && tanggalLahirCellValue != null &&
                    umurCellValue != null && jenisKelaminCellValue != null && selectedDokterCellValue != null &&
                    tanggalKunjunganCellValue != null && alamatCellValue != null && jenisPemeriksaanCellValue != null)
                {
                    string rm = rmCellValue.ToString();
                    string nama = namaCellValue.ToString();

                    // Check if the value can be parsed to DateTime
                    DateTime tanggalLahir;
                    if (DateTime.TryParse(tanggalLahirCellValue.ToString(), out tanggalLahir))
                    {
                        string umur = umurCellValue.ToString();
                        string jenisKelamin = jenisKelaminCellValue.ToString();
                        string dokter = selectedDokterCellValue.ToString();

                        // New fields
                        DateTime tanggalKunjungan;
                        if (DateTime.TryParse(tanggalKunjunganCellValue.ToString(), out tanggalKunjungan))
                        {
                            dateTimePicker2.Value = tanggalKunjungan;
                        }
                        else
                        {
                            // Handle the case where parsing to DateTime fails for "Tanggal Kunjungan"
                            // You may want to show an error message or take appropriate action
                        }

                        txt_Rm.Text = rm;
                        txt_Nama.Text = nama;
                        dateTimePicker1.Value = tanggalLahir;
                        txt_Umur.Text = umur;

                        if (jenisKelamin == "Laki - laki")
                        {
                            radioButtonPria.Checked = true;
                            radioButtonWanita.Checked = false;
                        }
                        else
                        {
                            radioButtonPria.Checked = false;
                            radioButtonWanita.Checked = true;
                        }

                        comboBoxDokter.SelectedItem = dokter;
                        // Set selected value for comboBoxDokter
                        comboBoxDokter.SelectedItem = selectedDokterCellValue;


                        // New fields
                        richTextBox1.Text = alamatCellValue.ToString();
                        txt_Tindakan.Text = jenisPemeriksaanCellValue.ToString();

                        // Set the editing flag to true
                        isEditing1 = true;

                        // Change the text of the btn_Save button to indicate editing
                        //btn_Save.Text = "Ubah";

                        button2.Enabled = true;

                        txt_Rm.Enabled = false;

                        EnableButtons();

                        btn_Save.Enabled = false;
                    }
                    else
                    {
                        // Handle the case where parsing to DateTime fails for "Tanggal Lahir"
                        // You may want to show an error message or take appropriate action
                    }
                }
            }
        }

        // Memeriksa apakah data sudah diisi dengan benar sebelum mengaktifkan tombol "Save"
        //private void UpdateSaveButtonStatus()
        //{
        //    // Periksa apakah semua TextBox telah diisi
        //    bool allTextBoxesFilled = !String.IsNullOrEmpty(txt_Rm.Text) &&
        //                              !String.IsNullOrEmpty(txt_Nama.Text) &&
        //                              //!String.IsNullOrEmpty(txt_Tindakan.Text) &&
        //                              !String.IsNullOrEmpty(richTextBox1.Text);

        //    // Periksa apakah RadioButton telah dipilih
        //    bool radioButtonSelected = radioButtonPria.Checked || radioButtonWanita.Checked;

        //    // Periksa apakah TanggalLahir dan TanggalKunjungan sudah diisi
        //    bool tanggalLahirFilled = dateTimePicker1.Value != DateTime.MinValue;
        //    //bool tanggalKunjunganFilled = dateTimePicker2.Value != DateTime.MinValue;

        //    // Periksa apakah NamaDokter sudah dipilih
        //    bool namaDokterSelected = comboBoxDokter.SelectedItem != null;

        //    // Aktifkan tombol "Save" hanya jika semua data yang diperlukan telah diisi
        //    btn_Save.Enabled = allTextBoxesFilled && radioButtonSelected &&
        //                          tanggalLahirFilled &&
        //                          namaDokterSelected;
        //    btn_Cancel.Enabled = btn_Save.Enabled;
        //}

        //private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        //// Event handler untuk memperbarui status tombol "Save" setiap kali isi dari TextBox atau RadioButton berubah
        //private void textBox_TextChanged(object sender, EventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        //private void radioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        //private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        

        //private void Control_ValueChanged(object sender, EventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        //private void RadioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}

        //private void Control_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    UpdateSaveButtonStatus();
        //}


        private void combo1_Validated(object sender, EventArgs e)
        {
            var tb = (ComboBox)sender;
            if (string.IsNullOrEmpty(tb.Text))
            {
                errorProvider.SetError(tb, "error");
            }
        }

        private void textBox_Validated(object sender, EventArgs e)
        {
            var tb = (System.Windows.Forms.TextBox)sender;
            if (string.IsNullOrEmpty(tb.Text))
            {
                errorProvider.SetError(tb, "error");
            }
        }

        private void clearTextbox()
        {
            txt_Nama.Clear();
            txt_Tindakan.Clear();
            txt_Rm.Clear();
            radioButtonPria.Checked = false;
            radioButtonWanita.Checked = false;
            txt_Umur.Clear();
            comboBoxDokter.SelectedIndex = -1;
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker1.Value = DateTime.Today;
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label1;
            DisableButtons();


            button2.Enabled = false;
            //btn_DeleteForm.Enabled = false; 
            this.dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Montserrat", 12);

            dateTimePicker1.MaxDate = DateTime.Today.AddDays(1).AddTicks(-1);
            dateTimePicker1.Value = DateTime.Today.Date;

            //LoadDataFromCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");

            string filePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv";

            if (File.Exists(filePath))
            {
                LoadDataFromCSV(filePath);
            }
            else
            {
                // Handle the case where the file does not exist
                // MessageBox.Show("The CSV file does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Menampilkan data di datagridview
            RefreshDataGridView();

            // Attach event handlers
            txt_Rm.TextChanged += TextChangedHandler;
            txt_Nama.TextChanged += TextChangedHandler;
            dateTimePicker1.ValueChanged += TextChangedHandler;
            dateTimePicker2.ValueChanged += TextChangedHandler;
            radioButtonPria.CheckedChanged += TextChangedHandler;
            radioButtonWanita.CheckedChanged += TextChangedHandler;
            comboBoxDokter.SelectedIndexChanged += TextChangedHandler;
            richTextBox1.TextChanged += TextChangedHandler;
            txt_Tindakan.TextChanged += TextChangedHandler;

            // Initial state check
            UpdateButtonStates();

            string dataPasienPath = @"D:\GLEndoscope\Database\dataPasien\";
            string dataDokterPath = @"D:\GLEndoscope\Database\dataDokter\";

            // Cek apakah folder sudah ada sebelum membuatnya
            if (!Directory.Exists(dataPasienPath))
            {
                // Buat folder dataPasien jika belum ada
                Directory.CreateDirectory(dataPasienPath);
            }

            if (!Directory.Exists(dataDokterPath))
            {
                // Buat folder dataDokter jika belum ada
                Directory.CreateDirectory(dataDokterPath);
            }


            comboBoxDokter.DropDownStyle = ComboBoxStyle.DropDownList;

            DisableButtons();

        }

        private void DisableButtons()
        {
            btn_Cancel.Enabled = false;
            button5.Enabled = false;
            btn_DeleteForm.Enabled = false;
            btn_Save.Enabled = false;
            button4.Enabled = true;
            button2.Enabled = false;
        }

        private void EnableButtons()
        {
            btn_Cancel.Enabled = true;
            button5.Enabled = true;
            btn_DeleteForm.Enabled = true;
            btn_Save.Enabled = true;
            button4.Enabled = false;
            button2.Enabled = true;
        }


        private void dateTimePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Mencegah pengeditan manual melalui keyboard
            e.Handled = true;
        }

        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Mencegah pengeditan manual melalui keyboard
            e.Handled = true;
        }

        private void TextChangedHandler(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            //bool anyEmpty = string.IsNullOrEmpty(txt_Rm.Text) ||
            //                string.IsNullOrEmpty(txt_Nama.Text) ||
            //                string.IsNullOrEmpty(richTextBox1.Text) ||
            //                string.IsNullOrEmpty(txt_Tindakan.Text) ||
            //                comboBoxDokter.SelectedItem == null;

            //// Jika sedang dalam mode pengeditan dan tidak ada perubahan, maka atur btn_Save menjadi tidak aktif
            //if (isEditing && !anyChangesMade)
            //{
            //    btn_Save.Enabled = false;

            //    //bool allFieldsFilled = !string.IsNullOrEmpty(txt_Rm.Text) &&
            //    //           !string.IsNullOrEmpty(txt_Nama.Text) &&
            //    //           !string.IsNullOrEmpty(richTextBox1.Text) &&
            //    //           !string.IsNullOrEmpty(txt_Tindakan.Text) &&
            //    //           comboBoxDokter.SelectedItem != null &&
            //    //           (radioButtonPria.Checked || radioButtonWanita.Checked); // Pastikan salah satu radio button terpilih

            //    //btn_Save.Enabled = allFieldsFilled;
            //    //btn_DeleteForm.Enabled = allFieldsFilled;
            //    //button5.Enabled = allFieldsFilled;
            //    //btn_Cancel.Enabled = allFieldsFilled;
            //}
            //else
            //{
            //    // Atur btn_Save sesuai dengan status apakah ada kolom yang belum diisi atau tidak
            //    btn_Save.Enabled = !anyEmpty;
            //}

            //// Atur btn_DeleteForm, button5, dan btn_Cancel seperti sebelumnya
            //btn_DeleteForm.Enabled = !anyEmpty;
            //button5.Enabled = !anyEmpty;
            //btn_Cancel.Enabled = !anyEmpty; 
        }


        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            anyChangesMade = true;
            UpdateButtonStates(); // Panggil UpdateButtonStates setelah ada perubahan

            //UpdateSaveButtonStatus();
        }


        private void LoadDataFromCSV(string filePath)
        {
            // Clear existing data in dataList before loading from CSV
            dataList.Clear();

            // Load data from CSV
            LoadFromCSV(filePath);
        }



        private void LoadFromCSV(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.HasHeaderRecord = true; // Set ke true jika file CSV Anda memiliki header
                    csv.Configuration.IgnoreBlankLines = true;

                    while (csv.Read())
                    {
                        // Extract values from the CSV columns
                        string rm = csv.GetField<string>(1);
                        string nama = csv.GetField<string>(2);
                        DateTime tanggalLahir = csv.GetField<DateTime>(3);
                        string umur = HitungUmur(tanggalLahir);
                        string selectedDokter = csv.GetField<string>(6);
                        DateTime tanggalKunjungan = csv.GetField<DateTime>(7);
                        string jenisKelamin = csv.GetField<string>(5);
                        string alamat = csv.GetField<string>(8);
                        string jenisPemeriksaan = csv.GetField<string>(9);
                        //selectedDokter = $"\"{selectedDokter}\"";   
                        // Add the data to the dataList
                        dataList.Add(new DataItem
                        {
                            No = dataList.Count + 1,
                            Rm = rm,
                            Nama = nama,
                            TanggalLahir = tanggalLahir,
                            Umur = umur,
                            JenisKelamin = jenisKelamin,
                            Dokter = selectedDokter,
                            TanggalKunjungan = tanggalKunjungan,
                            Alamat = alamat,
                            JenisPemeriksaan = jenisPemeriksaan,
                            // Add other properties similarly
                        });
                    }
                }

                // Refresh the DataGridView after loading data
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from CSV: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshDataGridView()
        {
            // Membersihkan datagridview
            dataGridView1.Rows.Clear();

            // Menambahkan data dari list ke datagridview
            foreach (var data in dataList)
            {
                dataGridView1.Rows.Add(data.No, data.Rm, data.Nama, data.TanggalLahir.ToShortDateString(), data.Umur, data.JenisKelamin, data.Dokter, data.TanggalKunjungan.ToShortDateString(), data.Alamat, data.JenisPemeriksaan);
            }
        }



        private void ExportRowToCSV(DataGridViewRow row)
        {
            // Create a StringBuilder to store the CSV data
            StringBuilder csvContent = new StringBuilder();

            // Add column headers to the CSV
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                //csvContent.Append($"\"{column.HeaderText}\",");
                csvContent.Append(column.HeaderText + ",");
            }
            csvContent.AppendLine();

            // Add data from the selected row to the CSV
            foreach (DataGridViewCell cell in row.Cells)
            {
                string cellValue = cell.Value != null ? cell.Value.ToString() : ""; // Handle null values
                if (cellValue.Contains(","))
                {
                    csvContent.Append($"\"{cellValue}\","); // Surround cell value with quotes if it contains comma
                }
                else
                {
                    csvContent.Append($"{cellValue},"); // Otherwise, add cell value as is
                }
            }
            csvContent.AppendLine();

            // Specify the path for the CSV file
            string csvFilePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataDefault.csv";

            // Write the CSV content to the file
            File.WriteAllText(csvFilePath, csvContent.ToString());
        }

        private string RemoveExtraSpaces(string input)
        {
            return string.Join(" ", input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        //private void button5_Click_1(object sender, EventArgs e)
        //{
        //    // Memeriksa apakah txt_Rm telah diisi
        //    if (string.IsNullOrEmpty(txt_Rm.Text))
        //    {
        //        MessageBox.Show("Silakan pilih data yang ingin diperbarui terlebih dahulu.", "Data Belum Dipilih", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return; // Keluar dari method karena data belum dipilih
        //    }

        //    // Validasi setiap entri data
        //    string rm = txt_Rm.Text.Trim();
        //    if (string.IsNullOrEmpty(rm))
        //    {
        //        MessageBox.Show("No. RM harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string nama = RemoveExtraSpaces(txt_Nama.Text);
        //    if (string.IsNullOrEmpty(nama))
        //    {
        //        MessageBox.Show("Nama harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    DateTime tanggalLahir = dateTimePicker1.Value;
        //    DateTime tanggalKunjungan = dateTimePicker2.Value.Date;

        //    string umur = HitungUmur(tanggalLahir);
        //    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";

        //    string alamat = RemoveExtraSpaces(richTextBox1.Text);
        //    if (string.IsNullOrEmpty(alamat))
        //    {
        //        MessageBox.Show("Alamat harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string jenisPemeriksaan = txt_Tindakan.Text.Trim();
        //    if (string.IsNullOrEmpty(jenisPemeriksaan))
        //    {
        //        MessageBox.Show("Jenis Pemeriksaan harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string selectedDokter = comboBoxDokter.SelectedItem?.ToString().Trim() ?? "";
        //    if (string.IsNullOrEmpty(selectedDokter))
        //    {
        //        MessageBox.Show("Dokter harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Find the index of the selected row in the DataGridView
        //    int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

        //    // Get the rm and tanggalKunjungan values from the selected row
        //    string rmFromGrid = dataGridView1.Rows[selectedRowIndex].Cells["RM"].Value.ToString();
        //    DateTime tanggalKunjunganFromGrid = Convert.ToDateTime(dataGridView1.Rows[selectedRowIndex].Cells["Tanggal Kunjungan"].Value);

        //    // Update the existing record in the dataList
        //    DataItem existingItem = dataList.Find(item => item.Rm == rmFromGrid && item.TanggalKunjungan == tanggalKunjunganFromGrid);
        //    if (existingItem != null)
        //    {
        //        // Perbarui data item yang ada
        //        existingItem.Nama = nama;
        //        existingItem.TanggalLahir = tanggalLahir;
        //        existingItem.Umur = umur;
        //        existingItem.JenisKelamin = jenisKelamin;
        //        existingItem.Dokter = selectedDokter;
        //        existingItem.TanggalKunjungan = tanggalKunjungan;
        //        existingItem.Alamat = alamat;
        //        existingItem.JenisPemeriksaan = jenisPemeriksaan;

        //        // Tampilkan pesan keberhasilan
        //        MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Data tidak ditemukan untuk diubah.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //    RefreshDataGridView();

        //    // Export to CSV
        //    ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");
        //    ExportLastRowToCSV(filePath1, dataList.LastOrDefault());

        //    // Clear the form fields
        //    ClearTextBoxes();

        //    txt_Rm.Enabled = true;

        //    DisableButtons();
        //    isEditing = false;
        //}



        private void button5_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Rm.Text))
            {
                MessageBox.Show("Silakan pilih data yang ingin diperbarui terlebih dahulu.", "Data Belum Dipilih", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string rm = txt_Rm.Text.Trim();
            if (string.IsNullOrEmpty(rm))
            {
                MessageBox.Show("No. RM harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nama = RemoveExtraSpaces(txt_Nama.Text);
            if (string.IsNullOrEmpty(nama))
            {
                MessageBox.Show("Nama harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tanggalLahir = dateTimePicker1.Value;
            DateTime tanggalKunjungan = dateTimePicker2.Value.Date;

            string umur = HitungUmur(tanggalLahir);
            string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";

            string alamat = RemoveExtraSpaces(richTextBox1.Text);
            if (string.IsNullOrEmpty(alamat))
            {
                MessageBox.Show("Alamat harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jenisPemeriksaan = txt_Tindakan.Text.Trim();
            if (string.IsNullOrEmpty(jenisPemeriksaan))
            {
                MessageBox.Show("Jenis Pemeriksaan harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedDokter = comboBoxDokter.SelectedItem?.ToString().Trim() ?? "";
            if (string.IsNullOrEmpty(selectedDokter))
            {
                MessageBox.Show("Dokter harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataItem existingItem = dataList.Find(item => item.Rm == rm);

            if (existingItem != null)
            {
                string oldNama = existingItem.Nama;
                existingItem.Nama = nama;
                existingItem.TanggalLahir = tanggalLahir;
                existingItem.Umur = umur;
                existingItem.JenisKelamin = jenisKelamin;
                existingItem.Dokter = selectedDokter;
                existingItem.TanggalKunjungan = tanggalKunjungan;
                existingItem.Alamat = alamat;
                existingItem.JenisPemeriksaan = jenisPemeriksaan;

                if (!string.Equals(oldNama, nama, StringComparison.OrdinalIgnoreCase))
                {
                    string oldFolderPath = $@"D:\GLEndoscope\{tanggalKunjungan:yyyy}\{tanggalKunjungan:MMMM}\{tanggalKunjungan:ddMMyyyy}\{rm}-{oldNama}";
                    string newFolderPath = $@"D:\GLEndoscope\{tanggalKunjungan:yyyy}\{tanggalKunjungan:MMMM}\{tanggalKunjungan:ddMMyyyy}\{rm}-{nama}";

                    if (Directory.Exists(oldFolderPath))
                    {
                        Directory.Move(oldFolderPath, newFolderPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }
                }

                MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan untuk diubah.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RefreshDataGridView();

            ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");
            //ExportLastRowToCSV(filePath1, dataList.LastOrDefault());

            ClearTextBoxes();
            txt_Rm.Enabled = true;

            DisableButtons();
            isEditing = false;
        }







        //kode paling terakhir. hanya error pada setdefault ketika sudah diubah
        //private void button5_Click_1(object sender, EventArgs e)
        //{
        //    // Memeriksa apakah txt_Rm telah diisi
        //    if (string.IsNullOrEmpty(txt_Rm.Text))
        //    {
        //        MessageBox.Show("Silakan pilih data yang ingin diperbarui terlebih dahulu.", "Data Belum Dipilih", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return; // Keluar dari method karena data belum dipilih
        //    }

        //    // Validasi setiap entri data
        //    string rm = txt_Rm.Text.Trim(); // No RM
        //    if (string.IsNullOrEmpty(rm))
        //    {
        //        MessageBox.Show("No. RM harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string nama = RemoveExtraSpaces(txt_Nama.Text); // Nama baru
        //    if (string.IsNullOrEmpty(nama))
        //    {
        //        MessageBox.Show("Nama harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    DateTime tanggalLahir = dateTimePicker1.Value;
        //    DateTime tanggalKunjungan = dateTimePicker2.Value.Date;

        //    string umur = HitungUmur(tanggalLahir);
        //    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";

        //    string alamat = RemoveExtraSpaces(richTextBox1.Text);
        //    if (string.IsNullOrEmpty(alamat))
        //    {
        //        MessageBox.Show("Alamat harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string jenisPemeriksaan = txt_Tindakan.Text.Trim();
        //    if (string.IsNullOrEmpty(jenisPemeriksaan))
        //    {
        //        MessageBox.Show("Jenis Pemeriksaan harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string selectedDokter = comboBoxDokter.SelectedItem?.ToString().Trim() ?? "";
        //    if (string.IsNullOrEmpty(selectedDokter))
        //    {
        //        MessageBox.Show("Dokter harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Cari item berdasarkan No RM di dalam dataList
        //    DataItem existingItem = dataList.Find(item => item.Rm == rm);

        //    if (existingItem != null)
        //    {
        //        // Simpan nama lama sebelum diubah
        //        string oldNama = existingItem.Nama;

        //        // Perbarui data item yang ada
        //        existingItem.Nama = nama;
        //        existingItem.TanggalLahir = tanggalLahir;
        //        existingItem.Umur = umur;
        //        existingItem.JenisKelamin = jenisKelamin;
        //        existingItem.Dokter = selectedDokter;
        //        existingItem.TanggalKunjungan = tanggalKunjungan;
        //        existingItem.Alamat = alamat;
        //        existingItem.JenisPemeriksaan = jenisPemeriksaan;

        //        // Jika nama pasien berubah, rename folder lama ke nama baru
        //        if (!string.Equals(oldNama, nama, StringComparison.OrdinalIgnoreCase))
        //        {
        //            // Folder lama berdasarkan nama lama
        //            string baseOldFolderPath = $@"D:\GLEndoscope\{tanggalKunjungan:yyyy}\{tanggalKunjungan:MMMM}\{tanggalKunjungan:ddMMyyyy}";
        //            string searchPattern = $"{rm}-{oldNama}";

        //            // Dapatkan semua folder yang memiliki nama yang sesuai dengan pattern
        //            string[] oldFolders = Directory.GetDirectories(baseOldFolderPath, $"*{searchPattern}*");

        //            foreach (string oldFolderPath in oldFolders)
        //            {
        //                // Folder baru berdasarkan nama baru
        //                string newFolderPath = oldFolderPath.Replace($"{rm}-{oldNama}", $"{rm}-{nama}");

        //                // Jika folder lama ada dan folder baru belum ada, rename folder menjadi nama baru
        //                if (!Directory.Exists(newFolderPath))
        //                {
        //                    try
        //                    {
        //                        Directory.Move(oldFolderPath, newFolderPath);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show($"Gagal merubah nama folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    }
        //                }
        //            }
        //        }


        //        // Tampilkan pesan keberhasilan
        //        MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Data tidak ditemukan untuk diubah.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //    RefreshDataGridView();

        //    ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");
        //    ExportLastRowToCSV(filePath1, existingItem);

        //    // Clear the form fields
        //    ClearTextBoxes();

        //    // Refresh the DataGridView after saving or updating
        //    RefreshDataGridView();

        //    txt_Rm.Enabled = true;

        //    DisableButtons();
        //    isEditing = false;
        //    isEditing1 = false;
        //}



        //private void button5_Click_1(object sender, EventArgs e)
        //{
        //    // Memeriksa apakah txt_Rm telah diisi
        //    if (string.IsNullOrEmpty(txt_Rm.Text))
        //    {
        //        MessageBox.Show("Silakan pilih data yang ingin diperbarui terlebih dahulu.", "Data Belum Dipilih", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return; // Keluar dari method karena data belum dipilih
        //    }

        //    // Validasi setiap entri data
        //    string rm = txt_Rm.Text.Trim();
        //    if (string.IsNullOrEmpty(rm))
        //    {
        //        MessageBox.Show("No. RM harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string nama = RemoveExtraSpaces(txt_Nama.Text);
        //    if (string.IsNullOrEmpty(nama))
        //    {
        //        MessageBox.Show("Nama harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    DateTime tanggalLahir = dateTimePicker1.Value;
        //    DateTime tanggalKunjungan = dateTimePicker2.Value.Date;

        //    string umur = HitungUmur(tanggalLahir);
        //    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";

        //    string alamat = RemoveExtraSpaces(richTextBox1.Text);
        //    if (string.IsNullOrEmpty(alamat))
        //    {
        //        MessageBox.Show("Alamat harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string jenisPemeriksaan = txt_Tindakan.Text.Trim();
        //    if (string.IsNullOrEmpty(jenisPemeriksaan))
        //    {
        //        MessageBox.Show("Jenis Pemeriksaan harus diisi.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string selectedDokter = comboBoxDokter.SelectedItem?.ToString().Trim() ?? "";
        //    if (string.IsNullOrEmpty(selectedDokter))
        //    {
        //        MessageBox.Show("Dokter harus dipilih.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Find the index of the selected row in the DataGridView
        //    int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

        //    // Get the rm and tanggalKunjungan values from the selected row
        //    string rmFromGrid = dataGridView1.Rows[selectedRowIndex].Cells["RM"].Value.ToString();
        //    DateTime tanggalKunjunganFromGrid = Convert.ToDateTime(dataGridView1.Rows[selectedRowIndex].Cells["Tanggal Kunjungan"].Value);

        //    // Update the existing record in the dataList
        //    DataItem existingItem = dataList.Find(item => item.Rm == rmFromGrid && item.TanggalKunjungan == tanggalKunjunganFromGrid);
        //    if (existingItem != null)
        //    {
        //        existingItem.Nama = nama;
        //        existingItem.TanggalLahir = tanggalLahir;
        //        existingItem.Umur = umur;
        //        existingItem.JenisKelamin = jenisKelamin;
        //        existingItem.Dokter = selectedDokter;
        //        existingItem.TanggalKunjungan = tanggalKunjungan;
        //        existingItem.Alamat = alamat;
        //        existingItem.JenisPemeriksaan = jenisPemeriksaan;
        //    }

        //    RefreshDataGridView();

        //    // Export to CSV
        //    ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");
        //    ExportLastRowToCSV(filePath1, dataList.LastOrDefault());

        //    // Clear the form fields
        //    ClearTextBoxes();

        //    // Refresh the DataGridView after saving or updating
        //    RefreshDataGridView();

        //    txt_Rm.Enabled = true;

        //    // Tampilkan pesan keberhasilan
        //    MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    DisableButtons();
        //    isEditing = false;
        //}

        //private void button5_Click_1(object sender, EventArgs e)
        //{

        //    // Memeriksa apakah txt_Rm telah diisi
        //    if (string.IsNullOrEmpty(txt_Rm.Text))
        //    {
        //        MessageBox.Show("Silakan pilih data yang ingin diperbarui terlebih dahulu.", "Data Belum Dipilih", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return; // Keluar dari method karena data belum dipilih
        //    }
        //    string rm = txt_Rm.Text;
        //    string nama = txt_Nama.Text;
        //    DateTime tanggalLahir = dateTimePicker1.Value;
        //    DateTime tanggalKunjungan = dateTimePicker2.Value.Date;
        //    string umur = HitungUmur(tanggalLahir);
        //    string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
        //    string alamat = richTextBox1.Text;
        //    string jenisPemeriksaan = txt_Tindakan.Text;
        //    string selectedDokter = comboBoxDokter.SelectedItem?.ToString() ?? "";
        //    // Ganti koma dengan tilde (~)
        //    selectedDokter = selectedDokter;
        //    // Find the index of the selected row in the DataGridView
        //    int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

        //    // Get the rm and tanggalKunjungan values from the selected row
        //    string rmFromGrid = dataGridView1.Rows[selectedRowIndex].Cells["RM"].Value.ToString();
        //    DateTime tanggalKunjunganFromGrid = Convert.ToDateTime(dataGridView1.Rows[selectedRowIndex].Cells["Tanggal Kunjungan"].Value);

        //    // Update the existing record in the dataList
        //    DataItem existingItem = dataList.Find(item => item.Rm == rmFromGrid && item.TanggalKunjungan == tanggalKunjunganFromGrid);
        //    if (existingItem != null)
        //    {
        //        existingItem.Nama = nama;
        //        existingItem.TanggalLahir = tanggalLahir;
        //        existingItem.Umur = umur;
        //        existingItem.JenisKelamin = jenisKelamin;
        //        existingItem.Dokter = selectedDokter;
        //        existingItem.TanggalKunjungan = tanggalKunjungan;
        //        existingItem.Alamat = alamat;
        //        existingItem.JenisPemeriksaan = jenisPemeriksaan;
        //    }

        //    RefreshDataGridView();
        //    // Export to CSV
        //    ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");

        //    ExportLastRowToCSV(filePath1, dataList.LastOrDefault());

        //    // Clear the form fields
        //    ClearTextBoxes();

        //    // Refresh the DataGridView after saving or updating
        //    RefreshDataGridView();
        //    //btn_Save.Enabled = true;
        //    txt_Rm.Enabled = true;
        //    //btn_Cancel.Enabled = true;
        //    // Tampilkan pesan keberhasilan
        //    MessageBox.Show("Data berhasil diubah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    DisableButtons();
        //    // Set the editing flag to true
        //    isEditing1 = false;

        //}

        private void ExportLastRowToCSV(string filePath, DataItem lastDataItem)
        {
            if (lastDataItem != null)
            {

                File.WriteAllText(filePath, string.Empty);
                bool fileExists = File.Exists(filePath);
                bool isEmpty = fileExists && new FileInfo(filePath).Length == 0;

                using (var writer = new StreamWriter(filePath, true)) // `true` untuk menambahkan ke file
                using (var csv = new CsvWriter(writer))
                {
                    if (!fileExists || isEmpty)
                    {
                        csv.WriteField("No");
                        csv.WriteField("Rm");
                        csv.WriteField("Nama");
                        csv.WriteField("Tanggal Lahir");
                        csv.WriteField("Umur");
                        csv.WriteField("Jenis Kelamin");
                        csv.WriteField("Dokter");
                        csv.WriteField("Tanggal Kunjungan");
                        csv.WriteField("Alamat");
                        csv.WriteField("Jenis Pemeriksaan");
                        csv.NextRecord();
                    }

                    csv.WriteField(lastDataItem.No);
                    csv.WriteField(lastDataItem.Rm);
                    csv.WriteField(lastDataItem.Nama);
                    csv.WriteField(lastDataItem.TanggalLahir.ToString("dd-MM-yyyy"));
                    csv.WriteField(lastDataItem.Umur);
                    csv.WriteField(lastDataItem.JenisKelamin);
                    csv.WriteField(lastDataItem.Dokter);
                    csv.WriteField(lastDataItem.TanggalKunjungan.ToString("dd-MM-yyyy"));
                    csv.WriteField(lastDataItem.Alamat);
                    csv.WriteField(lastDataItem.JenisPemeriksaan);
                    csv.NextRecord();
                }
            }
        }


        private void ExportToCSV(string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer))
                {
                    // Write the header
                    writer.WriteLine("No,Rm,Nama,TanggalLahir,Umur,JenisKelamin,Dokter,TanggalKunjungan,Alamat,JenisPemeriksaan");

                    // Write each data item with custom formatting
                    foreach (var item in dataList)
                    {
                        // Menulis data dengan format tanggal kustom
                        writer.WriteLine($"{item.No}," +
                            $"\"{item.Rm}\"," +
                            $"\"{item.Nama}\"," +
                            $"{item.TanggalLahir:dd/MM/yyyy}," +
                            $"{item.Umur}," +
                            $"\"{item.JenisKelamin}\"," +
                            $"\"{item.Dokter}\"," +
                            $"{item.TanggalKunjungan:dd/MM/yyyy}," +
                            $"\"{item.Alamat}\"," +
                            $"\"{item.JenisPemeriksaan}\"");
                    }
                }

                MessageBox.Show("Data berhasil disimpan", "Export Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //kode paling tereakhir sebelum ada method yg baru di atas ini
        //private void ExportToCSV(string filePath)
        //{
        //    try
        //    {
        //        // Ensure the directory exists
        //        string directory = Path.GetDirectoryName(filePath);
        //        if (!Directory.Exists(directory))
        //        {
        //            Directory.CreateDirectory(directory);
        //        }

        //        using (var writer = new StreamWriter(filePath))
        //        using (var csv = new CsvWriter(writer))
        //        {


        //            csv.WriteRecords(dataList);
        //        }

        //        // MessageBox.Show("Data berhasil disimpan", "Export Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}




        private void PopulateComboBox(string filePath)
        {

            try
            {
                // Bersihkan item ComboBox sebelum mengisi ulang
                comboBoxDokter.Items.Clear();

                // Membaca data dari file CSV menggunakan CsvReader dari CsvHelper
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    // Set konfigurasi CsvReader
                    csv.Configuration.HasHeaderRecord = false; // Tidak ada header dalam file CSV

                    // Melangkah ke baris pertama (header) dan melewatinya
                    csv.Read();

                    // Loop membaca setiap baris setelah header
                    while (csv.Read())
                    {
                        var namaDokter = csv.GetField<string>(2); // Ambil nilai dari kolom pertama (indeks 0)
                        comboBoxDokter.Items.Add(namaDokter);
                    }
                }
            }
            catch (Exception ex)
            {
                // Tangani pengecualian jika terjadi
                //MessageBox.Show($"Gagal mengisi ComboBox: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void enableFormRegister()
        {
            //txt_Code.Enabled = true;
            txt_Nama.Enabled = true;
            txt_Tindakan.Enabled = true;
            btn_Save.Enabled = true;
            btn_Cancel.Enabled = true;
        }

        private void ExportToExcel(DataGridView dataGridView, string filePath)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;

            try
            {
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Ekspor header
                for (int i = 1; i <= dataGridView.Columns.Count; i++)
                {
                    worksheet.Cells[1, i] = dataGridView.Columns[i - 1].HeaderText;
                }

                // Ekspor data
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Simpan workbook
                workbook.SaveAs(filePath);
                workbook.Close();
            }
            finally
            {
                // Tutup aplikasi Excel
                excelApp.Quit();
                // Rilis objek COM
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                excelApp = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
            saveFileDialog.Title = "Save Excel File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToExcel(dataGridView1, saveFileDialog.FileName);
                MessageBox.Show("Ekspor ke excel berhasil!", "Ekspor Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            isEditing = false;
            //btn_Save.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                textBox4.Clear();
            }
        }

        private void textBoxRt_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBoxRw_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //refreshsql();
            btn_Refresh.Enabled = false;
            txt_Search1.Clear();
        }

        private void enableMenuUpdate()
        {
            btn_Cancel.Enabled = true;
        }

        private void txt_ID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            int kondisi = 1;
            TransfEvent(kondisi.ToString());
        }

        private bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        private bool IsEmptyDate(DateTime value)
        {
            return value == DateTime.MinValue;
        }

        // Metode untuk memeriksa apakah RadioButton dipilih
        private bool IsRadioButtonSelected(RadioButton radioButton)
        {
            return radioButton.Checked;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            RemoveTrailingCommaAndSpace();

            string rm = txt_Rm.Text.Trim();
            string nama = RemoveExtraSpaces(txt_Nama.Text);
            DateTime tanggalLahir = dateTimePicker1.Value;
            DateTime tanggalKunjungan = dateTimePicker2.Value.Date;
            string umur = HitungUmur(tanggalLahir);
            string jenisKelamin = radioButtonPria.Checked ? "Laki - laki" : "Perempuan";
            string selectedDokter = comboBoxDokter.SelectedItem?.ToString().Trim() ?? "";
            string alamat = RemoveExtraSpaces(richTextBox1.Text);
            string jenisPemeriksaan = txt_Tindakan.Text.Trim();

            // Add a new record to dataList
            if (IsNullOrEmpty(rm) || IsNullOrEmpty(nama) || IsEmptyDate(tanggalLahir) || IsEmptyDate(tanggalKunjungan) || IsNullOrEmpty(umur) || IsNullOrEmpty(selectedDokter) || (!IsRadioButtonSelected(radioButtonPria) && !IsRadioButtonSelected(radioButtonWanita)) || IsNullOrEmpty(alamat) || IsNullOrEmpty(jenisPemeriksaan))
            {
                MessageBox.Show("Harap lengkapi semua data sebelum menyimpan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Keluar dari metode jika ada kolom yang belum diisi
            }

            if (dataList.Any(item => item.Rm == rm && item.TanggalKunjungan == tanggalKunjungan))
            {
                MessageBox.Show("Data dengan No. RM dan Tanggal Kunjungan yang sama sudah ada.", "Duplikasi Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dataList.Add(new DataItem
            {
                No = dataList.Count + 1,
                Rm = rm,
                Nama = nama,
                TanggalLahir = tanggalLahir,
                Umur = umur,
                JenisKelamin = jenisKelamin,
                Dokter = selectedDokter,
                TanggalKunjungan = tanggalKunjungan,
                Alamat = alamat,
                JenisPemeriksaan = jenisPemeriksaan
            });

            RefreshDataGridView();

            // Export to CSV
            //ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv", dataList);
            ExportToCSV("D:\\GLEndoscope\\Database\\dataPasien\\dataPasien.csv");

            ExportLastRowToCSV(filePath1, dataList.LastOrDefault());
            //ExportLastRowToCSV();

            // Clear the form fields
            ClearTextBoxes();

            // Refresh the DataGridView after saving or updating
            RefreshDataGridView();

            txt_Rm.Enabled = true;

            // Set the editing flag to true
            //isEditing = false;
        }






        private void RemoveTrailingCommaAndSpace()
        {
            string nama = txt_Nama.Text.Trim().Replace(",", "");
            DateTime tanggalLahir = dateTimePicker1.Value;
            DateTime tanggalKunjungan = dateTimePicker2.Value.Date;
            //string umur = HitungUmur(tanggalLahir);
            string umur = HitungUmur(tanggalLahir);
            string jenisKelamin = (radioButtonPria.Checked ? "Laki - laki" : "Perempuan").Trim().Replace(",", "");
            string alamat = richTextBox1.Text.Trim().Replace(",", "");
            string jenisPemeriksaan = txt_Tindakan.Text.Trim().Replace(",", "");
            string selectedDokter = (comboBoxDokter.SelectedItem?.ToString() ?? "").Trim().Replace(",", "");

        }

        private void ClearTextBoxes()
        {
            txt_Rm.Clear();
            txt_Nama.Clear();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            //txt_Umur.Clear();
            radioButtonPria.Checked = false;
            radioButtonWanita.Checked = false;
            comboBoxDokter.SelectedIndex = -1;
            richTextBox1.Clear();
            txt_Tindakan.Clear();

            // Reset the flag and button text
            //isEditing = false;
            //btn_Save.Text = "Simpan";
        }


        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            txt_Rm.Enabled = true;
            DisableButtons();
            isEditing1 = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int kondisi = 1;
            TransfEvent(kondisi.ToString());
            clearTextbox();
            this.Close();
        }
        private void textBoxRT_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void textBoxRW_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void comboBoxDokter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void panelUser_Paint(object sender, PaintEventArgs e)
        {
            if (panelUser.BorderStyle == BorderStyle.FixedSingle)
            {
                int thickness = 2;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(Color.Black, thickness))
                {
                    e.Graphics.DrawRectangle(p, new System.Drawing.Rectangle(halfThickness, halfThickness, panelUser.ClientSize.Width - thickness, panelUser.ClientSize.Height - thickness));
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "Laki - laki")
            {
                radioButtonPria.Checked = true;
                textBox1.Clear();
            }
            else if (textBox1.Text == "Perempuan")
            {
                radioButtonWanita.Checked = true;
                textBox1.Clear();
            }
        }

        private void btn_Default_Click(object sender, EventArgs e)
        {
            btn_Cancel.Enabled = true;
        }

        //kode sebelumnya yg tidak pernah di edit
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    // Check if you are in editing mode
        //    if (isEditing1)
        //    {
        //        // Export the data of the last clicked row to CSV
        //        if (dataGridView1.SelectedCells.Count > 0)
        //        {
        //            int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
        //            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
        //            {
        //                ExportRowToCSV(dataGridView1.Rows[rowIndex]);

        //                MessageBox.Show("Nama Pasien dijadikan Setel Utama", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Silakan klik dua kali baris untuk memilihnya sebelum mengekspor ke CSV.", "Ekspor ke CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Tidak ada data yang sedang diedit. Silakan klik dua kali baris untuk mulai mengedit.", "Ekspor ke CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //    }

        //    ClearTextBoxes();
        //    txt_Rm.Enabled = true;
        //    DisableButtons();
        //    isEditing1 = false;

        //}


        private void button2_Click(object sender, EventArgs e)
        {
            if (isEditing1)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
                    {
                        var selectedPatient = (DataItem)dataGridView1.Rows[rowIndex].DataBoundItem;
                        defaultPatient = selectedPatient; // Store the default patient

                        ExportRowToCSV(dataGridView1.Rows[rowIndex]);

                        MessageBox.Show("Nama Pasien dijadikan Setel Utama", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Silakan klik dua kali baris untuk memilihnya sebelum mengekspor ke CSV.", "Ekspor ke CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Tidak ada data yang sedang diedit. Silakan klik dua kali baris untuk mulai mengedit.", "Ekspor ke CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ClearTextBoxes();
            txt_Rm.Enabled = true;
            DisableButtons();
            isEditing1 = false;
        }


        private void btn_Search1_Click(object sender, EventArgs e)
        {
            PerformSearch();
            txt_Rm.Enabled = true;
            isEditing1 = false;
            ClearTextBoxes();
            DisableButtons();
        }

        private void PerformSearch()
        {
           string keyword = txt_Search1.Text.ToLower();

            List<DataItem> searchResults = dataList
                .Where(item =>
                    item.Rm.ToLower().Contains(keyword) ||
                    item.Nama.ToLower().Contains(keyword) ||
                    item.JenisKelamin.ToLower().Contains(keyword) ||
                    item.Dokter.ToLower().Contains(keyword) ||
                    item.Alamat.ToLower().Contains(keyword) ||
                    item.JenisPemeriksaan.ToLower().Contains(keyword) ||
                    item.TanggalLahir.ToString("dd/MM/yyyy").Contains(keyword))
                .ToList();

            // Menampilkan pesan jika tidak ada hasil
            if (searchResults.Count == 0)
            {
                MessageBox.Show("Tidak ada hasil yang ditemukan.", "Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DisplaySearchResults1(searchResults);
            }
        }
        private void DisplaySearchResults1(List<DataItem> searchResults)
        {
            dataGridView1.Rows.Clear();

            foreach (var data in searchResults)
            {
                dataGridView1.Rows.Add(data.No, data.Rm, data.Nama, data.TanggalLahir.ToString("dd/MM/yyyy"), data.Umur, data.JenisKelamin, data.Dokter, data.TanggalKunjungan.ToShortDateString(), data.Alamat, data.JenisPemeriksaan);
            }
        }

        private void DisplaySearchResults(List<DataItem> results)
        {
            // Clear the existing rows in the DataGridView
            dataGridView1.Rows.Clear();

            // Add the search results to the DataGridView
            foreach (var result in results)
            {
                dataGridView1.Rows.Add(
                    result.No,
                    result.Rm,
                    result.Nama,
                    result.TanggalLahir,
                    result.Umur,
                    result.JenisKelamin,
                    result.Dokter,
                    result.TanggalKunjungan,
                    result.Alamat,
                    result.JenisPemeriksaan
                );
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            // Refresh the DataGridView after saving or updating
            isEditing1 = false;
            RefreshDataGridView();
            txt_Search1.Clear();
            ClearTextBoxes();
        }
    }
}
