﻿
namespace WindowsFormsApp1
{
    partial class FormUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUser));
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.txt_Search1 = new System.Windows.Forms.TextBox();
            this.btn_Search1 = new System.Windows.Forms.Button();
            this.btn_DeleteForm = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Nama = new System.Windows.Forms.TextBox();
            this.txt_Tindakan = new System.Windows.Forms.TextBox();
            this.panelUser = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxIDPasien = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBoxDokter = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.radioButtonWanita = new System.Windows.Forms.RadioButton();
            this.radioButtonPria = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_Rm = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Umur = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_Save = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelUser.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(185, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(0, 0);
            this.button1.TabIndex = 71;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(1323, 949);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(232, 36);
            this.button3.TabIndex = 69;
            this.button3.Text = "Keluar";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(673, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 31);
            this.label5.TabIndex = 68;
            this.label5.Text = "Data Pasien";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(84)))), ((int)(((byte)(175)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(12, 949);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(232, 36);
            this.button2.TabIndex = 69;
            this.button2.Text = "Setel Utama";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("btn_Refresh.Image")));
            this.btn_Refresh.Location = new System.Drawing.Point(1491, 9);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(30, 30);
            this.btn_Refresh.TabIndex = 67;
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // txt_Search1
            // 
            this.txt_Search1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Search1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_Search1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Search1.Location = new System.Drawing.Point(1154, 12);
            this.txt_Search1.Name = "txt_Search1";
            this.txt_Search1.Size = new System.Drawing.Size(288, 24);
            this.txt_Search1.TabIndex = 65;
            // 
            // btn_Search1
            // 
            this.btn_Search1.Image = ((System.Drawing.Image)(resources.GetObject("btn_Search1.Image")));
            this.btn_Search1.Location = new System.Drawing.Point(1452, 8);
            this.btn_Search1.Name = "btn_Search1";
            this.btn_Search1.Size = new System.Drawing.Size(30, 30);
            this.btn_Search1.TabIndex = 64;
            this.btn_Search1.UseVisualStyleBackColor = true;
            this.btn_Search1.Click += new System.EventHandler(this.btn_Search1_Click);
            // 
            // btn_DeleteForm
            // 
            this.btn_DeleteForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_DeleteForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_DeleteForm.ForeColor = System.Drawing.Color.White;
            this.btn_DeleteForm.Image = ((System.Drawing.Image)(resources.GetObject("btn_DeleteForm.Image")));
            this.btn_DeleteForm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_DeleteForm.Location = new System.Drawing.Point(1251, 316);
            this.btn_DeleteForm.Name = "btn_DeleteForm";
            this.btn_DeleteForm.Size = new System.Drawing.Size(141, 38);
            this.btn_DeleteForm.TabIndex = 63;
            this.btn_DeleteForm.Text = "Hapus";
            this.btn_DeleteForm.UseVisualStyleBackColor = false;
            this.btn_DeleteForm.Click += new System.EventHandler(this.btn_DeleteForm_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.Location = new System.Drawing.Point(956, 318);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(141, 36);
            this.btn_Cancel.TabIndex = 61;
            this.btn_Cancel.Text = "Batal";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(791, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 18);
            this.label7.TabIndex = 58;
            this.label7.Text = "Alamat";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(783, 249);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 18);
            this.label4.TabIndex = 48;
            this.label4.Text = "Jenis Pemeriksaan";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 44;
            this.label1.Text = "Nama";
            // 
            // txt_Nama
            // 
            this.txt_Nama.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Nama.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_Nama.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Nama.Location = new System.Drawing.Point(178, 113);
            this.txt_Nama.MaxLength = 30;
            this.txt_Nama.Name = "txt_Nama";
            this.txt_Nama.Size = new System.Drawing.Size(548, 24);
            this.txt_Nama.TabIndex = 1;
            // 
            // txt_Tindakan
            // 
            this.txt_Tindakan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Tindakan.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_Tindakan.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Tindakan.Location = new System.Drawing.Point(944, 247);
            this.txt_Tindakan.Name = "txt_Tindakan";
            this.txt_Tindakan.Size = new System.Drawing.Size(595, 24);
            this.txt_Tindakan.TabIndex = 12;
            // 
            // panelUser
            // 
            this.panelUser.BackColor = System.Drawing.Color.White;
            this.panelUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUser.Controls.Add(this.button5);
            this.panelUser.Controls.Add(this.dateTimePicker2);
            this.panelUser.Controls.Add(this.dateTimePicker1);
            this.panelUser.Controls.Add(this.richTextBox1);
            this.panelUser.Controls.Add(this.button4);
            this.panelUser.Controls.Add(this.textBox4);
            this.panelUser.Controls.Add(this.label6);
            this.panelUser.Controls.Add(this.textBoxIDPasien);
            this.panelUser.Controls.Add(this.textBox3);
            this.panelUser.Controls.Add(this.textBox2);
            this.panelUser.Controls.Add(this.textBox1);
            this.panelUser.Controls.Add(this.comboBoxDokter);
            this.panelUser.Controls.Add(this.label11);
            this.panelUser.Controls.Add(this.radioButtonWanita);
            this.panelUser.Controls.Add(this.radioButtonPria);
            this.panelUser.Controls.Add(this.label10);
            this.panelUser.Controls.Add(this.label8);
            this.panelUser.Controls.Add(this.label9);
            this.panelUser.Controls.Add(this.txt_Rm);
            this.panelUser.Controls.Add(this.label3);
            this.panelUser.Controls.Add(this.txt_Umur);
            this.panelUser.Controls.Add(this.panel1);
            this.panelUser.Controls.Add(this.button3);
            this.panelUser.Controls.Add(this.label5);
            this.panelUser.Controls.Add(this.label4);
            this.panelUser.Controls.Add(this.button2);
            this.panelUser.Controls.Add(this.label7);
            this.panelUser.Controls.Add(this.txt_Tindakan);
            this.panelUser.Controls.Add(this.btn_Save);
            this.panelUser.Controls.Add(this.label1);
            this.panelUser.Controls.Add(this.btn_Cancel);
            this.panelUser.Controls.Add(this.txt_Nama);
            this.panelUser.Controls.Add(this.btn_DeleteForm);
            this.panelUser.Location = new System.Drawing.Point(12, 12);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(1563, 992);
            this.panelUser.TabIndex = 72;
            this.panelUser.Paint += new System.Windows.Forms.PaintEventHandler(this.panelUser_Paint);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(1104, 316);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(141, 38);
            this.button5.TabIndex = 118;
            this.button5.Text = "Ubah";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Location = new System.Drawing.Point(944, 85);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(595, 22);
            this.dateTimePicker2.TabIndex = 117;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(177, 147);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(549, 22);
            this.dateTimePicker1.TabIndex = 116;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(944, 114);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(595, 127);
            this.richTextBox1.TabIndex = 115;
            this.richTextBox1.Text = "";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(84)))), ((int)(((byte)(175)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(12, 389);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(232, 36);
            this.button4.TabIndex = 113;
            this.button4.Text = "Ekspor Excel";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(198, 316);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(0, 20);
            this.textBox4.TabIndex = 107;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(15, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 18);
            this.label6.TabIndex = 93;
            this.label6.Text = "Tanggal Lahir";
            // 
            // textBoxIDPasien
            // 
            this.textBoxIDPasien.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxIDPasien.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIDPasien.Location = new System.Drawing.Point(479, 5);
            this.textBoxIDPasien.Name = "textBoxIDPasien";
            this.textBoxIDPasien.Size = new System.Drawing.Size(0, 24);
            this.textBoxIDPasien.TabIndex = 91;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(449, 16);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(0, 20);
            this.textBox3.TabIndex = 90;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(343, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(0, 20);
            this.textBox2.TabIndex = 88;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(0, 20);
            this.textBox1.TabIndex = 87;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // comboBoxDokter
            // 
            this.comboBoxDokter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDokter.FormattingEnabled = true;
            this.comboBoxDokter.Location = new System.Drawing.Point(178, 245);
            this.comboBoxDokter.Name = "comboBoxDokter";
            this.comboBoxDokter.Size = new System.Drawing.Size(548, 26);
            this.comboBoxDokter.TabIndex = 6;
            this.comboBoxDokter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxDokter_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(15, 248);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 18);
            this.label11.TabIndex = 85;
            this.label11.Text = "Nama Dokter";
            // 
            // radioButtonWanita
            // 
            this.radioButtonWanita.AutoSize = true;
            this.radioButtonWanita.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonWanita.Location = new System.Drawing.Point(325, 211);
            this.radioButtonWanita.Name = "radioButtonWanita";
            this.radioButtonWanita.Size = new System.Drawing.Size(111, 22);
            this.radioButtonWanita.TabIndex = 5;
            this.radioButtonWanita.TabStop = true;
            this.radioButtonWanita.Text = "Perempuan";
            this.radioButtonWanita.UseVisualStyleBackColor = true;
            // 
            // radioButtonPria
            // 
            this.radioButtonPria.AutoSize = true;
            this.radioButtonPria.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonPria.Location = new System.Drawing.Point(178, 210);
            this.radioButtonPria.Name = "radioButtonPria";
            this.radioButtonPria.Size = new System.Drawing.Size(99, 22);
            this.radioButtonPria.TabIndex = 4;
            this.radioButtonPria.TabStop = true;
            this.radioButtonPria.Text = "Laki - laki";
            this.radioButtonPria.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(15, 208);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 18);
            this.label10.TabIndex = 82;
            this.label10.Text = "Jenis Kelamin";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(786, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 18);
            this.label8.TabIndex = 78;
            this.label8.Text = "Tanggal Kunjungan";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(15, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 18);
            this.label9.TabIndex = 77;
            this.label9.Text = "No. RM";
            // 
            // txt_Rm
            // 
            this.txt_Rm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Rm.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_Rm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Rm.Location = new System.Drawing.Point(178, 84);
            this.txt_Rm.MaxLength = 14;
            this.txt_Rm.Name = "txt_Rm";
            this.txt_Rm.Size = new System.Drawing.Size(548, 24);
            this.txt_Rm.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 18);
            this.label3.TabIndex = 75;
            this.label3.Text = "Umur";
            // 
            // txt_Umur
            // 
            this.txt_Umur.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Umur.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Umur.Location = new System.Drawing.Point(178, 175);
            this.txt_Umur.MaxLength = 14;
            this.txt_Umur.Name = "txt_Umur";
            this.txt_Umur.ReadOnly = true;
            this.txt_Umur.Size = new System.Drawing.Size(548, 24);
            this.txt_Umur.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.txt_Search1);
            this.panel1.Controls.Add(this.btn_Search1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btn_Refresh);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(12, 431);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1543, 511);
            this.panel1.TabIndex = 72;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(21, 64);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1499, 381);
            this.dataGridView1.TabIndex = 72;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(129)))), ((int)(((byte)(13)))));
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.ForeColor = System.Drawing.Color.White;
            this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
            this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Save.Location = new System.Drawing.Point(1398, 316);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(141, 38);
            this.btn_Save.TabIndex = 60;
            this.btn_Save.Text = "Simpan";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // FormUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1587, 1016);
            this.ControlBox = false;
            this.Controls.Add(this.panelUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.FormUser_Load);
            this.panelUser.ResumeLayout(false);
            this.panelUser.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.TextBox txt_Search1;
        private System.Windows.Forms.Button btn_Search1;
        private System.Windows.Forms.Button btn_DeleteForm;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Nama;
        private System.Windows.Forms.TextBox txt_Tindakan;
        private System.Windows.Forms.Panel panelUser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonWanita;
        private System.Windows.Forms.RadioButton radioButtonPria;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_Rm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Umur;
        private System.Windows.Forms.ComboBox comboBoxDokter;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBoxIDPasien;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button button5;
        public System.Windows.Forms.TextBox textBox4;
    }
}