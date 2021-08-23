namespace PM_24084R
{
    partial class Form_24084R
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lb_L2_MaxC = new System.Windows.Forms.Label();
            this.lb_L2 = new System.Windows.Forms.Label();
            this.lb_L1_MIN = new System.Windows.Forms.Label();
            this.lb_L1_MAX = new System.Windows.Forms.Label();
            this.lb_L1_PCV = new System.Windows.Forms.Label();
            this.lb_L1_PC = new System.Windows.Forms.Label();
            this.tkb_L1_Output = new System.Windows.Forms.TrackBar();
            this.nud_L1_Output = new System.Windows.Forms.NumericUpDown();
            this.cbb_PortNo = new System.Windows.Forms.ComboBox();
            this.tss_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.tss_BaudRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tss_REV = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lb_L1_Output = new System.Windows.Forms.Label();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.lb_PortNo = new System.Windows.Forms.Label();
            this.cbb_L2_MaxC = new System.Windows.Forms.ComboBox();
            this.gpb_L1 = new System.Windows.Forms.GroupBox();
            this.pn_L1_Output = new System.Windows.Forms.Panel();
            this.pn_L1_Set = new System.Windows.Forms.Panel();
            this.pn_L1_LED = new System.Windows.Forms.Panel();
            this.ckb_L1_EXT = new System.Windows.Forms.CheckBox();
            this.btn_L1 = new System.Windows.Forms.Button();
            this.lb_L1_MaxC = new System.Windows.Forms.Label();
            this.cbb_L1_MaxC = new System.Windows.Forms.ComboBox();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.pn_Port = new System.Windows.Forms.Panel();
            this.btn_L2 = new System.Windows.Forms.Button();
            this.lb_L1 = new System.Windows.Forms.Label();
            this.gpb_ON_OFF = new System.Windows.Forms.GroupBox();
            this.cbb_SYNC_EN = new System.Windows.Forms.ComboBox();
            this.lb_PWR_ERR = new System.Windows.Forms.Label();
            this.gpb_L2 = new System.Windows.Forms.GroupBox();
            this.pn_L2_Output = new System.Windows.Forms.Panel();
            this.lb_L2_MIN = new System.Windows.Forms.Label();
            this.lb_L2_MAX = new System.Windows.Forms.Label();
            this.lb_L2_PCV = new System.Windows.Forms.Label();
            this.lb_L2_PC = new System.Windows.Forms.Label();
            this.tkb_L2_Output = new System.Windows.Forms.TrackBar();
            this.nud_L2_Output = new System.Windows.Forms.NumericUpDown();
            this.lb_L2_Output = new System.Windows.Forms.Label();
            this.pn_L2_Set = new System.Windows.Forms.Panel();
            this.pn_L2_LED = new System.Windows.Forms.Panel();
            this.ckb_L2_EXT = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lb_L3 = new System.Windows.Forms.Label();
            this.gpb_L3 = new System.Windows.Forms.GroupBox();
            this.pn_L3_Output = new System.Windows.Forms.Panel();
            this.lb_L3_MIN = new System.Windows.Forms.Label();
            this.lb_L3_MAX = new System.Windows.Forms.Label();
            this.lb_L3_PCV = new System.Windows.Forms.Label();
            this.lb_L3_PC = new System.Windows.Forms.Label();
            this.tkb_L3_Output = new System.Windows.Forms.TrackBar();
            this.nud_L3_Output = new System.Windows.Forms.NumericUpDown();
            this.lb_L3_Output = new System.Windows.Forms.Label();
            this.pn_L3_Set = new System.Windows.Forms.Panel();
            this.pn_L3_LED = new System.Windows.Forms.Panel();
            this.ckb_L3_EXT = new System.Windows.Forms.CheckBox();
            this.btn_L3 = new System.Windows.Forms.Button();
            this.lb_L3_MaxC = new System.Windows.Forms.Label();
            this.cbb_L3_MaxC = new System.Windows.Forms.ComboBox();
            this.lb_L4 = new System.Windows.Forms.Label();
            this.gpb_L4 = new System.Windows.Forms.GroupBox();
            this.pn_L4_Output = new System.Windows.Forms.Panel();
            this.lb_L4_MIN = new System.Windows.Forms.Label();
            this.lb_L4_MAX = new System.Windows.Forms.Label();
            this.lb_L4_PCV = new System.Windows.Forms.Label();
            this.lb_L4_PC = new System.Windows.Forms.Label();
            this.tkb_L4_Output = new System.Windows.Forms.TrackBar();
            this.nud_L4_Output = new System.Windows.Forms.NumericUpDown();
            this.lb_L4_Output = new System.Windows.Forms.Label();
            this.pn_L4_Set = new System.Windows.Forms.Panel();
            this.pn_L4_LED = new System.Windows.Forms.Panel();
            this.ckb_L4_EXT = new System.Windows.Forms.CheckBox();
            this.btn_L4 = new System.Windows.Forms.Button();
            this.lb_L4_MaxC = new System.Windows.Forms.Label();
            this.cbb_L4_MaxC = new System.Windows.Forms.ComboBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.cbxLightOffSetEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L1_Output)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L1_Output)).BeginInit();
            this.gpb_L1.SuspendLayout();
            this.pn_L1_Output.SuspendLayout();
            this.pn_L1_Set.SuspendLayout();
            this.pn_Port.SuspendLayout();
            this.gpb_ON_OFF.SuspendLayout();
            this.gpb_L2.SuspendLayout();
            this.pn_L2_Output.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L2_Output)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L2_Output)).BeginInit();
            this.pn_L2_Set.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.gpb_L3.SuspendLayout();
            this.pn_L3_Output.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L3_Output)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L3_Output)).BeginInit();
            this.pn_L3_Set.SuspendLayout();
            this.gpb_L4.SuspendLayout();
            this.pn_L4_Output.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L4_Output)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L4_Output)).BeginInit();
            this.pn_L4_Set.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_L2_MaxC
            // 
            this.lb_L2_MaxC.AutoSize = true;
            this.lb_L2_MaxC.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L2_MaxC.ForeColor = System.Drawing.Color.Maroon;
            this.lb_L2_MaxC.Location = new System.Drawing.Point(8, 8);
            this.lb_L2_MaxC.Name = "lb_L2_MaxC";
            this.lb_L2_MaxC.Size = new System.Drawing.Size(96, 18);
            this.lb_L2_MaxC.TabIndex = 0;
            this.lb_L2_MaxC.Text = "Max. Current";
            // 
            // lb_L2
            // 
            this.lb_L2.AutoSize = true;
            this.lb_L2.BackColor = System.Drawing.Color.Black;
            this.lb_L2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L2.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L2.ForeColor = System.Drawing.Color.White;
            this.lb_L2.Location = new System.Drawing.Point(241, 12);
            this.lb_L2.MaximumSize = new System.Drawing.Size(194, 25);
            this.lb_L2.MinimumSize = new System.Drawing.Size(194, 25);
            this.lb_L2.Name = "lb_L2";
            this.lb_L2.Size = new System.Drawing.Size(194, 25);
            this.lb_L2.TabIndex = 6;
            this.lb_L2.Text = "L2";
            this.lb_L2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_L2.UseMnemonic = false;
            // 
            // lb_L1_MIN
            // 
            this.lb_L1_MIN.AutoSize = true;
            this.lb_L1_MIN.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L1_MIN.Location = new System.Drawing.Point(152, 112);
            this.lb_L1_MIN.Name = "lb_L1_MIN";
            this.lb_L1_MIN.Size = new System.Drawing.Size(28, 15);
            this.lb_L1_MIN.TabIndex = 6;
            this.lb_L1_MIN.Text = "MIN";
            // 
            // lb_L1_MAX
            // 
            this.lb_L1_MAX.AutoSize = true;
            this.lb_L1_MAX.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L1_MAX.Location = new System.Drawing.Point(152, 8);
            this.lb_L1_MAX.Name = "lb_L1_MAX";
            this.lb_L1_MAX.Size = new System.Drawing.Size(30, 15);
            this.lb_L1_MAX.TabIndex = 5;
            this.lb_L1_MAX.Text = "MAX";
            // 
            // lb_L1_PCV
            // 
            this.lb_L1_PCV.AutoSize = true;
            this.lb_L1_PCV.BackColor = System.Drawing.Color.Black;
            this.lb_L1_PCV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L1_PCV.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L1_PCV.ForeColor = System.Drawing.Color.Lime;
            this.lb_L1_PCV.Location = new System.Drawing.Point(25, 92);
            this.lb_L1_PCV.MaximumSize = new System.Drawing.Size(64, 32);
            this.lb_L1_PCV.MinimumSize = new System.Drawing.Size(64, 32);
            this.lb_L1_PCV.Name = "lb_L1_PCV";
            this.lb_L1_PCV.Size = new System.Drawing.Size(64, 32);
            this.lb_L1_PCV.TabIndex = 2;
            this.lb_L1_PCV.Text = "0.0";
            this.lb_L1_PCV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_L1_PCV.UseCompatibleTextRendering = true;
            this.lb_L1_PCV.UseMnemonic = false;
            // 
            // lb_L1_PC
            // 
            this.lb_L1_PC.AutoSize = true;
            this.lb_L1_PC.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L1_PC.Location = new System.Drawing.Point(92, 112);
            this.lb_L1_PC.Name = "lb_L1_PC";
            this.lb_L1_PC.Size = new System.Drawing.Size(18, 15);
            this.lb_L1_PC.TabIndex = 3;
            this.lb_L1_PC.Text = "%";
            // 
            // tkb_L1_Output
            // 
            this.tkb_L1_Output.BackColor = System.Drawing.Color.DarkGray;
            this.tkb_L1_Output.LargeChange = 10;
            this.tkb_L1_Output.Location = new System.Drawing.Point(124, 2);
            this.tkb_L1_Output.Margin = new System.Windows.Forms.Padding(0);
            this.tkb_L1_Output.Maximum = 500;
            this.tkb_L1_Output.Name = "tkb_L1_Output";
            this.tkb_L1_Output.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tkb_L1_Output.Size = new System.Drawing.Size(45, 130);
            this.tkb_L1_Output.TabIndex = 4;
            this.tkb_L1_Output.TabStop = false;
            this.tkb_L1_Output.TickFrequency = 50;
            this.tkb_L1_Output.ValueChanged += new System.EventHandler(this.tkb_L1_Output_ValueChanged);
            // 
            // nud_L1_Output
            // 
            this.nud_L1_Output.BackColor = System.Drawing.Color.Black;
            this.nud_L1_Output.Font = new System.Drawing.Font("Arial", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.nud_L1_Output.ForeColor = System.Drawing.Color.Cyan;
            this.nud_L1_Output.Location = new System.Drawing.Point(10, 28);
            this.nud_L1_Output.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_L1_Output.Name = "nud_L1_Output";
            this.nud_L1_Output.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nud_L1_Output.Size = new System.Drawing.Size(96, 53);
            this.nud_L1_Output.TabIndex = 1;
            this.nud_L1_Output.TabStop = false;
            this.nud_L1_Output.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_L1_Output.ValueChanged += new System.EventHandler(this.nud_L1_Output_ValueChanged);
            // 
            // cbb_PortNo
            // 
            this.cbb_PortNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_PortNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbb_PortNo.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cbb_PortNo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbb_PortNo.FormattingEnabled = true;
            this.cbb_PortNo.Location = new System.Drawing.Point(11, 28);
            this.cbb_PortNo.Name = "cbb_PortNo";
            this.cbb_PortNo.Size = new System.Drawing.Size(94, 26);
            this.cbb_PortNo.TabIndex = 1;
            this.cbb_PortNo.TabStop = false;
            this.cbb_PortNo.Click += new System.EventHandler(this.cbb_PortNo_Click);
            // 
            // tss_Status
            // 
            this.tss_Status.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tss_Status.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.tss_Status.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tss_Status.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.tss_Status.Name = "tss_Status";
            this.tss_Status.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.tss_Status.Size = new System.Drawing.Size(708, 20);
            this.tss_Status.Spring = true;
            this.tss_Status.Text = "Disconnect";
            this.tss_Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tss_BaudRate
            // 
            this.tss_BaudRate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tss_BaudRate.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.tss_BaudRate.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.tss_BaudRate.Name = "tss_BaudRate";
            this.tss_BaudRate.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.tss_BaudRate.Size = new System.Drawing.Size(114, 20);
            this.tss_BaudRate.Text = "115200 8-N-1";
            // 
            // tss_REV
            // 
            this.tss_REV.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tss_REV.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.tss_REV.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.tss_REV.Name = "tss_REV";
            this.tss_REV.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.tss_REV.Size = new System.Drawing.Size(57, 20);
            this.tss_REV.Text = "V1.0";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lb_L1_Output
            // 
            this.lb_L1_Output.AutoSize = true;
            this.lb_L1_Output.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L1_Output.ForeColor = System.Drawing.Color.Blue;
            this.lb_L1_Output.Location = new System.Drawing.Point(8, 8);
            this.lb_L1_Output.Name = "lb_L1_Output";
            this.lb_L1_Output.Size = new System.Drawing.Size(91, 18);
            this.lb_L1_Output.TabIndex = 0;
            this.lb_L1_Output.Text = "Output (mA)";
            // 
            // btn_Connect
            // 
            this.btn_Connect.BackColor = System.Drawing.Color.LightGray;
            this.btn_Connect.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_Connect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Connect.Location = new System.Drawing.Point(9, 64);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(98, 28);
            this.btn_Connect.TabIndex = 2;
            this.btn_Connect.TabStop = false;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = false;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // lb_PortNo
            // 
            this.lb_PortNo.AutoSize = true;
            this.lb_PortNo.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_PortNo.Location = new System.Drawing.Point(8, 8);
            this.lb_PortNo.Name = "lb_PortNo";
            this.lb_PortNo.Size = new System.Drawing.Size(65, 18);
            this.lb_PortNo.TabIndex = 0;
            this.lb_PortNo.Text = "Port No.";
            // 
            // cbb_L2_MaxC
            // 
            this.cbb_L2_MaxC.BackColor = System.Drawing.Color.Black;
            this.cbb_L2_MaxC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_L2_MaxC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbb_L2_MaxC.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cbb_L2_MaxC.ForeColor = System.Drawing.Color.Yellow;
            this.cbb_L2_MaxC.FormattingEnabled = true;
            this.cbb_L2_MaxC.Items.AddRange(new object[] {
            " 500mA",
            " 600mA",
            " 700mA",
            " 800mA"});
            this.cbb_L2_MaxC.Location = new System.Drawing.Point(10, 28);
            this.cbb_L2_MaxC.MaxDropDownItems = 4;
            this.cbb_L2_MaxC.Name = "cbb_L2_MaxC";
            this.cbb_L2_MaxC.Size = new System.Drawing.Size(96, 32);
            this.cbb_L2_MaxC.TabIndex = 1;
            this.cbb_L2_MaxC.TabStop = false;
            this.cbb_L2_MaxC.SelectedIndexChanged += new System.EventHandler(this.cbb_L2_MaxC_SelectedIndexChanged);
            // 
            // gpb_L1
            // 
            this.gpb_L1.BackColor = System.Drawing.Color.Silver;
            this.gpb_L1.Controls.Add(this.pn_L1_Output);
            this.gpb_L1.Controls.Add(this.pn_L1_Set);
            this.gpb_L1.Enabled = false;
            this.gpb_L1.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.gpb_L1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gpb_L1.Location = new System.Drawing.Point(12, 26);
            this.gpb_L1.Name = "gpb_L1";
            this.gpb_L1.Size = new System.Drawing.Size(219, 286);
            this.gpb_L1.TabIndex = 5;
            this.gpb_L1.TabStop = false;
            // 
            // pn_L1_Output
            // 
            this.pn_L1_Output.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L1_Output.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L1_Output.Controls.Add(this.lb_L1_MIN);
            this.pn_L1_Output.Controls.Add(this.lb_L1_MAX);
            this.pn_L1_Output.Controls.Add(this.lb_L1_PCV);
            this.pn_L1_Output.Controls.Add(this.lb_L1_PC);
            this.pn_L1_Output.Controls.Add(this.tkb_L1_Output);
            this.pn_L1_Output.Controls.Add(this.nud_L1_Output);
            this.pn_L1_Output.Controls.Add(this.lb_L1_Output);
            this.pn_L1_Output.Location = new System.Drawing.Point(12, 134);
            this.pn_L1_Output.Name = "pn_L1_Output";
            this.pn_L1_Output.Size = new System.Drawing.Size(194, 140);
            this.pn_L1_Output.TabIndex = 1;
            // 
            // pn_L1_Set
            // 
            this.pn_L1_Set.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L1_Set.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L1_Set.Controls.Add(this.pn_L1_LED);
            this.pn_L1_Set.Controls.Add(this.ckb_L1_EXT);
            this.pn_L1_Set.Controls.Add(this.btn_L1);
            this.pn_L1_Set.Controls.Add(this.lb_L1_MaxC);
            this.pn_L1_Set.Controls.Add(this.cbb_L1_MaxC);
            this.pn_L1_Set.Location = new System.Drawing.Point(12, 22);
            this.pn_L1_Set.Name = "pn_L1_Set";
            this.pn_L1_Set.Size = new System.Drawing.Size(194, 102);
            this.pn_L1_Set.TabIndex = 0;
            // 
            // pn_L1_LED
            // 
            this.pn_L1_LED.BackColor = System.Drawing.Color.Black;
            this.pn_L1_LED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L1_LED.ForeColor = System.Drawing.Color.White;
            this.pn_L1_LED.Location = new System.Drawing.Point(124, 12);
            this.pn_L1_LED.Name = "pn_L1_LED";
            this.pn_L1_LED.Size = new System.Drawing.Size(54, 12);
            this.pn_L1_LED.TabIndex = 3;
            // 
            // ckb_L1_EXT
            // 
            this.ckb_L1_EXT.AutoSize = true;
            this.ckb_L1_EXT.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ckb_L1_EXT.ForeColor = System.Drawing.Color.Maroon;
            this.ckb_L1_EXT.Location = new System.Drawing.Point(10, 68);
            this.ckb_L1_EXT.Name = "ckb_L1_EXT";
            this.ckb_L1_EXT.Size = new System.Drawing.Size(177, 22);
            this.ckb_L1_EXT.TabIndex = 2;
            this.ckb_L1_EXT.TabStop = false;
            this.ckb_L1_EXT.Text = "EXT. ON/OFF Control";
            this.ckb_L1_EXT.UseVisualStyleBackColor = true;
            this.ckb_L1_EXT.CheckedChanged += new System.EventHandler(this.ckb_L1_EXT_CheckedChanged);
            // 
            // btn_L1
            // 
            this.btn_L1.BackColor = System.Drawing.Color.LightGray;
            this.btn_L1.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_L1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_L1.Location = new System.Drawing.Point(124, 27);
            this.btn_L1.Name = "btn_L1";
            this.btn_L1.Size = new System.Drawing.Size(56, 28);
            this.btn_L1.TabIndex = 4;
            this.btn_L1.TabStop = false;
            this.btn_L1.Text = "ON";
            this.btn_L1.UseVisualStyleBackColor = false;
            this.btn_L1.Click += new System.EventHandler(this.btn_L1_Click);
            // 
            // lb_L1_MaxC
            // 
            this.lb_L1_MaxC.AutoSize = true;
            this.lb_L1_MaxC.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L1_MaxC.ForeColor = System.Drawing.Color.Maroon;
            this.lb_L1_MaxC.Location = new System.Drawing.Point(8, 8);
            this.lb_L1_MaxC.Name = "lb_L1_MaxC";
            this.lb_L1_MaxC.Size = new System.Drawing.Size(96, 18);
            this.lb_L1_MaxC.TabIndex = 0;
            this.lb_L1_MaxC.Text = "Max. Current";
            // 
            // cbb_L1_MaxC
            // 
            this.cbb_L1_MaxC.BackColor = System.Drawing.Color.Black;
            this.cbb_L1_MaxC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_L1_MaxC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbb_L1_MaxC.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cbb_L1_MaxC.ForeColor = System.Drawing.Color.Yellow;
            this.cbb_L1_MaxC.FormattingEnabled = true;
            this.cbb_L1_MaxC.Items.AddRange(new object[] {
            " 500mA",
            " 600mA",
            " 700mA",
            " 800mA"});
            this.cbb_L1_MaxC.Location = new System.Drawing.Point(10, 28);
            this.cbb_L1_MaxC.MaxDropDownItems = 4;
            this.cbb_L1_MaxC.Name = "cbb_L1_MaxC";
            this.cbb_L1_MaxC.Size = new System.Drawing.Size(96, 32);
            this.cbb_L1_MaxC.TabIndex = 1;
            this.cbb_L1_MaxC.TabStop = false;
            this.cbb_L1_MaxC.SelectedIndexChanged += new System.EventHandler(this.cbb_L1_MaxC_SelectedIndexChanged);
            // 
            // richTextBox
            // 
            this.richTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.richTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox.Location = new System.Drawing.Point(229, 324);
            this.richTextBox.MaxLength = 32767;
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox.Size = new System.Drawing.Size(521, 160);
            this.richTextBox.TabIndex = 3;
            this.richTextBox.TabStop = false;
            this.richTextBox.Text = "";
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 115200;
            this.serialPort.ReadBufferSize = 128;
            this.serialPort.WriteBufferSize = 64;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // pn_Port
            // 
            this.pn_Port.BackColor = System.Drawing.Color.DarkGray;
            this.pn_Port.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_Port.Controls.Add(this.btn_Connect);
            this.pn_Port.Controls.Add(this.cbb_PortNo);
            this.pn_Port.Controls.Add(this.lb_PortNo);
            this.pn_Port.Location = new System.Drawing.Point(762, 324);
            this.pn_Port.Name = "pn_Port";
            this.pn_Port.Size = new System.Drawing.Size(120, 106);
            this.pn_Port.TabIndex = 2;
            // 
            // btn_L2
            // 
            this.btn_L2.BackColor = System.Drawing.Color.LightGray;
            this.btn_L2.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_L2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_L2.Location = new System.Drawing.Point(124, 27);
            this.btn_L2.Name = "btn_L2";
            this.btn_L2.Size = new System.Drawing.Size(56, 28);
            this.btn_L2.TabIndex = 4;
            this.btn_L2.TabStop = false;
            this.btn_L2.Text = "ON";
            this.btn_L2.UseVisualStyleBackColor = false;
            this.btn_L2.Click += new System.EventHandler(this.btn_L2_Click);
            // 
            // lb_L1
            // 
            this.lb_L1.AutoSize = true;
            this.lb_L1.BackColor = System.Drawing.Color.Black;
            this.lb_L1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L1.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L1.ForeColor = System.Drawing.Color.White;
            this.lb_L1.Location = new System.Drawing.Point(24, 12);
            this.lb_L1.MaximumSize = new System.Drawing.Size(194, 25);
            this.lb_L1.MinimumSize = new System.Drawing.Size(194, 25);
            this.lb_L1.Name = "lb_L1";
            this.lb_L1.Size = new System.Drawing.Size(194, 25);
            this.lb_L1.TabIndex = 4;
            this.lb_L1.Text = "L1";
            this.lb_L1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_L1.UseMnemonic = false;
            // 
            // gpb_ON_OFF
            // 
            this.gpb_ON_OFF.Controls.Add(this.cbb_SYNC_EN);
            this.gpb_ON_OFF.Enabled = false;
            this.gpb_ON_OFF.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.gpb_ON_OFF.Location = new System.Drawing.Point(24, 365);
            this.gpb_ON_OFF.Name = "gpb_ON_OFF";
            this.gpb_ON_OFF.Size = new System.Drawing.Size(122, 72);
            this.gpb_ON_OFF.TabIndex = 13;
            this.gpb_ON_OFF.TabStop = false;
            this.gpb_ON_OFF.Text = "ON/OFF";
            // 
            // cbb_SYNC_EN
            // 
            this.cbb_SYNC_EN.BackColor = System.Drawing.Color.Black;
            this.cbb_SYNC_EN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_SYNC_EN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbb_SYNC_EN.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cbb_SYNC_EN.ForeColor = System.Drawing.Color.Magenta;
            this.cbb_SYNC_EN.FormattingEnabled = true;
            this.cbb_SYNC_EN.Items.AddRange(new object[] {
            "ASYNC",
            "SYNC2",
            "SYNC3",
            "SYNC4"});
            this.cbb_SYNC_EN.Location = new System.Drawing.Point(12, 26);
            this.cbb_SYNC_EN.MaxDropDownItems = 4;
            this.cbb_SYNC_EN.Name = "cbb_SYNC_EN";
            this.cbb_SYNC_EN.Size = new System.Drawing.Size(96, 32);
            this.cbb_SYNC_EN.TabIndex = 0;
            this.cbb_SYNC_EN.TabStop = false;
            this.cbb_SYNC_EN.SelectedIndexChanged += new System.EventHandler(this.cbb_SYNC_EN_SelectedIndexChanged);
            // 
            // lb_PWR_ERR
            // 
            this.lb_PWR_ERR.AutoSize = true;
            this.lb_PWR_ERR.BackColor = System.Drawing.Color.Black;
            this.lb_PWR_ERR.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_PWR_ERR.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_PWR_ERR.ForeColor = System.Drawing.Color.Gray;
            this.lb_PWR_ERR.Location = new System.Drawing.Point(24, 324);
            this.lb_PWR_ERR.MaximumSize = new System.Drawing.Size(122, 25);
            this.lb_PWR_ERR.MinimumSize = new System.Drawing.Size(122, 25);
            this.lb_PWR_ERR.Name = "lb_PWR_ERR";
            this.lb_PWR_ERR.Size = new System.Drawing.Size(122, 25);
            this.lb_PWR_ERR.TabIndex = 12;
            this.lb_PWR_ERR.Text = "Power Error";
            this.lb_PWR_ERR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_PWR_ERR.UseMnemonic = false;
            // 
            // gpb_L2
            // 
            this.gpb_L2.BackColor = System.Drawing.Color.Silver;
            this.gpb_L2.Controls.Add(this.pn_L2_Output);
            this.gpb_L2.Controls.Add(this.pn_L2_Set);
            this.gpb_L2.Enabled = false;
            this.gpb_L2.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.gpb_L2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gpb_L2.Location = new System.Drawing.Point(229, 26);
            this.gpb_L2.Name = "gpb_L2";
            this.gpb_L2.Size = new System.Drawing.Size(219, 286);
            this.gpb_L2.TabIndex = 7;
            this.gpb_L2.TabStop = false;
            // 
            // pn_L2_Output
            // 
            this.pn_L2_Output.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L2_Output.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L2_Output.Controls.Add(this.lb_L2_MIN);
            this.pn_L2_Output.Controls.Add(this.lb_L2_MAX);
            this.pn_L2_Output.Controls.Add(this.lb_L2_PCV);
            this.pn_L2_Output.Controls.Add(this.lb_L2_PC);
            this.pn_L2_Output.Controls.Add(this.tkb_L2_Output);
            this.pn_L2_Output.Controls.Add(this.nud_L2_Output);
            this.pn_L2_Output.Controls.Add(this.lb_L2_Output);
            this.pn_L2_Output.Location = new System.Drawing.Point(12, 134);
            this.pn_L2_Output.Name = "pn_L2_Output";
            this.pn_L2_Output.Size = new System.Drawing.Size(194, 140);
            this.pn_L2_Output.TabIndex = 1;
            // 
            // lb_L2_MIN
            // 
            this.lb_L2_MIN.AutoSize = true;
            this.lb_L2_MIN.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L2_MIN.Location = new System.Drawing.Point(152, 112);
            this.lb_L2_MIN.Name = "lb_L2_MIN";
            this.lb_L2_MIN.Size = new System.Drawing.Size(28, 15);
            this.lb_L2_MIN.TabIndex = 6;
            this.lb_L2_MIN.Text = "MIN";
            // 
            // lb_L2_MAX
            // 
            this.lb_L2_MAX.AutoSize = true;
            this.lb_L2_MAX.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L2_MAX.Location = new System.Drawing.Point(152, 8);
            this.lb_L2_MAX.Name = "lb_L2_MAX";
            this.lb_L2_MAX.Size = new System.Drawing.Size(30, 15);
            this.lb_L2_MAX.TabIndex = 5;
            this.lb_L2_MAX.Text = "MAX";
            // 
            // lb_L2_PCV
            // 
            this.lb_L2_PCV.AutoSize = true;
            this.lb_L2_PCV.BackColor = System.Drawing.Color.Black;
            this.lb_L2_PCV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L2_PCV.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L2_PCV.ForeColor = System.Drawing.Color.Lime;
            this.lb_L2_PCV.Location = new System.Drawing.Point(25, 92);
            this.lb_L2_PCV.MaximumSize = new System.Drawing.Size(64, 32);
            this.lb_L2_PCV.MinimumSize = new System.Drawing.Size(64, 32);
            this.lb_L2_PCV.Name = "lb_L2_PCV";
            this.lb_L2_PCV.Size = new System.Drawing.Size(64, 32);
            this.lb_L2_PCV.TabIndex = 2;
            this.lb_L2_PCV.Text = "0.0";
            this.lb_L2_PCV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_L2_PCV.UseCompatibleTextRendering = true;
            this.lb_L2_PCV.UseMnemonic = false;
            // 
            // lb_L2_PC
            // 
            this.lb_L2_PC.AutoSize = true;
            this.lb_L2_PC.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L2_PC.Location = new System.Drawing.Point(92, 112);
            this.lb_L2_PC.Name = "lb_L2_PC";
            this.lb_L2_PC.Size = new System.Drawing.Size(18, 15);
            this.lb_L2_PC.TabIndex = 3;
            this.lb_L2_PC.Text = "%";
            // 
            // tkb_L2_Output
            // 
            this.tkb_L2_Output.BackColor = System.Drawing.Color.DarkGray;
            this.tkb_L2_Output.LargeChange = 10;
            this.tkb_L2_Output.Location = new System.Drawing.Point(124, 2);
            this.tkb_L2_Output.Margin = new System.Windows.Forms.Padding(0);
            this.tkb_L2_Output.Maximum = 500;
            this.tkb_L2_Output.Name = "tkb_L2_Output";
            this.tkb_L2_Output.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tkb_L2_Output.Size = new System.Drawing.Size(45, 130);
            this.tkb_L2_Output.TabIndex = 4;
            this.tkb_L2_Output.TabStop = false;
            this.tkb_L2_Output.TickFrequency = 50;
            this.tkb_L2_Output.ValueChanged += new System.EventHandler(this.tkb_L2_Output_ValueChanged);
            // 
            // nud_L2_Output
            // 
            this.nud_L2_Output.BackColor = System.Drawing.Color.Black;
            this.nud_L2_Output.Font = new System.Drawing.Font("Arial", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.nud_L2_Output.ForeColor = System.Drawing.Color.Cyan;
            this.nud_L2_Output.Location = new System.Drawing.Point(10, 28);
            this.nud_L2_Output.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_L2_Output.Name = "nud_L2_Output";
            this.nud_L2_Output.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nud_L2_Output.Size = new System.Drawing.Size(96, 53);
            this.nud_L2_Output.TabIndex = 1;
            this.nud_L2_Output.TabStop = false;
            this.nud_L2_Output.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_L2_Output.ValueChanged += new System.EventHandler(this.nud_L2_Output_ValueChanged);
            // 
            // lb_L2_Output
            // 
            this.lb_L2_Output.AutoSize = true;
            this.lb_L2_Output.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L2_Output.ForeColor = System.Drawing.Color.Blue;
            this.lb_L2_Output.Location = new System.Drawing.Point(8, 8);
            this.lb_L2_Output.Name = "lb_L2_Output";
            this.lb_L2_Output.Size = new System.Drawing.Size(91, 18);
            this.lb_L2_Output.TabIndex = 0;
            this.lb_L2_Output.Text = "Output (mA)";
            // 
            // pn_L2_Set
            // 
            this.pn_L2_Set.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L2_Set.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L2_Set.Controls.Add(this.pn_L2_LED);
            this.pn_L2_Set.Controls.Add(this.ckb_L2_EXT);
            this.pn_L2_Set.Controls.Add(this.btn_L2);
            this.pn_L2_Set.Controls.Add(this.lb_L2_MaxC);
            this.pn_L2_Set.Controls.Add(this.cbb_L2_MaxC);
            this.pn_L2_Set.Location = new System.Drawing.Point(12, 22);
            this.pn_L2_Set.Name = "pn_L2_Set";
            this.pn_L2_Set.Size = new System.Drawing.Size(194, 102);
            this.pn_L2_Set.TabIndex = 0;
            // 
            // pn_L2_LED
            // 
            this.pn_L2_LED.BackColor = System.Drawing.Color.Black;
            this.pn_L2_LED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L2_LED.ForeColor = System.Drawing.Color.White;
            this.pn_L2_LED.Location = new System.Drawing.Point(124, 12);
            this.pn_L2_LED.Name = "pn_L2_LED";
            this.pn_L2_LED.Size = new System.Drawing.Size(54, 12);
            this.pn_L2_LED.TabIndex = 3;
            // 
            // ckb_L2_EXT
            // 
            this.ckb_L2_EXT.AutoSize = true;
            this.ckb_L2_EXT.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ckb_L2_EXT.ForeColor = System.Drawing.Color.Maroon;
            this.ckb_L2_EXT.Location = new System.Drawing.Point(10, 68);
            this.ckb_L2_EXT.Name = "ckb_L2_EXT";
            this.ckb_L2_EXT.Size = new System.Drawing.Size(177, 22);
            this.ckb_L2_EXT.TabIndex = 2;
            this.ckb_L2_EXT.TabStop = false;
            this.ckb_L2_EXT.Text = "EXT. ON/OFF Control";
            this.ckb_L2_EXT.UseVisualStyleBackColor = true;
            this.ckb_L2_EXT.CheckedChanged += new System.EventHandler(this.ckb_L2_EXT_CheckedChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.DarkGray;
            this.statusStrip.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tss_Status,
            this.tss_BaudRate,
            this.tss_REV});
            this.statusStrip.Location = new System.Drawing.Point(0, 496);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(894, 26);
            this.statusStrip.TabIndex = 1;
            // 
            // lb_L3
            // 
            this.lb_L3.AutoSize = true;
            this.lb_L3.BackColor = System.Drawing.Color.Black;
            this.lb_L3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L3.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L3.ForeColor = System.Drawing.Color.White;
            this.lb_L3.Location = new System.Drawing.Point(458, 12);
            this.lb_L3.MaximumSize = new System.Drawing.Size(194, 25);
            this.lb_L3.MinimumSize = new System.Drawing.Size(194, 25);
            this.lb_L3.Name = "lb_L3";
            this.lb_L3.Size = new System.Drawing.Size(194, 25);
            this.lb_L3.TabIndex = 8;
            this.lb_L3.Text = "L3";
            this.lb_L3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_L3.UseMnemonic = false;
            // 
            // gpb_L3
            // 
            this.gpb_L3.BackColor = System.Drawing.Color.Silver;
            this.gpb_L3.Controls.Add(this.pn_L3_Output);
            this.gpb_L3.Controls.Add(this.pn_L3_Set);
            this.gpb_L3.Enabled = false;
            this.gpb_L3.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.gpb_L3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gpb_L3.Location = new System.Drawing.Point(446, 26);
            this.gpb_L3.Name = "gpb_L3";
            this.gpb_L3.Size = new System.Drawing.Size(219, 286);
            this.gpb_L3.TabIndex = 9;
            this.gpb_L3.TabStop = false;
            // 
            // pn_L3_Output
            // 
            this.pn_L3_Output.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L3_Output.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L3_Output.Controls.Add(this.lb_L3_MIN);
            this.pn_L3_Output.Controls.Add(this.lb_L3_MAX);
            this.pn_L3_Output.Controls.Add(this.lb_L3_PCV);
            this.pn_L3_Output.Controls.Add(this.lb_L3_PC);
            this.pn_L3_Output.Controls.Add(this.tkb_L3_Output);
            this.pn_L3_Output.Controls.Add(this.nud_L3_Output);
            this.pn_L3_Output.Controls.Add(this.lb_L3_Output);
            this.pn_L3_Output.Location = new System.Drawing.Point(12, 134);
            this.pn_L3_Output.Name = "pn_L3_Output";
            this.pn_L3_Output.Size = new System.Drawing.Size(194, 140);
            this.pn_L3_Output.TabIndex = 1;
            // 
            // lb_L3_MIN
            // 
            this.lb_L3_MIN.AutoSize = true;
            this.lb_L3_MIN.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L3_MIN.Location = new System.Drawing.Point(152, 112);
            this.lb_L3_MIN.Name = "lb_L3_MIN";
            this.lb_L3_MIN.Size = new System.Drawing.Size(28, 15);
            this.lb_L3_MIN.TabIndex = 6;
            this.lb_L3_MIN.Text = "MIN";
            // 
            // lb_L3_MAX
            // 
            this.lb_L3_MAX.AutoSize = true;
            this.lb_L3_MAX.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L3_MAX.Location = new System.Drawing.Point(152, 8);
            this.lb_L3_MAX.Name = "lb_L3_MAX";
            this.lb_L3_MAX.Size = new System.Drawing.Size(30, 15);
            this.lb_L3_MAX.TabIndex = 5;
            this.lb_L3_MAX.Text = "MAX";
            // 
            // lb_L3_PCV
            // 
            this.lb_L3_PCV.AutoSize = true;
            this.lb_L3_PCV.BackColor = System.Drawing.Color.Black;
            this.lb_L3_PCV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L3_PCV.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L3_PCV.ForeColor = System.Drawing.Color.Lime;
            this.lb_L3_PCV.Location = new System.Drawing.Point(25, 92);
            this.lb_L3_PCV.MaximumSize = new System.Drawing.Size(64, 32);
            this.lb_L3_PCV.MinimumSize = new System.Drawing.Size(64, 32);
            this.lb_L3_PCV.Name = "lb_L3_PCV";
            this.lb_L3_PCV.Size = new System.Drawing.Size(64, 32);
            this.lb_L3_PCV.TabIndex = 2;
            this.lb_L3_PCV.Text = "0.0";
            this.lb_L3_PCV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_L3_PCV.UseCompatibleTextRendering = true;
            this.lb_L3_PCV.UseMnemonic = false;
            // 
            // lb_L3_PC
            // 
            this.lb_L3_PC.AutoSize = true;
            this.lb_L3_PC.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L3_PC.Location = new System.Drawing.Point(92, 112);
            this.lb_L3_PC.Name = "lb_L3_PC";
            this.lb_L3_PC.Size = new System.Drawing.Size(18, 15);
            this.lb_L3_PC.TabIndex = 3;
            this.lb_L3_PC.Text = "%";
            // 
            // tkb_L3_Output
            // 
            this.tkb_L3_Output.BackColor = System.Drawing.Color.DarkGray;
            this.tkb_L3_Output.LargeChange = 10;
            this.tkb_L3_Output.Location = new System.Drawing.Point(124, 2);
            this.tkb_L3_Output.Margin = new System.Windows.Forms.Padding(0);
            this.tkb_L3_Output.Maximum = 500;
            this.tkb_L3_Output.Name = "tkb_L3_Output";
            this.tkb_L3_Output.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tkb_L3_Output.Size = new System.Drawing.Size(45, 130);
            this.tkb_L3_Output.TabIndex = 4;
            this.tkb_L3_Output.TabStop = false;
            this.tkb_L3_Output.TickFrequency = 50;
            this.tkb_L3_Output.ValueChanged += new System.EventHandler(this.tkb_L3_Output_ValueChanged);
            // 
            // nud_L3_Output
            // 
            this.nud_L3_Output.BackColor = System.Drawing.Color.Black;
            this.nud_L3_Output.Font = new System.Drawing.Font("Arial", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.nud_L3_Output.ForeColor = System.Drawing.Color.Cyan;
            this.nud_L3_Output.Location = new System.Drawing.Point(10, 28);
            this.nud_L3_Output.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_L3_Output.Name = "nud_L3_Output";
            this.nud_L3_Output.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nud_L3_Output.Size = new System.Drawing.Size(96, 53);
            this.nud_L3_Output.TabIndex = 1;
            this.nud_L3_Output.TabStop = false;
            this.nud_L3_Output.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_L3_Output.ValueChanged += new System.EventHandler(this.nud_L3_Output_ValueChanged);
            // 
            // lb_L3_Output
            // 
            this.lb_L3_Output.AutoSize = true;
            this.lb_L3_Output.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L3_Output.ForeColor = System.Drawing.Color.Blue;
            this.lb_L3_Output.Location = new System.Drawing.Point(8, 8);
            this.lb_L3_Output.Name = "lb_L3_Output";
            this.lb_L3_Output.Size = new System.Drawing.Size(91, 18);
            this.lb_L3_Output.TabIndex = 0;
            this.lb_L3_Output.Text = "Output (mA)";
            // 
            // pn_L3_Set
            // 
            this.pn_L3_Set.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L3_Set.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L3_Set.Controls.Add(this.pn_L3_LED);
            this.pn_L3_Set.Controls.Add(this.ckb_L3_EXT);
            this.pn_L3_Set.Controls.Add(this.btn_L3);
            this.pn_L3_Set.Controls.Add(this.lb_L3_MaxC);
            this.pn_L3_Set.Controls.Add(this.cbb_L3_MaxC);
            this.pn_L3_Set.Location = new System.Drawing.Point(12, 22);
            this.pn_L3_Set.Name = "pn_L3_Set";
            this.pn_L3_Set.Size = new System.Drawing.Size(194, 102);
            this.pn_L3_Set.TabIndex = 0;
            // 
            // pn_L3_LED
            // 
            this.pn_L3_LED.BackColor = System.Drawing.Color.Black;
            this.pn_L3_LED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L3_LED.ForeColor = System.Drawing.Color.White;
            this.pn_L3_LED.Location = new System.Drawing.Point(124, 12);
            this.pn_L3_LED.Name = "pn_L3_LED";
            this.pn_L3_LED.Size = new System.Drawing.Size(54, 12);
            this.pn_L3_LED.TabIndex = 3;
            // 
            // ckb_L3_EXT
            // 
            this.ckb_L3_EXT.AutoSize = true;
            this.ckb_L3_EXT.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ckb_L3_EXT.ForeColor = System.Drawing.Color.Maroon;
            this.ckb_L3_EXT.Location = new System.Drawing.Point(10, 68);
            this.ckb_L3_EXT.Name = "ckb_L3_EXT";
            this.ckb_L3_EXT.Size = new System.Drawing.Size(177, 22);
            this.ckb_L3_EXT.TabIndex = 2;
            this.ckb_L3_EXT.TabStop = false;
            this.ckb_L3_EXT.Text = "EXT. ON/OFF Control";
            this.ckb_L3_EXT.UseVisualStyleBackColor = true;
            this.ckb_L3_EXT.CheckedChanged += new System.EventHandler(this.ckb_L3_EXT_CheckedChanged);
            // 
            // btn_L3
            // 
            this.btn_L3.BackColor = System.Drawing.Color.LightGray;
            this.btn_L3.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_L3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_L3.Location = new System.Drawing.Point(124, 27);
            this.btn_L3.Name = "btn_L3";
            this.btn_L3.Size = new System.Drawing.Size(56, 28);
            this.btn_L3.TabIndex = 4;
            this.btn_L3.TabStop = false;
            this.btn_L3.Text = "ON";
            this.btn_L3.UseVisualStyleBackColor = false;
            this.btn_L3.Click += new System.EventHandler(this.btn_L3_Click);
            // 
            // lb_L3_MaxC
            // 
            this.lb_L3_MaxC.AutoSize = true;
            this.lb_L3_MaxC.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L3_MaxC.ForeColor = System.Drawing.Color.Maroon;
            this.lb_L3_MaxC.Location = new System.Drawing.Point(8, 8);
            this.lb_L3_MaxC.Name = "lb_L3_MaxC";
            this.lb_L3_MaxC.Size = new System.Drawing.Size(96, 18);
            this.lb_L3_MaxC.TabIndex = 0;
            this.lb_L3_MaxC.Text = "Max. Current";
            // 
            // cbb_L3_MaxC
            // 
            this.cbb_L3_MaxC.BackColor = System.Drawing.Color.Black;
            this.cbb_L3_MaxC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_L3_MaxC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbb_L3_MaxC.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cbb_L3_MaxC.ForeColor = System.Drawing.Color.Yellow;
            this.cbb_L3_MaxC.FormattingEnabled = true;
            this.cbb_L3_MaxC.Items.AddRange(new object[] {
            " 500mA",
            " 600mA",
            " 700mA",
            " 800mA"});
            this.cbb_L3_MaxC.Location = new System.Drawing.Point(10, 28);
            this.cbb_L3_MaxC.MaxDropDownItems = 4;
            this.cbb_L3_MaxC.Name = "cbb_L3_MaxC";
            this.cbb_L3_MaxC.Size = new System.Drawing.Size(96, 32);
            this.cbb_L3_MaxC.TabIndex = 1;
            this.cbb_L3_MaxC.TabStop = false;
            this.cbb_L3_MaxC.SelectedIndexChanged += new System.EventHandler(this.cbb_L3_MaxC_SelectedIndexChanged);
            // 
            // lb_L4
            // 
            this.lb_L4.AutoSize = true;
            this.lb_L4.BackColor = System.Drawing.Color.Black;
            this.lb_L4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L4.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L4.ForeColor = System.Drawing.Color.White;
            this.lb_L4.Location = new System.Drawing.Point(675, 12);
            this.lb_L4.MaximumSize = new System.Drawing.Size(194, 25);
            this.lb_L4.MinimumSize = new System.Drawing.Size(194, 25);
            this.lb_L4.Name = "lb_L4";
            this.lb_L4.Size = new System.Drawing.Size(194, 25);
            this.lb_L4.TabIndex = 10;
            this.lb_L4.Text = "L4";
            this.lb_L4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_L4.UseMnemonic = false;
            // 
            // gpb_L4
            // 
            this.gpb_L4.BackColor = System.Drawing.Color.Silver;
            this.gpb_L4.Controls.Add(this.pn_L4_Output);
            this.gpb_L4.Controls.Add(this.pn_L4_Set);
            this.gpb_L4.Enabled = false;
            this.gpb_L4.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.gpb_L4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gpb_L4.Location = new System.Drawing.Point(663, 26);
            this.gpb_L4.Name = "gpb_L4";
            this.gpb_L4.Size = new System.Drawing.Size(219, 286);
            this.gpb_L4.TabIndex = 11;
            this.gpb_L4.TabStop = false;
            // 
            // pn_L4_Output
            // 
            this.pn_L4_Output.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L4_Output.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L4_Output.Controls.Add(this.lb_L4_MIN);
            this.pn_L4_Output.Controls.Add(this.lb_L4_MAX);
            this.pn_L4_Output.Controls.Add(this.lb_L4_PCV);
            this.pn_L4_Output.Controls.Add(this.lb_L4_PC);
            this.pn_L4_Output.Controls.Add(this.tkb_L4_Output);
            this.pn_L4_Output.Controls.Add(this.nud_L4_Output);
            this.pn_L4_Output.Controls.Add(this.lb_L4_Output);
            this.pn_L4_Output.Location = new System.Drawing.Point(12, 134);
            this.pn_L4_Output.Name = "pn_L4_Output";
            this.pn_L4_Output.Size = new System.Drawing.Size(194, 140);
            this.pn_L4_Output.TabIndex = 1;
            // 
            // lb_L4_MIN
            // 
            this.lb_L4_MIN.AutoSize = true;
            this.lb_L4_MIN.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L4_MIN.Location = new System.Drawing.Point(152, 112);
            this.lb_L4_MIN.Name = "lb_L4_MIN";
            this.lb_L4_MIN.Size = new System.Drawing.Size(28, 15);
            this.lb_L4_MIN.TabIndex = 6;
            this.lb_L4_MIN.Text = "MIN";
            // 
            // lb_L4_MAX
            // 
            this.lb_L4_MAX.AutoSize = true;
            this.lb_L4_MAX.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L4_MAX.Location = new System.Drawing.Point(152, 8);
            this.lb_L4_MAX.Name = "lb_L4_MAX";
            this.lb_L4_MAX.Size = new System.Drawing.Size(30, 15);
            this.lb_L4_MAX.TabIndex = 5;
            this.lb_L4_MAX.Text = "MAX";
            // 
            // lb_L4_PCV
            // 
            this.lb_L4_PCV.AutoSize = true;
            this.lb_L4_PCV.BackColor = System.Drawing.Color.Black;
            this.lb_L4_PCV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_L4_PCV.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L4_PCV.ForeColor = System.Drawing.Color.Lime;
            this.lb_L4_PCV.Location = new System.Drawing.Point(25, 92);
            this.lb_L4_PCV.MaximumSize = new System.Drawing.Size(64, 32);
            this.lb_L4_PCV.MinimumSize = new System.Drawing.Size(64, 32);
            this.lb_L4_PCV.Name = "lb_L4_PCV";
            this.lb_L4_PCV.Size = new System.Drawing.Size(64, 32);
            this.lb_L4_PCV.TabIndex = 2;
            this.lb_L4_PCV.Text = "0.0";
            this.lb_L4_PCV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_L4_PCV.UseCompatibleTextRendering = true;
            this.lb_L4_PCV.UseMnemonic = false;
            // 
            // lb_L4_PC
            // 
            this.lb_L4_PC.AutoSize = true;
            this.lb_L4_PC.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L4_PC.Location = new System.Drawing.Point(92, 112);
            this.lb_L4_PC.Name = "lb_L4_PC";
            this.lb_L4_PC.Size = new System.Drawing.Size(18, 15);
            this.lb_L4_PC.TabIndex = 3;
            this.lb_L4_PC.Text = "%";
            // 
            // tkb_L4_Output
            // 
            this.tkb_L4_Output.BackColor = System.Drawing.Color.DarkGray;
            this.tkb_L4_Output.LargeChange = 10;
            this.tkb_L4_Output.Location = new System.Drawing.Point(124, 2);
            this.tkb_L4_Output.Margin = new System.Windows.Forms.Padding(0);
            this.tkb_L4_Output.Maximum = 500;
            this.tkb_L4_Output.Name = "tkb_L4_Output";
            this.tkb_L4_Output.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tkb_L4_Output.Size = new System.Drawing.Size(45, 130);
            this.tkb_L4_Output.TabIndex = 4;
            this.tkb_L4_Output.TabStop = false;
            this.tkb_L4_Output.TickFrequency = 50;
            this.tkb_L4_Output.ValueChanged += new System.EventHandler(this.tkb_L4_Output_ValueChanged);
            // 
            // nud_L4_Output
            // 
            this.nud_L4_Output.BackColor = System.Drawing.Color.Black;
            this.nud_L4_Output.Font = new System.Drawing.Font("Arial", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.nud_L4_Output.ForeColor = System.Drawing.Color.Cyan;
            this.nud_L4_Output.Location = new System.Drawing.Point(10, 28);
            this.nud_L4_Output.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_L4_Output.Name = "nud_L4_Output";
            this.nud_L4_Output.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nud_L4_Output.Size = new System.Drawing.Size(96, 53);
            this.nud_L4_Output.TabIndex = 1;
            this.nud_L4_Output.TabStop = false;
            this.nud_L4_Output.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_L4_Output.ValueChanged += new System.EventHandler(this.nud_L4_Output_ValueChanged);
            // 
            // lb_L4_Output
            // 
            this.lb_L4_Output.AutoSize = true;
            this.lb_L4_Output.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L4_Output.ForeColor = System.Drawing.Color.Blue;
            this.lb_L4_Output.Location = new System.Drawing.Point(8, 8);
            this.lb_L4_Output.Name = "lb_L4_Output";
            this.lb_L4_Output.Size = new System.Drawing.Size(91, 18);
            this.lb_L4_Output.TabIndex = 0;
            this.lb_L4_Output.Text = "Output (mA)";
            // 
            // pn_L4_Set
            // 
            this.pn_L4_Set.BackColor = System.Drawing.Color.DarkGray;
            this.pn_L4_Set.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L4_Set.Controls.Add(this.pn_L4_LED);
            this.pn_L4_Set.Controls.Add(this.ckb_L4_EXT);
            this.pn_L4_Set.Controls.Add(this.btn_L4);
            this.pn_L4_Set.Controls.Add(this.lb_L4_MaxC);
            this.pn_L4_Set.Controls.Add(this.cbb_L4_MaxC);
            this.pn_L4_Set.Location = new System.Drawing.Point(12, 22);
            this.pn_L4_Set.Name = "pn_L4_Set";
            this.pn_L4_Set.Size = new System.Drawing.Size(194, 102);
            this.pn_L4_Set.TabIndex = 0;
            // 
            // pn_L4_LED
            // 
            this.pn_L4_LED.BackColor = System.Drawing.Color.Black;
            this.pn_L4_LED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pn_L4_LED.ForeColor = System.Drawing.Color.White;
            this.pn_L4_LED.Location = new System.Drawing.Point(124, 12);
            this.pn_L4_LED.Name = "pn_L4_LED";
            this.pn_L4_LED.Size = new System.Drawing.Size(54, 12);
            this.pn_L4_LED.TabIndex = 3;
            // 
            // ckb_L4_EXT
            // 
            this.ckb_L4_EXT.AutoSize = true;
            this.ckb_L4_EXT.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ckb_L4_EXT.ForeColor = System.Drawing.Color.Maroon;
            this.ckb_L4_EXT.Location = new System.Drawing.Point(10, 68);
            this.ckb_L4_EXT.Name = "ckb_L4_EXT";
            this.ckb_L4_EXT.Size = new System.Drawing.Size(177, 22);
            this.ckb_L4_EXT.TabIndex = 2;
            this.ckb_L4_EXT.TabStop = false;
            this.ckb_L4_EXT.Text = "EXT. ON/OFF Control";
            this.ckb_L4_EXT.UseVisualStyleBackColor = true;
            this.ckb_L4_EXT.CheckedChanged += new System.EventHandler(this.ckb_L4_EXT_CheckedChanged);
            // 
            // btn_L4
            // 
            this.btn_L4.BackColor = System.Drawing.Color.LightGray;
            this.btn_L4.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btn_L4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_L4.Location = new System.Drawing.Point(124, 27);
            this.btn_L4.Name = "btn_L4";
            this.btn_L4.Size = new System.Drawing.Size(56, 28);
            this.btn_L4.TabIndex = 4;
            this.btn_L4.TabStop = false;
            this.btn_L4.Text = "ON";
            this.btn_L4.UseVisualStyleBackColor = false;
            this.btn_L4.Click += new System.EventHandler(this.btn_L4_Click);
            // 
            // lb_L4_MaxC
            // 
            this.lb_L4_MaxC.AutoSize = true;
            this.lb_L4_MaxC.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lb_L4_MaxC.ForeColor = System.Drawing.Color.Maroon;
            this.lb_L4_MaxC.Location = new System.Drawing.Point(8, 8);
            this.lb_L4_MaxC.Name = "lb_L4_MaxC";
            this.lb_L4_MaxC.Size = new System.Drawing.Size(96, 18);
            this.lb_L4_MaxC.TabIndex = 0;
            this.lb_L4_MaxC.Text = "Max. Current";
            // 
            // cbb_L4_MaxC
            // 
            this.cbb_L4_MaxC.BackColor = System.Drawing.Color.Black;
            this.cbb_L4_MaxC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_L4_MaxC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbb_L4_MaxC.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cbb_L4_MaxC.ForeColor = System.Drawing.Color.Yellow;
            this.cbb_L4_MaxC.FormattingEnabled = true;
            this.cbb_L4_MaxC.Items.AddRange(new object[] {
            " 500mA",
            " 600mA",
            " 700mA",
            " 800mA"});
            this.cbb_L4_MaxC.Location = new System.Drawing.Point(10, 28);
            this.cbb_L4_MaxC.MaxDropDownItems = 4;
            this.cbb_L4_MaxC.Name = "cbb_L4_MaxC";
            this.cbb_L4_MaxC.Size = new System.Drawing.Size(96, 32);
            this.cbb_L4_MaxC.TabIndex = 1;
            this.cbb_L4_MaxC.TabStop = false;
            this.cbb_L4_MaxC.SelectedIndexChanged += new System.EventHandler(this.cbb_L4_MaxC_SelectedIndexChanged);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(762, 450);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(120, 34);
            this.button_OK.TabIndex = 14;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // cbxLightOffSetEnabled
            // 
            this.cbxLightOffSetEnabled.AutoSize = true;
            this.cbxLightOffSetEnabled.Location = new System.Drawing.Point(24, 450);
            this.cbxLightOffSetEnabled.Name = "cbxLightOffSetEnabled";
            this.cbxLightOffSetEnabled.Size = new System.Drawing.Size(102, 22);
            this.cbxLightOffSetEnabled.TabIndex = 15;
            this.cbxLightOffSetEnabled.Text = "LightOffset";
            this.cbxLightOffSetEnabled.UseVisualStyleBackColor = true;
            this.cbxLightOffSetEnabled.CheckedChanged += new System.EventHandler(this.cbxLightOffSetEnabled_CheckedChanged);
            // 
            // Form_24084R
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(894, 522);
            this.Controls.Add(this.cbxLightOffSetEnabled);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.lb_L4);
            this.Controls.Add(this.lb_L3);
            this.Controls.Add(this.gpb_L4);
            this.Controls.Add(this.gpb_L3);
            this.Controls.Add(this.lb_L1);
            this.Controls.Add(this.lb_L2);
            this.Controls.Add(this.gpb_L1);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.pn_Port);
            this.Controls.Add(this.gpb_ON_OFF);
            this.Controls.Add(this.lb_PWR_ERR);
            this.Controls.Add(this.gpb_L2);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "Form_24084R";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PM-24084R (C#)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_24084R_FormClosing);
            this.Load += new System.EventHandler(this.Form_24084R_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L1_Output)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L1_Output)).EndInit();
            this.gpb_L1.ResumeLayout(false);
            this.pn_L1_Output.ResumeLayout(false);
            this.pn_L1_Output.PerformLayout();
            this.pn_L1_Set.ResumeLayout(false);
            this.pn_L1_Set.PerformLayout();
            this.pn_Port.ResumeLayout(false);
            this.pn_Port.PerformLayout();
            this.gpb_ON_OFF.ResumeLayout(false);
            this.gpb_L2.ResumeLayout(false);
            this.pn_L2_Output.ResumeLayout(false);
            this.pn_L2_Output.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L2_Output)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L2_Output)).EndInit();
            this.pn_L2_Set.ResumeLayout(false);
            this.pn_L2_Set.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.gpb_L3.ResumeLayout(false);
            this.pn_L3_Output.ResumeLayout(false);
            this.pn_L3_Output.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L3_Output)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L3_Output)).EndInit();
            this.pn_L3_Set.ResumeLayout(false);
            this.pn_L3_Set.PerformLayout();
            this.gpb_L4.ResumeLayout(false);
            this.pn_L4_Output.ResumeLayout(false);
            this.pn_L4_Output.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkb_L4_Output)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_L4_Output)).EndInit();
            this.pn_L4_Set.ResumeLayout(false);
            this.pn_L4_Set.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_L2_MaxC;
        private System.Windows.Forms.Label lb_L2;
        private System.Windows.Forms.Label lb_L1_MIN;
        private System.Windows.Forms.Label lb_L1_MAX;
        private System.Windows.Forms.Label lb_L1_PCV;
        private System.Windows.Forms.Label lb_L1_PC;
        private System.Windows.Forms.TrackBar tkb_L1_Output;
        private System.Windows.Forms.NumericUpDown nud_L1_Output;
        private System.Windows.Forms.ComboBox cbb_PortNo;
        private System.Windows.Forms.ToolStripStatusLabel tss_Status;
        private System.Windows.Forms.ToolStripStatusLabel tss_BaudRate;
        private System.Windows.Forms.ToolStripStatusLabel tss_REV;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lb_L1_Output;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Label lb_PortNo;
        private System.Windows.Forms.ComboBox cbb_L2_MaxC;
        private System.Windows.Forms.GroupBox gpb_L1;
        private System.Windows.Forms.Panel pn_L1_Output;
        private System.Windows.Forms.Panel pn_L1_Set;
        private System.Windows.Forms.Panel pn_L1_LED;
        private System.Windows.Forms.CheckBox ckb_L1_EXT;
        private System.Windows.Forms.Button btn_L1;
        private System.Windows.Forms.Label lb_L1_MaxC;
        private System.Windows.Forms.ComboBox cbb_L1_MaxC;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.Panel pn_Port;
        private System.Windows.Forms.Button btn_L2;
        private System.Windows.Forms.Label lb_L1;
        private System.Windows.Forms.GroupBox gpb_ON_OFF;
        private System.Windows.Forms.Label lb_PWR_ERR;
        private System.Windows.Forms.GroupBox gpb_L2;
        private System.Windows.Forms.Panel pn_L2_Output;
        private System.Windows.Forms.Label lb_L2_MIN;
        private System.Windows.Forms.Label lb_L2_MAX;
        private System.Windows.Forms.Label lb_L2_PCV;
        private System.Windows.Forms.Label lb_L2_PC;
        private System.Windows.Forms.TrackBar tkb_L2_Output;
        private System.Windows.Forms.NumericUpDown nud_L2_Output;
        private System.Windows.Forms.Label lb_L2_Output;
        private System.Windows.Forms.Panel pn_L2_Set;
        private System.Windows.Forms.Panel pn_L2_LED;
        private System.Windows.Forms.CheckBox ckb_L2_EXT;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Label lb_L3;
        private System.Windows.Forms.GroupBox gpb_L3;
        private System.Windows.Forms.Panel pn_L3_Output;
        private System.Windows.Forms.Label lb_L3_MIN;
        private System.Windows.Forms.Label lb_L3_MAX;
        private System.Windows.Forms.Label lb_L3_PCV;
        private System.Windows.Forms.Label lb_L3_PC;
        private System.Windows.Forms.TrackBar tkb_L3_Output;
        private System.Windows.Forms.NumericUpDown nud_L3_Output;
        private System.Windows.Forms.Label lb_L3_Output;
        private System.Windows.Forms.Panel pn_L3_Set;
        private System.Windows.Forms.Panel pn_L3_LED;
        private System.Windows.Forms.CheckBox ckb_L3_EXT;
        private System.Windows.Forms.Button btn_L3;
        private System.Windows.Forms.Label lb_L3_MaxC;
        private System.Windows.Forms.ComboBox cbb_L3_MaxC;
        private System.Windows.Forms.Label lb_L4;
        private System.Windows.Forms.GroupBox gpb_L4;
        private System.Windows.Forms.Panel pn_L4_Output;
        private System.Windows.Forms.Label lb_L4_MIN;
        private System.Windows.Forms.Label lb_L4_MAX;
        private System.Windows.Forms.Label lb_L4_PCV;
        private System.Windows.Forms.Label lb_L4_PC;
        private System.Windows.Forms.TrackBar tkb_L4_Output;
        private System.Windows.Forms.NumericUpDown nud_L4_Output;
        private System.Windows.Forms.Label lb_L4_Output;
        private System.Windows.Forms.Panel pn_L4_Set;
        private System.Windows.Forms.Panel pn_L4_LED;
        private System.Windows.Forms.CheckBox ckb_L4_EXT;
        private System.Windows.Forms.Button btn_L4;
        private System.Windows.Forms.Label lb_L4_MaxC;
        private System.Windows.Forms.ComboBox cbb_L4_MaxC;
        private System.Windows.Forms.ComboBox cbb_SYNC_EN;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.CheckBox cbxLightOffSetEnabled;
    }
}

