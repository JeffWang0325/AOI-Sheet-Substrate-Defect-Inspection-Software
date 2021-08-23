namespace DeltaSubstrateInspector.src.Modules.NewInspectModule.InductanceInsp
{
    partial class FormDAVSInsp
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
            this.txbInspName = new System.Windows.Forms.TextBox();
            this.labInspEdgeSkipSize = new System.Windows.Forms.Label();
            this.nudInspEdgeSkipHeight = new System.Windows.Forms.NumericUpDown();
            this.nudInspEdgeSkipSize = new System.Windows.Forms.NumericUpDown();
            this.cbxInspEnabled = new System.Windows.Forms.CheckBox();
            this.btnSetupName = new System.Windows.Forms.Button();
            this.labInspImgIndex = new System.Windows.Forms.Label();
            this.nudImageIndex = new System.Windows.Forms.NumericUpDown();
            this.comboBoxRegionSelect = new System.Windows.Forms.ComboBox();
            this.labRegionIndex = new System.Windows.Forms.Label();
            this.nudEdgeExtWidth = new System.Windows.Forms.NumericUpDown();
            this.nudEdgeExtHeight = new System.Windows.Forms.NumericUpDown();
            this.labEdgeExtSize = new System.Windows.Forms.Label();
            this.HWindowDisplay = new HalconDotNet.HSmartWindowControl();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.labInspType = new System.Windows.Forms.Label();
            this.comboBoxInspType = new System.Windows.Forms.ComboBox();
            this.labPredictionMode = new System.Windows.Forms.Label();
            this.comboBoxPredictionMode = new System.Windows.Forms.ComboBox();
            this.labPath = new System.Windows.Forms.Label();
            this.txbImgHSDirPath = new System.Windows.Forms.TextBox();
            this.txbModelPath = new System.Windows.Forms.TextBox();
            this.labModePath = new System.Windows.Forms.Label();
            this.btnSegTest = new System.Windows.Forms.Button();
            this.btnInspTest = new System.Windows.Forms.Button();
            this.cbxListSaveImg = new System.Windows.Forms.CheckedListBox();
            this.labNum = new System.Windows.Forms.Label();
            this.labSaveImg = new System.Windows.Forms.Label();
            this.labClassNum = new System.Windows.Forms.Label();
            this.cbxbFixSize = new System.Windows.Forms.CheckBox();
            this.nudFixSizeWidth = new System.Windows.Forms.NumericUpDown();
            this.nudFixSizeHeight = new System.Windows.Forms.NumericUpDown();
            this.labSaveImagePath = new System.Windows.Forms.Label();
            this.txbSaveImagePath = new System.Windows.Forms.TextBox();
            this.comboBoxMultiTHBand = new System.Windows.Forms.ComboBox();
            this.labMultiTHBand = new System.Windows.Forms.Label();
            this.comboBox_ImageSource = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspEdgeSkipHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspEdgeSkipSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeExtWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeExtHeight)).BeginInit();
            this.panelDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFixSizeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFixSizeHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // txbInspName
            // 
            this.txbInspName.Location = new System.Drawing.Point(91, 9);
            this.txbInspName.Name = "txbInspName";
            this.txbInspName.Size = new System.Drawing.Size(100, 23);
            this.txbInspName.TabIndex = 97;
            this.txbInspName.Text = "Defect";
            // 
            // labInspEdgeSkipSize
            // 
            this.labInspEdgeSkipSize.AutoSize = true;
            this.labInspEdgeSkipSize.Location = new System.Drawing.Point(9, 106);
            this.labInspEdgeSkipSize.Name = "labInspEdgeSkipSize";
            this.labInspEdgeSkipSize.Size = new System.Drawing.Size(83, 16);
            this.labInspEdgeSkipSize.TabIndex = 85;
            this.labInspEdgeSkipSize.Text = "邊緣忽略大小:";
            // 
            // nudInspEdgeSkipHeight
            // 
            this.nudInspEdgeSkipHeight.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInspEdgeSkipHeight.Location = new System.Drawing.Point(159, 103);
            this.nudInspEdgeSkipHeight.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudInspEdgeSkipHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInspEdgeSkipHeight.Name = "nudInspEdgeSkipHeight";
            this.nudInspEdgeSkipHeight.Size = new System.Drawing.Size(58, 23);
            this.nudInspEdgeSkipHeight.TabIndex = 83;
            this.nudInspEdgeSkipHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInspEdgeSkipHeight.ValueChanged += new System.EventHandler(this.nudInspEdgeSkipHeight_ValueChanged);
            // 
            // nudInspEdgeSkipSize
            // 
            this.nudInspEdgeSkipSize.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudInspEdgeSkipSize.Location = new System.Drawing.Point(98, 103);
            this.nudInspEdgeSkipSize.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudInspEdgeSkipSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInspEdgeSkipSize.Name = "nudInspEdgeSkipSize";
            this.nudInspEdgeSkipSize.Size = new System.Drawing.Size(58, 23);
            this.nudInspEdgeSkipSize.TabIndex = 83;
            this.nudInspEdgeSkipSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInspEdgeSkipSize.ValueChanged += new System.EventHandler(this.nudInspEdgeSkipSize_ValueChanged);
            // 
            // cbxInspEnabled
            // 
            this.cbxInspEnabled.AutoSize = true;
            this.cbxInspEnabled.Location = new System.Drawing.Point(12, 12);
            this.cbxInspEnabled.Name = "cbxInspEnabled";
            this.cbxInspEnabled.Size = new System.Drawing.Size(75, 20);
            this.cbxInspEnabled.TabIndex = 93;
            this.cbxInspEnabled.Text = "檢測啟用";
            this.cbxInspEnabled.UseVisualStyleBackColor = true;
            this.cbxInspEnabled.CheckedChanged += new System.EventHandler(this.cbxInspEnabled_CheckedChanged);
            // 
            // btnSetupName
            // 
            this.btnSetupName.Location = new System.Drawing.Point(197, 9);
            this.btnSetupName.Name = "btnSetupName";
            this.btnSetupName.Size = new System.Drawing.Size(67, 23);
            this.btnSetupName.TabIndex = 103;
            this.btnSetupName.Text = "確認名稱";
            this.btnSetupName.UseVisualStyleBackColor = true;
            this.btnSetupName.Click += new System.EventHandler(this.btnSetupName_Click);
            // 
            // labInspImgIndex
            // 
            this.labInspImgIndex.AutoSize = true;
            this.labInspImgIndex.Location = new System.Drawing.Point(82, 76);
            this.labInspImgIndex.Name = "labInspImgIndex";
            this.labInspImgIndex.Size = new System.Drawing.Size(48, 16);
            this.labInspImgIndex.TabIndex = 113;
            this.labInspImgIndex.Text = "影像ID:";
            // 
            // nudImageIndex
            // 
            this.nudImageIndex.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudImageIndex.Location = new System.Drawing.Point(142, 73);
            this.nudImageIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudImageIndex.Name = "nudImageIndex";
            this.nudImageIndex.Size = new System.Drawing.Size(47, 23);
            this.nudImageIndex.TabIndex = 114;
            this.nudImageIndex.ValueChanged += new System.EventHandler(this.nudImageIndex_ValueChanged);
            // 
            // comboBoxRegionSelect
            // 
            this.comboBoxRegionSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegionSelect.FormattingEnabled = true;
            this.comboBoxRegionSelect.Location = new System.Drawing.Point(93, 41);
            this.comboBoxRegionSelect.Name = "comboBoxRegionSelect";
            this.comboBoxRegionSelect.Size = new System.Drawing.Size(171, 24);
            this.comboBoxRegionSelect.TabIndex = 112;
            this.comboBoxRegionSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxRegionSelect_SelectedIndexChanged);
            // 
            // labRegionIndex
            // 
            this.labRegionIndex.AutoSize = true;
            this.labRegionIndex.Location = new System.Drawing.Point(9, 45);
            this.labRegionIndex.Name = "labRegionIndex";
            this.labRegionIndex.Size = new System.Drawing.Size(75, 16);
            this.labRegionIndex.TabIndex = 111;
            this.labRegionIndex.Text = "Region ID : ";
            // 
            // nudEdgeExtWidth
            // 
            this.nudEdgeExtWidth.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudEdgeExtWidth.Location = new System.Drawing.Point(98, 128);
            this.nudEdgeExtWidth.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudEdgeExtWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEdgeExtWidth.Name = "nudEdgeExtWidth";
            this.nudEdgeExtWidth.Size = new System.Drawing.Size(58, 23);
            this.nudEdgeExtWidth.TabIndex = 83;
            this.nudEdgeExtWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEdgeExtWidth.ValueChanged += new System.EventHandler(this.nudEdgeExtWidth_ValueChanged);
            // 
            // nudEdgeExtHeight
            // 
            this.nudEdgeExtHeight.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.nudEdgeExtHeight.Location = new System.Drawing.Point(159, 128);
            this.nudEdgeExtHeight.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudEdgeExtHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEdgeExtHeight.Name = "nudEdgeExtHeight";
            this.nudEdgeExtHeight.Size = new System.Drawing.Size(58, 23);
            this.nudEdgeExtHeight.TabIndex = 83;
            this.nudEdgeExtHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEdgeExtHeight.ValueChanged += new System.EventHandler(this.nudEdgeExtHeight_ValueChanged);
            // 
            // labEdgeExtSize
            // 
            this.labEdgeExtSize.AutoSize = true;
            this.labEdgeExtSize.Location = new System.Drawing.Point(9, 131);
            this.labEdgeExtSize.Name = "labEdgeExtSize";
            this.labEdgeExtSize.Size = new System.Drawing.Size(83, 16);
            this.labEdgeExtSize.TabIndex = 85;
            this.labEdgeExtSize.Text = "邊緣擴張大小:";
            // 
            // HWindowDisplay
            // 
            this.HWindowDisplay.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.HWindowDisplay.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.HWindowDisplay.HDoubleClickToFitContent = true;
            this.HWindowDisplay.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.HWindowDisplay.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 3000);
            this.HWindowDisplay.HKeepAspectRatio = true;
            this.HWindowDisplay.HMoveContent = true;
            this.HWindowDisplay.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.HWindowDisplay.ImeMode = System.Windows.Forms.ImeMode.On;
            this.HWindowDisplay.Location = new System.Drawing.Point(3, 3);
            this.HWindowDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.HWindowDisplay.Name = "HWindowDisplay";
            this.HWindowDisplay.Size = new System.Drawing.Size(910, 715);
            this.HWindowDisplay.TabIndex = 121;
            this.HWindowDisplay.WindowSize = new System.Drawing.Size(910, 715);
            // 
            // panelDisplay
            // 
            this.panelDisplay.Controls.Add(this.HWindowDisplay);
            this.panelDisplay.Location = new System.Drawing.Point(312, 9);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(916, 721);
            this.panelDisplay.TabIndex = 122;
            // 
            // labInspType
            // 
            this.labInspType.AutoSize = true;
            this.labInspType.Location = new System.Drawing.Point(9, 219);
            this.labInspType.Name = "labInspType";
            this.labInspType.Size = new System.Drawing.Size(59, 16);
            this.labInspType.TabIndex = 123;
            this.labInspType.Text = "檢測模式:";
            // 
            // comboBoxInspType
            // 
            this.comboBoxInspType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInspType.FormattingEnabled = true;
            this.comboBoxInspType.Items.AddRange(new object[] {
            "Online",
            "Offline"});
            this.comboBoxInspType.Location = new System.Drawing.Point(91, 216);
            this.comboBoxInspType.Name = "comboBoxInspType";
            this.comboBoxInspType.Size = new System.Drawing.Size(121, 24);
            this.comboBoxInspType.TabIndex = 124;
            this.comboBoxInspType.SelectedIndexChanged += new System.EventHandler(this.comboBoxInspType_SelectedIndexChanged);
            // 
            // labPredictionMode
            // 
            this.labPredictionMode.AutoSize = true;
            this.labPredictionMode.Location = new System.Drawing.Point(9, 256);
            this.labPredictionMode.Name = "labPredictionMode";
            this.labPredictionMode.Size = new System.Drawing.Size(59, 16);
            this.labPredictionMode.TabIndex = 125;
            this.labPredictionMode.Text = "預測模式:";
            // 
            // comboBoxPredictionMode
            // 
            this.comboBoxPredictionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPredictionMode.FormattingEnabled = true;
            this.comboBoxPredictionMode.Items.AddRange(new object[] {
            "BGR",
            "RGB"});
            this.comboBoxPredictionMode.Location = new System.Drawing.Point(91, 253);
            this.comboBoxPredictionMode.Name = "comboBoxPredictionMode";
            this.comboBoxPredictionMode.Size = new System.Drawing.Size(121, 24);
            this.comboBoxPredictionMode.TabIndex = 124;
            this.comboBoxPredictionMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxPredictionMode_SelectedIndexChanged);
            // 
            // labPath
            // 
            this.labPath.AutoSize = true;
            this.labPath.Location = new System.Drawing.Point(9, 292);
            this.labPath.Name = "labPath";
            this.labPath.Size = new System.Drawing.Size(89, 16);
            this.labPath.TabIndex = 127;
            this.labPath.Text = "交握暫存路徑 : ";
            // 
            // txbImgHSDirPath
            // 
            this.txbImgHSDirPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txbImgHSDirPath.Location = new System.Drawing.Point(104, 289);
            this.txbImgHSDirPath.Name = "txbImgHSDirPath";
            this.txbImgHSDirPath.ReadOnly = true;
            this.txbImgHSDirPath.Size = new System.Drawing.Size(202, 23);
            this.txbImgHSDirPath.TabIndex = 126;
            this.txbImgHSDirPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txbImgHSDirPath_MouseClick);
            // 
            // txbModelPath
            // 
            this.txbModelPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txbModelPath.Location = new System.Drawing.Point(104, 325);
            this.txbModelPath.Name = "txbModelPath";
            this.txbModelPath.ReadOnly = true;
            this.txbModelPath.Size = new System.Drawing.Size(202, 23);
            this.txbModelPath.TabIndex = 126;
            this.txbModelPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txbModelPath_MouseClick);
            // 
            // labModePath
            // 
            this.labModePath.AutoSize = true;
            this.labModePath.Location = new System.Drawing.Point(9, 328);
            this.labModePath.Name = "labModePath";
            this.labModePath.Size = new System.Drawing.Size(79, 16);
            this.labModePath.TabIndex = 127;
            this.labModePath.Text = "Model路徑 : ";
            // 
            // btnSegTest
            // 
            this.btnSegTest.Location = new System.Drawing.Point(154, 571);
            this.btnSegTest.Name = "btnSegTest";
            this.btnSegTest.Size = new System.Drawing.Size(71, 23);
            this.btnSegTest.TabIndex = 128;
            this.btnSegTest.Text = "切割";
            this.btnSegTest.UseVisualStyleBackColor = true;
            this.btnSegTest.Click += new System.EventHandler(this.btnSegTest_Click);
            // 
            // btnInspTest
            // 
            this.btnInspTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnInspTest.Location = new System.Drawing.Point(231, 571);
            this.btnInspTest.Name = "btnInspTest";
            this.btnInspTest.Size = new System.Drawing.Size(75, 23);
            this.btnInspTest.TabIndex = 129;
            this.btnInspTest.Text = "測試";
            this.btnInspTest.UseVisualStyleBackColor = true;
            this.btnInspTest.Click += new System.EventHandler(this.btnInspTest_Click);
            // 
            // cbxListSaveImg
            // 
            this.cbxListSaveImg.FormattingEnabled = true;
            this.cbxListSaveImg.Location = new System.Drawing.Point(12, 422);
            this.cbxListSaveImg.Name = "cbxListSaveImg";
            this.cbxListSaveImg.Size = new System.Drawing.Size(224, 94);
            this.cbxListSaveImg.TabIndex = 133;
            this.cbxListSaveImg.SelectedValueChanged += new System.EventHandler(this.cbxListSaveImg_SelectedValueChanged);
            this.cbxListSaveImg.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cbxListSaveImg_MouseDoubleClick);
            // 
            // labNum
            // 
            this.labNum.AutoSize = true;
            this.labNum.Location = new System.Drawing.Point(86, 362);
            this.labNum.Name = "labNum";
            this.labNum.Size = new System.Drawing.Size(15, 16);
            this.labNum.TabIndex = 130;
            this.labNum.Text = "0";
            // 
            // labSaveImg
            // 
            this.labSaveImg.AutoSize = true;
            this.labSaveImg.Location = new System.Drawing.Point(9, 391);
            this.labSaveImg.Name = "labSaveImg";
            this.labSaveImg.Size = new System.Drawing.Size(125, 16);
            this.labSaveImg.TabIndex = 131;
            this.labSaveImg.Text = "檢測時是否儲存影像 : ";
            // 
            // labClassNum
            // 
            this.labClassNum.AutoSize = true;
            this.labClassNum.Location = new System.Drawing.Point(9, 361);
            this.labClassNum.Name = "labClassNum";
            this.labClassNum.Size = new System.Drawing.Size(65, 16);
            this.labClassNum.TabIndex = 132;
            this.labClassNum.Text = "分類數量 : ";
            // 
            // cbxbFixSize
            // 
            this.cbxbFixSize.AutoSize = true;
            this.cbxbFixSize.Location = new System.Drawing.Point(9, 174);
            this.cbxbFixSize.Name = "cbxbFixSize";
            this.cbxbFixSize.Size = new System.Drawing.Size(75, 20);
            this.cbxbFixSize.TabIndex = 134;
            this.cbxbFixSize.Text = "固定大小";
            this.cbxbFixSize.UseVisualStyleBackColor = true;
            this.cbxbFixSize.CheckedChanged += new System.EventHandler(this.cbxbFixSize_CheckedChanged);
            // 
            // nudFixSizeWidth
            // 
            this.nudFixSizeWidth.Location = new System.Drawing.Point(98, 173);
            this.nudFixSizeWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudFixSizeWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFixSizeWidth.Name = "nudFixSizeWidth";
            this.nudFixSizeWidth.Size = new System.Drawing.Size(58, 23);
            this.nudFixSizeWidth.TabIndex = 135;
            this.nudFixSizeWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFixSizeWidth.ValueChanged += new System.EventHandler(this.nudFixSizeWidth_ValueChanged);
            // 
            // nudFixSizeHeight
            // 
            this.nudFixSizeHeight.Location = new System.Drawing.Point(159, 173);
            this.nudFixSizeHeight.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nudFixSizeHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFixSizeHeight.Name = "nudFixSizeHeight";
            this.nudFixSizeHeight.Size = new System.Drawing.Size(58, 23);
            this.nudFixSizeHeight.TabIndex = 135;
            this.nudFixSizeHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFixSizeHeight.ValueChanged += new System.EventHandler(this.nudFixSizeHeight_ValueChanged);
            // 
            // labSaveImagePath
            // 
            this.labSaveImagePath.AutoSize = true;
            this.labSaveImagePath.Location = new System.Drawing.Point(12, 534);
            this.labSaveImagePath.Name = "labSaveImagePath";
            this.labSaveImagePath.Size = new System.Drawing.Size(59, 16);
            this.labSaveImagePath.TabIndex = 136;
            this.labSaveImagePath.Text = "存圖路徑:";
            // 
            // txbSaveImagePath
            // 
            this.txbSaveImagePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txbSaveImagePath.Location = new System.Drawing.Point(104, 531);
            this.txbSaveImagePath.Name = "txbSaveImagePath";
            this.txbSaveImagePath.ReadOnly = true;
            this.txbSaveImagePath.Size = new System.Drawing.Size(202, 23);
            this.txbSaveImagePath.TabIndex = 126;
            this.txbSaveImagePath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txbSaveImagePath_MouseClick);
            // 
            // comboBoxMultiTHBand
            // 
            this.comboBoxMultiTHBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMultiTHBand.FormattingEnabled = true;
            this.comboBoxMultiTHBand.Items.AddRange(new object[] {
            "R",
            "G",
            "B",
            "Gray",
            "y",
            "u",
            "v",
            "v-u"});
            this.comboBoxMultiTHBand.Location = new System.Drawing.Point(234, 73);
            this.comboBoxMultiTHBand.Name = "comboBoxMultiTHBand";
            this.comboBoxMultiTHBand.Size = new System.Drawing.Size(54, 24);
            this.comboBoxMultiTHBand.TabIndex = 138;
            this.comboBoxMultiTHBand.SelectedIndexChanged += new System.EventHandler(this.comboBoxMultiTHBand_SelectedIndexChanged);
            // 
            // labMultiTHBand
            // 
            this.labMultiTHBand.AutoSize = true;
            this.labMultiTHBand.Location = new System.Drawing.Point(192, 76);
            this.labMultiTHBand.Name = "labMultiTHBand";
            this.labMultiTHBand.Size = new System.Drawing.Size(35, 16);
            this.labMultiTHBand.TabIndex = 137;
            this.labMultiTHBand.Text = "頻帶:";
            // 
            // comboBox_ImageSource
            // 
            this.comboBox_ImageSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ImageSource.FormattingEnabled = true;
            this.comboBox_ImageSource.Items.AddRange(new object[] {
            "原始",
            "影像A",
            "影像B"});
            this.comboBox_ImageSource.Location = new System.Drawing.Point(7, 73);
            this.comboBox_ImageSource.Name = "comboBox_ImageSource";
            this.comboBox_ImageSource.Size = new System.Drawing.Size(70, 24);
            this.comboBox_ImageSource.TabIndex = 142;
            this.comboBox_ImageSource.SelectedIndexChanged += new System.EventHandler(this.comboBox_ImageSource_SelectedIndexChanged);
            // 
            // FormDAVSInsp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 742);
            this.Controls.Add(this.comboBox_ImageSource);
            this.Controls.Add(this.comboBoxMultiTHBand);
            this.Controls.Add(this.labMultiTHBand);
            this.Controls.Add(this.labSaveImagePath);
            this.Controls.Add(this.nudFixSizeHeight);
            this.Controls.Add(this.nudFixSizeWidth);
            this.Controls.Add(this.cbxbFixSize);
            this.Controls.Add(this.cbxListSaveImg);
            this.Controls.Add(this.labNum);
            this.Controls.Add(this.labSaveImg);
            this.Controls.Add(this.labClassNum);
            this.Controls.Add(this.btnInspTest);
            this.Controls.Add(this.btnSegTest);
            this.Controls.Add(this.labModePath);
            this.Controls.Add(this.txbSaveImagePath);
            this.Controls.Add(this.txbModelPath);
            this.Controls.Add(this.labPath);
            this.Controls.Add(this.txbImgHSDirPath);
            this.Controls.Add(this.labPredictionMode);
            this.Controls.Add(this.comboBoxPredictionMode);
            this.Controls.Add(this.comboBoxInspType);
            this.Controls.Add(this.labInspType);
            this.Controls.Add(this.panelDisplay);
            this.Controls.Add(this.labInspImgIndex);
            this.Controls.Add(this.nudImageIndex);
            this.Controls.Add(this.comboBoxRegionSelect);
            this.Controls.Add(this.labRegionIndex);
            this.Controls.Add(this.labEdgeExtSize);
            this.Controls.Add(this.labInspEdgeSkipSize);
            this.Controls.Add(this.nudEdgeExtHeight);
            this.Controls.Add(this.nudInspEdgeSkipHeight);
            this.Controls.Add(this.btnSetupName);
            this.Controls.Add(this.nudEdgeExtWidth);
            this.Controls.Add(this.nudInspEdgeSkipSize);
            this.Controls.Add(this.txbInspName);
            this.Controls.Add(this.cbxInspEnabled);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormDAVSInsp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DAVS Insp";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTextureInsp_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.nudInspEdgeSkipHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspEdgeSkipSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImageIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeExtWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeExtHeight)).EndInit();
            this.panelDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudFixSizeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFixSizeHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txbInspName;
        private System.Windows.Forms.Label labInspEdgeSkipSize;
        private System.Windows.Forms.NumericUpDown nudInspEdgeSkipHeight;
        private System.Windows.Forms.NumericUpDown nudInspEdgeSkipSize;
        private System.Windows.Forms.CheckBox cbxInspEnabled;
        private System.Windows.Forms.Button btnSetupName;
        private System.Windows.Forms.Label labInspImgIndex;
        private System.Windows.Forms.NumericUpDown nudImageIndex;
        private System.Windows.Forms.ComboBox comboBoxRegionSelect;
        private System.Windows.Forms.Label labRegionIndex;
        private System.Windows.Forms.NumericUpDown nudEdgeExtWidth;
        private System.Windows.Forms.NumericUpDown nudEdgeExtHeight;
        private System.Windows.Forms.Label labEdgeExtSize;
        private HalconDotNet.HSmartWindowControl HWindowDisplay;
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.Label labInspType;
        private System.Windows.Forms.ComboBox comboBoxInspType;
        private System.Windows.Forms.Label labPredictionMode;
        private System.Windows.Forms.ComboBox comboBoxPredictionMode;
        private System.Windows.Forms.Label labPath;
        private System.Windows.Forms.TextBox txbImgHSDirPath;
        private System.Windows.Forms.TextBox txbModelPath;
        private System.Windows.Forms.Label labModePath;
        private System.Windows.Forms.Button btnSegTest;
        private System.Windows.Forms.Button btnInspTest;
        private System.Windows.Forms.CheckedListBox cbxListSaveImg;
        private System.Windows.Forms.Label labNum;
        private System.Windows.Forms.Label labSaveImg;
        private System.Windows.Forms.Label labClassNum;
        private System.Windows.Forms.CheckBox cbxbFixSize;
        private System.Windows.Forms.NumericUpDown nudFixSizeWidth;
        private System.Windows.Forms.NumericUpDown nudFixSizeHeight;
        private System.Windows.Forms.Label labSaveImagePath;
        private System.Windows.Forms.TextBox txbSaveImagePath;
        private System.Windows.Forms.ComboBox comboBoxMultiTHBand;
        private System.Windows.Forms.Label labMultiTHBand;
        private System.Windows.Forms.ComboBox comboBox_ImageSource;
    }
}