namespace DeltaSubstrateInspector.src.MainSetting
{
    partial class LightParamForm
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
            this.cbo_port = new System.Windows.Forms.ComboBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_exit = new System.Windows.Forms.Button();
            this.lbl_title = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbo_bao = new System.Windows.Forms.ComboBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.hWindowControl_cam = new HalconDotNet.HWindowControl();
            this.lbl_light_val = new System.Windows.Forms.Label();
            this.btn_new = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbo_port
            // 
            this.cbo_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_port.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.cbo_port.FormattingEnabled = true;
            this.cbo_port.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5"});
            this.cbo_port.Location = new System.Drawing.Point(351, 28);
            this.cbo_port.Margin = new System.Windows.Forms.Padding(5);
            this.cbo_port.Name = "cbo_port";
            this.cbo_port.Size = new System.Drawing.Size(155, 27);
            this.cbo_port.TabIndex = 36;
            // 
            // txt_name
            // 
            this.txt_name.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.txt_name.Location = new System.Drawing.Point(108, 28);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(155, 27);
            this.txt_name.TabIndex = 45;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(242)))));
            this.panel1.Controls.Add(this.btn_exit);
            this.panel1.Controls.Add(this.lbl_title);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(758, 64);
            this.panel1.TabIndex = 47;
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.Transparent;
            this.btn_exit.FlatAppearance.BorderSize = 0;
            this.btn_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_exit.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_exit.ForeColor = System.Drawing.Color.White;
            this.btn_exit.Location = new System.Drawing.Point(713, 9);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(44, 40);
            this.btn_exit.TabIndex = 48;
            this.btn_exit.Text = "X";
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(16, 18);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(214, 19);
            this.lbl_title.TabIndex = 1;
            this.lbl_title.Text = "新增光源 Create Light Model";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(32, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 48;
            this.label3.Text = "光源名稱";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(286, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 49;
            this.label4.Text = "通訊埠";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(57, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 19);
            this.label6.TabIndex = 50;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(525, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 51;
            this.label7.Text = "鮑率";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(35, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 20);
            this.label8.TabIndex = 52;
            this.label8.Text = "光源強度";
            // 
            // cbo_bao
            // 
            this.cbo_bao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_bao.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.cbo_bao.FormattingEnabled = true;
            this.cbo_bao.Items.AddRange(new object[] {
            "Item1",
            "Item2",
            "Item3",
            "Item4",
            "Item5"});
            this.cbo_bao.Location = new System.Drawing.Point(574, 28);
            this.cbo_bao.Margin = new System.Windows.Forms.Padding(5);
            this.cbo_bao.Name = "cbo_bao";
            this.cbo_bao.Size = new System.Drawing.Size(155, 27);
            this.cbo_bao.TabIndex = 53;
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.Color.White;
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(139, 154);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(489, 45);
            this.trackBar1.TabIndex = 54;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.hWindowControl_cam);
            this.panel2.Controls.Add(this.lbl_light_val);
            this.panel2.Controls.Add(this.cbo_bao);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.btn_new);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cbo_port);
            this.panel2.Controls.Add(this.txt_name);
            this.panel2.Location = new System.Drawing.Point(3, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(758, 457);
            this.panel2.TabIndex = 56;
            // 
            // hWindowControl_cam
            // 
            this.hWindowControl_cam.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_cam.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_cam.ImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_cam.Location = new System.Drawing.Point(6, 176);
            this.hWindowControl_cam.Name = "hWindowControl_cam";
            this.hWindowControl_cam.Size = new System.Drawing.Size(619, 268);
            this.hWindowControl_cam.TabIndex = 58;
            this.hWindowControl_cam.WindowSize = new System.Drawing.Size(619, 268);
            // 
            // lbl_light_val
            // 
            this.lbl_light_val.AutoSize = true;
            this.lbl_light_val.BackColor = System.Drawing.Color.White;
            this.lbl_light_val.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_light_val.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_light_val.Location = new System.Drawing.Point(69, 122);
            this.lbl_light_val.Name = "lbl_light_val";
            this.lbl_light_val.Size = new System.Drawing.Size(44, 18);
            this.lbl_light_val.TabIndex = 57;
            this.lbl_light_val.Text = "value";
            // 
            // btn_new
            // 
            this.btn_new.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_new.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_new.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_new.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_new.Location = new System.Drawing.Point(631, 402);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(114, 42);
            this.btn_new.TabIndex = 56;
            this.btn_new.Text = "OK";
            this.btn_new.UseVisualStyleBackColor = false;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBox1.Location = new System.Drawing.Point(6, 176);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(619, 268);
            this.pictureBox1.TabIndex = 55;
            this.pictureBox1.TabStop = false;
            // 
            // LightParamForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(767, 524);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LightParamForm";
            this.Text = "LightParamForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LightParamForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbo_port;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbo_bao;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Label lbl_light_val;
        public HalconDotNet.HWindowControl hWindowControl_cam;
    }
}