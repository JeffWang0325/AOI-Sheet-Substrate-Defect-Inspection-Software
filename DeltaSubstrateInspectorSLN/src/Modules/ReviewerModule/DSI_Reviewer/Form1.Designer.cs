namespace DSI_Reviewer
{
    partial class FormReviewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl_Review = new System.Windows.Forms.TabControl();
            this.tabPage_VisionPosition = new System.Windows.Forms.TabPage();
            this.groupBox_Cal = new System.Windows.Forms.GroupBox();
            this.richTextBox_Calc = new System.Windows.Forms.RichTextBox();
            this.richTextBox_Ｈistory = new System.Windows.Forms.RichTextBox();
            this.button_CalReset = new System.Windows.Forms.Button();
            this.hWindowControl_ImageShow02 = new HalconDotNet.HSmartWindowControl();
            this.richTextBox_Info02 = new System.Windows.Forms.RichTextBox();
            this.hWindowControl_ImageShow01 = new HalconDotNet.HSmartWindowControl();
            this.richTextBox_Info01 = new System.Windows.Forms.RichTextBox();
            this.tabPage_LaserCali = new System.Windows.Forms.TabPage();
            this.hWindowControl_LaserImageShow01 = new HalconDotNet.HSmartWindowControl();
            this.richTextBox_InfoLaser = new System.Windows.Forms.RichTextBox();
            this.button_LoadFileList = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox_nowReviewRoot = new System.Windows.Forms.TextBox();
            this.dataGridView_DirList = new System.Windows.Forms.DataGridView();
            this.textBox_nowSelectDirName = new System.Windows.Forms.TextBox();
            this.tabControl_Review.SuspendLayout();
            this.tabPage_VisionPosition.SuspendLayout();
            this.groupBox_Cal.SuspendLayout();
            this.tabPage_LaserCali.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DirList)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_Review
            // 
            this.tabControl_Review.Controls.Add(this.tabPage_VisionPosition);
            this.tabControl_Review.Controls.Add(this.tabPage_LaserCali);
            this.tabControl_Review.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl_Review.Location = new System.Drawing.Point(258, 12);
            this.tabControl_Review.Name = "tabControl_Review";
            this.tabControl_Review.SelectedIndex = 0;
            this.tabControl_Review.Size = new System.Drawing.Size(1400, 837);
            this.tabControl_Review.TabIndex = 1;
            this.tabControl_Review.SelectedIndexChanged += new System.EventHandler(this.tabControl_Review_SelectedIndexChanged);
            // 
            // tabPage_VisionPosition
            // 
            this.tabPage_VisionPosition.Controls.Add(this.groupBox_Cal);
            this.tabPage_VisionPosition.Controls.Add(this.hWindowControl_ImageShow02);
            this.tabPage_VisionPosition.Controls.Add(this.richTextBox_Info02);
            this.tabPage_VisionPosition.Controls.Add(this.hWindowControl_ImageShow01);
            this.tabPage_VisionPosition.Controls.Add(this.richTextBox_Info01);
            this.tabPage_VisionPosition.Location = new System.Drawing.Point(4, 29);
            this.tabPage_VisionPosition.Name = "tabPage_VisionPosition";
            this.tabPage_VisionPosition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_VisionPosition.Size = new System.Drawing.Size(1392, 804);
            this.tabPage_VisionPosition.TabIndex = 0;
            this.tabPage_VisionPosition.Text = "視覺定位";
            this.tabPage_VisionPosition.UseVisualStyleBackColor = true;
            // 
            // groupBox_Cal
            // 
            this.groupBox_Cal.Controls.Add(this.richTextBox_Calc);
            this.groupBox_Cal.Controls.Add(this.richTextBox_Ｈistory);
            this.groupBox_Cal.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox_Cal.Location = new System.Drawing.Point(6, 602);
            this.groupBox_Cal.Name = "groupBox_Cal";
            this.groupBox_Cal.Size = new System.Drawing.Size(1364, 203);
            this.groupBox_Cal.TabIndex = 82;
            this.groupBox_Cal.TabStop = false;
            this.groupBox_Cal.Text = "歷史紀錄與統計";
            // 
            // richTextBox_Calc
            // 
            this.richTextBox_Calc.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox_Calc.Location = new System.Drawing.Point(685, 21);
            this.richTextBox_Calc.Name = "richTextBox_Calc";
            this.richTextBox_Calc.Size = new System.Drawing.Size(673, 176);
            this.richTextBox_Calc.TabIndex = 82;
            this.richTextBox_Calc.Text = "";
            // 
            // richTextBox_Ｈistory
            // 
            this.richTextBox_Ｈistory.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox_Ｈistory.Location = new System.Drawing.Point(6, 21);
            this.richTextBox_Ｈistory.Name = "richTextBox_Ｈistory";
            this.richTextBox_Ｈistory.Size = new System.Drawing.Size(670, 176);
            this.richTextBox_Ｈistory.TabIndex = 80;
            this.richTextBox_Ｈistory.Text = "";
            // 
            // button_CalReset
            // 
            this.button_CalReset.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_CalReset.Location = new System.Drawing.Point(141, 12);
            this.button_CalReset.Name = "button_CalReset";
            this.button_CalReset.Size = new System.Drawing.Size(111, 54);
            this.button_CalReset.TabIndex = 81;
            this.button_CalReset.Text = "重置";
            this.button_CalReset.UseVisualStyleBackColor = true;
            this.button_CalReset.Click += new System.EventHandler(this.button_CalReset_Click);
            // 
            // hWindowControl_ImageShow02
            // 
            this.hWindowControl_ImageShow02.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWindowControl_ImageShow02.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWindowControl_ImageShow02.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_ImageShow02.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_ImageShow02.HDoubleClickToFitContent = true;
            this.hWindowControl_ImageShow02.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWindowControl_ImageShow02.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_ImageShow02.HKeepAspectRatio = false;
            this.hWindowControl_ImageShow02.HMoveContent = true;
            this.hWindowControl_ImageShow02.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hWindowControl_ImageShow02.Location = new System.Drawing.Point(691, 3);
            this.hWindowControl_ImageShow02.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl_ImageShow02.Name = "hWindowControl_ImageShow02";
            this.hWindowControl_ImageShow02.Size = new System.Drawing.Size(679, 509);
            this.hWindowControl_ImageShow02.TabIndex = 79;
            this.hWindowControl_ImageShow02.WindowSize = new System.Drawing.Size(679, 509);
            // 
            // richTextBox_Info02
            // 
            this.richTextBox_Info02.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox_Info02.Location = new System.Drawing.Point(691, 515);
            this.richTextBox_Info02.Name = "richTextBox_Info02";
            this.richTextBox_Info02.Size = new System.Drawing.Size(679, 81);
            this.richTextBox_Info02.TabIndex = 78;
            this.richTextBox_Info02.Text = "";
            // 
            // hWindowControl_ImageShow01
            // 
            this.hWindowControl_ImageShow01.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWindowControl_ImageShow01.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWindowControl_ImageShow01.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_ImageShow01.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_ImageShow01.HDoubleClickToFitContent = true;
            this.hWindowControl_ImageShow01.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWindowControl_ImageShow01.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_ImageShow01.HKeepAspectRatio = false;
            this.hWindowControl_ImageShow01.HMoveContent = true;
            this.hWindowControl_ImageShow01.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hWindowControl_ImageShow01.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl_ImageShow01.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl_ImageShow01.Name = "hWindowControl_ImageShow01";
            this.hWindowControl_ImageShow01.Size = new System.Drawing.Size(679, 509);
            this.hWindowControl_ImageShow01.TabIndex = 77;
            this.hWindowControl_ImageShow01.WindowSize = new System.Drawing.Size(679, 509);
            // 
            // richTextBox_Info01
            // 
            this.richTextBox_Info01.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.richTextBox_Info01.Location = new System.Drawing.Point(3, 515);
            this.richTextBox_Info01.Name = "richTextBox_Info01";
            this.richTextBox_Info01.Size = new System.Drawing.Size(679, 81);
            this.richTextBox_Info01.TabIndex = 76;
            this.richTextBox_Info01.Text = "";
            // 
            // tabPage_LaserCali
            // 
            this.tabPage_LaserCali.Controls.Add(this.hWindowControl_LaserImageShow01);
            this.tabPage_LaserCali.Controls.Add(this.richTextBox_InfoLaser);
            this.tabPage_LaserCali.Location = new System.Drawing.Point(4, 29);
            this.tabPage_LaserCali.Name = "tabPage_LaserCali";
            this.tabPage_LaserCali.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_LaserCali.Size = new System.Drawing.Size(1392, 804);
            this.tabPage_LaserCali.TabIndex = 1;
            this.tabPage_LaserCali.Text = "雷射校正";
            this.tabPage_LaserCali.UseVisualStyleBackColor = true;
            // 
            // hWindowControl_LaserImageShow01
            // 
            this.hWindowControl_LaserImageShow01.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWindowControl_LaserImageShow01.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWindowControl_LaserImageShow01.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_LaserImageShow01.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_LaserImageShow01.HDoubleClickToFitContent = true;
            this.hWindowControl_LaserImageShow01.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWindowControl_LaserImageShow01.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_LaserImageShow01.HKeepAspectRatio = false;
            this.hWindowControl_LaserImageShow01.HMoveContent = true;
            this.hWindowControl_LaserImageShow01.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hWindowControl_LaserImageShow01.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl_LaserImageShow01.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl_LaserImageShow01.Name = "hWindowControl_LaserImageShow01";
            this.hWindowControl_LaserImageShow01.Size = new System.Drawing.Size(679, 509);
            this.hWindowControl_LaserImageShow01.TabIndex = 79;
            this.hWindowControl_LaserImageShow01.WindowSize = new System.Drawing.Size(679, 509);
            // 
            // richTextBox_InfoLaser
            // 
            this.richTextBox_InfoLaser.Location = new System.Drawing.Point(3, 515);
            this.richTextBox_InfoLaser.Name = "richTextBox_InfoLaser";
            this.richTextBox_InfoLaser.Size = new System.Drawing.Size(679, 81);
            this.richTextBox_InfoLaser.TabIndex = 78;
            this.richTextBox_InfoLaser.Text = "";
            // 
            // button_LoadFileList
            // 
            this.button_LoadFileList.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_LoadFileList.Location = new System.Drawing.Point(12, 12);
            this.button_LoadFileList.Name = "button_LoadFileList";
            this.button_LoadFileList.Size = new System.Drawing.Size(111, 54);
            this.button_LoadFileList.TabIndex = 0;
            this.button_LoadFileList.Text = "載入";
            this.button_LoadFileList.UseVisualStyleBackColor = true;
            this.button_LoadFileList.Click += new System.EventHandler(this.button_LoadFileList_Click);
            // 
            // textBox_nowReviewRoot
            // 
            this.textBox_nowReviewRoot.Location = new System.Drawing.Point(12, 70);
            this.textBox_nowReviewRoot.Name = "textBox_nowReviewRoot";
            this.textBox_nowReviewRoot.Size = new System.Drawing.Size(240, 22);
            this.textBox_nowReviewRoot.TabIndex = 0;
            // 
            // dataGridView_DirList
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_DirList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_DirList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DirList.EnableHeadersVisualStyles = false;
            this.dataGridView_DirList.Location = new System.Drawing.Point(12, 126);
            this.dataGridView_DirList.Name = "dataGridView_DirList";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_DirList.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_DirList.RowHeadersVisible = false;
            this.dataGridView_DirList.RowTemplate.Height = 24;
            this.dataGridView_DirList.Size = new System.Drawing.Size(240, 723);
            this.dataGridView_DirList.TabIndex = 32;
            this.dataGridView_DirList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_DirList_CellContentClick);
            this.dataGridView_DirList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_DirList_CellEnter);
            // 
            // textBox_nowSelectDirName
            // 
            this.textBox_nowSelectDirName.Location = new System.Drawing.Point(12, 98);
            this.textBox_nowSelectDirName.Name = "textBox_nowSelectDirName";
            this.textBox_nowSelectDirName.Size = new System.Drawing.Size(240, 22);
            this.textBox_nowSelectDirName.TabIndex = 33;
            // 
            // FormReviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1678, 861);
            this.Controls.Add(this.textBox_nowSelectDirName);
            this.Controls.Add(this.dataGridView_DirList);
            this.Controls.Add(this.button_CalReset);
            this.Controls.Add(this.textBox_nowReviewRoot);
            this.Controls.Add(this.button_LoadFileList);
            this.Controls.Add(this.tabControl_Review);
            this.Name = "FormReviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DSI_Reviewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormReviewer_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.tabControl_Review.ResumeLayout(false);
            this.tabPage_VisionPosition.ResumeLayout(false);
            this.groupBox_Cal.ResumeLayout(false);
            this.tabPage_LaserCali.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DirList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl_Review;
        private System.Windows.Forms.TabPage tabPage_VisionPosition;
        private System.Windows.Forms.TabPage tabPage_LaserCali;
        private System.Windows.Forms.Button button_LoadFileList;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox_nowReviewRoot;
        private System.Windows.Forms.DataGridView dataGridView_DirList;
        private System.Windows.Forms.TextBox textBox_nowSelectDirName;
        private HalconDotNet.HSmartWindowControl hWindowControl_ImageShow02;
        private System.Windows.Forms.RichTextBox richTextBox_Info02;
        private HalconDotNet.HSmartWindowControl hWindowControl_ImageShow01;
        private System.Windows.Forms.RichTextBox richTextBox_Info01;
        private System.Windows.Forms.RichTextBox richTextBox_Ｈistory;
        private System.Windows.Forms.Button button_CalReset;
        private System.Windows.Forms.GroupBox groupBox_Cal;
        private System.Windows.Forms.RichTextBox richTextBox_Calc;
        private HalconDotNet.HSmartWindowControl hWindowControl_LaserImageShow01;
        private System.Windows.Forms.RichTextBox richTextBox_InfoLaser;
    }
}

