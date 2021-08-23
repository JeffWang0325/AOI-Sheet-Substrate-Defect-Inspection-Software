namespace DeltaSubstrateInspector.src.PositionMethod.LocateMethod.Location
{
    partial class Form_editDefect
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.button_SetColor = new System.Windows.Forms.Button();
            this.nud_Priority = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_yes = new System.Windows.Forms.Button();
            this.colorDialog_SetColor = new System.Windows.Forms.ColorDialog();
            this.trackBar_transparency = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label_transparencyValue_Min = new System.Windows.Forms.Label();
            this.label_transparencyValue_Max = new System.Windows.Forms.Label();
            this.label_transparencyValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Priority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_transparency)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(35, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "瑕疵名稱";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Name.Location = new System.Drawing.Point(12, 43);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(130, 26);
            this.textBox_Name.TabIndex = 15;
            // 
            // button_SetColor
            // 
            this.button_SetColor.BackColor = System.Drawing.Color.Red;
            this.button_SetColor.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_SetColor.ForeColor = System.Drawing.Color.Transparent;
            this.button_SetColor.Location = new System.Drawing.Point(301, 40);
            this.button_SetColor.Name = "button_SetColor";
            this.button_SetColor.Size = new System.Drawing.Size(49, 48);
            this.button_SetColor.TabIndex = 166;
            this.button_SetColor.UseVisualStyleBackColor = false;
            this.button_SetColor.Click += new System.EventHandler(this.button_SetColor_Click);
            // 
            // nud_Priority
            // 
            this.nud_Priority.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nud_Priority.Location = new System.Drawing.Point(175, 43);
            this.nud_Priority.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nud_Priority.Name = "nud_Priority";
            this.nud_Priority.Size = new System.Drawing.Size(88, 26);
            this.nud_Priority.TabIndex = 165;
            this.nud_Priority.Tag = "";
            this.nud_Priority.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(306, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 20);
            this.label4.TabIndex = 164;
            this.label4.Text = "顏色";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(193, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 163;
            this.label3.Text = "優先權";
            // 
            // button_cancel
            // 
            this.button_cancel.BackColor = System.Drawing.Color.Red;
            this.button_cancel.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_cancel.ForeColor = System.Drawing.Color.White;
            this.button_cancel.Location = new System.Drawing.Point(243, 124);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(110, 39);
            this.button_cancel.TabIndex = 162;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = false;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_yes
            // 
            this.button_yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_yes.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_yes.ForeColor = System.Drawing.Color.White;
            this.button_yes.Location = new System.Drawing.Point(127, 124);
            this.button_yes.Name = "button_yes";
            this.button_yes.Size = new System.Drawing.Size(110, 39);
            this.button_yes.TabIndex = 161;
            this.button_yes.Text = "確定";
            this.button_yes.UseVisualStyleBackColor = false;
            this.button_yes.Click += new System.EventHandler(this.button_yes_Click);
            // 
            // trackBar_transparency
            // 
            this.trackBar_transparency.Location = new System.Drawing.Point(392, 40);
            this.trackBar_transparency.Maximum = 255;
            this.trackBar_transparency.Name = "trackBar_transparency";
            this.trackBar_transparency.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_transparency.Size = new System.Drawing.Size(45, 104);
            this.trackBar_transparency.TabIndex = 167;
            this.trackBar_transparency.Value = 255;
            this.trackBar_transparency.Scroll += new System.EventHandler(this.trackBar_transparency_Scroll);
            this.trackBar_transparency.ValueChanged += new System.EventHandler(this.trackBar_transparency_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(380, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 20);
            this.label1.TabIndex = 168;
            this.label1.Text = "透明度";
            // 
            // label_transparencyValue_Min
            // 
            this.label_transparencyValue_Min.AutoSize = true;
            this.label_transparencyValue_Min.BackColor = System.Drawing.Color.Transparent;
            this.label_transparencyValue_Min.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_transparencyValue_Min.ForeColor = System.Drawing.Color.Black;
            this.label_transparencyValue_Min.Location = new System.Drawing.Point(369, 120);
            this.label_transparencyValue_Min.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_transparencyValue_Min.Name = "label_transparencyValue_Min";
            this.label_transparencyValue_Min.Size = new System.Drawing.Size(18, 20);
            this.label_transparencyValue_Min.TabIndex = 169;
            this.label_transparencyValue_Min.Text = "0";
            this.label_transparencyValue_Min.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_transparencyValue_Max
            // 
            this.label_transparencyValue_Max.AutoSize = true;
            this.label_transparencyValue_Max.BackColor = System.Drawing.Color.Transparent;
            this.label_transparencyValue_Max.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_transparencyValue_Max.ForeColor = System.Drawing.Color.Black;
            this.label_transparencyValue_Max.Location = new System.Drawing.Point(359, 43);
            this.label_transparencyValue_Max.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_transparencyValue_Max.Name = "label_transparencyValue_Max";
            this.label_transparencyValue_Max.Size = new System.Drawing.Size(36, 20);
            this.label_transparencyValue_Max.TabIndex = 170;
            this.label_transparencyValue_Max.Text = "255";
            this.label_transparencyValue_Max.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_transparencyValue
            // 
            this.label_transparencyValue.AutoSize = true;
            this.label_transparencyValue.BackColor = System.Drawing.Color.Transparent;
            this.label_transparencyValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_transparencyValue.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label_transparencyValue.Location = new System.Drawing.Point(413, 43);
            this.label_transparencyValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_transparencyValue.Name = "label_transparencyValue";
            this.label_transparencyValue.Size = new System.Drawing.Size(36, 20);
            this.label_transparencyValue.TabIndex = 171;
            this.label_transparencyValue.Text = "255";
            this.label_transparencyValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_editDefect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 170);
            this.Controls.Add(this.label_transparencyValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar_transparency);
            this.Controls.Add(this.label_transparencyValue_Max);
            this.Controls.Add(this.label_transparencyValue_Min);
            this.Controls.Add(this.button_SetColor);
            this.Controls.Add(this.nud_Priority);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_yes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_Name);
            this.Name = "Form_editDefect";
            this.Text = "Form_editDefect";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_editDefect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nud_Priority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_transparency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.Button button_SetColor;
        private System.Windows.Forms.NumericUpDown nud_Priority;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_yes;
        private System.Windows.Forms.ColorDialog colorDialog_SetColor;
        private System.Windows.Forms.TrackBar trackBar_transparency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_transparencyValue_Min;
        private System.Windows.Forms.Label label_transparencyValue_Max;
        private System.Windows.Forms.Label label_transparencyValue;
    }
}