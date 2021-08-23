namespace Risitanse_AOI.UIControl
{
    partial class ColorSpaceSetting
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmb_colorspace = new System.Windows.Forms.ComboBox();
            this.channel_1_img = new System.Windows.Forms.PictureBox();
            this.channel_3_img = new System.Windows.Forms.PictureBox();
            this.channel_2_img = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.channel_1_img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel_3_img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel_2_img)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.channel_1_img);
            this.panel2.Controls.Add(this.channel_3_img);
            this.panel2.Controls.Add(this.channel_2_img);
            this.panel2.Location = new System.Drawing.Point(13, 14);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(351, 1055);
            this.panel2.TabIndex = 49;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmb_colorspace);
            this.groupBox2.Location = new System.Drawing.Point(15, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(322, 88);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            // 
            // cmb_colorspace
            // 
            this.cmb_colorspace.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmb_colorspace.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.cmb_colorspace.FormattingEnabled = true;
            this.cmb_colorspace.Items.AddRange(new object[] {
            "一般",
            "RGB",
            "HSV",
            "LCH"});
            this.cmb_colorspace.Location = new System.Drawing.Point(11, 29);
            this.cmb_colorspace.Name = "cmb_colorspace";
            this.cmb_colorspace.Size = new System.Drawing.Size(300, 38);
            this.cmb_colorspace.TabIndex = 51;
            this.cmb_colorspace.Text = "色彩空間";
            // 
            // channel_1_img
            // 
            this.channel_1_img.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.channel_1_img.Location = new System.Drawing.Point(26, 111);
            this.channel_1_img.Margin = new System.Windows.Forms.Padding(4);
            this.channel_1_img.Name = "channel_1_img";
            this.channel_1_img.Size = new System.Drawing.Size(300, 300);
            this.channel_1_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.channel_1_img.TabIndex = 13;
            this.channel_1_img.TabStop = false;
            this.channel_1_img.Tag = "1";
            // 
            // channel_3_img
            // 
            this.channel_3_img.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.channel_3_img.Location = new System.Drawing.Point(26, 727);
            this.channel_3_img.Margin = new System.Windows.Forms.Padding(4);
            this.channel_3_img.Name = "channel_3_img";
            this.channel_3_img.Size = new System.Drawing.Size(300, 300);
            this.channel_3_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.channel_3_img.TabIndex = 11;
            this.channel_3_img.TabStop = false;
            this.channel_3_img.Tag = "3";
            // 
            // channel_2_img
            // 
            this.channel_2_img.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.channel_2_img.Location = new System.Drawing.Point(26, 419);
            this.channel_2_img.Margin = new System.Windows.Forms.Padding(4);
            this.channel_2_img.Name = "channel_2_img";
            this.channel_2_img.Size = new System.Drawing.Size(300, 300);
            this.channel_2_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.channel_2_img.TabIndex = 12;
            this.channel_2_img.TabStop = false;
            this.channel_2_img.Tag = "2";
            // 
            // ColorSpaceSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "ColorSpaceSetting";
            this.Size = new System.Drawing.Size(377, 1083);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.channel_1_img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel_3_img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel_2_img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmb_colorspace;
        private System.Windows.Forms.PictureBox channel_1_img;
        private System.Windows.Forms.PictureBox channel_3_img;
        private System.Windows.Forms.PictureBox channel_2_img;
    }
}
