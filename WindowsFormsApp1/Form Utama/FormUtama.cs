using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CsvHelper;


using AForge.Video.FFMPEG;
using AForge.Video.VFW;
using System.Runtime.InteropServices;
using System.Diagnostics;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing.Imaging;
using System.Media;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.WindowsAPICodePack.Dialogs;
using WindowsFormsApp1.Form_Utama;
using WindowsFormsApp1.Format_2;
using WindowsFormsApp1.FormSwitcing;
using WindowsFormsApp1.Format_3;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

using SystemTask = System.Threading.Tasks.Task;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

       
        bool vCamera = false;
        bool vRecord = false;

        private int CustomerWebcam_CapabilitiesIndexMin;
        private int CustomerWebcam_CapabilitiesIndexMax;


        System.Windows.Forms.Timer t1; 
        Stopwatch s1; 
        private Stopwatch stopWatch = null; 
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice; 
        private FilterInfoCollection VideoCaptureDevices; 
        private VideoCaptureDevice FinalVideo = null;
        private VideoCaptureDeviceForm captureDevice; 
        private Bitmap video; 
        public VideoFileWriter FileWriter = new VideoFileWriter();
        private SaveFileDialog saveAvi; 
        string tanggal, jam, id, Name, Code, Date, action, action1, gabung, address, tanggalHari, splitBulan, splitTahun, noRM;
        string codeDefault, namaDefault; 
        [DllImport("kernel32.dll")]
        static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize); 
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern int MessageBoxTimeout(IntPtr hwnd, String text, String title, uint type, Int16 wLanguageId, Int32 milliseconds);

        private Bitmap video1;

        
        private FileSystemWatcher watcher;

        private System.Windows.Forms.Timer timer;



        public Form1()
        {
            InitializeComponent();


            // Inisialisasi FileSystemWatcher
            watcher = new FileSystemWatcher();
            watcher.Path = @"D:\GLEndoscope\Obs";
            watcher.Filter = "*.*";
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = false;

            // Tambahkan event handler untuk kejadian file dibuat
            watcher.Created += new FileSystemEventHandler(OnFileCreated);

            // Mulai memantau
            watcher.EnableRaisingEvents = true;

            // Inisialisasi Timer
            //timer = new System.Windows.Forms.Timer();
            //timer.Interval = 5000; // Interval dalam milidetik (misalnya 5000 untuk 5 detik)
            //timer.Tick += new EventHandler(Timer_Tick); // Tambahkan event handler untuk event Tick
            //timer.Start(); // Mulai Timer

            // Ambil data dari database pertama kali
            UpdateDataFromDatabase();

            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Cek apakah richTextBox1.Text tidak kosong
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                // Sembunyikan label7
                label7.Visible = false;
            }
            else
            {
                // Tampilkan label7
                label7.Visible = true;
            }
        }

       

        //private void OnFileCreated(object sender, FileSystemEventArgs e)
        //{
        //    string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Image\";
        //    string dir1 = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Video\";

        //    string[] imageExtensions = { ".png", ".jpg" };
        //    string[] videoExtensions = { ".mp4", ".avi" };

        //    // Periksa ekstensi file
        //    string fileExtension = Path.GetExtension(e.FullPath).ToLower();

        //    // Jika file adalah gambar, pindahkan ke direktori gambar
        //    if (imageExtensions.Contains(fileExtension))
        //    {
        //        string destinationFile = Path.Combine(dir, Path.GetFileName(e.FullPath));

        //        //MessageBox.Show(dir);
        //        if (!Directory.Exists(dir))
        //        {
        //            Directory.CreateDirectory(dir);
        //        }

        //        // Tunggu hingga file selesai ditulis oleh sistem
        //        while (IsFileLocked(new FileInfo(e.FullPath)))
        //        {
        //            System.Threading.Thread.Sleep(500);
        //        }

        //        // Pindahkan file
        //        File.Move(e.FullPath, destinationFile);

        //        // Perbarui nilai TextBox secara langsung
        //        //textBox3.Text = dir;
        //    }
        //    // Jika file adalah video, pindahkan ke direktori video
        //    else if (videoExtensions.Contains(fileExtension))
        //    {
        //        string destinationFile = Path.Combine(dir1, Path.GetFileName(e.FullPath));

        //        //MessageBox.Show(dir);
        //        if (!Directory.Exists(dir1))
        //        {
        //            Directory.CreateDirectory(dir1);
        //        }

        //        // Tunggu hingga file selesai ditulis oleh sistem
        //        while (IsFileLocked(new FileInfo(e.FullPath)))
        //        {
        //            System.Threading.Thread.Sleep(500);
        //        }



        //        // Pindahkan file
        //        File.Move(e.FullPath, destinationFile);

        //        // Perbarui nilai TextBox secara langsung
        //        //textBox3.Text = dir;
        //    }
        //}


        //private void OnFileCreated(object sender, FileSystemEventArgs e)
        //{
        //    string dir = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Image\";
        //    string dir1 = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Video\";

        //    string[] imageExtensions = { ".png", ".jpg" };
        //    string[] videoExtensions = { ".mp4", ".avi" };

        //    // Periksa ekstensi file
        //    string fileExtension = Path.GetExtension(e.FullPath).ToLower();

        //    try
        //    {
        //        // Jika file adalah gambar, pindahkan ke direktori gambar
        //        if (imageExtensions.Contains(fileExtension))
        //        {
        //            string destinationFile = Path.Combine(dir, Path.GetFileName(e.FullPath));
        //            MoveFile(e.FullPath, destinationFile);

        //            // Debugging output
        //            //MessageBox.Show($"Image moved to: {destinationFile}");

        //            // Perbarui nilai TextBox secara langsung
        //            //textBox3.Text = dir;
        //        }
        //        // Jika file adalah video, pindahkan ke direktori video
        //        else if (videoExtensions.Contains(fileExtension))
        //        {
        //            string destinationFile = Path.Combine(dir1, Path.GetFileName(e.FullPath));
        //            MoveFile(e.FullPath, destinationFile);

        //            // Debugging output
        //            //MessageBox.Show($"Video moved to: {destinationFile}");

        //            // Perbarui nilai TextBox secara langsung
        //            //textBox3.Text = dir1;
        //        }
        //        else
        //        {
        //            // Debugging output for unsupported file type
        //            //MessageBox.Show($"Unsupported file type: {fileExtension}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        //MessageBox.Show($"Error moving file: {ex.Message}");
        //    }
        //}

        //private void MoveFile(string sourcePath, string destinationPath)
        //{
        //    if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
        //    {
        //        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
        //    }

        //    while (IsFileLockedCustom(new FileInfo(sourcePath))) // Use custom method name
        //    {
        //        System.Threading.Thread.Sleep(500);
        //    }

        //    File.Move(sourcePath, destinationPath);
        //}

        //private bool IsFileLockedCustom(FileInfo file) // Use custom method name
        //{
        //    try
        //    {
        //        using (FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        //        {
        //            stream.Close();
        //        }
        //    }
        //    catch (IOException)
        //    {
        //        return true;
        //    }
        //    return false;
        //}



        // nepi dieu 

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            string dir = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Image\";
            string dir1 = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Video\";

            string[] imageExtensions = { ".png", ".jpg" };
            string[] videoExtensions = { ".mp4", ".avi" };

            // Periksa ekstensi file
            string fileExtension = Path.GetExtension(e.FullPath).ToLower();

            try
            {
                // Jika file adalah gambar, pindahkan ke direktori gambar
                if (imageExtensions.Contains(fileExtension))
                {
                    string destinationFile = Path.Combine(dir, Path.GetFileName(e.FullPath));
                    MoveImageFile(e.FullPath, destinationFile);
                }
                // Jika file adalah video, panggil metode baru untuk pemindahan video
                else if (videoExtensions.Contains(fileExtension))
                {
                    OnNewFile(sender, e);
                }
                else
                {
                    // Debugging output for unsupported file type
                    MessageBox.Show($"Unsupported file type: {fileExtension}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error moving file: {ex.Message}");
            }
        }

        private void MoveImageFile(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            }

            while (IsFileLockedCustom(new FileInfo(sourcePath))) // Use custom method name
            {
                System.Threading.Thread.Sleep(500);
            }

            File.Move(sourcePath, destinationPath);
        }

        private bool IsFileLockedCustom(FileInfo file) // Use custom method name
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }

        private void OnNewFile(object sender, FileSystemEventArgs e)
        {
            // Memastikan file sudah selesai ditulis dengan menunggu beberapa waktu
            SystemTask.Run(async () =>
            {
                await WaitForFile(e.FullPath);
                MoveVideoFile(e.FullPath);
            });
        }

        private async SystemTask WaitForFile(string filePath)
        {
            const int delay = 1000; // 1 detik
            bool fileIsAccessible = false;

            while (!fileIsAccessible)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        fileIsAccessible = true;
                    }
                }
                catch (IOException)
                {
                    // Jika file masih diakses, tunggu sebelum mencoba lagi
                    await SystemTask.Delay(delay);
                }
            }
        }

        private void MoveVideoFile(string sourcePath)
        {
            try
            {
                string dir1 = $@"D:\GLEndoscope\{splitTahun}\{splitBulan}\{tanggal}\{gabung}\Video\";
                string fileName = Path.GetFileName(sourcePath);
                string targetPath = Path.Combine(dir1, fileName);

                // Pastikan folder tujuan ada
                if (!Directory.Exists(dir1))
                {
                    Directory.CreateDirectory(dir1);
                }

                // Pindahkan file
                File.Move(sourcePath, targetPath);
                //MessageBox.Show($"File moved to {targetPath}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error moving file: {ex.Message}");
            }
        }



        //nepi dieu 





        private void UpdateDataFromDatabase()
        {
            tanggal = DateTime.Now.ToString("ddMMyyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];

            string csvFilePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataDefault.csv";

            try
            {
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Read(); // Skip header record

                    while (csv.Read())
                    {
                        // Read data from the CSV
                        var noRM = csv.GetField<string>("Rm")?.Trim();
                        var name = csv.GetField<string>("Nama")?.Trim();
                        var action = csv.GetField<string>("Jenis Pemeriksaan")?.Trim();

                        // Generate directory paths based on the extracted data
                       string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Image";
                       string dir1 = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Video";

                        // Create directories if they don't exist
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        if (!Directory.Exists(dir1))
                        {
                            Directory.CreateDirectory(dir1);
                        }

                        // Update UI elements
                        lblCode.Text = noRM;
                        richTextBox1.Text = name;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Cek apakah file sedang terkunci
        //private bool IsFileLocked(FileInfo file)
        //{
        //    FileStream stream = null;

        //    try
        //    {
        //        stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        //    }
        //    catch (IOException)
        //    {
        //        // File sedang terkunci
        //        return true;
        //    }
        //    finally
        //    {
        //        if (stream != null)
        //            stream.Close();
        //    }

        //    // File tidak terkunci
        //    return false;
        //}


        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            CloseCurrentVideoSource();

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start();

            // reset stop watch
            stopWatch = null;

            // start timer
            timer1.Start();
            this.Cursor = Cursors.Default;
        }

        private void CloseCurrentVideoSource()
        {
            if (videoSourcePlayer.VideoSource != null)
            {
                videoSourcePlayer.SignalToStop();

                // wait ~ 3 seconds
                for (int i = 0; i < 30; i++)
                {
                    if (!videoSourcePlayer.IsRunning)
                        break;
                    System.Threading.Thread.Sleep(100);
                }

                if (videoSourcePlayer.IsRunning)
                {
                    videoSourcePlayer.Stop();
                }

                videoSourcePlayer.VideoSource = null;
            }
        }

         

        private void stopCamera()
        {
            //close 
            //this.FinalVideo.Stop();
            this.FinalVideo.SignalToStop();
            this.FinalVideo = null;
            FileWriter.Close();
            //this.AVIwriter.Close();
            pictureBox1.Image = null;
        }

         

        private void Form1_Load(object sender, EventArgs e)
        { 
            this.ActiveControl = label1;
            s1 = new Stopwatch();
            t1 = new System.Windows.Forms.Timer();
            t1.Interval = 10;
            t1.Tick += T1_Tick;
            t1.Start(); 
            videoSourcePlayer.Visible = false;
            panelAtas.Visible = false;
            panelBawah.Visible = false;
            FormUser newMDIChild = new FormUser();
            newMDIChild.MdiParent = this;
            newMDIChild.StartPosition = FormStartPosition.Manual;
            newMDIChild.Left = 0;
            newMDIChild.Top = 0;
            newMDIChild.TransfEvent += frm_TransfEvent;
            newMDIChild.Show();
            btn_patient.Enabled = false;
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = SystemColors.ButtonHighlight; 
            lblRec1.Visible = false;
            picRec1.Visible = false; 
            var ellipseRadius2 = new System.Drawing.Drawing2D.GraphicsPath();
            ellipseRadius2.StartFigure();
            ellipseRadius2.AddArc(new Rectangle(0, 0, 10, 10), 180, 90);
            ellipseRadius2.AddLine(10, 0, panelPatientData.Width - 20, 0);
            ellipseRadius2.AddArc(new Rectangle(panelPatientData.Width - 10, 0, 10, 10), -90, 90);
            ellipseRadius2.AddLine(panelPatientData.Width, 20, panelPatientData.Width, panelPatientData.Height - 10);
            ellipseRadius2.AddArc(new Rectangle(panelPatientData.Width - 10, panelPatientData.Height - 10, 10, 10), 0, 90);
            ellipseRadius2.AddLine(panelPatientData.Width - 10, panelPatientData.Height, 20, panelPatientData.Height);
            ellipseRadius2.AddArc(new Rectangle(0, panelPatientData.Height - 10, 10, 10), 90, 90);
            ellipseRadius2.CloseAllFigures();
            panelPatientData.Region = new Region(ellipseRadius2);

            createFolder();




        }

        private void T1_Tick(object sender, EventArgs e)
        { 
           TimeSpan ts = s1.Elapsed;
           string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
           ts.Hours, ts.Minutes, ts.Seconds,
           ts.Milliseconds / 10); 
           lblRec1.Text = elapsedTime; 
        }
         

         
          

        private void timer1_Tick(object sender, EventArgs e)
        {
            int test = 1;
            textBox2.Text = test.ToString();
            IVideoSource videoSource = videoSourcePlayer.VideoSource;

            if (videoSource != null)
            {
                int framesReceived = videoSource.FramesReceived;

                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }
                else
                {
                    stopWatch.Stop();
                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }

            jam = DateTime.Now.ToString("hhmmss");
            tanggal = DateTime.Now.ToString("ddMMyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];
            tanggalHari = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss dddd");
            getPatient(); 
            GC.Collect();
            GC.WaitForPendingFinalizers(); 
            //label6.Text = vCamera.ToString();

        }

        private void btn_patient_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            { 
                if (txt_Form.Text != "")
                { 
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    FormUser newMDIChild = new FormUser();
                    newMDIChild.MdiParent = this;
                    newMDIChild.StartPosition = FormStartPosition.Manual;
                    newMDIChild.Left = 0;
                    newMDIChild.Top = 0;
                    newMDIChild.TransfEvent += frm_TransfEvent;
                    newMDIChild.textBox4.Text = "formPasien";
                    newMDIChild.Show();
                    videoSourcePlayer.Visible = false;
                    panelAtas.Visible = false;
                    panelBawah.Visible = false;
                    btn_patient.Enabled = false;
                    btn_patient.BackColor = Color.FromArgb(0, 85, 119);
                    int Fuser = 1;
                    txt_Form.Text = Fuser.ToString();
                    string kirim = "kirim";
                    newMDIChild.textBox3.Text = kirim;
                } 
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Stop Camera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Stop Record terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        { 
            if (textBox1.Text == "2")
            {
                FinalVideo.Stop();
                Environment.Exit(0);
            }
        } 

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(900);
            SendKeys.SendWait("{Enter}");//or Esc
        }

        private void txtFoot_TextChanged(object sender, EventArgs e)
        {
            //string foot = "r";
            //string foot1 = "R";
            string foot = "1";
            string foot1 = "1";
            if (txtFoot.Text == foot.ToString() || txtFoot.Text == foot1.ToString())
            { 
                textBox2.Clear();
                txtFoot.Focus();
                pictureBox2.Image = pictureBox1.Image; 
                string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Image\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                pictureBox1.Image.Save(dir + jam + ".bmp", ImageFormat.Jpeg);
                string tgl = tanggalHari;
                string nama = Name;
                string tindakan = action;
                PointF tanggalLocation = new PointF(30f, 25f);
                PointF namaLocation = new PointF(1650f, 25f);
                PointF tindakanLocation = new PointF(1650f, 50f);
                string imageFilePath = dir + jam + ".bmp";
                string imageFilePathJPG = dir + jam + ".jpg";
                Bitmap newBitmap;
                using (var bitmap = (Bitmap)System.Drawing.Image.FromFile(imageFilePath))//load the image file
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font arialFont = new Font("Arial", 15))
                        {
                            graphics.DrawString(tgl, arialFont, Brushes.White, tanggalLocation);
                            graphics.DrawString(nama, arialFont, Brushes.White, namaLocation);
                            graphics.DrawString(tindakan, arialFont, Brushes.White, tindakanLocation);
                        }
                    }
                    newBitmap = new Bitmap(bitmap);
                }

                newBitmap.Save(imageFilePath);//save the image file
                newBitmap.Dispose();
                System.Drawing.Image Dummy = System.Drawing.Image.FromFile(imageFilePath);
                Dummy.Save(imageFilePathJPG, ImageFormat.Jpeg);
                Dummy.Dispose(); 
                backgroundWorker1.RunWorkerAsync();
                MessageBox.Show("Gambar tersimpan ke Folder", "Mengambil Gambar", MessageBoxButtons.OK, MessageBoxIcon.Information);


                if (File.Exists(imageFilePath))
                {

                    File.Delete(imageFilePath);
                }
                else
                {

                } 
                txtFoot.Clear();
            }
        }  

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (txt_Form.Text != "")
            {
                this.ActiveControl = label2;
                string message = "Tutup halaman terlebih dahulu";
                string title = "Peringatan";
                MessageBox.Show(message, title);

            }
            else
            {
                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                FormPrint newMDIChilddddd = new FormPrint();
                newMDIChilddddd.MdiParent = this;
                newMDIChilddddd.StartPosition = FormStartPosition.Manual;
                newMDIChilddddd.Left = 0;
                newMDIChilddddd.Top = 0;
                newMDIChilddddd.TransfEventtttt += frm_TransfEvent4;
                newMDIChilddddd.TransfEventPrint1 += frm_TransfEventPrint1;
                newMDIChilddddd.Show();
                int Fone = 2;
                txt_Form.Text = Fone.ToString();
                button3.Enabled = false;
                button3.BackColor = Color.FromArgb(0, 85, 119);
            }
        } 

        private void btn_Capture_Click(object sender, EventArgs e)
        {

            //btn_Capture.BackColor = Color.FromArgb(0, 85, 119);

            textBox2.Clear();
            txtFoot.Focus();
            pictureBox2.Image = pictureBox1.Image; 
            string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" + gabung + @"\Image\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            pictureBox1.Image.Save(dir + jam + ".bmp", ImageFormat.Jpeg);
            string tgl = tanggalHari;
            string nama = Name;
            string tindakan = action;
            PointF tanggalLocation = new PointF(30f, 25f);
            PointF namaLocation = new PointF(1650f, 25f);
            PointF tindakanLocation = new PointF(1650f, 50f);
            string imageFilePath = dir + jam + ".bmp";
            string imageFilePathJPG = dir + jam + ".jpg";
            Bitmap newBitmap;
            using (var bitmap = (Bitmap)System.Drawing.Image.FromFile(imageFilePath))//load the image file
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Arial", 15))
                    {
                        graphics.DrawString(tgl, arialFont, Brushes.White, tanggalLocation);
                        graphics.DrawString(nama, arialFont, Brushes.White, namaLocation);
                        graphics.DrawString(tindakan, arialFont, Brushes.White, tindakanLocation);
                    }
                }
                newBitmap = new Bitmap(bitmap);
            }

            newBitmap.Save(imageFilePath);//save the image file
            newBitmap.Dispose();
            System.Drawing.Image Dummy = System.Drawing.Image.FromFile(imageFilePath);
            Dummy.Save(imageFilePathJPG, ImageFormat.Jpeg);
            Dummy.Dispose(); 
            backgroundWorker1.RunWorkerAsync();
            MessageBox.Show("Image Saved to Folder", "Capture", MessageBoxButtons.OK ,MessageBoxIcon.Information);

            if (File.Exists(imageFilePath))
            {

                File.Delete(imageFilePath);
            } 
            //btn_Capture.BackColor = Color.FromArgb(0, 107, 150); 
        } 

        private void textBoxPrint_TextChanged(object sender, EventArgs e)
        {
            int print1Picture = 1;
            int print4Picture = 2;
            int print6Picture = 3;
            int format2Print = 4; 
            int print2CaptureCirebon = 5; 
            int print4CaptureCirebon = 6;  
            int print6CaptureCirebon = 7;  
            int format3Print = 8;  

            if (textBoxPrint.Text == print1Picture.ToString())
            { 
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                } 

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder(); 
                Form1Print newMDIChildd = new Form1Print();
                newMDIChildd.MdiParent = this;
                newMDIChildd.StartPosition = FormStartPosition.Manual;
                newMDIChildd.Left = 0;
                newMDIChildd.Top = 0; 
                newMDIChildd.TransfEventt += frm_TransfEvent1;
                newMDIChildd.TransfEvenPrint4 += frm_TransfEventPrint4;
                newMDIChildd.TransfEvenPrint6 += frm_TransfEventPrint6;
                newMDIChildd.TransfEventPrint1G += frm_TransfEventPrint1G;
                newMDIChildd.Show();
                int Fone = 2;
                txt_Form.Text = Fone.ToString(); 
                string test = "kirim"; 
                newMDIChildd.textBox3.Text = test; 
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == print4Picture.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                } 

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                Form4Print newMDIChilddd = new Form4Print();
                newMDIChilddd.MdiParent = this;
                newMDIChilddd.StartPosition = FormStartPosition.Manual;
                newMDIChilddd.Left = 0;
                newMDIChilddd.Top = 0; 
                newMDIChilddd.TransfEventtt += frm_TransfEvent2;
                newMDIChilddd.TransfEventPrint1 += frm_TransfEventPrint14;
                newMDIChilddd.TransfEventPrint6 += frm_TransfEventPrint16;
                newMDIChilddd.TransfEvenPrint4G += frm_TransfEvenPrint4G;
                newMDIChilddd.Show();
                int Ffour = 3;
                txt_Form.Text = Ffour.ToString(); 
                string kirim = "kirim";
                newMDIChilddd.textBox3.Text = kirim; 
                textBoxPrint.Clear(); 
            }

            else if (textBoxPrint.Text == print6Picture.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                Form6Print newMDIChildddd = new Form6Print();
                newMDIChildddd.MdiParent = this;
                newMDIChildddd.StartPosition = FormStartPosition.Manual;
                newMDIChildddd.Left = 0;
                newMDIChildddd.Top = 0; 
                newMDIChildddd.TransfEventttt += frm_TransfEvent3;
                newMDIChildddd.TransfEventPrint1 += frm_TransfEvent61;
                newMDIChildddd.TransfEventPrint6 += frm_TransfEvent64;
                newMDIChildddd.TransfEventPrint6G += frm_TransfEventPrint6G;
                newMDIChildddd.Show();
                int Fsix = 4;
                txt_Form.Text = Fsix.ToString(); 
                string kirim = "kirim";
                newMDIChildddd.textBox3.Text = kirim; 
                textBoxPrint.Clear();  
            }

            else if (textBoxPrint.Text == format2Print.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                Form21Gambar form21 = new Form21Gambar();
                form21.MdiParent = this;
                form21.StartPosition = FormStartPosition.Manual;
                form21.Left = 0;
                form21.Top = 0;
                form21.TEViewC6Gambar += frm_TEViewC6Gambar;
                form21.TEFormat2 += frm_TransfEventFormat2;
                form21.TEViewC2Gambar += frm_TEViewC2Gambar;
                form21.TEViewC4Gambar += frm_TEViewC4Gambar;
                form21.TEViewC21G += frm_TEViewC21G;
                form21.Show();
                int Fsix = 4;
                txt_Form.Text = Fsix.ToString(); 
                string kirim = "kirim";
                form21.textBox2.Text = kirim; 
                textBoxPrint.Clear();
            } 

            else if (textBoxPrint.Text == print2CaptureCirebon.ToString())
            { 
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                Form22Gambar form22 = new Form22Gambar();
                form22.MdiParent = this;
                form22.StartPosition = FormStartPosition.Manual;
                form22.Left = 0;
                form22.Top = 0;

                form22.TEViewC21Gambar += frm_TEViewC21Gambar;
                form22.TEViewC24Gambar += frm_TEViewC24Gambar;
                form22.TEViewC26Gambar += frm_TEViewC26Gambar;
                form22.TEClose2Gambar += frm_TEClose2Gambar;
                form22.TEViewC2 += frm_TEViewC2;

                form22.Show();
                int Fsix = 5;
                txt_Form.Text = Fsix.ToString(); 
                string kirim = "kirim";
                form22.textBox2.Text = kirim; 
                textBoxPrint.Clear(); 
            }

            else if (textBoxPrint.Text == print4CaptureCirebon.ToString())
            { 
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true; 
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150); 
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                Form24Gambar form24 = new Form24Gambar();
                form24.MdiParent = this;
                form24.StartPosition = FormStartPosition.Manual;
                form24.Left = 0;
                form24.Top = 0;
                form24.TEViewC41Gambar += frm_TEViewC41Gambar;
                form24.TEViewC42Gambar += frm_TEViewC42Gambar;
                form24.TEViewC46Gambar += frm_TEViewC46Gambar;
                form24.TEClose4Gambar += frm_TEClose4Gambar; 
                form24.TEViewC24G += frm_TEViewC24G; 
                string kirim = "kirim";
                form24.textBox2.Text = kirim;
                form24.Show();
                int Fsix = 6;
                txt_Form.Text = Fsix.ToString(); 
                textBoxPrint.Clear(); 
            }

            else if (textBoxPrint.Text == print6CaptureCirebon.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                Form26Gambar form26 = new Form26Gambar();
                form26.MdiParent = this;
                form26.StartPosition = FormStartPosition.Manual;
                form26.Left = 0;
                form26.Top = 0;
                //formCirebon6Gambar.TEViewC41Gambar += frm_TEViewC41Gambar;
                //formCirebon6Gambar.TEViewC42Gambar += frm_TEViewC42Gambar;
                form26.TEClose6Gambar += frm_TEClose6Gambar;
                form26.TEViewC64Gambar += frm_TEViewC64Gambar;
                form26.TEViewC62Gambar += frm_TEViewC62Gambar;
                form26.TEViewC61Gambar += frm_TEViewC61Gambar;
                form26.TEViewC46G += frm_TEViewC46G;
                string kirim = "kirim";
                form26.textBox2.Text = kirim;
                form26.Show();
                int Fsix = 7;
                txt_Form.Text = Fsix.ToString();
                textBoxPrint.Clear();
            }

            else if (textBoxPrint.Text == format3Print.ToString())
            {
                if (textBox1.Text == "2")
                {
                    FinalVideo.Stop();
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //btn_Capture.Enabled = false;
                }

                videoSourcePlayer.Visible = false;
                panelAtas.Visible = false;
                panelBawah.Visible = false;
                createFolder();
                Form310Gambar form310 = new Form310Gambar();
                form310.MdiParent = this;
                form310.StartPosition = FormStartPosition.Manual;
                form310.Left = 0;
                form310.Top = 0; 
                form310.TransfEventPrint310 += frm_TransfEventPrint310;  
                form310.TEClose10Gambar += frm_TEClose10Gambar; 
                form310.TransfEventPrint310Print += frm_TransfEventPrint310Print; 
                string kirim = "kirim";
                form310.textBox2.Text = kirim;
                form310.Show();
                int Fsix = 7;
                txt_Form.Text = Fsix.ToString();
                textBoxPrint.Clear();
            }
        }

        private void frm_TransfEventPrint310Print(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEClose10Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEventPrint310(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint1G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEvenPrint4G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint6G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC21G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC2(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC24G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC46G(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC46Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC26Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC61Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC62Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC64Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEClose6Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TEViewC6Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEClose4Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TEClose2Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TEViewC42Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC41Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC24Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TEViewC21Gambar(string value)
        {
            textBoxPrint.Text = value;
        } 

        private void frm_TEViewC4Gambar(string value)
        {
            textBoxPrint.Text = value;
        } 
        private void frm_TEViewC2Gambar(string value)
        {
            textBoxPrint.Text = value;
        }

        private void txt_kondisi_TextChanged(object sender, EventArgs e)
        {
            int nn = 1;
            int mm = 2;
            int ss = 3;
            int kk = 4;
            int dd = 5;
            int aa = 6; 
            int fDokter = 7; 
            int FormatClose = 9; 
            int FKlikF4 = 10;
            int FKlikF146 = 11;
            int FKlikF141 = 13; 
            int FKlikF16 = 12;
            int FKlikF161 = 14;
            int FKlikF164 = 15; 
            int FS24Gambar = 16; 
            int FS310Gambar = 17; 

            if (txt_kondisi.Text == nn.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                btn_patient.Enabled = true; 
                btn_patient.BackColor = Color.FromArgb(0, 107, 150); 
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == ss.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear(); 
            }
            else if (txt_kondisi.Text == kk.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == dd.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == aa.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == fDokter.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear(); 
                buttonDokter.Enabled = true;
                buttonDokter.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            } 
            else if (txt_kondisi.Text == FormatClose.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FKlikF4.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FKlikF146.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }

            else if (txt_kondisi.Text == FKlikF16.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FKlikF141.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }

            else if (txt_kondisi.Text == FKlikF161.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }


            else if (txt_kondisi.Text == FKlikF164.ToString())
            {
                videoSourcePlayer.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FS24Gambar.ToString())
            { 
                panel2.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                videoSourcePlayer.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
            }
            else if (txt_kondisi.Text == FS310Gambar.ToString())
            {
                panel2.Visible = true;
                panelAtas.Visible = true;
                panelBawah.Visible = true;
                videoSourcePlayer.Visible = true;
                txt_kondisi.Clear();
                txt_Form.Clear();
                txtFoot.Focus();
                button3.Enabled = true;
                button3.BackColor = Color.FromArgb(0, 107, 150);
            }
        } 

        //ini untuk membuat folder pada saat aplikasi di-running
        private void createFolder()
        {
            getPatient();
            jam = DateTime.Now.ToString("hhmmss");
            tanggal = DateTime.Now.ToString("ddMMyyy");
            string text = DateTime.Now.ToString("Y");
            string[] arr = text.Split(' ');
            splitBulan = arr[0];
            splitTahun = arr[1];
            tanggalHari = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss dddd");

            //ini adalah ketika tidak ada folder akan menanmbahkan folder secara otomatis
            pictureBox2.Image = pictureBox1.Image;
            string dir = @"D:\GLEndoscope\" + splitTahun + @"\" + splitBulan + @"\" + tanggal + @"\" ; 

            //MessageBox.Show(dir);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
             
        }

        void frm_TransfEvent(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEvent1(string value)
        {
            txt_kondisi.Text = value;
        }

        private void buttonDokter_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                { 
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                }

                else
                {
                    FormDokter formDokter = new FormDokter();
                    formDokter.MdiParent = this;
                    formDokter.StartPosition = FormStartPosition.Manual;
                    formDokter.Left = 0;
                    formDokter.Top = 0;
                    formDokter.TransfEventDokter += frm_TransfEventDokter;
                    formDokter.Show();
                    videoSourcePlayer.Visible = false;
                    panelAtas.Visible = false;
                    panelBawah.Visible = false;
                    buttonDokter.BackColor = Color.FromArgb(0, 85, 119);
                    buttonDokter.Enabled = false;
                    int Fuser = 8;
                    txt_Form.Text = Fuser.ToString();
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Stop Camera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Stop Record terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }  
        }

        private void frm_TransfEventDokter(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEvent2(string value)
        {
            txt_kondisi.Text = value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (FinalVideo == null)
                    {

                    }
                    else
                    {
                        FinalVideo.SignalToStop();
                    }

                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    createFolder();
                    panel2.Visible = false;
                    panelAtas.Visible = false;
                    panelBawah.Visible = false;
                    videoSourcePlayer.Visible = false;
                    FormSwitcing2Gambar formSwitcing2Gambar = new FormSwitcing2Gambar();
                    formSwitcing2Gambar.MdiParent = this;
                    formSwitcing2Gambar.StartPosition = FormStartPosition.Manual;
                    formSwitcing2Gambar.Left = 0;
                    formSwitcing2Gambar.Top = 0;
                    formSwitcing2Gambar.TEFS2Gambar += frm_TEFS2Gambar;
                    formSwitcing2Gambar.textBox1.Text = "1";
                    formSwitcing2Gambar.Show();
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Stop Camera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Stop Record terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } 
        }

        private void frm_TEFS2Gambar(string value)
        {
            txt_kondisi.Text = value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    if (FinalVideo == null)
                    {

                    }
                    else
                    {
                        FinalVideo.SignalToStop();
                    }
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //FinalVideo.SignalToStop();
                    createFolder();
                    panel2.Visible = false;
                    panelAtas.Visible = false;
                    panelBawah.Visible = false;
                    videoSourcePlayer.Visible = false;
                    FormSwitcing4Gambar formSwitcing4Gambar = new FormSwitcing4Gambar();
                    formSwitcing4Gambar.MdiParent = this;
                    formSwitcing4Gambar.StartPosition = FormStartPosition.Manual;
                    formSwitcing4Gambar.Left = 0;
                    formSwitcing4Gambar.Top = 0;
                    formSwitcing4Gambar.TEFS4Gambar += frm_TEFS4Gambar;
                    formSwitcing4Gambar.textBox1.Text = "1";
                    formSwitcing4Gambar.Show();
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Stop Camera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Stop Record terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } 
        }

        private void btn_Record_OBS_Click(object sender, EventArgs e)
        {
            if (vCamera != true)
            {
                if (txt_Form.Text != "")
                {
                    MessageBox.Show("Tutup halaman terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    //FinalVideo.Stop();
                    //buttonRecStart.Enabled = true;
                    //buttonRecStart.BackColor = Color.FromArgb(0, 107, 150);
                    //btn_Record_OBS.BackColor = Color.FromArgb(0, 85, 119);
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WorkingDirectory = "C:/Program Files/obs-studio/bin/64bit"; // like cd path command
                    startInfo.FileName = "obs64.exe";
                    Process.Start(startInfo);
                    //btn_Record_OBS.Enabled = false;
                    //buttonRecSave.Enabled = false;
                }
            }
            else
            {
                if (vRecord != true)
                {
                    MessageBox.Show("Tekan Stop Camera terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Tekan Stop Record terlebih dahulu ", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        } 

        private void frm_TEFS4Gambar(string value)
        {
            txt_kondisi.Text = value;
        }  

        private void frm_TransfEvent3(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEvent4(string value)
        {
            txt_kondisi.Text = value;
        }

        private void frm_TransfEventPrint1(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint4(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint6(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint14(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventPrint16(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEvent61(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEvent64(string value)
        {
            textBoxPrint.Text = value;
        }

        private void frm_TransfEventFormat2(string value)
        { 
            txt_kondisi.Text = value;
        }

        private void getPatient()
        { 

            // Specify the path for the CSV file
            string csvFilePath = "D:\\GLEndoscope\\Database\\dataPasien\\dataDefault.csv";

            // Check if the CSV file exists
            if (File.Exists(csvFilePath))
            {
                // Call the method to read data from the CSV file
                ReadDataFromCSV(csvFilePath);
            }
            else
            {
                // Handle the case where the file does not exist
                //Console.WriteLine("CSV file does not exist.");
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
                        var noRM = csv.GetField<string>("Rm");
                        var name = csv.GetField<string>("Nama");
                        var action = csv.GetField<string>("Jenis Pemeriksaan");
                        var date = csv.GetField<string>("Tanggal Kunjungan");
                        gabung = noRM + "-" + name;
                        //Name = name;
                        //action1 = action;

                        // Tampilkan data di Label dan RichTextBox
                        lblCode.Text = noRM;
                        richTextBox1.Text = name;
                        //richTextBox1.Visible = false;

                        //MessageBox.Show($"Nilai name: {name}\nNilai action: {action}", "Nilai Name dan Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tidak ada data yang tersedia. Mohon isi data Pasien terlebih dahulu.", "Informasi!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
