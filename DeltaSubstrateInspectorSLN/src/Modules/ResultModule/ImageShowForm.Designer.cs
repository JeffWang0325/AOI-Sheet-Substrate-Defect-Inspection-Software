namespace DeltaSubstrateInspector.src.Modules.ResultModule
{
    partial class ImageShowForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView_UI2Dindex = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.rbtn_defect = new System.Windows.Forms.RadioButton();
            this.rbtn_origin = new System.Windows.Forms.RadioButton();
            this.comboBox_LightImage = new System.Windows.Forms.ComboBox();
            this.hWindowControl_InspectResultImage = new HalconDotNet.HSmartWindowControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_General = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox_ImageShowForm = new System.Windows.Forms.PictureBox();
            this.hSmartWindowControl_ImageShowForm = new HalconDotNet.HSmartWindowControl();
            this.tabPage_Adv = new System.Windows.Forms.TabPage();
            this.txt_GrayValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_CursorCoordinate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_InspectResult = new System.Windows.Forms.Panel();
            this.tabPage_Recheck = new System.Windows.Forms.TabPage();
            this.label75 = new System.Windows.Forms.Label();
            this.textBox_Path_DefectTest = new System.Windows.Forms.TextBox();
            this.label74 = new System.Windows.Forms.Label();
            this.radioButton_Recheck = new System.Windows.Forms.RadioButton();
            this.radioButton_Result = new System.Windows.Forms.RadioButton();
            this.button_SaveAs_DefectTest = new System.Windows.Forms.Button();
            this.button_Save_DefectTest = new System.Windows.Forms.Button();
            this.cbx_Priority = new System.Windows.Forms.CheckBox();
            this.cbx_NG_classification = new System.Windows.Forms.CheckBox();
            this.groupBox_Recheck = new System.Windows.Forms.GroupBox();
            this.txt_cellID = new System.Windows.Forms.TextBox();
            this.label_cellID = new System.Windows.Forms.Label();
            this.radioButton_MultiCells = new System.Windows.Forms.RadioButton();
            this.radioButton_SingleCell = new System.Windows.Forms.RadioButton();
            this.cbx_Recheck = new System.Windows.Forms.CheckBox();
            this.cbx_Do_Recheck = new System.Windows.Forms.CheckBox();
            this.button_Clear_Recheck = new System.Windows.Forms.Button();
            this.groupBox_DefectsClassify = new System.Windows.Forms.GroupBox();
            this.btn_Edit_DefectsClassify = new System.Windows.Forms.Button();
            this.listView_Edit = new System.Windows.Forms.ListView();
            this.columnHeader_Key = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Color = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox_DefectsResult = new System.Windows.Forms.GroupBox();
            this.listView_Result = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBox_orig = new System.Windows.Forms.CheckBox();
            this.cbx_LightImage_Recheck = new System.Windows.Forms.ComboBox();
            this.label_Color = new System.Windows.Forms.Label();
            this.button_SetColor_Recheck = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.comboBox_DefectType = new System.Windows.Forms.ComboBox();
            this.hSmartWindowControl_Recheck = new HalconDotNet.HSmartWindowControl();
            this.btn_SaveImageList = new System.Windows.Forms.Button();
            this.button_ShowParaForm = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip_DispID = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_ID = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_Recheck = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_OK = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UI2Dindex)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage_General.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageShowForm)).BeginInit();
            this.tabPage_Adv.SuspendLayout();
            this.tabPage_Recheck.SuspendLayout();
            this.groupBox_Recheck.SuspendLayout();
            this.groupBox_DefectsClassify.SuspendLayout();
            this.groupBox_DefectsResult.SuspendLayout();
            this.contextMenuStrip_DispID.SuspendLayout();
            this.contextMenuStrip_Recheck.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dataGridView_UI2Dindex);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1246, 239);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(115, 529);
            this.panel1.TabIndex = 1;
            this.panel1.Visible = false;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dataGridView_UI2Dindex
            // 
            this.dataGridView_UI2Dindex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_UI2Dindex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_UI2Dindex.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_UI2Dindex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView_UI2Dindex.Name = "dataGridView_UI2Dindex";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_UI2Dindex.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_UI2Dindex.RowHeadersVisible = false;
            this.dataGridView_UI2Dindex.RowTemplate.Height = 24;
            this.dataGridView_UI2Dindex.Size = new System.Drawing.Size(115, 574);
            this.dataGridView_UI2Dindex.TabIndex = 31;
            this.dataGridView_UI2Dindex.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "瑕疵位置";
            // 
            // rbtn_defect
            // 
            this.rbtn_defect.AutoSize = true;
            this.rbtn_defect.Checked = true;
            this.rbtn_defect.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn_defect.ForeColor = System.Drawing.Color.White;
            this.rbtn_defect.Location = new System.Drawing.Point(6, 7);
            this.rbtn_defect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtn_defect.Name = "rbtn_defect";
            this.rbtn_defect.Size = new System.Drawing.Size(85, 28);
            this.rbtn_defect.TabIndex = 2;
            this.rbtn_defect.TabStop = true;
            this.rbtn_defect.Text = "瑕疵圖";
            this.rbtn_defect.UseVisualStyleBackColor = true;
            this.rbtn_defect.CheckedChanged += new System.EventHandler(this.Change_image);
            // 
            // rbtn_origin
            // 
            this.rbtn_origin.AutoSize = true;
            this.rbtn_origin.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtn_origin.ForeColor = System.Drawing.Color.White;
            this.rbtn_origin.Location = new System.Drawing.Point(132, 7);
            this.rbtn_origin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtn_origin.Name = "rbtn_origin";
            this.rbtn_origin.Size = new System.Drawing.Size(66, 28);
            this.rbtn_origin.TabIndex = 3;
            this.rbtn_origin.Text = "原圖";
            this.rbtn_origin.UseVisualStyleBackColor = true;
            this.rbtn_origin.CheckedChanged += new System.EventHandler(this.Change_image);
            // 
            // comboBox_LightImage
            // 
            this.comboBox_LightImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.comboBox_LightImage.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_LightImage.ForeColor = System.Drawing.Color.White;
            this.comboBox_LightImage.FormattingEnabled = true;
            this.comboBox_LightImage.Location = new System.Drawing.Point(1012, 6);
            this.comboBox_LightImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_LightImage.Name = "comboBox_LightImage";
            this.comboBox_LightImage.Size = new System.Drawing.Size(210, 32);
            this.comboBox_LightImage.TabIndex = 5;
            this.comboBox_LightImage.Visible = false;
            this.comboBox_LightImage.SelectedIndexChanged += new System.EventHandler(this.comboBox_LightImage_SelectedIndexChanged);
            // 
            // hWindowControl_InspectResultImage
            // 
            this.hWindowControl_InspectResultImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hWindowControl_InspectResultImage.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hWindowControl_InspectResultImage.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_InspectResultImage.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.hWindowControl_InspectResultImage.HDoubleClickToFitContent = true;
            this.hWindowControl_InspectResultImage.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hWindowControl_InspectResultImage.HImagePart = new System.Drawing.Rectangle(0, 0, 4096, 2160);
            this.hWindowControl_InspectResultImage.HKeepAspectRatio = false;
            this.hWindowControl_InspectResultImage.HMoveContent = true;
            this.hWindowControl_InspectResultImage.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hWindowControl_InspectResultImage.Location = new System.Drawing.Point(3, 7);
            this.hWindowControl_InspectResultImage.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl_InspectResultImage.Name = "hWindowControl_InspectResultImage";
            this.hWindowControl_InspectResultImage.Size = new System.Drawing.Size(948, 752);
            this.hWindowControl_InspectResultImage.TabIndex = 71;
            this.hWindowControl_InspectResultImage.WindowSize = new System.Drawing.Size(948, 752);
            this.hWindowControl_InspectResultImage.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl_InspectResultImage_HMouseMove);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage_General);
            this.tabControl1.Controls.Add(this.tabPage_Adv);
            this.tabControl1.Controls.Add(this.tabPage_Recheck);
            this.tabControl1.Location = new System.Drawing.Point(2, 13);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1239, 839);
            this.tabControl1.TabIndex = 72;
            // 
            // tabPage_General
            // 
            this.tabPage_General.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
            this.tabPage_General.Controls.Add(this.panel2);
            this.tabPage_General.Controls.Add(this.comboBox_LightImage);
            this.tabPage_General.Controls.Add(this.rbtn_origin);
            this.tabPage_General.Controls.Add(this.rbtn_defect);
            this.tabPage_General.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage_General.Location = new System.Drawing.Point(4, 22);
            this.tabPage_General.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_General.Name = "tabPage_General";
            this.tabPage_General.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_General.Size = new System.Drawing.Size(1231, 813);
            this.tabPage_General.TabIndex = 0;
            this.tabPage_General.Text = "一般";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox_ImageShowForm);
            this.panel2.Controls.Add(this.hSmartWindowControl_ImageShowForm);
            this.panel2.Location = new System.Drawing.Point(3, 43);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1222, 765);
            this.panel2.TabIndex = 6;
            // 
            // pictureBox_ImageShowForm
            // 
            this.pictureBox_ImageShowForm.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox_ImageShowForm.Location = new System.Drawing.Point(3, 7);
            this.pictureBox_ImageShowForm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox_ImageShowForm.Name = "pictureBox_ImageShowForm";
            this.pictureBox_ImageShowForm.Size = new System.Drawing.Size(1216, 755);
            this.pictureBox_ImageShowForm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_ImageShowForm.TabIndex = 0;
            this.pictureBox_ImageShowForm.TabStop = false;
            this.pictureBox_ImageShowForm.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            this.pictureBox_ImageShowForm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox_ImageShowForm.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox_ImageShowForm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            this.pictureBox_ImageShowForm.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // hSmartWindowControl_ImageShowForm
            // 
            this.hSmartWindowControl_ImageShowForm.AllowDrop = true;
            this.hSmartWindowControl_ImageShowForm.AutoScroll = true;
            this.hSmartWindowControl_ImageShowForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hSmartWindowControl_ImageShowForm.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hSmartWindowControl_ImageShowForm.BackColor = System.Drawing.SystemColors.HotTrack;
            this.hSmartWindowControl_ImageShowForm.HDoubleClickToFitContent = true;
            this.hSmartWindowControl_ImageShowForm.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hSmartWindowControl_ImageShowForm.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hSmartWindowControl_ImageShowForm.HKeepAspectRatio = false;
            this.hSmartWindowControl_ImageShowForm.HMoveContent = true;
            this.hSmartWindowControl_ImageShowForm.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hSmartWindowControl_ImageShowForm.Location = new System.Drawing.Point(3, 7);
            this.hSmartWindowControl_ImageShowForm.Margin = new System.Windows.Forms.Padding(0);
            this.hSmartWindowControl_ImageShowForm.Name = "hSmartWindowControl_ImageShowForm";
            this.hSmartWindowControl_ImageShowForm.Size = new System.Drawing.Size(1216, 755);
            this.hSmartWindowControl_ImageShowForm.TabIndex = 27;
            this.hSmartWindowControl_ImageShowForm.WindowSize = new System.Drawing.Size(1216, 755);
            // 
            // tabPage_Adv
            // 
            this.tabPage_Adv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
            this.tabPage_Adv.Controls.Add(this.txt_GrayValue);
            this.tabPage_Adv.Controls.Add(this.label4);
            this.tabPage_Adv.Controls.Add(this.txt_CursorCoordinate);
            this.tabPage_Adv.Controls.Add(this.label2);
            this.tabPage_Adv.Controls.Add(this.panel_InspectResult);
            this.tabPage_Adv.Controls.Add(this.hWindowControl_InspectResultImage);
            this.tabPage_Adv.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage_Adv.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Adv.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Adv.Name = "tabPage_Adv";
            this.tabPage_Adv.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Adv.Size = new System.Drawing.Size(1231, 813);
            this.tabPage_Adv.TabIndex = 1;
            this.tabPage_Adv.Text = "進階";
            // 
            // txt_GrayValue
            // 
            this.txt_GrayValue.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_GrayValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_GrayValue.Location = new System.Drawing.Point(713, 768);
            this.txt_GrayValue.Name = "txt_GrayValue";
            this.txt_GrayValue.ReadOnly = true;
            this.txt_GrayValue.Size = new System.Drawing.Size(186, 29);
            this.txt_GrayValue.TabIndex = 76;
            this.txt_GrayValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(646, 771);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 75;
            this.label4.Text = "灰階值:";
            // 
            // txt_CursorCoordinate
            // 
            this.txt_CursorCoordinate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_CursorCoordinate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_CursorCoordinate.Location = new System.Drawing.Point(435, 768);
            this.txt_CursorCoordinate.Name = "txt_CursorCoordinate";
            this.txt_CursorCoordinate.ReadOnly = true;
            this.txt_CursorCoordinate.Size = new System.Drawing.Size(186, 29);
            this.txt_CursorCoordinate.TabIndex = 74;
            this.txt_CursorCoordinate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(282, 771);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 20);
            this.label2.TabIndex = 73;
            this.label2.Text = "滑鼠影像座標 (x, y):";
            // 
            // panel_InspectResult
            // 
            this.panel_InspectResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel_InspectResult.AutoScroll = true;
            this.panel_InspectResult.Location = new System.Drawing.Point(954, 7);
            this.panel_InspectResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_InspectResult.Name = "panel_InspectResult";
            this.panel_InspectResult.Size = new System.Drawing.Size(274, 802);
            this.panel_InspectResult.TabIndex = 72;
            // 
            // tabPage_Recheck
            // 
            this.tabPage_Recheck.Controls.Add(this.label75);
            this.tabPage_Recheck.Controls.Add(this.textBox_Path_DefectTest);
            this.tabPage_Recheck.Controls.Add(this.label74);
            this.tabPage_Recheck.Controls.Add(this.radioButton_Recheck);
            this.tabPage_Recheck.Controls.Add(this.radioButton_Result);
            this.tabPage_Recheck.Controls.Add(this.button_SaveAs_DefectTest);
            this.tabPage_Recheck.Controls.Add(this.button_Save_DefectTest);
            this.tabPage_Recheck.Controls.Add(this.cbx_Priority);
            this.tabPage_Recheck.Controls.Add(this.cbx_NG_classification);
            this.tabPage_Recheck.Controls.Add(this.groupBox_Recheck);
            this.tabPage_Recheck.Controls.Add(this.groupBox_DefectsClassify);
            this.tabPage_Recheck.Controls.Add(this.groupBox_DefectsResult);
            this.tabPage_Recheck.Controls.Add(this.checkBox_orig);
            this.tabPage_Recheck.Controls.Add(this.cbx_LightImage_Recheck);
            this.tabPage_Recheck.Controls.Add(this.label_Color);
            this.tabPage_Recheck.Controls.Add(this.button_SetColor_Recheck);
            this.tabPage_Recheck.Controls.Add(this.label50);
            this.tabPage_Recheck.Controls.Add(this.comboBox_DefectType);
            this.tabPage_Recheck.Controls.Add(this.hSmartWindowControl_Recheck);
            this.tabPage_Recheck.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Recheck.Name = "tabPage_Recheck";
            this.tabPage_Recheck.Size = new System.Drawing.Size(1231, 813);
            this.tabPage_Recheck.TabIndex = 2;
            this.tabPage_Recheck.Text = "人工覆判與統計結果";
            this.tabPage_Recheck.UseVisualStyleBackColor = true;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.label75.ForeColor = System.Drawing.Color.Blue;
            this.label75.Location = new System.Drawing.Point(866, 42);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(46, 21);
            this.label75.TabIndex = 98;
            this.label75.Text = "路徑:";
            this.label75.Visible = false;
            // 
            // textBox_Path_DefectTest
            // 
            this.textBox_Path_DefectTest.BackColor = System.Drawing.Color.White;
            this.textBox_Path_DefectTest.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.textBox_Path_DefectTest.ForeColor = System.Drawing.Color.Blue;
            this.textBox_Path_DefectTest.Location = new System.Drawing.Point(912, 39);
            this.textBox_Path_DefectTest.Name = "textBox_Path_DefectTest";
            this.textBox_Path_DefectTest.ReadOnly = true;
            this.textBox_Path_DefectTest.Size = new System.Drawing.Size(312, 29);
            this.textBox_Path_DefectTest.TabIndex = 97;
            this.textBox_Path_DefectTest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Path_DefectTest.Visible = false;
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label74.ForeColor = System.Drawing.Color.YellowGreen;
            this.label74.Location = new System.Drawing.Point(877, 10);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(78, 21);
            this.label74.TabIndex = 94;
            this.label74.Text = "顯示資訊:";
            // 
            // radioButton_Recheck
            // 
            this.radioButton_Recheck.AutoSize = true;
            this.radioButton_Recheck.Enabled = false;
            this.radioButton_Recheck.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_Recheck.ForeColor = System.Drawing.Color.DarkViolet;
            this.radioButton_Recheck.Location = new System.Drawing.Point(1054, 11);
            this.radioButton_Recheck.Name = "radioButton_Recheck";
            this.radioButton_Recheck.Size = new System.Drawing.Size(123, 24);
            this.radioButton_Recheck.TabIndex = 96;
            this.radioButton_Recheck.Text = "人工覆判結果";
            this.radioButton_Recheck.UseVisualStyleBackColor = true;
            this.radioButton_Recheck.CheckedChanged += new System.EventHandler(this.radioButton_defTest_CheckedChanged);
            // 
            // radioButton_Result
            // 
            this.radioButton_Result.AutoSize = true;
            this.radioButton_Result.Checked = true;
            this.radioButton_Result.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_Result.ForeColor = System.Drawing.Color.Black;
            this.radioButton_Result.Location = new System.Drawing.Point(960, 11);
            this.radioButton_Result.Name = "radioButton_Result";
            this.radioButton_Result.Size = new System.Drawing.Size(91, 24);
            this.radioButton_Result.TabIndex = 95;
            this.radioButton_Result.TabStop = true;
            this.radioButton_Result.Text = "檢測結果";
            this.radioButton_Result.UseVisualStyleBackColor = true;
            this.radioButton_Result.CheckedChanged += new System.EventHandler(this.radioButton_defTest_CheckedChanged);
            // 
            // button_SaveAs_DefectTest
            // 
            this.button_SaveAs_DefectTest.BackColor = System.Drawing.Color.Blue;
            this.button_SaveAs_DefectTest.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_SaveAs_DefectTest.ForeColor = System.Drawing.Color.White;
            this.button_SaveAs_DefectTest.Location = new System.Drawing.Point(1173, 71);
            this.button_SaveAs_DefectTest.Name = "button_SaveAs_DefectTest";
            this.button_SaveAs_DefectTest.Size = new System.Drawing.Size(52, 39);
            this.button_SaveAs_DefectTest.TabIndex = 93;
            this.button_SaveAs_DefectTest.Text = "另存";
            this.button_SaveAs_DefectTest.UseVisualStyleBackColor = false;
            this.button_SaveAs_DefectTest.Visible = false;
            // 
            // button_Save_DefectTest
            // 
            this.button_Save_DefectTest.BackColor = System.Drawing.Color.Blue;
            this.button_Save_DefectTest.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Save_DefectTest.ForeColor = System.Drawing.Color.White;
            this.button_Save_DefectTest.Location = new System.Drawing.Point(1111, 71);
            this.button_Save_DefectTest.Name = "button_Save_DefectTest";
            this.button_Save_DefectTest.Size = new System.Drawing.Size(52, 39);
            this.button_Save_DefectTest.TabIndex = 92;
            this.button_Save_DefectTest.Text = "儲存";
            this.button_Save_DefectTest.UseVisualStyleBackColor = false;
            this.button_Save_DefectTest.Visible = false;
            // 
            // cbx_Priority
            // 
            this.cbx_Priority.AutoCheck = false;
            this.cbx_Priority.AutoSize = true;
            this.cbx_Priority.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbx_Priority.ForeColor = System.Drawing.Color.Black;
            this.cbx_Priority.Location = new System.Drawing.Point(967, 75);
            this.cbx_Priority.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_Priority.Name = "cbx_Priority";
            this.cbx_Priority.Size = new System.Drawing.Size(108, 24);
            this.cbx_Priority.TabIndex = 91;
            this.cbx_Priority.Tag = "瑕疵優先權";
            this.cbx_Priority.Text = "瑕疵優先權";
            this.cbx_Priority.UseVisualStyleBackColor = true;
            // 
            // cbx_NG_classification
            // 
            this.cbx_NG_classification.AutoCheck = false;
            this.cbx_NG_classification.AutoSize = true;
            this.cbx_NG_classification.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbx_NG_classification.ForeColor = System.Drawing.Color.Black;
            this.cbx_NG_classification.Location = new System.Drawing.Point(871, 75);
            this.cbx_NG_classification.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_NG_classification.Name = "cbx_NG_classification";
            this.cbx_NG_classification.Size = new System.Drawing.Size(92, 24);
            this.cbx_NG_classification.TabIndex = 90;
            this.cbx_NG_classification.Tag = "瑕疵分類";
            this.cbx_NG_classification.Text = "瑕疵分類";
            this.cbx_NG_classification.UseVisualStyleBackColor = true;
            // 
            // groupBox_Recheck
            // 
            this.groupBox_Recheck.Controls.Add(this.txt_cellID);
            this.groupBox_Recheck.Controls.Add(this.label_cellID);
            this.groupBox_Recheck.Controls.Add(this.radioButton_MultiCells);
            this.groupBox_Recheck.Controls.Add(this.radioButton_SingleCell);
            this.groupBox_Recheck.Controls.Add(this.cbx_Recheck);
            this.groupBox_Recheck.Controls.Add(this.cbx_Do_Recheck);
            this.groupBox_Recheck.Controls.Add(this.button_Clear_Recheck);
            this.groupBox_Recheck.Enabled = false;
            this.groupBox_Recheck.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox_Recheck.ForeColor = System.Drawing.Color.DarkViolet;
            this.groupBox_Recheck.Location = new System.Drawing.Point(871, 104);
            this.groupBox_Recheck.Name = "groupBox_Recheck";
            this.groupBox_Recheck.Size = new System.Drawing.Size(358, 108);
            this.groupBox_Recheck.TabIndex = 89;
            this.groupBox_Recheck.TabStop = false;
            this.groupBox_Recheck.Text = "人工覆判";
            // 
            // txt_cellID
            // 
            this.txt_cellID.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_cellID.ForeColor = System.Drawing.Color.Magenta;
            this.txt_cellID.Location = new System.Drawing.Point(6, 73);
            this.txt_cellID.Name = "txt_cellID";
            this.txt_cellID.ReadOnly = true;
            this.txt_cellID.Size = new System.Drawing.Size(173, 25);
            this.txt_cellID.TabIndex = 71;
            this.txt_cellID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_cellID
            // 
            this.label_cellID.AutoSize = true;
            this.label_cellID.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_cellID.ForeColor = System.Drawing.Color.Magenta;
            this.label_cellID.Location = new System.Drawing.Point(9, 53);
            this.label_cellID.Name = "label_cellID";
            this.label_cellID.Size = new System.Drawing.Size(170, 17);
            this.label_cellID.TabIndex = 81;
            this.label_cellID.Text = "滑鼠點擊左鍵Cell座標 (X, Y):";
            // 
            // radioButton_MultiCells
            // 
            this.radioButton_MultiCells.AutoSize = true;
            this.radioButton_MultiCells.Checked = true;
            this.radioButton_MultiCells.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_MultiCells.ForeColor = System.Drawing.Color.DarkViolet;
            this.radioButton_MultiCells.Location = new System.Drawing.Point(192, 54);
            this.radioButton_MultiCells.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton_MultiCells.Name = "radioButton_MultiCells";
            this.radioButton_MultiCells.Size = new System.Drawing.Size(123, 24);
            this.radioButton_MultiCells.TabIndex = 80;
            this.radioButton_MultiCells.TabStop = true;
            this.radioButton_MultiCells.Text = "多顆標註模式";
            this.radioButton_MultiCells.UseVisualStyleBackColor = true;
            // 
            // radioButton_SingleCell
            // 
            this.radioButton_SingleCell.AutoSize = true;
            this.radioButton_SingleCell.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_SingleCell.ForeColor = System.Drawing.Color.DarkViolet;
            this.radioButton_SingleCell.Location = new System.Drawing.Point(192, 79);
            this.radioButton_SingleCell.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton_SingleCell.Name = "radioButton_SingleCell";
            this.radioButton_SingleCell.Size = new System.Drawing.Size(123, 24);
            this.radioButton_SingleCell.TabIndex = 79;
            this.radioButton_SingleCell.Text = "單顆標註模式";
            this.radioButton_SingleCell.UseVisualStyleBackColor = true;
            // 
            // cbx_Recheck
            // 
            this.cbx_Recheck.AutoCheck = false;
            this.cbx_Recheck.AutoSize = true;
            this.cbx_Recheck.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbx_Recheck.ForeColor = System.Drawing.Color.DarkViolet;
            this.cbx_Recheck.Location = new System.Drawing.Point(10, 27);
            this.cbx_Recheck.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_Recheck.Name = "cbx_Recheck";
            this.cbx_Recheck.Size = new System.Drawing.Size(92, 24);
            this.cbx_Recheck.TabIndex = 72;
            this.cbx_Recheck.Tag = "人工覆判";
            this.cbx_Recheck.Text = "人工覆判";
            this.cbx_Recheck.UseVisualStyleBackColor = true;
            // 
            // cbx_Do_Recheck
            // 
            this.cbx_Do_Recheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbx_Do_Recheck.BackColor = System.Drawing.Color.DarkViolet;
            this.cbx_Do_Recheck.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.cbx_Do_Recheck.ForeColor = System.Drawing.Color.White;
            this.cbx_Do_Recheck.Location = new System.Drawing.Point(159, 14);
            this.cbx_Do_Recheck.MaximumSize = new System.Drawing.Size(9999, 9999);
            this.cbx_Do_Recheck.Name = "cbx_Do_Recheck";
            this.cbx_Do_Recheck.Size = new System.Drawing.Size(95, 40);
            this.cbx_Do_Recheck.TabIndex = 23;
            this.cbx_Do_Recheck.Text = "人工覆判";
            this.cbx_Do_Recheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbx_Do_Recheck.UseVisualStyleBackColor = false;
            this.cbx_Do_Recheck.CheckedChanged += new System.EventHandler(this.cbx_Do_Recheck_CheckedChanged);
            // 
            // button_Clear_Recheck
            // 
            this.button_Clear_Recheck.BackColor = System.Drawing.Color.DarkViolet;
            this.button_Clear_Recheck.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.button_Clear_Recheck.ForeColor = System.Drawing.Color.White;
            this.button_Clear_Recheck.Location = new System.Drawing.Point(260, 14);
            this.button_Clear_Recheck.Name = "button_Clear_Recheck";
            this.button_Clear_Recheck.Size = new System.Drawing.Size(95, 40);
            this.button_Clear_Recheck.TabIndex = 74;
            this.button_Clear_Recheck.Text = "清空人工覆判";
            this.button_Clear_Recheck.UseVisualStyleBackColor = false;
            this.button_Clear_Recheck.Click += new System.EventHandler(this.button_Clear_Recheck_Click);
            // 
            // groupBox_DefectsClassify
            // 
            this.groupBox_DefectsClassify.Controls.Add(this.btn_Edit_DefectsClassify);
            this.groupBox_DefectsClassify.Controls.Add(this.listView_Edit);
            this.groupBox_DefectsClassify.Enabled = false;
            this.groupBox_DefectsClassify.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox_DefectsClassify.ForeColor = System.Drawing.Color.YellowGreen;
            this.groupBox_DefectsClassify.Location = new System.Drawing.Point(871, 218);
            this.groupBox_DefectsClassify.Name = "groupBox_DefectsClassify";
            this.groupBox_DefectsClassify.Size = new System.Drawing.Size(358, 220);
            this.groupBox_DefectsClassify.TabIndex = 88;
            this.groupBox_DefectsClassify.TabStop = false;
            this.groupBox_DefectsClassify.Text = "瑕疵種類";
            // 
            // btn_Edit_DefectsClassify
            // 
            this.btn_Edit_DefectsClassify.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Edit_DefectsClassify.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Edit_DefectsClassify.ForeColor = System.Drawing.Color.Black;
            this.btn_Edit_DefectsClassify.Location = new System.Drawing.Point(82, 16);
            this.btn_Edit_DefectsClassify.Name = "btn_Edit_DefectsClassify";
            this.btn_Edit_DefectsClassify.Size = new System.Drawing.Size(78, 29);
            this.btn_Edit_DefectsClassify.TabIndex = 21;
            this.btn_Edit_DefectsClassify.Text = "編輯顏色";
            this.btn_Edit_DefectsClassify.UseVisualStyleBackColor = false;
            this.btn_Edit_DefectsClassify.Click += new System.EventHandler(this.btn_Edit_DefectsClassify_Click);
            // 
            // listView_Edit
            // 
            this.listView_Edit.BackColor = System.Drawing.Color.AliceBlue;
            this.listView_Edit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_Edit.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Key,
            this.columnHeader_Name,
            this.columnHeader_Color});
            this.listView_Edit.FullRowSelect = true;
            this.listView_Edit.GridLines = true;
            this.listView_Edit.Location = new System.Drawing.Point(5, 51);
            this.listView_Edit.MultiSelect = false;
            this.listView_Edit.Name = "listView_Edit";
            this.listView_Edit.Size = new System.Drawing.Size(348, 160);
            this.listView_Edit.TabIndex = 19;
            this.listView_Edit.UseCompatibleStateImageBehavior = false;
            this.listView_Edit.View = System.Windows.Forms.View.Details;
            this.listView_Edit.SelectedIndexChanged += new System.EventHandler(this.listView_Edit_SelectedIndexChanged);
            // 
            // columnHeader_Key
            // 
            this.columnHeader_Key.Text = "瑕疵名稱";
            this.columnHeader_Key.Width = 180;
            // 
            // columnHeader_Name
            // 
            this.columnHeader_Name.Text = "優先權";
            this.columnHeader_Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_Name.Width = 75;
            // 
            // columnHeader_Color
            // 
            this.columnHeader_Color.Text = "顏色";
            this.columnHeader_Color.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_Color.Width = 80;
            // 
            // groupBox_DefectsResult
            // 
            this.groupBox_DefectsResult.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_DefectsResult.Controls.Add(this.listView_Result);
            this.groupBox_DefectsResult.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox_DefectsResult.ForeColor = System.Drawing.Color.YellowGreen;
            this.groupBox_DefectsResult.Location = new System.Drawing.Point(871, 438);
            this.groupBox_DefectsResult.Name = "groupBox_DefectsResult";
            this.groupBox_DefectsResult.Size = new System.Drawing.Size(358, 190);
            this.groupBox_DefectsResult.TabIndex = 87;
            this.groupBox_DefectsResult.TabStop = false;
            this.groupBox_DefectsResult.Text = "統計結果";
            // 
            // listView_Result
            // 
            this.listView_Result.BackColor = System.Drawing.Color.AliceBlue;
            this.listView_Result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView_Result.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_Result.FullRowSelect = true;
            this.listView_Result.GridLines = true;
            this.listView_Result.Location = new System.Drawing.Point(5, 22);
            this.listView_Result.MultiSelect = false;
            this.listView_Result.Name = "listView_Result";
            this.listView_Result.Size = new System.Drawing.Size(348, 160);
            this.listView_Result.TabIndex = 20;
            this.listView_Result.UseCompatibleStateImageBehavior = false;
            this.listView_Result.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "瑕疵名稱";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "NG顆數";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 75;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "良率 (%)";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 80;
            // 
            // checkBox_orig
            // 
            this.checkBox_orig.AutoSize = true;
            this.checkBox_orig.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkBox_orig.Location = new System.Drawing.Point(545, 12);
            this.checkBox_orig.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_orig.Name = "checkBox_orig";
            this.checkBox_orig.Size = new System.Drawing.Size(92, 24);
            this.checkBox_orig.TabIndex = 75;
            this.checkBox_orig.Text = "顯示原圖";
            this.checkBox_orig.UseVisualStyleBackColor = true;
            this.checkBox_orig.CheckedChanged += new System.EventHandler(this.checkBox_orig_CheckedChanged);
            // 
            // cbx_LightImage_Recheck
            // 
            this.cbx_LightImage_Recheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.cbx_LightImage_Recheck.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbx_LightImage_Recheck.ForeColor = System.Drawing.Color.White;
            this.cbx_LightImage_Recheck.FormattingEnabled = true;
            this.cbx_LightImage_Recheck.Location = new System.Drawing.Point(655, 7);
            this.cbx_LightImage_Recheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbx_LightImage_Recheck.Name = "cbx_LightImage_Recheck";
            this.cbx_LightImage_Recheck.Size = new System.Drawing.Size(210, 32);
            this.cbx_LightImage_Recheck.TabIndex = 74;
            this.cbx_LightImage_Recheck.SelectedIndexChanged += new System.EventHandler(this.cbx_LightImage_Recheck_SelectedIndexChanged);
            // 
            // label_Color
            // 
            this.label_Color.AutoSize = true;
            this.label_Color.BackColor = System.Drawing.Color.Transparent;
            this.label_Color.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Color.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_Color.Location = new System.Drawing.Point(1041, 641);
            this.label_Color.Name = "label_Color";
            this.label_Color.Size = new System.Drawing.Size(51, 15);
            this.label_Color.TabIndex = 54;
            this.label_Color.Text = "顏色設定";
            this.label_Color.Visible = false;
            // 
            // button_SetColor_Recheck
            // 
            this.button_SetColor_Recheck.BackColor = System.Drawing.Color.Red;
            this.button_SetColor_Recheck.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_SetColor_Recheck.ForeColor = System.Drawing.Color.Transparent;
            this.button_SetColor_Recheck.Location = new System.Drawing.Point(1041, 658);
            this.button_SetColor_Recheck.Name = "button_SetColor_Recheck";
            this.button_SetColor_Recheck.Size = new System.Drawing.Size(49, 48);
            this.button_SetColor_Recheck.TabIndex = 53;
            this.button_SetColor_Recheck.UseVisualStyleBackColor = false;
            this.button_SetColor_Recheck.Visible = false;
            this.button_SetColor_Recheck.Click += new System.EventHandler(this.button_SetColor_Recheck_Click);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label50.ForeColor = System.Drawing.Color.Black;
            this.label50.Location = new System.Drawing.Point(879, 643);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(77, 20);
            this.label50.TabIndex = 50;
            this.label50.Text = "瑕疵類型:";
            this.label50.Visible = false;
            // 
            // comboBox_DefectType
            // 
            this.comboBox_DefectType.BackColor = System.Drawing.Color.White;
            this.comboBox_DefectType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_DefectType.ForeColor = System.Drawing.Color.Black;
            this.comboBox_DefectType.FormattingEnabled = true;
            this.comboBox_DefectType.Location = new System.Drawing.Point(883, 678);
            this.comboBox_DefectType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_DefectType.Name = "comboBox_DefectType";
            this.comboBox_DefectType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBox_DefectType.Size = new System.Drawing.Size(143, 28);
            this.comboBox_DefectType.TabIndex = 49;
            this.comboBox_DefectType.Visible = false;
            this.comboBox_DefectType.SelectedIndexChanged += new System.EventHandler(this.comboBox_DefectType_SelectedIndexChanged);
            // 
            // hSmartWindowControl_Recheck
            // 
            this.hSmartWindowControl_Recheck.AllowDrop = true;
            this.hSmartWindowControl_Recheck.AutoScroll = true;
            this.hSmartWindowControl_Recheck.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hSmartWindowControl_Recheck.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hSmartWindowControl_Recheck.BackColor = System.Drawing.SystemColors.HotTrack;
            this.hSmartWindowControl_Recheck.HDoubleClickToFitContent = true;
            this.hSmartWindowControl_Recheck.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hSmartWindowControl_Recheck.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hSmartWindowControl_Recheck.HKeepAspectRatio = false;
            this.hSmartWindowControl_Recheck.HMoveContent = true;
            this.hSmartWindowControl_Recheck.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelBackwardZoomsIn;
            this.hSmartWindowControl_Recheck.Location = new System.Drawing.Point(4, 45);
            this.hSmartWindowControl_Recheck.Margin = new System.Windows.Forms.Padding(0);
            this.hSmartWindowControl_Recheck.Name = "hSmartWindowControl_Recheck";
            this.hSmartWindowControl_Recheck.Size = new System.Drawing.Size(861, 747);
            this.hSmartWindowControl_Recheck.TabIndex = 2;
            this.hSmartWindowControl_Recheck.WindowSize = new System.Drawing.Size(861, 747);
            // 
            // btn_SaveImageList
            // 
            this.btn_SaveImageList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_SaveImageList.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.btn_SaveImageList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SaveImageList.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_SaveImageList.Location = new System.Drawing.Point(1246, 35);
            this.btn_SaveImageList.Margin = new System.Windows.Forms.Padding(2);
            this.btn_SaveImageList.Name = "btn_SaveImageList";
            this.btn_SaveImageList.Size = new System.Drawing.Size(115, 51);
            this.btn_SaveImageList.TabIndex = 73;
            this.btn_SaveImageList.Text = "儲存影像組";
            this.btn_SaveImageList.UseVisualStyleBackColor = false;
            this.btn_SaveImageList.Click += new System.EventHandler(this.btn_SaveImageList_Click);
            // 
            // button_ShowParaForm
            // 
            this.button_ShowParaForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_ShowParaForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(166)))), ((int)(((byte)(137)))));
            this.button_ShowParaForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ShowParaForm.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_ShowParaForm.Location = new System.Drawing.Point(1246, 96);
            this.button_ShowParaForm.Margin = new System.Windows.Forms.Padding(2);
            this.button_ShowParaForm.Name = "button_ShowParaForm";
            this.button_ShowParaForm.Size = new System.Drawing.Size(115, 51);
            this.button_ShowParaForm.TabIndex = 74;
            this.button_ShowParaForm.Text = "參數調整";
            this.button_ShowParaForm.UseVisualStyleBackColor = false;
            this.button_ShowParaForm.Click += new System.EventHandler(this.button_ShowParaForm_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip_DispID
            // 
            this.contextMenuStrip_DispID.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.contextMenuStrip_DispID.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_ID});
            this.contextMenuStrip_DispID.Name = "contextMenuStrip_DispID";
            this.contextMenuStrip_DispID.Size = new System.Drawing.Size(115, 28);
            // 
            // toolStripMenuItem_ID
            // 
            this.toolStripMenuItem_ID.ForeColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItem_ID.Name = "toolStripMenuItem_ID";
            this.toolStripMenuItem_ID.Size = new System.Drawing.Size(114, 24);
            this.toolStripMenuItem_ID.Text = "(1, 1)";
            // 
            // contextMenuStrip_Recheck
            // 
            this.contextMenuStrip_Recheck.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.contextMenuStrip_Recheck.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_OK});
            this.contextMenuStrip_Recheck.Name = "contextMenuStrip1";
            this.contextMenuStrip_Recheck.Size = new System.Drawing.Size(102, 28);
            // 
            // ToolStripMenuItem_OK
            // 
            this.ToolStripMenuItem_OK.CheckOnClick = true;
            this.ToolStripMenuItem_OK.ForeColor = System.Drawing.Color.Green;
            this.ToolStripMenuItem_OK.Name = "ToolStripMenuItem_OK";
            this.ToolStripMenuItem_OK.Size = new System.Drawing.Size(101, 24);
            this.ToolStripMenuItem_OK.Text = "OK";
            // 
            // ImageShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
            this.ClientSize = new System.Drawing.Size(1364, 863);
            this.Controls.Add(this.button_ShowParaForm);
            this.Controls.Add(this.btn_SaveImageList);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ImageShowForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ImageShowForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageShowForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.ImageShowForm_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UI2Dindex)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_General.ResumeLayout(false);
            this.tabPage_General.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageShowForm)).EndInit();
            this.tabPage_Adv.ResumeLayout(false);
            this.tabPage_Adv.PerformLayout();
            this.tabPage_Recheck.ResumeLayout(false);
            this.tabPage_Recheck.PerformLayout();
            this.groupBox_Recheck.ResumeLayout(false);
            this.groupBox_Recheck.PerformLayout();
            this.groupBox_DefectsClassify.ResumeLayout(false);
            this.groupBox_DefectsResult.ResumeLayout(false);
            this.contextMenuStrip_DispID.ResumeLayout(false);
            this.contextMenuStrip_Recheck.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbtn_defect;
        private System.Windows.Forms.RadioButton rbtn_origin;
        private System.Windows.Forms.ComboBox comboBox_LightImage;
        private System.Windows.Forms.DataGridView dataGridView_UI2Dindex;
        private HalconDotNet.HSmartWindowControl hWindowControl_InspectResultImage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_General;
        private System.Windows.Forms.TabPage tabPage_Adv;
        private System.Windows.Forms.Panel panel_InspectResult;
        private System.Windows.Forms.Button btn_SaveImageList;
        private System.Windows.Forms.TextBox txt_CursorCoordinate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_GrayValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_ShowParaForm;
        private System.Windows.Forms.TabPage tabPage_Recheck;
        private HalconDotNet.HSmartWindowControl hSmartWindowControl_Recheck;
        private System.Windows.Forms.ComboBox comboBox_DefectType;
        private System.Windows.Forms.Label label_Color;
        private System.Windows.Forms.Button button_SetColor_Recheck;
        private System.Windows.Forms.ComboBox cbx_LightImage_Recheck;
        private System.Windows.Forms.CheckBox checkBox_orig;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox_ImageShowForm;
        private System.Windows.Forms.Button button_SaveAs_DefectTest;
        private System.Windows.Forms.Button button_Save_DefectTest;
        private System.Windows.Forms.CheckBox cbx_Priority;
        private System.Windows.Forms.CheckBox cbx_NG_classification;
        private System.Windows.Forms.GroupBox groupBox_Recheck;
        private System.Windows.Forms.TextBox txt_cellID;
        private System.Windows.Forms.Label label_cellID;
        private System.Windows.Forms.RadioButton radioButton_MultiCells;
        private System.Windows.Forms.RadioButton radioButton_SingleCell;
        private System.Windows.Forms.CheckBox cbx_Recheck;
        private System.Windows.Forms.CheckBox cbx_Do_Recheck;
        private System.Windows.Forms.Button button_Clear_Recheck;
        private System.Windows.Forms.GroupBox groupBox_DefectsClassify;
        private System.Windows.Forms.Button btn_Edit_DefectsClassify;
        private System.Windows.Forms.ListView listView_Edit;
        private System.Windows.Forms.ColumnHeader columnHeader_Key;
        private System.Windows.Forms.ColumnHeader columnHeader_Name;
        private System.Windows.Forms.ColumnHeader columnHeader_Color;
        private System.Windows.Forms.GroupBox groupBox_DefectsResult;
        private System.Windows.Forms.ListView listView_Result;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.RadioButton radioButton_Recheck;
        private System.Windows.Forms.RadioButton radioButton_Result;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_DispID;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_ID;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Recheck;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_OK;
        private HalconDotNet.HSmartWindowControl hSmartWindowControl_ImageShowForm;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.TextBox textBox_Path_DefectTest;
    }
}