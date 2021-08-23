namespace DeltaSubstrateInspector.src.MainPanels
{
    partial class InspectionView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chip_id_col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chip_process_col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chip_result_col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.good_col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ng_col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnl_insp_model = new System.Windows.Forms.Panel();
            this.hWindowControl_cam = new HalconDotNet.HWindowControl();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_save = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox_InspectMap = new System.Windows.Forms.PictureBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.button_BigMap = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.labelNumberOfInspectionNG = new System.Windows.Forms.Label();
            this.labelNumberOfInspectionOK = new System.Windows.Forms.Label();
            this.labelNumberOfInspection = new System.Windows.Forms.Label();
            this.label_NumberOfInspection = new System.Windows.Forms.Label();
            this.label_NumberOfInspectionNG = new System.Windows.Forms.Label();
            this.label_NumberOfInspectionOK = new System.Windows.Forms.Label();
            this.panel_Static = new System.Windows.Forms.Panel();
            this.labelNumberOfInspectionNGChip = new System.Windows.Forms.Label();
            this.label_NumberOfNCChipCount = new System.Windows.Forms.Label();
            this.button_Reset = new System.Windows.Forms.Button();
            this.panel_CaptureImageInfo = new System.Windows.Forms.Panel();
            this.label_CaptureCount = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox_InspectView = new System.Windows.Forms.PictureBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.hSmartWindowControl_InspectView = new HalconDotNet.HSmartWindowControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_InspectMap)).BeginInit();
            this.panel_Static.SuspendLayout();
            this.panel_CaptureImageInfo.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_InspectView)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(87, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chip_id_col,
            this.chip_process_col,
            this.chip_result_col,
            this.good_col,
            this.ng_col,
            this.col_1,
            this.col_2,
            this.col_3,
            this.col_4,
            this.col_5});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(19, 718);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowTemplate.Height = 31;
            this.dataGridView1.Size = new System.Drawing.Size(967, 269);
            this.dataGridView1.TabIndex = 4;
            // 
            // chip_id_col
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.chip_id_col.DefaultCellStyle = dataGridViewCellStyle2;
            this.chip_id_col.DividerWidth = 1;
            this.chip_id_col.HeaderText = "序號";
            this.chip_id_col.Name = "chip_id_col";
            this.chip_id_col.Width = 150;
            // 
            // chip_process_col
            // 
            this.chip_process_col.DividerWidth = 1;
            this.chip_process_col.HeaderText = "運行結果";
            this.chip_process_col.Name = "chip_process_col";
            this.chip_process_col.Width = 125;
            // 
            // chip_result_col
            // 
            this.chip_result_col.DividerWidth = 1;
            this.chip_result_col.HeaderText = "判定結果";
            this.chip_result_col.Name = "chip_result_col";
            this.chip_result_col.Width = 125;
            // 
            // good_col
            // 
            this.good_col.DividerWidth = 1;
            this.good_col.HeaderText = "良率 (%)";
            this.good_col.Name = "good_col";
            this.good_col.Width = 125;
            // 
            // ng_col
            // 
            this.ng_col.DividerWidth = 1;
            this.ng_col.HeaderText = "NG 顆數";
            this.ng_col.Name = "ng_col";
            this.ng_col.Width = 125;
            // 
            // col_1
            // 
            this.col_1.DividerWidth = 1;
            this.col_1.HeaderText = "";
            this.col_1.Name = "col_1";
            this.col_1.Width = 300;
            // 
            // col_2
            // 
            this.col_2.DividerWidth = 1;
            this.col_2.HeaderText = "";
            this.col_2.Name = "col_2";
            this.col_2.Width = 150;
            // 
            // col_3
            // 
            this.col_3.DividerWidth = 1;
            this.col_3.HeaderText = "";
            this.col_3.Name = "col_3";
            this.col_3.Width = 150;
            // 
            // col_4
            // 
            this.col_4.DividerWidth = 1;
            this.col_4.HeaderText = "";
            this.col_4.Name = "col_4";
            this.col_4.Width = 150;
            // 
            // col_5
            // 
            this.col_5.DividerWidth = 1;
            this.col_5.HeaderText = "";
            this.col_5.Name = "col_5";
            this.col_5.Width = 150;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pnl_insp_model);
            this.panel1.Location = new System.Drawing.Point(19, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(322, 637);
            this.panel1.TabIndex = 5;
            // 
            // pnl_insp_model
            // 
            this.pnl_insp_model.AutoScroll = true;
            this.pnl_insp_model.Location = new System.Drawing.Point(12, 34);
            this.pnl_insp_model.Name = "pnl_insp_model";
            this.pnl_insp_model.Size = new System.Drawing.Size(299, 591);
            this.pnl_insp_model.TabIndex = 2;
            // 
            // hWindowControl_cam
            // 
            this.hWindowControl_cam.BackColor = System.Drawing.Color.Black;
            this.hWindowControl_cam.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl_cam.ImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_cam.Location = new System.Drawing.Point(1261, 54);
            this.hWindowControl_cam.Name = "hWindowControl_cam";
            this.hWindowControl_cam.Size = new System.Drawing.Size(545, 299);
            this.hWindowControl_cam.TabIndex = 7;
            this.hWindowControl_cam.WindowSize = new System.Drawing.Size(545, 299);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 103);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(7, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 24);
            this.label1.TabIndex = 11;
            this.label1.Text = "儲存";
            // 
            // lbl_save
            // 
            this.lbl_save.AutoSize = true;
            this.lbl_save.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_save.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_save.Location = new System.Drawing.Point(122, 39);
            this.lbl_save.Name = "lbl_save";
            this.lbl_save.Size = new System.Drawing.Size(33, 18);
            this.lbl_save.TabIndex = 12;
            this.lbl_save.Text = "OFF";
            this.lbl_save.Click += new System.EventHandler(this.lbl_save_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.pictureBox_InspectMap);
            this.panel5.Controls.Add(this.lbl_save);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.btn_save);
            this.panel5.Controls.Add(this.button_BigMap);
            this.panel5.Controls.Add(this.button8);
            this.panel5.Location = new System.Drawing.Point(1261, 429);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(545, 562);
            this.panel5.TabIndex = 7;
            // 
            // pictureBox_InspectMap
            // 
            this.pictureBox_InspectMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pictureBox_InspectMap.Location = new System.Drawing.Point(3, 65);
            this.pictureBox_InspectMap.Name = "pictureBox_InspectMap";
            this.pictureBox_InspectMap.Size = new System.Drawing.Size(537, 492);
            this.pictureBox_InspectMap.TabIndex = 1;
            this.pictureBox_InspectMap.TabStop = false;
            // 
            // btn_save
            // 
            this.btn_save.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.OFF_edited;
            this.btn_save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_save.FlatAppearance.BorderSize = 0;
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Location = new System.Drawing.Point(60, 32);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(62, 31);
            this.btn_save.TabIndex = 10;
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_BigMap
            // 
            this.button_BigMap.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.inspection;
            this.button_BigMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_BigMap.FlatAppearance.BorderSize = 0;
            this.button_BigMap.Location = new System.Drawing.Point(495, 25);
            this.button_BigMap.Name = "button_BigMap";
            this.button_BigMap.Size = new System.Drawing.Size(44, 38);
            this.button_BigMap.TabIndex = 8;
            this.button_BigMap.UseVisualStyleBackColor = true;
            this.button_BigMap.Click += new System.EventHandler(this.BigMap_Click);
            // 
            // button8
            // 
            this.button8.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.settings;
            this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.Location = new System.Drawing.Point(447, 25);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(44, 38);
            this.button8.TabIndex = 9;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // labelNumberOfInspectionNG
            // 
            this.labelNumberOfInspectionNG.BackColor = System.Drawing.SystemColors.Window;
            this.labelNumberOfInspectionNG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelNumberOfInspectionNG.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfInspectionNG.Location = new System.Drawing.Point(102, 129);
            this.labelNumberOfInspectionNG.Name = "labelNumberOfInspectionNG";
            this.labelNumberOfInspectionNG.Size = new System.Drawing.Size(144, 27);
            this.labelNumberOfInspectionNG.TabIndex = 23;
            this.labelNumberOfInspectionNG.Text = "0";
            this.labelNumberOfInspectionNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNumberOfInspectionOK
            // 
            this.labelNumberOfInspectionOK.BackColor = System.Drawing.SystemColors.Window;
            this.labelNumberOfInspectionOK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelNumberOfInspectionOK.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfInspectionOK.Location = new System.Drawing.Point(102, 96);
            this.labelNumberOfInspectionOK.Name = "labelNumberOfInspectionOK";
            this.labelNumberOfInspectionOK.Size = new System.Drawing.Size(144, 27);
            this.labelNumberOfInspectionOK.TabIndex = 22;
            this.labelNumberOfInspectionOK.Text = "0";
            this.labelNumberOfInspectionOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNumberOfInspection
            // 
            this.labelNumberOfInspection.BackColor = System.Drawing.SystemColors.Window;
            this.labelNumberOfInspection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelNumberOfInspection.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfInspection.Location = new System.Drawing.Point(102, 64);
            this.labelNumberOfInspection.Name = "labelNumberOfInspection";
            this.labelNumberOfInspection.Size = new System.Drawing.Size(144, 27);
            this.labelNumberOfInspection.TabIndex = 21;
            this.labelNumberOfInspection.Text = "0";
            this.labelNumberOfInspection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_NumberOfInspection
            // 
            this.label_NumberOfInspection.AutoSize = true;
            this.label_NumberOfInspection.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NumberOfInspection.Location = new System.Drawing.Point(5, 67);
            this.label_NumberOfInspection.Name = "label_NumberOfInspection";
            this.label_NumberOfInspection.Size = new System.Drawing.Size(67, 24);
            this.label_NumberOfInspection.TabIndex = 20;
            this.label_NumberOfInspection.Text = "總片數";
            // 
            // label_NumberOfInspectionNG
            // 
            this.label_NumberOfInspectionNG.AutoSize = true;
            this.label_NumberOfInspectionNG.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NumberOfInspectionNG.ForeColor = System.Drawing.Color.Red;
            this.label_NumberOfInspectionNG.Location = new System.Drawing.Point(5, 133);
            this.label_NumberOfInspectionNG.Name = "label_NumberOfInspectionNG";
            this.label_NumberOfInspectionNG.Size = new System.Drawing.Size(77, 24);
            this.label_NumberOfInspectionNG.TabIndex = 19;
            this.label_NumberOfInspectionNG.Text = "NG片數";
            // 
            // label_NumberOfInspectionOK
            // 
            this.label_NumberOfInspectionOK.AutoSize = true;
            this.label_NumberOfInspectionOK.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NumberOfInspectionOK.ForeColor = System.Drawing.Color.Green;
            this.label_NumberOfInspectionOK.Location = new System.Drawing.Point(5, 100);
            this.label_NumberOfInspectionOK.Name = "label_NumberOfInspectionOK";
            this.label_NumberOfInspectionOK.Size = new System.Drawing.Size(75, 24);
            this.label_NumberOfInspectionOK.TabIndex = 18;
            this.label_NumberOfInspectionOK.Text = "OK片數";
            // 
            // panel_Static
            // 
            this.panel_Static.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Static.Controls.Add(this.labelNumberOfInspectionNGChip);
            this.panel_Static.Controls.Add(this.label_NumberOfNCChipCount);
            this.panel_Static.Controls.Add(this.button_Reset);
            this.panel_Static.Controls.Add(this.labelNumberOfInspection);
            this.panel_Static.Controls.Add(this.labelNumberOfInspectionNG);
            this.panel_Static.Controls.Add(this.label_NumberOfInspectionOK);
            this.panel_Static.Controls.Add(this.labelNumberOfInspectionOK);
            this.panel_Static.Controls.Add(this.label_NumberOfInspectionNG);
            this.panel_Static.Controls.Add(this.label_NumberOfInspection);
            this.panel_Static.Location = new System.Drawing.Point(992, 724);
            this.panel_Static.Name = "panel_Static";
            this.panel_Static.Size = new System.Drawing.Size(251, 263);
            this.panel_Static.TabIndex = 24;
            // 
            // labelNumberOfInspectionNGChip
            // 
            this.labelNumberOfInspectionNGChip.BackColor = System.Drawing.SystemColors.Window;
            this.labelNumberOfInspectionNGChip.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelNumberOfInspectionNGChip.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfInspectionNGChip.Location = new System.Drawing.Point(102, 169);
            this.labelNumberOfInspectionNGChip.Name = "labelNumberOfInspectionNGChip";
            this.labelNumberOfInspectionNGChip.Size = new System.Drawing.Size(144, 27);
            this.labelNumberOfInspectionNGChip.TabIndex = 26;
            this.labelNumberOfInspectionNGChip.Text = "0";
            this.labelNumberOfInspectionNGChip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_NumberOfNCChipCount
            // 
            this.label_NumberOfNCChipCount.AutoSize = true;
            this.label_NumberOfNCChipCount.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NumberOfNCChipCount.ForeColor = System.Drawing.Color.Brown;
            this.label_NumberOfNCChipCount.Location = new System.Drawing.Point(5, 173);
            this.label_NumberOfNCChipCount.Name = "label_NumberOfNCChipCount";
            this.label_NumberOfNCChipCount.Size = new System.Drawing.Size(96, 24);
            this.label_NumberOfNCChipCount.TabIndex = 25;
            this.label_NumberOfNCChipCount.Text = "NG總顆數";
            // 
            // button_Reset
            // 
            this.button_Reset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_Reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Reset.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Reset.ForeColor = System.Drawing.Color.White;
            this.button_Reset.Location = new System.Drawing.Point(83, 3);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(163, 49);
            this.button_Reset.TabIndex = 24;
            this.button_Reset.Text = "重置";
            this.button_Reset.UseVisualStyleBackColor = false;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // panel_CaptureImageInfo
            // 
            this.panel_CaptureImageInfo.BackColor = System.Drawing.Color.Gray;
            this.panel_CaptureImageInfo.Controls.Add(this.label_CaptureCount);
            this.panel_CaptureImageInfo.Location = new System.Drawing.Point(1261, 353);
            this.panel_CaptureImageInfo.Name = "panel_CaptureImageInfo";
            this.panel_CaptureImageInfo.Size = new System.Drawing.Size(545, 49);
            this.panel_CaptureImageInfo.TabIndex = 25;
            // 
            // label_CaptureCount
            // 
            this.label_CaptureCount.AutoSize = true;
            this.label_CaptureCount.BackColor = System.Drawing.Color.Transparent;
            this.label_CaptureCount.Font = new System.Drawing.Font("Ebrima", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CaptureCount.ForeColor = System.Drawing.Color.White;
            this.label_CaptureCount.Location = new System.Drawing.Point(4, 14);
            this.label_CaptureCount.Name = "label_CaptureCount";
            this.label_CaptureCount.Size = new System.Drawing.Size(55, 21);
            this.label_CaptureCount.TabIndex = 2;
            this.label_CaptureCount.Text = "Count:";
            // 
            // panel7
            // 
            this.panel7.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.bar2;
            this.panel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label7);
            this.panel7.Controls.Add(this.label11);
            this.panel7.Location = new System.Drawing.Point(19, 16);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(322, 40);
            this.panel7.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Ebrima", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.label7.Location = new System.Drawing.Point(14, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "INSPECT MODEL";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.label11.Location = new System.Drawing.Point(143, 10);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 24);
            this.label11.TabIndex = 0;
            this.label11.Text = "檢測模組";
            // 
            // panel9
            // 
            this.panel9.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.bar2;
            this.panel9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.label14);
            this.panel9.Location = new System.Drawing.Point(357, 16);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(886, 40);
            this.panel9.TabIndex = 17;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Ebrima", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.ForeColor = System.Drawing.Color.DimGray;
            this.label14.Location = new System.Drawing.Point(14, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 21);
            this.label14.TabIndex = 1;
            this.label14.Text = "INSPECT VIEW";
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.bar2;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label12);
            this.panel3.Location = new System.Drawing.Point(19, 680);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1224, 40);
            this.panel3.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Ebrima", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.ForeColor = System.Drawing.Color.DimGray;
            this.label12.Location = new System.Drawing.Point(8, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 21);
            this.label12.TabIndex = 1;
            this.label12.Text = "STATIC INFO";
            // 
            // pictureBox_InspectView
            // 
            this.pictureBox_InspectView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pictureBox_InspectView.Location = new System.Drawing.Point(357, 52);
            this.pictureBox_InspectView.Name = "pictureBox_InspectView";
            this.pictureBox_InspectView.Size = new System.Drawing.Size(886, 616);
            this.pictureBox_InspectView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_InspectView.TabIndex = 0;
            this.pictureBox_InspectView.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.bar2;
            this.panel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.label2);
            this.panel8.Location = new System.Drawing.Point(1261, 16);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(545, 40);
            this.panel8.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Ebrima", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(11, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Capture Image";
            // 
            // panel6
            // 
            this.panel6.BackgroundImage = global::DeltaSubstrateInspector.Properties.Resources.bar2;
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label8);
            this.panel6.Location = new System.Drawing.Point(1261, 414);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(545, 40);
            this.panel6.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Ebrima", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(14, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 21);
            this.label8.TabIndex = 2;
            this.label8.Text = "INSPECT MAP";
            // 
            // hSmartWindowControl_InspectView
            // 
            this.hSmartWindowControl_InspectView.AllowDrop = true;
            this.hSmartWindowControl_InspectView.AutoScroll = true;
            this.hSmartWindowControl_InspectView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hSmartWindowControl_InspectView.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hSmartWindowControl_InspectView.BackColor = System.Drawing.SystemColors.HotTrack;
            this.hSmartWindowControl_InspectView.HDoubleClickToFitContent = true;
            this.hSmartWindowControl_InspectView.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hSmartWindowControl_InspectView.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hSmartWindowControl_InspectView.HKeepAspectRatio = false;
            this.hSmartWindowControl_InspectView.HMoveContent = true;
            this.hSmartWindowControl_InspectView.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hSmartWindowControl_InspectView.Location = new System.Drawing.Point(357, 54);
            this.hSmartWindowControl_InspectView.Margin = new System.Windows.Forms.Padding(0);
            this.hSmartWindowControl_InspectView.Name = "hSmartWindowControl_InspectView";
            this.hSmartWindowControl_InspectView.Size = new System.Drawing.Size(886, 614);
            this.hSmartWindowControl_InspectView.TabIndex = 26;
            this.hSmartWindowControl_InspectView.WindowSize = new System.Drawing.Size(886, 614);
            // 
            // InspectionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.panel_CaptureImageInfo);
            this.Controls.Add(this.panel_Static);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.hWindowControl_cam);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.hSmartWindowControl_InspectView);
            this.Controls.Add(this.pictureBox_InspectView);
            this.Name = "InspectionView";
            this.Size = new System.Drawing.Size(1819, 996);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_InspectMap)).EndInit();
            this.panel_Static.ResumeLayout(false);
            this.panel_Static.PerformLayout();
            this.panel_CaptureImageInfo.ResumeLayout(false);
            this.panel_CaptureImageInfo.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_InspectView)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_InspectView;
        private System.Windows.Forms.PictureBox pictureBox_InspectMap;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnl_insp_model;
        public HalconDotNet.HWindowControl hWindowControl_cam;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button_BigMap;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_save;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelNumberOfInspectionNG;
        private System.Windows.Forms.Label labelNumberOfInspectionOK;
        private System.Windows.Forms.Label labelNumberOfInspection;
        private System.Windows.Forms.Label label_NumberOfInspection;
        private System.Windows.Forms.Label label_NumberOfInspectionNG;
        private System.Windows.Forms.Label label_NumberOfInspectionOK;
        private System.Windows.Forms.Panel panel_Static;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Panel panel_CaptureImageInfo;
        private System.Windows.Forms.Label label_CaptureCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn chip_id_col;
        private System.Windows.Forms.DataGridViewTextBoxColumn chip_process_col;
        private System.Windows.Forms.DataGridViewTextBoxColumn chip_result_col;
        private System.Windows.Forms.DataGridViewTextBoxColumn good_col;
        private System.Windows.Forms.DataGridViewTextBoxColumn ng_col;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_3;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_4;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_5;
        private System.Windows.Forms.Label labelNumberOfInspectionNGChip;
        private System.Windows.Forms.Label label_NumberOfNCChipCount;
        private HalconDotNet.HSmartWindowControl hSmartWindowControl_InspectView;
    }
}
