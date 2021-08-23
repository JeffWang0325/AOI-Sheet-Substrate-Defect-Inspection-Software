namespace IOModule
{
    partial class ConfigModule
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
            this.cbx_LightOffSet = new System.Windows.Forms.CheckBox();
            this.lbl_LightOffSet = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cbx_LightComportName = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbx_CameraInterface = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbx_BypassIO = new System.Windows.Forms.CheckBox();
            this.lbl_BypassIO = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbx_CallBackType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbx_BypassLightControl = new System.Windows.Forms.CheckBox();
            this.lbl_BypassLightControl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbx_LightOffSet
            // 
            this.cbx_LightOffSet.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbx_LightOffSet.AutoSize = true;
            this.cbx_LightOffSet.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
            this.cbx_LightOffSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbx_LightOffSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_LightOffSet.Location = new System.Drawing.Point(233, 87);
            this.cbx_LightOffSet.Name = "cbx_LightOffSet";
            this.cbx_LightOffSet.Size = new System.Drawing.Size(65, 34);
            this.cbx_LightOffSet.TabIndex = 96;
            this.cbx_LightOffSet.Tag = "LightOffSet";
            this.cbx_LightOffSet.Text = "         ";
            this.cbx_LightOffSet.UseVisualStyleBackColor = true;
            this.cbx_LightOffSet.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // lbl_LightOffSet
            // 
            this.lbl_LightOffSet.AutoSize = true;
            this.lbl_LightOffSet.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lbl_LightOffSet.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_LightOffSet.Location = new System.Drawing.Point(304, 97);
            this.lbl_LightOffSet.Name = "lbl_LightOffSet";
            this.lbl_LightOffSet.Size = new System.Drawing.Size(38, 20);
            this.lbl_LightOffSet.TabIndex = 95;
            this.lbl_LightOffSet.Text = "OFF";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label21.Location = new System.Drawing.Point(10, 95);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(94, 20);
            this.label21.TabIndex = 94;
            this.label21.Text = "LightOffSet";
            // 
            // cbx_LightComportName
            // 
            this.cbx_LightComportName.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.cbx_LightComportName.FormattingEnabled = true;
            this.cbx_LightComportName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15"});
            this.cbx_LightComportName.Location = new System.Drawing.Point(195, 57);
            this.cbx_LightComportName.Name = "cbx_LightComportName";
            this.cbx_LightComportName.Size = new System.Drawing.Size(170, 28);
            this.cbx_LightComportName.TabIndex = 93;
            this.cbx_LightComportName.Tag = "LightComportName";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label16.Location = new System.Drawing.Point(10, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(161, 20);
            this.label16.TabIndex = 92;
            this.label16.Text = "LightComportName";
            // 
            // cbx_CameraInterface
            // 
            this.cbx_CameraInterface.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.cbx_CameraInterface.FormattingEnabled = true;
            this.cbx_CameraInterface.Location = new System.Drawing.Point(195, 127);
            this.cbx_CameraInterface.Name = "cbx_CameraInterface";
            this.cbx_CameraInterface.Size = new System.Drawing.Size(170, 28);
            this.cbx_CameraInterface.TabIndex = 98;
            this.cbx_CameraInterface.Tag = "CameraInterface";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label1.Location = new System.Drawing.Point(10, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 97;
            this.label1.Text = "相機介面";
            // 
            // cbx_BypassIO
            // 
            this.cbx_BypassIO.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbx_BypassIO.AutoSize = true;
            this.cbx_BypassIO.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
            this.cbx_BypassIO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbx_BypassIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_BypassIO.Location = new System.Drawing.Point(233, 157);
            this.cbx_BypassIO.Name = "cbx_BypassIO";
            this.cbx_BypassIO.Size = new System.Drawing.Size(65, 34);
            this.cbx_BypassIO.TabIndex = 101;
            this.cbx_BypassIO.Tag = "BypassIO";
            this.cbx_BypassIO.Text = "         ";
            this.cbx_BypassIO.UseVisualStyleBackColor = true;
            this.cbx_BypassIO.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // lbl_BypassIO
            // 
            this.lbl_BypassIO.AutoSize = true;
            this.lbl_BypassIO.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lbl_BypassIO.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_BypassIO.Location = new System.Drawing.Point(304, 167);
            this.lbl_BypassIO.Name = "lbl_BypassIO";
            this.lbl_BypassIO.Size = new System.Drawing.Size(38, 20);
            this.lbl_BypassIO.TabIndex = 100;
            this.lbl_BypassIO.Text = "OFF";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label3.Location = new System.Drawing.Point(10, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 99;
            this.label3.Text = "BypassIO";
            // 
            // cbx_CallBackType
            // 
            this.cbx_CallBackType.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.cbx_CallBackType.FormattingEnabled = true;
            this.cbx_CallBackType.Location = new System.Drawing.Point(195, 232);
            this.cbx_CallBackType.Name = "cbx_CallBackType";
            this.cbx_CallBackType.Size = new System.Drawing.Size(170, 28);
            this.cbx_CallBackType.TabIndex = 106;
            this.cbx_CallBackType.Tag = "CallBackType";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label2.Location = new System.Drawing.Point(10, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 105;
            this.label2.Text = "CallBackType";
            // 
            // cbx_BypassLightControl
            // 
            this.cbx_BypassLightControl.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbx_BypassLightControl.AutoSize = true;
            this.cbx_BypassLightControl.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
            this.cbx_BypassLightControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbx_BypassLightControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_BypassLightControl.Location = new System.Drawing.Point(233, 192);
            this.cbx_BypassLightControl.Name = "cbx_BypassLightControl";
            this.cbx_BypassLightControl.Size = new System.Drawing.Size(65, 34);
            this.cbx_BypassLightControl.TabIndex = 104;
            this.cbx_BypassLightControl.Tag = "BypassLightControl";
            this.cbx_BypassLightControl.Text = "         ";
            this.cbx_BypassLightControl.UseVisualStyleBackColor = true;
            this.cbx_BypassLightControl.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // lbl_BypassLightControl
            // 
            this.lbl_BypassLightControl.AutoSize = true;
            this.lbl_BypassLightControl.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lbl_BypassLightControl.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_BypassLightControl.Location = new System.Drawing.Point(304, 202);
            this.lbl_BypassLightControl.Name = "lbl_BypassLightControl";
            this.lbl_BypassLightControl.Size = new System.Drawing.Size(38, 20);
            this.lbl_BypassLightControl.TabIndex = 103;
            this.lbl_BypassLightControl.Text = "OFF";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.label5.Location = new System.Drawing.Point(10, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 20);
            this.label5.TabIndex = 102;
            this.label5.Text = "BypassLightControl";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.lbl_title);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(490, 43);
            this.panel1.TabIndex = 107;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(15, 12);
            this.lbl_title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(87, 19);
            this.lbl_title.TabIndex = 1;
            this.lbl_title.Text = "Config模組";
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_Save.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Save.ForeColor = System.Drawing.Color.White;
            this.btn_Save.Location = new System.Drawing.Point(393, 280);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 28);
            this.btn_Save.TabIndex = 108;
            this.btn_Save.Text = "儲存";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.DarkGray;
            this.btn_Close.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Close.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Close.Location = new System.Drawing.Point(308, 280);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 28);
            this.btn_Close.TabIndex = 109;
            this.btn_Close.Text = "關閉";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // ConfigModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 320);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbx_CallBackType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbx_BypassLightControl);
            this.Controls.Add(this.lbl_BypassLightControl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbx_BypassIO);
            this.Controls.Add(this.lbl_BypassIO);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbx_CameraInterface);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbx_LightOffSet);
            this.Controls.Add(this.lbl_LightOffSet);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.cbx_LightComportName);
            this.Controls.Add(this.label16);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConfigModule";
            this.Text = "Config模組";
            this.Load += new System.EventHandler(this.ConfigModule_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbx_LightOffSet;
        private System.Windows.Forms.Label lbl_LightOffSet;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cbx_LightComportName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbx_CameraInterface;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbx_BypassIO;
        private System.Windows.Forms.Label lbl_BypassIO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbx_CallBackType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbx_BypassLightControl;
        private System.Windows.Forms.Label lbl_BypassLightControl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Close;
    }
}