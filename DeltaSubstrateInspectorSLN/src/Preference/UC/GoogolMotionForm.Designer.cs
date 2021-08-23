namespace DeltaSubstrateInspector.src.Preference.UC
{
    partial class GoogolMotionForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hWindowControl_move = new HalconDotNet.HWindowControl();
            this.backgroundWorker_display = new System.ComponentModel.BackgroundWorker();
            this.groupBox_status = new System.Windows.Forms.GroupBox();
            this.btn_closing = new System.Windows.Forms.Button();
            this.btn_digitalIO_form = new System.Windows.Forms.Button();
            this.lab_axis3_conn = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_connecting = new System.Windows.Forms.Button();
            this.lab_axis2_conn = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lab_axis1_conn = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lab_googol_conn = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_adda_model_initial = new System.Windows.Forms.Button();
            this.btn_setting_sync_voltage = new System.Windows.Forms.Button();
            this.txt_sync_voltage = new System.Windows.Forms.TextBox();
            this.label58 = new System.Windows.Forms.Label();
            this.btn_model1_ch_4 = new System.Windows.Forms.Button();
            this.btn_model1_ch_3 = new System.Windows.Forms.Button();
            this.btn_model1_ch_2 = new System.Windows.Forms.Button();
            this.btn_model1_ch_1 = new System.Windows.Forms.Button();
            this.txt_model1_ch_1 = new System.Windows.Forms.TextBox();
            this.txt_model1_ch_2 = new System.Windows.Forms.TextBox();
            this.txt_model1_ch_3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_model1_ch_4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_signal = new System.Windows.Forms.TextBox();
            this.btn_run = new System.Windows.Forms.Button();
            this.btn_home = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox_comport = new System.Windows.Forms.ComboBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_connect = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.lab_xyz_connect = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btn_go = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_speed = new System.Windows.Forms.TextBox();
            this.btn_set = new System.Windows.Forms.Button();
            this.btn_trigger_signal = new System.Windows.Forms.Button();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox_status.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.hWindowControl_move);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 735);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Move Path";
            // 
            // hWindowControl_move
            // 
            this.hWindowControl_move.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_move.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_move.ImagePart = new System.Drawing.Rectangle(0, 0, 500, 700);
            this.hWindowControl_move.Location = new System.Drawing.Point(6, 21);
            this.hWindowControl_move.Name = "hWindowControl_move";
            this.hWindowControl_move.Size = new System.Drawing.Size(500, 700);
            this.hWindowControl_move.TabIndex = 0;
            this.hWindowControl_move.WindowSize = new System.Drawing.Size(500, 700);
            // 
            // backgroundWorker_display
            // 
            this.backgroundWorker_display.WorkerReportsProgress = true;
            this.backgroundWorker_display.WorkerSupportsCancellation = true;
            this.backgroundWorker_display.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_display_DoWork);
            this.backgroundWorker_display.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_display_ProgressChanged);
            // 
            // groupBox_status
            // 
            this.groupBox_status.Controls.Add(this.btn_closing);
            this.groupBox_status.Controls.Add(this.btn_digitalIO_form);
            this.groupBox_status.Controls.Add(this.lab_axis3_conn);
            this.groupBox_status.Controls.Add(this.label6);
            this.groupBox_status.Controls.Add(this.btn_connecting);
            this.groupBox_status.Controls.Add(this.lab_axis2_conn);
            this.groupBox_status.Controls.Add(this.label4);
            this.groupBox_status.Controls.Add(this.lab_axis1_conn);
            this.groupBox_status.Controls.Add(this.label3);
            this.groupBox_status.Controls.Add(this.lab_googol_conn);
            this.groupBox_status.Controls.Add(this.label1);
            this.groupBox_status.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.groupBox_status.Location = new System.Drawing.Point(6, 6);
            this.groupBox_status.Name = "groupBox_status";
            this.groupBox_status.Size = new System.Drawing.Size(350, 238);
            this.groupBox_status.TabIndex = 1;
            this.groupBox_status.TabStop = false;
            this.groupBox_status.Text = "Motion Status";
            // 
            // btn_closing
            // 
            this.btn_closing.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_closing.Location = new System.Drawing.Point(243, 198);
            this.btn_closing.Name = "btn_closing";
            this.btn_closing.Size = new System.Drawing.Size(100, 34);
            this.btn_closing.TabIndex = 30;
            this.btn_closing.Text = "Close";
            this.btn_closing.UseVisualStyleBackColor = true;
            this.btn_closing.Click += new System.EventHandler(this.btn_closing_Click);
            // 
            // btn_digitalIO_form
            // 
            this.btn_digitalIO_form.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_digitalIO_form.Location = new System.Drawing.Point(126, 198);
            this.btn_digitalIO_form.Name = "btn_digitalIO_form";
            this.btn_digitalIO_form.Size = new System.Drawing.Size(100, 34);
            this.btn_digitalIO_form.TabIndex = 31;
            this.btn_digitalIO_form.Text = "Digital IO";
            this.btn_digitalIO_form.UseVisualStyleBackColor = true;
            this.btn_digitalIO_form.Click += new System.EventHandler(this.btn_digitalIO_form_Click);
            // 
            // lab_axis3_conn
            // 
            this.lab_axis3_conn.BackColor = System.Drawing.Color.Red;
            this.lab_axis3_conn.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.lab_axis3_conn.ForeColor = System.Drawing.Color.Snow;
            this.lab_axis3_conn.Location = new System.Drawing.Point(101, 150);
            this.lab_axis3_conn.Name = "lab_axis3_conn";
            this.lab_axis3_conn.Size = new System.Drawing.Size(243, 32);
            this.lab_axis3_conn.TabIndex = 8;
            this.lab_axis3_conn.Text = "Disconnecting";
            this.lab_axis3_conn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label6.Location = new System.Drawing.Point(38, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "Axis 3：";
            // 
            // btn_connecting
            // 
            this.btn_connecting.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_connecting.Location = new System.Drawing.Point(9, 198);
            this.btn_connecting.Name = "btn_connecting";
            this.btn_connecting.Size = new System.Drawing.Size(100, 34);
            this.btn_connecting.TabIndex = 29;
            this.btn_connecting.Text = "Connect";
            this.btn_connecting.UseVisualStyleBackColor = true;
            this.btn_connecting.Click += new System.EventHandler(this.btn_connecting_Click);
            // 
            // lab_axis2_conn
            // 
            this.lab_axis2_conn.BackColor = System.Drawing.Color.Red;
            this.lab_axis2_conn.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.lab_axis2_conn.ForeColor = System.Drawing.Color.Snow;
            this.lab_axis2_conn.Location = new System.Drawing.Point(101, 107);
            this.lab_axis2_conn.Name = "lab_axis2_conn";
            this.lab_axis2_conn.Size = new System.Drawing.Size(243, 32);
            this.lab_axis2_conn.TabIndex = 6;
            this.lab_axis2_conn.Text = "Disconnecting";
            this.lab_axis2_conn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label4.Location = new System.Drawing.Point(38, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Axis 2：";
            // 
            // lab_axis1_conn
            // 
            this.lab_axis1_conn.BackColor = System.Drawing.Color.Red;
            this.lab_axis1_conn.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.lab_axis1_conn.ForeColor = System.Drawing.Color.Snow;
            this.lab_axis1_conn.Location = new System.Drawing.Point(101, 64);
            this.lab_axis1_conn.Name = "lab_axis1_conn";
            this.lab_axis1_conn.Size = new System.Drawing.Size(243, 32);
            this.lab_axis1_conn.TabIndex = 4;
            this.lab_axis1_conn.Text = "Disconnecting";
            this.lab_axis1_conn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label3.Location = new System.Drawing.Point(38, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Axis 1：";
            // 
            // lab_googol_conn
            // 
            this.lab_googol_conn.BackColor = System.Drawing.Color.Red;
            this.lab_googol_conn.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.lab_googol_conn.ForeColor = System.Drawing.Color.Snow;
            this.lab_googol_conn.Location = new System.Drawing.Point(101, 21);
            this.lab_googol_conn.Name = "lab_googol_conn";
            this.lab_googol_conn.Size = new System.Drawing.Size(243, 32);
            this.lab_googol_conn.TabIndex = 2;
            this.lab_googol_conn.Text = "Disconnecting";
            this.lab_googol_conn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Motion Card：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_adda_model_initial);
            this.groupBox2.Controls.Add(this.btn_setting_sync_voltage);
            this.groupBox2.Controls.Add(this.txt_sync_voltage);
            this.groupBox2.Controls.Add(this.label58);
            this.groupBox2.Controls.Add(this.btn_model1_ch_4);
            this.groupBox2.Controls.Add(this.btn_model1_ch_3);
            this.groupBox2.Controls.Add(this.btn_model1_ch_2);
            this.groupBox2.Controls.Add(this.btn_model1_ch_1);
            this.groupBox2.Controls.Add(this.txt_model1_ch_1);
            this.groupBox2.Controls.Add(this.txt_model1_ch_2);
            this.groupBox2.Controls.Add(this.txt_model1_ch_3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txt_model1_ch_4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.groupBox2.Location = new System.Drawing.Point(6, 250);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 227);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Digital -> Analog Channel";
            // 
            // btn_adda_model_initial
            // 
            this.btn_adda_model_initial.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.btn_adda_model_initial.Location = new System.Drawing.Point(282, 21);
            this.btn_adda_model_initial.Name = "btn_adda_model_initial";
            this.btn_adda_model_initial.Size = new System.Drawing.Size(62, 155);
            this.btn_adda_model_initial.TabIndex = 74;
            this.btn_adda_model_initial.Text = "Zero ";
            this.btn_adda_model_initial.UseVisualStyleBackColor = true;
            this.btn_adda_model_initial.Click += new System.EventHandler(this.btn_adda_model_initial_Click);
            // 
            // btn_setting_sync_voltage
            // 
            this.btn_setting_sync_voltage.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.btn_setting_sync_voltage.Location = new System.Drawing.Point(207, 187);
            this.btn_setting_sync_voltage.Name = "btn_setting_sync_voltage";
            this.btn_setting_sync_voltage.Size = new System.Drawing.Size(137, 29);
            this.btn_setting_sync_voltage.TabIndex = 73;
            this.btn_setting_sync_voltage.Text = "Sync Setting";
            this.btn_setting_sync_voltage.UseVisualStyleBackColor = true;
            this.btn_setting_sync_voltage.Click += new System.EventHandler(this.btn_setting_sync_voltage_Click);
            // 
            // txt_sync_voltage
            // 
            this.txt_sync_voltage.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_sync_voltage.Location = new System.Drawing.Point(101, 193);
            this.txt_sync_voltage.MaxLength = 5;
            this.txt_sync_voltage.Name = "txt_sync_voltage";
            this.txt_sync_voltage.Size = new System.Drawing.Size(100, 23);
            this.txt_sync_voltage.TabIndex = 72;
            this.txt_sync_voltage.Text = "0";
            this.txt_sync_voltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label58.Location = new System.Drawing.Point(6, 197);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(88, 16);
            this.label58.TabIndex = 71;
            this.label58.Text = "Sync Voltage :";
            // 
            // btn_model1_ch_4
            // 
            this.btn_model1_ch_4.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.btn_model1_ch_4.Location = new System.Drawing.Point(207, 150);
            this.btn_model1_ch_4.Name = "btn_model1_ch_4";
            this.btn_model1_ch_4.Size = new System.Drawing.Size(70, 26);
            this.btn_model1_ch_4.TabIndex = 37;
            this.btn_model1_ch_4.Text = "Output";
            this.btn_model1_ch_4.UseVisualStyleBackColor = true;
            this.btn_model1_ch_4.Click += new System.EventHandler(this.btn_model1_ch_4_Click);
            // 
            // btn_model1_ch_3
            // 
            this.btn_model1_ch_3.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.btn_model1_ch_3.Location = new System.Drawing.Point(207, 107);
            this.btn_model1_ch_3.Name = "btn_model1_ch_3";
            this.btn_model1_ch_3.Size = new System.Drawing.Size(70, 26);
            this.btn_model1_ch_3.TabIndex = 36;
            this.btn_model1_ch_3.Text = "Output";
            this.btn_model1_ch_3.UseVisualStyleBackColor = true;
            this.btn_model1_ch_3.Click += new System.EventHandler(this.btn_model1_ch_3_Click);
            // 
            // btn_model1_ch_2
            // 
            this.btn_model1_ch_2.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.btn_model1_ch_2.Location = new System.Drawing.Point(207, 64);
            this.btn_model1_ch_2.Name = "btn_model1_ch_2";
            this.btn_model1_ch_2.Size = new System.Drawing.Size(70, 26);
            this.btn_model1_ch_2.TabIndex = 35;
            this.btn_model1_ch_2.Text = "Output";
            this.btn_model1_ch_2.UseVisualStyleBackColor = true;
            this.btn_model1_ch_2.Click += new System.EventHandler(this.btn_model1_ch_2_Click);
            // 
            // btn_model1_ch_1
            // 
            this.btn_model1_ch_1.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.btn_model1_ch_1.Location = new System.Drawing.Point(207, 21);
            this.btn_model1_ch_1.Name = "btn_model1_ch_1";
            this.btn_model1_ch_1.Size = new System.Drawing.Size(70, 26);
            this.btn_model1_ch_1.TabIndex = 34;
            this.btn_model1_ch_1.Text = "Output";
            this.btn_model1_ch_1.UseVisualStyleBackColor = true;
            this.btn_model1_ch_1.Click += new System.EventHandler(this.btn_model1_ch_1_Click);
            // 
            // txt_model1_ch_1
            // 
            this.txt_model1_ch_1.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_model1_ch_1.Location = new System.Drawing.Point(101, 25);
            this.txt_model1_ch_1.Name = "txt_model1_ch_1";
            this.txt_model1_ch_1.Size = new System.Drawing.Size(100, 23);
            this.txt_model1_ch_1.TabIndex = 27;
            this.txt_model1_ch_1.Text = "0";
            this.txt_model1_ch_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_model1_ch_2
            // 
            this.txt_model1_ch_2.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_model1_ch_2.Location = new System.Drawing.Point(101, 67);
            this.txt_model1_ch_2.Name = "txt_model1_ch_2";
            this.txt_model1_ch_2.Size = new System.Drawing.Size(100, 23);
            this.txt_model1_ch_2.TabIndex = 28;
            this.txt_model1_ch_2.Text = "0";
            this.txt_model1_ch_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_model1_ch_3
            // 
            this.txt_model1_ch_3.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_model1_ch_3.Location = new System.Drawing.Point(101, 109);
            this.txt_model1_ch_3.Name = "txt_model1_ch_3";
            this.txt_model1_ch_3.Size = new System.Drawing.Size(100, 23);
            this.txt_model1_ch_3.TabIndex = 29;
            this.txt_model1_ch_3.Text = "0";
            this.txt_model1_ch_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label2.Location = new System.Drawing.Point(23, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 16);
            this.label2.TabIndex = 33;
            this.label2.Text = "Channel 4 :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label12.Location = new System.Drawing.Point(23, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 16);
            this.label12.TabIndex = 26;
            this.label12.Text = "Channel 1 :";
            // 
            // txt_model1_ch_4
            // 
            this.txt_model1_ch_4.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_model1_ch_4.Location = new System.Drawing.Point(101, 151);
            this.txt_model1_ch_4.Name = "txt_model1_ch_4";
            this.txt_model1_ch_4.Size = new System.Drawing.Size(100, 23);
            this.txt_model1_ch_4.TabIndex = 30;
            this.txt_model1_ch_4.Text = "0";
            this.txt_model1_ch_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label5.Location = new System.Drawing.Point(23, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 16);
            this.label5.TabIndex = 32;
            this.label5.Text = "Channel 3 :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label7.Location = new System.Drawing.Point(23, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 16);
            this.label7.TabIndex = 31;
            this.label7.Text = "Channel 2 :";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(534, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(368, 510);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage1.Controls.Add(this.groupBox_status);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(360, 484);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Googol Motion";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(360, 484);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "XYZ Table";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_port);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.btn_trigger_signal);
            this.groupBox4.Controls.Add(this.btn_set);
            this.groupBox4.Controls.Add(this.txt_speed);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.btn_go);
            this.groupBox4.Controls.Add(this.txt_signal);
            this.groupBox4.Controls.Add(this.btn_run);
            this.groupBox4.Controls.Add(this.btn_home);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.groupBox4.Location = new System.Drawing.Point(6, 128);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(350, 346);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Motion Setting";
            // 
            // txt_signal
            // 
            this.txt_signal.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_signal.Location = new System.Drawing.Point(188, 29);
            this.txt_signal.Name = "txt_signal";
            this.txt_signal.Size = new System.Drawing.Size(100, 23);
            this.txt_signal.TabIndex = 32;
            this.txt_signal.Text = "0";
            this.txt_signal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_run
            // 
            this.btn_run.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_run.Location = new System.Drawing.Point(87, 306);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(75, 34);
            this.btn_run.TabIndex = 31;
            this.btn_run.Text = "Run";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // btn_home
            // 
            this.btn_home.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_home.Location = new System.Drawing.Point(6, 306);
            this.btn_home.Name = "btn_home";
            this.btn_home.Size = new System.Drawing.Size(75, 34);
            this.btn_home.TabIndex = 30;
            this.btn_home.Text = "Home";
            this.btn_home.UseVisualStyleBackColor = true;
            this.btn_home.Click += new System.EventHandler(this.btn_home_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label10.Location = new System.Drawing.Point(12, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(177, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "Output Signal Wait Time(s)：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox_comport);
            this.groupBox3.Controls.Add(this.btn_close);
            this.groupBox3.Controls.Add(this.btn_connect);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.lab_xyz_connect);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(350, 116);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Motion Status";
            // 
            // comboBox_comport
            // 
            this.comboBox_comport.FormattingEnabled = true;
            this.comboBox_comport.Location = new System.Drawing.Point(102, 71);
            this.comboBox_comport.Name = "comboBox_comport";
            this.comboBox_comport.Size = new System.Drawing.Size(73, 27);
            this.comboBox_comport.TabIndex = 32;
            // 
            // btn_close
            // 
            this.btn_close.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_close.Location = new System.Drawing.Point(269, 66);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 34);
            this.btn_close.TabIndex = 30;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_connect
            // 
            this.btn_connect.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_connect.Location = new System.Drawing.Point(188, 66);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 34);
            this.btn_connect.TabIndex = 29;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label14.Location = new System.Drawing.Point(22, 77);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 16);
            this.label14.TabIndex = 3;
            this.label14.Text = "ComPort：";
            // 
            // lab_xyz_connect
            // 
            this.lab_xyz_connect.BackColor = System.Drawing.Color.Red;
            this.lab_xyz_connect.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.lab_xyz_connect.ForeColor = System.Drawing.Color.Snow;
            this.lab_xyz_connect.Location = new System.Drawing.Point(101, 21);
            this.lab_xyz_connect.Name = "lab_xyz_connect";
            this.lab_xyz_connect.Size = new System.Drawing.Size(243, 32);
            this.lab_xyz_connect.TabIndex = 2;
            this.lab_xyz_connect.Text = "Disconnecting";
            this.lab_xyz_connect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label16.Location = new System.Drawing.Point(12, 32);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 16);
            this.label16.TabIndex = 0;
            this.label16.Text = "XYZ Table：";
            // 
            // btn_go
            // 
            this.btn_go.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_go.Location = new System.Drawing.Point(168, 306);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(75, 34);
            this.btn_go.TabIndex = 33;
            this.btn_go.Text = "Go";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label8.Location = new System.Drawing.Point(133, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 16);
            this.label8.TabIndex = 34;
            this.label8.Text = "Speed：";
            // 
            // txt_speed
            // 
            this.txt_speed.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_speed.Location = new System.Drawing.Point(188, 59);
            this.txt_speed.Name = "txt_speed";
            this.txt_speed.Size = new System.Drawing.Size(100, 23);
            this.txt_speed.TabIndex = 35;
            this.txt_speed.Text = "100";
            this.txt_speed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_set
            // 
            this.btn_set.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_set.Location = new System.Drawing.Point(294, 29);
            this.btn_set.Name = "btn_set";
            this.btn_set.Size = new System.Drawing.Size(50, 53);
            this.btn_set.TabIndex = 36;
            this.btn_set.Text = "Set";
            this.btn_set.UseVisualStyleBackColor = true;
            this.btn_set.Click += new System.EventHandler(this.btn_set_Click);
            // 
            // btn_trigger_signal
            // 
            this.btn_trigger_signal.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_trigger_signal.Location = new System.Drawing.Point(6, 266);
            this.btn_trigger_signal.Name = "btn_trigger_signal";
            this.btn_trigger_signal.Size = new System.Drawing.Size(156, 34);
            this.btn_trigger_signal.TabIndex = 37;
            this.btn_trigger_signal.Text = "Trigger Signal";
            this.btn_trigger_signal.UseVisualStyleBackColor = true;
            this.btn_trigger_signal.Click += new System.EventHandler(this.btn_trigger_signal_Click);
            // 
            // txt_port
            // 
            this.txt_port.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txt_port.Location = new System.Drawing.Point(188, 88);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(100, 23);
            this.txt_port.TabIndex = 39;
            this.txt_port.Text = "12";
            this.txt_port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.label9.Location = new System.Drawing.Point(55, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 16);
            this.label9.TabIndex = 38;
            this.label9.Text = "Output Signal Port：";
            // 
            // GoogolMotionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 741);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "GoogolMotionForm";
            this.Text = "MotionForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GoogolMotionForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox_status.ResumeLayout(false);
            this.groupBox_status.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private HalconDotNet.HWindowControl hWindowControl_move;
        private System.ComponentModel.BackgroundWorker backgroundWorker_display;
        private System.Windows.Forms.GroupBox groupBox_status;
        private System.Windows.Forms.Label lab_axis1_conn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lab_googol_conn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lab_axis3_conn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lab_axis2_conn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_adda_model_initial;
        private System.Windows.Forms.Button btn_setting_sync_voltage;
        private System.Windows.Forms.TextBox txt_sync_voltage;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Button btn_model1_ch_4;
        private System.Windows.Forms.Button btn_model1_ch_3;
        private System.Windows.Forms.Button btn_model1_ch_2;
        private System.Windows.Forms.Button btn_model1_ch_1;
        private System.Windows.Forms.TextBox txt_model1_ch_1;
        private System.Windows.Forms.TextBox txt_model1_ch_2;
        private System.Windows.Forms.TextBox txt_model1_ch_3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_model1_ch_4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_closing;
        private System.Windows.Forms.Button btn_digitalIO_form;
        private System.Windows.Forms.Button btn_connecting;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lab_xyz_connect;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox comboBox_comport;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_home;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.TextBox txt_signal;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.TextBox txt_speed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_set;
        private System.Windows.Forms.Button btn_trigger_signal;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label9;
    }
}