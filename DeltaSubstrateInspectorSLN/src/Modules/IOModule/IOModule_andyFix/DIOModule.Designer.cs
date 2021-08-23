namespace IOModule
{
    partial class DIO_Board
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DIO_Board));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ModuleMeasureDone = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ModuleMeasureStart = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ModuleOnline = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Sys_MeasureDone = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Sys_MeasureStart = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Sys_Online = new System.Windows.Forms.Label();
            this.instantDiCtrl1 = new Automation.BDaq.InstantDiCtrl(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.instantDoCtrl1 = new Automation.BDaq.InstantDoCtrl(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sytem Online";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.ModuleMeasureDone);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.ModuleMeasureStart);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.ModuleOnline);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.Sys_MeasureDone);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.Sys_MeasureStart);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Sys_Online);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(25, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 120);
            this.panel1.TabIndex = 1;
            // 
            // ModuleMeasureDone
            // 
            this.ModuleMeasureDone.BackColor = System.Drawing.Color.Silver;
            this.ModuleMeasureDone.Location = new System.Drawing.Point(271, 84);
            this.ModuleMeasureDone.Name = "ModuleMeasureDone";
            this.ModuleMeasureDone.Size = new System.Drawing.Size(28, 28);
            this.ModuleMeasureDone.TabIndex = 25;
            this.ModuleMeasureDone.Click += new System.EventHandler(this.output_control);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(237, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "Module Measure Done";
            // 
            // ModuleMeasureStart
            // 
            this.ModuleMeasureStart.BackColor = System.Drawing.Color.Silver;
            this.ModuleMeasureStart.Location = new System.Drawing.Point(162, 84);
            this.ModuleMeasureStart.Name = "ModuleMeasureStart";
            this.ModuleMeasureStart.Size = new System.Drawing.Size(28, 28);
            this.ModuleMeasureStart.TabIndex = 24;
            this.ModuleMeasureStart.Click += new System.EventHandler(this.output_control);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(124, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "Module Measure Start";
            // 
            // ModuleOnline
            // 
            this.ModuleOnline.BackColor = System.Drawing.Color.Silver;
            this.ModuleOnline.Location = new System.Drawing.Point(49, 84);
            this.ModuleOnline.Name = "ModuleOnline";
            this.ModuleOnline.Size = new System.Drawing.Size(28, 28);
            this.ModuleOnline.TabIndex = 23;
            this.ModuleOnline.Click += new System.EventHandler(this.output_control);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Module Online";
            // 
            // Sys_MeasureDone
            // 
            this.Sys_MeasureDone.BackColor = System.Drawing.Color.Silver;
            this.Sys_MeasureDone.Location = new System.Drawing.Point(271, 27);
            this.Sys_MeasureDone.Name = "Sys_MeasureDone";
            this.Sys_MeasureDone.Size = new System.Drawing.Size(28, 28);
            this.Sys_MeasureDone.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "System Measure Done";
            // 
            // Sys_MeasureStart
            // 
            this.Sys_MeasureStart.BackColor = System.Drawing.Color.Silver;
            this.Sys_MeasureStart.Location = new System.Drawing.Point(162, 27);
            this.Sys_MeasureStart.Name = "Sys_MeasureStart";
            this.Sys_MeasureStart.Size = new System.Drawing.Size(28, 28);
            this.Sys_MeasureStart.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "System Measure Start";
            // 
            // Sys_Online
            // 
            this.Sys_Online.BackColor = System.Drawing.Color.Silver;
            this.Sys_Online.Location = new System.Drawing.Point(49, 27);
            this.Sys_Online.Name = "Sys_Online";
            this.Sys_Online.Size = new System.Drawing.Size(28, 28);
            this.Sys_Online.TabIndex = 20;
            // 
            // instantDiCtrl1
            // 
            this.instantDiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantDiCtrl1._StateStream")));
            // 
            // timer1
            // 
            this.timer1.Interval = 30;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // instantDoCtrl1
            // 
            this.instantDoCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantDoCtrl1._StateStream")));
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.OrangeRed;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(368, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "x";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.SeaGreen;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(25, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 20);
            this.button2.TabIndex = 3;
            this.button2.Text = "connect";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DIO_Board
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(400, 170);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DIO_Board";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ModuleMeasureDone;
        private System.Windows.Forms.Label ModuleMeasureStart;
        private System.Windows.Forms.Label ModuleOnline;
        private System.Windows.Forms.Label Sys_MeasureDone;
        private System.Windows.Forms.Label Sys_MeasureStart;
        private System.Windows.Forms.Label Sys_Online;
        private Automation.BDaq.InstantDiCtrl instantDiCtrl1;
        private System.Windows.Forms.Timer timer1;
        private Automation.BDaq.InstantDoCtrl instantDoCtrl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

