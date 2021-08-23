namespace DeltaSubstrateInspector.UIControl
{
    partial class InspectResultUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectResultUI));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_DefectName = new System.Windows.Forms.Label();
            this.checkedListBox_DefectSel = new System.Windows.Forms.CheckedListBox();
            this.button_SetColor = new System.Windows.Forms.Button();
            this.colorDialog_SetColor = new System.Windows.Forms.ColorDialog();
            this.label_Color = new System.Windows.Forms.Label();
            this.checkBox_DrawType = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.button_isDefect = new System.Windows.Forms.Button();
            this.checkBox_DefectFrame = new System.Windows.Forms.CheckBox();
            this.nud_DefectFrame = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DefectFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(242)))));
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(0, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 171);
            this.panel1.TabIndex = 6;
            // 
            // lbl_DefectName
            // 
            this.lbl_DefectName.AutoSize = true;
            this.lbl_DefectName.BackColor = System.Drawing.Color.Transparent;
            this.lbl_DefectName.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_DefectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbl_DefectName.Location = new System.Drawing.Point(10, 3);
            this.lbl_DefectName.Name = "lbl_DefectName";
            this.lbl_DefectName.Size = new System.Drawing.Size(86, 17);
            this.lbl_DefectName.TabIndex = 8;
            this.lbl_DefectName.Text = "DefectName";
            // 
            // checkedListBox_DefectSel
            // 
            this.checkedListBox_DefectSel.CheckOnClick = true;
            this.checkedListBox_DefectSel.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkedListBox_DefectSel.FormattingEnabled = true;
            this.checkedListBox_DefectSel.Items.AddRange(new object[] {
            "Image",
            "Defect Region",
            "Defect Cell Center"});
            this.checkedListBox_DefectSel.Location = new System.Drawing.Point(14, 46);
            this.checkedListBox_DefectSel.Name = "checkedListBox_DefectSel";
            this.checkedListBox_DefectSel.Size = new System.Drawing.Size(145, 64);
            this.checkedListBox_DefectSel.TabIndex = 9;
            this.checkedListBox_DefectSel.SelectedValueChanged += new System.EventHandler(this.checkedListBox_DefectSel_SelectedValueChanged);
            this.checkedListBox_DefectSel.DoubleClick += new System.EventHandler(this.checkedListBox_DefectSel_DoubleClick);
            // 
            // button_SetColor
            // 
            this.button_SetColor.BackColor = System.Drawing.Color.Red;
            this.button_SetColor.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_SetColor.ForeColor = System.Drawing.Color.Transparent;
            this.button_SetColor.Location = new System.Drawing.Point(160, 20);
            this.button_SetColor.Name = "button_SetColor";
            this.button_SetColor.Size = new System.Drawing.Size(49, 48);
            this.button_SetColor.TabIndex = 10;
            this.button_SetColor.UseVisualStyleBackColor = false;
            this.button_SetColor.Click += new System.EventHandler(this.button_SetColor_Click);
            // 
            // label_Color
            // 
            this.label_Color.AutoSize = true;
            this.label_Color.BackColor = System.Drawing.Color.Transparent;
            this.label_Color.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_Color.Location = new System.Drawing.Point(160, 3);
            this.label_Color.Name = "label_Color";
            this.label_Color.Size = new System.Drawing.Size(51, 15);
            this.label_Color.TabIndex = 11;
            this.label_Color.Text = "顏色設定";
            // 
            // checkBox_DrawType
            // 
            this.checkBox_DrawType.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_DrawType.AutoSize = true;
            this.checkBox_DrawType.Checked = true;
            this.checkBox_DrawType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DrawType.Location = new System.Drawing.Point(160, 92);
            this.checkBox_DrawType.Name = "checkBox_DrawType";
            this.checkBox_DrawType.Size = new System.Drawing.Size(41, 23);
            this.checkBox_DrawType.TabIndex = 12;
            this.checkBox_DrawType.Text = "填滿";
            this.checkBox_DrawType.UseVisualStyleBackColor = true;
            this.checkBox_DrawType.CheckedChanged += new System.EventHandler(this.checkBox_DrawType_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(159, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "型式";
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.richTextBox_log.ForeColor = System.Drawing.Color.LimeGreen;
            this.richTextBox_log.Location = new System.Drawing.Point(13, 135);
            this.richTextBox_log.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.Size = new System.Drawing.Size(146, 35);
            this.richTextBox_log.TabIndex = 14;
            this.richTextBox_log.Text = "";
            // 
            // button_isDefect
            // 
            this.button_isDefect.BackColor = System.Drawing.Color.Red;
            this.button_isDefect.Enabled = false;
            this.button_isDefect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_isDefect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_isDefect.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_isDefect.Location = new System.Drawing.Point(15, 22);
            this.button_isDefect.Margin = new System.Windows.Forms.Padding(2);
            this.button_isDefect.Name = "button_isDefect";
            this.button_isDefect.Size = new System.Drawing.Size(94, 22);
            this.button_isDefect.TabIndex = 75;
            this.button_isDefect.Text = "Defect";
            this.button_isDefect.UseVisualStyleBackColor = false;
            // 
            // checkBox_DefectFrame
            // 
            this.checkBox_DefectFrame.AutoSize = true;
            this.checkBox_DefectFrame.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.checkBox_DefectFrame.Location = new System.Drawing.Point(16, 108);
            this.checkBox_DefectFrame.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_DefectFrame.Name = "checkBox_DefectFrame";
            this.checkBox_DefectFrame.Size = new System.Drawing.Size(79, 21);
            this.checkBox_DefectFrame.TabIndex = 76;
            this.checkBox_DefectFrame.Text = "瑕疵外框";
            this.checkBox_DefectFrame.UseVisualStyleBackColor = true;
            this.checkBox_DefectFrame.CheckedChanged += new System.EventHandler(this.checkBox_DefectFrame_CheckedChanged);
            // 
            // nud_DefectFrame
            // 
            this.nud_DefectFrame.DecimalPlaces = 1;
            this.nud_DefectFrame.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nud_DefectFrame.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nud_DefectFrame.Location = new System.Drawing.Point(95, 109);
            this.nud_DefectFrame.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nud_DefectFrame.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nud_DefectFrame.Name = "nud_DefectFrame";
            this.nud_DefectFrame.Size = new System.Drawing.Size(58, 23);
            this.nud_DefectFrame.TabIndex = 84;
            this.nud_DefectFrame.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nud_DefectFrame.ValueChanged += new System.EventHandler(this.nud_DefectFrame_ValueChanged);
            // 
            // InspectResultUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.checkBox_DefectFrame);
            this.Controls.Add(this.nud_DefectFrame);
            this.Controls.Add(this.button_isDefect);
            this.Controls.Add(this.richTextBox_log);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_DrawType);
            this.Controls.Add(this.label_Color);
            this.Controls.Add(this.button_SetColor);
            this.Controls.Add(this.checkedListBox_DefectSel);
            this.Controls.Add(this.lbl_DefectName);
            this.Controls.Add(this.panel1);
            this.Name = "InspectResultUI";
            this.Size = new System.Drawing.Size(212, 171);
            ((System.ComponentModel.ISupportInitialize)(this.nud_DefectFrame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_DefectName;
        private System.Windows.Forms.CheckedListBox checkedListBox_DefectSel;
        private System.Windows.Forms.Button button_SetColor;
        private System.Windows.Forms.ColorDialog colorDialog_SetColor;
        private System.Windows.Forms.Label label_Color;
        private System.Windows.Forms.CheckBox checkBox_DrawType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.Button button_isDefect;
        private System.Windows.Forms.CheckBox checkBox_DefectFrame;
        private System.Windows.Forms.NumericUpDown nud_DefectFrame;
    }
}
